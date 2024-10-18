using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using System.Drawing;
using System.Linq;

namespace BlueLedger.PL.PC.EOP
{
    public partial class EOPEdit : BasePage
    {
        #region --Attributes--
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.APP.Config _config = new Blue.BL.APP.Config();

        private string _mode { get { return Request.QueryString["MODE"] == null ? "" : Request.QueryString["MODE"].ToString(); } }

        private string _vid { get { return Request.QueryString["VID"] == null ? "" : Request.QueryString["VID"].ToString(); } }

        private string _eopId { get { return Request.QueryString["ID"].ToString() ?? "0"; } }

        private int _digitQty { get { return (int)ViewState["_digitQty"]; } set { ViewState["_digitQty"] = value; } }

        protected DataTable _dtEop
        {
            get { return ViewState["_dtEop"] as DataTable; }
            set { ViewState["_dtEop"] = value; }
        }

        protected DataTable _dtEopDt
        {
            get { return ViewState["_dtEopDt"] as DataTable; }
            set { ViewState["_dtEopDt"] = value; }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                if (_mode.ToUpper() == "NEW")
                {
                    ShowCreateDialog();
                }
                else
                    Page_Retrieve();
            }
            else
            {
            }
        }

        private void Page_Retrieve()
        {
            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = "";

            // IN.Eop
            query = "SELECT eop.*, l.LocationName FROM [IN].Eop JOIN [IN].StoreLocation l ON l.LocationCode=eop.StoreId WHERE EopId=@id";
            _dtEop = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", _eopId) });

            // IN.EopDt
            query = "SELECT * FROM [IN].EopDt WHERE EopId=@id ORDER BY EopDtId";
            _dtEopDt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", _eopId) });
            _dtEopDt.PrimaryKey = new DataColumn[] { _dtEopDt.Columns["EopDtId"] };

            // Digit Qty
            var digitQty = _config.GetValue("APP", "Default", "DigitQty", LoginInfo.ConnStr);

            _digitQty = string.IsNullOrEmpty(digitQty) ? 2 : Convert.ToInt32(digitQty);

            Page_Setting();
        }

        private void Page_Setting()
        {
            // Header
            if (_dtEop.Rows.Count > 0)
            {
                var dr = _dtEop.Rows[0];

                txt_Location.Text = string.Format("{0} : {1}", dr["StoreId"].ToString(), dr["LocationName"].ToString());
                de_Date.Date = Convert.ToDateTime(dr["Date"]);

                lbl_EndDate.Text = Convert.ToDateTime(dr["EndDate"]).ToString("dd/MM/yyyy");
                lbl_Status.Text = dr["Status"].ToString();
            }

            gv_EopDt.DataSource = _dtEopDt;
            gv_EopDt.DataBind();
        }

        // Action(s)
        protected void btn_New_Create_Click(object sender, EventArgs e)
        {
            if (ddl_Location_New.SelectedItem != null)
            {

                pop_New.ShowOnPageLoad = false;

                var locationCode = ddl_Location_New.SelectedItem.Value.ToString();

                CreateNew(locationCode);
            }
            else
                ShowAlert("Please select a location.");

        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            //Session["AccountMappPrint"] = GetData1(true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "print", string.Format("<script>window.open('EopPrint.aspx?id={0}', 'Print');</script>", _eopId), false);

        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("EOP.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + _eopId + "&VID=" + _vid);
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            var startDate = Convert.ToDateTime(_dtEop.Rows[0]["StartDate"]);



            if (de_Date.Date < startDate)
            {
                ShowAlert("Date must be in open period.");
                return;
            }

            SaveWithCommit(false);
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            if (FoundNotAssinged())
            {
                ShowAlert("Some products are not been assigned quantity.");

                return;
            }

            SaveWithCommit(true);
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            lbl_Confirm_Delete.Text = "Do you want to delete this pyhsical count of " + txt_Location.Text + "?";
            pop_Confirm_Delete.ShowOnPageLoad = true;
        }

        protected void btn_Confirm_Delete_Yes_Click(object sender, EventArgs e)
        {
            var query = "DELETE FROM [IN].EopDt WHERE EopId=@id; DELETE FROM [IN].Eop WHERE EopId=@id; ";
            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("id", _eopId) });

