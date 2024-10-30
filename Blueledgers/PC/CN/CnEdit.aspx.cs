using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;
using BlueLedger.PL.BaseClass;
using System.Text;
using System.Drawing;


namespace BlueLedger.PL.PC.CN
{
    public partial class CnEdit : BasePage
    {
        private readonly int CN_QTY_INDEX = 0;
        private readonly int CN_AMT_INDEX = 1;

        #region "Attribute"

        private readonly Blue.BL.dbo.Bu _bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        #endregion

        #region --URL Parameters--

        protected string _BuCode { get { return Request.Params["BuCode"].ToString() ?? ""; } }
        protected string _VID { get { return Request.Params["VID"] == null ? "" : Request.Params["VID"].ToString() ?? ""; } }
        protected string _ID { get { return Request.Params["ID"] == null ? "" : Request.Params["ID"].ToString() ?? ""; } }
        protected string _MODE { get { return Request.Params["Mode"] == null ? "" : Request.Params["Mode"].ToString().ToUpper(); } }

        #endregion

        protected DefaultValues _default
        {
            get { return ViewState["DefaultValues"] as DefaultValues; }
            set { ViewState["DefaultValues"] = value; }
        }

        protected DataTable _dtCn
        {
            //get { return ViewState["dtRec"] as DataTable; }
            //set { ViewState["dtRec"] = value; }
            get { return ViewState["_dtCn" + _ID] as DataTable; }
            set { ViewState["_dtCn" + _ID] = value; }

        }

        protected DataTable _dtCnDt
        {
            //get { return ViewState["dtRecDt"] as DataTable; }
            //set { ViewState["dtRecDt"] = value; }
            get { return ViewState["_dtCnDt" + _ID] as DataTable; }
            set { ViewState["_dtCnDt" + _ID] = value; }

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            var currency = _config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            var digitAmt = _config.GetValue("APP", "Default", "DigitAmt", hf_ConnStr.Value);
            var digitQty = _config.GetValue("APP", "Default", "DigitQty", hf_ConnStr.Value);
            var taxRate = _config.GetValue("APP", "Default", "TaxRate", hf_ConnStr.Value);
            var costMethod = _config.GetValue("IN", "SYS", "COST", hf_ConnStr.Value);

            _default = new DefaultValues
            {
                Currency = currency,
                DigitAmt = string.IsNullOrEmpty(digitAmt) ? 2 : Convert.ToInt32(digitAmt),
                DigitQty = string.IsNullOrEmpty(digitQty) ? 2 : Convert.ToInt32(digitQty),
                TaxRate = string.IsNullOrEmpty(taxRate) ? 0 : Convert.ToDecimal(taxRate),
                CostMethod = costMethod.ToUpper()
            };

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }

        }

        private void Page_Retrieve()
        {
            LoadCurrencyRate(_default.Currency, DateTime.Today);
            LoadVendor();

            _dtCn = _bu.DbExecuteQuery("SELECT * FROM PC.Cn WHERE CnNo=@id", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);

            if (_MODE == "NEW")
            {
                var drNew = _dtCn.NewRow();

                drNew["DocDate"] = DateTime.Today;
                drNew["CnDate"] = DateTime.Today;
                drNew["VendorCode"] = "";
                drNew["CurrencyCode"] = _default.Currency;
                drNew["ExRateAudit"] = 1;
                drNew["DocStatus"] = "New";
                drNew["ExportStatus"] = false;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedBy"] = LoginInfo.LoginName;

                _dtCn.Rows.Add(drNew);

            }
            else
            {
            }

            var dr = _dtCn.Rows[0];

            lbl_CnNo.Text = dr["CnNo"].ToString();
            de_CnDate.Date = Convert.ToDateTime(dr["CnDate"]);
            txt_DocNo.Text = dr["DocNo"].ToString();
            de_DocDate.Date = Convert.ToDateTime(dr["DocDate"]);


            ddl_Vendor.Value = dr["VendorCode"].ToString();
            ddl_Currency.Value = dr["CurrencyCode"].ToString();
            se_CurrencyRate.Value = Convert.ToDecimal(dr["ExRateAudit"]);
            lbl_Status.Text = dr["DocStatus"].ToString();

            txt_Desc.Text = dr["Description"].ToString();

            var query = @"
SELECT
	cndt.*,

	l.LocationName,
	p.ProductDesc1,
	p.ProductDesc2
FROM
	PC.CnDt
	LEFT JOIN [IN].StoreLocation l ON l.LocationCode=cndt.Location
	LEFT JOIN [IN].Product p ON p.ProductCode=cndt.ProductCode
WHERE
	CnNo = @id";
            _dtCnDt = _bu.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);


