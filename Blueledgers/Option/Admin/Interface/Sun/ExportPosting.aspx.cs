using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class ExportPosting : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly SqlCommand cmd = new SqlCommand();
        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly SqlDataAdapter da = new SqlDataAdapter();
        private readonly DataSet ds = new DataSet();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private SqlConnection con;
        private Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private DataSet dsExport = new DataSet();
        private DataSet dsRestore = new DataSet();
        private DataTable dtInterfaceExp = new DataTable();
        private DataTable dtInterfaceExpFalse = new DataTable();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        #endregion

        #region " Operations "

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                txt_FromDate.Date = ServerDateTime.Date;
                txt_ToDate.Date = ServerDateTime.Date;

                //btn_Export.Attributes.Add("Onclick", "return confirm('Confirm export?');");
            }
        }

        protected void grd_Preview2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellPos = 7;

                string accountCode = e.Row.Cells[cellPos].Text.Trim();

                if (accountCode == "&nbsp;" || accountCode == string.Empty)
                    e.Row.BackColor = System.Drawing.Color.Tomato;
                else
                    e.Row.Cells[cellPos].BackColor = System.Drawing.Color.Transparent;
            }


        }


        protected void btn_Export_Click(object sender, EventArgs e)
        {
            Export();
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            Preview();
        }

        private void Export()
        {

            con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();
            string sqlText = "SELECT COUNT(*) FROM [ADMIN].vExportToAP WHERE CAST(DocDate AS DATE) BETWEEN CAST(@FFDate AS DATE) AND CAST(@TTDate AS DATE) AND ISNULL(TheAccountNumber, '') = ''";
            cmd.Connection = con;
            cmd.CommandText = sqlText;
            cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Date.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Date.ToString("yyyy-MM-dd");

            Int32 count = (Int32)cmd.ExecuteScalar();

            con.Close();
            if (count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Warning", "alert('Some account code(s) are missing.');", true);
            }
            else
            {

                DataTable dtExport = accountMapp.GetInterfaceExport(txt_FromDate.Date.ToString("yyyy-MM-dd"), txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);


                if (dtExport.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Warning", "alert('No data found, file may was already exported or no data.');", true);
                }
                else
                {

                    string txt = string.Empty;

                    foreach (DataRow dr in dtExport.Rows)
                    {
                        txt = txt + dr[0].ToString() + "\r\n";
                    }

                    //Download the Text file.
                    string fileName = "AP" + Convert.ToDateTime(txt_FromDate.Value).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(txt_ToDate.Value).ToString("yyyyMMdd") + ".txt";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(txt);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        private void Preview()
        {
            con = new SqlConnection(LoginInfo.ConnStr);

            //rptquery = "Select * from vPre_Exp where DocDate >= @FFDate and DocDate <= @TTDate";
            string sqlText = "SELECT * FROM [ADMIN].vExportToAP WHERE CAST(DocDate AS DATE) BETWEEN CAST(@FFDate AS DATE) AND CAST(@TTDate AS DATE) Order By DocDate, DocNo, RecordType Desc";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlText;
            cmd.Connection = con;
            cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Date.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Date.ToString("yyyy-MM-dd");

            da.SelectCommand = cmd;
            da.Fill(ds, "ExportData");

            grd_Preview2.DataSource = ds;
            grd_Preview2.DataBind();

        }


        #endregion
    }

}