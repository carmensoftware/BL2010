using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.UserControls.ViewHandler
{
    public partial class ListPageLookup : BaseUserControl
    {
        #region "Attributes"

        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr = new Blue.BL.APP.ViewHandlerCrtr();
        private Blue.BL.APP.ViewHandlerCols viewHandlerCols = new Blue.BL.APP.ViewHandlerCols();
        private Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private Blue.BL.APP.Field field = new Blue.BL.APP.Field();
        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private DataSet dsActiveBU = new DataSet();
        private DataSet dsEachBUTrans = new DataSet();
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        private Blue.BL.APP.WF wf = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();

        private DataTable[] dtTransection;
        //private DataTable[] dtTranFull;
        private DataSet dsForSearch = new DataSet();
        private List<int> ExpandItem = new List<int>();

        private int prevPageIndex = 0;
        private string filterText = string.Empty;
        private bool enableMultipleBU = true;


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

        private bool _allowClosePO = false;
        [Category("Behavior")]
        [Description("Allow user control to display close PO button")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AllowClosePO
        {
            get { return this._allowClosePO; }
            set { this._allowClosePO = value; }
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

        //[Category("List-Behavior")]
        //[Description("Gets GridView Object on ListPage UserControl")]
        //[DefaultValue(false)]
        //[Browsable(false)]
        //public ASPxGridView ListObject
        //{
        //    get { return grd_DataList; }
        //}

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

        //OP Create VID
        public string VID { get; set; }
        ////

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
            get { return this.menu_CmdBar.Items[4]; }
        }

        /// <summary>
        /// Gets WFId from ViewhandlerID
        /// </summary>
        private int WFId
        {
            get { return viewHandler.GetWFId(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr); }
        }

        /// <summary>
        /// Gets WFStep from ViewhandlerID
        /// </summary>
        private int WFStep
        {
            get { return viewHandler.GetWFStep(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr); }
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

        private SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }
        }

        public DataTable dtBuKeys
        {
            get
            {
                string strFilter = string.Empty;
                string strSearch = txt_FullTextSearch.Text.Trim();

                //DataSet dsFind = new DataSet();
                DataTable[] dtFind;

                //--Create datatable with 'BUCode' & 'No.' column. 
                DataTable dtReport = new DataTable();
                DataColumn cl = new DataColumn("BUCode");
                dtReport.Columns.Add(cl);

                cl = new DataColumn("No");
                dtReport.Columns.Add(cl);



                if (Session["dtTransection"] != null)
                {
                    dtFind = (DataTable[])Session["dtTransection"];


                    for (int i = 0; i < dtFind.Count(); i++)
                    {
                        if (dtFind[i] == null) continue;
                        if (dtFind[i].Rows.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int jj = 0; jj < dtFind[i].Rows.Count; jj++)
                            {
                                sb.Append("'" + dtFind[i].Rows[jj][0] + "',");
                            }

                            if (sb.Length > 0)
                            {
                                DataRow dr = dtReport.NewRow();
                                dr[0] = dtFind[i].ToString();
                                dr[1] = sb.ToString().Substring(0, sb.Length - 1);
                                dtReport.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                }

                return dtReport;
            }
        }

        public string ListKeys
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        #region "Operations"

        public void setPage(int pageIndex)
        {
            prevPageIndex = pageIndex;
        }

        public void setFilter(string value)
        {
            filterText = value;
        }

        public void setViewMultipleBU(bool value)
        {
            enableMultipleBU = value;
        }

        public override void DataBind()
        {
            base.DataBind();

            this.Binding_ddl_View2();
            this.Page_Retrieve();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.Page_Retrieve();
                if (dsActiveBU.Tables[bu.TableName] != null)
                {
                    int test = prevPageIndex;
                    grd_BU.DataSource = dsActiveBU.Tables[bu.TableName];
                }
            }
            else
            {
                dsActiveBU = (DataSet)Session["dsActiveBU"];
                dtTransection = (DataTable[])Session["dtTransection"];

                if (ViewState["ExpandItem"] != null)
                {
                    this.ExpandItem = (List<int>)ViewState["ExpandItem"];
                }

                this.Page_Setting();
            }
        }

        private void Page_Retrieve()
        {
            bool getActiveBU = false;

            // Clear all existing data.
            dsActiveBU.Clear();

            //If logged in BU is HQ the get all active bu data else get only the selected BU.
            if (LoginInfo.BuInfo.IsHQ && enableMultipleBU)
            {
                enableMultipleBU = true;  // Reset to default
                getActiveBU = bu.GetList(dsActiveBU, LoginInfo.BuInfo.BuGrpCode);
            }
            else
            {
                getActiveBU = bu.Get(dsActiveBU, LoginInfo.BuInfo.BuCode);
            }

            if (getActiveBU)
            {
                dtTransection = new DataTable[dsActiveBU.Tables[bu.TableName].Rows.Count];

                Session["dsActiveBU"] = dsActiveBU;
                Session["dtTransection"] = dtTransection;


                this.Page_Setting();
            }
        }

        private void Page_Setting()
        {
            // Page title setting -----------------------------------------------------------------
            lbl_Title.Text = this.Title;

            // View handler setting ---------------------------------------------------------------            
            btn_ViewCreate2.Enabled = this.AllowViewCreate; // Allow/Not Allow to create new view            

            // Not allow to edit standard 
            if (ddl_View2.SelectedItem != null && ddl_View2.SelectedItem.Value != string.Empty)
            {
                btn_ViewModify2.Enabled = !viewHandler.GetIsStandard(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr);
            }

            // Display transaction detail ---------------------------------------------------------
            grd_BU.DataSource = dsActiveBU.Tables[bu.TableName];
            grd_BU.DataBind();

            // Always hide Expand/Collapse column if selected bu is not HQ
            grd_BU.Columns[0].Visible = LoginInfo.BuInfo.IsHQ;

            // Menu setting -----------------------------------------------------------------------
            // Create
            menu_CmdBar.Items[0].Visible = this.AllowCreate;

            if (this.AllowCreate)
            {
                if (this.EditPageURL != string.Empty)
                {
                    menu_CmdBar.Items[0].NavigateUrl = this.EditPageURL + "?MODE=new";
                }
            }

            if (WorkFlowEnable)
            {
                menu_CmdBar.Items[0].Visible = workFlowDt.GetAllowCreate(WFId, WFStep, LoginInfo.ConnStr);

                //if (WFId == 0)
                //{
                //    menu_CmdBar.Items[0].Enabled = true;
                //}
            }

            menu_CmdBar.Items[1].Visible = this.AllowDelete;
            menu_CmdBar.Items[2].Visible = this.AllowVoid;
            menu_CmdBar.Items[3].Visible = this.AllowClosePO;
            menu_CmdBar.Items[4].Visible = this.AllowPrint;

        }

        private void Binding_ddl_View2()
        {
            ddl_View2.DataSource = viewHandler.GetList(PageCode, LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_View2.DataBind();

            if (ddl_View2.Items.Count == 0)
            {
                btn_ViewGo2.Enabled = false;
                btn_ViewModify2.Enabled = false;
                return;
            }

            if (Request.Cookies[PageCode] != null)
            {
                if (Request.Cookies[PageCode].Value != string.Empty)
                {
                    ddl_View2.SelectedValue = Request.Cookies[PageCode].Value;
                }
                else
                {
                    Response.Cookies[PageCode].Value = ddl_View2.SelectedItem.Value;
                    Response.Cookies[PageCode].Expires = DateTime.MaxValue;
                }
            }
            else
            {
                Response.Cookies.Add(new HttpCookie(PageCode));
                Response.Cookies[PageCode].Value = ddl_View2.SelectedItem.Value;
                Response.Cookies[PageCode].Expires = DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Refresh the data list. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_View2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Cookies[PageCode].Value = ddl_View2.SelectedItem.Value;
            Response.Cookies[PageCode].Expires = DateTime.MaxValue;

            txt_FullTextSearch.Text = string.Empty;

            VID = ddl_View2.SelectedItem.Value;

            this.Page_Retrieve();
        }

        protected void btn_ViewGo2_Click(object sender, EventArgs e)
        {
            txt_FullTextSearch.Text = string.Empty;

            this.Page_Retrieve();
        }

        protected void btn_ViewModify2_Click(object sender, EventArgs e)
        {
            if (ListPageCuzURL == null)
            {
                return;
            }

            if (ListPageCuzURL == string.Empty)
            {
                return;
            }

            Response.Redirect(ListPageCuzURL + "?VIEW_NO=" + ddl_View2.SelectedItem.Value.ToString());
        }

        protected void btn_ViewCreate2_Click(object sender, EventArgs e)
        {
            if (ListPageCuzURL == null)
            {
                return;
            }

            if (ListPageCuzURL == string.Empty)
            {
                return;
            }

            Response.Redirect(ListPageCuzURL);
        }

        /// <summary>
        /// Display All BU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_BU_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_BU") != null)
                {
                    Label lbl_BU = e.Row.FindControl("lbl_BU") as Label;
                    lbl_BU.Text = DataBinder.Eval(e.Row.DataItem, "BuName").ToString();
                }

                if (e.Row.FindControl("hf_BuCode") != null)
                {
                    HiddenField hf_BuCode = e.Row.FindControl("hf_BuCode") as HiddenField;
                    hf_BuCode.Value = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();
                }

                // 2012-03-06: Receive issue want to show all property.
                if (e.Row.FindControl("btn_Expand") != null)
                {
                    ImageButton btn_Expand = e.Row.FindControl("btn_Expand") as ImageButton;
                    btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";
                }

                if (e.Row.FindControl("grd_Trans") != null)
                {
                    GridView grd_Trans = e.Row.FindControl("grd_Trans") as GridView;
                    //DataTable[] dtTransection = (DataTable[])Session["dtTransection"];

                    // Clear old data
                    //ViewState["dsEachBuTrans"] = string.Empty;
                    dsEachBUTrans.Clear();

                    string BUCodeHQ = string.Empty;

                    if (LoginInfo.BuInfo.IsHQ)
                    {
                        BUCodeHQ = LoginInfo.BuInfo.BuCode;
                    }

                    // Get transaction detail data
                    bool result = false;
                    bool ChkK = false;

                    if (ddl_View2.SelectedItem != null)
                    {
                        result = viewHandler.GetDataList(dsEachBUTrans, int.Parse(ddl_View2.SelectedItem.Value),
                            PageCode, KeyFieldName, filterText, DataBinder.Eval(e.Row.DataItem, "BuCode").ToString(),
                            LoginInfo.LoginName,
                            bu.GetConnectionString(LoginInfo.BuInfo.BuCode));

                        ChkK = viewHandler.GetDataListK(dsForSearch, int.Parse(ddl_View2.SelectedItem.Value),
                            PageCode, KeyFieldName, DataBinder.Eval(e.Row.DataItem, "BuCode").ToString(),
                            LoginInfo.LoginName,
                            bu.GetConnectionString(LoginInfo.BuInfo.BuCode));
                    }

                    if (result)
                    {

                        // Hide this row if no transaction data
                        if (dsEachBUTrans.Tables[PageCode].Rows.Count == 0)
                        {
                            e.Row.Visible = false;
                        }

                        dtTransection[e.Row.DataItemIndex] = dsForSearch.Tables[e.Row.DataItemIndex];

                        // Modified by Ake (2014-03-05)
                        //Session["dtTransection"] = dtTransection;

                        //GridViewRow selectedRow = grd_Trans.Parent.Parent as GridViewRow;
                        dtTransection[e.Row.DataItemIndex] = dsEachBUTrans.Tables[PageCode];
                        Session["dtTransection"] = dtTransection;

                        //---------------------------------------------------------------------

                        //ViewState["dsEachBuTrans"] = dsEachBUTrans;

                        // Find column index of start adding column
                        int startColumnIndex = 0;

                        if (viewHandler.GetIsWFEnable(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr))
                        {
                            startColumnIndex = 4;
                        }
                        else
                        {
                            startColumnIndex = 3;
                        }

                        // Add Columns
                        for (int i = startColumnIndex; i < dsEachBUTrans.Tables[PageCode].Columns.Count; i++)
                        {
                            BoundField column = new BoundField();
                            string columnName = dsEachBUTrans.Tables[PageCode].Columns[i].ColumnName;
                            column.HeaderText = (LoginInfo.BuFmtInfo.IsDefaultLangCode ?
                                                            field.GetDesc(columnName, LoginInfo.ConnStr) : field.GetOthDesc(columnName, LoginInfo.ConnStr));
                            column.DataField = columnName;
                            column.SortExpression = columnName;
                            column.HeaderStyle.Width = Unit.Percentage(viewHandlerCols.GetColumnWidth(int.Parse(ddl_View2.SelectedItem.Value), columnName, LoginInfo.ConnStr));

                            // Set column style
                            switch (dsEachBUTrans.Tables[PageCode].Columns[i].DataType.ToString())
                            {
                                case "System.Int16":
                                case "System.Int32":
                                case "System.Int64":
                                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                                    column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                    column.DataFormatString = "{0:D}";
                                    break;

                                case "System.Decimal":
                                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                                    column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                    column.DataFormatString = "{0:N}";
                                    break;

                                case "System.DateTime":
                                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                    //column.HeaderStyle.Width            = 10%;
                                    column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                    column.DataFormatString = "{0:dd/MM/yyyy}";

                                    break;

                                default:
                                    column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                    column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                    //column.ItemStyle.AddAttributesToRender

                                    //    e.Row.Cells[1].Attributes.Add("style", "word-wrap:break-word;width:50%");

                                    break;
                            }

                            grd_Trans.Columns.Add(column);
                        }

                        // Add "Process Status" column if this view is related to workflow or thereis column ApprStatus
                        if (WorkFlowEnable && dsEachBUTrans.Tables[PageCode].Columns.Contains("ApprStatus"))
                        {
                            BoundField wfColumn = new BoundField();
                            wfColumn.HeaderText = "Process Status";
                            wfColumn.DataField = "ApprStatus";
                            wfColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                            wfColumn.HeaderStyle.Width = Unit.Pixel(100);
                            wfColumn.ItemStyle.HorizontalAlign = wfColumn.HeaderStyle.HorizontalAlign;
                            grd_Trans.Columns.Add(wfColumn);
                        }

                        // Display transaction detail data    
                        //DataTable dtTemp = dsEachBUTrans.Tables[PageCode].AsEnumerable().Take(grd_Trans.PageSize).CopyToDataTable();
                        //grd_Trans.DataSource = dtTemp;
                        //Repeater rptPager =  e.Row.FindControl("rptPager") as Repeater;
                        //PopulatePager(rptPager, dsEachBUTrans.Tables[PageCode].Rows.Count, 1);
                        //Session["GridViewRow"] = e.Row;

                        Label lbl_Message = (Label)e.Row.FindControl("lbl_Message");
                        lbl_Message.Visible = false;

                        if (PageCode.ToLower().Contains("vendor")) // PageCode.ToLower().Contains("productlist") || 
                            if (!IsPostBack && dsEachBUTrans.Tables[PageCode].Rows.Count > 1000)
                            {
                                grd_Trans.DataSource = (from row in dsEachBUTrans.Tables[PageCode].AsEnumerable()
                                                        orderby row.Field<string>(0) //descending
                                                        select row).AsEnumerable().Take(grd_Trans.PageSize).CopyToDataTable();
                                lbl_Message.Visible = true;
                            }
                            else
                                grd_Trans.DataSource = (from row in dsEachBUTrans.Tables[PageCode].AsEnumerable()
                                                        orderby row.Field<string>(0) //descending
                                                        select row).CopyToDataTable();
                        else
                        {

                            grd_Trans.DataSource = dsEachBUTrans.Tables[PageCode];
                        }


                        //grd_Trans.Visible    = !LoginInfo.BuInfo.IsHQ; // Always show trasaction detail if selected bu is not HQ
                        //grd_Trans.Visible = true;

                        foreach (int number in this.ExpandItem)
                        {
                            if (e.Row.RowIndex == number)
                            {
                                grd_Trans.Visible = false;

                                ImageButton btn_Expand = e.Row.FindControl("btn_Expand") as ImageButton;
                                btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                            }
                        }

                        grd_Trans.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
                        //grd_Trans.DataBind();

                        // Modified by Ake (2014-03-04)
                        if (prevPageIndex >= 0)
                        {
                            grd_Trans.PageIndex = prevPageIndex;
                            prevPageIndex = 0;
                        }

                        grd_Trans.DataBind();

                    }
                }
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            this.GetCustomersPageWise(pageIndex);

        }

        private void GetCustomersPageWise(int pageIndex)
        {
            GridViewRow grid = Session["GridViewRow"] as GridViewRow;

            GridView grd_Trans = grid.FindControl("grd_Trans") as GridView;
            Repeater rptPager = grid.FindControl("rptPager") as Repeater;

            DataTable dtTemp = dsEachBUTrans.Tables[PageCode].AsEnumerable()
                .Skip(pageIndex * grd_Trans.PageSize).Take(grd_Trans.PageSize).CopyToDataTable();
            grd_Trans.DataSource = dtTemp;
            PopulatePager(rptPager, dsEachBUTrans.Tables[PageCode].Rows.Count, pageIndex);
        }

        private void PopulatePager(Repeater rptPager, int recordCount, int currentPage)
        {
            int pageSize = 25;
            double dblPageCount = (double)((decimal)recordCount / pageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                pages.Add(new ListItem("First", "1", currentPage > 1));
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        /// <summary>
        /// Expand/Collapse the transaction detail of selected BU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Expand_Click(object sender, ImageClickEventArgs e)
        {
            // Change Expand/Collapse Image
            ImageButton btn_Expand = sender as ImageButton;
            GridViewRow selectedRow = btn_Expand.Parent.Parent as GridViewRow;
            GridView grd_Trans = selectedRow.FindControl("grd_Trans") as GridView;

            // If grid view of transaction detail was display, hide it and change the image to expand
            // otherwise display it and change image to collapse.
            if (grd_Trans.Visible)
            {
                grd_Trans.Visible = false;
                btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                this.ExpandItem.Add(selectedRow.RowIndex);
            }
            else
            {
                grd_Trans.Visible = true;
                btn_Expand.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";
                this.ExpandItem.Remove(selectedRow.RowIndex);
            }

            ViewState["ExpandItem"] = this.ExpandItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Trans_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView grd_Trans = sender as GridView;
                int sortColumnIndex = GetSortColumnIndex(grd_Trans);

                if (sortColumnIndex != -1)
                {
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "OnGridRowMouseOver(this)");
                e.Row.Attributes.Add("onmouseout", "OnGridRowMouseOut(this)");
            }
        }


        /// <summary>
        /// Display Process Status column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Trans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grd_Trans = sender as GridView;

                // Modified by Ake (2014-03-04)
                string urlFilterText = filterText.Replace("'", "~");
                //urlFilterText = string.Empty; // Disable temporary for solving problem filter not work

                e.Row.Attributes.Add("onclick", "OnGridRowClick('" + DataBinder.Eval(e.Row.DataItem, "BuCode").ToString() +
                    "', '" + DataBinder.Eval(e.Row.DataItem, KeyFieldName).ToString() +
                    "', '" + ddl_View2.SelectedItem.Value + "&Page=" + grd_Trans.PageIndex.ToString() + "&Filter=" + urlFilterText + "')");


                BoundField apprStatus = new BoundField();
                apprStatus.DataField = "ApprStatus";



                if (WorkFlowEnable && grd_Trans.Columns[grd_Trans.Columns.Count - 1].HeaderText.ToUpper() == "PROCESS STATUS")
                {
                    string newStatus = string.Empty;

                    foreach (char eachStep in e.Row.Cells[grd_Trans.Columns.Count - 1].Text)
                    {
                        switch (eachStep)
                        {
                            case 'A': // Approve
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/APP.gif\" style=\"width: 8px; height: 16px\" />";
                                break;

                            case '_': // In process
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/NA.gif\" style=\"width: 8px; height: 16px\" />";
                                break;

                            case 'P': // Partial
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/PAR.gif\" style=\"width: 8px; height: 16px\" />";
                                break;

                            case 'R': // Reject
                                newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/REJ.gif\" style=\"width: 8px; height: 16px\" />";
                                break;
                        }
                    }

                    e.Row.Cells[grd_Trans.Columns.Count - 1].Text = newStatus;
                }
            }
        }

        /// <summary>
        /// Display transaction data of selected page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Trans_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsTemp = new DataSet();
            dtTransection = (DataTable[])Session["dtTransection"];

            // Instance transaction detail GridView Object to display data.

            GridView grd_Trans = sender as GridView;
            GridViewRow selectedRow = grd_Trans.Parent.Parent as GridViewRow;


            DataView dv = this.SearchData(dtTransection[selectedRow.RowIndex]);

            if (ViewState["SortExpression"] != null)
            {
                dv.Sort = ViewState["SortExpression"].ToString() + (GridViewSortDirection.ToString().ToUpper() == "DESCENDING" ? DESCENDING : ASCENDING);
            }


            grd_Trans.DataSource = dv;
            grd_Trans.PageIndex = e.NewPageIndex;
            grd_Trans.DataBind();

        }

        public void setPageIndex(int Index)
        {
            if (Session["dtTransection"] != null)
            {
                dtTransection = (DataTable[])Session["dtTransection"];


                for (var i = 0; i <= dtTransection.Count() - 1; i++)
                {
                    var grd_Trans = grd_BU.Rows[i].FindControl("grd_Trans") as GridView;

                    if (grd_Trans != null)
                    {
                        grd_Trans.DataSource = dtTransection[i];
                        grd_Trans.PageIndex = Index;
                        grd_Trans.DataBind();

                        break;
                    }

                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Trans_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grd_Trans = sender as GridView;
            string sortExpression = e.SortExpression;
            ViewState["SortExpression"] = sortExpression;

            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(grd_Trans, sortExpression, DESCENDING);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(grd_Trans, sortExpression, ASCENDING);
            }
        }

        private void SortGridView(GridView grd_Trans, string sortExpression, string direction)
        {
            DataSet dsTemp = new DataSet();
            dtTransection = (DataTable[])Session["dtTransection"];

            // Get transaction data.
            GridViewRow parentRow = grd_Trans.Parent.Parent as GridViewRow;
            HiddenField hf_BuCode = parentRow.FindControl("hf_BuCode") as HiddenField;

            DataView dv = new DataView(dtTransection[parentRow.RowIndex]);
            dv.Sort = sortExpression + direction;
            grd_Trans.DataSource = dv;
            grd_Trans.DataBind();
            //}
        }

        private int GetSortColumnIndex(GridView grd_Trans)
        {
            foreach (DataControlField field in grd_Trans.Columns)
            {
                if (field.SortExpression == (string)ViewState["SortExpression"])
                {
                    return grd_Trans.Columns.IndexOf(field);
                }
            }

            return -1;
        }

        private void AddSortImage(int columnIndex, GridViewRow headerRow)
        {
            Image sortImage = new Image();

            if (GridViewSortDirection == SortDirection.Ascending)
            {
                sortImage.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/up.gif";
                sortImage.AlternateText = "Ascending Order";
            }
            else
            {
                sortImage.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/down.gif";
                sortImage.AlternateText = "Descending Order";
            }

            headerRow.Cells[columnIndex].Controls.Add(sortImage);
        }

        private DataView SearchData(DataTable dtSearch)
        {
            string strFilter = string.Empty;
            string strSearch = txt_FullTextSearch.Text.Trim();

            DataView dvTable = dtSearch.DefaultView;

            if (strSearch.ToString() != string.Empty)
            {
                if (dvTable.Table.Columns.Count > 0)
                {
                    for (int x = 0; x < dvTable.Table.Columns.Count; x++)
                    {
                        if (x == 0)
                        {
                            switch (dvTable.Table.Columns[x].DataType.Name.ToString())
                            {
                                case "DateTime":

                                    DateTime dt;

                                    if (DateTime.TryParse(strSearch, out dt))
                                    {
                                        //DateTime dt = System.Convert.ToDateTime(strSearch);                                            
                                        strFilter += "[" + dvTable.Table.Columns[x].ToString() + "] = '" + dt.ToShortDateString() + "'";
                                    }
                                    break;

                                case "Decimal":

                                    Decimal decChk;

                                    if (Decimal.TryParse(strSearch, out decChk))
                                    {
                                        strFilter += "[" + dvTable.Table.Columns[x].ToString() + "] = '" + decChk + "'";
                                    }
                                    break;

                                case "Int32":

                                    Int32 intChk;

                                    if (Int32.TryParse(strSearch, out intChk))
                                    {
                                        strFilter += "[" + dvTable.Table.Columns[x].ToString() + "] = '" + intChk + "'";
                                    }

                                    break;

                                case "Boolean":

                                    //Boolean booChk;

                                    //if (Boolean.TryParse(strSearch, out booChk))
                                    //{
                                    //    strFilter += "[" + dvTable.Table.Columns[x].ToString() + "] = '" + booChk + "'";
                                    //} 

                                    break;

                                case "Byte":

                                    break;

                                default:
                                    strFilter = "[" + dvTable.Table.Columns[x].ToString() + "] LIKE '%" + strSearch + "%'";

                                    break;
                            }
                        }
                        else
                        {
                            switch (dvTable.Table.Columns[x].DataType.Name.ToString())
                            {
                                case "DateTime":

                                    DateTime dt;

                                    if (DateTime.TryParse(strSearch, out dt))
                                    {
                                        //DateTime dt   = System.Convert.ToDateTime(strSearch);
                                        strFilter += " or [" + dvTable.Table.Columns[x].ToString() + "] = '" + dt.ToShortDateString() + "'";
                                    }
                                    //DateTime dt   = System.Convert.ToDateTime(strSearch);
                                    //strFilter    += "or [" + dvTable.Table.Columns[x].ToString() + "] = '"+ dt.ToShortDateString() +"'";

                                    break;

                                case "Decimal":

                                    Decimal decChk;

                                    if (Decimal.TryParse(strSearch, out decChk))
                                    {
                                        strFilter += "or [" + dvTable.Table.Columns[x].ToString() + "] = '" + decChk + "'";
                                    }
                                    break;

                                case "Int32":

                                    Int32 intChk;

                                    if (Int32.TryParse(strSearch, out intChk))
                                    {
                                        strFilter += " or [" + dvTable.Table.Columns[x].ToString() + "] = '" + intChk + "'";
                                    }

                                    break;

                                case "Boolean":

                                    //Boolean booChk;

                                    //if (Boolean.TryParse(strSearch, out booChk))
                                    //{
                                    //    strFilter += "[" + dvTable.Table.Columns[x].ToString() + "] = '" + booChk + "'";
                                    //}

                                    break;

                                case "Byte":

                                    break;

                                default:

                                    if (strFilter.ToString() != String.Empty)
                                    {
                                        strFilter += " or [" + dvTable.Table.Columns[x].ToString() + "] LIKE '%" + strSearch + "%'";
                                    }
                                    else
                                    {
                                        strFilter += " [" + dvTable.Table.Columns[x].ToString() + "] LIKE '%" + strSearch + "%'";
                                    }


                                    break;
                            }
                        }
                    }
                }

                dvTable.RowFilter = strFilter;
            }

            return dvTable;
        }

        private void Search()
        {
            //19/03/2012 set ViewState = false
            // Page title setting -----------------------------------------------------------------
            lbl_Title.Text = this.Title;

            // View handler setting ---------------------------------------------------------------            
            btn_ViewCreate2.Enabled = this.AllowViewCreate; // Allow/Not Allow to create new view            

            // Not allow to edit standard 
            if (ddl_View2.SelectedItem.Value != string.Empty)
            {
                btn_ViewModify2.Enabled = !viewHandler.GetIsStandard(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr);
            }

            dtTransection = (DataTable[])Session["dtTransection"];
            var strSearch = txt_FullTextSearch.Text.Trim();

            if (dtTransection.Any())
            {
                for (var i = 0; i <= dtTransection.Count() - 1; i++)
                {
                    var grdTrans = grd_BU.Rows[i].FindControl("grd_Trans") as GridView;

                    if (string.IsNullOrEmpty(strSearch.ToString()) || dtTransection[i] == null || grdTrans == null)
                        continue;
                    var dvTable = this.SearchData(dtTransection[i]);
                    dtTransection[i] = dvTable.ToTable();

                    grdTrans.DataSource = dvTable;
                    grdTrans.DataBind();


                }
            }

            Session["dtTransection"] = dtTransection;
        }

        protected void btn_Search_Click(object sender, ImageClickEventArgs e)
        {
            this.Search();
        }

        protected void btn_SearchAdvance_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void txt_FullTextSearch_TextChanged(object sender, EventArgs e)
        {
            this.Search();
        }

        protected void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
