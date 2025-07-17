using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Drawing;
using System.Text;


namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class ExportRestore : BasePage
    {
        const string moduleID = "2.9.31";

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.RolePermission permission = new Blue.BL.ADMIN.RolePermission();

        private string prevDocNo = "";
        private bool _toggle_color = false;
        private DataTable _dtAccMapDesc;


        #region "Attributes"
        protected bool _hasPermissionEdit
        {
            get
            {
                var pagePermiss = permission.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

                return pagePermiss >= 3;
            }
        }

        //private DataTable _dtExport
        //{
        //    get { return ViewState["_dtExport"] as DataTable; }
        //    set { ViewState["_dtExport"] = value; }
        //}


        #endregion

        #region "Event(s)"

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                de_FromDate.Date = ServerDateTime.Date;
                de_ToDate.Date = ServerDateTime.Date;
            }

            _dtAccMapDesc = config.DbExecuteQuery("SELECT TOP(1) DescA1, DescA2, DescA3 FROM [ADMIN].AccountMappView WHERE PostType='AP'", null, LoginInfo.ConnStr);

        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            //var dateFrom = de_FromDate.Date;
            //var dateTo = de_ToDate.Date;

            //Preview(dateFrom, dateTo);
            BindData();
        }

        protected void btn_Restore_Click(object sender, EventArgs e)
        {
            var dateFrom = de_FromDate.Date;
            var dateTo = de_ToDate.Date;

            Restore(dateFrom, dateTo);
        }


        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string _descA1 = "", _descA2 = "", _descA3 = "";

            if (_dtAccMapDesc != null && _dtAccMapDesc.Rows.Count > 0)
            {
                _descA1 = _dtAccMapDesc.Rows[0]["DescA1"].ToString();
                _descA2 = _dtAccMapDesc.Rows[0]["DescA2"].ToString();
                _descA3 = _dtAccMapDesc.Rows[0]["DescA3"].ToString();
            }

            #region --header--
            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (e.Row.FindControl("lbl_A1_Header") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A1_Header") as Label;

                    lbl.Text = _descA1;
                }

                if (e.Row.FindControl("lbl_A2_Header") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2_Header") as Label;

                    lbl.Text = _descA2;
                }

            }
            #endregion

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                var docNo = DataBinder.Eval(dataItem, "DocNo").ToString();

                var exportStatus = DataBinder.Eval(dataItem, "ExportStatus").ToString();

                var docType = DataBinder.Eval(dataItem, "DocType").ToString();
                var sunAccountNo = DataBinder.Eval(dataItem, "SunAccountNo").ToString();
                var a1 = DataBinder.Eval(dataItem, "A1").ToString();
                var a2 = DataBinder.Eval(dataItem, "A2").ToString();
                //var locationCode = DataBinder.Eval(dataItem, "LocationCode").ToString();
                //var itemGroupCode = DataBinder.Eval(dataItem, "ItemGroupCode").ToString();

                var color = Color.Transparent;

                if (string.IsNullOrEmpty(prevDocNo) || prevDocNo != docNo)
                {
                    _toggle_color = !_toggle_color;
                    prevDocNo = docNo;
                }

                e.Row.BackColor = _toggle_color ? Color.Transparent : Color.WhiteSmoke;


                if (e.Row.FindControl("lbl_SunAccountNo") != null)
                {
                    var lbl = e.Row.FindControl("lbl_SunAccountNo") as Label;
                    var value = sunAccountNo;

                    lbl.Text = string.IsNullOrEmpty(value) ? "Not Set" : value;
                    lbl.ForeColor = string.IsNullOrEmpty(value) ? Color.Tomato : Color.Black;
                    lbl.Font.Bold = string.IsNullOrEmpty(value) ? false : true;

                    if (string.IsNullOrEmpty(value) && docType == "VendorLine")
                    {
                        lbl.ToolTip = "Set at 'Acct. Link Ref.' in Vendor Profile.";
                    }
                    else if (string.IsNullOrEmpty(value) && docType == "ExpenseLine")
                    {
                        lbl.ToolTip = string.Format("Set at '{0}' in Account Mapping.", _descA3);
                    }
                    else if (string.IsNullOrEmpty(value) && docType == "TaxLine")
                    {
                        lbl.ToolTip = "Set at 'Tax Account' in Product.";
                    }

                }

                if (e.Row.FindControl("lbl_A1") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A1") as Label;

                    var value = a1;

                    lbl.Text = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? "Not Set" : value;
                    lbl.ForeColor = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? Color.Tomato : Color.Black;
                    lbl.Font.Bold = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? false : true;

                    //if (string.IsNullOrEmpty(value) && docType == "ExpenseLine")
                    //{
                    //    lbl.ToolTip = string.Format("Set '{0}' at location='{1}' and itemgroup='{2}' in Account Mapping.", _descA1, locationCode, itemGroupCode);
                    //}
                }

                if (e.Row.FindControl("lbl_A2") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2") as Label;

                    var value = a2;

                    lbl.Text = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? "Not Set" : value;
                    lbl.ForeColor = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? Color.Tomato : Color.Black;
                    lbl.Font.Bold = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? false : true;

                    //if (string.IsNullOrEmpty(value) && docType == "ExpenseLine")
                    //{
                    //    lbl.ToolTip = string.Format("Set '{0}' at location='{1}' and itemgroup='{2}' in Account Mapping.", _descA2, locationCode, itemGroupCode);
                    //}

                }

                if (e.Row.FindControl("img_ExportStatus") != null)
                {
                    var img = e.Row.FindControl("img_ExportStatus") as System.Web.UI.WebControls.Image;

                    img.Visible = Convert.ToBoolean(exportStatus);

                }

                //if (e.Row.FindControl("lbl_Remark") != null)
                //{
                //    var lbl = e.Row.FindControl("lbl_Remark") as Label;
                //    var errors = new List<string>();

                //    if (string.IsNullOrEmpty(sunAccountNo))
                //    {
                //        errors.Add("No Sun vendor or Tax Account. *set Sun Vendor at Vendor Profile / Tax accunt at Product.");
                //        errors.Add("");
                //    }

                //    if (string.IsNullOrEmpty(a1) || string.IsNullOrEmpty(a2))
                //    {
                //        errors.Add("No mapping accounts. *set at Account Mapping.");
                //        errors.Add("");
                //    }


                //    lbl.Text = string.Join("<br/>", errors);
                //    lbl.ForeColor = Color.Tomato;
                //}
            }
        }

        protected void gv_Data_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;


            var dateFrom = de_FromDate.Date;
            var dateTo = de_ToDate.Date;

            BindData();
        }

        #endregion

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void BindData()
        {
            var fDate = de_FromDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var tDate = de_ToDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = "EXEC [Tool].ExportToAP_SunSystems @FDate=@FDate, @TDate=@TDate, @IsExport=0, @Status='E'";            
            var parameters = new SqlParameter[] 
            {
                new SqlParameter("@FDate", fDate),
                new SqlParameter("@TDate", tDate),
            };

            var dt = sql.ExecuteQuery(query, parameters);

            gv_Data.DataSource = dt;
            gv_Data.DataBind();
        }

        private void Restore(DateTime dateFrom, DateTime dateTo)
        {
            var fDate = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var tDate = dateTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var query = @"
UPDATE PC.Rec SET ExportStatus=0 WHERE RecDate BETWEEN @FDate AND @TDate
UPDATE PC.Cn SET ExportStatus=0 WHERE CnDate BETWEEN @FDate AND @TDate";
            var parameters = new SqlParameter[] 
            {
                new SqlParameter("@FDate", fDate),
                new SqlParameter("@TDate", tDate)
            };

            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, parameters);

            BindData();

            ShowAlert(string.Format("All documents from {0} to {1} are restored.", dateFrom.ToString("dd/MM/yyyy"), dateTo.ToString("dd/MM/yyyy")));
        }



    }
}