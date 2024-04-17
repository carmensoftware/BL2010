using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data;
using System.Drawing;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Admin.Interface.AccountMap
{
    public partial class AccountMapp2 : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();


        // Session variable(s)
        private int _pageNo
        {
            get
            {
                return Session["_pageNo"] == null ? 1 : Convert.ToInt32(Session["_pageNo"]);
            }
            set
            {
                Session["_pageNo"] = value;
            }
        }

        private int _totalPageNo
        {
            get
            {
                return Session["_totalPageNo"] == null ? 1 : Convert.ToInt32(Session["_totalPageNo"]);
            }
            set
            {
                Session["_totalPageNo"] = value;
            }
        }

        private DataTable _dtView
        {
            get
            {
                //return (DataTable)Session["_dtView"];
                return (DataTable)ViewState["_dtView"];
            }
            set
            {
                //Session["_dtView"] = value;
                ViewState["_dtView"] = value;
            }
        }
        private DataTable _dtEditData
        {
            get
            {
                return (DataTable)Session["_dtSaveData"];
                //return (DataTable)ViewState["_dtSaveData"];
            }
            set
            {
                Session["_dtSaveData"] = value;
                //ViewState["_dtSaveData"] = value;
            }
        }


        #region -- Event(s) --

        protected override void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Page_Setting();
                lbl_test.Text = "!IsPostBack = " + _pageNo.ToString(); ;
            }
            else
            {
                lbl_test.Text = "PostBack > " + _pageNo.ToString();
            }

        }

        private void Page_Setting()
        {
            SetEdiMode(false);

            ddl_View.DataSource = GetAllView();
            ddl_View.DataTextField = "ViewName";
            ddl_View.DataValueField = "ID";
            ddl_View.DataBind();

            var id = ddl_View.Items.Count > 0 ? Convert.ToInt32(ddl_View.SelectedValue) : -1;
            var pageNo = 1;
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);

            Bind_GridView(id, pageNo, pageSize);
        }

        // -----------------------------------------------------------------

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            SetEdiMode(true);

            var id = Convert.ToInt32(ddl_View.SelectedValue);
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);

            _dtEditData = GetAllData(id);

            Bind_GridView(id, _pageNo, pageSize);

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(ddl_View.SelectedValue);
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);


            SetEdiMode(false);
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(ddl_View.SelectedValue);
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);

            SetEdiMode(false);
            _dtEditData = null;

            Bind_GridView(id, _pageNo, pageSize);
        }

        // -----------------------------------------------------------------

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pageNo = 1;

            SetDataPage(pageNo);
        }

        protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDataPage(_pageNo);
        }

        // -----------------------------------------------------------------
        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gv = sender as GridView;
            var isEdit = btn_Save.Visible;

            var drView = _dtView != null && _dtView.Rows.Count > 0 ? _dtView.Rows[0] : null;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var id = DataBinder.Eval(e.Row.DataItem, "Id").ToString();

                var hf_Id = e.Row.FindControl("hf_Id") as HiddenField;

                hf_Id.Value = id;


                for (int i = 1; i < 10; i++)
                {
                    var value = DataBinder.Eval(e.Row.DataItem, "A" + i.ToString()).ToString();
                    var type = "";
                    
                    var lbl = e.Row.FindControl("lbl_A" + i.ToString()) as Label;
                    lbl.Visible = !isEdit;
                    lbl.Text = value;


                    if (drView != null)
                    {
                        type = drView["TypeA" + i.ToString()].ToString();
                    }

                    var txt = e.Row.FindControl("txt_A" + i.ToString()) as TextBox;
                    txt.Visible = false;

                    var ddl = e.Row.FindControl("ddl_A" + i.ToString()) as ASPxComboBox;
                    ddl.Visible = false;




                    if (isEdit && _dtEditData != null)
                    {
                        var dr = _dtEditData.Select(string.Format("Id='{0}'", id));

                        if (dr != null)
                        {
                            value = dr[0]["A" + i.ToString()].ToString();
                        }

                        txt.Text = value;

                        if (!string.IsNullOrEmpty(type))
                        {
                            var dtDropdown = new DataTable();
                            var ddlWidth = 200;

                            switch (type.ToLower())
                            {
                                case "acccode":
                                    dtDropdown = config.DbExecuteQuery("SELECT AccCode as [Value], CONCAT(AccCode,' : ', AccDesc1,' | ', AccDesc2) as [Text] FROM [ADMIN].AccountCode WHERE AccType <> 'Header' ORDER BY AccCode", null, LoginInfo.ConnStr);
                                    ddlWidth = 400;
                                    break;
                                case "accdept":
                                    dtDropdown = config.DbExecuteQuery("SELECT DeptCode as [Value], CONCAT(DeptCode,' : ',DeptDesc) as [Text] FROM [ADMIN].AccountDepartment ORDER BY DeptCode", null, LoginInfo.ConnStr);
                                    break;
                            }

                            ddl.DataSource = dtDropdown;
                            ddl.TextField = "Text";
                            ddl.ValueField = "Value";
                            ddl.Width = ddlWidth;
                            ddl.EnableAnimation = true;
                            ddl.CallbackPageSize = 50;
                            ddl.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                            ddl.AutoPostBack = true;
                            ddl.DataBind();

                            ddl.Value = value;

                           

                            ddl.Visible = true;
                        }
                        else
                            txt.Visible = true;
                    }

                }
            }
        }

        protected void btn_page_prev_Click(object sender, EventArgs e)
        {
            var btn = (Button)(sender);
            var pageNo = Convert.ToInt32(btn_page_p1.Text) - 1;

            SetDataPage(pageNo);
        }

        protected void btn_page_next_Click(object sender, EventArgs e)
        {
            var btn = (Button)(sender);
            var pageNo = Convert.ToInt32(btn_page_p10.Text) + 1;

            SetDataPage(pageNo);
        }

        protected void btn_page_p_Click(object sender, EventArgs e)
        {
            var btn = (Button)(sender);
            var pageNo = Convert.ToInt32(btn.Text);

            SetDataPage(pageNo);
        }

        #endregion

        // Private method(s)

        private void Bind_GridView(int id, int pageNo, int pageSize)
        {

            SetGridColumn(id);
            gv_Data.DataSource = GetDataPage(id, pageNo, pageSize);
            gv_Data.DataBind();

            SetPageNo(pageNo);
        }



        private void SetDataPage(int pageNo)
        {
            var id = Convert.ToInt32(ddl_View.SelectedValue);
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);


            if (btn_Save.Visible)
            {
                SaveRecentEditData(_dtEditData);
            }

            Bind_GridView(id, pageNo, pageSize);
        }

        private void SetPageNo(int pageNo)
        {
            var pageSize = Convert.ToInt32(ddl_PageSize.SelectedValue);

            btn_page_p1.BackColor = Color.White;
            btn_page_p2.BackColor = Color.White;
            btn_page_p3.BackColor = Color.White;
            btn_page_p4.BackColor = Color.White;
            btn_page_p5.BackColor = Color.White;
            btn_page_p6.BackColor = Color.White;
            btn_page_p7.BackColor = Color.White;
            btn_page_p8.BackColor = Color.White;
            btn_page_p9.BackColor = Color.White;
            btn_page_p10.BackColor = Color.White;


            var digit = (pageNo / 10);
            var pageStart = pageNo % 10 == 0 ? pageNo - 9 : ((pageNo / 10) * 10) + 1;


            btn_page_p1.Text = (pageStart + 0).ToString();
            btn_page_p2.Text = (pageStart + 1).ToString();
            btn_page_p3.Text = (pageStart + 2).ToString();
            btn_page_p4.Text = (pageStart + 3).ToString();
            btn_page_p5.Text = (pageStart + 4).ToString();
            btn_page_p6.Text = (pageStart + 5).ToString();
            btn_page_p7.Text = (pageStart + 6).ToString();
            btn_page_p8.Text = (pageStart + 7).ToString();
            btn_page_p9.Text = (pageStart + 8).ToString();
            btn_page_p10.Text = (pageStart + 9).ToString();


            btn_page_prev.Visible = pageNo > 10;
            btn_page_next.Visible = (_totalPageNo / pageSize) > (pageStart + 9);


            switch (pageNo % 10)
            {
                case 1:
                    btn_page_p1.BackColor = Color.SkyBlue;
                    break;
                case 2:
                    btn_page_p2.BackColor = Color.SkyBlue;
                    break;
                case 3:
                    btn_page_p3.BackColor = Color.SkyBlue;
                    break;
                case 4:
                    btn_page_p4.BackColor = Color.SkyBlue;
                    break;
                case 5:
                    btn_page_p5.BackColor = Color.SkyBlue;
                    break;
                case 6:
                    btn_page_p6.BackColor = Color.SkyBlue;
                    break;
                case 7:
                    btn_page_p7.BackColor = Color.SkyBlue;
                    break;
                case 8:
                    btn_page_p8.BackColor = Color.SkyBlue;
                    break;
                case 9:
                    btn_page_p9.BackColor = Color.SkyBlue;
                    break;
                case 0:
                    btn_page_p10.BackColor = Color.SkyBlue;
                    break;
            }

            _pageNo = pageNo;
        }

        private void SetEdiMode(bool isEdit)
        {
            btn_Edit.Visible = !isEdit;

            btn_Save.Visible = isEdit;
            btn_Cancel.Visible = isEdit;
        }

        private DataTable GetAllView()
        {
            return config.DbExecuteQuery("SELECT * FROM [ADMIN].AccountMappView ORDER BY ID", null, LoginInfo.ConnStr);
        }

        private DataTable GetAllData(int viewId)
        {
            var connStr = LoginInfo.ConnStr;
            var sql = string.Format("SELECT PostType FROM [ADMIN].AccountMappView WHERE ID={0} ", viewId);

            var dtView = config.DbExecuteQuery(sql, null, connStr);

            if (dtView != null && dtView.Rows.Count > 0)
            {
                var dr = dtView.Rows[0];

                var postType = dr["PostType"].ToString();

                return config.DbExecuteQuery(string.Format("SELECT ID, A1, A2, A3, A4, A5, A6, A7, A8, A9 FROM [ADMIN].AccountMapp WHERE PostType='{0}'", postType), null, connStr);
            }
            else
                return null;
        }

        private DataTable GetDataPage(int viewId, int pageNo = 0, int pageSize = 25)
        {
            var connStr = LoginInfo.ConnStr;
            var dt = new DataTable();


            var rows = pageSize <= 0 ? 25 : pageSize;
            var offset_rows = pageNo <= 1 ? 0 : rows * (pageNo - 1);

            //var sql = string.Format("SELECT * FROM [ADMIN].AccountMappView WHERE ID={0} ORDER BY StoreCode, CategoryCode, SubCategoryCode, ItemGroupCode, A1, A2 OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", viewId, offset_rows, rows);
            var sql = string.Format("SELECT * FROM [ADMIN].AccountMappView WHERE ID={0} ", viewId);

            _dtView = config.DbExecuteQuery(sql, null, connStr);

            if (_dtView != null && _dtView.Rows.Count > 0)
            {
                var dr = _dtView.Rows[0];

                var postType = dr["PostType"].ToString();

                sql = @"
SELECT 
    ID,
	ac.StoreCode as LocationCode,
	l.LocationName,
	ac.CategoryCode,
	c.CategoryName,
	ac.SubCategoryCode,
	s.CategoryName as SubCategoryName,
	ac.ItemGroupCode,
	i.CategoryName as ItemGroupName,
	A1,
	A2,
	A3,
	A4,
	A5,
	A6,
	A7,
	A8,
	A9
FROM
	[ADMIN].AccountMapp ac
	LEFT JOIN [IN].StoreLocation l
		ON l.LocationCode=ac.StoreCode
	LEFT JOIN [IN].ProductCategory c
		ON c.CategoryCode = ac.CategoryCode AND c.LevelNo=1
	LEFT JOIN [IN].ProductCategory s
		ON s.CategoryCode = ac.SubCategoryCode AND s.LevelNo=2
	LEFT JOIN [IN].ProductCategory i
		ON i.CategoryCode = ac.ItemGroupCode AND i.LevelNo=3
WHERE
	ac.PostType = '{0}'
ORDER BY 
    StoreCode, 
    CategoryCode, 
    SubCategoryCode, 
    ItemGroupCode, 
    A1, 
    A2 
OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY
";

                dt = config.DbExecuteQuery(string.Format(sql, postType, offset_rows, rows), null, connStr);

                // Get Total page

                var dtCount = config.DbExecuteQuery(string.Format("SELECT COUNT(*) FROM [ADMIN].AccountMapp WHERE PostType='{0}'", postType), null, connStr);

                _totalPageNo = dtCount == null || dtCount.Rows.Count == 0 ? 0 : Convert.ToInt32(dtCount.Rows[0][0]);
            }

            return dt;
        }

        private void SetGridColumn(int viewId)
        {
            var connStr = LoginInfo.ConnStr;
            var dtView = config.DbExecuteQuery("SELECT * FROM [ADMIN].AccountMappView WHERE ID=" + viewId.ToString(), null, connStr);

            var gv = gv_Data;

            var showLocation = false;
            var showCategory = false;
            var showSubCategory = false;
            var showItemGroup = false;

            var showKeyA1 = false;
            var showKeyA2 = false;
            var showKeyA3 = false;
            var showKeyA4 = false;
            var showKeyA5 = false;
            var showKeyA6 = false;
            var showKeyA7 = false;
            var showKeyA8 = false;
            var showKeyA9 = false;

            var showA1 = false;
            var showA2 = false;
            var showA3 = false;
            var showA4 = false;
            var showA5 = false;
            var showA6 = false;
            var showA7 = false;
            var showA8 = false;
            var showA9 = false;

            var descKeyA1 = "";
            var descKeyA2 = "";
            var descKeyA3 = "";
            var descKeyA4 = "";
            var descKeyA5 = "";
            var descKeyA6 = "";
            var descKeyA7 = "";
            var descKeyA8 = "";
            var descKeyA9 = "";

            var descA1 = "";
            var descA2 = "";
            var descA3 = "";
            var descA4 = "";
            var descA5 = "";
            var descA6 = "";
            var descA7 = "";
            var descA8 = "";
            var descA9 = "";

            if (dtView != null && dtView.Rows.Count > 0)
            {
                var dr = dtView.Rows[0];

                showLocation = Convert.ToBoolean(dr["StoreCode"]);
                showCategory = Convert.ToBoolean(dr["CategoryCode"]);
                showSubCategory = Convert.ToBoolean(dr["SubCategoryCode"]);
                showItemGroup = Convert.ToBoolean(dr["ItemGroupCode"]);

                showKeyA1 = Convert.ToBoolean(dr["KeyA1"]);
                showKeyA2 = Convert.ToBoolean(dr["KeyA2"]);
                showKeyA3 = Convert.ToBoolean(dr["KeyA3"]);
                showKeyA4 = Convert.ToBoolean(dr["KeyA4"]);
                showKeyA5 = Convert.ToBoolean(dr["KeyA5"]);
                showKeyA6 = Convert.ToBoolean(dr["KeyA6"]);
                showKeyA7 = Convert.ToBoolean(dr["KeyA7"]);
                showKeyA8 = Convert.ToBoolean(dr["KeyA8"]);
                showKeyA9 = Convert.ToBoolean(dr["KeyA9"]);


                descKeyA1 = dr["DescKeyA1"].ToString();
                descKeyA2 = dr["DescKeyA2"].ToString();
                descKeyA3 = dr["DescKeyA3"].ToString();
                descKeyA4 = dr["DescKeyA4"].ToString();
                descKeyA5 = dr["DescKeyA5"].ToString();
                descKeyA6 = dr["DescKeyA6"].ToString();
                descKeyA7 = dr["DescKeyA7"].ToString();
                descKeyA8 = dr["DescKeyA8"].ToString();
                descKeyA9 = dr["DescKeyA9"].ToString();

                showA1 = Convert.ToBoolean(dr["A1"]);
                showA2 = Convert.ToBoolean(dr["A2"]);
                showA3 = Convert.ToBoolean(dr["A3"]);
                showA4 = Convert.ToBoolean(dr["A4"]);
                showA5 = Convert.ToBoolean(dr["A5"]);
                showA6 = Convert.ToBoolean(dr["A6"]);
                showA7 = Convert.ToBoolean(dr["A7"]);
                showA8 = Convert.ToBoolean(dr["A8"]);
                showA9 = Convert.ToBoolean(dr["A9"]);

                descA1 = dr["DescA1"].ToString();
                descA2 = dr["DescA2"].ToString();
                descA3 = dr["DescA3"].ToString();
                descA4 = dr["DescA4"].ToString();
                descA5 = dr["DescA5"].ToString();
                descA6 = dr["DescA6"].ToString();
                descA7 = dr["DescA7"].ToString();
                descA8 = dr["DescA8"].ToString();
                descA9 = dr["DescA9"].ToString();

            }

            // set gridview

            gv.Columns[0].Visible = showLocation;
            gv.Columns[1].Visible = showCategory;
            gv.Columns[2].Visible = showSubCategory;
            gv.Columns[3].Visible = showItemGroup;

            // A1- A8 (key)
            gv.Columns[4].Visible = showKeyA1;
            gv.Columns[5].Visible = showKeyA2;
            gv.Columns[6].Visible = showKeyA3;
            gv.Columns[7].Visible = showKeyA4;
            gv.Columns[8].Visible = showKeyA5;
            gv.Columns[9].Visible = showKeyA6;
            gv.Columns[10].Visible = showKeyA7;
            gv.Columns[11].Visible = showKeyA8;
            gv.Columns[12].Visible = showKeyA9;

            // A1 - A8 (Value)
            gv.Columns[4].Visible = showA1;
            gv.Columns[5].Visible = showA2;
            gv.Columns[6].Visible = showA3;
            gv.Columns[7].Visible = showA4;
            gv.Columns[8].Visible = showA5;
            gv.Columns[9].Visible = showA6;
            gv.Columns[10].Visible = showA7;
            gv.Columns[11].Visible = showA8;
            gv.Columns[12].Visible = showA9;


            // A1 - A8 (HeaderText)
            gv.Columns[4].HeaderText = showKeyA1 ? descKeyA1 : descA1;
            gv.Columns[5].HeaderText = showKeyA2 ? descKeyA2 : descA2;
            gv.Columns[6].HeaderText = showKeyA3 ? descKeyA3 : descA3;
            gv.Columns[7].HeaderText = showKeyA4 ? descKeyA4 : descA4;
            gv.Columns[8].HeaderText = showKeyA5 ? descKeyA5 : descA5;
            gv.Columns[9].HeaderText = showKeyA6 ? descKeyA6 : descA6;
            gv.Columns[10].HeaderText = showKeyA7 ? descKeyA7 : descA7;
            gv.Columns[11].HeaderText = showKeyA8 ? descKeyA8 : descA8;
            gv.Columns[12].HeaderText = showKeyA9 ? descKeyA9 : descA9;

        }



        private void SaveRecentEditData(DataTable dt)
        {
            foreach (GridViewRow row in gv_Data.Rows)
            {
                var hf_Id = row.FindControl("hf_Id") as HiddenField;
                var id = hf_Id.Value.ToString();

                for (int i = 1; i < 10; i++)
                {
                    var txt = row.FindControl("txt_A" + i.ToString()) as TextBox;
                    var ddl = row.FindControl("ddl_A" + i.ToString()) as ASPxComboBox;

                    if (txt != null || ddl != null)
                    {

                        //var value = txt != null ? txt.Text : ddl.SelectedItem.Value.ToString();
                        var value = txt != null ? txt.Text : ddl.SelectedItem.Value.ToString();


                        if (dt != null)
                        {
                            var dr = dt.Select(string.Format("ID='{0}'", id));

                            if (dr != null)
                            {
                                dr[0]["A" + i.ToString()] = value;
                            }
                        }
                    }
                }
            }
        }
    }


}