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
using Blue.DAL;
using System.Text.RegularExpressions;

namespace BlueLedger.PL.UserControls.ViewHandler
{
    public partial class ListPage2 : BaseUserControl
    {
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.ViewHandlerCrtr viewHandlerCrtr = new Blue.BL.APP.ViewHandlerCrtr();
        private readonly Blue.BL.APP.ViewHandlerCols viewHandlerCols = new Blue.BL.APP.ViewHandlerCols();
        private readonly Blue.BL.APP.ViewHandlerOrder viewHandlerOrder = new Blue.BL.APP.ViewHandlerOrder();
        private readonly Blue.BL.APP.WF wf = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();

        private readonly Blue.BL.APP.Field field = new Blue.BL.APP.Field();

        private readonly Blue.BL.dbo.BUUser buUser = new Blue.BL.dbo.BUUser();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        // ViewState variable(s)



        #region --Request.QueryString--

        public string VID
        {
            get
            {
                var vid = string.Empty;

                if (Request.QueryString["VID"] != null)
                    vid = Request.QueryString["VID"].ToString();
                else if (Request.Cookies[PageCode] != null)
                {
                    vid = Request.Cookies[PageCode].Value;
                }

                return  vid;
            }
        }

        private int _page
        {
            get
            {
                var value = Request.QueryString["page"];

                return string.IsNullOrEmpty(value) ? 1 : int.Parse(value);
            }
        }

        private int _per_page
        {
            get
            {
                var value = Request.QueryString["per_page"];

                return string.IsNullOrEmpty(value) ? 20 : int.Parse(value);
            }
        }


        #endregion

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
                return wf.GetIsActive(Module, SubModule, LoginInfo.ConnStr);
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

        #region --Menu (AddOn)

        /// <summary>
        /// Adds or Removes Menu Header
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

        #endregion

        #region --Not used but must have--
        public DataTable dtBuKeys
        {
            get { return new DataTable(); }
        }

        public void setViewMultipleBU(bool value)
        {
        }

        public void setPage(int pageIndex)
        {
            //prevPageIndex = pageIndex;
        }

        public void setFilter(string value)
        {
            //filterText = value;
        }


        #endregion



        // Event(s)
        #region --Event(s)--

        public override void DataBind()
        {
            base.DataBind();

            this.Binding_ddl_View2();
            //this.Page_Retrieve();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            {
            }

            this.Page_Retrieve();


        }


        private void Page_Retrieve()
        {
            Bind_Data();

            this.Page_Setting();
        }

        private void Page_Setting()
        {
            lbl_Title.Text = this.Title;
            // Menu setting -----------------------------------------------------------------------

            var wfId = viewHandler.GetWFId(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr);
            var wfStep = viewHandler.GetWFStep(int.Parse(ddl_View2.SelectedItem.Value), LoginInfo.ConnStr);

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
                menu_CmdBar.Items.FindByName("Create").Visible = wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr);
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

            SetPageNo();

