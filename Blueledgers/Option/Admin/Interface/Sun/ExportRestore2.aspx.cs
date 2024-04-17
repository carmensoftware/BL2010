using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class ExportRestore2 : BasePage
    {
        #region "Attributes"

        private readonly SqlCommand cmd = new SqlCommand();
        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly SqlDataAdapter da = new SqlDataAdapter();
        private readonly DataSet ds = new DataSet();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.IN.StoreGrp storeGrp = new Blue.BL.IN.StoreGrp();
        private Blue.BL.ADMIN.AccountMapp MyaccountMapp = new Blue.BL.ADMIN.AccountMapp();
        private Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private SqlConnection con;
        private DataSet dsExport = new DataSet();
        private DataSet dsRestore = new DataSet();
        private DataTable dtInterfaceExp = new DataTable();
        private DataTable dtInterfaceExpFalse = new DataTable();
        private string rptquery = string.Empty;

        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        #endregion

        #region " Operations "

        /// <summary>
        ///     Load page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                grd_load();
            }
        }

        /// <summary>
        /// </summary>
        private void grd_load()
        {
            //Default FFDate and TTDate
            txt_FromDate.Date = ServerDateTime.Date;
            txt_ToDate.Date = ServerDateTime.Date;
            ddl_StoreGrp.DataSource = storeGrp.GetList(LoginInfo.ConnStr);
            ddl_StoreGrp.DataBind();
        }

        /// <summary>
        ///     Click button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Restore_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="_exp"></param>
        private void isExp(string _exp)
        {
            //con = new SqlConnection(LoginInfo.ConnStr.ToString());
            //con.Open();

            //rptquery = "UPDATE PC.REC SET ExportStatus= 0 where ExportStatus= 1 and convert(varchar, RecDate, 103)>=@FDate and convert(varchar, RecDate, 103)<=@TDate";

            //cmd.CommandType = System.Data.CommandType.Text;
            //cmd.CommandText = rptquery;
            //cmd.Connection  = con;
            //cmd.Parameters.Add("FDate", System.Data.SqlDbType.VarChar).Value = txt_FromDate.Date.ToString("dd/MM/yyyy");
            //cmd.Parameters.Add("TDate", System.Data.SqlDbType.VarChar).Value = txt_ToDate.Date.ToString("dd/MM/yyyy");

            //da.SelectCommand = cmd;
            //cmd.ExecuteNonQuery();

            var dsUpdate = new DataSet();

            //Get data for update
            cn.GetCnbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 1, LoginInfo.ConnStr);
            rec.GetRecbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 1, LoginInfo.ConnStr);

            if (dsUpdate != null)
            {
                if (dsUpdate.Tables[cn.TableName].Rows.Count > 0)
                {
                    for (var a = 0; a < dsUpdate.Tables[cn.TableName].Rows.Count; a++)
                    {
                        dsUpdate.Tables[cn.TableName].Rows[a]["ExportStatus"] = 0;
                    }
                }

                if (dsUpdate.Tables[rec.TableName].Rows.Count > 0)
                {
                    for (var b = 0; b < dsUpdate.Tables[rec.TableName].Rows.Count; b++)
                    {
                        dsUpdate.Tables[rec.TableName].Rows[b]["ExportStatus"] = 0;
                    }
                }

                //Commit to database.
                var cnResult = cn.UpdateExportStatus(dsUpdate, LoginInfo.ConnStr);
                var recResult = rec.UpdateExportStatus(dsUpdate, LoginInfo.ConnStr);

                if (cnResult & recResult)
                {
                    //grd_Preview.DataSource = null;
                    //grd_Preview.DataBind();

                    grd_Preview2.DataSource = null;
                    grd_Preview2.DataBind();
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private DataTable GetDatatable()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("ID", typeof(string));
        //    table.Columns.Add("Version", typeof(string));
        //    table.Columns.Add("Period1", typeof(string));
        //    table.Columns.Add("Period2", typeof(string));
        //    table.Columns.Add("Period3", typeof(string));
        //    table.Columns.Add("Period4", typeof(string));
        //    table.Columns.Add("Period5", typeof(string));
        //    table.Columns.Add("Period6", typeof(string));

        //    //
        //    // Here we add five DataRows.
        //    //
        //    table.Rows.Add("1    ", "VERSION ","","                            42601","","","","");
        //    table.Rows.Add("2    ", "1       ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       jjjlkjlkj", "       A.S.A");
        //    table.Rows.Add("3    ", "141001  ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       jjjlkjlkj", "       Vat 7");
        //    table.Rows.Add("4    ", "1       ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       jjjlkjlkj", "       Beefe");
        //    table.Rows.Add("5    ", "1       ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       jjjlkjlkj", "       ALL S");
        //    table.Rows.Add("6    ", "1       ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       mlmlmalma", "       ALL S");
        //    table.Rows.Add("7    ", "1       ",   "            201000820100823", "  L", "            12840000C ", "     INVIN", "       mlmlmalma", "       Beef");
        //    return table;
        //}

        /// <summary>
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            var lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        /// <summary>
        ///     Click button preview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            GrdPreView();
        }

        /// <summary>
        /// </summary>
        private void GrdPreView()
        {
            con = new SqlConnection(LoginInfo.ConnStr);
            //rptquery        = "Select * from vPre_Restore where DocDate >= @FFDate and DocDate <= @TTDate";

            rptquery = "SELECT (SELECT TOP 1 BuCode FROM Admin.BU) AS BuCode, " +
                       "(SELECT TOP 1 [Name] FROM Admin.BU) AS BuName, " +
                       "'RCV' AS Doctype, " +
                       "CONVERT(DateTime, CONVERT(varchar, r.RecDate, 102)) AS DocDate, " +
                       "CONVERT(DateTime, CONVERT(varchar, r.InvoiceDate, 102)) AS InvoiceDate, " +
                       "r.RecNo AS DocNo, " +
                       "r.InvoiceNo, " +
                       "v.SunVendorCode AS VendorCode, " +
                       "v.Name, " +
                       "SUM(rd.NetAmt) AS RecordAmount, " +
                       "SUM(rd.TaxAmt) AS TaxAmt, " +
                       "SUM(rd.NetAmt) + SUM(rd.TaxAmt) AS TotalAmt " +
                       "FROM " +
                       "PC.REC r " +
                       "INNER JOIN PC.RECDt AS rd ON (rd.RecNo = r.RecNo AND " +
                       "rd.LocationCode in (SELECT LocationCode FROM [IN].StoreLocation WHERE StoreGrp = '" +
                       ddl_StoreGrp.SelectedItem.Value + "')) " +
                       "INNER JOIN AP. Vendor AS v ON v.VendorCode = r.VendorCode " +
                       "LEFT OUTER JOIN PC.Po AS po ON po.PoNo = rd.PoNo " +
                       "WHERE " +
                       "(rd.TotalAmt <> 0) " +
                       "AND (rd.Status = 'Committed') " +
                       "AND (r.IsCashConsign = 0) " +
                       "AND (r.ExportStatus = 1) " +
                       "AND (CONVERT(DateTime, CONVERT(varchar, r.RecDate, 102)) >= @FFDate AND CONVERT(DateTime, CONVERT(varchar, r.RecDate, 102)) <= @TTDate) " +
                       "GROUP BY " +
                       "r.VendorCode, " +
                       "v.Name, " +
                       "r.InvoiceNo, " +
                       "r.InvoiceDate, " +
                       "r.RecDate, " +
                       "r.RecNo, " +
                       "v.SunVendorCode, " +
                       "r.PoSource";

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = rptquery;
            cmd.Connection = con;
            //cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Value.ToString();
            //cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Value.ToString();
            cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Date.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Date.ToString("yyyy-MM-dd");
            da.SelectCommand = cmd;
            da.Fill(ds, "vPre_Restore");
            //grd_Preview.DataSource = ds;
            //grd_Preview.DataBind();

            grd_Preview2.DataSource = ds;
            grd_Preview2.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            //this.isExp("REST");

            // Update Export Status to 'false'
            RestoreExportStatus();

            GrdPreView();

            pop_Confrim.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Update export status to true/false
        /// </summary>
        /// <param name="exportStatus"></param>
        private void RestoreExportStatus()
        {
            var dbHandler = new Blue.DAL.DbHandler();
            var dbParams = new Blue.DAL.DbParameter[3];

            var cmd = "PC.RestoreExportStatus";

            dbParams[0] = new Blue.DAL.DbParameter("@FDate", txt_FromDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new Blue.DAL.DbParameter("@TDate", txt_ToDate.Date.ToString("yyyy-MM-dd"));
            dbParams[2] = new Blue.DAL.DbParameter("@StoreGrp", ddl_StoreGrp.SelectedItem.Value);

            dbHandler.DbExecuteNonQuery(cmd, dbParams, LoginInfo.ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = false;
        }

        #endregion

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "PRINT":
                        var objArrList = new ArrayList();
                        objArrList.Add(txt_FromDate.Value);
                        objArrList.Add(txt_ToDate.Value);
                        Session["s_arrNo"] = objArrList;

                        var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=170" + "&BuCode=" +
                                         LoginInfo.BuInfo.BuCode;
                        Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("<script>");
                        //Response.Write("window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("</script>");
                        break;
                }
            }
        }
    }
}