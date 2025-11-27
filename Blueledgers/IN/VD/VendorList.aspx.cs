using System;
using System.Collections;
using System.Data;
using BlueLedger.PL.BaseClass;
//using System.Net.Http;
//using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

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

        private DataTable _dtCategory
        {
            get
            {
                return ViewState["_dtCategory"] as DataTable;
            }

            set
            {
                ViewState["_dtCategory"] = value;
            }
        }

        #region "Operations"


        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            var mode = Request.QueryString["mode"];

            panel_List.Visible = false;
            panel_Category.Visible = false;

            // Retrieve data and binding with controls
            if (!IsPostBack)
            {
                var btn = new DevExpress.Web.ASPxMenu.MenuItem("Vendor Category", "VendorCategory");

                btn.NavigateUrl = "~/IN/VD/VendorList.aspx?mode=category";
                btn.ItemStyle.ForeColor = System.Drawing.Color.White;
                ListPage.menuItems.Insert(0, btn);

                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Vendor List", "VL"));
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Vendor Detail", "VD"));
                ListPage.DataBind();



                if (mode != null && mode.ToString().ToLower() == "category")
                {
                    GetVendorCategory();

                    panel_Category.Visible = true;
                }
                else
                {
                    panel_List.Visible = true;
                }
                Bind_VendorCategory();

            }
            else
            {
                dsVendorList = (DataSet)Session["dsVendorList"];

                if (mode != null && mode.ToString().ToLower() == "category")
                {
                    panel_Category.Visible = true;
                }
                else
                {
                    panel_List.Visible = true;
                }
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
            Binding_View();
            Binding_VendorList();
        }


        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
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
            var error = SyncVendor();
            
            if (string.IsNullOrEmpty(error))
            {
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void btn_Category_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/IN/VD/VendorList.aspx");
        }

        protected void btn_Category_Create_Click(object sender, EventArgs e)
        {
            txt_CategoryCode_New.Text = "";
            txt_CategoryName_New.Text = "";
            chk_CategoryIsActive_New.Checked = true;

            pop_Category_Create.ShowOnPageLoad = true;
        }

        protected void gv_Category_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditIndex = e.NewEditIndex;
            Bind_VendorCategory();
        }

        protected void gv_Category_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;


            gv.EditIndex = -1;
            Bind_VendorCategory();
        }

        protected void gv_Category_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gv = sender as GridView;
            var chk = gv.Rows[e.RowIndex].FindControl("chk_IsActive") as CheckBox;

            var code = gv.DataKeys[e.RowIndex].Value.ToString();
            var name = (gv.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
            var isActive = chk.Checked;

            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Code",code),
                new Blue.DAL.DbParameter("@Name",name),
                new Blue.DAL.DbParameter("@IsActive",isActive ? "1" :"0"),
                new Blue.DAL.DbParameter("@LoginName",LoginInfo.LoginName),
            };
            var query = "UPDATE AP.VendorCategory SET [Name]=@name, IsActive=@IsActive, UpdatedDate=GETDATE(), UpdatedBy=@LoginName WHERE VendorCategoryCode=@code";
            config.DbExecuteQuery(query, parameters, LoginInfo.ConnStr);

            gv.EditIndex = -1;
            GetVendorCategory();
            Bind_VendorCategory();
        }

        protected void gv_Category_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gv = sender as GridView;
            var code = gv.DataKeys[e.RowIndex].Value.ToString();

            var dtCount = config.DbExecuteQuery("SELECT COUNT(VendorCategoryCode) FROM [AP].Vendor WHERE VendorCategoryCode=@code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@code", code) }, LoginInfo.ConnStr);

            if (dtCount != null && dtCount.Rows.Count > 0)
            {
                var count = Convert.ToInt32(dtCount.Rows[0][0]);

                if (count > 0)
                {
                    lbl_Pop_Alert.Text = string.Format("This code '{0}' is using in some vendors.", code);
                    pop_Alert.ShowOnPageLoad = true;

                    return;
                }
            }

            hf_VendorCategoryCode.Value = code;
            lbl_Category_ConfirmDelete.Text = string.Format("Do you want to delete this category '{0}'", code);
            pop_Category_ConfirmDelete.ShowOnPageLoad = true;
        }

        protected void btn_Category_ConfirmDelete_Yes_Click(object sender, EventArgs e)
        {
            var code = hf_VendorCategoryCode.Value;

            config.DbExecuteQuery("DELETE FROM AP.VendorCategory WHERE VendorCategoryCode=@code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@code", code) }, LoginInfo.ConnStr);

            GetVendorCategory();
            Bind_VendorCategory();

            pop_Category_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_Pop_Category_Create_Save_Click(object sender, EventArgs e)
        {
            var code = txt_CategoryCode_New.Text.Trim().ToUpper();
            var name = string.IsNullOrEmpty(txt_CategoryName_New.Text.Trim())
                ? txt_CategoryCode_New.Text.Trim()
                : txt_CategoryName_New.Text.Trim();
            var isActive = chk_CategoryIsActive_New.Checked;

            var dt = config.DbExecuteQuery("SELECT VendorCategoryCode FROM AP.VendorCategory WHERE VendorCategoryCode=@code", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@code", code) }, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                lbl_Pop_Alert.Text = string.Format("This code '{0}' has already exist.", code);
                pop_Alert.ShowOnPageLoad = true;
                return;
            }

            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Code",code),
                new Blue.DAL.DbParameter("@Name",name),
                new Blue.DAL.DbParameter("@IsActive",isActive ? "1" :"0"),
                new Blue.DAL.DbParameter("@LoginName",LoginInfo.LoginName),
            };
            var query = "INSERT INTO AP.VendorCategory (VendorCategoryCode,[Name],IsActive, UpdatedDate, UpdatedBy, CreatedDate, CreatedBy) VALUES (@code,@name,@IsActive,GETDATE(),@LoginName,GETDATE(),@LoginName)";
            config.DbExecuteQuery(query, parameters, LoginInfo.ConnStr);


            GetVendorCategory();
            Bind_VendorCategory();

            pop_Category_Create.ShowOnPageLoad = false;
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

        private void GetVendorCategory()
        {
            _dtCategory = config.DbExecuteQuery("SELECT * FROM [AP].VendorCategory ORDER BY VendorCategoryCode", null, LoginInfo.ConnStr);
        }

        private void Bind_VendorCategory()
        {

            gv_Category.DataSource = _dtCategory;
            gv_Category.DataBind();
        }

        private string SyncVendor()
        {
            var error = "";
            
            try
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
                            //var vnTaxNo = item.VnTaxNo;
                            var vnTaxNo = string.IsNullOrEmpty(item.VnTaxNo) ? "" : item.VnTaxNo.Replace("\'", "\'\'");
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
                            sql += string.Format("@VendorCode=N'{0}',", vnCode);
                            sql += string.Format("@CategoryCode=N'{0}',", vnCateCode);
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

                        //lbl_Title.Text = string.Join("; ", script);


                        using (SqlConnection connection = new SqlConnection(LoginInfo.ConnStr))
                        {
                            SqlCommand command = new SqlCommand(string.Join("; ", script), connection);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                Response.Write(string.Format("<script>console.log(`{0}`);</script>", error));
            }

            return error;
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




        #endregion
    }
}