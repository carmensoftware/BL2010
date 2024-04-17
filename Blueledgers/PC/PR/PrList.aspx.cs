using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PR
{
    public partial class PrList : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();
        private Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.APP.WF workflow = new Blue.BL.APP.WF();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.1";
        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
            hf_LoginName.Value = LoginInfo.LoginName;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //var vid = Request.Cookies["[PC].[vPrList]"] == null ? "" : Request.Cookies["[PC].[vPrList]"].Value;
                var vid = Request.QueryString["vid"] == null ? "" : Request.QueryString["vid"].ToString();

                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Manually", "MC"));
                ListPage.CreateItems.Items.FindByName("MC").NavigateUrl = "~/PC/PR/PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new" + "&VID=" + vid + "&Type=C";

                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From Market List", "ML"));
                ListPage.CreateItems.Items.FindByName("ML").NavigateUrl = "~/PC/PR/PrList.aspx?MODE=ML";

                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("From Standard Order", "SO"));
                ListPage.CreateItems.Items.FindByName("SO").NavigateUrl = "~/PC/PR/PrList.aspx?MODE=SO";

                ListPage.DataBind();

            }

            //ListPage.CreateItems.Items.FindByName("MC").NavigateUrl = "~/PC/PR/PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new" + "&VID=" + Request.Cookies["[PC].[vPrList]"].Value + "&Type=C";


            // Assign menu action to Session["MODE"] and reset menu button selecting

            if (Request.Params["MODE"] != null)
            {
                Session["MODE"] = Request.Params["MODE"].ToString();
                Response.Redirect("PrList.aspx");
            }

            // Action menu by Session
            if (Session["MODE"] != null)
            {
                string mode = Session["MODE"].ToString();
                switch (mode)
                {
                    case "ML":
                        DisplayMarketList();
                        break;
                    case "SO":
                        DisplayStandardOrderList();
                        break;
                    //case "PRINT":
                    //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    //    break;
                }
                Session.Remove("MODE");
            }


            Control_HeaderMenuBar();
            base.Page_Load(sender, e);

        }

        #region "Methods"

        // Added on: 06/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage.CreateItems.Visible = (pagePermiss >= 3) ? ListPage.CreateItems.Visible : false;
        }
        // End Added.

        /// <summary>
        ///     Display Market List/Standard Order
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        //private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        //{

        //    switch (e.Item.Name.ToUpper())
        //    {
        //        case "MC":
        //            Response.Redirect("PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&MODE=new" + "&VID=" + Request.Cookies["[PC].[vPrList]"].Value + "&Type=C");
        //            break;

        //        case "SL":
        //            Response.Redirect("~/Option/StockLevel/StockLevLst.aspx");
        //            break;

        //        case "ML":
        //            DisplayMarketList();
        //            break;

        //        case "SO":
        //            DisplayStandardOrderList();
        //            break;

        //        case "PDT":
        //            Session.Remove("paramField");


        //            Session["dtBuKeys"] = ListPage.dtBuKeys;
        //            var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=111";
        //            ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //                "<script>window.open('" + reportLink + "','_blank')</script>");
        //            break;

        //        case "PSM":
        //            Session.Remove("paramField");


        //            Session["dtBuKeys"] = ListPage.dtBuKeys;
        //            var reportLink2 = "../../RPT/ReportCriteria.aspx?category=001&reportid=110";
        //            ClientScript.RegisterStartupScript(GetType(), "newWindow",
        //                "<script>window.open('" + reportLink2 + "','_blank')</script>");
        //            break;
        //    }
        //}

        /// <summary>
        ///     Display Market List
        /// </summary>
        private void DisplayMarketList()
        {
            // Get maket list data        
            ods_Template.SelectParameters[0].DefaultValue = "M";
            //grd_Template.DataBind();

            // Display maket list data
            pop_Template.HeaderText = "Market List";
            pop_Template.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Display Standard Order List
        /// </summary>
        private void DisplayStandardOrderList()
        {
            // Get standard order data  
            ods_Template.SelectParameters[0].DefaultValue = "O";
            //grd_Template.DataBind();

            // Display maket list data
            pop_Template.HeaderText = "Standard Order";
            pop_Template.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Generate PR from selected Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OK_Click(object sender, EventArgs e)
        {
            var dsTemplateDt = new DataSet();
            var CountItem = 0;

            // ***************************** Gridview Template ************************************ //

            for (var i = 0; i <= grd_Template1.Rows.Count - 1; i++)
            {
                var Chk_Item = (CheckBox)grd_Template1.Rows[i].FindControl("Chk_Item");
                var hf_TmpNo = (HiddenField)grd_Template1.Rows[i].FindControl("hf_TmpNo");

                if (Chk_Item.Checked)
                {
                    var getTemplate = template.Get(dsTemplateDt, int.Parse(hf_TmpNo.Value), LoginInfo.ConnStr);

                    if (!getTemplate)
                    {
                        return;
                    }

                    // Get Template Detail Data                
                    var result = templateDt.GetReqQtyList(dsTemplateDt, int.Parse(hf_TmpNo.Value), LoginInfo.ConnStr);

                    if (!result)
                    {
                        return;
                    }

                    CountItem++;
                }
            }

            if (CountItem < 1)
            {
                pop_ChkItem.ShowOnPageLoad = true;
                return;
            }

            Session["dsTemplateDt"] = dsTemplateDt;

            //Response.Redirect("MLtoPr.aspx?VID=" + Request.Cookies["[PC].[vPrList]"].Value.ToString());

            if (ods_Template.SelectParameters[0].DefaultValue == "M")
            {
                Response.Redirect("MLtoPr.aspx?VID=" + Request.Cookies["[PC].[vPrList]"].Value + "&Type=M");
            }
            else
            {
                Response.Redirect("MLtoPr.aspx?VID=" + Request.Cookies["[PC].[vPrList]"].Value + "&Type=O");
            }

        }

        #endregion

        protected void grd_Template1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hf_TmpNo = (HiddenField)e.Row.FindControl("hf_TmpNo");

                if (e.Row.FindControl("hf_TmpNo") != null)
                {
                    hf_TmpNo.Value = DataBinder.Eval(e.Row.DataItem, "TmpNo").ToString();
                }
            }
        }



        protected void btnp_ChkItem_Click(object sender, EventArgs e)
        {
            pop_ChkItem.ShowOnPageLoad = false;
        }
    }
}