            Response.Redirect("EOPList.aspx");
        }


        // Empty Item
        protected void chk_OnlyEmpty_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;

            foreach (GridViewRow row in gv_EopDt.Rows)
            {
                var se_Qty = row.FindControl("se_Qty") as ASPxSpinEdit;

                if (chk.Checked)
                    row.Visible = string.IsNullOrEmpty(se_Qty.Text);
                else
                    row.Visible = true;

            }

        }

        protected void btn_SetZero_Click(object sender, EventArgs e)
        {
            pop_Confirm_SetZero.ShowOnPageLoad = true;
        }

        protected void btn_Yes_Confirm_SetZero_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gv_EopDt.Rows)
            {
                var se_Qty = row.FindControl("se_Qty") as ASPxSpinEdit;

                if (string.IsNullOrEmpty(se_Qty.Text))
                {
                    se_Qty.Value = 0;
                    SetQtyColor(se_Qty);
                }
            }

            pop_Confirm_SetZero.ShowOnPageLoad = false;

        }

        // GridView
        protected void gv_EopDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("hf_EopId") != null)
                {
                    var hf = e.Row.FindControl("hf_EopId") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "EopId").ToString();
                }

                if (e.Row.FindControl("hf_EopDtId") != null)
                {
                    var hf = e.Row.FindControl("hf_EopDtId") as HiddenField;

                    hf.Value = DataBinder.Eval(e.Row.DataItem, "EopDtId").ToString();
                }


                if (e.Row.FindControl("se_Qty") != null)
                {
                    var se = e.Row.FindControl("se_Qty") as ASPxSpinEdit;

                    se.Value = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                    se.DecimalPlaces = _digitQty;
                    se.DisplayFormatString = "N" + _digitQty.ToString();

                    SetQtyColor(se);

                }
            }
        }

        protected void se_Qty_ValueChanged(object sender, EventArgs e)
        {
            var se = sender as ASPxSpinEdit;
            var row = se.NamingContainer;

            //var btn_SaveItem = row.FindControl("btn_SaveItem") as ImageButton;
            var btn_SaveItem = row.FindControl("btn_SaveItem") as Button;

            if (btn_SaveItem != null)
            {
                btn_SaveItem.Visible = true;
            }
        }

        protected void btn_SaveItem_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var row = btn.NamingContainer;

            var hf_EopId = row.FindControl("hf_EopId") as HiddenField;
            var hf_EopDtId = row.FindControl("hf_EopDtId") as HiddenField;
            var se_Qty = row.FindControl("se_Qty") as ASPxSpinEdit;

            var eopId = hf_EopId.Value;
            var eopDtId = hf_EopDtId.Value;

            if (string.IsNullOrEmpty(se_Qty.Text))
            {
                var querey = "UPDATE [IN].EopDt SET Qty=NULL WHERE EopId=@eopId AND EopDtId=@eopDtId";

                new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(querey, new SqlParameter[]
                {
                    new SqlParameter("eopId", eopId),
                    new SqlParameter("eopDtId", eopDtId)
                });

                SetQtyColor(se_Qty);

            }
            else
            {

                var querey = "UPDATE [IN].EopDt SET Qty=@qty WHERE EopId=@eopId AND EopDtId=@eopDtId";

                new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(querey, new SqlParameter[]
                {
                    new SqlParameter("eopId", eopId),
                    new SqlParameter("eopDtId", eopDtId),
                    new SqlParameter("qty", se_Qty.Value)
                });
                SetQtyColor(se_Qty);

            }

            btn.Visible = false;
        }

        #region --Method(s)--

        private void ShowAlert(string text)
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

        private void SetQtyColor(ASPxSpinEdit se)
        {
            if (string.IsNullOrEmpty(se.Text))
            {
                se.BackColor = Color.Yellow;
            }
            else
            {
                se.BackColor = Color.White;
            }
        }

        private void UpdateQty(string eopId, string eopDtId, string qty)
        {
            try
            {

                if (string.IsNullOrEmpty(qty))
                {
                    var querey = "UPDATE [IN].EopDt SET Qty=NULL WHERE EopId=@eopId AND EopDtId=@eopDtId";

                    new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(querey, new SqlParameter[]
                    {
                        new SqlParameter("eopId", eopId),
                        new SqlParameter("eopDtId", eopDtId)
                    });

                }
                else
                {
                    var querey = "UPDATE [IN].EopDt SET Qty=@qty WHERE EopId=@eopId AND EopDtId=@eopDtId";

                    new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(querey, new SqlParameter[]
                    {
                        new SqlParameter("eopId", eopId),
                        new SqlParameter("eopDtId", eopDtId),
                        new SqlParameter("qty", Convert.ToDecimal(qty))
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ShowCreateDialog()
        {
            #region --Querey--
            var query = @"

DECLARE @StartDate DATE = (SELECT TOP(1) StartDate FROM [IN].Period WHERE IsClose = 0 ORDER BY [StartDate])

;WITH
loc AS(
	SELECT 
		StoreId 
	FROM 
		[IN].Eop 
	WHERE 
		StartDate =@StartDate
)
SELECT
	l.LocationCode,
	l.LocationName
FROM
	[IN].StoreLocation l
	LEFT JOIN loc ON loc.StoreId=l.LocationCode 
WHERE
	l.EOP = 1
    AND l.IsActive=1
	AND loc.StoreId IS NULL
ORDER BY
    l.LocationCode";
            #endregion

            var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                var items = dt.AsEnumerable()
                    .Select(x => new ListEditItem
                    {
                        Text = string.Format("{0} : {1}", x.Field<string>("LocationCode"), x.Field<string>("LocationName")),
                        Value = x.Field<string>("LocationCode")
                    })
                    .ToArray();

                ddl_Location_New.Items.Clear();
                ddl_Location_New.Items.AddRange(items);
            }

            pop_New.ShowOnPageLoad = true;
        }

        private void CreateNew(string locationCode)
        {
            #region --query--
            var query = @"
DECLARE @StartDate DATE = (SELECT TOP(1) StartDate FROM [IN].Period WHERE IsClose = 0 ORDER BY [StartDate])
DECLARE @EndDate DATE = (SELECT TOP(1) EndDate FROM [IN].Period WHERE StartDate=@StartDate)

DECLARE @Id INT = ISNULL((SELECT MAX(EopId) FROM [IN].Eop),0) + 1
INSERT INTO [IN].Eop (EopId, StoreId, [Date], [Status], StartDate, EndDate, CreateDate, CreateBy, UpdateDate, UpdateBy)
SELECT
	@Id as EopId,
	@LocationCode as StoreId,
	CAST(GETDATE() AS DATE) [Date],
	'Printed' as [Status],
	@StartDate [StartDate],
	@EndDate [EndDate],
	GETDATE() as CreateDate,
	@LoginName as CreateBy,
	GETDATE() as UpdateDate,
	@LoginName as UpdateBy

INSERT INTO [IN].EopDt (EopDtId, EopId, ProductCode, ProductDesc1, ProductDesc2, InventoryUnit, Qty)
SELECT
	ROW_NUMBER() OVER(ORDER BY pl.ProductCode) as EopDtId,
	@Id as EopId,
	pl.ProductCode,
	p.ProductDesc1,
	p.ProductDesc2,
	p.InventoryUnit,
	NULL as Qty
FROM
	[IN].ProdLoc pl
	JOIN [IN].Product p ON p.ProductCode=pl.ProductCode
WHERE
	p.IsActive = 1
	AND pl.LocationCode = @LocationCode

SELECT @Id as EopId";
            #endregion

            var parameters = new SqlParameter[]
            {
                new SqlParameter("LocationCode", locationCode),
                new SqlParameter("LoginName", LoginInfo.LoginName)
            };

            try
            {
                var dt = new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, parameters);

                var id = dt.Rows[0][0].ToString();

                Response.Redirect("EOPEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + id + "&VID=" + _vid);



            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }
        }

        private void SaveWithCommit(bool isCommit)
        {
            try
            {
                foreach (GridViewRow row in gv_EopDt.Rows)
                {
                    var hf_EopId = row.FindControl("hf_EopId") as HiddenField;
                    var hf_EopDtId = row.FindControl("hf_EopDtId") as HiddenField;
                    var se_Qty = row.FindControl("se_Qty") as ASPxSpinEdit;

                    var eopId = hf_EopId.Value;
                    var eopDtId = hf_EopDtId.Value;

                    UpdateQty(eopId, eopDtId, se_Qty.Text);
                }

                if (isCommit)
                {
                    new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery("UPDATE [IN].Eop SET [Status]='Committed' WHERE EopId=@id", new SqlParameter[] { new SqlParameter("id", _eopId) });
                }


                Response.Redirect(string.Format("EOP.aspx?BuCode={0}&ID={1}&VID={2}", LoginInfo.BuInfo.BuCode, _eopId, _vid));

            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }

        }


        private bool FoundNotAssinged()
        {
            var result = false;

            foreach (GridViewRow row in gv_EopDt.Rows)
            {
                var se_Qty = row.FindControl("se_Qty") as ASPxSpinEdit;

                if (string.IsNullOrEmpty(se_Qty.Text))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}