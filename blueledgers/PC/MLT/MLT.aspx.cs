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
    public partial class MLT : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private readonly Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();
        private string MsgError = string.Empty;
        private DataSet dsTemplateEdit = new DataSet();
        private Blue.BL.Option.Inventory.ProdLoc prodLoc = new Blue.BL.Option.Inventory.ProdLoc();
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
            //lbl_Location.Text   = drTemplate["LocationCode"].ToString();
            lbl_Desc.Text = drTemplate["Desc"].ToString();
            lbl_StoreName.Text = drTemplate["LocationCode"] + " : " +
                                 storeLct.GetName(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
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
                    Response.Redirect("MLTEdit.aspx?MODE=New&Type=" +
                                      dsTemplateEdit.Tables[template.TableName].Rows[0]["TmpTypeCode"]);
                    break;

                case "EDIT":
                    Response.Redirect("MLTEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"]);
                    break;

                case "DELETE":
                    pop_ConfrimDeleteTemplate.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    //var objArrList = new ArrayList();
                    //objArrList.Add(Request.Params["ID"]);
                    //objArrList.Add("'M'");
                    //Session["s_arrNo"] = objArrList;

                    //var paramFieldReport = new string[1, 3];
                    //paramFieldReport[0, 0] = "Type";
                    //paramFieldReport[0, 1] = "=";
                    //paramFieldReport[0, 2] = "M";
                    //Session["paramFieldReport"] = paramFieldReport;

                    //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=152";
                    ////Response.Write("<script>");
                    ////Response.Write("window.open('" + reportLink + "','_blank'  )");
                    ////Response.Write("</script>");
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "BACK":
                    Response.Redirect("MLTLst.aspx");
                    break;

                case "COPY":
                    Response.Redirect("MLTEdit.aspx?MODE=Copy&ID=" + Request.Params["ID"]);
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
                    Create();
                    break;

                case "DELETE":
                    Delete();
                    break;
            }
        }

        /// <summary>
        ///     Create New Market List.
        /// </summary>
        private void Create()
        {
            //grd_MarketList.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_MarketList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            //grd_MarketList.AddNewRow();
            //Response.Redirect("MLTEdit.aspx?MODE=NList&ID=" + Request.Params["ID"]);
            Response.Redirect("MLTEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"]);
        }

        /// <summary>
        ///     Display Condfirm Delete and Unit.
        /// </summary>
        private void Delete()
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
            //grd_MarketList.CancelEdit();

            //if (grd_MarketList.Selection.Count > 0)
            //{
            //    pop_ConfrimDelete.ShowOnPageLoad = true;
            //    return;
            //}
        }

        //protected void grd_MarketList_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "CompositeKey")
        //    {
        //        string TmpNo = Convert.ToString(e.GetListSourceFieldValue("TmpNo"));
        //        string TmpDtNo = Convert.ToString(e.GetListSourceFieldValue("TmpDtNo"));
        //        e.Value = TmpNo + "," + TmpDtNo;
        //    }
        //}
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Display Template Detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_TemplateDt_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.FindControl("lbl_Product") != null)
        //        {
        //            Label lbl_ProductCode = (Label)e.Row.FindControl("lbl_ProductCode");                   
        //            lbl_ProductCode.Text  = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
        //        }
        //        if (e.Row.FindControl("lbl_ProductDesc1") != null)
        //        {
        //            Label lbl_ProductDesc1 = (Label)e.Row.FindControl("lbl_ProductDesc1");
        //            lbl_ProductDesc1.Text  = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
        //        }
        //        if (e.Row.FindControl("lbl_Unit") != null)
        //        {
        //            Label lbl_Unit = (Label)e.Row.FindControl("lbl_Unit");
        //            lbl_Unit.Text  = unit.GetName(DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString(), LoginInfo.ConnStr);
        //        }
        //    }
        //}
        protected void cmb_Product_Load(object sender, EventArgs e)
        {
            //ASPxComboBox cmb_Product = (ASPxComboBox)grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns[2] as GridViewDataColumn, "cmb_Product");
            //cmb_Product.DataSource = product.GetLookUp_LocationCode(cmb_Product.Value.ToString(), LoginInfo.ConnStr);
            //cmb_Product.DataBind();
            //if(cmb_Product.Items.Count > 0)
            //{
            //    cmb_Product.SelectedIndex = 0;
            //}
        }

        //protected void cmb_Product_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["ProductCode"] as GridViewDataColumn, "cmb_Product") != null)
        //    {
        //        ASPxComboBox cmb_Product = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["ProductCode"] as GridViewDataColumn, "cmb_Product") as ASPxComboBox;
        //        ASPxLabel lbl_Desc = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["Description"] as GridViewDataColumn, "lbl_Desc") as ASPxLabel;
        //        lbl_Desc.Text = product.GetName(cmb_Product.Value.ToString(), LoginInfo.ConnStr);
        //        ASPxLabel lbl_Unit = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["Unit"] as GridViewDataColumn, "lbl_Unit") as ASPxLabel;
        //        lbl_Unit.Text = product.GetOrderUnit(cmb_Product.Value.ToString(), LoginInfo.ConnStr);
        //        //ASPxComboBox cmb_Unit = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["Unit"] as GridViewDataColumn, "cmb_Unit") as ASPxComboBox;
        //        //cmb_Unit.Value = product.GetOrderUnit(cmb_Product.Value.ToString(), LoginInfo.ConnStr);

        //        //DropDownList ddl_unit = (DropDownList)grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns[4] as GridViewDataColumn, "ddl_unit");
        //        //string orderUnit = unit.GetName(product.GetOrderUnit(cmb_Product.SelectedItem.Value.ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr);

        //        //ddl_unit.DataSource = dsPo.Tables[unit.TableName];
        //        //ddl_unit.DataTextField = "Name";
        //        //ddl_unit.DataValueField = "Name";
        //        //ddl_unit.DataBind();
        //        //ddl_unit.SelectedValue = orderUnit.ToString();
        //    }
        //}

        #endregion

        //protected void grd_MarketList_Rowupdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{

        //    DataRow drUpdating = dsTemplateEdit.Tables[templateDt.TableName].Rows[grd_MarketList.EditingRowVisibleIndex];

        //    ASPxComboBox cmb_Product = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["ProductCode"] as GridViewDataColumn, "cmb_Product") as ASPxComboBox;
        //    drUpdating["ProductCode"] = cmb_Product.Value.ToString();
        //    ASPxLabel lbl_Unit = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["Unit"] as GridViewDataColumn, "lbl_Unit") as ASPxLabel;
        //    drUpdating["UnitCode"] = product.GetOrderUnit(cmb_Product.Value.ToString(),LoginInfo.ConnStr);
        //    //ASPxComboBox cmb_Unit = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["UnitCode"] as GridViewDataColumn, "cmb_Unit") as ASPxComboBox;
        //    //drUpdating["UnitCode"] = cmb_Unit.Value.ToString();
        //    //drUpdating["UnitCode"] = e.NewValues["UnitCode"].ToString();

        //    //Save To Database.
        //    bool SaveTemplateDt = templateDt.Save(dsTemplateEdit, LoginInfo.ConnStr);

        //    if (SaveTemplateDt)
        //    {
        //        grd_MarketList.CancelEdit();
        //        grd_MarketList.DataBind();

        //        e.Cancel = true;

        //        Session["dsTemplateEdit"] = dsTemplateEdit;
        //    }
        //}

        //protected void grd_MarketList_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DataRow drInserting = dsTemplateEdit.Tables[templateDt.TableName].NewRow();

        //    drInserting["TmpNo"] = int.Parse(Request.Params["ID"]);
        //    drInserting["TmpDtNo"] = templateDt.GetTmpDtNewNo(LoginInfo.ConnStr);
        //    ASPxComboBox cmb_Product = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["ProductCode"] as GridViewDataColumn, "cmb_Product") as ASPxComboBox;
        //    drInserting["ProductCode"] = cmb_Product.Value.ToString();
        //    ASPxLabel lbl_Unit = grd_MarketList.FindEditRowCellTemplateControl(grd_MarketList.Columns["UnitCode"] as GridViewDataColumn, "lbl_Unit") as ASPxLabel;
        //    drInserting["UnitCode"] = product.GetOrderUnit(cmb_Product.Value.ToString(),LoginInfo.ConnStr);
        //    //drInserting["UnitCode"] = e.NewValues["UnitCode"].ToString();

        //    dsTemplateEdit.Tables[templateDt.TableName].Rows.Add(drInserting);

        //    //save to database.
        //    bool SaveTemplateDt = templateDt.Save(dsTemplateEdit, LoginInfo.ConnStr);

        //    if (SaveTemplateDt)
        //    {
        //        grd_MarketList.CancelEdit();
        //        grd_MarketList.DataBind();

        //        e.Cancel = true;

        //        Session["dsTemplateEdit"] = dsTemplateEdit;
        //    }
        //}

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_MarketList.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_MarketList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Delete From Table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_MarketList.GetSelectedFieldValues("TmpDtNo");
            var columnValues = new List<object>();
            var grd_Gird = grd_MarketList;

            foreach (GridViewRow grd_Row in grd_Gird.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    columnValues.Add(grd_Gird.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            foreach (int delTemplateDt in columnValues)
            {
                foreach (DataRow drDeleting in dsTemplateEdit.Tables[templateDt.TableName].Rows)
                {
                    if (drDeleting.RowState != DataRowState.Deleted)
                    {
                        if (drDeleting["TmpDtNo"].ToString().ToUpper() == delTemplateDt.ToString().ToUpper())
                        {
                            drDeleting.Delete();
                        }
                    }
                }
            }

            //Save to Database.
            var SaveTemplateDt = templateDt.Save(dsTemplateEdit, LoginInfo.ConnStr);

            if (SaveTemplateDt)
            {
                //grd_MarketList.Selection.UnselectAll();
                foreach (GridViewRow grd_Row in grd_MarketList.Rows)
                {
                    var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                    if (chk_Item.Checked)
                    {
                        chk_Item.Checked = false;
                    }
                }

                grd_MarketList.DataBind();
                pop_ConfrimDelete.ShowOnPageLoad = false;
                //grd_MarketList.Selection.UnselectAll();
                //this.Page_Retrieve();
                //Session["dsTemplateEdit"] = dsTemplateEdit;
            }
        }

        /// <summary>
        ///     Cancel Delete From Template Table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDeleteTemplate_Click(object sender, EventArgs e)
        {
            //grd_MarketList.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_MarketList.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDeleteTemplate.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Delete From Template Table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteTemplate_Click(object sender, EventArgs e)
        {
            var Template = Request.Params["ID"];

            foreach (DataRow drTemplateDt in dsTemplateEdit.Tables[templateDt.TableName].Rows)
            {
                drTemplateDt.Delete();
            }

            foreach (DataRow drTemplate in dsTemplateEdit.Tables[template.TableName].Rows)
            {
                drTemplate.Delete();
            }

            //Save To DataBase.
            var saveTemplate = template.Delete(dsTemplateEdit, LoginInfo.ConnStr);

            if (saveTemplate)
            {
                pop_ConfrimDeleteTemplate.ShowOnPageLoad = false;
                Response.Redirect("MLTLst.aspx?ID=" + Template);
            }
        }

        /// <summary>
        ///     Show Edit ProductCode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Product_Init(object sender, EventArgs e)
        {
            var cmb_Product = (ASPxComboBox) sender;
            var container = cmb_Product.NamingContainer as GridViewEditItemTemplateContainer;

            if (!container.Grid.IsNewRowEditing)
            {
                cmb_Product.Value = DataBinder.Eval(container.DataItem, "ProductCode").ToString();
            }
            else
            {
                cmb_Product.Value = string.Empty;
            }
            cmb_Product.DataBind();
        }

        /// <summary>
        ///     Show Edit ProductDesc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbl_Desc_Init(object sender, EventArgs e)
        {
            var lbl_Desc = (ASPxLabel) sender;
            var container = lbl_Desc.NamingContainer as GridViewEditItemTemplateContainer;


            if (!container.Grid.IsNewRowEditing)
            {
                lbl_Desc.Text = DataBinder.Eval(container.DataItem, "ProductDesc1").ToString();
            }
            else
            {
                lbl_Desc.Text = string.Empty;
            }
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

        protected void lbl_Unit_Init(object sender, EventArgs e)
        {
            var lbl_Unit = (ASPxLabel) sender;
            var container = lbl_Unit.NamingContainer as GridViewEditItemTemplateContainer;

            if (!container.Grid.IsNewRowEditing)
            {
                lbl_Unit.Value = DataBinder.Eval(container.DataItem, "UnitCode").ToString();
            }
            else
            {
                lbl_Unit.Value = string.Empty;
            }
            lbl_Unit.DataBind();
        }
    }
}