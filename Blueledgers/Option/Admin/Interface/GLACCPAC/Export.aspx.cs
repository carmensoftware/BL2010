using System;
using System.Collections;
using System.Data;
using System.IO;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.AccpacGL
{
    public partial class Export : BasePage
    {
        private readonly Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();
            }
        }

        private void Page_Setting()
        {
            //Default FFDate and TTDate
            txt_FromDate.Date = ServerDateTime.Date;
            txt_ToDate.Date = ServerDateTime.Date;
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            CheckAccountCodeMapping();
            CheckVendorCodeMapping();
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            ViewExport();
        }

        private void ViewExport()
        {
            grd_Preview.DataSource = accountMapp.GetPreviewExportToCarmen(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);
            grd_Preview.DataBind();
        }

        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            ExportPosting();
        }

        /// <summary>
        ///     Export to carmen
        /// </summary>
        private void ExportPosting()
        {
            var Data = new ArrayList();
            var dtExp = accountMapp.GetExportToCarmen(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);


            //Request By P'Op
            if (dtExp.Rows.Count > 0)
            {
                //Add fields name in header
                Data.Add(
                    "RowType|VendorCode|RefVendorCode|AccountCode|InputDate|InvoiceDate|InvoiceNo|TotalAmount|TaxAmount|ProductID|ProductDesc|LocationCode|ItemGroupName|");

                foreach (DataRow drExp in dtExp.Rows)
                {
                    var line = string.Empty;

                    line += drExp["RowType"] + "|" + drExp["VendorCode"] + "|" + drExp["RefVendorCode"] + "|" +
                            drExp["AccountCode"] +
                            "|" + DateTime.Parse(drExp["InputDate"].ToString()).ToString("yyyyMMdd") + "|" +
                            DateTime.Parse(drExp["InvoiceDate"].ToString()).ToString("yyyyMMdd") +
                            "|" + drExp["InvoiceNo"] + "|" + drExp["TotalAmount"].ToString().Replace(".00", "") + "|" +
                            drExp["TaxAmount"].ToString().Replace(".00", "") +
                            "|" + drExp["ProductID"] + "|" + drExp["ProductDesc"] + "|" + drExp["LocationCode"] + "|" +
                            drExp["ItemGroupName"] + "|";

                    Data.Add(line);
                }
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

                for (var i = 0; i < Data.Count; i++)
                {
                    sw.Write(Data[i]);
                    sw.Write(sw.NewLine);
                }

                // Close StreamWriter
                sw.Close();
            }
            else
            {
                // IF not file Exists,create new file.
                var sw = File.CreateText(fullpath);
                for (var i = 0; i < Data.Count; i++)
                {
                    sw.Write(Data[i]);
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

            UpdateExportStatus();
            ViewExport();

            Response.End();
        }

        /// <summary>
        ///     Check vendor code mapping if user not mapp vendor code, user cannot export document has the vendor.
        /// </summary>
        private void CheckVendorCodeMapping()
        {
            var dtVdMapp = vendor.GetExpVendor(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);

            if (dtVdMapp.Rows.Count > 0)
            {
                lbl_WarningVendor.Text =
                    "Please insert 'Acct. Link Ref.#' at page vendor profile following list of vendor below :<br>";

                for (var i = 0; i < dtVdMapp.Rows.Count; i++)
                {
                    lbl_WarningVendor.Text = lbl_WarningVendor.Text + dtVdMapp.Rows[i]["VendorName"] + "<br>";
                }

                pop_WarningVendor.ShowOnPageLoad = true;
            }
            pop_Confirm.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Check account code mapping if user not mapp account code, user cannot export.
        /// </summary>
        private void CheckAccountCodeMapping()
        {
            var dtAccMapp = accountMapp.GetInterfaceExpNotMap(txt_FromDate.Date.ToString("yyyy-MM-dd"),
                txt_ToDate.Date.ToString("yyyy-MM-dd"), LoginInfo.ConnStr);

            if (dtAccMapp.Rows.Count > 0)
            {
                lbl_Warning_AccMap.Text = "Please map account code number at<br>";

                for (var i = 0; i < dtAccMapp.Rows.Count; i++)
                {
                    lbl_Warning_AccMap.Text = lbl_Warning_AccMap.Text + "Store Location '" +
                                              storeLct.GetName(dtAccMapp.Rows[i]["LocationCode"].ToString(),
                                                  LoginInfo.ConnStr) + "'<br>";

                    //check itemgroup in table admin.accountMapp
                    if (dtAccMapp.Rows[i]["ItemGroupCode"].ToString() != string.Empty)
                    {
                        lbl_Warning_AccMap.Text = lbl_Warning_AccMap.Text + "Item Group '" +
                                                  prodCat.GetName(dtAccMapp.Rows[i]["ItemGroupCode"].ToString(),
                                                      LoginInfo.ConnStr) + "'<br>";
                    }
                }

                pop_Warning_AccountMapp.ShowOnPageLoad = true;
            }
        }


        /// <summary>
        ///     Update export status to true/false
        /// </summary>
        /// <param name="exportStatus"></param>
        private void UpdateExportStatus()
        {
            var dbHandler = new Blue.DAL.DbHandler();
            var dbParams = new Blue.DAL.DbParameter[2];

            var cmd =
                " UPDATE [PC].[REC] SET ExportStatus = 'True' WHERE RecDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND RecDate <= DATEADD(s,86399,@ToDate) " +
                " UPDATE [PC].[RECDt] SET ExportStatus = 'True' WHERE RecNo in (SELECT RecNo FROM [PC].[REC] WHERE ExportStatus = 'True' AND RecDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND RecDate <= DATEADD(s,86399,@ToDate)) " +
                " UPDATE [PC].[CN] SET ExportStatus = 'True' WHERE CnDate >= DATEADD(dd, 0, DATEDIFF(dd, 0, @FromDate)) AND CnDate <= DATEADD(s,86399,@ToDate)";

            dbParams[0] = new Blue.DAL.DbParameter("@FromDate", txt_FromDate.Date.ToString("yyyy-MM-dd"));
            dbParams[1] = new Blue.DAL.DbParameter("@ToDate", txt_ToDate.Date.ToString("yyyy-MM-dd"));
            //dbParams[2] = new DAL.DbParameter("@StoreGrp", ddl_StoreGrp.SelectedItem.Value);

            //dbHandler.DbExecuteNonQuery(cmd, dbParams, LoginInfo.ConnStr);
            dbHandler.DbExecuteQuery(cmd, dbParams, LoginInfo.ConnStr);
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_Confirm.ShowOnPageLoad = false;
        }

        protected void btn_Warning_AccMap_Click(object sender, EventArgs e)
        {
            pop_Warning_AccountMapp.ShowOnPageLoad = false;
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

        protected void btn_Continue_Click(object sender, EventArgs e)
        {
            pop_WarningVendor.ShowOnPageLoad = false;
            pop_Confirm.ShowOnPageLoad = true;
        }

        protected void btn_Vendor_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/IN/VD/VendorList.aspx");
        }

        protected void btn_VendorCancel_Click(object sender, EventArgs e)
        {
            pop_WarningVendor.ShowOnPageLoad = false;
        }
    }
}