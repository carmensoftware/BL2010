using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlueLedger;

using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxPopupControl;

//using System.Web.UI.Design;

namespace BlueLedger.PL.UserControls.ViewHandler
{
    public partial class ListPageCuz2 : BaseUserControl
    {
        #region "Attributes"

        private string _title;
        private string _listPageURL;
        private string _pageCode;
        private DataSet dsViewHandler = new DataSet();

        private Blue.BL.APP.Field field                      = new Blue.BL.APP.Field();
        private Blue.BL.APP.Lookup lookup                    = new Blue.BL.APP.Lookup();
        private Blue.BL.APP.LookupItem lookupItem            = new Blue.BL.APP.LookupItem();
        private Blue.BL.APP.ViewHandler viewHandler          = new Blue.BL.APP.ViewHandler();
        private Blue.BL.APP.ViewHandlerCols viewHandlerCols  = new Blue.BL.APP.ViewHandlerCols();
        private Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr  = new Blue.BL.APP.ViewHandlerCrtr();

        
        ASPxTextBox txt_String;
        ASPxTextBox txt_Numeric;
        ASPxDateEdit txt_Date;
        ASPxCheckBox chk_Boolean;
        ASPxDropDownEdit ddl_Lookup;
        ASPxComboBox cmbFieldName;
        ASPxComboBox cmb_Lookup;
        DataTable dtLookup;

        [Category("Appearance")]
        [Description("Gets or set list page tile.")]
        [Browsable(true)]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        [Category("Behavior")]
        [Description("URL of list page")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [DefaultValue("")]
        [Bindable(true)]
        public string ListPageURL
        {
            get { return this._listPageURL; }
            set { this._listPageURL = value; }
        }

        [Category("Misc")]
        [Description("Gets or set a group of view list")]
        [Browsable(true)]
        public string PageCode
        {
            get { return this._pageCode; }
            set { this._pageCode = value; }
        }

        /// <summary>
        /// Dataset for keep the ViewHandler data.
        /// </summary>
        private DataSet DataSouce
        {
            get 
            {
                return (DataSet)Session["DataSource"];
            }
            set 
            {
                Session["DataSource"] = value;
            }
        }

        private string ListPageCuzMode
        {
            get
            {
                return Session["ListPageCuzMode"].ToString();
            }
            set
            {
                Session["ListPageCuzMode"] = value;
            }
        }
        #endregion

        #region "Operations"

        //public override void DataBind()
        //{
        //    Page_Retrieve();
        //}
        
        /// <summary>
        /// Page Init event workable sorting and paging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected  void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                this.DataSouce = (DataSet)Session["DataSource"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["VIEW_NO"] == null)
            {
                // New View -----------------------------------------------------------------------
                // Get ViewHandler Schema
                bool getViewHandlerSchema = viewHandler.GetSchema(dsViewHandler, LoginInfo.ConnStr);

                if (getViewHandlerSchema)
                {
                    DataRow drViewHandler        = dsViewHandler.Tables[viewHandler.TableName].NewRow();
                    drViewHandler["ViewNo"]      = viewHandler.GetNewID(LoginInfo.ConnStr);
                    drViewHandler["PageCode"]    = PageCode;
                    drViewHandler["Desc"]        = string.Empty;
                    drViewHandler["IsPublic"]    = true;
                    drViewHandler["SearchIn"]    = "A";
                    drViewHandler["IsAdvance"]   = false;
                    drViewHandler["CreatedDate"] = ServerDateTime;
                    drViewHandler["CreatedBy"]   = LoginInfo.LoginName;
                    drViewHandler["UpdatedDate"] = ServerDateTime;
                    drViewHandler["UpdatedBy"]   = LoginInfo.LoginName;
                    dsViewHandler.Tables[viewHandler.TableName].Rows.Add(drViewHandler);
                }
                else
                {
                    // Display Error Message
                    return;
                }

                // Get ViewHandlerCols Schema    
                bool getViewHandlerCols = viewHandlerCols.GetSchema(dsViewHandler, LoginInfo.ConnStr);

                if (!getViewHandlerCols)
                {
                    // Display Error Message
                    return;
                }

                // Get ViewHandlerCrtr Schema
                bool getViewHandlerCrtr = viewHandlerCrtr.GetSchema(dsViewHandler, LoginInfo.ConnStr);

                if (!getViewHandlerCrtr)
                {
                    // Display Error Message
                    return;
                }
            }
            else
            {
                // Edit View ----------------------------------------------------------------------
                // Get ViewHandler data
                bool getViewHandler = viewHandler.Get(dsViewHandler, int.Parse(Request.Params["VIEW_NO"].ToString()), LoginInfo.ConnStr);

                if (!getViewHandler)
                {
                    // Display Error Message
                    return;
                }

                // Get ViewHandlerCols data
                bool getViewHandlerCols = viewHandlerCols.GetList(dsViewHandler, int.Parse(Request.Params["VIEW_NO"].ToString()), LoginInfo.ConnStr);

                if (!getViewHandlerCols)
                {
                    // Display Error Message
                    return;
                }

                // Get ViewHandlerCrtr data
                bool getViewHandlerCrtr = viewHandlerCrtr.GetList(dsViewHandler, int.Parse(Request.Params["VIEW_NO"].ToString()), LoginInfo.ConnStr);

                if (!getViewHandlerCrtr)
                {
                    // Display Error Message
                    return;
                }
            }

            this.DataSouce = dsViewHandler;

            this.Page_Setting();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Page_Setting()
        {
            dsViewHandler = this.DataSouce;

            // Display title ------------------------------------------------------------------
            lbl_Title.Text = Title;

            // Display search in option related to module
            rbl_SearchIn.Items[0].Text = rbl_SearchIn.Items[0].Text + " " + Title;
            rbl_SearchIn.Items[1].Text = rbl_SearchIn.Items[1].Text + " " + Title;

            // Display ViewHandler data
            DataRow drViewHandler       = dsViewHandler.Tables[viewHandler.TableName].Rows[0];
            txt_Desc.Text               = drViewHandler["Desc"].ToString();
            chk_IsPublic.Checked        = bool.Parse(drViewHandler["IsPublic"].ToString());
            rbl_SearchIn.SelectedValue  = drViewHandler["SearchIn"].ToString();

            // Display/Hide Advance Option
            chk_IsAdvance.Checked = bool.Parse(drViewHandler["IsAdvance"].ToString());

            if (chk_IsAdvance.Checked)
            {
                DisplayAdvanceOption();

                txt_AdvOpt.Text = drViewHandler["AdvOpt"].ToString();
            }
            else
            {
                HideAdvanceOption();
            }

            // Display ViewHandlerCols data
            grd_ViewCols.DataSource = dsViewHandler.Tables[viewHandlerCols.TableName];
            grd_ViewCols.DataBind();

            //lst_AvaCols.DataSource = field.GetListViewAvaCols(drViewHandler["PageCode"].ToString(),
            //                         int.Parse(drViewHandler["ViewNo"].ToString()), LoginInfo.ConnStr);

            //lst_AvaCols.TextField = "Desc";
            //lst_AvaCols.ValueField = "FieldName";
            //lst_AvaCols.DataTextField = "Desc";
            //lst_AvaCols.DataValueField = "FieldName";            
            //lst_AvaCols.DataBind();

            //lst_SelCols.DataSource = field.GetListViewSelCols(drViewHandler["PageCode"].ToString(),
            //                         int.Parse(drViewHandler["ViewNo"].ToString()), LoginInfo.ConnStr);
            //lst_SelCols.TextField = "Desc";
            //lst_SelCols.ValueField = "FieldName";
            //lst_SelCols.DataTextField  = "Desc";
            //lst_SelCols.DataValueField = "FieldName";
            //lst_SelCols.DataBind();

            // Display ViewHandlerCrtr data            
            //grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criterias.DataBind();

            grd_Criteria.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.DataBind();
        }

        /// <summary>
        /// Get schema name by pagecode
        /// </summary>
        /// <param name="pageCode"></param>
        /// <returns></returns>
        public string GetSchemaName()
        {
            string result = this.PageCode.Split('.')[0];

            return result.Replace("[", string.Empty).Replace("]", string.Empty);        
        }        

        /// <summary>
        /// String data type textbox show or hide process and
        /// data assigning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_String_Init(object sender, EventArgs e)
        {
            txt_String = (ASPxTextBox)sender;
            GridViewEditItemTemplateContainer container = txt_String.NamingContainer as GridViewEditItemTemplateContainer;
                        
                txt_String.Visible = true;
                
                if (!container.Grid.IsNewRowEditing)
                {
                    txt_String.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
                    
                    if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "S")
                    {
                        txt_String.Visible = true;
                        txt_String.Text    = DataBinder.Eval(container.DataItem, "Value").ToString();
                    }
                    else
                    {
                        txt_String.Visible = false;
                    }
                }
                else
                {
                    txt_String.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
                    txt_String.Visible = true;
                    txt_String.Text    = string.Empty;
                }
        }