            ddl_Vendor.Enabled = _dtCnDt.Rows.Count == 0;

        }


        #region --Event(s)--

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                Response.Redirect("Cn.aspx?ID=" + lbl_CnNo.Text + "&BuCode=" + LoginInfo.BuInfo.BuCode);
            }
            else
            {
                Response.Redirect("CnList.aspx");
            }

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            SaveAndCommit(false);
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            SaveAndCommit(true);
        }


        // Header
        protected void ddl_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var code = ddl.Value;

            _dtCn.Rows[0]["VendorCode"] = code;
        }

        protected void ddl_Currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var code = ddl.Text;


            if (code == _default.Currency)
            {
                se_CurrencyRate.Text = "1.00";
                se_CurrencyRate.Enabled = false;
            }
            else
            {

                var date = de_CnDate.Date.Date;
                var query = "SELECT TOP(1) CurrencyRate FROM [Ref].[CurrencyExchange] WHERE CurrencyCode=@code AND CAST(UpdatedDate AS DATE) <= CAST(@date as DATE) ORDER BY UpdatedDate DESC";

                var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery(query, new SqlParameter[]
            {
                new SqlParameter("Code", code),
                new SqlParameter("Date", date)
            });

                var rate = 0m;

                if (dt != null && dt.Rows.Count > 0)
                {
                    rate = Convert.ToDecimal(dt.Rows[0][0]);
                }

                se_CurrencyRate.Value = rate;
                se_CurrencyRate.Enabled = true;

            }

            UpdateCurrencyRate((decimal)se_CurrencyRate.Value);
        }

        protected void se_CurrencyRate_NumberChanged(object sender, EventArgs e)
        {
            var item = sender as ASPxSpinEdit;
            var rate = Convert.ToDecimal(item.Value);

            UpdateCurrencyRate(rate);
        }

        // Detail
        protected void btn_AddItem_Click(object sender, EventArgs e)
        {
            var vendorCode = ddl_Vendor.Value == null ? "" : ddl_Vendor.Value.ToString();

            if (string.IsNullOrEmpty(vendorCode))
            {
                ShowWarning("Please select a vendor");

                return;
            }

            AddItem(vendorCode);
        }
        
        protected void btn_DeleteItem_Click(object sender, EventArgs e)
        {
        }

        // gv_Deatail

        protected void gv_Detail_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;


            }
        }

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;

            //for (var i = _dtRecDt.Rows.Count - 1; i >= 0; i--)
            //{
            //    var dr = _dtRecDt.Rows[i];
            //    if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
            //        dr.Delete();
            //}

            gv.DataSource = _dtCnDt;
            gv.EditIndex = -1;
            gv.DataBind();

            //SetEditItem(false);
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditRowStyle.BackColor = Color.FromArgb(254, 249, 231);
            gv.EditIndex = e.NewEditIndex;
            gv.DataSource = _dtCnDt;
            gv.DataBind();

            var row = gv.Rows[e.NewEditIndex];

            var Img_Btn = row.FindControl("Img_Btn") as ImageButton;
            var ddl_Location = row.FindControl("ddl_Location") as ASPxComboBox;

            Img_Btn.Visible = false;

            //SetEditItem(true);
        }

        protected void gv_Detail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gv = sender as GridView;
            var row = gv.Rows[e.RowIndex];

            DataRow dr = _dtCnDt.Rows[e.RowIndex];

            

            gv.EditIndex = -1;
            gv.DataSource = _dtCnDt;
            gv.DataBind();

            //SetEditItem(false);
        }

        protected void gv_Detail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gv = sender as GridView;

            //var hf_RecDtNo = gv.Rows[e.RowIndex].FindControl("hf_RecDtNo") as HiddenField;
            //var recDtNo = Convert.ToInt32(hf_RecDtNo.Value);
            //var item = _dtCnDt.AsEnumerable().FirstOrDefault(x => x.Field<int>("CnDtNo") == recDtNo);

            //if (item != null)
            //    item.Delete();

            gv.DataSource = _dtCnDt;
            gv.DataBind();
        }


        #endregion

        // Method(s)
        private void ShowWarning(string text)
        {
            pop_Alert.HeaderText = "Warning";

            lbl_Pop_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void ShowInfo(string text)
        {
            pop_Alert.HeaderText = "Information";

            lbl_Pop_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }


        private void SaveAndCommit(bool isCommit)
        {
            if (ddl_Vendor.SelectedIndex < 0)
            {
                ShowWarning("Vendor is required.");

                return;
            }

            if (se_CurrencyRate.Number <= 0)
            {
                ShowWarning("Invalid currency rate.");

                return;
            }


        }

        private void UpdateCurrencyRate(decimal value)
        {
        }

        private void LoadCurrencyRate(string currencyCode, DateTime date)
        {
            var dt = new Helpers.SQL(hf_ConnStr.Value).ExecuteQuery("SELECT CurrencyCode as [Value], CurrencyCode as [Text] FROM [Ref].Currency WHERE IsActived=1 ORDER BY CurrencyCode");

            var items = dt.AsEnumerable()
                .Select(x => new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text"),
                    Selected = x.Field<string>("Value") == currencyCode
                })
                .ToArray();


            ddl_Currency.Items.Clear();
            ddl_Currency.Items.AddRange(items);
        }

        private void LoadVendor()
        {
            var query = "SELECT VendorCode as [Value], CONCAT(VendorCode, ' : ', [Name]) as [Text] FROM AP.Vendor WHERE IsActive=1 ORDER BY VendorCode";
            var dt = _bu.DbExecuteQuery(query, null, hf_ConnStr.Value);

            ddl_Vendor.Items.Clear();
            ddl_Vendor.Items.AddRange(dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                }).ToArray());
        }

        private void AddItem(string vendorCode)
        {
            var dr = _dtCnDt.Rows.Count == 0 ? null : _dtCnDt.Rows[_dtCnDt.Rows.Count-1];

            var cnType = dr == null ? "Q" : dr["CnType"].ToString();
            var recNo = dr == null ? "" : dr["RecNo"].ToString();
            var cnDtNo = dr == null ? 1 : Convert.ToInt32(dr["CnDtNo"]) + 1;

            var drNew = _dtCnDt.NewRow();


            drNew["CnNo"] = _ID;
            drNew["CnNo"] = cnDtNo;
            drNew["RecNo"] = recNo;
            drNew["RecQty"] = 0;
            drNew["FocQty"] = 0;
            drNew["Price"] = 0;
            drNew["TaxType"] = "N";
            drNew["TaxRate"] = 0;
            drNew["CurrNetAmt"] = 0;
            drNew["CurrTaxAmt"] = 0;
            drNew["CurrTotalAmt"] = 0;
            drNew["NetAmt"] = 0;
            drNew["TaxAmt"] = 0;
            drNew["TotalAmt"] = 0;






            _dtCnDt.Rows.Add(drNew);

            gv_Detail.DataSource = _dtCnDt;
            gv_Detail.DataBind();

        }

        // Classes
        public class DefaultValues
        {
            public string Currency { get; set; }
            public int DigitAmt { get; set; }
            public int DigitQty { get; set; }
            public decimal TaxRate { get; set; }
            public string CostMethod { get; set; }
        }

    }
}