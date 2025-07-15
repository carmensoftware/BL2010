using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Drawing;
using System.Text;
using System.Collections.Generic;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class ExportPosting : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private string prevDocNo = "";
        private bool _toggle_color = false;
        private DataTable _dtAccMapDesc;


        #region "Attributes"

        private DataTable _dtExport
        {
            get { return ViewState["_dtExport"] as DataTable; }
            set { ViewState["_dtExport"] = value; }
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


            _dtAccMapDesc = config.DbExecuteQuery("SELECT TOP(1) DescA1, DescA2, DescA3 FROM [ADMIN].AccountMappView WHERE PostType='AP'", null, LoginInfo.ConnStr);

        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            var dateFrom = de_FromDate.Date;
            var dateTo = de_ToDate.Date;

            Preview(dateFrom, dateTo);
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            var dateFrom = de_FromDate.Date;
            var dateTo = de_ToDate.Date;

            Export(dateFrom, dateTo);
        }


        protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = (sender as DropDownList).SelectedItem.Value.ToString();

            switch (value)
            {
                case "0":
                    var rowNoExport = _dtExport.Select("ExportStatus=0");

                    gv_Data.DataSource = rowNoExport.Length > 0 ? rowNoExport.CopyToDataTable() : null;
                    gv_Data.DataBind();
                    break;
                case "1":
                    var rowExport = _dtExport.Select("ExportStatus=1");

                    gv_Data.DataSource = rowExport.Length > 0 ? rowExport.CopyToDataTable() : null;
                    gv_Data.DataBind();
                    break;
                default:
                    gv_Data.DataSource = _dtExport;
                    gv_Data.DataBind();

                    break;
            }
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

        #endregion

        private void ShowAlert(string text)
        {
            lbl_Alert.Text = text;
            pop_Alert.ShowOnPageLoad = true;
        }

        private void Preview(DateTime dateFrom, DateTime dateTo)
        {
            var fDate = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var tDate = dateTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var sql = new Helpers.SQL(LoginInfo.ConnStr);
            var query = "EXEC [Tool].ExportToAP_SunSystems @FDate=@FDate, @TDate=@TDate, @IsExport=0";
            var parameters = new SqlParameter[] 
            {
                new SqlParameter("@FDate", fDate),
                new SqlParameter("@TDate", tDate)
            };

            var dt = sql.ExecuteQuery(query, parameters);

            _dtExport = dt;

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
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Warning", "alert('No data to export.');", true);
                ShowAlert("No data found to export.");

                return;
            }

            if (dt != null && dt.Rows.Count == 1)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Warning", "alert('Some transactions have not been assign the mapping values.');", true);

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