        /// <summary>
        /// Numeric data type textbox show or hide process and
        /// data assigning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_Numeric_Init(object sender, EventArgs e)
        {
            txt_Numeric = (ASPxTextBox)sender;
            GridViewEditItemTemplateContainer container = txt_Numeric.NamingContainer as GridViewEditItemTemplateContainer;

            if (!container.Grid.IsNewRowEditing)
            {
                txt_Numeric.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);

                if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "N")
                {
                    txt_Numeric.Visible = true;
                    txt_Numeric.Text    = DataBinder.Eval(container.DataItem, "Value").ToString();
                }
                else
                {
                    txt_Numeric.Visible = false;
                }
            }
            else
            {
                txt_Numeric.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
                txt_Numeric.Visible            = false;
                txt_Numeric.Text               = string.Empty;                
            }
        }

        /// <summary>
        /// Datetime data type textbox show or hide process and
        /// data assigning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_Date_Init(object sender, EventArgs e)
        {
            //txt_Date = (ASPxDateEdit)sender;
            //GridViewEditItemTemplateContainer container = txt_Date.NamingContainer as GridViewEditItemTemplateContainer;

           
            //if (!container.Grid.IsNewRowEditing)
            //{
            //    txt_Date.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);

                //if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "D")
                //{
                //    txt_Date.Visible = true;
                //    txt_Date.Value = DataBinder.Eval(container.DataItem, "Value").ToString();
                //}
                //else
                //{
                //    txt_Date.Visible = false;
                //}               
            //}
            //else
            //{
            //    txt_Date.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
            //    txt_Date.Visible            = false;
            //    txt_Date.Value              = string.Empty;
            //}
        }

        /// <summary>
        /// Boolean data type checkbox show or hide process and
        /// data assigning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_Boolean_Init(object sender, EventArgs e)
        {
            chk_Boolean = (ASPxCheckBox)sender;
            GridViewEditItemTemplateContainer container = chk_Boolean.NamingContainer as GridViewEditItemTemplateContainer;
           
            if (!container.Grid.IsNewRowEditing)
            {
                chk_Boolean.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);

                if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "B")
                {
                    chk_Boolean.Visible  = true;
                    chk_Boolean.Checked  = bool.Parse(DataBinder.Eval(container.DataItem, "Value").ToString());
                }
                else
                {
                    chk_Boolean.Visible = false;
                }
            }
            else
            {
                chk_Boolean.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
                chk_Boolean.Visible = false;
                chk_Boolean.Checked = false;            
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Lookup_Init(object sender, EventArgs e)
        {
            cmb_Lookup = (ASPxComboBox)sender;
            GridViewEditItemTemplateContainer container = cmb_Lookup.NamingContainer as GridViewEditItemTemplateContainer;

            if (!container.Grid.IsNewRowEditing)
            {
                cmb_Lookup.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);

                if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "L")
                {
                    cmb_Lookup.Visible = true;
                    cmb_Lookup.Value = DataBinder.Eval(container.DataItem, "Value").ToString();
                }
                else
                {
                    cmb_Lookup.Visible = false;
                }
            }
            else
            {
                cmb_Lookup.ClientInstanceName = String.Format("txt{0}", container.VisibleIndex);
                cmb_Lookup.Visible            = false;
                cmb_Lookup.Value              = string.Empty;
            }
            
            //dtLookup = lookup.GetItemList(field.GetLookupID(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), string.Empty, LoginInfo.ConnStr), LoginInfo.ConnStr);
            //cmb_Lookup.DataSource = dtLookup;
            //cmb_Lookup.DataBind();            
        }

        /// <summary>
        /// FieldName/ColumnName combo databinding process and
        /// data assigning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbFieldName_Init(object sender, EventArgs e)
        {
            cmbFieldName = (ASPxComboBox)sender;
            GridViewEditItemTemplateContainer container = cmbFieldName.NamingContainer as GridViewEditItemTemplateContainer;

            
            if (!container.Grid.IsNewRowEditing)
            {
                cmbFieldName.Value = DataBinder.Eval(container.DataItem, "FieldName").ToString();
            }
            else
            {
                cmbFieldName.Value = string.Empty;
            }

            DataTable dtField       = field.GetList(this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), LoginInfo.ConnStr);
            cmbFieldName.DataSource = dtField;
            cmbFieldName.DataBind();
        }

        /// <summary>
        /// Command bar click.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {   
            dsViewHandler = this.DataSouce;

            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    // Validation
                    if (txt_Desc.Text.Trim() == string.Empty)
                    {
                        // Display error message.
                        PopupWindow pcWindow    = new PopupWindow("Please enter View Name");
                        pcWindow.ShowOnPageLoad = true;
                        pcWindow.Modal          = true;
                        ASPxPopupControl.Windows.Add(pcWindow);
                        return;
                    }

                    //if (lst_SelCols.Items.Count == 0)
                    //{
                    //    // Display error message.
                    //    PopupWindow pcWindow    = new PopupWindow("Please select display column");
                    //    pcWindow.ShowOnPageLoad = true;
                    //    pcWindow.Modal          = true;
                    //    ASPxPopupControl.Windows.Add(pcWindow);
                    //    return;
                    //}

                    // ViewHandler
                    DataRow drViewHandler = dsViewHandler.Tables[viewHandler.TableName].Rows[0];

                    if (Request.Params["VIEW_NO"] == null)
                    {
                        drViewHandler["ViewNo"]         = viewHandler.GetNewID(LoginInfo.ConnStr);
                        drViewHandler["CreatedDate"]    = ServerDateTime;
                        drViewHandler["CreatedBy"]      = LoginInfo.LoginName;
                    }

                    drViewHandler["PageCode"]       = this.PageCode;
                    drViewHandler["Desc"]           = txt_Desc.Text.Trim();
                    drViewHandler["IsPublic"]       = chk_IsPublic.Checked;
                    drViewHandler["IsStandard"]     = false;
                    drViewHandler["SearchIn"]       = rbl_SearchIn.SelectedItem.Value;
                    drViewHandler["IsAdvance"]      = chk_IsAdvance.Checked;
                    drViewHandler["AdvOpt"]         = (chk_IsAdvance.Checked ? txt_AdvOpt.Text.Trim() : string.Empty);
                    drViewHandler["UpdatedDate"]    = ServerDateTime;
                    drViewHandler["UpdatedBy"]      = LoginInfo.LoginName;

                    // ViewHandler Column
                    foreach (DataRow drViewHanlderCols in dsViewHandler.Tables[viewHandlerCols.TableName].Rows)
                    {
                        // Delete all exist display column
                        drViewHanlderCols.Delete();
                    }

                    //for (int i = 0; i < lst_SelCols.Items.Count; i++)
                    //{
                    //    // Insert selected display column
                    //    DataRow drViewHandlerCols       = dsViewHandler.Tables[viewHandlerCols.TableName].NewRow();
                    //    drViewHandlerCols["ViewNo"]     = drViewHandler["ViewNo"];
                    //    drViewHandlerCols["ViewColNo"]  = i + 1;
                    //    drViewHandlerCols["SeqNo"]      = i + 1;
                    //    drViewHandlerCols["FieldName"]  = lst_SelCols.Items[i].Value.ToString();
                    //    dsViewHandler.Tables[viewHandlerCols.TableName].Rows.Add(drViewHandlerCols);
                    //}
                    
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
                    bool saved = viewHandler.Save(dsViewHandler, LoginInfo.ConnStr);
                    
                    if (saved)
                    { 
                        // Update selected view cookies
                        Response.Cookies[PageCode].Value    = drViewHandler["ViewNo"].ToString();
                        Response.Cookies[PageCode].Expires  = DateTime.MaxValue;
                        Response.Redirect(ListPageURL);
                    }
                    else
                    {                    
                        // Display Error Message
                        return;
                    }
                    
                    break;
                case "DELETE":
                    // Display confrim message.
                    pop_Confirm.ShowOnPageLoad = true;
                    break;

                case "BACK":
                    Response.Redirect(ListPageURL);
                    break;
            }                        
        }

        /// <summary>
        /// Display/Hide Advance Option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_AdvOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_IsAdvance.Checked)
            {
                // Display Advance Option
                DisplayAdvanceOption();
            }
            else
            {
                // Hide Advance Option
                HideAdvanceOption();
            }
        }
         
        /// <summary>
        /// Binding for criteria gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    GridViewDataColumn  col       = (GridViewDataColumn)grd_Criterias.Columns["Value"];
            //    ASPxLabel     lbl_Value       = (ASPxLabel)grd_Criterias.FindRowCellTemplateControl(e.VisibleIndex, col, "lbl_Value");

            //    if (lbl_Value != null)
            //    {
            //        lbl_Value.Text = e.GetValue("Value").ToString();
            //    }
            //}
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "FieldName")
            {
                ASPxComboBox filter = e.Editor as ASPxComboBox;

                dsViewHandler       = this.DataSouce;

                string TableName    = dsViewHandler.Tables[viewHandler.TableName].Rows[0]["PageCode"].ToString();
                DataTable dtField   = field.GetList(TableName.Replace("[", string.Empty).Replace("]", string.Empty), LoginInfo.ConnStr);

                filter.DataSource   = dtField;
                filter.DataBindItems();
            }
        }

        /// <summary>
        /// Assign keys value for primary key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string ViewNo       = e.GetListSourceFieldValue("ViewNo").ToString();
                string ViewCrtrNo   = e.GetListSourceFieldValue("ViewCrtrNo").ToString();
                e.Value             = ViewNo + "," + ViewCrtrNo;
            }
        }

        /// <summary>
        /// Delete View Criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //dsViewHandler   = this.DataSouce;
            //string[] pk     = e.Keys[0].ToString().Split(',');

            //// Find deleting row.
            //DataRow drDeleting = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Find(pk);
            //drDeleting.Delete();

            //// Update SeqNo (re-order column number)
            //int i = 1;
            //foreach (DataRow drViewHandlerCrtr in dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows)
            //{
            //    if (drViewHandlerCrtr.RowState != DataRowState.Deleted)
            //    {
            //        drViewHandlerCrtr["ViewCrtrNo"] = i;
            //        drViewHandlerCrtr["SeqNo"]      = i;
            //        i++;
            //    }
            //}

            //// Update changed to session.
            //grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criterias.DataBind();
            //this.DataSouce = dsViewHandler;

            //e.Cancel = true;
        }

        /// <summary>
        /// Update View Criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //dsViewHandler = this.DataSouce;

            //// Find updating row.
            //string[] pk         = e.Keys[0].ToString().Split(',');            
            //DataRow drUpdating  = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Find(pk);
           
            ////Update
            ////Get FieldName
            //drUpdating["FieldName"]     = cmbFieldName.SelectedItem.Value.ToString();
            
            //// Operator.
            //drUpdating["Operator"]      = e.NewValues["Operator"];

            //ASPxTextBox txt_String      = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_String");
            //ASPxTextBox txt_Numeric     = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Numeric");
            //ASPxDateEdit txt_Date       = (ASPxDateEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Date");
            //ASPxCheckBox chk_Boolean    = (ASPxCheckBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "chk_Boolean");
            //ASPxComboBox cmb_Lookup     = (ASPxComboBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "cmb_Lookup");


            //if ((txt_String != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "S"))
            //{
            //    drUpdating["Value"] = txt_String.Text.ToString();
            //}
            //else if ((txt_Numeric != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "N"))
            //{
            //    drUpdating["Value"] = txt_Numeric.Text.ToString();
            //}
            //else if ((txt_Date != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "D"))
            //{
            //    drUpdating["Value"] = txt_Date.Date.ToString("dd/MM/yyyy");//txt_Date.Value.ToString();
            //}
            //else if ((chk_Boolean != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "B"))
            //{
            //    drUpdating["Value"] = chk_Boolean.Checked;
            //}
            //else if ((cmb_Lookup != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "L"))
            //{
            //    drUpdating["Value"] = cmb_Lookup.SelectedItem.Value.ToString();
            //}
            
            //drUpdating["IsAsking"]      = (e.NewValues["IsAsking"] == null ? DBNull.Value : e.NewValues["IsAsking"]);
            //drUpdating["Desc"]          = e.NewValues["Desc"];
            //drUpdating["LogicalOp"]     = e.NewValues["LogicalOp"];

            //// Refresh view criteria list
            //grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criterias.CancelEdit();
            //grd_Criterias.DataBind();

            //this.DataSouce = dsViewHandler;
           
            //e.Cancel = true;
        }

        /// <summary>
        /// Assign default value to new row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            //dsViewHandler = this.DataSouce;
          
            //e.NewValues["ViewNo"]       = int.Parse(dsViewHandler.Tables[viewHandler.TableName].Rows[0]["ViewNo"].ToString());
            //e.NewValues["ViewCrtrNo"]   = grd_Criterias.VisibleRowCount + 1;
            //e.NewValues["SeqNo"]        = grd_Criterias.VisibleRowCount + 1;
        }
        
        /// <summary>
        /// Insert new view criteria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Criterias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //dsViewHandler = this.DataSouce;
            
            //// Insert new row
            //DataRow drInserting         = dsViewHandler.Tables[viewHandlerCrtr.TableName].NewRow();
            //drInserting["ViewNo"]       = int.Parse(dsViewHandler.Tables[viewHandler.TableName].Rows[0]["ViewNo"].ToString());
            //drInserting["ViewCrtrNo"]   = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Count + 1;  //grd_Criterias.VisibleRowCount + 1;
            //drInserting["SeqNo"]        = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Count + 1;  //grd_Criterias.VisibleRowCount + 1;
            //drInserting["FieldName"]    = cmbFieldName.SelectedItem.Value.ToString();
            //drInserting["Operator"]     = e.NewValues["Operator"];

            //// Serching control for make sure that what control is visible.
            //ASPxTextBox txt_String      = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_String");
            //ASPxTextBox txt_Numeric     = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Numeric");
            //ASPxDateEdit txt_Date       = (ASPxDateEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Date");
            //ASPxCheckBox chk_Boolean    = (ASPxCheckBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "chk_Boolean");
            //ASPxComboBox cmb_Lookup     = (ASPxComboBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "cmb_Lookup");

            //// Condition for based on selected data type and data assigned.
            //if ((txt_String != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "S"))
            //{
            //    drInserting["Value"] = txt_String.Text.ToString();
            //}
            //else if ((txt_Numeric != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "N"))
            //{
            //    drInserting["Value"] = txt_Numeric.Text.ToString();
            //}
            //else if ((txt_Date != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "D"))
            //{
            //    drInserting["Value"] = txt_Date.Date.ToString("dd/MM/yyyy"); //txt_Date.Value.ToString();
            //}
            //else if ((chk_Boolean != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "B"))
            //{
            //    drInserting["Value"] = chk_Boolean.Checked;
            //}
            //else if ((cmb_Lookup != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmbFieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "L"))
            //{
            //    drInserting["Value"] = cmb_Lookup.SelectedItem.Value.ToString();
            //}
                        
            //drInserting["IsAsking"]     = (e.NewValues["IsAsking"] == null ? DBNull.Value : e.NewValues["IsAsking"]);
            //drInserting["Desc"]         = e.NewValues["Desc"];
            //drInserting["LogicalOp"]    = e.NewValues["LogicalOp"];
            //dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Add(drInserting);
            
            ////Refresh view criteria list
            //grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criterias.CancelEdit();
            //grd_Criterias.DataBind();            
            
            //this.DataSouce = dsViewHandler;

            //e.Cancel = true;
        }
        
        /// <summary>
        /// Display Advance Criteria Option
        /// </summary>
        private void DisplayAdvanceOption()
        {
            //grd_Criteria.Columns[7].HeaderStyle.Width = Unit.Percentage(grd_Criteria.Columns[7].HeaderStyle.Width.Value + grd_Criteria.Columns[8].HeaderStyle.Width.Value);
           // grd_Criteria.Columns[7].ItemStyle.Width = Unit.Percentage(grd_Criteria.Columns[7].ItemStyle.Width.Value + grd_Criteria.Columns[8].ItemStyle.Width.Value);
            grd_Criteria.Columns[7].Visible = false;
            p_IsAdvance.Visible = true;
        }

        /// <summary>
        /// Hide Advance Criteria Option
        /// </summary>
        private void HideAdvanceOption()
        {
            //grd_Criteria.Columns[7].HeaderStyle.Width = Unit.Percentage(grd_Criteria.Columns[7].HeaderStyle.Width.Value - grd_Criteria.Columns[8].HeaderStyle.Width.Value);
            //grd_Criteria.Columns[7].ItemStyle.Width = Unit.Percentage(grd_Criteria.Columns[7].ItemStyle.Width.Value - grd_Criteria.Columns[8].ItemStyle.Width.Value);
            grd_Criteria.Columns[7].Visible = true;
            p_IsAdvance.Visible = false;
        }

        /// <summary>
        /// Add new criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AddCriteria_Click(object sender, EventArgs e)
        {
            dsViewHandler = this.DataSouce;

            //grd_Criterias.AddNewRow();
            //grd_Criterias.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            //grd_Criterias.DataBind();

            DataRow drNew = dsViewHandler.Tables[viewHandlerCrtr.TableName].NewRow();
            drNew["ViewNo"] = int.Parse(dsViewHandler.Tables[viewHandler.TableName].Rows[0]["ViewNo"].ToString());
            drNew["ViewCrtrNo"] = grd_Criteria.Rows.Count + 1;
            drNew["SeqNo"] = grd_Criteria.Rows.Count + 1;
            drNew["FieldName"] = string.Empty;
            drNew["Operator"] = string.Empty;
            dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Add(drNew);

            grd_Criteria.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.EditIndex = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows.Count - 1;
            grd_Criteria.DataBind();

            this.ListPageCuzMode = "NEW";
        }

        /// <summary>
        /// Add View Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_Add_Click(object sender, EventArgs e)
        //{
        //    if (lst_AvaCols.SelectedItem != null)
        //    {
        //        lst_SelCols.Items.Add(lst_AvaCols.SelectedItem);
        //        //lst_SelCols.Items.Add(lst_AvaCols.SelectedItem.Text);
        //    }
        //}

        /// <summary>
        /// Remove View Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_Remove_Click(object sender, EventArgs e)
        //{
        //    if (lst_SelCols.SelectedItem != null)
        //    {
        //        lst_AvaCols.Items.Add(lst_SelCols.SelectedItem);
        //        //lst_AvaCols.Items.Add(lst_SelCols.SelectedItem.Text);
        //    }
        //}

        /// <summary>
        /// Move View Column Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_MoveUp_Click(object sender, EventArgs e)
        //{
        //    if (lst_SelCols.SelectedItem != null)
        //    {
        //        if (lst_SelCols.SelectedItem.Index > 0)
        //        //if (lst_SelCols.SelectedIndex > 0)
        //        {
        //            lst_SelCols.Items.Insert(lst_SelCols.SelectedItem.Index - 1, lst_SelCols.SelectedItem);
        //            //lst_SelCols.Items.Insert(lst_SelCols.SelectedIndex - 1, lst_SelCols.SelectedItem);
        //        }
        //    }
        //}

        /// <summary>
        /// Move View Column Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_MoveDown_Click(object sender, EventArgs e)
        //{
        //    if (lst_SelCols.SelectedItem != null)
        //    {
        //        if (lst_SelCols.SelectedItem.Index < lst_SelCols.Items.Count - 1)
        //        {
        //            lst_SelCols.Items.Insert(lst_SelCols.SelectedItem.Index + 1, lst_SelCols.SelectedItem);
        //        }
        //        //if (lst_SelCols.SelectedIndex < lst_SelCols.Items.Count - 1)
        //        //{
        //        //    lst_SelCols.Items.Insert(lst_SelCols.SelectedIndex + 1, lst_SelCols.SelectedItem);
        //        //}
        //    }
        //}

        /// <summary>
        /// Confirm delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Yes_Click(object sender, EventArgs e)
        {
            dsViewHandler = this.DataSouce;

            // Delete ViewHandler
            dsViewHandler.Tables[viewHandler.TableName].Rows[0].Delete();

            // Delete ViewHandlerCols
            foreach (DataRow drViewHandlerCols in dsViewHandler.Tables[viewHandlerCols.TableName].Rows)
            {
                if (drViewHandlerCols.RowState != DataRowState.Deleted)
                {
                    drViewHandlerCols.Delete();
                }
            }

            // Delete ViewHandlerCrtr
            foreach (DataRow drViewHandlerCrtr in dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows)
            {
                if (drViewHandlerCrtr.RowState != DataRowState.Deleted)
                {
                    drViewHandlerCrtr.Delete();
                }
            }

            // Commit changed to database
            bool delete = viewHandler.Delete(dsViewHandler, LoginInfo.ConnStr);

            if (delete)
            {
                // Clear cookies
                Response.Cookies[PageCode].Expires = DateTime.Now;

                // Redirec to List Page
                Response.Redirect(ListPageURL);
            }
            else
            { 
                // Display Error Message
            }
        }

        /// <summary>
        /// FieldName aspxcombo box selected changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_FieldName_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //if (grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn, "cmb_FieldName") != null)
            //{
            //    ASPxComboBox cmb_FieldName  = grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["FieldName"] as GridViewDataColumn, "cmb_FieldName") as ASPxComboBox;
                
            //    ASPxTextBox txt_String      = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_String");
            //    ASPxTextBox txt_Numeric     = (ASPxTextBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Numeric");
            //    ASPxDateEdit txt_Date       = (ASPxDateEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "txt_Date");
            //    ASPxCheckBox chk_Boolean    = (ASPxCheckBox)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "chk_Boolean");
            //    ASPxDropDownEdit ddl_Lookup = (ASPxDropDownEdit)grd_Criterias.FindEditRowCellTemplateControl(grd_Criterias.Columns["Value"] as GridViewDataColumn, "ddl_Lookup");
                
            //    // Display input control depend on field type.
            //    switch (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), cmb_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString())
            //    {
            //        case "S": // String
            //            // Visibility setting
            //            txt_String.Visible  = true;
            //            txt_Numeric.Visible = false;
            //            txt_Date.Visible    = false;
            //            chk_Boolean.Visible = false;
            //            cmb_Lookup.Visible  = false;

            //            txt_String.Text     = string.Empty;

            //            break;

            //        case "N": // Numeric
            //            // Visibility setting
            //            txt_String.Visible  = false;
            //            txt_Numeric.Visible = true;
            //            txt_Date.Visible    = false;
            //            chk_Boolean.Visible = false;
            //            cmb_Lookup.Visible = false;

            //            txt_Numeric.Text    = string.Empty;

            //            break;

            //        case "D": // Date
            //            // Visibility setting
            //            txt_String.Visible  = false;
            //            txt_Numeric.Visible = false;
            //            txt_Date.Visible    = true;
            //            chk_Boolean.Visible = false;
            //            cmb_Lookup.Visible = false;

            //            txt_Date.Value       = string.Empty;

            //            break;

            //        case "B": // Boolean
            //            // Visibility setting
            //            txt_String.Visible  = false;
            //            txt_Numeric.Visible = false;
            //            txt_Date.Visible    = false;
            //            chk_Boolean.Visible = true;
            //            cmb_Lookup.Visible = false;
            //            chk_Boolean.Checked = false;

            //            break;

            //        case "L": // Lookup                            

                       
            //            cmb_Lookup.Visible  = true;
            //            txt_String.Visible  = false;
            //            txt_Numeric.Visible = false;
            //            txt_Date.Visible    = false;
            //            chk_Boolean.Visible = false;
                        
            //            break;

            //        default:

            //            // Visibility setting
            //            txt_String.Visible  = true;
            //            txt_Numeric.Visible = false;
            //            txt_Date.Visible    = false;
            //            chk_Boolean.Visible = false;
            //            ddl_Lookup.Visible  = false;

            //            break;
            //    }
            //}            
        }

        #endregion  
    
        #region "Operation - View Column"

        /// <summary>
        /// Display Journal Voucher view column data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ViewCols_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add delete command.
                if (grd_ViewCols.EditIndex < 0)
                {
                    LinkButton lnkb_Del = (LinkButton)e.Row.Cells[0].Controls[2];
                    lnkb_Del.OnClientClick = "return confirm('Are you sure to delete this column?');";
                }

                // SeqNo
                if (e.Row.FindControl("lbl_SeqNo") != null)
                {
                    Label lbl_SeqNo = (Label)e.Row.FindControl("lbl_SeqNo");
                    lbl_SeqNo.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }
                if (e.Row.FindControl("txt_SeqNo") != null)
                {
                    TextBox txt_SeqNo = (TextBox)e.Row.FindControl("txt_SeqNo");
                    txt_SeqNo.Text = DataBinder.Eval(e.Row.DataItem, "SeqNo").ToString();
                }

                // FieldName
                if (e.Row.FindControl("lbl_FieldName") != null)
                {
                    Label lbl_FieldName = (Label)e.Row.FindControl("lbl_FieldName");
                    lbl_FieldName.Text = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ?
                        field.GetDesc(DataBinder.Eval(e.Row.DataItem, "FieldName").ToString(), LoginInfo.ConnStr) :
                        field.GetOthDesc(DataBinder.Eval(e.Row.DataItem, "FieldName").ToString(), LoginInfo.ConnStr));
                }
                if (e.Row.FindControl("ddl_FieldName") != null)
                {
                    DropDownList ddl_FieldName = (DropDownList)e.Row.FindControl("ddl_FieldName");
                    ddl_FieldName.DataSource = field.GetList(this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), LoginInfo.ConnStr);
                    ddl_FieldName.DataTextField = (LoginInfo.BuFmtInfo.LangCode.ToUpper() == "EN-US" ? "Desc" : "OthDesc");
                    ddl_FieldName.DataValueField = "FieldName";
                    ddl_FieldName.DataBind();
                    ddl_FieldName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "FieldName").ToString();
                }

                // Width
                if (e.Row.FindControl("lbl_Width") != null)
                {
                    Label lbl_Width = (Label)e.Row.FindControl("lbl_Width");
                    lbl_Width.Text = DataBinder.Eval(e.Row.DataItem, "Width").ToString();
                }
            }
        }

        /// <summary>
        /// Edit Journal Voucher view column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ViewCols_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //grd_ViewCols.DataSource = dsJVView.Tables[jvViewCol.TableName];
            //grd_ViewCols.EditIndex = e.NewEditIndex;
            //grd_ViewCols.DataBind();

            //// Enable new button
            //btn_NewAccViewCol.Enabled = true;
        }

        /// <summary>
        /// Cancel add/edit Journal Voucher view column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ViewCols_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //// Remove adding row
            //if (dsJVView.Tables[jvViewCol.TableName].Rows[grd_ViewCols.Rows[e.RowIndex].DataItemIndex].RowState == DataRowState.Added &&
            //    btn_NewAccViewCol.Enabled == false)
            //{
            //    dsJVView.Tables[jvViewCol.TableName].Rows[grd_ViewCols.Rows[e.RowIndex].DataItemIndex].Delete();
            //}

            //grd_ViewCols.DataSource = dsJVView.Tables[jvViewCol.TableName];
            //grd_ViewCols.EditIndex = -1;
            //grd_ViewCols.DataBind();

            //// Enable new button
            //btn_NewAccViewCol.Enabled = true;
        }

        /// <summary>
        /// Save changed Journal Voucher view column to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ViewCols_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //// Initial edit controls
            //DropDownList ddl_FieldName = (DropDownList)grd_ViewCols.Rows[e.RowIndex].FindControl("ddl_FieldName");

            //// Update changing
            //DataRow drUpdate = dsJVView.Tables[jvViewCol.TableName].Rows[grd_ViewCols.Rows[e.RowIndex].DataItemIndex];
            //drUpdate["FieldName"] = ddl_FieldName.SelectedItem.Value;

            //// Reresh Journal Voucher column list
            //grd_ViewCols.DataSource = dsJVView.Tables[jvViewCol.TableName];
            //grd_ViewCols.EditIndex = -1;
            //grd_ViewCols.DataBind();

            //Session["dsJVView"] = dsJVView;

            //// Enable new button
            //btn_NewAccViewCol.Enabled = true;
        }

        /// <summary>
        /// Insert new Journal Voucher view column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_NewAccViewCol_Click(object sender, EventArgs e)
        {
            //DataRow drNew = dsJVView.Tables[jvViewCol.TableName].NewRow();

            //// Assign default value
            //drNew["ViewID"] = dsJVView.Tables[jvView.TableName].Rows[0]["ViewID"];

            //if (dsJVView.Tables[jvViewCol.TableName].Rows.Count > 0)
            //{
            //    drNew["ViewColID"] = int.Parse(dsJVView.Tables[jvViewCol.TableName].Rows[dsJVView.Tables[jvViewCol.TableName].Rows.Count - 1]["ViewColID"].ToString()) + 1;
            //}
            //else
            //{
            //    drNew["ViewColID"] = 1;
            //}

            //drNew["SeqNo"] = dsJVView.Tables[jvViewCol.TableName].Rows.Count + 1;
            //drNew["FieldName"] = string.Empty;

            //dsJVView.Tables[jvViewCol.TableName].Rows.Add(drNew);

            //// Reorder SeqNo
            //int intSeqNo = 1;

            //foreach (DataRow drJVViewCol in dsJVView.Tables[jvViewCol.TableName].Rows)
            //{
            //    if (drJVViewCol.RowState != DataRowState.Deleted && drJVViewCol.RowState != DataRowState.Detached)
            //    {
            //        drJVViewCol["SeqNo"] = intSeqNo;
            //        intSeqNo++;
            //    }
            //}

            //// Reresh Journal Voucher column list
            //grd_ViewCols.DataSource = dsJVView.Tables[jvViewCol.TableName];
            //grd_ViewCols.EditIndex = grd_ViewCols.Rows.Count;
            //grd_ViewCols.DataBind();

            //Session["dsJVView"] = dsJVView;


            //// Disable new button
            //btn_NewAccViewCol.Enabled = false;
        }

        /// <summary>
        /// Delete Journal Voucher view column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ViewCols_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //// Delete Journal Voucher view column
            //// Find keys value of deleting row
            //int ViewID = int.Parse(grd_ViewCols.DataKeys[e.RowIndex]["ViewID"].ToString());
            //int ViewColumnID = int.Parse(grd_ViewCols.DataKeys[e.RowIndex]["ViewColID"].ToString());

            //// Delete Journal Voucher view column data which match the view id and view column id.
            //for (int i = dsJVView.Tables[jvViewCol.TableName].Rows.Count - 1; i >= 0; i--)
            //{
            //    DataRow drJVViewCol = dsJVView.Tables[jvViewCol.TableName].Rows[i];

            //    if (drJVViewCol.RowState != DataRowState.Deleted && drJVViewCol.RowState != DataRowState.Detached)
            //    {
            //        if (int.Parse(drJVViewCol["ViewID"].ToString()) == ViewID &&
            //            int.Parse(drJVViewCol["ViewColID"].ToString()) == ViewColumnID)
            //        {
            //            drJVViewCol.Delete();
            //        }
            //    }
            //}

            //// Reorder SeqNo
            //int intSeqNo = 1;

            //foreach (DataRow drJVViewCol in dsJVView.Tables[jvViewCol.TableName].Rows)
            //{
            //    if (drJVViewCol.RowState != DataRowState.Deleted && drJVViewCol.RowState != DataRowState.Detached)
            //    {
            //        drJVViewCol["SeqNo"] = intSeqNo;
            //        intSeqNo++;
            //    }
            //}

            //// Reresh Journal Voucher column list
            //grd_ViewCols.DataSource = dsJVView.Tables[jvViewCol.TableName];
            //grd_ViewCols.EditIndex = -1;
            //grd_ViewCols.DataBind();

            //Session["dsJVView"] = dsJVView;

            //// Enable new button
            //btn_NewAccViewCol.Enabled = true;
        }

        #endregion


        #region "Operation - View Criteria"

        protected void grd_Criteria_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Criteria.DataSource = this.DataSouce.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.EditIndex = e.NewEditIndex;
            grd_Criteria.DataBind();

            this.ListPageCuzMode = "EDIT";
        }

        protected void grd_Criteria_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList ddl_FieldName = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_FieldName") as DropDownList;
            DropDownList ddl_Operator = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_Operator") as DropDownList;
            DropDownList ddl_Lookup = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_Lookup") as DropDownList;
            DropDownList ddl_LogicalOp = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_LogicalOp") as DropDownList;
            TextBox txt_String = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_String") as TextBox;
            TextBox txt_Numeric = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_Numeric") as TextBox;
            ASPxDateEdit txt_Date = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_Date") as ASPxDateEdit;
            TextBox txt_FilterDesc = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_FilterDesc") as TextBox;
            CheckBox chk_Boolean = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("chk_Boolean") as CheckBox;
            CheckBox chk_IsAsking = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("chk_IsAsking") as CheckBox;

            dsViewHandler = this.DataSouce;

            DataRow drUpdating = dsViewHandler.Tables[viewHandlerCrtr.TableName].Rows[grd_Criteria.Rows[grd_Criteria.EditIndex].DataItemIndex];
            
            //Update
            //Get FieldName
            drUpdating["FieldName"] = ddl_FieldName.SelectedItem.Value.ToString();

            // Operator.
            drUpdating["Operator"] = ddl_Operator.SelectedItem.Value.ToString();

            if ((txt_String != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "S"))
            {
                drUpdating["Value"] = txt_String.Text.ToString();
            }
            else if ((txt_Numeric != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "N"))
            {
                drUpdating["Value"] = txt_Numeric.Text.ToString();
            }
            else if ((txt_Date != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "D"))
            {
                //drUpdating["Value"] = txt_Date.Date.ToString("dd/MM/yyyy");//txt_Date.Value.ToString();
                drUpdating["Value"] = txt_Date.Date;
            }
            else if ((chk_Boolean != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "B"))
            {
                drUpdating["Value"] = chk_Boolean.Checked;
            }
            else if ((ddl_Lookup != null) && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "L"))
            {
                drUpdating["Value"] = ddl_Lookup.SelectedItem.Value.ToString();
            }

            //drUpdating["IsAsking"] = (e.NewValues["IsAsking"] == null ? DBNull.Value : e.NewValues["IsAsking"]);
            drUpdating["IsAsking"] = chk_IsAsking.Checked;
            drUpdating["Desc"] = txt_FilterDesc.Text;
            drUpdating["LogicalOp"] = ddl_LogicalOp.SelectedItem.Value.ToString();

            // Refresh view criteria list
            grd_Criteria.DataSource = dsViewHandler.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.EditIndex = -1;
            grd_Criteria.DataBind();

            this.DataSouce = dsViewHandler;
        }

        protected void grd_Criteria_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (this.ListPageCuzMode.ToUpper() == "NEW")
            {
                this.DataSouce.Tables[viewHandlerCrtr.TableName].Rows[this.DataSouce.Tables[viewHandlerCrtr.TableName].Rows.Count - 1].Delete();
            }
            if (this.ListPageCuzMode.ToUpper() == "EDIT")
            {
                this.DataSouce.Tables[viewHandlerCrtr.TableName].Rows[this.DataSouce.Tables[viewHandlerCrtr.TableName].Rows.Count - 1].CancelEdit();
            }

            this.ListPageCuzMode = string.Empty;

            grd_Criteria.DataSource = this.DataSouce.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.EditIndex = -1;
            grd_Criteria.DataBind();
        }

        protected void grd_Criteria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_FieldName = e.Row.FindControl("ddl_FieldName") as DropDownList;
                TextBox txt_String = e.Row.FindControl("txt_String") as TextBox;
                TextBox txt_Numeric = e.Row.FindControl("txt_Numeric") as TextBox;
                ASPxDateEdit txt_Date = e.Row.FindControl("txt_Date") as ASPxDateEdit;
                CheckBox chk_Boolean = e.Row.FindControl("chk_Boolean") as CheckBox;
                DropDownList ddl_Lookup = e.Row.FindControl("ddl_Lookup") as DropDownList;

                if (e.Row.FindControl("ddl_FieldName") != null)
                {
                    ddl_FieldName.DataSource = field.GetList(this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), LoginInfo.ConnStr);
                    ddl_FieldName.DataValueField = "FieldName";
                    ddl_FieldName.DataTextField = "Desc";
                    ddl_FieldName.DataBind();
                    ddl_FieldName.SelectedIndex = 0;

                    if (ddl_FieldName.SelectedIndex != -1)
                    {
                        ddl_FieldName.SelectedValue = DataBinder.Eval(e.Row.DataItem, "FieldName").ToString();

                        if (txt_String != null && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "S"))
                        {
                            txt_String.Text = DataBinder.Eval(e.Row.DataItem, "Value").ToString();
                        }

                        if (txt_Numeric != null && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "N"))
                        {
                            txt_Numeric.Text = DataBinder.Eval(e.Row.DataItem, "Value").ToString();
                        }

                        if (txt_Date != null && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "D"))
                        {
                            txt_Date.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "Value").ToString());
                        }

                        if (chk_Boolean != null && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "B"))
                        {
                            chk_Boolean.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Value") == DBNull.Value ? false : DataBinder.Eval(e.Row.DataItem, "Value"));
                        }

                        if (ddl_Lookup != null && (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString() == "L"))
                        {
                            ddl_Lookup.DataSource = lookup.GetItemList(field.GetLookupID(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr);
                            ddl_Lookup.DataTextField = "Text";
                            ddl_Lookup.DataValueField = "Value";
                            ddl_Lookup.DataBind();

                            if (DataBinder.Eval(e.Row.DataItem, "Value") == DBNull.Value)
                            {
                                ddl_Lookup.SelectedIndex = 0;
                            }
                            else
                            {
                                ddl_Lookup.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Value").ToString();
                            }
                        }

                        // Display input control depend on field type.
                        switch (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString())
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

                                //txt_Date.Text = string.Empty;

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


                                ddl_Lookup.Visible = true;
                                txt_String.Visible = false;
                                txt_Numeric.Visible = false;
                                txt_Date.Visible = false;
                                chk_Boolean.Visible = false;

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
               
                if (e.Row.FindControl("lbl_ColName") != null)
                {
                    Label lbl_ColName = e.Row.FindControl("lbl_ColName") as Label;
                    lbl_ColName.Text = field.GetDesc(DataBinder.Eval(e.Row.DataItem, "FieldName").ToString(), LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_Operator") != null)
                {
                    Label lbl_Operator = e.Row.FindControl("lbl_Operator") as Label;
                    lbl_Operator.Text = DataBinder.Eval(e.Row.DataItem, "Operator").ToString();
                }

                if (e.Row.FindControl("lbl_Value") != null)
                {
                    Label lbl_Value = e.Row.FindControl("lbl_Value") as Label;

                    if (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), DataBinder.Eval(e.Row.DataItem, "FieldName").ToString(), LoginInfo.ConnStr).ToString() == "D")
                    {
                        lbl_Value.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Value")).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                    }
                    else
                    {
                        lbl_Value.Text = DataBinder.Eval(e.Row.DataItem, "Value").ToString();
                    }
                }

                if (e.Row.FindControl("lbl_FilterDesc") != null)
                {
                    Label lbl_FilterDesc = e.Row.FindControl("lbl_FilterDesc") as Label;
                    lbl_FilterDesc.Text = DataBinder.Eval(e.Row.DataItem, "Desc").ToString();
                }

                if (e.Row.FindControl("txt_FilterDesc") != null)
                {
                    TextBox txt_FilterDesc = e.Row.FindControl("txt_FilterDesc") as TextBox;
                    txt_FilterDesc.Text = DataBinder.Eval(e.Row.DataItem, "Desc").ToString();
                }

                if (e.Row.FindControl("lbl_LogicalOp") != null)
                {
                    Label lbl_LogicalOp = e.Row.FindControl("lbl_LogicalOp") as Label;
                    lbl_LogicalOp.Text = DataBinder.Eval(e.Row.DataItem, "LogicalOp").ToString();
                }


                if (e.Row.FindControl("chk_IsAsking") != null)
                {
                    CheckBox chk_IsAsking = e.Row.FindControl("chk_IsAsking") as CheckBox;
                    chk_IsAsking.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsAsking") == DBNull.Value ? false : DataBinder.Eval(e.Row.DataItem, "IsAsking"));
                }
            }
        }

        protected void grd_Criteria_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.DataSouce.Tables[viewHandlerCrtr.TableName].Rows[grd_Criteria.Rows[e.RowIndex].DataItemIndex].Delete();

            grd_Criteria.DataSource = this.DataSouce.Tables[viewHandlerCrtr.TableName];
            grd_Criteria.EditIndex = -1;
            grd_Criteria.DataBind();
        }

        protected void btn_No_Click(object sender, EventArgs e)
        {
            pop_Confirm.ShowOnPageLoad = false;
        }

        protected void ddl_FieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_FieldName") != null)
            {
                TextBox txt_String = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_String") as TextBox;
                TextBox txt_Numeric = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_Numeric") as TextBox;
                ASPxDateEdit txt_Date = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("txt_Date") as ASPxDateEdit;
                CheckBox chk_Boolean = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("chk_Boolean") as CheckBox;
                DropDownList ddl_FieldName = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_FieldName") as DropDownList;
                DropDownList ddl_Lookup = grd_Criteria.Rows[grd_Criteria.EditIndex].FindControl("ddl_Lookup") as DropDownList;

                // Display input control depend on field type.
                switch (field.GetFieldType(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr).ToString())
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

                        ddl_Lookup.Visible = true;
                        txt_String.Visible = false;
                        txt_Numeric.Visible = false;
                        txt_Date.Visible = false;
                        chk_Boolean.Visible = false;

                        ddl_Lookup.DataSource = lookup.GetItemList(field.GetLookupID(this.GetSchemaName(), this.PageCode.Replace("[", string.Empty).Replace("]", string.Empty), ddl_FieldName.SelectedItem.Value.ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr);
                        ddl_Lookup.DataTextField = "Text";
                        ddl_Lookup.DataValueField = "Value";
                        ddl_Lookup.DataBind();
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
