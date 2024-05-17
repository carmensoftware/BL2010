using System;
using System.Collections;
using System.Data;
using BlueLedger.PL.BaseClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BlueLedger.PL.IN
{
    public partial class VendorList : BasePage
    {
        #region "Attributies"

        private readonly Blue.BL.AP.VendorReport VendorReport = new Blue.BL.AP.VendorReport();
        private readonly Blue.BL.AP.VendorTool VendorTool = new Blue.BL.AP.VendorTool();
        private readonly Blue.BL.AP.VendorView VendorView = new Blue.BL.AP.VendorView();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private Blue.BL.AP.Vendor Vendor = new Blue.BL.AP.Vendor();
        private Blue.BL.AP.VendorViewColumn VendorViewColumn = new Blue.BL.AP.VendorViewColumn();
        private Blue.BL.Profile.BankAccount bankAccount = new Blue.BL.Profile.BankAccount();
        private DataSet dsVendorList = new DataSet();
        private Blue.BL.Application.Field field = new Blue.BL.Application.Field();
        private Blue.BL.AP.Invoice invoice = new Blue.BL.AP.Invoice();
        private Blue.BL.AP.InvoiceDefault invoiceDefault = new Blue.BL.AP.InvoiceDefault();
        private Blue.BL.Application.Menu menu = new Blue.BL.Application.Menu();
        private Blue.BL.AP.Payment payment = new Blue.BL.AP.Payment();
        private Blue.BL.AP.VendorCategory vendorCategory = new Blue.BL.AP.VendorCategory();
        private Blue.BL.AP.VendorDefaultWHT vendorDefaultWHT = new Blue.BL.AP.VendorDefaultWHT();
        private Blue.BL.AP.VendorMisc vendorMisc = new Blue.BL.AP.VendorMisc();
        //private int viewID;

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.5";

        #endregion

        #region "Operations"


        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Retrieve data and binding with controls
            if (!IsPostBack)
            {
                //this.Page_Retrieve();
                //this.Page_Setting();
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Vendor List", "VL"));
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Vendor Detail", "VD"));
                ListPage.DataBind();
            }
            else
            {
                dsVendorList = (DataSet)Session["dsVendorList"];
            }

            ValidatePermission();

            ListPage.CreateItems.Menu.ItemClick += menu_ItemClick;

            base.Page_Load(sender, e);

            var isSync = Request.QueryString["sync"] ?? "";
            btn_SyncVendor.Visible = isSync.ToString().ToLower() == "true";

        }


        /// <summary>
        ///     Get data from database
        /// </summary>
        private void Page_Retrieve()
        {
            // Get vendor report link
            VendorReport.GetVendorReportList(dsVendorList, LoginInfo.ConnStr);

            // Get vendor tool link
            VendorTool.GetVendorToolList(dsVendorList, LoginInfo.ConnStr);

            // Store data in session
            Session["dsVendorList"] = dsVendorList;
        }

        /// <summary>
        ///     Binding controls
        /// </summary>
        private void Page_Setting()
        {
            // Binding ddl_View
            Binding_View();

            // Binding grd_VendorList
            Binding_VendorList();


        }

        /// <summary>
        ///     Binding view
        /// </summary>
        private void Binding_View()
        {
            var dtVendorView = new DataTable();

            // Get data
            dtVendorView = VendorView.GetVendorViewList(3, LoginInfo.ConnStr);


        }

        protected void ValidatePermission()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage.CreateItems.Visible = (pagePermiss >= 3) ? ListPage.CreateItems.Visible : false;
        }



        private string GetDefaultVendorViewID(DataTable vendorView)
        {
            var vendorViewID = string.Empty;

            // Findign default vendor view
            foreach (DataRow dr in vendorView.Rows)
            {
                if (dr["IsDefault"].ToString() == "True")
                {
                    vendorViewID = dr["VendorViewID"].ToString();
                    break;
                }
            }

            // return result
            return vendorViewID;
        }

        private void Binding_VendorList()
        {
        }



        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            var objArrList = new ArrayList();
            var reportLink = string.Empty;
            switch (e.Item.Name.ToUpper())
            {
                case "VL":

                    Session["dtBuKeys"] = ListPage.dtBuKeys;

                    Session.Remove("SubReportName");
                    Session["SubReportName"] = "content";

                    reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=130";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");

                    break;
                case "VD":

                    Session["dtBuKeys"] = ListPage.dtBuKeys;

                    Session.Remove("SubReportName");
                    Session["SubReportName"] = "content";

                    reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=131";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");

                    break;
            }
        }



        protected void btn_Go_Click(object sender, EventArgs e)
        {
            // Re-binding account list
            Binding_VendorList();
        }


        protected void btn_SyncVendor_Click(object sender, EventArgs e)
        {
            SyncVendor();
            Response.Redirect(Request.RawUrl);
        }


        private void SyncVendor()
        {
            var configVendor = config.GetValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);

            if (!string.IsNullOrEmpty(configVendor))
            {
                var keys = new KeyValues();

                keys.Text = configVendor;

                string type = keys.Value("type");

                if (type.ToLower() == "api")
                {
                    var host = keys.Value("host");
                    var token = keys.Value("auth");

                    var api = new CarmenApi(host, token);
                    var vendors = api.GetVendors();

                    var script = new StringBuilder();
                    var recordCount = 1;
                    var builder = new StringBuilder();

                    foreach (var item in vendors)
                    {

                        var vnCode = item.VnCode;
                        var vnName = item.VnName.Replace("\'", "\'\'");
                        var vnAdd1 = string.IsNullOrEmpty(item.VnAdd1) ? "" : item.VnAdd1.Replace("\'", "\'\'");
                        var vnAdd2 = string.IsNullOrEmpty(item.VnAdd2) ? "" : item.VnAdd2.Replace("\'", "\'\'");
                        var vnAdd3 = string.IsNullOrEmpty(item.VnAdd3) ? "" : item.VnAdd3.Replace("\'", "\'\'");
                        var vnAdd4 = string.IsNullOrEmpty(item.VnAdd4) ? "" : item.VnAdd4.Replace("\'", "\'\'");
                        var vnTel = string.IsNullOrEmpty(item.VnTel) ? "" : item.VnTel.Replace("\'", "\'\'");
                        var vnFax = string.IsNullOrEmpty(item.VnFax) ? "" : item.VnFax.Replace("\'", "\'\'");
                        var vnCateCode = item.VnCateCode;
                        var vnTaxNo = string.IsNullOrEmpty(item.VnTaxNo) ? "" : item.VnTaxNo.Replace("\'", "\'\'"); // item.VnTaxNo;
                        var branchNo = string.IsNullOrEmpty(item.BranchNo) ? "" : item.BranchNo.Replace("\'", "\'\'");  //item.BranchNo;

                        branchNo = branchNo.Length > 20 ? branchNo.Substring(0, 20) : branchNo;

                        var vnVat1 = item.VnVat1.Substring(0, 1);
                        var vnTaxR1 = item.VnTaxR1 ?? 0;
                        var vnTerm = item.VnTerm ?? 0;
                        var active = item.Active ? "1" : "0";
                        var lastModified = item.LastModified == null ? null : string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.LastModified);

                        var address = string.Format("{0} {1} {2} {3}", vnAdd1, vnAdd2, vnAdd3, vnAdd4).Trim();
                        var userModified = string.IsNullOrEmpty(item.UserModified);


                        builder.Append("EXEC [Tool].[Vendor_InsertOrUpdate] ");
                        builder.AppendFormat("@VendorCode='{0}',", vnCode);
                        builder.AppendFormat("@CategoryCode='{0}',", vnCateCode);
                        builder.AppendFormat("@VendorName=N'{0}',", vnName.Replace("'", "''"));
                        builder.AppendFormat("@Address=N'{0}',", address.Replace("'", "''"));
                        builder.AppendFormat("@Tel=N'{0}',", vnTel.Replace("'", "''"));
                        builder.AppendFormat("@Fax=N'{0}',", vnFax.Replace("'", "''"));
                        builder.AppendFormat("@TaxId=N'{0}',", vnTaxNo.Replace("'", "''"));
                        builder.AppendFormat("@BranchId=N'{0}',", branchNo.Replace("'", "''"));
                        builder.AppendFormat("@TaxType='{0}',", vnVat1);
                        builder.AppendFormat("@TaxRate='{0}',", vnTaxR1);
                        builder.AppendFormat("@CreditTerm='{0}',", vnTerm);
                        builder.AppendFormat("@IsActive='{0}',", active);
                        builder.AppendFormat("@UpdatedBy='{0}',", userModified);
                        builder.AppendFormat("@UpdatedDate='{0}'", lastModified);
                        builder.AppendLine(";");


                        if (recordCount > 500)
                        {
                            script.AppendLine(builder.ToString());
                            recordCount = 0;
                            builder.Clear();

                        }

                        recordCount++;

                    }

                    script.AppendLine(builder.ToString());

                    //throw new Exception(script.ToString());

                    // run scripts
                    using (SqlConnection conn = new SqlConnection(LoginInfo.ConnStr))
                    {
                        SqlCommand command = new SqlCommand(script.ToString(), conn);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }



        }

        #endregion
    }
}