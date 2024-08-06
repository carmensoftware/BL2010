using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.IN.MLT
{
    public partial class SOT : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();
        private string MsgError = string.Empty;
        private DataSet dsTemplateEdit = new DataSet();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();

        public string TemplateDetailMode
        {
            get
            {
                if (ViewState["TemplateDetailMode"] != null)
                {
                    return ViewState["TemplateDetailMode"].ToString();
                }

                return string.Empty;
            }
            set { ViewState["TemplateDetailMode"] = value; }
        }

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTemplateEdit = (DataSet) Session["dsTemplateEdit"];
            }
        }

        /// <summary>
        ///     Get business MLTEdit data related to login user.
        /// </summary>
        private void Page_Retrieve()
        {
            var tmpNo = int.Parse(Request.Params["ID"]);

            // Get Template Data
            var getTemplate = template.Get(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

            if (!getTemplate)
            {
                return;
            }

            // Get template Detail Data
            var getTemplateDt = templateDt.GetListByTmpNo(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

            if (!getTemplateDt)
            {
                return;
            }
            Session["dsTemplateEdit"] = dsTemplateEdit;

            Page_Setting();
        }

        /// <summary>
        ///     Display business MLTEdit data which retrieved from Page_Retrieve procedure.
        /// </summary>
        private void Page_Setting()
        {
            // Display template
            var drTemplate = dsTemplateEdit.Tables[template.TableName].Rows[0];

            lbl_TmpNo.Text = drTemplate["TmpNo"].ToString();
            chk_Active.Checked = bool.Parse(drTemplate["IsActived"].ToString());
            lbl_Location.Text = drTemplate["LocationCode"].ToString();
            lbl_Desc.Text = drTemplate["Desc"].ToString();
            lbl_StoreName.Text = storeLct.GetName(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----storeLct.GetName2(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);

            // Page Title
            lbl_Title.Text = (drTemplate["TmpTypeCode"].ToString().ToUpper() == "M" ? "Market List" : "Standard Order");
            // by brian
            //menu_CmdBar.Items[3].NavigateUrl = "../../RPT/Default.aspx?page=ml&id=" + Request.Params["ID"].ToString();
            //menu_CmdBar.Items[3].Target = "_blank";
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("SOTEdit.aspx?MODE=New&TYPE=" +
                                      dsTemplateEdit.Tables[template.TableName].Rows[0]["TmpTypeCode"]);
                    break;

                case "EDIT":
                    Response.Redirect("SOTEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"]);
                    break;

                case "DELETE":
                    pop_ConfrimDeleteTemplate.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    //var objArrList = new ArrayList();
                    //objArrList.Add(Request.Params["ID"]);
                    //objArrList.Add("'O'");
                    //Session["s_arrNo"] = objArrList;

                    //var paramFieldReport = new string[1, 3];
                    //paramFieldReport[0, 0] = "Type";
                    //paramFieldReport[0, 1] = "=";
                    //paramFieldReport[0, 2] = "O";
                    //Session["paramFieldReport"] = paramFieldReport;

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=153";
                    ////Response.Write("<script>");
                    ////Response.Write("window.open('" + reportLink + "','_blank'  )");
                    ////Response.Write("</script>");
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "BACK":
                    Response.Redirect("SOTLst.aspx");
                    break;

                case "COPY":
                    Response.Redirect("SOTEdit.aspx?MODE=Copy&ID=" + Request.Params["ID"]);
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ActionDt_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    //grd_StdOrdList.Selection.UnselectAll();
                    foreach (GridViewRow grd_Row in grd_StdOrdList.Rows)
                    {
                        var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                        if (chk_Item.Checked)
                        {
                            chk_Item.Checked = false;
                        }
                    }

                    //Response.Redirect("SOTEdit.aspx?MODE=NList&ID=" + Request.Params["ID"]);
                    Response.Redirect("SOTEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"]);
                    break;

                case "DELETE":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;
            }
        }


        /// <summary>
        ///     Delete Data on Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_StdOrdList.GetSelectedFieldValues("TmpDtNo");
            var columnValues = new List<object>();
            var grd_Grid = grd_StdOrdList;

            foreach (GridViewRow grd_Row in grd_Grid.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    columnValues.Add(grd_Grid.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            foreach (int delTmpDtCode in columnValues)
            {
                foreach (DataRow drDeleting in dsTemplateEdit.Tables[templateDt.TableName].Rows)
                {
                    if (drDeleting.RowState != DataRowState.Deleted)
                    {
                        if (drDeleting["TmpDtNo"].ToString().ToUpper() == delTmpDtCode.ToString().ToUpper())
                        {
                            drDeleting.Delete();
                        }
                    }
                }
            }

            // Save to database
            var save = templateDt.Save(dsTemplateEdit, LoginInfo.ConnStr);

            if (save)
            {
                //grd_StdOrdList.Selection.UnselectAll();
                foreach (GridViewRow grd_Row in grd_StdOrdList.Rows)
                {
                    var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                    if (chk_Item.Checked)
                    {
                        chk_Item.Checked = false;
                    }
                }

                grd_StdOrdList.DataBind();
                pop_ConfrimDelete.ShowOnPageLoad = false;
            }
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_StdOrdList.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_StdOrdList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_ConfrimDeleteTemplate_Click(object sender, EventArgs e)
        {
            var TmpNo = Request.Params["ID"];

            foreach (DataRow drDeletingTmpDt in dsTemplateEdit.Tables[templateDt.TableName].Rows)
            {
                drDeletingTmpDt.Delete();
            }

            var TmpDtSave = templateDt.Save(dsTemplateEdit, LoginInfo.ConnStr);
            {
                foreach (DataRow drDeletingTmp in dsTemplateEdit.Tables[template.TableName].Rows)
                {
                    drDeletingTmp.Delete();
                }
                var save = template.Save(dsTemplateEdit, LoginInfo.ConnStr);

                if (save)
                {
                    pop_ConfrimDeleteTemplate.ShowOnPageLoad = false;

                    Response.Redirect("SOTLst.aspx");
                }
            }
        }

        protected void btn_CancelDeleteTemplate_Click(object sender, EventArgs e)
        {
            //grd_StdOrdList.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_StdOrdList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDeleteTemplate.ShowOnPageLoad = false;
        }

        protected void lbl_Local_Desc_Init(object sender, EventArgs e)
        {
            var lbl_Local_Desc = (ASPxLabel) sender;
            var container = lbl_Local_Desc.NamingContainer as GridViewEditItemTemplateContainer;


            if (!container.Grid.IsNewRowEditing)
            {
                lbl_Local_Desc.Text = DataBinder.Eval(container.DataItem, "ProductDesc2").ToString();
            }
            else
            {
                lbl_Local_Desc.Text = string.Empty;
            }
        }

        #endregion
    }
}