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

using System.Web.UI.HtmlControls;


namespace BlueLedger.PL.UserControls.ViewHandler
{
    public partial class ListPageHQ : BaseUserControl
    {
        #region "Attributes"
        
        private DataSet dsDataList                      = new DataSet();
        private Blue.BL.APP.ViewHandler viewHandler          = new Blue.BL.APP.ViewHandler();
        private Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr  = new Blue.BL.APP.ViewHandlerCrtr();
        private Blue.BL.APP.WF workFlow                      = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt workFlowDt                  = new Blue.BL.APP.WFDt();
        private Blue.BL.APP.Field field                      = new Blue.BL.APP.Field();        
       
        #region "Appearance"

        private string _title;
        [Category("Appearance")]
        [Description("Gets or set list page tile.")]
        [Browsable(true)]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        #endregion

        #region "Behavior"

        private bool _allowCreate = true;
        [Category("Behavior")]
        [Description("Allow user control to display create button")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool AllowCreate
        {
            get { return this._allowCreate; }
            set { this._allowCreate = value; }
        }

        [Category("Behavior")]
        [Description("Allow display work-flow status column on the list.")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool WorkFlowEnable
        {
            get 
            { 
                return workFlow.GetIsActive(Module, SubModule, LoginInfo.ConnStr); 
            }
        }

        private bool _allowPrint = true;
        [Category("Behavior")] 
        [Description("Allow user control to display print button")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool AllowPrint
        {
            get { return this._allowPrint; }
            set { this._allowPrint = value; }
        }

        #endregion
        
        #region "View-Behavior"

        private bool _allowViewCreate = true;
        [Category("View-Behavior")]
        [Description("Allow user control to enble view create button")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool AllowViewCreate
        {
            get { return this._allowViewCreate; }
            set { this._allowViewCreate = value; }
        }

        private string _listPageCuzURL;
        [Category("View-Behavior")]
        [Description("URL of list page customize")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [DefaultValue("")]
        [Bindable(true)]
        public string ListPageCuzURL
        {
            get { return this._listPageCuzURL; }
            set { this._listPageCuzURL = value; }
        }

        #endregion

        #region "List-Behavior"

        [Category("List-Behavior")]
        [Description("Gets GridView Object on ListPage UserControl")]
        [DefaultValue(false)]
        [Browsable(false)]
        public ASPxGridView ListObject
        {
            get { return grd_DataList; }
        }

        private bool _allowEdit;
        [Category("List-Behavior")]
        [Description("Allow user control to display Edit Button at the first column")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AllowEdit
        {
            get { return this._allowEdit; }
            set { this._allowEdit = value; }
        }

        private string _editButtonText;
        [Category("List-Behavior")]
        [Description("Text for display on the edit button")]
        [DefaultValue("Edit")]
        [Browsable(true)]
        public string EditButtonText
        {
            get { return this._editButtonText; }
            set { this._editButtonText = value; }
        }

        private GridViewEditingMode _editMode;
        [Category("List-Behavior")]
        [Description("Allow user control to display Edit Button at the first column")]
        [DefaultValue(GridViewEditingMode.Inline)]
        [Browsable(true)]
        public GridViewEditingMode EditMode
        {
            get 
            {
                return this._editMode;
            }
            set
            {
                this._editMode = value;
            }
        }

        private string _deleteButtonText;
        [Category("List-Behavior")]
        [Description("Text for display on the delete button")]
        [DefaultValue("Delete")]
        [Browsable(true)]
        public string DeleteButtonText
        {
            get { return this._deleteButtonText; }
            set { this._deleteButtonText = value; }
        }

        private bool _allowDelete;
        [Category("List-Behavior")]
        [Description("Allow user control to display Delete Button at the first column")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AllowDelete
        {
            get { return this._allowDelete; }
            set { this._allowDelete = value; }
        }

        private bool _allowVoid;
        [Category("List-Behavior")]
        [Description("Allow user control to display Void Button at the first column")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AllowVoid
        {
            get { return this._allowVoid; }
            set { this._allowVoid = value; }
        }

        private string _voidButtonText;
        [Category("List-Behavior")]
        [Description("Text for display on the void button")]
        [DefaultValue("Void")]
        [Browsable(true)]
        public string VoidButtonText
        {
            get { return this._voidButtonText; }
            set { this._voidButtonText = value; }
        }        

        private string _keyFieldName;
        [Category("List-Behavior")]
        [Description("Key field name of the list")]
        [DefaultValue("")]
        [Browsable(true)]
        public string KeyFieldName
        {
            get { return this._keyFieldName; }
            set { this._keyFieldName = value; }
        }

        private string _detailPageURL = string.Empty;
        [Category("List-Behavior")]
        [Description("URL of detail page")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [DefaultValue("")]
        [Bindable(true)]
        public string DetailPageURL
        {
            get { return this._detailPageURL; }
            set { this._detailPageURL = value; }
        }

        private string _editPageURL = string.Empty;
        [Category("List-Behavior")]
        [Description("URL of edit page")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [DefaultValue("")]
        [Bindable(true)]
        public string EditPageURL
        {
            get { return this._editPageURL; }
            set { this._editPageURL = value; }
        }

        #endregion
        
        #region "Misc"

        private string _pageCode;
        [Category("Misc")]
        [Description("Gets or set a group of view list")]        
        [Browsable(true)]
        public string PageCode
        {
            get { return this._pageCode; }
            set { this._pageCode = value; }
        }

        private string _module;        
        [Category("Misc")]
        [Description("Gets or set a module of view list")]
        [Browsable(true)]
        public string Module
        {
            get { return this._module; }
            set { this._module = value; }
        }

        private string _subModule;
        [Category("Misc")]
        [Description("Gets or set a sub module for get the work flow option")]
        [Browsable(true)]
        public string SubModule
        {
            get { return this._subModule; }
            set { this._subModule = value; }
        }

        

        #endregion

        /// <summary>
        /// Adds or Removes create options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem CreateItems
        {
            get { return this.menu_CmdBar.Items[0]; }
        }

        /// <summary>
        /// Add or Removes print options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem PrintItems
        {
            get { return this.menu_CmdBar.Items[3]; }
        }

        /// <summary>
        /// Close Button
        /// Display on PO list only
        /// </summary>
        //public DevExpress.Web.ASPxMenu.MenuItem CloseItem 
        //{
        //    get { return this.menu_CmdBar.Items[3]; }
        //}

        /// <summary>
        /// Gets WFId from ViewhandlerID
        /// </summary>
        private int WFId
        {
            get { return viewHandler.GetWFId(int.Parse(ddl_View.Value.ToString()), LoginInfo.ConnStr); }
        }

        /// <summary>
        /// Gets WFStep from ViewhandlerID
        /// </summary>
        private int WFStep
        {
            get { return viewHandler.GetWFStep(int.Parse(ddl_View.Value.ToString()), LoginInfo.ConnStr); }
        }

        /// <summary>
        /// 
        /// </summary>
        private DataTable dtViewCrtrAsking
        {
            get 
            {
                return (DataTable)Session["dtViewCrtrAsking"];
            }

            set             
            {
                Session["dtViewCrtrAsking"] = value;
            }
        }

        #endregion

        #region "Operations"
                
        /// <summary>
        /// Get and display list page data.
        /// </summary>
        public override void DataBind()
        {
            // Display title ------------------------------------------------------------------
            lbl_Title.Text = Title;            

            // Display WorkFlow List
            pnl_WFLegend.Visible = WorkFlowEnable;
            
            // Display all View Name
            ddl_View.DataSource = viewHandler.GetList(PageCode, LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_View.DataBind();           

            if (ddl_View.Items.Count > 0)
            {
                if (Request.Cookies[PageCode] != null)
                {
                    if (Request.Cookies[PageCode].Value != string.Empty)
                    {
                        if (ddl_View.Items.FindByValue(int.Parse(Request.Cookies[PageCode].Value.ToString())) != null)
                        {
                            //ddl_View.Text   = viewHandler.GetDesc(int.Parse(Request.Cookies[PageCode].Value.ToString()), LoginInfo.ConnStr);
                            ddl_View.Value = int.Parse(Request.Cookies[PageCode].Value.ToString());
                        }
                        else
                        {
                            ddl_View.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddl_View.SelectedIndex = 0;
                    }
                }
                else
                {
                    ddl_View.SelectedIndex = 0;
                }
            }
            else
            {
                btn_ViewGo.Enabled      = false;
                btn_ViewModify.Enabled  = false;
            }

            // Allow/Not Allow to create new view
            btn_ViewCreate.Enabled = this.AllowViewCreate;

            // Not allow to edit standard 
            if (ddl_View.Value != null)
            {
                btn_ViewModify.Enabled = !viewHandler.GetIsStandard(int.Parse(ddl_View.Value.ToString()), 
                    LoginInfo.ConnStr);
            }

            // Visible or Hide the command bar button. ----------------------------------------
            menu_CmdBar.Items[0].Visible = AllowCreate;

            if (AllowCreate)
            {
                if (EditPageURL != string.Empty)
                {
                    menu_CmdBar.Items[0].NavigateUrl = EditPageURL + "?MODE=new";                    
                }
            }

            menu_CmdBar.Items[0].Enabled    = workFlowDt.GetAllowCreate(WFId, WFStep, LoginInfo.ConnStr);
            menu_CmdBar.Items[1].Visible    = AllowDelete;
            menu_CmdBar.Items[1].Text       = DeleteButtonText;
            menu_CmdBar.Items[2].Visible    = AllowVoid;
            menu_CmdBar.Items[2].Text       = VoidButtonText;
            menu_CmdBar.Items[3].Visible    = AllowPrint;

            // Display data list
            this.RefreshList();
            
            base.DataBind();
            //this.DisplayFilter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_View_Load(object sender, EventArgs e)
        {
            ddl_View.DataSource = viewHandler.GetList(PageCode, LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_View.DataBind(); 
        }

        /// <summary>
        /// Refresh list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update cookies
            Response.Cookies[PageCode].Value    = ddl_View.Value.ToString();
            Response.Cookies[PageCode].Expires  = DateTime.MaxValue;

            menu_CmdBar.Items[0].Enabled = workFlowDt.GetAllowCreate(WFId, WFStep, LoginInfo.ConnStr);

            this.RefreshList();

            //this.DisplayFilter();
        }

        /// <summary>
        /// Refresh list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            this.RefreshList();
        }

        /// <summary>
        /// Redirect to modify view page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ViewModify_Click(object sender, EventArgs e)
        {
            Response.Redirect(ListPageCuzURL + "?VIEW_NO=" + ddl_View.SelectedItem.Value.ToString());
        }

        /// <summary>
        /// Redirect to create view page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ViewCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(ListPageCuzURL);
        }        

        /// <summary>
        /// Refresh list.
        /// </summary>
        protected void grd_DataList_OnLoad(object sender, EventArgs e)
        {
            //dsDataList = (DataSet)Session["dsDataList"];

            //if (dsDataList != null)
            //{
            //    grd_DataList.DataSource     = dsDataList.Tables[PageCode];
            //    grd_DataList.KeyFieldName   = KeyFieldName;

            //    if (this.DetailPageURL != string.Empty)
            //    {
            //        grd_DataList.ClientSideEvents.RowDblClick = "function(s, e) { OnGridDoubleClick(e.visibleIndex, '" + KeyFieldName + "'); }";
            //    }

            //    grd_DataList.DataBind();
            //}

            //if (IsPostBack && WorkFlowEnable && grd_DataList.Columns["ApprStatus"] != null)
            //{
            //    ((GridViewDataColumn)grd_DataList.Columns["ApprStatus"]).DataItemTemplate = new WorkFlowColumnTemplate();
            //}

            //if (IsPostBack && (this.AllowDelete || this.AllowVoid))
            //{
            //    grd_DataList.Columns[0].HeaderTemplate = new SelectColumnHeaderTemplate();
            //}            
        }

        /// <summary>
        /// Display list page data.
        /// </summary>
        private void RefreshList()
        {
            if (ddl_View.Value == null)
            {
                return;
            }

            // Clear all existing data in the list table.
            if (dsDataList != null)
            {
                if (dsDataList.Tables.Count > 0)
                {
                    if (dsDataList.Tables[PageCode] != null)
                    {
                        dsDataList.Tables[PageCode].Clear();
                    }
                }
            }

            // Get list data by view no
            bool result = false;

            if (LoginInfo.BuInfo.IsHQ)
            {
                result = viewHandler.GetDataListHQ(LoginInfo.BuInfo.BuGrpCode, dsDataList, int.Parse(ddl_View.Value.ToString()),
                    PageCode, KeyFieldName, LoginInfo.LoginName);
            }
            else
            {                
                result = viewHandler.GetDataList(dsDataList, int.Parse(ddl_View.Value.ToString()),
                         PageCode, KeyFieldName, "", LoginInfo.BuInfo.BuCode, LoginInfo.LoginName, LoginInfo.ConnStr);
            }

            if (result)
            {
                // Clear all exist columns in grid view
                grd_DataList.Columns.Clear();

                // Add Dynamic Data Column
                if (LoginInfo.BuInfo.IsHQ)
                {
                    for (int i = (WorkFlowEnable ? 3 : 2); i < dsDataList.Tables[PageCode].Columns.Count; i++)
                    {
                        GridViewDataColumn column = new GridViewDataColumn();
                        string columnName = dsDataList.Tables[PageCode].Columns[i].ColumnName;
                        column.FieldName = columnName;
                        column.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                        column.Caption = (LoginInfo.BuFmtInfo.IsDefaultLangCode ?
                            field.GetDesc(columnName, LoginInfo.ConnStr) : field.GetOthDesc(columnName, LoginInfo.ConnStr));
                        grd_DataList.Columns.Add(column);
                    }
                }
                else
                {                    
                    for (int i = (WorkFlowEnable ? 4 : 3); i < dsDataList.Tables[PageCode].Columns.Count; i++)
                    {
                        GridViewDataColumn column = new GridViewDataColumn();
                        string columnName = dsDataList.Tables[PageCode].Columns[i].ColumnName;
                        column.FieldName = columnName;
                        column.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                        column.Caption = (LoginInfo.BuFmtInfo.IsDefaultLangCode ?
                            field.GetDesc(columnName, LoginInfo.ConnStr) : field.GetOthDesc(columnName, LoginInfo.ConnStr));
                        grd_DataList.Columns.Add(column);
                    }
                }

                // Display WorkFlow column
                if (WorkFlowEnable && dsDataList.Tables[PageCode].Columns.Contains("ApprStatus"))
                {
                    GridViewDataTextColumn wfColumn     = new GridViewDataTextColumn();
                    wfColumn.DataItemTemplate           = new WorkFlowColumnTemplate();
                    wfColumn.FieldName                  = "ApprStatus";
                    wfColumn.Caption                    = "Process Status";
                    wfColumn.Settings.AllowSort         = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                    grd_DataList.Columns.Add(wfColumn);
                }
                
                grd_DataList.DataSource     = dsDataList.Tables[PageCode];
                grd_DataList.KeyFieldName   = KeyFieldName;

                if (this.DetailPageURL != string.Empty)
                {
                    grd_DataList.ClientSideEvents.RowDblClick = "function(s, e) { OnGridDoubleClick(e.visibleIndex, 'BuCode;" + KeyFieldName + "'); }";
                }

                // Group By BuCode
                if (LoginInfo.BuInfo.IsHQ)
                {
                    grd_DataList.GroupBy(grd_DataList.Columns["Buisness Unit Name"]);
                }

                grd_DataList.DataBind();
                grd_DataList.ExpandAll();

                Session["dsDataList"] = dsDataList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayFilter()
        {
            int viewNo = int.Parse(ddl_View.SelectedItem.Value.ToString());

            //Retrieve data
            bool result = viewHandlerCrtr.GetByIsAsking(dsDataList, viewNo ,LoginInfo.ConnStr);

            if (result)
            {
                //Send data to session.
                this.dtViewCrtrAsking = dsDataList.Tables[viewHandlerCrtr.TableName];

                foreach (DataRow drViewCrtr in this.dtViewCrtrAsking.Rows)
                {
                    HtmlTableCell tbCell1 = new HtmlTableCell();
                    Label lblDesc         = new Label();
                    lblDesc.Text          = drViewCrtr["Desc"].ToString();
                    tbCell1.Controls.Add(lblDesc);

                    HtmlTableCell tbCell2 = new HtmlTableCell();

                    string chkFieldType   = drViewCrtr["FieldTypeCode"].ToString();

                    switch (chkFieldType.ToUpper())
                    {
                        case "B": // Boolean    
                            ASPxCheckBox chkValue = new ASPxCheckBox();
                            chkValue.ID           = "chk" + drViewCrtr["FieldName"].ToString() + drViewCrtr["ViewCrtrNo"].ToString();
                            tbCell2.Controls.Add(chkValue);

                            break;

                        case "D": // DateTime
                            ASPxDateEdit dteValue = new ASPxDateEdit();
                            dteValue.ID           = "dte" + drViewCrtr["FieldName"].ToString() + drViewCrtr["ViewCrtrNo"].ToString();
                            tbCell2.Controls.Add(dteValue);

                            break;

                        case "L": // Lookup
                            
                            break;

                        case "N": // Numeric
                            ASPxSpinEdit spnValue = new ASPxSpinEdit();
                            spnValue.ID           = "spn" + drViewCrtr["FieldName"].ToString() + drViewCrtr["ViewCrtrNo"].ToString();
                            tbCell2.Controls.Add(spnValue);
                           
                            break;

                        case "S": // String
                            ASPxTextBox txtValue = new ASPxTextBox();
                            txtValue.ID          = "txt" + drViewCrtr["FieldName"].ToString() + drViewCrtr["ViewCrtrNo"].ToString();
                            tbCell2.Controls.Add(txtValue);
                            
                            break;
                    }

                    HtmlTableRow tbRow = new HtmlTableRow();
                    tbRow.Cells.Add(tbCell1);
                    tbRow.Cells.Add(tbCell2);

                    HtmlTable tb = new HtmlTable();
                    tb.Rows.Add(tbRow);

                    //pnl_Filter.Controls.Add(tb);
                }

                //pnl_Filter.Visible = true;
                //pnl_Button.Visible = true;
            }
            else
            {
                //pnl_Filter.Visible = false;
                //pnl_Button.Visible = false;
            }           
        }

        /// <summary>
        /// Click filter button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Filter_Click(object sender, EventArgs e)
        {
            foreach (DataRow drViewCrtr in this.dtViewCrtrAsking.Rows)
            { 
                string chkFieldType   = drViewCrtr["FieldTypeCode"].ToString();

                switch (chkFieldType.ToUpper())
                {
                    case "B": // Boolean    
                        //ASPxCheckBox chkValue = (ASPxCheckBox)pnl_Filter.FindControl("chk" + drViewCrtr["FieldName"].ToString() + drViewCrtr["ViewCrtrNo"].ToString());
                        
                        break;

                    case "D": // DateTime
                        //ASPxDateEdit dteValue = new ASPxDateEdit();
                        break;

                    case "L": // Lookup
                        
                        break;

                    case "N": // Numeric
                        //ASPxSpinEdit spnValue = new ASPxSpinEdit();
                       
                        break;

                    case "S": // String
                        //ASPxTextBox txtValue = new ASPxTextBox();
                        
                        break;
                }
            }
        }

        #endregion

        #region "ColumnTemplate"

        class WorkFlowColumnTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {   
                GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;

                for (int i = 0; i < gridContainer.Text.Length; i++)
                {
                    Image imgWF = new Image();

                    switch (gridContainer.Text.Substring(i, 1).ToUpper())
                    {
                        case "_": // Wait for approve
                            imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/NA.gif";
                            break;

                        case "A": // Approve
                            imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/APP.gif";
                            break;

                        case "R": // Reject
                            imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/REJ.gif";
                            break;

                        case "P": // Partial Approve
                            imgWF.ImageUrl = "~/App_Themes/Default/Images/WF/PAR.gif";
                            break;
                    }

                    container.Controls.Add(imgWF);
                }
            }
        }

        class SelectColumnHeaderTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                HtmlInputCheckBox checkBox = new HtmlInputCheckBox();
                checkBox.Attributes.Add("onclick", "listPageGrid.SelectAllRowsOnPage(this.checked)");
                container.Controls.Add(checkBox);                
            }
        }

        #endregion    
    }
}