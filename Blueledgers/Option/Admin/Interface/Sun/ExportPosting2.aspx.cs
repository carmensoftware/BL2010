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
    public partial class ExportPosting2 : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.AccountMapp MyaccountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly SqlCommand cmd = new SqlCommand();
        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly SqlDataAdapter da = new SqlDataAdapter();
        private readonly DataSet ds = new DataSet();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.IN.StoreGrp storeGrp = new Blue.BL.IN.StoreGrp();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
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
                //vPre_Exp
                grd_load();

                btn_Export.Attributes.Add("Onclick", "return confirm('Confirm export?');");
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
        ///     Update field ExportStatus.
        /// </summary>
        /// <param name="_exp"></param>
        private void isExp(string _exp)
        {
            //con = new SqlConnection(LoginInfo.ConnStr.ToString());
            //con.Open();

            //// Update Table Receiving.
            //rptquery = "UPDATE PC.REC SET ExportStatus= 1 where ExportStatus= 0 and convert(varchar, RecDate, 103)>=@FDate and convert(varchar, RecDate, 103)<=@TDate";

            //cmd.CommandType = System.Data.CommandType.Text;
            //cmd.CommandText = rptquery;
            //cmd.Connection  = con;
            //cmd.Parameters.Add("FDate", System.Data.SqlDbType.VarChar).Value = txt_FromDate.Date.ToString("dd/MM/yyyy");
            //cmd.Parameters.Add("TDate", System.Data.SqlDbType.VarChar).Value = txt_ToDate.Date.ToString("dd/MM/yyyy");

            //da.SelectCommand = cmd;
            //cmd.ExecuteNonQuery();

            var dsUpdate = new DataSet();

            //Get data for update
            cn.GetCnbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 0, LoginInfo.ConnStr);
            rec.GetRecbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 0, LoginInfo.ConnStr);

            if (dsUpdate != null)
            {
                if (dsUpdate.Tables[cn.TableName].Rows.Count > 0)
                {
                    for (var a = 0; a < dsUpdate.Tables[cn.TableName].Rows.Count; a++)
                    {
                        dsUpdate.Tables[cn.TableName].Rows[a]["ExportStatus"] = 1;
                    }
                }

                if (dsUpdate.Tables[rec.TableName].Rows.Count > 0)
                {
                    for (var b = 0; b < dsUpdate.Tables[rec.TableName].Rows.Count; b++)
                    {
                        dsUpdate.Tables[rec.TableName].Rows[b]["ExportStatus"] = 1;
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

        /// <summary>
        ///     Click button export.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Export_Click(object sender, EventArgs e)
        {
            //Call export.
            Export();

            //pop_Confrim.ShowOnPageLoad = true;
        }

        protected void btn_Export_Click1(object sender, EventArgs e)
        {
            Export();
        }

        /// <summary>
        /// </summary>
        private void Export()
        {
            con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();

            var dtGetLctAndItemGruop = new DataTable();
            dtGetLctAndItemGruop = accountMapp.GetInterfaceExpNotMap(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);

            if (dtGetLctAndItemGruop.Rows.Count > 0)
            {
                //string[] Location   = new string[dtGetLctAndItemGruop.Rows.Count];
                //string[] ItemGroup  = new string[dtGetLctAndItemGruop.Rows.Count];

                lbl_Warning_AccMap.Text = "Please map account code number at<br>";

                for (var i = 0; i < dtGetLctAndItemGruop.Rows.Count; i++)
                {
                    lbl_Warning_AccMap.Text = lbl_Warning_AccMap.Text + "Store Location '" +
                                              storeLct.GetName(dtGetLctAndItemGruop.Rows[i]["LocationCode"].ToString(),
                                                  LoginInfo.ConnStr) + "'" +
                                              //----02/03/2012----storeLct.GetName2(dtGetLctAndItemGruop.Rows[i]["LocationCode"].ToString(),LoginInfo.ConnStr)
                                              "<br> Item Group '" +
                                              prodCat.GetName(dtGetLctAndItemGruop.Rows[i]["ItemGroupCode"].ToString(),
                                                  LoginInfo.ConnStr) + "'";
                }

                pop_Warning_AccountMapp.ShowOnPageLoad = true;
                return;
            }

            dtInterfaceExp = MyaccountMapp.GetInterfaceExport(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), ddl_StoreGrp.SelectedItem.Value, LoginInfo.ConnStr);

            var data = new ArrayList();
            //const string format = "{0,-10}{1,12}{2,1}{3,3}{4,32}{5,1} {6,5}     {7,-10}     {8,-25}{9,8}{10,1}{11,109}{12,1}{13,18}{14,130}";
            //const string format = "{0,-10}{1,16}{2,33}{3,48}{4,66}{5,68}{6, 93}{7,126}{8,182}{9,241}{10,377}";
            const string format =
                "{0,-10}{1,12}{2,1}{3,3}{4,32}{5,1} {6,5}     {7,-10}     {8,-25}{9,8}{10,1}{11,109}{12,-15}{13,-15}{14,120}";

            data.Add("VERSION                         42601");

            foreach (DataRow dr in dtInterfaceExp.Rows)
            {
                var DocType = dr["DocType"].ToString();
                var RecordType = dr["RecordType"].ToString();
                var VendorCode = dr["VendorCode"].ToString();
                var DBCR = dr["DBCR"].ToString();
                //string DocNo        = dr["DocNo"].ToString();
                var RecordAmount = dr["RecordAmount"].ToString();
                var TransNo = dr["TransNo"].ToString();
                var TransDate = DateTime.Parse(dr["TransDate"].ToString()).ToString("yyyyMMdd");
                var Description = string.Empty;
                var accdate = string.Empty;
                var perdate = string.Empty;
                var DocNo = string.Empty;
                var A1 = dr["A1"].ToString();
                var A2 = dr["A2"].ToString();

                if (dr["Description"].ToString().Length > 25)
                {
                    Description = dr["Description"].ToString().Substring(0, 25);
                }
                else
                {
                    Description = dr["Description"].ToString();
                }

                if (dr["DocDate"].ToString() != string.Empty)
                {
                    accdate = DateTime.Parse(dr["DocDate"].ToString()).ToString("yyyyMMdd");
                }

                if (dr["DocDate"].ToString() != string.Empty)
                {
                    perdate = DateTime.Parse(dr["DocDate"].ToString()).ToString("yyyy0MM");
                }

                if (dr["DocNo"].ToString().Length > 10)
                {
                    DocNo = dr["DocNo"].ToString().Substring(dr["DocNo"].ToString().Length - 10, 10);
                }
                else
                {
                    DocNo = dr["DocNo"].ToString();
                }

                var recdate = DateTime.Parse(dr["TransDate"].ToString()).ToString("yyyyMMdd");
                var perrecdate = DateTime.Parse(dr["TransDate"].ToString()).ToString("yyyy0MM");
                var TheAccountNumber = dr["TheAccountNumber"].ToString();

                //string line             = string.Format(format, TheAccountNumber, perdate, accdate, "L", RecordAmount.Replace(".00", ""), DBCR, DocType, DocNo, Description, recdate, perrecdate, " ", A1, A2, " ");
                var line = string.Format(format, TheAccountNumber, perrecdate, accdate, "L",
                    RecordAmount.Replace(".00", ""), DBCR, DocType, DocNo, Description, recdate, perrecdate, " ", A1, A2,
                    " ");

                data.Add(line);
            }

            // Need to create directory.if c drive not allowed to create file.
            // Directory + FileName
            //string dir = "ExportBO" + DateTime.Now.ToShortDateString;
            var directory = Directory.CreateDirectory(@"C:\SUN").FullName;

            var filePath = ("\\IN" +
                            Convert.ToDateTime(txt_FromDate.Value).ToString("dd-MM-yyyy").Replace("-", string.Empty) +
                            Convert.ToDateTime(txt_ToDate.Value).ToString("dd-MM-yyyy").Replace("-", string.Empty) +
                            ".txt");

            var fullpath = (directory + filePath);

            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();

                // IF not file Exists,create new file.
                var sw = File.CreateText(fullpath);

                for (var i = 0; i < data.Count; i++)
                {
                    sw.Write(data[i]);
                    sw.Write(sw.NewLine);
                }

                // Close StreamWriter
                sw.Close();
            }
            else
            {
                // IF not file Exists,create new file.
                var sw = File.CreateText(fullpath);
                for (var i = 0; i < data.Count; i++)
                {
                    sw.Write(data[i]);
                    sw.Write(sw.NewLine);
                }

                // Close StreamWriter
                sw.Close();
            }


            System.IO.FileStream fs = null;
            fs = System.IO.File.Open(fullpath, System.IO.FileMode.Open);
            var btFile = new byte[fs.Length];
            fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            Response.AddHeader("Content-disposition", "attachment; filename=" + filePath);
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(btFile);

            //Update field ExportStatus.
            //this.isExp("Exp");
            UpdateExportStatus();

            GrdPreView();

            Response.End();
        }

        /// <summary>
        ///     Update export status to true/false
        /// </summary>
        /// <param name="exportStatus"></param>
        private void UpdateExportStatus()
        {
            var dbHandler = new Blue.DAL.DbHandler();
            var dbParams = new Blue.DAL.DbParameter[3];

            var cmd = "PC.UpdateExportStatus";

            dbParams[0] = new Blue.DAL.DbParameter("@FDate", txt_FromDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new Blue.DAL.DbParameter("@TDate", txt_ToDate.Date.ToString("yyyy-MM-dd"));
            dbParams[2] = new Blue.DAL.DbParameter("@StoreGrp", ddl_StoreGrp.SelectedItem.Value);

            dbHandler.DbExecuteNonQuery(cmd, dbParams, LoginInfo.ConnStr);
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
            //rptquery        = "Select * from vPre_Exp where convert(varchar, RecDate, 103)>=@FFDate and convert(varchar, RecDate, 103)<=@TTDate";
            //rptquery        = "Select * from vPre_Exp where DocDate >= @FFDate and DocDate <= @TTDate";

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
                       "AND (r.ExportStatus = 0) " +
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
            da.Fill(ds, "vPre_Exp");

            //grd_Preview.DataSource = ds;
            //grd_Preview.DataBind();

            grd_Preview2.DataSource = ds;
            grd_Preview2.DataBind();
        }

        /// <summary>
        ///     Click button confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = false;

            //this.btn_Confrim.Attributes.Add("Onclick", "return confirm('Confirm export?');");

            Export();
        }

        /// <summary>
        ///     Click button cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            //Display pop up.
            pop_Confrim.ShowOnPageLoad = false;
        }

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

                        var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=160" + "&BuCode=" +
                                         LoginInfo.BuInfo.BuCode;
                        Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("<script>");
                        //Response.Write("window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("</script>");
                        break;
                }
            }
        }

        protected void grd_Preview2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_InvoiceNo") != null)
                {
                    var lbl_InvoiceNo = e.Row.FindControl("lbl_InvoiceNo") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "InvoiceNo").ToString().Length > 10)
                    {
                        lbl_InvoiceNo.Text = DataBinder.Eval(e.Row.DataItem, "InvoiceNo").ToString().Substring(
                            DataBinder.Eval(e.Row.DataItem, "InvoiceNo").ToString().Length - 10, 10);
                    }
                    else
                    {
                        lbl_InvoiceNo.Text = DataBinder.Eval(e.Row.DataItem, "InvoiceNo").ToString();
                    }
                }
            }
        }

        protected void btn_Warning_AccMap_Click(object sender, EventArgs e)
        {
            pop_Warning_AccountMapp.ShowOnPageLoad = false;
            Response.Redirect("~/Option/Admin/Interface/Sun/AccountMapp.aspx");
        }

        #endregion
    }
}