            Response.Cookies.Remove(PageCode);
            Response.Cookies.Add(new HttpCookie(PageCode));
            Response.Cookies[PageCode].Value = ddl_View2.SelectedItem.Value;
            Response.Cookies[PageCode].Expires = DateTime.Now.AddHours(8);

        }

        // -----------------------------------------

        protected void ddl_View2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_Search.Text = string.Empty;

            var viewId = ddl_View2.SelectedItem.Value;
            RedirectView(viewId);

        }

        protected void btn_ViewGo2_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
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

        protected void btn_Search_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void gv_Data_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "OnGridRowMouseOver(this)");
                e.Row.Attributes.Add("onmouseout", "OnGridRowMouseOut(this)");
            }
        }

        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gv = sender as GridView;

            if (e.Row.RowType == DataControlRowType.Header)
            {
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var buCode = LoginInfo.BuInfo.BuCode;
                var keyFieldValue = DataBinder.Eval(e.Row.DataItem, KeyFieldName).ToString();
                var viewNo = ddl_View2.SelectedItem.Value;
                var page = _page.ToString();

                e.Row.Attributes.Add("key", keyFieldValue);

                // Add event to cell
                var startIndex = 0;

                for (int i = startIndex; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("onclick", string.Format("GridCell_Click('{0}', '{1}', '{2}','{3}')", buCode, keyFieldValue, viewNo, page));
                }

                var lastColumnIndex = gv.Columns.Count - 1;
                var lastColumn = gv.Columns[lastColumnIndex];

                if (WorkFlowEnable && lastColumn.HeaderText.ToUpper() == "PROCESS STATUS")
                {
                    #region
                    BoundField apprStatus = new BoundField();

                    apprStatus.DataField = "ApprStatus";

                    string newStatus = string.Empty;

                    foreach (char eachStep in e.Row.Cells[gv.Columns.Count - 1].Text)
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

                    e.Row.Cells[lastColumnIndex].Text = newStatus;
                }
                    #endregion

            }
        }



        protected void btn_Page_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;


            var page = btn.Text;
            var per_page = "20";

            switch (btn.ID.ToLower())
            {
                case "btn_page_previous":
                    var page1 = btn_P1.Text;
                    if (page1 == "1")
                        page = "1";
                    else
                        page = (int.Parse(page1) - 1).ToString();
                    break;
                case "btn_page_next":
                    page = (int.Parse(btn_P10.Text) + 1).ToString();

                    break;
            }

            RedirectPage(page, per_page);
        }

        #endregion

        #region --Method(s)--


        private void RedirectView(string viewId)
        {
            var key = "VID";
            var value = viewId;
            var url = Request.RawUrl;

            if (Request.QueryString.Count == 0) // no query string
            {
                url = string.Format("{0}?{1}={2}", Request.Path, key, value);
            }
            else
            {
                var query = HttpUtility.ParseQueryString(Request.Url.Query);

                query.Remove(key);

                if (query.Count == 0)
                    url = string.Format("{0}?{1}={2}", Request.Path, key, value);
                else
                    url = string.Format("{0}?{1}&{2}={3}", Request.Path, query, key, value);
            }

            Response.Redirect(url);
        }

        private void RedirectPage(string page, string per_page = "20")
        {
            var url = string.Format("{0}?VID={1}&page={2}", Request.Path, VID, page);

            Response.Redirect(url);
        }

        private void Binding_ddl_View2()
        {
            ddl_View2.DataSource = viewHandler.GetList(PageCode, LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_View2.DataBind();

            if (ddl_View2.Items.Count == 0)
            {
                btn_ViewGo2.Enabled = false;
                //btn_ViewModify2.Enabled = false;
                return;
            }


            if (string.IsNullOrEmpty(VID))
            {
                var viewId = ddl_View2.SelectedItem.Value;
                RedirectView(viewId);

            }
            else
            {
                ddl_View2.SelectedValue = VID;
            }
        }

        private DataTable GetData(int viewNo, string keyFieldName, string searchText)
        {
            var buCode = LoginInfo.BuInfo.BuCode;
            var connStr = bu.GetConnectionString(buCode);
            var loginName = LoginInfo.LoginName;

            var p = new DbParameter[1];
            p[0] = new DbParameter("@ViewNo", viewNo.ToString());

            var dtView = viewHandler.DbExecuteQuery("EXEC APP.GetViewHandlerByViewNo @ViewNo", p, connStr);

            if (dtView == null || dtView.Rows.Count == 0)
                return null;

            var drView = dtView.Rows[0];
            var pageCode = drView["PageCode"].ToString();
            var advOption = drView["AdvOpt"].ToString();
            var wfId = string.IsNullOrEmpty(drView["WfId"].ToString()) ? 0 : int.Parse(drView["WfId"].ToString());
            var wfStep = string.IsNullOrEmpty(drView["WfStep"].ToString()) ? 0 : int.Parse(drView["WfStep"].ToString());
            var isHOD = drView["IsHOD"].ToString() == "1";

            var sql = new StringBuilder();

            sql.Append("SELECT " + keyFieldName + ", ");


            // if it is workflow then adding ApprStatus
            if (wfId > 0 && wfStep > 0)
            {
                sql.Append("ApprStatus, ");
            }

            // Get column list 
            var columnList = viewHandlerCols.GetColumnList(viewNo, connStr);
            sql.Append(columnList);
            sql.AppendFormat(" FROM {0} ", pageCode);
            sql.AppendFormat(" WHERE 1=1 ");


            var dbParams = new DbParameter[0];
            // Get criteria
            var criteriaList = viewHandlerCrtr.GetCriteriaList(viewNo, advOption, ref dbParams, connStr);

            //lbl_View.Text = criteriaList + " >> " + dbParams[0].ParameterName.ToString() + " = " + dbParams[0].ParameterValue.ToString();

            // Assign Special Params to dbParams
            for (int i = 0; i < dbParams.Length; i++)
            {
                if (dbParams[i].ParameterValue == "@UserDepartment")
                {
                    DataTable dt = viewHandler.DbExecuteQuery(string.Format("SELECT TOP(1) DepCode FROM [ADMIN].UserDepartment WHERE LoginName='{0}'", loginName), null, connStr);
                    if (dt.Rows.Count > 0)
                    {
                        dbParams[i].ParameterValue = Regex.Replace(dbParams[i].ParameterValue.ToString(), "@UserDepartment", dt.Rows[0][0].ToString(), RegexOptions.IgnoreCase);
                    }
                }
                else if (dbParams[i].ParameterValue == "@LoginName")
                {
                    dbParams[i].ParameterValue = Regex.Replace(dbParams[i].ParameterValue.ToString(), "@LoginName", loginName, RegexOptions.IgnoreCase);
                }

            }


            // Always add work-flow criteria to query when work-flow was enable
            #region
            if (wfId > 0)
            {

                // Add Normal Criteria
                string criteriaField = Regex.Replace(wfDt.GetCriteria(wfId, wfStep, connStr), "@LoginName", " '" + loginName + "' ", RegexOptions.IgnoreCase);

                criteriaList += string.Format("{0} {1} ", (criteriaList != string.Empty ? " AND " : " "), criteriaField);

                var userChkField = wfDt.GetUserChkField(wfId, wfStep, connStr);
                if (userChkField != string.Empty)
                {
                    if (userChkField.Split(',').Length > 0)
                    {
                        string[] fields = userChkField.Split(',');

                        criteriaList += string.Format(" {0} ({1} = '{2}'", (criteriaList.Trim() != string.Empty ? "AND" : " "), fields[0].Trim(), loginName);
                        for (int i = 1; i <= fields.Length - 1; i++)
                        {
                            criteriaList += string.Format(" OR {0} = '{1}'", fields[i].Trim(), loginName);
                        }

                        criteriaList += " ) ";
                    }
                    else
                        criteriaList += string.Format(" {0}  {1} = '{2}'", (criteriaList.Trim() != string.Empty ? "AND" : " "), userChkField, loginName);
                }

                // Add Addition Critera (User Permission Group)
                var permissionGrp = wfDt.GetPermissionGrp(wfId, wfStep, connStr);
                if (permissionGrp != string.Empty)
                {
                    criteriaList += string.Format("{0}'{1}' IN ({2})", (criteriaList != string.Empty ? " AND " : " "), loginName, permissionGrp);
                }
            }
            #endregion

            // Get sorting
            var sortingList = viewHandlerOrder.GetSortingList(viewNo, connStr);

            // Criteria Condition
            if (criteriaList.Trim() != string.Empty)
                sql.AppendFormat(" AND {0}", criteriaList);

            // Filter Condition

            if (!string.IsNullOrEmpty(searchText))
            {
                var filterText = new List<string>();
                var columns = columnList.Split(',').Select(x => x.Trim()).ToArray();

                foreach (var column in columns)
                {
                    var fieldName = column.Split(' ')[0].Trim().TrimStart('[').TrimEnd(']');

                    filterText.Add(string.Format(" [{0}] LIKE '%{1}%' ", fieldName, searchText));
                }

                var filterStatement = string.Join("OR", filterText);
                //lbl_View.Text = filterStatement;
                sql.AppendFormat(" AND ({0})", filterStatement);

            }
            // Head of Location Condition
            if (isHOD)
            {
                sql.AppendFormat(" AND [HOD] IN (SELECT DepCode FROM [Admin].vHeadOfDepartment WHERE LoginName = '{0}')", loginName);
            }


            if (sortingList.Trim() != string.Empty)
            {
                sql.AppendFormat(" ORDER BY {0}", sortingList);
            }
            else
            {
                sql.AppendFormat(" ORDER BY {0} DESC", keyFieldName);
            }

            var offset = (_page - 1) * _per_page;
            var fetch = _per_page;

            sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, fetch);

            //lbl_View.Text = sql.ToString()  ;

            var dtData = viewHandler.DbExecuteQuery(sql.ToString(), dbParams, connStr);

            return dtData == null ? new DataTable() : dtData;
        }

        private void Bind_Data()
        {
            var viewNo = int.Parse(VID);

            var gv = gv_Data;

            var searchText = txt_Search.Text.Trim();
            var dtData = GetData(viewNo, KeyFieldName, searchText);

            // Find column index of start adding column
            int startColumnIndex = 0;

            if (viewHandler.GetIsWFEnable(viewNo, LoginInfo.ConnStr))
            {
                // Skip Columns
                // 0 = [KeyFieldName]
                // 1 = [ApprStatus]
                startColumnIndex = 2;
            }
            else
            {
                // Skip Columns
                // 0 = [KeyFieldName]
                startColumnIndex = 1;
            }

            gv.Columns.Clear();


            // Add Columns
            for (int i = startColumnIndex; i < dtData.Columns.Count; i++)
            {
                var columnName = dtData.Columns[i].ColumnName;
                var column = new BoundField();
                #region

                column.HeaderText = field.GetDesc(columnName, LoginInfo.ConnStr);
                column.DataField = columnName;
                column.HeaderStyle.Width = Unit.Pixel(300);

                // Set column style
                switch (dtData.Columns[i].DataType.ToString())
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
                        var fmt = "{0:N2}";

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

                #endregion

                gv.Columns.Add(column);
            }

            // Add "Process Status" column if this view is related to workflow or thereis column ApprStatus
            if (WorkFlowEnable && dtData.Columns.Contains("ApprStatus"))
            {
                BoundField wfColumn = new BoundField();
                wfColumn.HeaderText = "Process Status";
                wfColumn.DataField = "ApprStatus";
                wfColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                wfColumn.HeaderStyle.Width = Unit.Pixel(120);

                wfColumn.ItemStyle.HorizontalAlign = wfColumn.HeaderStyle.HorizontalAlign;

                gv_Data.Columns.Add(wfColumn);
            }
            gv.DataSource = dtData;
            gv.DataBind();
        }

        private void SetPageNo()
        {
            var page = _page <= 0 ? 1 : _page;
            var per_page = _per_page;

            var digit = page % 10;
            digit = digit == 0 ? 10 : digit;
            var start = page < 11 ? 1 : page - digit + 1;

            btn_P1.Text = start++.ToString();
            btn_P2.Text = start++.ToString();
            btn_P3.Text = start++.ToString();
            btn_P4.Text = start++.ToString();
            btn_P5.Text = start++.ToString();
            btn_P6.Text = start++.ToString();
            btn_P7.Text = start++.ToString();
            btn_P8.Text = start++.ToString();
            btn_P9.Text = start++.ToString();
            btn_P10.Text = start++.ToString();


            // Reset button colors

            var backColor = default(System.Drawing.Color);
            var foreColor = default(System.Drawing.Color);

            btn_P1.BackColor = backColor;
            btn_P1.ForeColor = foreColor;
            btn_P2.BackColor = backColor;
            btn_P2.ForeColor = foreColor;
            btn_P3.BackColor = backColor;
            btn_P3.ForeColor = foreColor;
            btn_P4.BackColor = backColor;
            btn_P4.ForeColor = foreColor;
            btn_P5.BackColor = backColor;
            btn_P5.ForeColor = foreColor;
            btn_P6.BackColor = backColor;
            btn_P6.ForeColor = foreColor;
            btn_P7.BackColor = backColor;
            btn_P7.ForeColor = foreColor;
            btn_P8.BackColor = backColor;
            btn_P8.ForeColor = foreColor;
            btn_P9.BackColor = backColor;
            btn_P9.ForeColor = foreColor;
            btn_P10.BackColor = backColor;
            btn_P10.ForeColor = foreColor;

            backColor = System.Drawing.Color.Black;
            foreColor = System.Drawing.Color.White;

            var index = digit == 0 ? 10 : digit;
            switch (index)
            {
                case 1:
                    btn_P1.BackColor = backColor;
                    btn_P1.ForeColor = foreColor;
                    break;
                case 2:
                    btn_P2.BackColor = backColor;
                    btn_P2.ForeColor = foreColor;
                    break;
                case 3:
                    btn_P3.BackColor = backColor;
                    btn_P3.ForeColor = foreColor;
                    break;
                case 4:
                    btn_P4.BackColor = backColor;
                    btn_P4.ForeColor = foreColor;
                    break;
                case 5:
                    btn_P5.BackColor = backColor;
                    btn_P5.ForeColor = foreColor;
                    break;
                case 6:
                    btn_P6.BackColor = backColor;
                    btn_P6.ForeColor = foreColor;
                    break;
                case 7:
                    btn_P7.BackColor = backColor;
                    btn_P7.ForeColor = foreColor;
                    break;
                case 8:
                    btn_P8.BackColor = backColor;
                    btn_P8.ForeColor = foreColor;
                    break;
                case 9:
                    btn_P9.BackColor = backColor;
                    btn_P9.ForeColor = foreColor;
                    break;
                case 10:
                    btn_P10.BackColor = backColor;
                    btn_P10.ForeColor = foreColor;
                    break;
            }


        }





        #endregion
    }
}