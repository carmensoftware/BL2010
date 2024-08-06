using System;
using System.Collections;
using System.Data;
using BlueLedger.PL.BaseClass;
//using System.Net.Http;
//using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

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
        private int viewID;

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

            // Display Title
            //if (ddl_View.SelectedItem != null)
            //{
            //    lbl_ListTitle.Text   = ddl_View.SelectedItem.Text.ToString();
            //    btn_EditView.Enabled = true;
            //}
            //else
            //{
            //    lbl_ListTitle.Text = string.Empty;
            //}

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

            // Binding ddl_View
            //if (dtVendorView.Rows.Count > 0)
            //{
            //    ddl_View.DataSource     = dtVendorView;
            //    ddl_View.DataTextField  = "Description";
            //    ddl_View.DataValueField = "VendorViewID";
            //    ddl_View.DataBind();

            //    // Set selected item
            //    // 1 : Checking the lastest selected form cookies
            //    if (Request.Cookies["LastestVendorViewSelect"] != null)
            //    {
            //        ddl_View.SelectedValue = Request.Cookies["LastestVendorViewSelect"].Value.ToString();
            //    }
            //    else
            //    {
            //        // Finding default account view id
            //        string defautlVendorViewID = this.GetDefaultVendorViewID(dtVendorView);

            //        // 2 : Finding the default view
            //        if (defautlVendorViewID != string.Empty)
            //        {
            //            ddl_View.SelectedValue = defautlVendorViewID;
            //        }
            //        // 3 : Using first item for default
            //        else
            //        {
            //            if (dtVendorView.Rows.Count > 0)
            //            {
            //                ddl_View.SelectedValue = dtVendorView.Rows[0]["VendorViewID"].ToString();
            //            }
            //            else
            //            {
            //                ddl_View.SelectedValue = "";
            //            }   
            //        }
            //    }

            //    // Update cookies
            //    if (ddl_View.SelectedItem != null)
            //    { 
            //        Response.Cookies["LastestVendorViewSelect"].Value = (ddl_View.SelectedItem.Value);
            //        Response.Cookies["LastestVendorViewSelect"].Expires = DateTime.MaxValue;
            //    }
            //}
        }

        protected void ValidatePermission()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage.CreateItems.Visible = (pagePermiss >= 3) ? ListPage.CreateItems.Visible : false;
        }


        /// <summary>
        ///     Get default view
        /// </summary>
        /// <param name="vendorView"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Binding grd_VendorList
        /// </summary>
        private void Binding_VendorList()
        {
            //if (ddl_View.SelectedItem != null)
            //{
            //    int viewID = Convert.ToInt32(ddl_View.SelectedItem.Value);

            //    // Clear dataset.
            //    dsVendorList.Clear();

            //    // Get data
            //    Vendor.GetVendorList(dsVendorList, viewID, 3, LoginInfo.ConnStr);

            //    // Get data from vendor view column.
            //    VendorViewColumn.GetVendorViewColumnList(viewID, dsVendorList, LoginInfo.ConnStr);

            //    // If number of dt column is less than grid column the set the column visibility
            //    if (grd_VendorList.Columns.Count > dsVendorList.Tables[VendorViewColumn.TableName].Rows.Count)
            //    {
            //        for (int i = 1; i < grd_VendorList.Columns.Count; i++)
            //        {
            //            if (i > dsVendorList.Tables[VendorViewColumn.TableName].Rows.Count)
            //            {
            //                grd_VendorList.Columns[i].Visible = false;
            //            }
            //            else
            //            {
            //                grd_VendorList.Columns[i].Visible = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 1; i < grd_VendorList.Columns.Count; i++)
            //        {
            //            if (!grd_VendorList.Columns[i].Visible)
            //            {
            //                grd_VendorList.Columns[i].Visible = true;
            //            }
            //        }
            //    }


            //    // Binding
            //    grd_VendorList.DataSource = dsVendorList.Tables[Vendor.TableName];
            //    grd_VendorList.DataBind();

            //}
        }


        /// <summary>
        ///     Display Market List/Standard Order
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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


        //private String GetArrVLNo()
        //{

        //    StringBuilder sb = new StringBuilder();

        //    GridView grdd = (GridView)ListPage.FindControl("grd_BU");
        //    GridView grdTran = (GridView)grdd.Rows[0].FindControl("grd_Trans");


        //    for (int i = 0; i < grdTran.Rows.Count; i++)
        //    {
        //        string prId = grdTran.Rows[i].Cells[0].Text;

        //        sb.Append("'" + prId + "',");
        //    }
        //    if (sb.Length > 0)
        //    {
        //        return sb.ToString().Substring(0, sb.Length - 1);
        //    }
        //    else
        //    {
        //        return "'*'";
        //    }

        //    //ASPxGridView grdd = (ASPxGridView)ListPage.FindControl("grd_DataList");

        //    //for (int i = 0; i < grdd.VisibleRowCount; i++)
        //    //{
        //    //    string prId = grdd.GetRowValues(i, "AP.vVendorProfile.VendorCode").ToString();

        //    //    sb.Append("'" + prId + "',");
        //    //}
        //    //if (sb.Length > 0)
        //    //{
        //    //    return sb.ToString().Substring(0, sb.Length - 1);
        //    //}
        //    //else
        //    //{
        //    //    return "'*'";
        //    //}
        //}

        //protected void ddl_View_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Display Title
        //    lbl_ListTitle.Text = ddl_View.SelectedItem.Text;
        //    // Update the lastes selected view cookies
        //    Response.Cookies["LastestVendorViewSelect"].Value = ddl_View.SelectedItem.Value;
        //    Response.Cookies["LastestVendorViewSelect"].Expires = DateTime.MaxValue;
        //    // Re-binding vendor grid
        //    this.Binding_VendorList();
        //}
        /// <summary>
        ///     Get data using selected view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     Refresh data using current view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            var values = config.GetValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);

            if (!string.IsNullOrEmpty(values))
            {
                var keys = new KeyValues();

                keys.Text = values;

                string intfType = keys.Value("type");

                if (intfType.ToLower() == "api")
                {
                    var baseUrl = keys.Value("host");
                    var auth = keys.Value("auth");
                    var endpoint = keys.Value("vendor");

                    try
                    {
                        var api = new API(baseUrl, auth);

                        var json = api.Get(endpoint);
                        var result = JsonConvert.DeserializeObject<RootData>(json);


                        var script = new List<string>();

                        foreach (DataVendor item in result.Data)
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
                            var vnTaxNo = item.VnTaxNo;
                            var branchNo = item.BranchNo;
                            var vnVat1 = item.VnVat1.Substring(0, 1);
                            var vnTaxR1 = item.VnTaxR1 ?? 0;
                            var vnTerm = item.VnTerm ?? 0;
                            var active = item.Active ? "1" : "0";
                            var lastModified = item.LastModified == null ? null : string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.LastModified);

                            var address = string.Format("{0} {1} {2} {3}", vnAdd1, vnAdd2, vnAdd3, vnAdd4).Trim();
                            var userModified = string.IsNullOrEmpty(item.UserModified);

                            //using (SqlConnection connection = new SqlConnection(LoginInfo.ConnStr))
                            //{
                            string sql = "EXEC [Tool].[Vendor_InsertOrUpdate] ";
                            sql += string.Format("@VendorCode='{0}',", vnCode);
                            sql += string.Format("@CategoryCode='{0}',", vnCateCode);
                            sql += string.Format("@VendorName=N'{0}',", vnName);
                            sql += string.Format("@Address=N'{0}',", address);
                            sql += string.Format("@Tel=N'{0}',", vnTel);
                            sql += string.Format("@Fax=N'{0}',", vnFax);
                            sql += string.Format("@TaxId=N'{0}',", vnTaxNo);
                            sql += string.Format("@BranchId=N'{0}',", branchNo);
                            sql += string.Format("@TaxType='{0}',", vnVat1);
                            sql += string.Format("@TaxRate='{0}',", vnTaxR1);
                            sql += string.Format("@CreditTerm='{0}',", vnTerm);
                            sql += string.Format("@IsActive='{0}',", active);
                            sql += string.Format("@UpdatedBy='{0}',", userModified);
                            sql += string.Format("@UpdatedDate='{0}'", lastModified);


                            script.Add(sql);
                        }

                        using (SqlConnection connection = new SqlConnection(LoginInfo.ConnStr))
                        {
                            SqlCommand command = new SqlCommand(string.Join("; ", script), connection);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                        }


                    }
                    catch (Exception ex)
                    {
                        Response.Write(string.Format("<script>alert(`{0}`);</script>", ex.Message));
                    }



                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri(baseUrl);
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    client.DefaultRequestHeaders.Add("Authorization", auth);

                    //    HttpResponseMessage response = client.GetAsync(keys.Value("vendor")).Result;

                    //    //lbl_Test.Text = baseUrl;

                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        //lbl_Test.Text = "Success";

                    //        var json = response.Content.ReadAsStringAsync().Result;
                    //        var result = JsonConvert.DeserializeObject<RootData>(json);


                    //        var script = new List<string>();

                    //        foreach (DataVendor item in result.Data)
                    //        {
                    //            var vnCode = item.VnCode;
                    //            var vnName = item.VnName.Replace("\'", "\'\'");
                    //            var vnAdd1 = string.IsNullOrEmpty(item.VnAdd1) ? "" : item.VnAdd1.Replace("\'", "\'\'");
                    //            var vnAdd2 = string.IsNullOrEmpty(item.VnAdd2) ? "" : item.VnAdd2.Replace("\'", "\'\'");
                    //            var vnAdd3 = string.IsNullOrEmpty(item.VnAdd3) ? "" : item.VnAdd3.Replace("\'", "\'\'");
                    //            var vnAdd4 = string.IsNullOrEmpty(item.VnAdd4) ? "" : item.VnAdd4.Replace("\'", "\'\'");
                    //            var vnTel = string.IsNullOrEmpty(item.VnTel) ? "" : item.VnTel.Replace("\'", "\'\'");
                    //            var vnFax = string.IsNullOrEmpty(item.VnFax) ? "" : item.VnFax.Replace("\'", "\'\'");
                    //            var vnCateCode = item.VnCateCode;
                    //            var vnTaxNo = item.VnTaxNo;
                    //            var branchNo = item.BranchNo;
                    //            var vnVat1 = item.VnVat1.Substring(0, 1);
                    //            var vnTaxR1 = item.VnTaxR1 ?? 0;
                    //            var vnTerm = item.VnTerm ?? 0;
                    //            var active = item.Active ? "1" : "0";
                    //            var lastModified = item.LastModified == null ? null : string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.LastModified);

                    //            var address = string.Format("{0} {1} {2} {3}", vnAdd1, vnAdd2, vnAdd3, vnAdd4).Trim();
                    //            var userModified = string.IsNullOrEmpty(item.UserModified);

                    //            //using (SqlConnection connection = new SqlConnection(LoginInfo.ConnStr))
                    //            //{
                    //            string sql = "EXEC [Tool].[Vendor_InsertOrUpdate] ";
                    //            sql += string.Format("@VendorCode='{0}',", vnCode);
                    //            sql += string.Format("@CategoryCode='{0}',", vnCateCode);
                    //            sql += string.Format("@VendorName=N'{0}',", vnName);
                    //            sql += string.Format("@Address=N'{0}',", address);
                    //            sql += string.Format("@Tel=N'{0}',", vnTel);
                    //            sql += string.Format("@Fax=N'{0}',", vnFax);
                    //            sql += string.Format("@TaxId=N'{0}',", vnTaxNo);
                    //            sql += string.Format("@BranchId=N'{0}',", branchNo);
                    //            sql += string.Format("@TaxType='{0}',", vnVat1);
                    //            sql += string.Format("@TaxRate='{0}',", vnTaxR1);
                    //            sql += string.Format("@CreditTerm='{0}',", vnTerm);
                    //            sql += string.Format("@IsActive='{0}',", active);
                    //            sql += string.Format("@UpdatedBy='{0}',", userModified);
                    //            sql += string.Format("@UpdatedDate='{0}'", lastModified);


                    //            script.Add(sql);
                    //        }


                    //        //lbl_Test.Text = string.Join("; <br />", script);


                    //        using (SqlConnection connection = new SqlConnection(LoginInfo.ConnStr))
                    //        {
                    //            SqlCommand command = new SqlCommand(string.Join("; ", script), connection);
                    //            command.Connection.Open();
                    //            command.ExecuteNonQuery();
                    //        }
                    //    }
                    //}

                }
            }
        }

        public class RootData
        {
            public RootData()
            {
                Data = new List<DataVendor>();
            }

            public IEnumerable<DataVendor> Data { get; set; }
        }

        public class DataVendor
        {
            public int Id { get; set; }
            public string VnCode { get; set; }
            public string VnName { get; set; }
            public string VnAdd1 { get; set; }
            public string VnAdd2 { get; set; }
            public string VnAdd3 { get; set; }
            public string VnAdd4 { get; set; }
            public string VnTel { get; set; }
            public string VnFax { get; set; }
            public string VnCateCode { get; set; }
            public string VnTaxNo { get; set; }
            public string BranchNo { get; set; }
            public string VnVat1 { get; set; }
            public decimal? VnTaxR1 { get; set; }
            public int? VnTerm { get; set; }
            public bool Active { get; set; }
            public DateTime? LastModified { get; set; }
            public string UserModified { get; set; }


            //public string VnCateDesc { get; set; }
            //public string VnEmail { get; set; }
            //public int VnDisTrm { get; set; }
            //public decimal VnDisPct { get; set; }
            //public string VnRegNo { get; set; }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_VendorList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {

        //        // Biding Header
        //        if (grd_VendorList.Columns[1].Visible != false)
        //        {
        //            grd_VendorList.Columns[1].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[0]);


        //            // Set size column 0.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[1].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[1].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr1") != null)
        //            {
        //                Label lbl_Hdr1 = (Label)e.Row.FindControl("lbl_Hdr1");
        //                lbl_Hdr1.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[0].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[2].Visible != false)
        //        {
        //            grd_VendorList.Columns[2].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[1]);


        //            // Set size column 1.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[2].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[2].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr2") != null)
        //            {
        //                Label lbl_Hdr2 = (Label)e.Row.FindControl("lbl_Hdr2");
        //                lbl_Hdr2.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[1].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[3].Visible != false)
        //        {
        //            grd_VendorList.Columns[3].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[2]);


        //            // Set size column 2.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[3].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[3].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr3") != null)
        //            {
        //                Label lbl_Hdr3 = (Label)e.Row.FindControl("lbl_Hdr3");
        //                lbl_Hdr3.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[2].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[4].Visible != false)
        //        {
        //            grd_VendorList.Columns[4].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[3]);


        //            // Set size column 3.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[4].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[4].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr4") != null)
        //            {
        //                Label lbl_Hdr4 = (Label)e.Row.FindControl("lbl_Hdr4");
        //                lbl_Hdr4.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[3].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[5].Visible != false)
        //        {
        //            grd_VendorList.Columns[5].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[4]);


        //            // Set size column 4.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[5].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[5].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr5") != null)
        //            {
        //                Label lbl_Hdr5 = (Label)e.Row.FindControl("lbl_Hdr5");
        //                lbl_Hdr5.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[4].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[6].Visible != false)
        //        {
        //            grd_VendorList.Columns[6].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[5]);


        //            // Set size column 5.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[6].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[6].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr6") != null)
        //            {
        //                Label lbl_Hdr6 = (Label)e.Row.FindControl("lbl_Hdr6");
        //                lbl_Hdr6.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[5].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[7].Visible != false)
        //        {
        //            grd_VendorList.Columns[7].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[6]);

        //            // Set size column 6.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[7].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[7].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr7") != null)
        //            {
        //                Label lbl_Hdr7 = (Label)e.Row.FindControl("lbl_Hdr7");
        //                lbl_Hdr7.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[6].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[8].Visible != false)
        //        {
        //            grd_VendorList.Columns[8].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[7]);


        //            // Set size column 7
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[8].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[8].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr8") != null)
        //            {
        //                Label lbl_Hdr8 = (Label)e.Row.FindControl("lbl_Hdr8");
        //                lbl_Hdr8.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[7].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[9].Visible != false)
        //        {
        //            grd_VendorList.Columns[9].HeaderStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[8]);


        //            // Set size column 8
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["WidthType"].Equals("%"))
        //                {
        //                    grd_VendorList.Columns[9].HeaderStyle.Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    grd_VendorList.Columns[9].HeaderStyle.Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr9") != null)
        //            {
        //                Label lbl_Hdr9 = (Label)e.Row.FindControl("lbl_Hdr9");
        //                lbl_Hdr9.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[8].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }
        //    }

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes.Add("onMouseOver", "GridView_Row_MouseOver(this);");
        //        e.Row.Attributes.Add("onMouseOut", "GridView_Row_MouseOut(this);");
        //        e.Row.Attributes.Add("onDblClick", "GridView_Row_DblClick('Vendor.aspx?vendorCode=" +
        //        dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][dsVendorList.Tables[Vendor.TableName].Columns.Count - 1].ToString() + "','_self');");
        //        e.Row.Style.Add("cursor", "hand");


        //        // Edit
        //        if (e.Row.FindControl("lnkb_Edit") != null)
        //        {
        //            HyperLink lnkb_Edit = (HyperLink)e.Row.FindControl("lnkb_Edit");
        //            lnkb_Edit.NavigateUrl = "VendorEdit.aspx?action=edit&vendorCode=" +
        //                                    DataBinder.Eval(e.Row.DataItem, "vendorCode").ToString();
        //        }

        //        // Binding item data
        //        if (grd_VendorList.Columns[1].Visible != false)
        //        {
        //            grd_VendorList.Columns[1].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[0]);

        //            if (e.Row.FindControl("lbl_Item1") != null)
        //            {
        //                Label lbl_Item1 = (Label)e.Row.FindControl("lbl_Item1");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[0].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][0] != DBNull.Value)
        //                    {
        //                        lbl_Item1.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][0]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[0].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating1 = e.Row.FindControl("Rating1") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][0] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][0]));


        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item1.Visible = false;
        //                            rating1.CurrentRating = ratValue;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating1.Visible = false;
        //                        lbl_Item1.Text = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][0].ToString();
        //                    }
        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[2].Visible != false)
        //        {
        //            grd_VendorList.Columns[2].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[1]);

        //            if (e.Row.FindControl("lbl_Item2") != null)
        //            {
        //                Label lbl_Item2 = (Label)e.Row.FindControl("lbl_Item2");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[1].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][1] != DBNull.Value)
        //                    {
        //                        lbl_Item2.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][1]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[1].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating2 = e.Row.FindControl("Rating2") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][1] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][1]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            rating2.CurrentRating = ratValue;
        //                            lbl_Item2.Visible = false;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating2.Visible = false;
        //                        lbl_Item2.Text = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][1].ToString();
        //                    }

        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[3].Visible != false)
        //        {
        //            grd_VendorList.Columns[3].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[2]);

        //            if (e.Row.FindControl("lbl_Item3") != null)
        //            {
        //                Label lbl_Item3 = (Label)e.Row.FindControl("lbl_Item3");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[2].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][2] != DBNull.Value)
        //                    {
        //                        lbl_Item3.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][2]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[2].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating3 = e.Row.FindControl("Rating3") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][2] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][2]));


        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item3.Visible = false;
        //                            rating3.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating3.Visible = false;
        //                        lbl_Item3.Text = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][2].ToString();
        //                    }


        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[4].Visible != false)
        //        {
        //            grd_VendorList.Columns[4].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[3]);

        //            if (e.Row.FindControl("lbl_Item4") != null)
        //            {
        //                Label lbl_Item4 = (Label)e.Row.FindControl("lbl_Item4");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[3].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][3] != DBNull.Value)
        //                    {
        //                        lbl_Item4.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][3]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else

        //                {
        //                    // Get columnName
        //                    string columnName = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[3].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating4 = e.Row.FindControl("Rating4") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][3] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][3]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item4.Visible = false;
        //                            rating4.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating4.Visible = false;
        //                        lbl_Item4.Text  = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][3].ToString();
        //                    }
        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[5].Visible != false)
        //        {
        //            grd_VendorList.Columns[5].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[4]);

        //            if (e.Row.FindControl("lbl_Item5") != null)
        //            {
        //                Label lbl_Item5 = (Label)e.Row.FindControl("lbl_Item5");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[4].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][4] != DBNull.Value)
        //                    {
        //                        lbl_Item5.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][4]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName                 = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[4].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating5 = e.Row.FindControl("Rating5") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][4] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][4]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item5.Visible     = false;
        //                            rating5.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating5.Visible = false;
        //                        lbl_Item5.Text  = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][4].ToString();
        //                    }
        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[6].Visible != false)
        //        {
        //            grd_VendorList.Columns[6].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[5]);

        //            if (e.Row.FindControl("lbl_Item6") != null)
        //            {
        //                Label lbl_Item6 = (Label)e.Row.FindControl("lbl_Item6");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[5].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][5] != DBNull.Value)
        //                    {
        //                        lbl_Item6.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][5]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {

        //                    // Get columnName
        //                    string columnName                 = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[5].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating6 = e.Row.FindControl("Rating6") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][5] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][5]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item6.Visible = false;
        //                            rating6.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating6.Visible = false;
        //                        lbl_Item6.Text = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][5].ToString();
        //                    }


        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[7].Visible != false)
        //        {
        //            grd_VendorList.Columns[7].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[6]);

        //            if (e.Row.FindControl("lbl_Item7") != null)
        //            {
        //                Label lbl_Item7 = (Label)e.Row.FindControl("lbl_Item7");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[6].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][6] != DBNull.Value)
        //                    {
        //                        lbl_Item7.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][6]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {

        //                    // Get columnName
        //                    string columnName                 = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[6].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating7 = e.Row.FindControl("Rating7") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][6] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][6]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item7.Visible = false;
        //                            rating7.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating7.Visible = false;
        //                        lbl_Item7.Text = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][6].ToString();
        //                    }


        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[8].Visible != false)
        //        {
        //            grd_VendorList.Columns[8].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[7]);

        //            if (e.Row.FindControl("lbl_Item8") != null)
        //            {
        //                Label lbl_Item8 = (Label)e.Row.FindControl("lbl_Item8");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[7].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][7] != DBNull.Value)
        //                    {
        //                        lbl_Item8.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][7]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName                 = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[7].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating8 = e.Row.FindControl("Rating8") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][7] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][7]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item8.Visible = false;
        //                            rating8.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating8.Visible = false;
        //                        lbl_Item8.Text  = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][7].ToString();
        //                    }


        //                }
        //            }
        //        }

        //        if (grd_VendorList.Columns[9].Visible != false)
        //        {
        //            grd_VendorList.Columns[9].ItemStyle.HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[8]);

        //            if (e.Row.FindControl("lbl_Item9") != null)
        //            {
        //                Label lbl_Item9 = (Label)e.Row.FindControl("lbl_Item9");

        //                if (dsVendorList.Tables[Vendor.TableName].Columns[8].DataType.Name.ToUpper() == "DATETIME")
        //                {
        //                    if (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][8] != DBNull.Value)
        //                    {
        //                        lbl_Item9.Text = ((DateTime)dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][8]).ToString(DateTimeFormat);
        //                    }
        //                }
        //                else
        //                {
        //                    // Get columnName
        //                    string columnName                 = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[8].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);

        //                    // Find ajax rating control.
        //                    AjaxControlToolkit.Rating rating9 = e.Row.FindControl("Rating9") as AjaxControlToolkit.Rating;

        //                    // Binding for rating control.
        //                    if (columnName == "Rating")
        //                    {

        //                        int ratValue = (dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][8] == System.DBNull.Value ? Convert.ToInt32(string.Empty) : Convert.ToInt32(dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][8]));

        //                        if (ratValue > 0 & ratValue < 6)
        //                        {
        //                            lbl_Item9.Visible     = false;
        //                            rating9.CurrentRating = ratValue;

        //                        }

        //                    }
        //                    else
        //                    {
        //                        rating9.Visible = false;
        //                        lbl_Item9.Text  = dsVendorList.Tables[Vendor.TableName].Rows[e.Row.DataItemIndex][8].ToString();
        //                    }


        //                }
        //            }
        //        }

        //    }

        //    // Binding if empty row for header.
        //    if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        //    {
        //        Table tbl_EmptyDataRow = (Table)e.Row.FindControl("tbl_EmptyDataRow");


        //        if (grd_VendorList.Columns[0].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[0].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[0]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[0].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[0].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[0]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr1") != null)
        //            {
        //                Label lbl_Hdr1 = (Label)e.Row.FindControl("lbl_Hdr1");
        //                lbl_Hdr1.Text = "&nbsp;&nbsp;&nbsp;&nbsp;" +
        //                                 field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[0].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[1].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[1].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[1]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[1].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[1].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[1]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr2") != null)
        //            {
        //                Label lbl_Hdr2 = (Label)e.Row.FindControl("lbl_Hdr2");
        //                lbl_Hdr2.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[1].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[2].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[2].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[2]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[2].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[2].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[2]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr3") != null)
        //            {
        //                Label lbl_Hdr3 = (Label)e.Row.FindControl("lbl_Hdr3");
        //                lbl_Hdr3.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[2].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[3].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[3].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[3]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[3].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[3].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[3]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr4") != null)
        //            {
        //                Label lbl_Hdr4 = (Label)e.Row.FindControl("lbl_Hdr4");
        //                lbl_Hdr4.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[3].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[4].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[4].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[4]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[4].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[4].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[4]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr5") != null)
        //            {
        //                Label lbl_Hdr5 = (Label)e.Row.FindControl("lbl_Hdr5");
        //                lbl_Hdr5.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[4].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[5].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[5].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[5]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[5].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[5].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[5]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr6") != null)
        //            {
        //                Label lbl_Hdr6 = (Label)e.Row.FindControl("lbl_Hdr6");
        //                lbl_Hdr6.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[5].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[6].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[6].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[6]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[6].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[6].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[6]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr7") != null)
        //            {
        //                Label lbl_Hdr7 = (Label)e.Row.FindControl("lbl_Hdr7");
        //                lbl_Hdr7.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[6].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[7].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[7].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[7]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[7].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[7].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[7]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr8") != null)
        //            {
        //                Label lbl_Hdr8 = (Label)e.Row.FindControl("lbl_Hdr8");
        //                lbl_Hdr8.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[7].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }

        //        if (grd_VendorList.Columns[8].Visible != false)
        //        {
        //            tbl_EmptyDataRow.Rows[0].Cells[8].HorizontalAlign = GetColumnHorizentalAlign(dsVendorList.Tables[Vendor.TableName].Columns[8]);


        //            // Set size.
        //            if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString() != string.Empty)
        //            {
        //                if (dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["WidthType"].Equals("%"))
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[8].Width = Unit.Percentage(double.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString()));
        //                }
        //                else
        //                {
        //                    tbl_EmptyDataRow.Rows[0].Cells[8].Width = Unit.Pixel(int.Parse(dsVendorList.Tables[VendorViewColumn.TableName].Rows[8]["Width"].ToString()));
        //                }
        //            }

        //            if (e.Row.FindControl("lbl_Hdr9") != null)
        //            {
        //                Label lbl_Hdr9 = (Label)e.Row.FindControl("lbl_Hdr9");
        //                lbl_Hdr9.Text = field.GetDisplayText(dsVendorList.Tables[Vendor.TableName].Columns[8].ColumnName, LoginInfo.BuFmtInfo.LangCode, LoginInfo.ConnStr);
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// delete for vendor list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grd_VendorList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    DataSet dsVendorDelete = new DataSet();

        //    // Get Vendorcode for deleted row
        //    int vendorCode = Convert.ToInt32(grd_VendorList.DataKeys[e.RowIndex].Values[0].ToString());


        //    // Get Vendor Misc for deleted row
        //    vendorMisc.GetList(dsVendorDelete, vendorCode.ToString(), LoginInfo.ConnStr);

        //    if (dsVendorDelete.Tables[vendorMisc.TableName] != null)
        //    {
        //        foreach (DataRow drvendorMisc in dsVendorDelete.Tables[vendorMisc.TableName].Rows)
        //        {
        //            drvendorMisc.Delete();
        //        }
        //    }


        //    // Get vendor for profilecode.
        //    Vendor.GetVendor(vendorCode.ToString(),dsVendorDelete,LoginInfo.ConnStr);

        //    // Get vendordefaultwht for deleted row
        //    vendorDefaultWHT.GetVendorDefaultWHTList(dsVendorDelete.Tables[Vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorDelete, LoginInfo.ConnStr);

        //    if (dsVendorDelete.Tables[vendorDefaultWHT.TableName] != null)
        //    {
        //        foreach (DataRow drVendorDefaultWHT in dsVendorDelete.Tables[vendorDefaultWHT.TableName].Rows)
        //        {
        //            drVendorDefaultWHT.Delete();
        //        }
        //    }

        //    // Get bank account for deleted row
        //    bankAccount.GetBankAccountList(dsVendorDelete.Tables[Vendor.TableName].Rows[0]["ProfileCode"].ToString(), dsVendorDelete, LoginInfo.ConnStr);

        //    // Delete bankaccount  for selected row.
        //    for (int i = dsVendorDelete.Tables[bankAccount.TableName].Rows.Count - 1; i >= 0; i--)
        //    {
        //        DataRow drBankAccount = dsVendorDelete.Tables[bankAccount.TableName].Rows[i];

        //        if (drBankAccount.RowState != DataRowState.Deleted)
        //        {
        //            drBankAccount.Delete();
        //        }
        //    }


        //    // Get vendorCategory for deleted row
        //    vendorCategory.GetVendorCategoryListByVendorCategoryCode(dsVendorDelete, dsVendorDelete.Tables[Vendor.TableName].Rows[0]["VendorCategoryCode"].ToString(), LoginInfo.ConnStr);

        //    if (dsVendorDelete.Tables[vendorCategory.TableName] != null)
        //    {
        //        foreach (DataRow drVendorCategory in dsVendorDelete.Tables[vendorCategory.TableName].Rows)
        //        {
        //            drVendorCategory.Delete();
        //        }
        //    }


        //    // Delete vendor selected row
        //    for (int i = dsVendorDelete.Tables[Vendor.TableName].Rows.Count - 1; i >= 0; i--)
        //    {
        //        DataRow drVendor = dsVendorDelete.Tables[Vendor.TableName].Rows[i];

        //        if (drVendor.RowState != DataRowState.Deleted)
        //        {
        //            if (Convert.ToInt32(drVendor["VendorCode"]) == Convert.ToInt32(vendorCode))
        //            {
        //                drVendor.Delete();
        //                continue;
        //            }
        //        }
        //    }

        //    bool deleted = Vendor.Delete(dsVendorDelete, LoginInfo.ConnStr);

        //    if (deleted)
        //    {
        //        this.Binding_VendorList();
        //    }
        //    else
        //    {

        //        // Display error message
        //        //MessageBox("You can not delete this row because reference from another process !");
        //    }    
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_EditView_Click(object sender, EventArgs e)
        //{
        //    if (ddl_View.SelectedItem != null)
        //    { 
        //        Response.Redirect("VendorView.aspx?action=edit&id=" + ddl_View.SelectedItem.Value);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lnkb_Search_Click(object sender, EventArgs e)
        //{ 

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_NewView_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("VendorView.aspx?action=new");
        //}        

        /// <summary>
        /// Create new vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lnkb_New_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("VendorEdit.aspx?action=new");
        //}

        /// <summary>
        /// Get horizental alignment from DataColumn.DataType
        /// </summary>
        /// <param name="grdColumn"></param>
        /// <returns></returns>
        //private HorizontalAlign GetColumnHorizentalAlign(DataColumn grdColumn)
        //{
        //    switch (grdColumn.DataType.Name.ToString().ToUpper())
        //    {
        //        case "BYTE":
        //        case "DECIMAL":
        //        case "DOUBLE":
        //        case "INT16":
        //        case "INT32":
        //        case "INT64":
        //        case "SBYTE":
        //        case "SINAPE":
        //        case "UINT16":
        //        case "UINT32":
        //        case "UINT64":
        //            return HorizontalAlign.Right;
        //            break;
        //        default:
        //            return HorizontalAlign.Left;
        //            break;
        //    }
        //}

        #endregion
    }
}