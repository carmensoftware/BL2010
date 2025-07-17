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

            btn_Config.Visible = _hasPermissionEdit;

            _dtAccMapDesc = config.DbExecuteQuery("SELECT TOP(1) DescA1, DescA2, DescA3 FROM [ADMIN].AccountMappView WHERE PostType='AP'", null, LoginInfo.ConnStr);

        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            //var dateFrom = de_FromDate.Date;
            //var dateTo = de_ToDate.Date;

            //Preview(dateFrom, dateTo);
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
            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = @"

DECLARE @doc XML = (SELECT TOP(1) [Value] FROM APP.Config WHERE [Module]='APP' AND SubModule='INTF' AND [Key]='SunSystems')
DECLARE @Version nvarchar(10) =  @doc.value('(/Config/Version)[1]', 'varchar(10)')
DECLARE @JournalType nvarchar(5) =  @doc.value('(/Config/JournalType)[1]', 'varchar(5)')
DECLARE @SingleExport nvarchar(5) = @doc.value('(/Config/SingleExport)[1]', 'varchar(5)')
DECLARE @TaxAccountType nvarchar(20) = @doc.value('(/Config/TaxAccountCode/@Type)[1]', 'varchar(20)')
DECLARE @TaxAccountCode nvarchar(20) = @doc.value('(/Config/TaxAccountCode)[1]', 'varchar(20)')

SELECT 
	@Version as [Version], 
	@JournalType as [JournalType], 
	@SingleExport as [SingleExport],
	@TaxAccountType as TaxAccountType,
	@TaxAccountCode as TaxAccountCode";
            var dt = sql.ExecuteQuery(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                ddl_Config_Version.SelectedValue = dr["Version"].ToString();
                txt_Config_JournalType.Text = dr["JournalType"].ToString();
                ddl_Config_TaxAccountType.SelectedValue = dr["TaxAccountType"].ToString();
                txt_Config_TaxAccountCode.Text = dr["TaxAccountCode"].ToString();

                ddl_Config_SingleExport.SelectedValue = string.IsNullOrEmpty(dr["SingleExport"].ToString()) ? "true" : dr["SingleExport"].ToString();

            }




            pop_Config.ShowOnPageLoad = true;
        }

        protected void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            var version = ddl_Config_Version.SelectedItem.Value.ToString();
            var journalType = txt_Config_JournalType.Text.Trim();
            var taxAccType = ddl_Config_TaxAccountType.SelectedItem.Value.ToString();
            var taxAccCode = txt_Config_TaxAccountCode.Text.Trim();
            var singleExport = ddl_Config_SingleExport.SelectedItem.Value.ToString();


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
            config.Append("</Config>");

            var query = @"
DELETE FROM APP.Config WHERE [Module]='APP' AND SubModule='INTF' AND [Key]='SunSystems'

INSERT INTO APP.Config (Module, SubModule, [Key], [Value], UpdatedBy, UpdatedDate)
VALUES ('APP','INTF','SunSystems', @Value, 'SYSTEM',GETDATE())";
            new Helpers.SQL(LoginInfo.ConnStr).ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@Value", config.ToString()) });

            pop_Config.ShowOnPageLoad = false;
        }

        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
            //var value = (sender as DropDownList).SelectedItem.Value.ToString();

            //switch (value)
            //{
            //    case "0":
            //        var rowNoExport = _dtExport.Select("ExportStatus=0");

            //        gv_Data.DataSource = rowNoExport.Length > 0 ? rowNoExport.CopyToDataTable() : null;
            //        gv_Data.DataBind();
            //        break;
            //    case "1":
            //        var rowExport = _dtExport.Select("ExportStatus=1");

            //        gv_Data.DataSource = rowExport.Length > 0 ? rowExport.CopyToDataTable() : null;
            //        gv_Data.DataBind();
            //        break;
            //    default:
            //        gv_Data.DataSource = _dtExport;
            //        gv_Data.DataBind();

            //        break;
            //}
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
                var locationCode = DataBinder.Eval(dataItem, "LocationCode").ToString();
                var itemGroupCode = DataBinder.Eval(dataItem, "ItemGroupCode").ToString();

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

                    if (string.IsNullOrEmpty(value) && docType == "ExpenseLine")
                    {
                        lbl.ToolTip = string.Format("Set '{0}' at location='{1}' and itemgroup='{2}' in Account Mapping.", _descA1, locationCode, itemGroupCode);
                    }
                }

                if (e.Row.FindControl("lbl_A2") != null)
                {
                    var lbl = e.Row.FindControl("lbl_A2") as Label;

                    var value = a2;

                    lbl.Text = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? "Not Set" : value;
                    lbl.ForeColor = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? Color.Tomato : Color.Black;
                    lbl.Font.Bold = string.IsNullOrEmpty(value) && docType == "ExpenseLine" ? false : true;

                    if (string.IsNullOrEmpty(value) && docType == "ExpenseLine")
                    {
                        lbl.ToolTip = string.Format("Set '{0}' at location='{1}' and itemgroup='{2}' in Account Mapping.", _descA2, locationCode, itemGroupCode);
                    }

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

            //_dtExport = dt;

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
                ShowAlert("Some transactions have not been assigned the values.");

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
    }

}