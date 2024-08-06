using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.Transfer
{
    public partial class TrfLst : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsStoreReq = new DataSet();
        private readonly Blue.BL.IN.StandardRequistion stdReq = new Blue.BL.IN.StandardRequistion();
        private readonly Blue.BL.IN.StandardRequisitionDetail stdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        private Blue.BL.IN.Transfer trf = new Blue.BL.IN.Transfer();
        private Blue.BL.IN.TransferDt trfDt = new Blue.BL.IN.TransferDt();

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page_Retrieve();
            }

            base.Page_Load(sender, e);

            ListPage2.CreateItems.Menu.ItemClick += Menu_ItemClick;
        }

        private void Page_Retrieve()
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
            //ListPage2.PageCode = Request.Params["pagecode"].ToString();
            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create", "CNEW"));
            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create from Standard Requisition",
                "SR"));
            ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Store Requisition List", "SL"));

            ListPage2.DataBind();
        }

        private void Menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CNEW":
                    Response.Redirect("TrfEdit.aspx?MODE=New&VID=" + Request.Cookies["[IN].[vTrfList]"].Value);
                    break;

                case "SR":
                    DisplayTemplate();
                    break;
                case "SL":

                    for (var i = 0; i < ListPage2.dtBuKeys.Rows.Count; i++)
                    {
                        ListPage2.dtBuKeys.Rows[i][1] =
                            ListPage2.dtBuKeys.Rows[i][1].ToString().Replace("'", "").Replace("*", "0");
                    }
                    Session["dtBuKeys"] = ListPage2.dtBuKeys;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=324";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }

        private void DisplayTemplate()
        {
            var Get = stdReq.Get(dsStoreReq, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (Get)
            {
                grd_Template.DataSource = dsStoreReq.Tables[stdReq.TableName];
                grd_Template.DataBind();

                //Session["dsStoreReq"] = dsStoreReq;
                pop_Template.ShowOnPageLoad = true;
            }
        }

        protected void btn_TemplateOk_Click(object sender, EventArgs e)
        {
            var columnValues = new List<object>();

            var grd_Grid = grd_Template;

            foreach (GridViewRow grd_Row in grd_Grid.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    columnValues.Add(grd_Grid.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            for (var i = 0; i < columnValues.Count; i++)
            {
                var Get = stdReq.Get(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);

                if (!Get)
                {
                    return;
                }

                var GetDt = stdReqDt.Get(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);

                if (!GetDt)
                {
                    return;
                }
            }

            Session["dsStoreReqDt"] = dsStoreReq;

            Response.Redirect("CreateFromStdReq.aspx?MODE=Create&VID=" + Request.Cookies["[IN].[vTrfList]"].Value);
        }
    }
}