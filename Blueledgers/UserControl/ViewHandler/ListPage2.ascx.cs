using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.UserControls.ViewHandler
{
    public partial class ListPage2 : BaseUserControl
    {
        #region "Attributes"

        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr = new Blue.BL.APP.ViewHandlerCrtr();
        private Blue.BL.APP.ViewHandlerCols viewHandlerCols = new Blue.BL.APP.ViewHandlerCols();
        private Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private Blue.BL.APP.Field field = new Blue.BL.APP.Field();
        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();

        private DataSet dsActiveBU = new DataSet();
        private DataSet dsEachBUTrans = new DataSet();
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        private Blue.BL.APP.WF wf = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();

        private DataTable[] dtTransaction;
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


        private bool _allowExport = false;
        [Category("Behavior")]
        [Description("Allow user control to display Export button")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AllowExport
        {
            get { return this._allowExport; }
            set { this._allowExport = value; }
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
        /// Adds or Removes Menu Header
        /// Added on: 15/09/2017
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItemCollection menuItems
        {
            get { return this.menu_CmdBar.Items; }
        }

        /// <summary>
        /// Adds or Removes create options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem CreateItems
        {
            //get { return this.menu_CmdBar.Items[0]; }
            get { return this.menu_CmdBar.Items.FindByName("Create"); }
        }

        /// <summary>
        /// Add or Removes print options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem PrintItems
        {
            //get { return this.menu_CmdBar.Items[4]; }
            get { return this.menu_CmdBar.Items.FindByName("Print"); }
        }

        /// <summary>
        /// Add or Removes print options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem ClosePOItems
        {
            //get { return this.menu_CmdBar.Items[3]; }
            get { return this.menu_CmdBar.Items.FindByName("ClosePO"); }
        }

        /// <summary>
        /// Add or Removes print options.
        /// </summary>
        public DevExpress.Web.ASPxMenu.MenuItem ExportItems
        {
            get { return this.menu_CmdBar.Items.FindByName("Export"); }
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



                if (Session["dtTransaction"] != null)
                {
                    dtFind = (DataTable[])Session["dtTransaction"];


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
                if (dsActiveBU.Tables[bu.TableName] != null)
                {
                    //int test = prevPageIndex;
                    grd_BU.DataSource = dsActiveBU.Tables[bu.TableName];
                }
            }
            else
            {
                dsActiveBU = (DataSet)Session["dsActiveBU"];
                dtTransaction = (DataTable[])Session["dtTransaction"];

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
                string msgError = string.Empty;
                dsActiveBU.Reset();
                getActiveBU = buUser.GetList(dsActiveBU, LoginInfo.LoginName, ref msgError);
                if (dsActiveBU.Tables.Contains(buUser.TableName))
                    dsActiveBU.Tables["BUUser"].TableName = bu.TableName;
            }
            else
            {
                getActiveBU = bu.Get(dsActiveBU, LoginInfo.BuInfo.BuCode);
            }

            if (getActiveBU)
            {
                dtTransaction = new DataTable[dsActiveBU.Tables[bu.TableName].Rows.Count];

                Session["dsActiveBU"] = dsActiveBU;
                Session["dtTransaction"] = dtTransaction;


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
            DataView dv = dsActiveBU.Tables[bu.TableName].DefaultView;
            dv.Sort = "IsHQ DESC";
            DataTable sortedDT = dv.ToTable();

            //grd_BU.DataSource = dsActiveBU.Tables[bu.TableName];
            grd_BU.DataSource = sortedDT;
            grd_BU.DataBind();

            // Always hide Expand/Collapse column if selected bu is not HQ
            grd_BU.Columns[0].Visible = LoginInfo.BuInfo.IsHQ;

            // Menu setting -----------------------------------------------------------------------
            // Create
            menu_CmdBar.Items.FindByName("Create").Visible = this.AllowCreate;

            if (this.AllowCreate)
            {
                if (this.EditPageURL != string.Empty)
                {
                    menu_CmdBar.Items.FindByName("Create").NavigateUrl = this.EditPageURL + "?MODE=new";
                }

            }

            if (WorkFlowEnable)
            {
                menu_CmdBar.Items.FindByName("Create").Visible = workFlowDt.GetAllowCreate(WFId, WFStep, LoginInfo.ConnStr);
            }

            menu_CmdBar.Items.FindByName("Delete").Visible = this.AllowDelete;
            menu_CmdBar.Items.FindByName("Void").Visible = this.AllowVoid;
            menu_CmdBar.Items.FindByName("ClosePO").Visible = this.AllowClosePO;
            menu_CmdBar.Items.FindByName("Export").Visible = this.AllowExport;
            menu_CmdBar.Items.FindByName("Print").Visible = this.AllowPrint;

            // WorkFlow information setting -------------------------------------------------------
            pnl_WFLegend.Visible = WorkFlowEnable;

            if (pnl_WFLegend.Visible)
            {
                dl_ProcessStatus.DataSource = wfDt.GetList(wf.GetWFId(this.Module, this.SubModule, LoginInfo.ConnStr), LoginInfo.ConnStr);
                dl_ProcessStatus.DataBind();
            }
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
            //txt_FullTextSearch.Text = string.Empty;

            //this.Page_Retrieve();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
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
                //int rowIndex = e.Row.RowIndex;
                string buCode = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();

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

                    string BUCodeHQ = string.Empty;

                    if (LoginInfo.BuInfo.IsHQ)
                    {
                        BUCodeHQ = LoginInfo.BuInfo.BuCode;
                    }

                    // Get transaction detail data

                    if (ddl_View2.SelectedItem != null)
                    {
                        dsEachBUTrans.Clear();

                        bool getData = viewHandler.GetDataList(dsEachBUTrans, int.Parse(ddl_View2.SelectedItem.Value), PageCode, KeyFieldName, filterText, buCode, LoginInfo.LoginName, bu.GetConnectionString(buCode));

                        if (getData)
                        {
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
                                //column.HeaderStyle.Width = Unit.Percentage(viewHandlerCols.GetColumnWidth(int.Parse(ddl_View2.SelectedItem.Value), columnName, LoginInfo.ConnStr));
                                column.HeaderStyle.Width = Unit.Pixel(300);


                                // Set column style
                                switch (dsEachBUTrans.Tables[PageCode].Columns[i].DataType.ToString())
                                {
                                    case "System.Int16":
                                    case "System.Int32":
                                    case "System.Int64":
                                        column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                                        column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                        column.DataFormatString = "{0:D}";
                                        column.HeaderStyle.Width = Unit.Pixel(120);
                                        break;

                                    case "System.Decimal":
                                        string fmt = "{0:N2}";
                                        DataTable dt = field.Get(columnName, LoginInfo.ConnStr);
                                        if (dt != null)
                                        {
                                            string scale = dt.Rows[0]["Scale"].ToString();
                                            fmt = "{0:N" + scale + "}";
                                        }


                                        column.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                                        column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                        //column.DataFormatString = "{0:#,0.00##}";
                                        column.DataFormatString = fmt;
                                        column.HeaderStyle.Width = Unit.Pixel(180);
                                        break;

                                    case "System.DateTime":
                                        column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                        //column.HeaderStyle.Width            = 10%;
                                        column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
                                        column.DataFormatString = "{0:dd/MM/yyyy}";
                                        column.HeaderStyle.Width = Unit.Pixel(120);
                                        break;

                                    default:
                                        column.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                        column.ItemStyle.HorizontalAlign = column.HeaderStyle.HorizontalAlign;
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
                                wfColumn.HeaderStyle.Width = Unit.Pixel(120);
                                
                                wfColumn.ItemStyle.HorizontalAlign = wfColumn.HeaderStyle.HorizontalAlign;
                                grd_Trans.Columns.Add(wfColumn);
                            }

                            //---------------------------------------------------------------------

                            GridViewRow selectedRow = grd_Trans.Parent.Parent as GridViewRow;

                            //grd_Trans.DataSource = dtTransaction[selectedRow.RowIndex];


                            grd_Trans.DataSource = SearchData(dsEachBUTrans.Tables[PageCode]).ToTable();
                            grd_Trans.DataBind();

                            dtTransaction[selectedRow.RowIndex] = dsEachBUTrans.Tables[PageCode];
                            Session["dtTransaction"] = dtTransaction;

                        }
                    }
                }
            }
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
                // Add event click to redirect to detail page.
                //e.Row.Attributes.Add("onclick", "OnGridRowClick('" + DataBinder.Eval(e.Row.DataItem, "BuCode").ToString() +
                //    "', '" + DataBinder.Eval(e.Row.DataItem, KeyFieldName).ToString() +
                //    "', '" + ddl_View2.SelectedItem.Value + "')");

                GridView grd_Trans = sender as GridView;

                // Modified by Ake (2014-03-04)
                string urlFilterText = filterText.Replace("'", "~");

                e.Row.Attributes.Add("onclick", "OnGridRowClick('" + DataBinder.Eval(e.Row.DataItem, "BuCode").ToString() +
                    "', '" + DataBinder.Eval(e.Row.DataItem, KeyFieldName).ToString() +
                    "', '" + ddl_View2.SelectedItem.Value + "&Page=" + grd_Trans.PageIndex.ToString() + "&Filter=" + urlFilterText + "')");



                if (WorkFlowEnable && grd_Trans.Columns[grd_Trans.Columns.Count - 1].HeaderText.ToUpper() == "PROCESS STATUS")
                {
                    BoundField apprStatus = new BoundField();
                    apprStatus.DataField = "ApprStatus";

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
            GridView grd_Trans = sender as GridView;
            GridViewRow selectedRow = grd_Trans.Parent.Parent as GridViewRow;

            grd_Trans.PageIndex = e.NewPageIndex;


            DataSet ds = new DataSet();
            HiddenField hf_BuCode = selectedRow.FindControl("hf_BuCode") as HiddenField;
            string buCode = hf_BuCode.Value.ToString();
            bool getData = viewHandler.GetDataList(ds, int.Parse(ddl_View2.SelectedItem.Value), PageCode, KeyFieldName, filterText, buCode, LoginInfo.LoginName, bu.GetConnectionString(buCode));
            if (getData)
            {
                grd_Trans.DataSource = SearchData(ds.Tables[0]);
                grd_Trans.DataBind();
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
            dtTransaction = (DataTable[])Session["dtTransaction"];

            // Get transaction data.
            GridViewRow parentRow = grd_Trans.Parent.Parent as GridViewRow;
            HiddenField hf_BuCode = parentRow.FindControl("hf_BuCode") as HiddenField;

            DataView dv = new DataView(dtTransaction[parentRow.RowIndex]);
            dv.Sort = sortExpression + direction;
            grd_Trans.DataSource = dv;
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
                                        strFilter += "[" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + dt.ToShortDateString() + "'";
                                    }
                                    break;

                                case "Decimal":

                                    Decimal decChk;

                                    if (Decimal.TryParse(strSearch, out decChk))
                                    {
                                        strFilter += "[" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + decChk + "'";
                                    }
                                    break;

                                case "Int32":
                                    Int32 intChk;

                                    if (Int32.TryParse(strSearch, out intChk))
                                    {
                                        strFilter += "[" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + intChk + "'";
                                    }
                                    break;

                                case "Boolean":
                                    break;

                                case "Byte":
                                    break;

                                default:
                                    strFilter = "[" + dvTable.Table.Columns[x].ColumnName.ToString() + "] LIKE '%" + strSearch + "%'";

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
                                        strFilter += " or [" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + dt.ToShortDateString() + "'";
                                    }

                                    break;

                                case "Decimal":

                                    Decimal decChk;

                                    if (Decimal.TryParse(strSearch, out decChk))
                                    {
                                        strFilter += "or [" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + decChk + "'";
                                    }
                                    break;

                                case "Int32":

                                    Int32 intChk;

                                    if (Int32.TryParse(strSearch, out intChk))
                                    {
                                        strFilter += " or [" + dvTable.Table.Columns[x].ColumnName.ToString() + "] = '" + intChk + "'";
                                    }

                                    break;

                                case "Boolean":

                                    break;

                                case "Byte":

                                    break;

                                default:

                                    if (strFilter.ToString() != String.Empty)
                                    {
                                        strFilter += " or [" + dvTable.Table.Columns[x].ColumnName.ToString() + "] LIKE '%" + strSearch + "%'";
                                    }
                                    else
                                    {
                                        strFilter += " [" + dvTable.Table.Columns[x].ColumnName.ToString() + "] LIKE '%" + strSearch + "%'";
                                    }


                                    break;
                            }
                        }
                    }
                }

                try
                {
                    dvTable.RowFilter = strFilter;
                }
                catch
                {
                }
            }

            return dvTable;
        }

        private void Search()
        {
            Page_Load(null, null);
        }

        protected void btn_Search_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("Search");
            this.Search();
        }






        #endregion

        #region Pages (Developping on 2016-12-09)


        private void GetCustomersPageWise(int pageIndex)
        {
        }

        private void PopulatePager(int recordCount, int currentPage)
        {
        }

        #endregion

    }
}
