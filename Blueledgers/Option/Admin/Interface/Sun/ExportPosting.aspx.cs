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
    public partial class ExportPosting : BasePage
    {
        const string moduleID = "2.9.31";

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.RolePermission permission = new Blue.BL.ADMIN.RolePermission();

        private string prevDocNo = "";
        private bool _toggle_color = false;

        //private AccountMappView _accMapView = new AccountMappView();

        //private DataTable _dtAccMapDesc;




        #region "Attributes"
        private AccMapView _accMapView
        {
            get { return ViewState["_accMapView"] as AccMapView; }
            set { ViewState["_accMapView"] = value; }
        }

        protected bool _hasPermissionEdit
        {
            get
            {
                var pagePermiss = permission.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

                return pagePermiss >= 3;
            }
        }


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

            btn_Config.Visible = _hasPermissionEdit;

            //_dtAccMapDesc = config.DbExecuteQuery("SELECT TOP(1) A1, A2, A3, DescA1, DescA2, DescA3 FROM [ADMIN].AccountMappView WHERE PostType='AP'", null, LoginInfo.ConnStr);
            GetAccountMappView();
        }


        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            var dateFrom = de_FromDate.Date;
            var dateTo = de_ToDate.Date;

            Export(dateFrom, dateTo);
        }

        protected void btn_Config_Click(object sender, EventArgs e)
        {
            SetConfig();

            pop_Config.ShowOnPageLoad = true;
        }

        protected void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();

            pop_Config.ShowOnPageLoad = false;
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region --header--
            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (e.Row.FindControl("lbl_A1_Header") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A1_Header") as Label;

                    lbl.Text = _accMapView.DescA1;
                }

                if (e.Row.FindControl("lbl_A2_Header") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2_Header") as Label;

                    lbl.Text = _accMapView.DescA2;
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
                        lbl.ToolTip = string.Format("Set at '{0}' in Account Mapping.", _accMapView.DescA3);
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

                }

                if (e.Row.FindControl("lbl_A2") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2") as Label;

                    var value = a2;


                    lbl.Text = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? "Not Set" : value;
                    lbl.ForeColor = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? Color.Tomato : Color.Black;
                    lbl.Font.Bold = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? false : true;


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
            BindData();
        }

        #endregion

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void GetAccountMappView()
        {
            if (_accMapView == null)
                _accMapView = new AccMapView();


            var dt = config.DbExecuteQuery("SELECT TOP(1) A1, A2, A3, DescA1, DescA2, DescA3 FROM [ADMIN].AccountMappView WHERE PostType='AP'", null, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                _accMapView.A1 = Convert.ToBoolean(dr["A1"]);
                _accMapView.A2 = Convert.ToBoolean(dr["A2"]);
                _accMapView.A3 = Convert.ToBoolean(dr["A3"]);

                _accMapView.DescA1 = dr["DescA1"].ToString();
                _accMapView.DescA2 = dr["DescA2"].ToString();
                _accMapView.DescA3 = dr["DescA3"].ToString();
            }

            //lbl_Title.Text = _accMapView.A2.ToString();
        }

        private SunConfig GetConfig()
        {
            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = @"
DECLARE @doc XML = (SELECT TOP(1) [Value] FROM APP.Config WHERE [Module]='APP' AND SubModule='INTF' AND [Key]='SunSystems')

DECLARE @Version nvarchar(10) =  @doc.value('(/Config/Version)[1]', 'varchar(10)')
DECLARE @JournalType nvarchar(5) =  @doc.value('(/Config/JournalType)[1]', 'varchar(5)')
DECLARE @TaxAccountType nvarchar(20) = @doc.value('(/Config/TaxAccountCode/@Type)[1]', 'varchar(20)')
DECLARE @TaxAccountCode nvarchar(20) = @doc.value('(/Config/TaxAccountCode)[1]', 'varchar(20)')

DECLARE @UseCommitDate nvarchar(5) = @doc.value('(/Config/UseCommitDate)[1]', 'varchar(5)')
DECLARE @SingleExport nvarchar(5) = @doc.value('(/Config/SingleExport)[1]', 'varchar(5)')

DECLARE @UseA1 nvarchar(5) = @doc.value('(/Config/UseA1)[1]', 'varchar(5)')
DECLARE @UseA2 nvarchar(5) = @doc.value('(/Config/UseA2)[1]', 'varchar(5)')
DECLARE @UseA3 nvarchar(5) = @doc.value('(/Config/UseA3)[1]', 'varchar(5)')


SELECT
	ISNULL(@Version,'42601') as [Version], 
	ISNULL(@JournalType,'MCINV') as [JournalType], 
	
	ISNULL(@TaxAccountType,'PRODUCT') as TaxAccountType,
	@TaxAccountCode as TaxAccountCode,

	ISNULL(@UseA1,'true') as UseA1,
	ISNULL(@UseA2,'true') as UseA2,
	ISNULL(@UseA3,'true') as UseA3,

	ISNULL(@UseCommitDate,'true') as UseCommitDate,
	ISNULL(@SingleExport,'true') as [SingleExport]
";
            var dt = sql.ExecuteQuery(query);

            var config = new SunConfig();

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                config.Version = dr["Version"].ToString();
                config.JournalType = dr["JournalType"].ToString();

                config.TaxAccountType = dr["TaxAccountType"].ToString();
                config.TaxAccountCode = dr["TaxAccountCode"].ToString();

                config.UseCommitDate = Convert.ToBoolean(dr["UseCommitDate"]);
                config.SingleExport = Convert.ToBoolean(dr["SingleExport"]);

                config.UseA1 = Convert.ToBoolean(dr["UseA1"]);
                config.UseA2 = Convert.ToBoolean(dr["UseA2"]);
                config.UseA3 = Convert.ToBoolean(dr["UseA3"]);

            }

            return config;
        }

        private void SetConfig()
        {
            var config = GetConfig();

            ddl_Config_Version.SelectedValue = config.Version;
            txt_Config_JournalType.Text = config.JournalType;
            ddl_Config_TaxAccountType.SelectedValue = config.TaxAccountType;
            txt_Config_TaxAccountCode.Text = config.TaxAccountCode;

            ddl_UseA1.SelectedValue = config.UseA1 ? "true" : "false";
            ddl_UseA2.SelectedValue = config.UseA2 ? "true" : "false";
            ddl_UseA3.SelectedValue = config.UseA3 ? "true" : "false";

            ddl_Config_UseCommitDate.SelectedValue = config.UseCommitDate ? "true" : "false";
            ddl_Config_SingleExport.SelectedValue = config.SingleExport ? "true" : "false";

        }

        private void SaveConfig()
        {
            var version = ddl_Config_Version.SelectedItem.Value.ToString();
            var journalType = txt_Config_JournalType.Text.Trim();
            var taxAccType = ddl_Config_TaxAccountType.SelectedItem.Value.ToString();
            var taxAccCode = txt_Config_TaxAccountCode.Text.Trim();
            var singleExport = ddl_Config_SingleExport.SelectedItem.Value.ToString();

            var useA1 = ddl_UseA1.SelectedItem.Value.ToString();
            var useA2 = ddl_UseA2.SelectedItem.Value.ToString();
            var useA3 = ddl_UseA3.SelectedItem.Value.ToString();


            if (string.IsNullOrEmpty(taxAccType) && string.IsNullOrEmpty(taxAccCode)) // Fix code
            {
                ShowAlert("Please set Tax Account Code.");

                return;
            }

            var config = new StringBuilder();

            config.Append("<Config>");
            config.Append(string.Format("<Version>{0}</Version>", version));
            config.Append(string.Format("<JournalType>{0}</JournalType>", journalType));
            config.Append(string.Format("<TaxAccountCode Type=\"{0}\">{1}</TaxAccountCode>", taxAccType, taxAccCode));
            config.Append(string.Format("<SingleExport>{0}</SingleExport>", singleExport));

            config.Append(string.Format("<UseA1>{0}</UseA1>", useA1));
            config.Append(string.Format("<UseA2>{0}</UseA2>", useA2));
            config.Append(string.Format("<UseA3>{0}</UseA3>", useA3));

            config.Append("</Config>");

            var query = @"
DELETE FROM APP.Config WHERE [Module]='APP' AND SubModule='INTF' AND [Key]='SunSystems'

INSERT INTO APP.Config (Module, SubModule, [Key], [Value], UpdatedBy, UpdatedDate)
VALUES ('APP','INTF','SunSystems', @Value, 'SYSTEM',GETDATE())";

            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@Value", config.ToString()) });
        }

        private void BindData()
        {
            var fDate = de_FromDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var tDate = de_ToDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var status = ddl_View.SelectedItem.Value.ToString();

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = "EXEC [Tool].ExportToAP_SunSystems @FDate=@FDate, @TDate=@TDate, @IsExport=0, @Status=@Status";
            var parameters = new SqlParameter[] 
            {
                new SqlParameter("@FDate", fDate),
                new SqlParameter("@TDate", tDate),
                new SqlParameter("@Status", status),

            };

            var dt = sql.ExecuteQuery(query, parameters);


            bool a1 = _accMapView.A1;
            bool a2 = _accMapView.A2;

            gv_Data.Columns[11].Visible = a1;
            gv_Data.Columns[12].Visible = a2;

            //lbl_Title.Text = a2.ToString();

            gv_Data.DataSource = dt;
            gv_Data.DataBind();
        }

        private void Export(DateTime dateFrom, DateTime dateTo)
        {
            var fDate = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var tDate = dateTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = "EXEC [Tool].ExportToAP_SunSystems @FDate=@FDate, @TDate=@TDate, @IsExport=1";
            var parameters = new SqlParameter[] 
            {
                new SqlParameter("@FDate", fDate),
                new SqlParameter("@TDate", tDate)
            };

            var dt = sql.ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count == 0)
            {
                ShowAlert("No data found to export.");

                return;
            }

            if (dt != null && dt.Rows.Count == 1)
            {
                ShowAlert(dt.Rows[0][0].ToString());

                return;
            }



            var text = "";

            foreach (DataRow dr in dt.Rows)
            {
                text += dr[0].ToString() + "\r\n";
            }

            //Download the Text file.
            var fileName = "AP" + dateFrom.ToString("yyyyMMdd") + "-" + dateTo.ToString("yyyyMMdd") + ".txt";

            SaveAsText(text, fileName);
        }

        private void SaveAsText(string text, string filename)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            Response.ContentType = "text/txt";
            Response.Write(text);
            Response.Flush();
            Response.End();
        }



        [Serializable]
        public class AccMapView
        {
            public AccMapView()
            {
                A1 = true;
                A2 = false;
                A3 = true;

                DescA1 = "";
                DescA2 = "";
                DescA3 = "";
            }

            public bool A1 { get; set; }
            public bool A2 { get; set; }
            public bool A3 { get; set; }

            public string DescA1 { get; set; }
            public string DescA2 { get; set; }
            public string DescA3 { get; set; }

        }

        public class SunConfig
        {
            public SunConfig()
            {
                Version = "42601";
                JournalType = "MCINV";
                TaxAccountType = "PRODUCT";
                TaxAccountCode = "";

                SingleExport = true;
                UseCommitDate = true;

                UseA1 = true;
                UseA2 = true;
                UseA3 = true;
            }

            public string Version { get; set; }
            public string JournalType { get; set; }
            public string TaxAccountType { get; set; }
            public string TaxAccountCode { get; set; }
            public bool SingleExport { get; set; }
            public bool UseCommitDate { get; set; }

            public bool UseA1 { get; set; }
            public bool UseA2 { get; set; }
            public bool UseA3 { get; set; }
        }
    }

}