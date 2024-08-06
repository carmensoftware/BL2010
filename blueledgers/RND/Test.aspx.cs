using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxPopupControl;

namespace Test
{
    public partial class RndTest : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsViewHandler = new DataSet();

        private readonly Blue.BL.APP.Field field = new Blue.BL.APP.Field();
        private readonly Blue.BL.APP.Lookup lookup = new Blue.BL.APP.Lookup();
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.ViewHandlerCols viewHandlerCols = new Blue.BL.APP.ViewHandlerCols();
        private readonly Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr = new Blue.BL.APP.ViewHandlerCrtr();
        private string _listPageURL;
        private string _pageCode;
        private string _title;


        private ASPxComboBox cmbFieldName;
        private Blue.BL.APP.LookupItem lookupItem = new Blue.BL.APP.LookupItem();
        private ASPxTextBox txtDescription;
        private Dictionary<string, object> values = new Dictionary<string, object>();

        /// <summary>
        ///     Dataset for keep the ViewHandler data.
        /// </summary>

        #endregion

        #region "Operations"

        ///// <summary>
        ///// Page Init event workable sorting and paging.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    Page_Retrieve();


            //}
            //else
            //{
            //    dsViewHandler = (DataSet)Session["dsViewHandler"];

            //}

            base.Page_Load(sender, e);
        }


        private void Page_Retrieve()
        {
            // Edit View ----------------------------------------------------------------------
            // Get ViewHandler data
            var getViewHandler = viewHandler.Get(dsViewHandler, int.Parse("2"), LoginInfo.ConnStr);

            if (!getViewHandler)
            {
                // Display Error Message
                return;
            }

            // Get ViewHandlerCols data
            var getViewHandlerCols = viewHandlerCols.GetList(dsViewHandler, int.Parse("2"), LoginInfo.ConnStr);

            if (!getViewHandlerCols)
            {
                // Display Error Message
                return;
            }

            // Get ViewHandlerCrtr data
            var getViewHandlerCrtr = viewHandlerCrtr.GetList(dsViewHandler, int.Parse("2"), LoginInfo.ConnStr);

            if (!getViewHandlerCrtr)
            {
                // Display Error Message
                return;
            }


            Session["dsViewHandler"] = dsViewHandler;

            Page_Setting();
        }

        protected void txtDescription_Load(object sender, EventArgs e)
        {
            txtDescription = (ASPxTextBox) sender;
            var container = txtDescription.NamingContainer as GridViewEditFormTemplateContainer;

            // You can remove the if statement, and try to insert a new record. You'll catch an exception, because the DataBinder returns null reference
            if (!container.Grid.IsNewRowEditing)
                txtDescription.Text = DataBinder.Eval(container.DataItem, "SeqNo").ToString();
        }


        protected void cmbFieldName_Init(object sender, EventArgs e)
        {
            cmbFieldName = (ASPxComboBox) sender;
            var container = cmbFieldName.NamingContainer as GridViewEditFormTemplateContainer;

            // You can remove the if statement, and try to insert a new record. You'll catch an exception, because the DataBinder returns null reference
            if (!container.Grid.IsNewRowEditing)
                cmbFieldName.Value = DataBinder.Eval(container.DataItem, "FieldName").ToString();


            var dtField = field.GetList("PC.vPrList", LoginInfo.ConnStr);


            cmbFieldName.DataSource = dtField;
            cmbFieldName.DataBind();
        }


        private void Page_Setting()
        {
            Session["dsViewHandler"] = dsViewHandler;

            // Display title ------------------------------------------------------------------
            lbl_Title.Text = Title;

            // Display search in option related to module
            rbl_SearchIn.Items[0].Text = rbl_SearchIn.Items[0].Text + " " + Title;
            rbl_SearchIn.Items[1].Text = rbl_SearchIn.Items[1].Text + " " + Title;

            // Display ViewHandler data
            var drViewHandler = dsViewHandler.Tables[viewHandler.TableName].Rows[0];
            txt_Desc.Text = drViewHandler["Desc"].ToString();
            chk_IsPublic.Checked = bool.Parse(drViewHandler["IsPublic"].ToString());
            rbl_SearchIn.SelectedValue = drViewHandler["SearchIn"].ToString();

            // Display/Hide Advance Option
            chk_IsAdvance.Checked = bool.Parse(drViewHandler["IsAdvance"].ToString());

            if (chk_IsAdvance.Checked)
            {
                //DisplayAdvanceOption();

                txt_AdvOpt.Text = drViewHandler["AdvOpt"].ToString();
            }

            // Display ViewHandlerCols data
            lst_AvaCols.DataSource = field.GetListViewAvaCols(drViewHandler["PageCode"].ToString(),
                int.Parse(drViewHandler["ViewNo"].ToString()), LoginInfo.ConnStr);
            lst_AvaCols.TextField = "Desc";
            lst_AvaCols.ValueField = "FieldName";
            lst_AvaCols.DataBind();

            lst_SelCols.DataSource = field.GetListViewSelCols(drViewHandler["PageCode"].ToString(),
                int.Parse(drViewHandler["ViewNo"].ToString()), LoginInfo.ConnStr);
            lst_SelCols.TextField = "Desc";
            lst_SelCols.ValueField = "FieldName";
            lst_SelCols.DataBind();

            // Display ViewHandlerCrtr data            
            grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            grd_Criterias.DataBind();

            //grd_Criteria.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criteria.DataBind();
        }

        protected void grd_Criterias_CustomUnboundColumnData(object sender,
            DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var ViewNo = e.GetListSourceFieldValue("ViewNo").ToString();
                var ViewCrtrNo = e.GetListSourceFieldValue("ViewCrtrNo").ToString();
                e.Value = ViewNo + "," + ViewCrtrNo;
            }
        }

        /// <summary>
        ///     Command bar click.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Session["dsViewHandler"] = dsViewHandler;

            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    // Validation
                    if (txt_Desc.Text.Trim() == string.Empty)
                    {
                        // Display error message.
                        var pcWindow = new PopupWindow("Please enter View Name");
                        pcWindow.ShowOnPageLoad = true;
                        pcWindow.Modal = true;
                        ASPxPopupControl.Windows.Add(pcWindow);
                        return;
                    }

                    if (lst_SelCols.Items.Count == 0)
                    {
                        // Display error message.
                        var pcWindow = new PopupWindow("Please select display column");
                        pcWindow.ShowOnPageLoad = true;
                        pcWindow.Modal = true;
                        ASPxPopupControl.Windows.Add(pcWindow);
                        return;
                    }

                    // ViewHandler
                    var drViewHandler = dsViewHandler.Tables[viewHandler.TableName].Rows[0];

                    if (Request.Params["VIEW_NO"] == null)
                    {
                        drViewHandler["ViewNo"] = viewHandler.GetNewID(LoginInfo.ConnStr);
                        drViewHandler["CreatedDate"] = ServerDateTime;
                        drViewHandler["CreatedBy"] = LoginInfo.LoginName;
                    }

                    drViewHandler["PageCode"] = "PageCode";
                    drViewHandler["Desc"] = txt_Desc.Text.Trim();
                    drViewHandler["IsPublic"] = chk_IsPublic.Checked;
                    drViewHandler["SearchIn"] = rbl_SearchIn.SelectedItem.Value;
                    drViewHandler["IsAdvance"] = chk_IsAdvance.Checked;
                    drViewHandler["AdvOpt"] = (chk_IsAdvance.Checked ? txt_AdvOpt.Text.Trim() : string.Empty);
                    drViewHandler["UpdatedDate"] = ServerDateTime;
                    drViewHandler["UpdatedBy"] = LoginInfo.LoginName;

                    // ViewHandler Column
                    foreach (DataRow drViewHanlderCols in dsViewHandler.Tables[viewHandlerCols.TableName].Rows)
                    {
                        // Delete all exist display column
                        drViewHanlderCols.Delete();
                    }

                    for (var i = 0; i < lst_SelCols.Items.Count; i++)
                    {
                        // Insert selected display column
                        var drViewHandlerCols = dsViewHandler.Tables[viewHandlerCols.TableName].NewRow();
                        drViewHandlerCols["ViewNo"] = drViewHandler["ViewNo"];
                        drViewHandlerCols["ViewColNo"] = i + 1;
                        drViewHandlerCols["SeqNo"] = i + 1;
                        drViewHandlerCols["FieldName"] = lst_SelCols.Items[i].Value.ToString();
                        dsViewHandler.Tables[viewHandlerCols.TableName].Rows.Add(drViewHandlerCols);
                    }

                    // ViewHandler Criteria
                    if (Request.Params["VIEW_NO"] == null)
                    {
                        foreach (DataRow drViewHandlerCrtr in dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows)
                        {
                            if (drViewHandlerCrtr.RowState != DataRowState.Deleted)
                            {
                                drViewHandlerCrtr["ViewNo"] = drViewHandler["ViewNo"].ToString();
                            }
                        }
                    }

                    // Save
                    var saved = viewHandler.Save(dsViewHandler, LoginInfo.ConnStr);

                    if (saved)
                    {
                        // Update selected view cookies
                        //Response.Cookies[PageCode].Value = drViewHandler["ViewNo"].ToString();
                        //Response.Cookies[PageCode].Expires = DateTime.MaxValue;
                        //Response.Redirect(ListPageURL);
                    }
                    else
                    {
                        // Display Error Message
                    }

                    break;
                case "DELETE":
                    // Display confrim message.
                    pop_Confirm.ShowOnPageLoad = true;
                    break;

                case "BACK":
                    //Response.Redirect(ListPageURL);
                    break;
            }
        }


        /// <summary>
        ///     Binding for criteria gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            // if (e.RowType == GridViewRowType.EditForm) {
            //     values.Clear();
            //    ASPxGridView gridView = (ASPxGridView)sender;
            //    ASPxComboBox cmb_FieldName = (ASPxComboBox)gridView.FindEditFormTemplateControl("cmb_FieldName");


            //    DataTable dtField = field.GetList("PC.vPrList", LoginInfo.ConnStr);

            //     cmb_FieldName.Value = (e.GetValue("FieldName") == null ? string.Empty : e.GetValue("FieldName").ToString());
            //    cmb_FieldName.DataSource = dtField;
            //    cmb_FieldName.DataBind();

            //    values.Add("FieldName", (cmb_FieldName.SelectedItem.Value == null ? string.Empty :cmb_FieldName.SelectedItem.Value.ToString() ));


            //    //ASPxTextBox txtBox = (ASPxTextBox)gridView.FindEditFormTemplateControl("txtDescription");

            //    //txtBox.Text = e.GetValue("SeqNo").ToString();
            //    //values.Add("Operator", txtBox.Text);


            //}        
        }


        /// <summary>
        ///     Update View Criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //dsViewHandler = this.DataSouce;

            //// Find updating row.
            //string[] pk = e.Keys[0].ToString().Split(',');
            //DataRow drUpdating = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Find(pk);

            ////Update
            ////Get FieldName
            //if (grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn, "cmb_FieldName") != null)
            //{

            //    ASPxComboBox cmb_FieldName = (ASPxComboBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn, "cmb_FieldName");
            //    drUpdating["FieldName"] = cmb_FieldName.SelectedItem.Value.ToString();
            //}

            //e.NewValues["Operator"] = values["Operator"];
            e.NewValues["FieldName"] = cmbFieldName.SelectedItem.Value.ToString(); ///values["FieldName"];


            e.NewValues["Operator"] = txtDescription.Text;

            // Operator.
            //drUpdating["Operator"] = values["Operator"];

            //ASPxTextBox txt_String = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_String");
            //ASPxTextBox txt_Numeric = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Numeric");
            //ASPxDateEdit txt_Date = (ASPxDateEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Date");
            //ASPxCheckBox chk_Boolean = (ASPxCheckBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "chk_Boolean");
            //ASPxDropDownEdit ddl_Lookup = (ASPxDropDownEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "ddl_Lookup");

            //if (txt_String.Text.ToString() != string.Empty)
            //{
            //    drUpdating["Value"] = txt_String.Text.ToString();
            //}
            //else if (txt_Numeric.Text.ToString() != string.Empty)
            //{
            //    drUpdating["Value"] = txt_Numeric.Text.ToString();
            //}
            //else if (txt_Date.Value != null)
            //{
            //    drUpdating["Value"] = txt_Date.Value.ToString();
            //}
            //else if (chk_Boolean.Checked == true)
            //{
            //    drUpdating["Value"] = chk_Boolean.Checked;
            //}
            //else if (ddl_Lookup.Value != null)
            //{
            //    drUpdating["Value"] = ddl_Lookup.Value.ToString();
            //}


            //drUpdating["IsAsking"] = (e.NewValues["IsAsking"] == null ? DBNull.Value : e.NewValues["IsAsking"]);
            //drUpdating["Desc"] = e.NewValues["Desc"];
            //drUpdating["LogicalOp"] = e.NewValues["LogicalOp"];


            //drUpdating["FieldName"] = values["FieldName"];
            //drUpdating["CompositeKey"] = values["CompositeKey"];


            // Refresh view criteria list
            grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            grd_Criterias.CancelEdit();
            grd_Criterias.DataBind();

            Session["dsViewHandler"] = dsViewHandler;
            Session["dsViewHandler"] = dsViewHandler;
            e.Cancel = true;
        }


        protected void cmb_FieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (
                grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn,
                    "cmb_FieldName") != null)
            {
                var cmb_FieldName =
                    grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn,
                        "cmb_FieldName") as ASPxComboBox;

                var txt_String =
                    (ASPxTextBox)
                        grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn,
                            "txt_String");
                var txt_Numeric =
                    (ASPxTextBox)
                        grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn,
                            "txt_Numeric");
                var txt_Date =
                    (ASPxDateEdit)
                        grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn,
                            "txt_Date");
                var chk_Boolean =
                    (ASPxCheckBox)
                        grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn,
                            "chk_Boolean");
                var ddl_Lookup =
                    (ASPxDropDownEdit)
                        grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn,
                            "ddl_Lookup");

                // Display input control depend on field type.
                switch (
                    field.GetFieldType("PC", "PC.vPrList", cmb_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr))
                {
                    case "S": // String
                        // Visibility setting
                        txt_String.Visible = true;
                        txt_Numeric.Visible = false;
                        txt_Date.Visible = false;
                        chk_Boolean.Visible = false;
                        ddl_Lookup.Visible = false;

                        txt_String.Text = string.Empty;

                        break;

                    case "N": // Numeric
                        // Visibility setting
                        txt_String.Visible = false;
                        txt_Numeric.Visible = true;
                        txt_Date.Visible = false;
                        chk_Boolean.Visible = false;
                        ddl_Lookup.Visible = false;

                        txt_Numeric.Text = string.Empty;

                        break;

                    case "D": // Date
                        // Visibility setting
                        txt_String.Visible = false;
                        txt_Numeric.Visible = false;
                        txt_Date.Visible = true;
                        chk_Boolean.Visible = false;
                        ddl_Lookup.Visible = false;

                        txt_Date.Text = string.Empty;

                        break;

                    case "B": // Boolean
                        // Visibility setting
                        txt_String.Visible = false;
                        txt_Numeric.Visible = false;
                        txt_Date.Visible = false;
                        chk_Boolean.Visible = true;
                        ddl_Lookup.Visible = false;

                        chk_Boolean.Checked = false;

                        break;

                    case "L": // Lookup                            

                        ddl_Lookup.DataSource =
                            lookup.GetItemList(
                                field.GetLookupID("PC", "PC.vPrList", cmb_FieldName.SelectedItem.Value.ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr);
                        ddl_Lookup.Text = "Text";
                        ddl_Lookup.KeyValue = "Value";
                        ddl_Lookup.Visible = true;
                        ddl_Lookup.DataBind();
                        ddl_Lookup.Value = string.Empty;

                        break;

                    default:

                        // Visibility setting
                        txt_String.Visible = true;
                        txt_Numeric.Visible = false;
                        txt_Date.Visible = false;
                        chk_Boolean.Visible = false;
                        ddl_Lookup.Visible = false;

                        break;
                }
            }
        }

        #endregion
    }
}