using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

using System.Data.SqlClient;
using Blue.BL;
using System.Collections.Generic;
using System.Globalization;

namespace BlueLedger.PL.PC.PR
{
    public partial class PrEdit : BasePage
    {
        private int decimalPoint = 2;
        private decimal Price;

        public string HiddenClassName { get; private set; }

        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.ProdCateType ProdCateType = new Blue.BL.Option.Inventory.ProdCateType();
        private readonly Blue.BL.Profile.Address address = new Blue.BL.Profile.Address();
        private readonly Blue.BL.Profile.BankAccount bankAccount = new Blue.BL.Profile.BankAccount();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config conf = new Blue.BL.APP.Config();
        private readonly Blue.BL.Profile.Contact contact = new Blue.BL.Profile.Contact();
        private readonly Blue.BL.Profile.ContactDetail contactDetail = new Blue.BL.Profile.ContactDetail();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.ADMIN.Department dep = new Blue.BL.ADMIN.Department();
        private readonly DataSet dsPriceList = new DataSet();
        private readonly Blue.BL.AP.InvoiceDefault invoiceDefault = new Blue.BL.AP.InvoiceDefault();
        private readonly Blue.BL.AP.InvoiceDefaultDetail invoiceDefaultDetail = new Blue.BL.AP.InvoiceDefaultDetail();
        private readonly Blue.BL.Import.JobCode jobCode = new Blue.BL.Import.JobCode();
        private readonly Blue.BL.AP.PaymentDefault paymentDefault = new Blue.BL.AP.PaymentDefault();
        private readonly Blue.BL.AP.PaymentDefaultAuto paymentDefaultAuto = new Blue.BL.AP.PaymentDefaultAuto();
        private readonly Blue.BL.AP.PaymentDefaultCash paymentDefaultCash = new Blue.BL.AP.PaymentDefaultCash();
        private readonly Blue.BL.AP.PaymentDefaultCheq paymentDefaultCheq = new Blue.BL.AP.PaymentDefaultCheq();
        private readonly Blue.BL.AP.PaymentDefaultCredit paymentDefaultCredit = new Blue.BL.AP.PaymentDefaultCredit();
        private readonly Blue.BL.AP.PaymentDefaultTrans paymentDefaultTrans = new Blue.BL.AP.PaymentDefaultTrans();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PL.PL pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt plDt = new Blue.BL.PC.PL.PLDt();
        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.PriceList priceList = new Blue.BL.IN.PriceList();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Profile.Profile profile = new Blue.BL.Profile.Profile();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.AP.VendorActiveLog vendorActiveLog = new Blue.BL.AP.VendorActiveLog();
        private readonly Blue.BL.AP.VendorAttachment vendorAttachment = new Blue.BL.AP.VendorAttachment();
        private readonly Blue.BL.AP.VendorComment vendorComment = new Blue.BL.AP.VendorComment();
        private readonly Blue.BL.AP.VendorDefaultWHT vendorDefaultWHT = new Blue.BL.AP.VendorDefaultWHT();
        private readonly Blue.BL.AP.VendorMisc vendorMisc = new Blue.BL.AP.VendorMisc();
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private Blue.BL.GnxLib GnxLib = new Blue.BL.GnxLib();
        private object Sender = new object();
        private Blue.BL.Profile.AddressPart addressPart = new Blue.BL.Profile.AddressPart();
        private Blue.BL.Profile.AddressType addressType = new Blue.BL.Profile.AddressType();
        private Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();
        private Blue.BL.dbo.User buyer = new Blue.BL.dbo.User();
        private Blue.BL.Profile.ContactCategory contactCategory = new Blue.BL.Profile.ContactCategory();
        private Blue.BL.Profile.ContactType contactType = new Blue.BL.Profile.ContactType();
        private DataSet dsPR = new DataSet();
        private DataSet dsPriceCompare = new DataSet();
        private DataSet dsVendor = new DataSet();
        private DataSet dsWF = new DataSet();
        private Blue.BL.Reference.PaymentMethod paymentMethod = new Blue.BL.Reference.PaymentMethod();

        //private string prNo = string.Empty;
        private Blue.BL.Option.Inventory.ProdCat productCate = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private Blue.BL.AP.VendorCategory vendorCategory = new Blue.BL.AP.VendorCategory();

        // Added on: 07/08/2017, On:  Fon.
        private Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();


        private string MODE
        {
            get { return Request.QueryString["MODE"]; }
        }

        private string BuCode
        {
            get { return Request.QueryString["BuCode"]; }
        }


        private bool WorkFlowEnable
        {
            get { return workFlow.GetIsActive("PC", "PR", hf_ConnStr.Value); }
        }

        private int wfId
        {
            get { return viewHandler.GetWFId(int.Parse(Request.Cookies["[PC].[vPrList]"].Value), hf_ConnStr.Value); }
            //get { return viewHandler.GetWFId(int.Parse(Request.Params["VID"].ToString()), hf_ConnStr.Value); }
        }

        private int wfStep
        {
            get { return viewHandler.GetWFStep(int.Parse(Request.Cookies["[PC].[vPrList]"].Value), hf_ConnStr.Value); }
            //get { return viewHandler.GetWFStep(int.Parse(Request.Params["VID"].ToString()), hf_ConnStr.Value); }
        }

        private string PRDtEditMode
        {
            get { return ViewState["PRDtEditMode"].ToString(); }
            set { ViewState["PRDtEditMode"] = value; }
        }

        private string HOD
        {
            get { return ViewState["HOD"].ToString(); }
            set { ViewState["HOD"] = value; }
        }

        // Added on: 06/09/2017, By: Fon
        private string baseCurrency
        {
            get { return conf.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }

        private string ACTION
        {
            get { return Request.QueryString["ACTION"]; }
        }
        // End Added.

        // Added on: 10/11/2017, By: Fon
        private string IsSingleLocation
        { get { return conf.GetValue("PC", "PR", "SingleLocation", hf_ConnStr.Value); } }

        private string LimitDetail
        { get { return conf.GetValue("PC", "PR", "LimitDetail", hf_ConnStr.Value); } }
        // End Aded.

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {

                CheckUserRequired();

                btn_PriceList.Attributes.Add("onclick", "javascript:return OpenNewWindow()");
                Page_Retrieve();
            }
            else
            {
                dsWF = (DataSet)Session["dsWF"];
                dsPR = (DataSet)Session["dsPR"];
            }

            if (LoginInfo.BuInfo.IsHQ)
            {
                HiddenClassName = "show"; // Display BU Code

            }
            else
            {
                HiddenClassName = "hidden"; // Hide BuCode
            }
        }

        private void Page_Retrieve()
        {
            #region Mode: New
            if (MODE.ToUpper() == "NEW")
            {
                var result = pr.GetStructure(dsPR, hf_ConnStr.Value);
                var resultDt = prDt.GetStructure(dsPR, hf_ConnStr.Value);

                if ((!result) || (!resultDt))
                {
                    // Display Error Message
                    return;
                }

                var drNew = dsPR.Tables[pr.TableName].NewRow();

                drNew["PrNo"] = pr.GetNewID(ServerDateTime, LoginInfo.ConnStr);
                drNew["PrDate"] = ServerDateTime;

                if (WorkFlowEnable)
                {
                    drNew["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", hf_ConnStr.Value);
                }

                // Modified on: 19/10/2017, By: Fon, Because Impact case show that useless.
                //drNew["HOD"] = dep.GetHeadOfDep(LoginInfo.DepartmentCode, hf_ConnStr.Value);
                drNew["HOD"] = string.Empty;
                // End Modified.

                //drNew["HODRE"]          = ;
                drNew["DocStatus"] = "In Process";
                drNew["IsVoid"] = false;
                drNew["CreatedDate"] = ServerDateTime;
                drNew["CreatedBy"] = LoginInfo.LoginName;
                drNew["UpdatedDate"] = ServerDateTime;
                drNew["UpdatedBy"] = LoginInfo.LoginName;

                // Add new row
                dsPR.Tables[pr.TableName].Rows.Add(drNew);

                GetAppWFdt();
                GetUserProdCateType();
                Session["dsWF"] = dsWF;
                Session["dsPr"] = dsPR;
            }
            #endregion

            #region Mode: Edit
            if (MODE.ToUpper() == "EDIT")
            {
                var MsgError = string.Empty;

                // Get invoice no from HTTP query string
                var PrNo = Request.QueryString["ID"];

                var result = pr.GetListByPrNo(dsPR, ref MsgError, PrNo, hf_ConnStr.Value);

                if (!result)
                {
                    // Display Error Message
                    return;
                }

                var resultDt = prDt.GetListBy_PrNo(dsPR, PrNo, hf_ConnStr.Value);

                if (!resultDt)
                {
                    // Display Error Message
                    return;
                }
                GetAppWFdt();
                GetUserProdCateType();
                Session["dsWF"] = dsWF;
                Session["dsPr"] = dsPR;
            }
            #endregion

            #region Mode: Template
            if (MODE.ToUpper() == "TEMPLATE")
            {
                dsPR = (DataSet)Session["dsTemplate"];

                GetAppWFdt();
                GetUserProdCateType();
                Session["dsWF"] = dsWF;
                Session["dsPr"] = dsPR;
            }
            #endregion


            ddl_PrType.DataSource = dsWF.Tables["UserProdCateType"];
            ddl_PrType.DataBind();


            Page_Setting();
        }

        private void Page_Setting()
        {
            // Display current process description
            if (Request.Cookies["[PC].[vPrList]"] != null)
            {
                if (Request.Cookies["[PC].[vPrList]"].Value != string.Empty)
                {
                    lbl_Process.Text = viewHandler.GetDesc(int.Parse(Request.Cookies["[PC].[vPrList]"].Value),
                        hf_ConnStr.Value);
                }

                //Disable Change All Delivery Date.
                if (lbl_Process.Text == "View All")
                {
                    dte_DeliDate.Enabled = false;
                    menu_OKBar.Items[0].Enabled = false;
                }
            }


            var drPr = dsPR.Tables[pr.TableName].Rows[0];

            txt_Ref.Text = (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "TEMPLATE"
                ? string.Empty
                : drPr["PrNo"].ToString());


            if (Request.Params["Type"].ToUpper() == "C") // Type Create
            {
                if (hf_PrType.Value == string.Empty)
                {
                    if (ddl_PrType.Items.Count > 0)
                        hf_PrType.Value = ddl_PrType.Items[0].Value.ToString();
                }
                //ddl_PrType.SelectedValue = hf_PrType.Value;
                ddl_PrType.Value = hf_PrType.Value;
            }
            else if (Request.Params["Type"].ToUpper() == "M") // Type Market List
            {
                //ddl_PrType.SelectedValue = "1";
                //hf_PrType.Value = ddl_PrType.SelectedValue;
                ddl_PrType.Value = "1";
                hf_PrType.Value = ddl_PrType.Value.ToString();
            }
            else if (Request.Params["Type"].ToUpper() == "O") // Type Non Market List
            {
                //ddl_PrType.SelectedValue = "2";
                ddl_PrType.Value = "2";
                //hf_PrType.Value = ddl_PrType.SelectedValue;
                hf_PrType.Value = ddl_PrType.Value.ToString();
            }
            else
            {
                //ddl_PrType.SelectedValue = drPr["PrType"].ToString();
                //hf_PrType.Value = ddl_PrType.SelectedValue;
                ddl_PrType.Value = drPr["PrType"].ToString();
                hf_PrType.Value = ddl_PrType.Value.ToString();

            }

            ddl_JobCode.Value = drPr["AddField1"].ToString();
            if (ddl_JobCode.Value != null)
            {
                var s = ddl_JobCode.Value.ToString();
                var ds = new JobCodeLookup().GetRecord(s, LoginInfo.ConnStr);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_JobCode.Text = string.Format("{0}:{1}", ds.Tables[0].Rows[0]["Code"],
                        ds.Tables[0].Rows[0]["Description"]);
                    ddl_JobCode.ToolTip = ddl_JobCode.Text;
                }
                else
                {
                    ddl_JobCode.Text = "";
                }
            }
            //txt_PrDate.Text = DateTime.Parse(drPr["PrDate"].ToString()).ToString("dd/MM/yyyy");
            txt_PrDate.Text = Convert.ToDateTime(drPr["PrDate"]).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            txt_PrDate.Enabled = (MODE.ToUpper() == "NEW"
                //? workFlowDt.IsEnableEdit(wfId, wfStep, "PC.Pr.PRDate", hf_ConnStr.Value)
                ? IsExistInField("PC.Pr.PRDate", "EnableField")
                : false);


            hf_PrDate.Value = period.GetLatestOpenStartDate(hf_ConnStr.Value).ToString();


            lbl_Requestor.Text = drPr["CreatedBy"].ToString();

            lbl_Status.Text = drPr["DocStatus"].ToString();

            txt_Desc.Text = drPr["Description"].ToString();

            ProcessStatus.ApprStatus = drPr["ApprStatus"].ToString();
            ProcessStatus.Visible = (MODE.ToUpper() == "NEW" ? false : true);

            // Date For Chage All Delivery Date.
            dte_DeliDate.Date = ServerDateTime;

            // gridview New expand
            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.DataBind();

            // Enable/Disable Allocate Vendor
            DataRow drWFDt = dsWF.Tables["APPwfdt"].Rows[0];


            if (Convert.ToBoolean(drWFDt["IsAllocateVendor"]))
            {
                btn_AutoAllocateVd.Visible = true;
                btn_PriceList.Enabled = true;

            }
            else
            {
                btn_AutoAllocateVd.Visible = false;
                btn_PriceList.Enabled = false;

            }

            // Enable/Disable Change All Delivery Date.
            if (dsWF.Tables["APPwfdt"].Rows[0]["EnableField"].ToString().Contains("PC.PrDt.ReqDate"))
            {
                dte_DeliDate.Enabled = true;
                menu_OKBar.Items[0].Enabled = true;
            }
            else
            {
                dte_DeliDate.Enabled = false;
                menu_OKBar.Items[0].Enabled = false;
            }

            //Visible Create and Delete Button.
            if (wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr))
            {
                menu_GrdBar.Items[0].Visible = true; // Create
                menu_GrdBar.Items[1].Visible = true; // Delete

                menu_CmdBar.Items.FindByName("Commit").Visible = true;

            }
            else
            {
                menu_GrdBar.Items[0].Visible = false; // Create
                menu_GrdBar.Items[1].Visible = false; // Delete

                menu_CmdBar.Items.FindByName("Commit").Visible = false;

            }


            if (dsPR.Tables[pr.TableName].Rows[0]["ApprStatus"].ToString().Contains('P'))
            {
                menu_GrdBar.Items[0].Enabled = false; // Create
                menu_GrdBar.Items[1].Enabled = false; // Delete
            }

            if (grd_PrDt1.Rows.Count > 0)
            {
                dte_DeliDate.Visible = true;
                menu_OKBar.Visible = true;
            }
            else
            {
                dte_DeliDate.Visible = false;
                menu_OKBar.Visible = false;
            }

            // Added on: 06/09/2017, By: Fon, For: action "Commit" in Pr.aspx
            if (ACTION != null)
            {
                if (ACTION.ToUpper() == "C")
                    Save("Commit");
            }
            // End Added.


        }

        protected void GetAppWFdt()
        {
            if (dsWF.Tables["APPwfdt"] != null)
                dsWF.Tables.Remove(dsWF.Tables["APPwfdt"]);

            DataTable dt = new DataTable("APPwfdt");
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();

            string sqlStr = "select * from [App].WFDt where [Step] = '" + wfStep + "' ";
            sqlStr += "and [WFId] = '" + wfId + "' ";
            SqlCommand cmd = new SqlCommand(sqlStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            dsWF.Tables.Add(dt);
        }

        protected void GetUserProdCateType()
        {
            if (dsWF.Tables["UserProdCateType"] != null)
                dsWF.Tables.Remove(dsWF.Tables["UserProdCateType"]);

            DataTable dt = new DataTable("UserProdCateType");
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();
            string sqlStr = "EXEC [IN].[ProdCateType_GetActivedListByLoginName] '" + LoginInfo.LoginName + "' ";
            SqlCommand cmd = new SqlCommand(sqlStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            dsWF.Tables.Add(dt);
        }

        protected bool IsExistInField(string search, string fieldName)
        {
            if (dsWF.Tables["APPwfdt"].Rows.Count > 0)
            {
                DataRow dr = dsWF.Tables["APPwfdt"].Rows[0];
                if (dr[fieldName].ToString().Contains(search.Trim()))
                    return true;
            }
            return false;
        }

        //Create Pr Detail.
        protected void CreateDetail()
        {

            int limitDetails = 0;
            if (LimitDetail != string.Empty)
            { limitDetails = Convert.ToInt32(LimitDetail); }

            // < : create new detail, = : cannot
            if (LimitDetail == string.Empty
                || grd_PrDt1.Rows.Count < limitDetails
                || limitDetails == 0)
            {
                #region Unlimit or Under-limit
                txt_PrDate.Enabled = false;
                //if (ddl_PrType.SelectedValue == null)
                if (ddl_PrType.Value == null)
                {
                    pop_AlertProdCateType.ShowOnPageLoad = true;
                    return;
                }

                dsPR = (DataSet)Session["dsPR"];
                for (var i = dsPR.Tables[prDt.TableName].Rows.Count; i > 0; i--)
                {
                    var item = dsPR.Tables[prDt.TableName].Rows[i - 1];
                    if (string.IsNullOrEmpty(item["ProductCode"].ToString()))
                    {
                        item.Delete();
                    }
                }


                // Enable Pr Type
                //ddl_PrType.Enabled = false;

                // Disable Save and Back button when click Save or Back.
                menu_CmdBar.Items.FindByName("Save").Visible = false;
                menu_CmdBar.Items.FindByName("Commit").Visible = false;
                menu_CmdBar.Items.FindByName("Back").Visible = true;

                // Disable Create and Delete Button When click Create or Delete. 
                menu_GrdBar.Items.FindByName("Create").Visible = false;
                menu_GrdBar.Items.FindByName("Delete").Visible = false;

                // Add new prdt row
                var drNew = dsPR.Tables[prDt.TableName].NewRow();

                drNew["PRNo"] = string.Empty;
                //drNew["PRDtNo"]         = dsPR.Tables[prDt.TableName].Rows.Count + 1;
                drNew["PRDtNo"] = (dsPR.Tables[prDt.TableName].Rows.Count == 0
                    ? 1
                    : int.Parse(
                        dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1]["PRDtNo"].ToString()) +
                      1);
                drNew["ProductCode"] = string.Empty;
                drNew["ReqQty"] = 0;
                drNew["ReqDate"] = DateTime.Now.AddDays(1);
                drNew["OrderQty"] = 0;
                drNew["ApprQty"] = 0;
                drNew["FOCQty"] = 0;
                drNew["RcvQty"] = 0;
                drNew["DiscPercent"] = 0;
                drNew["DiscAmt"] = 0;
                drNew["TaxAmt"] = 0;
                drNew["NetAmt"] = 0;
                drNew["TotalAmt"] = 0;
                drNew["TaxRate"] = 0;
                drNew["TaxAdj"] = false;
                drNew["Price"] = 0;
                drNew["ApprStatus"] = workFlow.GetDtApprStatus("PC", "PR", hf_ConnStr.Value);

                // Added on: 01/09/2017, By: Fon
                drNew["CurrNetAmt"] = 0;
                drNew["CurrDiscAmt"] = 0;
                drNew["CurrTaxAmt"] = 0;
                drNew["CurrTotalAmt"] = 0;
                drNew["CurrencyCode"] = baseCurrency;
                drNew["CurrencyRate"] = 1;
                // End Added.

                //Default New Line.
                if (dsPR.Tables[prDt.TableName].Rows.Count > 0) // copy from previous record.
                {
                    var drLastPrDt = dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1];

                    drNew["BuCode"] = drLastPrDt["BuCode"].ToString();
                    drNew["VendorCode"] = drLastPrDt["VendorCode"].ToString();
                    drNew["LocationCode"] = drLastPrDt["LocationCode"].ToString();
                    drNew["DeliPoint"] = drLastPrDt["DeliPoint"].ToString();
                    drNew["ReqDate"] = DateTime.Parse(drLastPrDt["ReqDate"].ToString());
                }
                else // first record
                {
                    if (LoginInfo.BuInfo.IsHQ)
                    {
                        drNew["BuCode"] = string.Empty;
                    }
                    else
                    {
                        drNew["BuCode"] = LoginInfo.BuInfo.BuCode;
                    }
                }

                dsPR.Tables[prDt.TableName].Rows.Add(drNew);

                // Gridview New Expand.
                grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
                grd_PrDt1.EditIndex = dsPR.Tables[prDt.TableName].Rows.Count - 1;
                grd_PrDt1.DataBind();

                PRDtEditMode = "NEW";
                Session["dsPR"] = dsPR;
                #endregion
            }
            else
            {
                lbl_PopupAlert.Text = string.Format("Purchase Request is limited {0} detail(s).", limitDetails);
                pop_Alert.HeaderText = "Warning (Purchase Request limit)";
                pop_Alert.ShowOnPageLoad = true;
            }
        }



        protected void DeleteDetail()
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        //Save Pr Header & Detail
        protected void Save(string action)
        {
            string _action = string.Empty;

            if (dsPR.Tables[prDt.TableName].Rows.Count == 0)
            {
                pop_Save.ShowOnPageLoad = true;
            }
            else
            {
                var drPr = dsPR.Tables[pr.TableName].Rows[0];

                if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "TEMPLATE")
                {
                    _action = "CREATE";

                    drPr["PrNo"] = pr.GetNewID(DateTime.Parse(txt_PrDate.Text), LoginInfo.ConnStr);
                    drPr["PrDate"] = DateTime.Parse(txt_PrDate.Text).AddHours(ServerDateTime.Hour).AddMinutes(ServerDateTime.Minute).AddSeconds(ServerDateTime.Second);
                    if (DBNull.Value.Equals(drPr["ApprStatus"]))
                        drPr["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", hf_ConnStr.Value);


                    foreach (DataRow drPrDt in dsPR.Tables[prDt.TableName].Rows)
                    {
                        drPrDt["PrNo"] = drPr["PrNo"].ToString();
                        drPrDt["AddField1"] = ddl_JobCode.SelectedIndex > -1 ? ddl_JobCode.Value.ToString() : string.Empty;
                        drPrDt["ApprQty"] = drPrDt["ReqQty"];

                        if (DBNull.Value.Equals(drPrDt["ApprStatus"]))
                            drPrDt["ApprStatus"] = workFlow.GetDtApprStatus("PC", "PR", hf_ConnStr.Value);
                    }

                    var dtUser = bu.DbExecuteQuery(string.Format("SELECT TOP(1) DepartmentCode FROM [ADMIN].vUser WHERE LoginName ='{0}'", LoginInfo.LoginName), null, hf_ConnStr.Value);
                    if (dtUser != null && dtUser.Rows.Count > 0)
                        drPr["HOD"] = dtUser.Rows[0][0].ToString();
                }

                if (MODE.ToUpper() == "EDIT")
                {
                    _action = "MODIFY";

                    if (DBNull.Value.Equals(drPr["ApprStatus"]))
                        drPr["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", hf_ConnStr.Value);

                    foreach (DataRow drPrDt in dsPR.Tables[prDt.TableName].Rows)
                    {
                        if (drPrDt.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        drPrDt["PrNo"] = drPr["PrNo"].ToString();
                        drPrDt["AddField1"] = ddl_JobCode.SelectedIndex > -1 ? ddl_JobCode.Value.ToString().Split(':')[0] : string.Empty;

                        if (DBNull.Value.Equals(drPrDt["ApprStatus"]))
                            drPrDt["ApprStatus"] = workFlow.GetDtApprStatus("PC", "PR", hf_ConnStr.Value);
                    }
                }

                drPr["PrType"] = ddl_PrType.SelectedItem.Value;
                drPr["Description"] = txt_Desc.Text;
                drPr["UpDatedDate"] = ServerDateTime;
                drPr["UpdatedBy"] = LoginInfo.LoginName;


                // Job Code
                drPr["AddField1"] = ddl_JobCode.SelectedIndex > -1 ? ddl_JobCode.Value.ToString() : string.Empty;

                // Remove PR Detail where ReqQty = 0
                for (var i = dsPR.Tables[prDt.TableName].Rows.Count - 1; i >= 0; i--)
                {
                    var drPrDt = dsPR.Tables[prDt.TableName].Rows[i];

                    if (dsWF.Tables["APPwfdt"].Rows[0]["EnableField"].ToString().Contains("PC.Pr.")
                        || dsWF.Tables["APPwfdt"].Rows[0]["EnableField"].ToString().Contains("PC.PrDt."))
                    {
                        if (drPrDt.RowState != DataRowState.Deleted)
                        {
                            if (decimal.Parse(drPrDt["ReqQty"].ToString()) == 0)
                            {
                                drPrDt.Delete();
                            }
                        }
                    }
                }

                // Check error before save
                foreach (DataTable dt in dsPR.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr.RowError))
                        {
                            AlertMessageBox(dr.RowError);
                            return;
                        }
                    }
                }

                var save = pr.Save(dsPR, bu.GetConnectionString(Request.Params["BuCode"]));

                object[] upAppr = new object[2];
                upAppr[0] = false;

                if (save)
                {
                    string refNo = drPr["PrNo"].ToString();

                    if (action.ToUpper() == "COMMIT")
                    {
                        // Update WorkFlowStatus
                        if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "TEMPLATE")
                        {
                            upAppr = UpdateApprStatus();
                        }

                        if (MODE.ToUpper() == "EDIT")
                        {
                            if (wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr))   //if (wfStep == 1)
                            {
                                upAppr = UpdateApprStatus();
                            }
                        }

                        if (wfStep <= 1)
                        {
                            SendEmailWorkflow.Send("A", refNo, 1, 1, LoginInfo.LoginName, hf_ConnStr.Value);
                        }
                    }


                    _transLog.Save("PC", "PR", refNo, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);


                    if (!pop_Alert.ShowOnPageLoad)
                        Response.Redirect("Pr.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + drPr["PrNo"] + "&VID=" + Request.Params["VID"]);
                }
            }
        }

        protected void Back()
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                Response.Redirect("~/PC/PR/Pr.aspx?BuCode=" + Request.Params["BuCode"] +
                                  "&ID=" + dsPR.Tables[pr.TableName].Rows[0]["PrNo"] +
                                  "&VID=" + Request.Params["VID"]);
            }
            else
            {
                Response.Redirect("~/PC/PR/PrList.aspx");
            }
        }

        //Update Data of Workflow Step 1
        //private void UpdateApprStatus()
        private object[] UpdateApprStatus()
        {
            object[] obj = new object[2];
            var dtWFDt = workFlowDt.Get(wfId, wfStep, hf_ConnStr.Value);

            var dbParams1 = new Blue.DAL.DbParameter[2];
            dbParams1[0] = new Blue.DAL.DbParameter("@PrNo", dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString());
            dbParams1[1] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

            workFlowDt.ExcecuteApprRule("APP.WF_PR_APPR_STEP_1", dbParams1, hf_ConnStr.Value);

            if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["SentEmail"]))
            {
                bool isSent = false;
                lbl_hide_action.Text = "Redirect".ToUpper();
                lbl_hide_value.Text = true.ToString();
                lbl_PrNo.Text = dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString();
                isSent = Control_SentEmail("A");

                obj[0] = isSent;
                obj[1] = "Sent Email";
            }
            return obj;
        }

        //Update Data of Workflow Step 3
        public void UpdateAllocateVendorApprStatus(string PRNo, int PRDtNo)
        {
            var dtWFDt = workFlowDt.Get(wfId, wfStep, hf_ConnStr.Value);

            var dbParams = new Blue.DAL.DbParameter[3];
            dbParams[0] = new Blue.DAL.DbParameter("@PrNo", PRNo);
            dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", PRDtNo.ToString());
            dbParams[2] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

            workFlowDt.ExcecuteApprRule(dtWFDt.Rows[0]["ApprRule"].ToString(), dbParams, hf_ConnStr.Value);
        }

        protected void ddl_BuCode_Load(object sender, EventArgs e)
        {
            //ViewState.Remove("ddl_BuCode");
            //ViewState.Remove("ddl_LocationCode");

            //var dsBu = new DataSet();

            //var result = bu.GetList(dsBu, LoginInfo.BuInfo.BuGrpCode);

            //if (result)
            //{
            //    var ddl_BuCode = sender as ASPxComboBox;
            //    ddl_BuCode.DataSource = dsBu.Tables[bu.TableName];
            //    ddl_BuCode.DataBind();
            //}

            var ddl_BuCode = sender as ASPxComboBox;
            ddl_BuCode.DataSource = bu.GetList(LoginInfo.BuInfo.BuGrpCode);
            ddl_BuCode.DataBind();

        }

        protected void ddl_BuCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_BuCode = sender as ASPxComboBox;
            ddl_BuCode.ToolTip = ddl_BuCode.Text;

            var ddl_LocationCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;
            if (ddl_LocationCode.Text != string.Empty)
            {
                string locationCode = ddl_LocationCode.Text.Split(':')[0].Trim();

                if (storeLct.StoreLctCountByLocationCode(locationCode, bu.GetConnectionString(ddl_BuCode.Text.Split(':')[0].Trim())) == 0)
                    ddl_LocationCode.Value = null;

                //var ddl_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
                //ddl_DeliPoint.Value = null;

            }


            //string old_BuCode = ViewState["ddl_BuCode"].ToString();
            //if ( != ddl_BuCode.Value.ToString())

            // Refresh Location list
            /*if (grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") != null)
            {
                var ddl_LocationCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;

                ddl_LocationCode.Value = null;
                //ddl_LocationCode.DataSource = storeLct.GetLookUp_ByCategoryType(ddl_PrType.SelectedItem.Value,
                ddl_LocationCode.DataSource = storeLct.GetLookUp_ByCategoryType(ddl_PrType.Value.ToString(), LoginInfo.LoginName, bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                ddl_LocationCode.DataBind();
            }*/

            //// Refresh Delivery Point list
            //if (grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") != null)
            //{
            //    var ddl_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
            //    ddl_DeliPoint.DataSource = deliPoint.GetList(bu.GetConnectionString(ddl_BuCode.Value.ToString()));
            //    ddl_DeliPoint.DataBind();
            //}


        }

        //protected void ddl_LocationCode_Load(object sender, EventArgs e)
        //{
        //    //var ddl_LocationCode = sender as ASPxComboBox;
        //    //var buCode =
        //    //    dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_LocationCode.NamingContainer).DataItemIndex][
        //    //        "BuCode"].ToString();

        //    //if (buCode != string.Empty)
        //    //{
        //    //    DataTable dt = storeLct.GetLookUp_ByCategoryType(ddl_PrType.SelectedItem.Value,
        //    //           LoginInfo.LoginName,
        //    //           bu.GetConnectionString(buCode));
        //    //    ddl_LocationCode.DataSource = dt;
        //    //    ddl_LocationCode.DataBind();

        //    //    hf_PrType.Value = ddl_PrType.SelectedValue;
        //    //}
        //}


        // Added on: 02/04/2018, By: Fon

        //protected bool IsBUHQActive()
        //{
        //    bool isHQBUactive = false;
        //    DataSet dsBU = new DataSet();
        //    string hqBUcode = bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode);

        //    if (hqBUcode != string.Empty) bu.GetList(dsBU, hqBUcode, System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString());
        //    if (dsBU.Tables.Contains(bu.TableName))
        //        if (dsBU.Tables[bu.TableName].Rows.Count > 0)
        //            isHQBUactive = (bool)dsBU.Tables[bu.TableName].Rows[0]["IsActived"];

        //    return isHQBUactive;
        //}

        // End Added.

        protected void ddl_DeliPoint_Load(object sender, EventArgs e)
        {
            var ddl_DeliPoint = sender as ASPxComboBox;
            var buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_DeliPoint.NamingContainer).DataItemIndex]["BuCode"].ToString();

            if (buCode != string.Empty)
            {
                ddl_DeliPoint.DataSource = deliPoint.GetList(bu.GetConnectionString(buCode));
                ddl_DeliPoint.DataBind();
            }
        }

        protected void ddl_BuCode_av_Load(object sender, EventArgs e)
        {
            var dsBuAv = new DataSet();

            var result = bu.GetList(dsBuAv, LoginInfo.BuInfo.BuGrpCode);

            if (result)
            {
                var ddl_BuCode_av = sender as ASPxComboBox;
                ddl_BuCode_av.DataSource = dsBuAv.Tables[bu.TableName];
                ddl_BuCode_av.DataBind();
            }
        }

        protected void ddl_LocationCode_OnItemRequestedByValue_SQL(object source,
           ListEditItemRequestedByValueEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            if (e.Value == null || e.Value == string.Empty || comboBox == null)
                return;

            var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
            string buCode = ddl_BuCode.Text.Split(':')[0].Trim();

            SqlDataSource1.ConnectionString = bu.GetConnectionString(buCode);

            string sqlText = "SELECT l.LocationCode, l.LocationName";
            sqlText += " FROM [IN].StoreLocation l";
            sqlText += " JOIN [ADMIN].UserStore us ON us.LocationCode = l.LocationCode AND us.LoginName = @LoginName";
            sqlText += " WHERE l.IsActive = 1";
            //sqlText += " AND l.LocationCode = @LocationCode";

            sqlText += " ORDER BY l.LocationCode";

            SqlDataSource1.SelectCommand = @sqlText;
            SqlDataSource1.SelectParameters.Clear();
            //SqlDataSource1.SelectParameters.Add("LocationCode", TypeCode.String, e.Value.ToString());
            SqlDataSource1.SelectParameters.Add("LoginName", TypeCode.String, LoginInfo.LoginName);
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        private DataTable GetLocations(string loginName, string connectionString)
        {
            string sql = "SELECT l.LocationCode, l.LocationName";
            sql += " FROM [IN].StoreLocation l";
            sql += " JOIN [ADMIN].UserStore us ON us.LocationCode = l.LocationCode";
            sql += " WHERE l.IsActive = 1";
            if (!string.IsNullOrEmpty(loginName))
                sql += "  AND us.LoginName = '" + loginName + "'";
            sql += " ORDER BY l.LocationCode";

            return bu.DbExecuteQuery(sql, null, connectionString);

        }

        protected void ddl_LocationCode_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;

            if (ddl_BuCode.Value == null)
                return;


            string buCode = ddl_BuCode.Text.Split(':')[0].Trim();

            SqlDataSource1.ConnectionString = bu.GetConnectionString(buCode);

            string sqlText = string.Empty;
            sqlText = "SELECT LocationCode, LocationName";
            sqlText += " FROM (";
            sqlText += "        SELECT l.LocationCode, l.LocationName, ROW_NUMBER() OVER(order by l.LocationCode) as rn";
            sqlText += "        FROM [IN].StoreLocation l";
            sqlText += "        JOIN [ADMIN].UserStore us ON us.LocationCode = l.LocationCode AND us.LoginName = @LoginName";
            sqlText += "        WHERE ( l.LocationCode + ' : ' + l.LocationName LIKE @filter)";
            sqlText += "          AND l.IsActive = 1";
            sqlText += "     ) as st";
            sqlText += " WHERE st.rn BETWEEN @startIndex AND @endIndex";



            SqlDataSource1.SelectCommand = @sqlText;


            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("LoginName", TypeCode.String, LoginInfo.LoginName);
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();

            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_LocationCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_LocationCode = sender as ASPxComboBox;
            ddl_LocationCode.ToolTip = ddl_LocationCode.Text;

            if (ddl_LocationCode.Value != null)
            {
                var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
                var ddl_ProductCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;
                var ddl_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
                var hf_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("hf_DeliPoint") as HiddenField;

                string buCode = ddl_BuCode.Text.Split(':')[0].Trim();
                string connString = bu.GetConnectionString(buCode);

                var LocationName = storeLct.GetName(ddl_LocationCode.Value.ToString(), connString);

                // Refresh Delivery Point list
                if (grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") != null)
                {
                    //var ddl_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
                    ddl_DeliPoint.DataSource = deliPoint.GetList(bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                    ddl_DeliPoint.DataBind();
                    ddl_DeliPoint.Value = storeLct.GetDeliveryPoint(ddl_LocationCode.Value.ToString(), connString);
                }

                // Added for: Check, is product in locations?
                if (ddl_ProductCode.Text != string.Empty)
                {
                    if (!IsProduct_InThisLocation(ddl_ProductCode.Text.Split(':')[0].Trim(), ddl_LocationCode.Value.ToString()))
                    {
                        TextBox txt_DescEn = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescEn");
                        TextBox txt_DescLL = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescLL");
                        txt_DescEn.Text = string.Empty;
                        txt_DescLL.Text = string.Empty;

                        if (LoginInfo.BuInfo.IsHQ == false)
                            ddl_ProductCode.Value = string.Empty;
                    }
                }

                hf_DeliPoint.Value = ddl_DeliPoint.Value.ToString();
            }

        }

        protected void ddl_ProductCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
        }

        protected void ddl_ProductCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;


            var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
            var ddl_LocationCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;
            var prType = ddl_PrType.SelectedItem.Value.ToString();

            if (ddl_BuCode.Value == null || ddl_LocationCode.Value == null)
                return;

            string buCode = ddl_BuCode.Text.Split(':')[0].Trim();
            string locationCode = ddl_LocationCode.Value.ToString();

            SqlDataSource1.ConnectionString = bu.GetConnectionString(buCode);


            if (LoginInfo.BuInfo.IsHQ && buCode != bu.GetHQBuCode(LoginInfo.BuInfo.BuGrpCode))
            {
                DataTable dt = bu.GetHQConnectionString(LoginInfo.BuInfo.BuGrpCode);
                string hqDBName = dt.Rows[0]["DatabaseName"].ToString();

                SqlDataSource1.SelectCommand =
                string.Format(@"SELECT ProductCode, ProductDesc1, ProductDesc2 
                                FROM (SELECT hq.ProductCode, hq.ProductDesc1, hq.ProductDesc2, row_number()over(order by hq.ProductCode) as [rn] 
                                        FROM [IN].Product p 
                                        LEFT JOIN [IN].ProductCategory pc ON pc.CategoryCode = p.ProductCate
                                        LEFT JOIN  {0}.[IN].Product hq ON hq.ProductCode = p.AddField10
                                        LEFT JOIN [IN].ProdLoc l ON l.ProductCode = p.ProductCode AND LocationCode = @LocationCode
                                        WHERE p.IsActive ='True' 
                                        AND pc.LevelNo = 3
						                AND pc.CategoryType = @PrType
                                        AND ( ISNULL(hq.ProductCode,'') + ' ' + ISNULL(hq.ProductDesc1,'') + ' ' + ISNULL(hq.ProductDesc2,'') LIKE @filter )
                                    ) as st 
                                WHERE st.[rn] between @startIndex and @endIndex ", hqDBName);

            }
            else
            {
                SqlDataSource1.SelectCommand =
                @"SELECT ProductCode, ProductDesc1, ProductDesc2 
                  FROM( SELECT p.ProductCode, p.ProductDesc1, p.ProductDesc2, row_number()over(order by p.[ProductCode]) as [rn] 
                        FROM [IN].Product p 
                        LEFT JOIN [IN].ProductCategory pc ON pc.CategoryCode = p.ProductCate
                        WHERE p.IsActive ='True' 
                        AND ( ISNULL(p.ProductCode, '') + ' ' + ISNULL(p.ProductDesc1,'') + ' ' + ISNULL(p.ProductDesc2,'') LIKE @filter )
                        AND pc.LevelNo = 3
						AND pc.CategoryType = @PrType
                        AND p.productcode IN (SELECT ProductCode FROM [IN].[ProdLoc] WHERE LocationCode = @LocationCode)
                      ) as st 
                 WHERE st.[rn] between @startIndex and @endIndex";
            }

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("PrType", TypeCode.String, prType.ToString());
            SqlDataSource1.SelectParameters.Add("LocationCode", TypeCode.String, locationCode);
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());

            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_ProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {

            var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
            var ddl_LocationCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;
            var ddl_ProductCode = sender as ASPxComboBox;
            var txt_DescEn = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescEn") as TextBox;
            var txt_DescLL = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescLL") as TextBox;
            var hf_ProductCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            var ddl_Unit = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;

            var productCode = ddl_ProductCode.Text.Split(':')[0].Trim();

            if (ddl_Unit != null)
            {
                ddl_Unit.DataSource = prodUnit.GetLookUp_OrderUnitByProductCode(ddl_ProductCode.ClientValue, bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                ddl_Unit.DataBind();
            }

            if (productCode != hf_ProductCode.Value)
            {
                txt_DescEn.Text = product.GetName(ddl_ProductCode.Text.Split(':')[0].Trim(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                txt_DescLL.Text = product.GetName2(ddl_ProductCode.Text.Split(':')[0].Trim(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                ddl_Unit.Value = prodUnit.GetDefaultOrderUnit(ddl_ProductCode.Text.Split(':')[0].Trim(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
            }

            hf_ProductCode.Value = productCode;
            //hf_ProductCode.Value = ddl_ProductCode.Text.Split(':')[0].Trim();

            // Get Product Information
            var items = txt_PrDate.Text.Split('/');
            var yyyy = Convert.ToInt32(items[2]);
            var mm = Convert.ToInt32(items[1]);
            var dd = Convert.ToInt32(items[0]);
            var prDate = new DateTime(yyyy, mm, dd);

            //var prDate = Convert.ToDateTime(txt_PrDate.Text, CultureInfo.InvariantCulture);
            var unitCode = ddl_Unit.Text.Trim();

            var info = GetProductInfo(productCode, unitCode, prDate);

            var row = grd_PrDt1.Rows[grd_PrDt1.EditIndex];

            var lbl_LastPrice = row.FindControl("lbl_LastPrice") as Label;
            var lbl_LastVendor = row.FindControl("lbl_LastVendor") as Label;

            lbl_LastPrice.Text = FormatAmt(info.LastPrice);
            lbl_LastVendor.Text = info.LastVendorCode + " : " + info.LastVendorName;
        }


        protected void ddl_BuCode_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_BuCode_av = sender as ASPxComboBox;

            // Refresh Location list
            if (grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode_av") != null)
            {
                var ddl_LocationCode_av =
                    grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode_av") as ASPxComboBox;
                ddl_LocationCode_av.DataSource = storeLct.GetList(LoginInfo.LoginName,
                    bu.GetConnectionString(ddl_BuCode_av.Value.ToString()));
                ddl_LocationCode_av.DataBind();
            }

            // Refresh Delivery Point list
            if (grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") != null)
            {
                var ddl_DeliPoint_av =
                    grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint_av") as ASPxComboBox;
                ddl_DeliPoint_av.DataSource = deliPoint.GetList(bu.GetConnectionString(ddl_BuCode_av.Value.ToString()));
                ddl_DeliPoint_av.DataBind();
            }
        }

        protected void ddl_LocationCode_av_Load(object sender, EventArgs e)
        {
            var ddl_LocationCode_av = sender as ASPxComboBox;
            var buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_LocationCode_av.NamingContainer).DataItemIndex]["BuCode"].ToString();
            var connStr = bu.GetConnectionString(buCode);
            if (buCode != string.Empty)
            {
                //ddl_LocationCode_av.DataSource = storeLct.GetLookUp_ByCategoryType(ddl_PrType.Value.ToString(), LoginInfo.LoginName, bu.GetConnectionString(buCode));
                ddl_LocationCode_av.DataSource = GetLocations(LoginInfo.LoginName, connStr);
                ddl_LocationCode_av.DataBind();
            }
        }

        protected void ddl_LocationCode_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
            var ddl_LocationCode_av = sender as ASPxComboBox;
            var ddl_ProductCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode_av") as ASPxComboBox;
            var ddl_Unit_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit_av") as ASPxComboBox;
            var ddl_DeliPoint_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint_av") as ASPxComboBox;
            var hf_DeliPoint_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("hf_DeliPoint_av") as HiddenField;


            var buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)(sender as ASPxComboBox).NamingContainer).DataItemIndex]["BuCode"].ToString();
            var connStr = bu.GetConnectionString(buCode);
            var locationCode = ddl_LocationCode_av.Value.ToString();
            //var LocationName = storeLct.GetName(ddl_LocationCode_av.Value.ToString(), hf_ConnStr.Value);
            var prType = ddl_PrType.Value.ToString();
            lbl_Title_Nm.Text = buCode;


            //Check Period            
            //if (period.GetIsValidDate(DateTime.Parse(txt_PrDate.Text), ddl_LocationCode_av.Value.ToString(),
            //    hf_ConnStr.Value))
            //{

            //Refresh Product list
            //            ddl_ProductCode_av.DataSource = product.GetLookUp_ByLocationCodeCateType(ddl_PrType.SelectedItem.Value,
            //ddl_ProductCode_av.DataSource = product.GetLookUp_ByLocationCodeCateType(ddl_PrType.Value.ToString(), ddl_LocationCode_av.Value.ToString(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
            ddl_ProductCode_av.DataSource = product.GetLookUp_ByLocationCodeCateType(prType, locationCode, connStr);
            ddl_ProductCode_av.DataBind();

            // Assing Delivery Point
            //ddl_DeliPoint_av.Value = storeLct.GetDeliveryPoint(ddl_LocationCode_av.Value.ToString(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
            if (ddl_DeliPoint_av.Value == null)
            {
                //ddl_DeliPoint_av.Value = storeLct.GetDeliveryPoint(ddl_LocationCode_av.Value.ToString(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                ddl_DeliPoint_av.Value = storeLct.GetDeliveryPoint(locationCode, connStr);
            }
            //if (ddl_DeliPoint_av.Value.ToString() == hf_DeliPoint_av.Value)
            //{
            //    //Assing Delivery Point
            //    ddl_DeliPoint_av.Value = storeLct.GetDeliveryPoint(ddl_LocationCode_av.Value.ToString(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
            //}

            hf_DeliPoint_av.Value = ddl_DeliPoint_av.Value.ToString();
        }

        protected void ddl_ProductCode_av_Load(object sender, EventArgs e)
        {
            //ASPxComboBox ddl_ProductCode_av = sender as ASPxComboBox;
            //string buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_ProductCode_av.NamingContainer).DataItemIndex]["BuCode"].ToString();
            //string locationCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_ProductCode_av.NamingContainer).DataItemIndex]["LocationCode"].ToString();

            //if (buCode != string.Empty && locationCode != string.Empty)
            //{
            //    ddl_ProductCode_av.DataSource = product.GetLookUp_ByLocationCodeCateType(ddl_PrType.SelectedItem.Value.ToString(), locationCode.ToString(),
            //     bu.GetConnectionString(buCode.ToString()));
            //    ddl_ProductCode_av.DataBind();
            //}
        }

        protected void ddl_ProductCode_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            var buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)(sender as ASPxComboBox).NamingContainer).DataItemIndex]["BuCode"].ToString();
            var ddl_LocationCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode_av") as ASPxComboBox;
            var ddl_ProductCode_av = sender as ASPxComboBox;
            var hf_ProductCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("hf_ProductCode_av") as HiddenField;
            var ddl_Unit_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit_av") as ASPxComboBox;
            var ddl_TaxType_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
            var connStr = bu.GetConnectionString(buCode);
            var productCode = ddl_ProductCode_av.Value.ToString().Split(':')[0].Trim();

            if (ddl_Unit_av != null)
            {
                ddl_Unit_av.DataSource = prodUnit.GetLookUp_OrderUnitByProductCode(productCode, connStr);
                ddl_Unit_av.DataBind();
            }

            if (ddl_ProductCode_av.Value.ToString() != hf_ProductCode_av.Value)
            {
                var dtProduct = product.GetProdList(productCode, connStr);

                ddl_Unit_av.Value = dtProduct.Rows[0]["OrderUnit"].ToString();
                ddl_TaxType_Grd_Av.Value = dtProduct.Rows[0]["TaxType"].ToString();
            }

            hf_ProductCode_av.Value = productCode;
        }

        protected void ddl_DeliPoint_av_Load(object sender, EventArgs e)
        {
            var ddl_DeliPoint_av = sender as ASPxComboBox;
            var buCode =
                dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_DeliPoint_av.NamingContainer).DataItemIndex][
                    "BuCode"].ToString();

            if (buCode != string.Empty)
            {
                ddl_DeliPoint_av.DataSource = deliPoint.GetList(bu.GetConnectionString(buCode));
                ddl_DeliPoint_av.DataBind();
            }
        }

        /// <summary>
        ///     Manual Assign vendor and price to pr detail from price list.
        ///     Compare PR detail's BU Code with Price list's BU Code
        ///     If PR detail's BU Code = Price list's BU Code, mean assign from price list which in the BU
        ///     Else assign from price list which in other BU. This case need to check existed Vendor in assigned BU
        ///     If not exist Vendor, need to add new vendor in the BU the assign price list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Assign_av_Click(object sender, EventArgs e)
        {
            //DataSet dsPriceList = new DataSet();

            var btn_Assign_av = sender as ASPxButton;
            var gvr_SelectedPriceList = btn_Assign_av.Parent.Parent as GridViewRow;

            // Find Controll Grid Detail
            var grd_PR = btn_Assign_av.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent as GridViewRow;

            var lbl_ReqQty_av = grd_PR.FindControl("lbl_ReqQty_av") as Label;
            var lbl_ApprQty_av = (Label)grd_PR.FindControl("lbl_ApprQty_av");
            var txt_ReqQty_av = grd_PR.FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            var txt_ApprQty_av = grd_PR.FindControl("txt_ApprQty_av") as ASPxSpinEdit;

            var apprQty = 0m;

            if (lbl_ReqQty_av != null)
            {
                apprQty = Convert.ToDecimal(string.IsNullOrEmpty(lbl_ApprQty_av.Text) ? lbl_ReqQty_av.Text : lbl_ApprQty_av.Text);
            }

            if (txt_ReqQty_av != null)
            {
                apprQty = txt_ApprQty_av.Number == 0m ? txt_ReqQty_av.Number : txt_ApprQty_av.Number;
            }

            var vendorCode = gvr_SelectedPriceList.Cells[2].Text.Split(':')[0].Trim();
            var vendorName = gvr_SelectedPriceList.Cells[2].Text.Split(':')[1].Trim();

            // Find PR detail bu code
            var drPrDt = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)gvr_SelectedPriceList.Parent.Parent.Parent.Parent.Parent.Parent).DataItemIndex];

            string price = gvr_SelectedPriceList.Cells[5].Text;
            string discPercent = gvr_SelectedPriceList.Cells[6].Text;
            string discAmt = gvr_SelectedPriceList.Cells[7].Text;
            string focQty = gvr_SelectedPriceList.Cells[8].Text;

            string taxType = gvr_SelectedPriceList.Cells[9].Text;
            string taxRate = gvr_SelectedPriceList.Cells[10].Text;
            string currencyCode = gvr_SelectedPriceList.Cells[11].Text;
            string currencyRate = gvr_SelectedPriceList.Cells[12].Text;

            price = string.IsNullOrEmpty(price) ? "0" : price;
            discPercent = string.IsNullOrEmpty(discPercent) ? "0" : discPercent;
            discAmt = string.IsNullOrEmpty(discAmt) ? "0" : discAmt;
            focQty = string.IsNullOrEmpty(focQty) ? "0" : focQty;

            taxType = string.IsNullOrEmpty(taxType) ? "N" : taxType;
            taxRate = string.IsNullOrEmpty(taxRate) ? "0" : taxRate;
            currencyCode = string.IsNullOrEmpty(currencyCode) ? baseCurrency : currencyCode;
            currencyRate = string.IsNullOrEmpty(currencyRate) ? "1" : currencyRate;




            // Assign Selected Price list to PR Detail.
            drPrDt["VendorCode"] = vendorCode.Trim();
            drPrDt["FOCQty"] = Convert.ToDecimal(focQty);
            drPrDt["Price"] = Convert.ToDecimal(price);
            drPrDt["TaxType"] = taxType;
            drPrDt["TaxRate"] = Convert.ToDecimal(taxRate);
            drPrDt["DiscPercent"] = Convert.ToDecimal(discPercent);
            drPrDt["DiscAmt"] = Convert.ToDecimal(discAmt);
            drPrDt["CurrencyCode"] = currencyCode;
            drPrDt["CurrencyRate"] = Convert.ToDecimal(currencyRate);

            //drPrDt["ApprQty"] = decimal.Parse(lbl_ApprQty_av.Text);
            drPrDt["ApprQty"] = apprQty;

            var DiscAmt = decimal.Parse(drPrDt["DiscAmt"].ToString());
            var DiscPercent = decimal.Parse(drPrDt["DiscPercent"].ToString());
            var ApprQty = decimal.Parse(drPrDt["ApprQty"].ToString());

            var priceRate = Convert.ToDecimal(price) * Convert.ToDecimal(currencyRate);

            // Update DiscPercent and DiscAmt                
            if (DiscAmt == 0 && DiscPercent > 0)
            {
                drPrDt["DiscAmt"] = ((priceRate * DiscPercent) / 100) * ApprQty;
            }
            else if (DiscAmt > 0 && DiscPercent > 0)
            {
                drPrDt["DiscAmt"] = ((priceRate * DiscPercent) / 100) * ApprQty;
            }
            else
            {
                drPrDt["DiscPerCent"] = (DiscAmt * 100) / (priceRate);
                DiscPercent = decimal.Parse(drPrDt["DiscPercent"].ToString());
            }


            // Net, Tax, Total
            //DiscAmt = (priceRate * DiscPercent) / 100; <-- This's mean per unit.
            // Modified on: 09/02/2018, By: Fon, For: Clearly varible. 
            var discPerUnit = (priceRate * DiscPercent) / 100;
            var calAmt = CalAmt(priceRate, DiscAmt, decimal.Parse(drPrDt["ApprQty"].ToString()));


            drPrDt["CurrNetAmt"] = NetAmt(drPrDt["TaxType"].ToString(), decimal.Parse(drPrDt["TaxRate"].ToString()), priceRate, discPerUnit, decimal.Parse(drPrDt["ApprQty"].ToString()));
            drPrDt["CurrDiscAmt"] = drPrDt["DiscAmt"];
            drPrDt["CurrTaxAmt"] = TaxAmt(drPrDt["TaxType"].ToString(), decimal.Parse(drPrDt["TaxRate"].ToString()), priceRate, discPerUnit, decimal.Parse(drPrDt["ApprQty"].ToString()));
            drPrDt["CurrTotalAmt"] = Amount(drPrDt["TaxType"].ToString(), decimal.Parse(drPrDt["TaxRate"].ToString()), priceRate, discPerUnit, decimal.Parse(drPrDt["ApprQty"].ToString()));

            decimal currRate = Convert.ToDecimal(drPrDt["CurrencyRate"].ToString());
            if (currRate == 0)
                currRate = 1;
            drPrDt["NetAmt"] = Convert.ToDecimal(drPrDt["CurrNetAmt"].ToString()) * currRate;
            drPrDt["DiscAmt"] = Convert.ToDecimal(drPrDt["CurrDiscAmt"].ToString()) * currRate;
            drPrDt["TaxAmt"] = Convert.ToDecimal(drPrDt["CurrTaxAmt"].ToString()) * currRate;
            drPrDt["TotalAmt"] = Convert.ToDecimal(drPrDt["CurrTotalAmt"].ToString()) * currRate;

            // End Modified.

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.DataBind();
        }

        protected void btn_ConfirmAddPriceLst_Click(object sender, EventArgs e)
        {
            var drPrDt = (DataRow)Session["drPrDt"];
            var drPriceList = (DataRow)Session["drPriceList"];
            var dsPriceList = (DataSet)Session["dsPriceList"];
            var dsAddPL = new DataSet();

            var getPL = pl.GetSchame(dsAddPL, bu.GetConnectionString(drPrDt["BuCode"].ToString()));

            if (!getPL)
            {
                return;
            }

            var getPLdt = plDt.GetSchame(dsAddPL, bu.GetConnectionString(drPrDt["BuCode"].ToString()));

            if (!getPLdt)
            {
                return;
            }

            var priceListNo = Convert.ToInt32(drPriceList[17]);
            var SeqNo = Convert.ToInt32(drPriceList[18]);
            var BuCode = drPriceList["BuCode"].ToString().Split(':')[0].Trim();

            // Check VendorCode in Properties.
            var getVendorCode = vendor.GetList(dsPriceList, drPriceList["VendorName"].ToString().Split(':')[1].Trim()
                , bu.GetConnectionString(drPrDt["BUCode"].ToString()));

            if (!getVendorCode)
            {
                return;
            }

            var drVendorCode = dsPriceList.Tables[vendor.TableName].Rows[0];

            var get = pl.Get(dsPriceList, priceListNo, bu.GetConnectionString(BuCode));

            if (!get)
            {
                return;
            }

            var getDT = plDt.GetList(dsPriceList, priceListNo, SeqNo, bu.GetConnectionString(BuCode));

            if (!getDT)
            {
                return;
            }

            //Add Header PL.
            var drPL = dsPriceList.Tables[pl.TableName].Rows[0];

            var drAddPL = dsAddPL.Tables[pl.TableName].NewRow();

            drAddPL["PriceLstNo"] = pl.GetNewID(bu.GetConnectionString(drPrDt["BuCode"].ToString()));
            drAddPL["RefNo"] = drPL["RefNo"].ToString();
            drAddPL["DateFrom"] = drPL["DateFrom"].ToString();
            drAddPL["DateTo"] = drPL["DateTo"].ToString();
            drAddPL["VendorCode"] = drVendorCode["VendorCode"].ToString();
            drAddPL["CreatedDate"] = ServerDateTime;
            drAddPL["CreatedBy"] = LoginInfo.LoginName;
            drAddPL["UpdatedDate"] = ServerDateTime;
            drAddPL["UpdatedBy"] = LoginInfo.LoginName;

            dsAddPL.Tables[pl.TableName].Rows.Add(drAddPL);

            //Add Detail PLDt.
            var drPLDt = dsPriceList.Tables[plDt.TableName].Rows[0];

            var drAddPLDt = dsAddPL.Tables[plDt.TableName].NewRow();

            drAddPLDt["PriceLstNo"] = drAddPL["PriceLstNo"].ToString();
            drAddPLDt["SeqNo"] = drPLDt["SeqNo"].ToString();
            drAddPLDt["ProductCode"] = drPLDt["ProductCode"].ToString();
            drAddPLDt["OrderUnit"] = drPLDt["OrderUnit"].ToString();
            drAddPLDt["VendorRank"] = drPLDt["VendorRank"].ToString();
            drAddPLDt["QtyFrom"] = drPLDt["QtyFrom"].ToString();
            drAddPLDt["QtyTo"] = drPLDt["QtyTo"].ToString();
            drAddPLDt["QuotedPrice"] = drPLDt["QuotedPrice"].ToString();
            drAddPLDt["MarketPrice"] = drPLDt["MarketPrice"].ToString();
            drAddPLDt["FOC"] = drPLDt["FOC"].ToString();
            drAddPLDt["Comment"] = drPLDt["Comment"].ToString();
            drAddPLDt["Discpercent"] = drPLDt["Discpercent"].ToString();
            drAddPLDt["DiscAmt"] = drPLDt["DiscAmt"].ToString();
            drAddPLDt["Tax"] = drPLDt["Tax"].ToString();
            drAddPLDt["TaxRate"] = drPLDt["TaxRate"].ToString();
            drAddPLDt["VendorProdCode"] = drPLDt["VendorProdCode"].ToString();
            drAddPLDt["NetAmt"] = drPLDt["NetAmt"].ToString();
            drAddPLDt["AvgPrice"] = drPLDt["AvgPrice"].ToString();
            drAddPLDt["LastPrice"] = drPLDt["LastPrice"].ToString();

            dsAddPL.Tables[plDt.TableName].Rows.Add(drAddPLDt);

            var SavePL = pl.Save(dsAddPL, bu.GetConnectionString(drPrDt["BuCode"].ToString()));

            if (SavePL)
            {
                pop_AddPriceLst.ShowOnPageLoad = false;
                Page_Setting();
            }
        }

        protected void btn_CancelAddPriceLst_Click(object sender, EventArgs e)
        {
            pop_AddPriceLst.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            for (var i = grd_PrDt1.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_PrDt1.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    dsPR.Tables[prDt.TableName].Rows[i].Delete();
                }
            }

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();

            pop_ConfirmDelete.ShowOnPageLoad = false;

            Session["dsPR"] = dsPR;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void txt_Price_av_NumberChanged(object sender, EventArgs e)
        {
            CalculationItem_av(grd_PrDt1.EditIndex);

            //UpdateNetAmount(); // On function is empty.
            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        protected void txt_ReqQty_av_NumberChanged(object sender, EventArgs e)
        {
            var editIndex = grd_PrDt1.EditIndex;
            var txt_ReqQty_av = grd_PrDt1.Rows[editIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            var txt_ApprQty_av = grd_PrDt1.Rows[editIndex].FindControl("txt_ApprQty_av") as ASPxSpinEdit;

            txt_ApprQty_av.Value = txt_ApprQty_av.Visible ? txt_ApprQty_av.Value : txt_ReqQty_av.Value;
            CalculationItem_av(grd_PrDt1.EditIndex);

            //lbl_Title_Nm.Text = txt_ApprQty_av.Text;

            //UpdateNetAmount(); // On function is empty.
            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        protected void txt_ApprQty_av_NumberChanged(object sender, EventArgs e)
        {
            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
            CalculationItem_av(grd_PrDt1.EditIndex);
        }

        protected void txt_TaxRate_Grd_Av_TextChanged(object sender, EventArgs e)
        {
            //UpdateNetAmount();
            CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        private void UpdateNetAmount()
        {
            #region Comment

            //decimal DiscAmt = 0;
            //decimal TaxRate = 0;
            //decimal Price = 0;
            //decimal ReqQty = 0;

            //ASPxSpinEdit txt_ReqQty_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;

            //if (!string.IsNullOrEmpty(txt_ReqQty_av.Text))
            //{
            //    ReqQty = decimal.Parse(txt_ReqQty_av.Text);
            //}

            //ASPxSpinEdit txt_NetAmt_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_NetAmt_av") as ASPxSpinEdit;
            //ASPxSpinEdit txt_TaxAmt_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_TaxAmt_av") as ASPxSpinEdit;
            //ASPxSpinEdit txt_TotalAmt_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_TotalAmt_av") as ASPxSpinEdit;
            //ASPxSpinEdit txt_Price_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;
            //ASPxSpinEdit txt_DiscAmt_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_DiscAmt_av") as ASPxSpinEdit;
            //ASPxSpinEdit txt_TaxRate_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("txt_TaxRate_av") as ASPxSpinEdit;
            //ASPxComboBox ddl_TaxTye_av = grd_PrDt.Rows[grd_PrDt.EditIndex].FindControl("ddl_TaxType_av") as ASPxComboBox;

            //if (!string.IsNullOrEmpty(txt_DiscAmt_av.Text))
            //{
            //    DiscAmt = decimal.Parse(txt_DiscAmt_av.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_TaxRate_av.Text))
            //{
            //    TaxRate = decimal.Parse(txt_TaxRate_av.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_Price_av.Text))
            //{
            //    Price = decimal.Parse(txt_Price_av.Text);
            //}

            //// Recalculate NetAmt
            //if (ddl_TaxTye_av.Value != null)
            //{
            //    switch (ddl_TaxTye_av.Value.ToString().ToUpper())
            //    {
            //        case "A":
            //            txt_NetAmt_av.Text = (((Price - DiscAmt) * ReqQty).ToString());
            //            txt_TaxAmt_av.Text = (((((Price - DiscAmt) * ReqQty) * TaxRate) / 100)).ToString();
            //            txt_TotalAmt_av.Text = (((Price - DiscAmt) * ReqQty) + (((((Price - DiscAmt) * ReqQty) * TaxRate) / 100))).ToString();
            //            break;

            //        case "I":
            //            txt_NetAmt_av.Text = (((Price - DiscAmt) * ReqQty) - ((((Price - DiscAmt) * TaxRate) / (100 + TaxRate)) * ReqQty)).ToString();
            //            txt_TaxAmt_av.Text = ((((Price - DiscAmt) * TaxRate) / (100 + TaxRate)) * ReqQty).ToString();
            //            txt_TotalAmt_av.Text = ((((Price - DiscAmt) - (((Price - DiscAmt) * TaxRate) / (100 + TaxRate))) * ReqQty) +
            //                                    ((((Price - DiscAmt) * TaxRate) / (100 + TaxRate)) * ReqQty)).ToString();
            //            break;

            //        default :
            //            txt_NetAmt_av.Text = (Price * ReqQty).ToString();
            //            txt_TaxAmt_av.Text = "0.00";
            //            txt_TotalAmt_av.Text = ((Price - DiscAmt) * ReqQty).ToString();
            //            break;
            //    }
            //}
            //else
            //{
            //    txt_NetAmt_av.Text = ((Price * ReqQty).ToString());
            //    txt_TotalAmt_av.Text = ((Price * ReqQty) - (DiscAmt * ReqQty)).ToString();
            //    txt_TaxAmt_av.Text = "0.00";
            //}

            #endregion
        }

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            pop_AlertTaxRate.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);

        }

        protected void btn_OK_Save_Click(object sender, EventArgs e)
        {
            pop_Save.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);

        }

        protected void btn_Price_OK_Click(object sender, EventArgs e)
        {
            pop_AlertPrice.ShowOnPageLoad = false;
        }

        //Auto Allocate Vendor
        protected void btn_AutoAllocateVd_Click(object sender, EventArgs e)
        {
            //pop_ConfirmAutoAlloVd.ShowOnPageLoad = true;
        }

        protected void btn_ConfirmAutoAlloVd_Click(object sender, EventArgs e)
        {
            var Allocate = 0;
            var NotAllocate = 0;

            for (var i = 0; i <= grd_PrDt1.Rows.Count - 1; i++)
            {
                var Chk_Item = (CheckBox)grd_PrDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                if (Chk_Item.Checked)
                {
                    var drUpdating = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.Rows[i].DataItemIndex];

                    if (drUpdating.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (drUpdating["ApprStatus"].ToString().Contains('R'))
                    {
                        continue;
                    }

                    var buConnStr = bu.GetConnectionString(drUpdating["BuCode"].ToString());

                    // Get Prefer Vendor data.

                    var productCode = drUpdating["ProductCode"].ToString();
                    var reqQty = decimal.Parse(drUpdating["ReqQty"].ToString());
                    var reqUnit = drUpdating["OrderUnit"].ToString();
                    var prDate = ddl_PrType.SelectedItem.Value.ToString() == "1"
                        ? DateTime.Parse(drUpdating["ReqDate"].ToString())
                        : DateTime.Parse(txt_PrDate.Text);

                    var isUsed = conf.GetValue("PC", "PR", "UseDeliveryDateForNonMarketList", hf_ConnStr.Value);

                    if (isUsed.ToUpper() == "TRUE" || isUsed == "1")
                    {
                        prDate = DateTime.Parse(drUpdating["ReqDate"].ToString());
                    }


                    var dtPriceList = priceList.GetList(productCode, prDate, reqQty, reqUnit, buConnStr);

                    // Update Pr Items data
                    #region dtPriceList record > 0

                    if (dtPriceList.Rows.Count > 0)
                    {
                        var drPriceList = dtPriceList.Rows[0];

                        drUpdating["VendorCode"] = drPriceList["VendorCode"].ToString();
                        drUpdating["RefNo"] = drPriceList["RefNo"].ToString();
                        drUpdating["Price"] = (drPriceList["Price"] != DBNull.Value
                            ? drPriceList["Price"].ToString()
                            : "0");
                        drUpdating["FOCQty"] = drPriceList["FOC"].ToString();
                        drUpdating["DiscPercent"] = (drPriceList["DiscountPercent"] != DBNull.Value
                            ? drPriceList["DiscountPercent"].ToString()
                            : "0");
                        drUpdating["DiscAmt"] = (drPriceList["DiscountAmt"] != DBNull.Value
                            ? drPriceList["DiscountAmt"].ToString()
                            : "0");
                        drUpdating["VendorProdCode"] = drPriceList["VendorProdCode"].ToString();
                        drUpdating["TaxType"] = drPriceList["Tax"].ToString();
                        drUpdating["TaxRate"] = (drPriceList["TaxRate"] != DBNull.Value
                            ? drPriceList["TaxRate"].ToString()
                            : "0");

                        var discAmt = decimal.Parse(drUpdating["DiscAmt"].ToString());
                        var discPercent = decimal.Parse(drUpdating["DiscPercent"].ToString());
                        var apprQty = decimal.Parse(drUpdating["ApprQty"].ToString());
                        if (apprQty == 0)
                            apprQty = reqQty;

                        drUpdating["ApprQty"] = apprQty;

                        // Update Net, Tax and Amount
                        // Added on: 09/08/2017, By: Fon
                        // var Price = decimal.Parse(drUpdating["Price"].ToString());
                        var price = decimal.Parse(drUpdating["Price"].ToString());
                        var priceRate = decimal.Parse(drUpdating["Price"].ToString())
                               * currency.GetLastCurrencyRate(drPriceList["CurrencyCode"].ToString()
                               , prDate, LoginInfo.ConnStr);

                        decimal currencyRate = 1;

                        if (drPriceList["CurrencyCode"].ToString() == string.Empty)
                        {
                            drPriceList["CurrencyCode"] = baseCurrency;
                            drPriceList["CurrencyRate"] = currencyRate;
                        }
                        else
                            currencyRate = decimal.Parse(drPriceList["CurrencyRate"].ToString());

                        drUpdating["CurrencyCode"] = drPriceList["CurrencyCode"].ToString();
                        drUpdating["CurrencyRate"] = drPriceList["CurrencyRate"].ToString();
                        // End Added.

                        // Update DiscPercent and DiscAmt                
                        if (discPercent > 0)
                        {
                            drUpdating["DiscAmt"] = ((priceRate * discPercent) / 100) * apprQty;

                        }
                        else
                        {
                            if (priceRate == 0)
                                drUpdating["DiscPerCent"] = 0;
                            else
                                drUpdating["DiscPerCent"] = (discAmt * 100) / (priceRate);

                            discPercent = decimal.Parse(drUpdating["DiscPerCent"].ToString());
                        }

                        //var DiscAmt = (priceRate * discPercent) / 100;
                        //discAmt = decimal.Parse(drUpdating["DiscAmt"].ToString());

                        // Net, Tax, Total
                        //var calAmt = CalAmt(priceRate, discAmt, apprQty);

                        decimal taxRate = decimal.Parse(drUpdating["TaxRate"].ToString());
                        string taxType = drUpdating["TaxType"].ToString();

                        decimal total = RoundAmt(price * apprQty);

                        decimal currDiscAmt = RoundAmt((price * discPercent / 100) * apprQty);
                        drUpdating["CurrDiscAmt"] = currDiscAmt;
                        drUpdating["DiscAmt"] = RoundAmt(currDiscAmt * currencyRate);

                        decimal currNetAmt = NetAmt(taxType, taxRate, total, currDiscAmt, 1);
                        drUpdating["CurrNetAmt"] = currNetAmt;
                        drUpdating["NetAmt"] = RoundAmt(currNetAmt * currencyRate);


                        decimal currTaxAmt = TaxAmt(taxType, taxRate, total, currDiscAmt, 1);
                        drUpdating["CurrTaxAmt"] = currTaxAmt;
                        drUpdating["TaxAmt"] = RoundAmt(currTaxAmt * currencyRate);


                        decimal currTotalAmt = Amount(taxType, taxRate, total, currDiscAmt, 1);
                        drUpdating["CurrTotalAmt"] = currTotalAmt;
                        drUpdating["TotalAmt"] = RoundAmt(currTotalAmt * currencyRate);

                        // End Added.
                        Allocate++;

                        //var drUpdating = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.Rows[i].DataItemIndex];
                        //if (drUpdating.RowState == DataRowState.Deleted)
                        //{
                        //    continue;
                        //}

                        //if (drUpdating["ApprStatus"].ToString().Contains('R'))
                        //{
                        //    continue;
                        //}

                        //// Default Approve Quantity by Request Quantity
                        ////drUpdating["ApprQty"] = drUpdating["ReqQty"].ToString();

                        //// Get Prefer Vendor data.
                        //var productCode = drUpdating["ProductCode"].ToString();
                        //var date = ddl_PrType.SelectedItem.Value.ToString() == "1"
                        //    ? DateTime.Parse(drUpdating["ReqDate"].ToString())
                        //    : DateTime.Parse(txt_PrDate.Text);
                        ////(product.GetApprovalLevel(drUpdating["ProductCode"].ToString(),
                        ////    bu.GetConnectionString(drUpdating["BuCode"].ToString())) == 2
                        ////    ? DateTime.Parse(drUpdating["ReqDate"].ToString())
                        ////    : DateTime.Parse(dsPR.Tables[pr.TableName].Rows[0]["PrDate"].ToString()));

                        //var reqQty = decimal.Parse(drUpdating["ReqQty"].ToString());
                        //var reqUnit = drUpdating["OrderUnit"].ToString();

                        //var dtPriceList = priceList.GetList(productCode, date, reqQty, reqUnit,
                        //    bu.GetConnectionString(drUpdating["BuCode"].ToString()));

                        //// Update Pr Items data
                        //#region dtPriceList count > 0
                        //if (dtPriceList.Rows.Count > 0)
                        //{
                        //    var drPriceList = dtPriceList.Rows[0];
                        //    drUpdating["VendorCode"] = drPriceList["VendorCode"].ToString();
                        //    drUpdating["RefNo"] = drPriceList["RefNo"].ToString();
                        //    drUpdating["Price"] = (drPriceList["Price"] != DBNull.Value
                        //        ? drPriceList["Price"].ToString()
                        //        : "0");
                        //    drUpdating["FOCQty"] = drPriceList["FOC"].ToString();
                        //    drUpdating["DiscPercent"] = (drPriceList["DiscountPercent"] != DBNull.Value
                        //        ? drPriceList["DiscountPercent"].ToString()
                        //        : "0");
                        //    drUpdating["DiscAmt"] = (drPriceList["DiscountAmt"] != DBNull.Value
                        //        ? drPriceList["DiscountAmt"].ToString()
                        //        : "0");
                        //    drUpdating["VendorProdCode"] = drPriceList["VendorProdCode"].ToString();
                        //    drUpdating["TaxType"] = drPriceList["Tax"].ToString();
                        //    drUpdating["TaxRate"] = (drPriceList["TaxRate"] != DBNull.Value
                        //        ? drPriceList["TaxRate"].ToString()
                        //        : "0");

                        //    // Modified on: 19/01/2018, By: Fon
                        //    //drUpdating["TaxAdj"] = false;
                        //    // End Modified.

                        //    var Percent = decimal.Parse(drUpdating["DiscPercent"].ToString());
                        //    var ApprQty = decimal.Parse(drUpdating["ApprQty"].ToString());

                        //    // Update Net, Tax and Amount
                        //    // Added on: 09/08/2017, By: Fon
                        //    // var Price = decimal.Parse(drUpdating["Price"].ToString());
                        //    var price = decimal.Parse(drUpdating["Price"].ToString());
                        //    var priceRate = decimal.Parse(drUpdating["Price"].ToString())
                        //           * currency.GetLastCurrencyRate(drPriceList["CurrencyCode"].ToString()
                        //           , date, LoginInfo.ConnStr);

                        //    decimal currencyRate = 1;
                        //    if (drPriceList["CurrencyCode"].ToString() == string.Empty)
                        //    {
                        //        drPriceList["CurrencyCode"] = baseCurrency;
                        //        drPriceList["CurrencyRate"] = currencyRate;
                        //    }
                        //    else
                        //        currencyRate = decimal.Parse(drPriceList["CurrencyRate"].ToString());

                        //    drUpdating["CurrencyCode"] = drPriceList["CurrencyCode"].ToString();
                        //    drUpdating["CurrencyRate"] = drPriceList["CurrencyRate"].ToString();
                        //    // End Added.

                        //    // Update DiscPercent and DiscAmt                
                        //    if (decimal.Parse(drUpdating["DiscAmt"].ToString()) == 0 &&
                        //        decimal.Parse(drUpdating["DiscPerCent"].ToString()) > 0)
                        //    {
                        //        drUpdating["DiscAmt"] = ((priceRate * Percent) / 100) * ApprQty;
                        //    }
                        //    else if (decimal.Parse(drUpdating["DiscAmt"].ToString()) > 0 &&
                        //             decimal.Parse(drUpdating["DiscPerCent"].ToString()) > 0)
                        //    {
                        //        drUpdating["DiscAmt"] = ((priceRate * Percent) / 100) * ApprQty;
                        //    }
                        //    else
                        //    {
                        //        if (priceRate == 0)
                        //            drUpdating["DiscPerCent"] = 0;
                        //        else
                        //            drUpdating["DiscPerCent"] = (decimal.Parse(drUpdating["DiscAmt"].ToString()) * 100) / (priceRate);
                        //        Percent = decimal.Parse(drUpdating["DiscPerCent"].ToString());
                        //    }

                        //    var DiscAmt = (priceRate * Percent) / 100;
                        //    // Net, Tax, Total
                        //    var calAmt = CalAmt(priceRate, DiscAmt,
                        //        decimal.Parse(drUpdating["ApprQty"].ToString()));

                        //    //drUpdating["NetAmt"] = NetAmt(drUpdating["TaxType"].ToString(),
                        //    //    decimal.Parse(drUpdating["TaxRate"].ToString()), priceRate, DiscAmt
                        //    //    , decimal.Parse(drUpdating["ApprQty"].ToString()));

                        //    //drUpdating["TaxAmt"] = TaxAmt(drUpdating["TaxType"].ToString(),
                        //    //    decimal.Parse(drUpdating["TaxRate"].ToString()), priceRate, DiscAmt
                        //    //    , decimal.Parse(drUpdating["ApprQty"].ToString()));

                        //    //drUpdating["TotalAmt"] = Amount(drUpdating["TaxType"].ToString(),
                        //    //    decimal.Parse(drUpdating["TaxRate"].ToString()), priceRate, DiscAmt
                        //    //    , decimal.Parse(drUpdating["ApprQty"].ToString()));

                        //    // Added on: 05/09/2017, By: Fon
                        //    decimal apprQty = 0, currDiscAmt = 0;
                        //    decimal.TryParse(drUpdating["ApprQty"].ToString(), out apprQty);

                        //    decimal currNetAmt = NetAmt(drUpdating["TaxType"].ToString(),
                        //        decimal.Parse(drUpdating["TaxRate"].ToString())
                        //        , price * apprQty, currDiscAmt, 1);
                        //    drUpdating["CurrNetAmt"] = currNetAmt;
                        //    drUpdating["NetAmt"] = currNetAmt * currencyRate;

                        //    currDiscAmt = ((price * Percent) / 100) * ApprQty;
                        //    drUpdating["CurrDiscAmt"] = currDiscAmt;
                        //    drUpdating["DiscAmt"] = currDiscAmt * currencyRate;


                        //    decimal currTaxAmt = TaxAmt(drUpdating["TaxType"].ToString(),
                        //        decimal.Parse(drUpdating["TaxRate"].ToString())
                        //        , price * apprQty, currDiscAmt, 1);
                        //    drUpdating["CurrTaxAmt"] = currTaxAmt;
                        //    drUpdating["TaxAmt"] = currTaxAmt * currencyRate;


                        //    decimal currTotalAmt = Amount(drUpdating["TaxType"].ToString(),
                        //        decimal.Parse(drUpdating["TaxRate"].ToString())
                        //        , price * apprQty, currDiscAmt, 1);
                        //    drUpdating["CurrTotalAmt"] = currTotalAmt;
                        //    drUpdating["TotalAmt"] = currTotalAmt * currencyRate;

                        //    // End Added.
                        //    Allocate++;
                    }
                    #endregion
                    else
                    {
                        NotAllocate++;
                    }
                }
            }


            string message = string.Empty;

            //Count Allocate.
            if (Allocate == 0 && NotAllocate > 0)
            {
                //lbl_AutoAllocate.Text = "No Allocate item.";
                message = Allocate + " item is allocated. " + NotAllocate + " item(s) are not allocated.";
            }
            else if (Allocate > 0 && NotAllocate == 0)
            {
                //lbl_AutoAllocate.Text = "Allocate Item is Succesfully";
                message = "All items are allocated.";
            }
            else if (Allocate > 0 && NotAllocate > 0)
            {
                //lbl_AutoAllocate.Text = Allocate + " item is Allocate. " + NotAllocate + " item is not Allocate.";
                message = Allocate + " item(s) are allocated. " + NotAllocate + " item(s) are not allocated.";
            }
            else if (Allocate == 0 && NotAllocate == 0)
            {
                //lbl_AutoAllocate.Text = Allocate + " item is Allocate. " + NotAllocate + " item is not Allocate.";
                message = "No allocated item.";
            }

            //pop_ConfirmAutoAlloVd.ShowOnPageLoad = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", string.Format("<script>alert('{0}');</script>", message), false);
            Page_Setting();
        }

        //protected void btn_CancelAutoAlloVd_Click(object sender, EventArgs e)
        //{
        //    pop_ConfirmAutoAlloVd.ShowOnPageLoad = false;
        //}

        //protected void btn_OK_PopAutoAlloVd_Click(object sender, EventArgs e)
        //{
        //    pop_ConfirmAutoAlloVd.ShowOnPageLoad = false;
        //    pop_AutoAlloVd.ShowOnPageLoad = false;
        //}

        // Change All Delivery Date on One Click.
        protected void Ok_DeliDate()
        {
            pop_ConfirmChangeDeliDate.ShowOnPageLoad = true;
        }

        protected void btn_CancelChangeDeliDate_Click(object sender, EventArgs e)
        {
            pop_ConfirmChangeDeliDate.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmChangeDeliDate_Click(object sender, EventArgs e)
        {
            for (var i = 0; i <= grd_PrDt1.Rows.Count - 1; i++)
            {
                var drUpdating = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.Rows[i].DataItemIndex];

                drUpdating["ReqDate"] = DateTime.Parse(dte_DeliDate.Date.ToString("dd/MM/yyyy"));
            }


            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.DataBind();
            //Page_Setting();

            pop_ConfirmChangeDeliDate.ShowOnPageLoad = false;
        }

        // Add New vendor from Assign Button.
        protected void btn_OkAddVendor_Click(object sender, EventArgs e)
        {
            dsPriceCompare = (DataSet)Session["dsPriceCompare"];

            pop_NewVendor.ShowOnPageLoad = true;

            grd_Vendor.DataSource = dsPriceCompare.Tables[priceList.TableName];
            grd_Vendor.DataBind();
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
        }

        protected void btn_CancelAddVendor_Click(object sender, EventArgs e)
        {
            pop_AddVendor.ShowOnPageLoad = false;

        }

        // Save New Vendor.
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            pop_ConfirmSaveAddVendor.ShowOnPageLoad = true;
            pop_NewVendor.ShowOnPageLoad = false;

        }

        protected void btn_ConfirmSaveAddVendor_Click(object sender, EventArgs e)
        {
            dsPriceCompare = (DataSet)Session["dsPriceCompare"];
            dsVendor = (DataSet)Session["dsVendor"];

            for (var i = 0; i <= grd_Vendor.Rows.Count - 1; i++)
            {
                var rdo_Vendor = grd_Vendor.Rows[i].Cells[0].FindControl("rdo_Vendor") as RadioButton;
                if (rdo_Vendor.Checked)
                {
                    var drSaveVendor = dsPriceCompare.Tables[priceList.TableName].Rows[i];

                    var BuCode = drSaveVendor["BuCode"].ToString().Split(':');

                    // Vendor
                    vendor.GetVendorStructure(dsVendor, LoginInfo.ConnStr);

                    // Profile
                    profile.GetProfileStructure(dsVendor, LoginInfo.ConnStr);

                    // Get vendorMisc.
                    vendorMisc.GetSchema(dsVendor, LoginInfo.ConnStr);

                    // Get bankaccount information
                    bankAccount.GetBankAccountStructure(dsVendor, LoginInfo.ConnStr);

                    // Get contact information.
                    contact.GetContactStructure(dsVendor, LoginInfo.ConnStr);

                    // Get contact detail structure for before click image button.
                    contactDetail.GetContactDetailStructure(dsVendor, LoginInfo.ConnStr);

                    // Get addresslist.
                    address.GetAddressStructure(dsVendor, LoginInfo.ConnStr);

                    // Get invoicelist.
                    invoiceDefault.GetInvoiceDefaultStructure(dsVendor, LoginInfo.ConnStr);

                    // Get invoicedetail structure.
                    invoiceDefaultDetail.GetInvoiceDefaultDetailStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentlist.
                    paymentDefault.GetPaymentDefaultStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentdefaultcash structure.
                    paymentDefaultCash.GetPaymentDefaultCashStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentdefaultcheq structure.
                    paymentDefaultCheq.GetPaymentDefaultCheqStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentdefaultcredit structure.
                    paymentDefaultCredit.GetPaymentDefaultCreditStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentdefaultauto structure.
                    paymentDefaultAuto.GetPaymentDefaultAutoStructure(dsVendor, LoginInfo.ConnStr);

                    // Get paymentdefaulttrans structure.
                    paymentDefaultTrans.GetPaymentDefaultTransStructure(dsVendor, LoginInfo.ConnStr);

                    // Get vendordefaultwht structure.
                    vendorDefaultWHT.GetVendorDefaultWHTStructure(dsVendor, LoginInfo.ConnStr);

                    // Get vendorComment.
                    vendorComment.GetSchema(dsVendor, LoginInfo.ConnStr);

                    // Get vendorActiveLog.
                    vendorActiveLog.GetSchema(dsVendor, LoginInfo.ConnStr);

                    // Get vendor attachment.
                    vendorAttachment.GetStructure(dsVendor, LoginInfo.ConnStr);

                    // Vendor Exist.
                    var dsVendorExist = new DataSet();

                    var resultVendorExist = vendor.GetListBu(dsVendorExist, Session["AssignBuCode"].ToString(),
                        drSaveVendor["VendorName"].ToString());

                    if (!resultVendorExist)
                    {
                        return;
                    }

                    if (dsVendorExist.Tables[vendor.TableName].Rows.Count > 0)
                    {
                        continue;
                    }

                    // Vendor.
                    var result = vendor.GetVendor(drSaveVendor["VendorCode"].ToString(), dsVendor,
                        bu.GetConnectionString(BuCode[0].Trim()));

                    if (!result)
                    {
                        return;
                    }

                    //Add New Vendor.
                    var drVendor = dsVendor.Tables[vendor.TableName].Rows[0];

                    var drNewVendor = dsVendor.Tables[vendor.TableName].NewRow();

                    drNewVendor["VendorCode"] =
                        vendor.GetNewVendorCode(bu.GetConnectionString(Session["AssignBuCode"].ToString()));
                    drNewVendor["SunVendorCode"] = drVendor["SunVendorCode"];
                    drNewVendor["ProfileCode"] = drVendor["ProfileCode"];
                    drNewVendor["Name"] = drVendor["Name"];
                    drNewVendor["VendorCategoryCode"] = drVendor["VendorCategoryCode"];
                    drNewVendor["TaxID"] = drVendor["TaxID"];
                    drNewVendor["RegisterNo"] = drVendor["RegisterNo"];
                    drNewVendor["CreditTerm"] = drVendor["CreditTerm"];
                    drNewVendor["DiscountTerm"] = drVendor["DiscountTerm"];
                    drNewVendor["DiscountRate"] = drVendor["DiscountRate"];
                    drNewVendor["Description"] = drVendor["Description"];
                    drNewVendor["Rating"] = drVendor["Rating"];
                    drNewVendor["TaxType"] = drVendor["TaxType"];
                    drNewVendor["TaxRate"] = drVendor["TaxRate"];
                    drNewVendor["IsActive"] = drVendor["IsActive"];
                    drNewVendor["CreatedBy"] = LoginInfo.LoginName;
                    drNewVendor["CreatedDate"] = ServerDateTime;
                    drNewVendor["UpdatedBy"] = LoginInfo.LoginName;
                    drNewVendor["UpdatedDate"] = ServerDateTime;

                    // New Row for vendor table.                    
                    dsVendor.Tables[vendor.TableName].Rows.Add(drNewVendor);

                    //Save New Vendor
                    var save = vendor.Save(dsVendor, bu.GetConnectionString(Session["AssignBuCode"].ToString().Trim()));

                    if (save)
                    {
                        var lbl_Vendor_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_Vendor_av") as Label;
                        var lbl_FOC_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_FOC_av") as Label;
                        var lbl_ReqQty_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_ReqQty_av") as Label;
                        var lbl_NetAmt_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_NetAmt_av") as Label;
                        var lbl_TaxAmt_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_TaxAmt_av") as Label;
                        var lbl_TotalAmt_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_TotalAmt_av") as Label;
                        var lbl_Price_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_Price_av") as Label;
                        var lbl_TaxType_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_TaxType_av") as Label;
                        var lbl_TaxRate_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_TaxRate_av") as Label;
                        var lbl_DiscPercent_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_DiscPercent_av") as Label;
                        var lbl_DiscAmt_av = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("lbl_DiscAmt_av") as Label;
                        var grd_PriceCompare = grd_PrDt1.Rows[int.Parse(Session["ItemIndex"].ToString())].FindControl("grd_PriceCompare") as GridView;

                        //DataRow drAssign = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)lbl_Vendor_av.NamingContainer).DataItemIndex];
                        var drAssign = dsPR.Tables[prDt.TableName].Rows[int.Parse(Session["ItemIndex"].ToString())];

                        lbl_Vendor_av.Text = drNewVendor["VendorCode"] + " : " + drNewVendor["Name"];
                        lbl_FOC_av.Text = drSaveVendor["FOC"].ToString();
                        lbl_Price_av.Text = drSaveVendor["Price"].ToString();
                        lbl_TaxType_av.Text = drSaveVendor["Tax"].ToString();
                        lbl_TaxRate_av.Text = drSaveVendor["TaxRate"].ToString();
                        lbl_DiscPercent_av.Text = drSaveVendor["DiscountPercent"].ToString();
                        lbl_DiscAmt_av.Text = drSaveVendor["DiscountAmt"].ToString();

                        drAssign["VendorCode"] = drNewVendor["VendorCode"].ToString();
                        drAssign["FOCQty"] = drSaveVendor["FOC"].ToString();
                        drAssign["Price"] = drSaveVendor["Price"].ToString();
                        drAssign["TaxType"] = drSaveVendor["Tax"].ToString();
                        drAssign["TaxRate"] = drSaveVendor["TaxRate"].ToString();
                        drAssign["DiscPercent"] = drSaveVendor["DiscountPercent"].ToString() == string.Empty
                            ? "0.00"
                            : lbl_DiscPercent_av.Text;
                        drAssign["DiscAmt"] = drSaveVendor["DiscountAmt"].ToString();
                        drAssign["ReqQty"] = lbl_ReqQty_av.Text;
                        drAssign["ApprQty"] = lbl_ReqQty_av.Text;

                        // Net, Tax, Total
                        var Price = decimal.Parse(drAssign["Price"].ToString());
                        var DiscAmt = decimal.Parse(drAssign["DiscAmt"].ToString());
                        var calAmt = CalAmt(Price, DiscAmt, decimal.Parse(drAssign["ApprQty"].ToString()));

                        drAssign["TotalAmt"] = Amount(drAssign["TaxType"].ToString(),
                            decimal.Parse(drAssign["TaxRate"].ToString())
                            , Price, DiscAmt, decimal.Parse(drAssign["ApprQty"].ToString()));

                        drAssign["NetAmt"] = NetAmt(drAssign["TaxType"].ToString(),
                            decimal.Parse(drAssign["TaxRate"].ToString())
                            , Price, DiscAmt, decimal.Parse(drAssign["ApprQty"].ToString()));

                        drAssign["TaxAmt"] = TaxAmt(drAssign["TaxType"].ToString(),
                            decimal.Parse(drAssign["TaxRate"].ToString())
                            , Price, DiscAmt, decimal.Parse(drAssign["ApprQty"].ToString()));

                        lbl_NetAmt_av.Text = drAssign["NetAmt"].ToString();
                        lbl_TotalAmt_av.Text = drAssign["TotalAmt"].ToString();
                        lbl_TaxAmt_av.Text = drAssign["TaxAmt"].ToString();

                        dsVendor.Clear();
                    }
                }
            }

            pop_NewVendor.ShowOnPageLoad = false;
            pop_AddVendor.ShowOnPageLoad = false;
            pop_ConfirmSaveAddVendor.ShowOnPageLoad = false;
        }

        protected void btn_CancelSaveAddVendor_Click(object sender, EventArgs e)
        {
            pop_ConfirmSaveAddVendor.ShowOnPageLoad = false;
            pop_NewVendor.ShowOnPageLoad = false;
            pop_AddVendor.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
        }

        // Gridview New Expand.
        protected void grd_PrDt1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ddl_PrType.Enabled = (sender as GridView).Rows.Count == 0;
            ddl_JobCode.Enabled = (sender as GridView).Rows.Count == 0;


            DataRow drWFDt = dsWF.Tables["APPwfdt"].Rows[0];
            string controlEnable = "EnableField";
            string controlHide = "HideField";


            //hong visible change delivery date 10092013
            if (grd_PrDt1.EditIndex == -1)
            {
                dte_DeliDate.Visible = true;
                menu_OKBar.Visible = true;
            }
            else
            {
                dte_DeliDate.Visible = false;
                menu_OKBar.Visible = false;
            }

            #region EmptyDataRow

            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                var p_Issue_Empty = e.Row.FindControl("p_Issue_Empty") as Panel;
                var p_AlloCate_Empty = e.Row.FindControl("p_AlloCate_Empty") as Panel;
                var chk_All = e.Row.FindControl("Chk_All") as CheckBox;

                //if (wfStep == 3) //if (wfStep == 3 || wfStep == 5)
                if (Convert.ToBoolean(drWFDt["IsAllocateVendor"]))
                {
                    p_Issue_Empty.Visible = false;
                    p_AlloCate_Empty.Visible = true;
                    chk_All.Checked = true;

                    if (e.Row.FindControl("lbl_BU_HD_av") != null)
                    {
                        var lbl_BU_HD_av = e.Row.FindControl("lbl_BU_HD_av") as Label;
                        lbl_BU_HD_av.Visible = LoginInfo.BuInfo.IsHQ;
                    }
                }
                else
                {
                    p_Issue_Empty.Visible = true;
                    p_AlloCate_Empty.Visible = false;

                    if (e.Row.FindControl("lbl_BuCode_HD") != null)
                    {
                        var lbl_BuCode_HD = e.Row.FindControl("lbl_BuCode_HD") as Label;
                        lbl_BuCode_HD.Visible = LoginInfo.BuInfo.IsHQ;
                    }
                }

                #region For allocate vendor Header
                if (e.Row.FindControl("lbl_Vendor_HDG_av") != null)
                {
                    var lbl_Vendor_HDG_av = (Label)e.Row.FindControl("lbl_Vendor_HDG_av");
                    lbl_Vendor_HDG_av.Visible = !IsExistInField("PC.PrDt.VendorCode", controlHide);
                }

                if (e.Row.FindControl("lbl_Store_HDG_av") != null)
                {
                    var lbl_Store_HDG_av = (Label)e.Row.FindControl("lbl_Store_HDG_av");
                    lbl_Store_HDG_av.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                }

                if (e.Row.FindControl("lbl_Sku_HDG_av") != null)
                {
                    var lbl_Sku_HDG_av = (Label)e.Row.FindControl("lbl_Sku_HDG_av");
                    lbl_Sku_HDG_av.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                }

                if (e.Row.FindControl("lbl_DescEn_HDG_av") != null)
                {
                    var lbl_DescEn_HDG_av = (Label)e.Row.FindControl("lbl_DescEn_HDG_av");
                    lbl_DescEn_HDG_av.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                }

                if (e.Row.FindControl("lbl_Descll_HDG_av") != null)
                {
                    var lbl_Descll_HDG_av = (Label)e.Row.FindControl("lbl_Descll_HDG_av");
                    lbl_Descll_HDG_av.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                }

                if (e.Row.FindControl("lbl_DeliDate_HDG_av") != null)
                {
                    var lbl_DeliDate_HDG_av = (Label)e.Row.FindControl("lbl_DeliDate_HDG_av");
                    lbl_DeliDate_HDG_av.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                }

                if (e.Row.FindControl("lbl_DeliPoint_HDG_av") != null)
                {
                    var lbl_DeliPotin_HDG_av = (Label)e.Row.FindControl("lbl_DeliPoint_HDG_av");
                    lbl_DeliPotin_HDG_av.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                }

                if (e.Row.FindControl("lbl_ReqQty_HDG_av") != null)
                {
                    var lbl_ReqQty_HDG_av = (Label)e.Row.FindControl("lbl_ReqQty_HDG_av");
                    lbl_ReqQty_HDG_av.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                }

                // Added on: 10/08/2017, By: Fon
                if (e.Row.FindControl("lbl_ApprQty_HDG_av") != null)
                {
                    var lbl_ApprQty_HDG_av = (Label)e.Row.FindControl("lbl_ApprQty_HDG_av");
                    lbl_ApprQty_HDG_av.Visible = !IsExistInField("PC.PrDt.AppeQty", controlHide);
                }
                // End Added.
                if (e.Row.FindControl("lbl_FOC_HDG_av") != null)
                {
                    var lbl_FOC_HDG_av = (Label)e.Row.FindControl("lbl_FOC_HDG_av");
                    lbl_FOC_HDG_av.Visible = !IsExistInField("PC.PrDt.FOCQty", controlHide);
                }

                if (e.Row.FindControl("lbl_Unit_HDG_av") != null)
                {
                    var lbl_Unit_HDG_av = (Label)e.Row.FindControl("lbl_Unit_HDG_av");
                    lbl_Unit_HDG_av.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                }

                if (e.Row.FindControl("lbl_Price_HDG_av") != null)
                {
                    var lbl_Price_HDG_av = (Label)e.Row.FindControl("lbl_Price_HDG_av");
                    lbl_Price_HDG_av.Visible = !IsExistInField("PC.PrDt.Price", controlHide);
                }
                #endregion
            }

            #endregion EmptyDataRow



            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                // Business Unit
                e.Row.Cells[2].Visible = LoginInfo.BuInfo.IsHQ;
            }


            #region Header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var p_Issue = e.Row.FindControl("p_Issue") as Panel;
                var p_AllocateVendor = e.Row.FindControl("p_AllocateVendor") as Panel;
                var isAllocateVendor = Convert.ToBoolean(drWFDt["IsAllocateVendor"]);
                var chk_All = e.Row.FindControl("Chk_All") as CheckBox;

                p_Issue.Visible = false;
                p_AllocateVendor.Visible = false;

                if (isAllocateVendor)
                {
                    #region <Header> Allocate Vendor

                    p_AllocateVendor.Visible = true;

                    chk_All.Checked = true;

                    if (e.Row.FindControl("lbl_BU_HD_av") != null)
                    {
                        var lbl_BU_HD_av = e.Row.FindControl("lbl_BU_HD_av") as Label;
                        //lbl_BU_HD_av.Visible = LoginInfo.BuInfo.IsHQ;
                    }

                    if (e.Row.FindControl("lbl_Vendor_HDG_av") != null)
                    {
                        var lbl_Vendor_HDG_av = (Label)e.Row.FindControl("lbl_Vendor_HDG_av");
                        lbl_Vendor_HDG_av.Visible = !IsExistInField("PC.PrDt.VendorCode", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Store_HDG_av") != null)
                    {
                        var lbl_Store_HDG_av = (Label)e.Row.FindControl("lbl_Store_HDG_av");
                        lbl_Store_HDG_av.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Sku_HDG_av") != null)
                    {
                        var lbl_Sku_HDG_av = (Label)e.Row.FindControl("lbl_Sku_HDG_av");
                        lbl_Sku_HDG_av.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DescEn_HDG_av") != null)
                    {
                        var lbl_DescEn_HDG_av = (Label)e.Row.FindControl("lbl_DescEn_HDG_av");
                        lbl_DescEn_HDG_av.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Descll_HDG_av") != null)
                    {
                        var lbl_Descll_HDG_av = (Label)e.Row.FindControl("lbl_Descll_HDG_av");
                        lbl_Descll_HDG_av.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DeliDate_HDG_av") != null)
                    {
                        var lbl_DeliDate_HDG_av = (Label)e.Row.FindControl("lbl_DeliDate_HDG_av");
                        lbl_DeliDate_HDG_av.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DeliPoint_HDG_av") != null)
                    {
                        var lbl_DeliPotin_HDG_av = (Label)e.Row.FindControl("lbl_DeliPoint_HDG_av");
                        lbl_DeliPotin_HDG_av.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                    }

                    if (e.Row.FindControl("lbl_ReqQty_HDG_av") != null)
                    {
                        var lbl_ReqQty_HDG_av = (Label)e.Row.FindControl("lbl_ReqQty_HDG_av");
                        lbl_ReqQty_HDG_av.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                    }

                    if (e.Row.FindControl("lbl_FOC_HDG_av") != null)
                    {
                        var lbl_FOC_HDG_av = (Label)e.Row.FindControl("lbl_FOC_HDG_av");
                        lbl_FOC_HDG_av.Visible = !IsExistInField("PC.PrDt.FOCQty", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Unit_HDG_av") != null)
                    {
                        var lbl_Unit_HDG_av = (Label)e.Row.FindControl("lbl_Unit_HDG_av");
                        lbl_Unit_HDG_av.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Price_HDG_av") != null)
                    {
                        var lbl_Price_HDG_av = (Label)e.Row.FindControl("lbl_Price_HDG_av");
                        lbl_Price_HDG_av.Visible = !IsExistInField("PC.PrDt.Price", controlHide);
                    }

                    // Added on: 28/08/2017, By: Fon
                    if (e.Row.FindControl("lbl_ApprQty_HDG_av") != null)
                    {
                        var lbl_ApprQty_HDG_av = (Label)e.Row.FindControl("lbl_ApprQty_HDG_av");
                        lbl_ApprQty_HDG_av.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                    }

                    if (e.Row.FindControl("lbl_CurrCode_HDG_av") != null)
                    {
                        var lbl_CurrCode_HDG = (Label)e.Row.FindControl("lbl_CurrCode_HDG_av");
                        lbl_CurrCode_HDG.Visible = !IsExistInField("PC.PrDt.Currency", controlHide);
                    }

                    if (e.Row.FindControl("lbl_CurrRate_HDG_av") != null)
                    {
                        Label lbl_CurrRate_HDG_av = (Label)e.Row.FindControl("lbl_CurrRate_HDG_av");
                        lbl_CurrRate_HDG_av.Visible = !IsExistInField("PC.PrDt.Currency", controlHide);
                    }
                    // End Added.
                    #endregion

                }
                else
                {
                    #region <Header> Issue

                    p_Issue.Visible = true;

                    if (e.Row.FindControl("lbl_BuCode_HD") != null)
                    {
                        var lbl_BuCode_HD = e.Row.FindControl("lbl_BuCode_HD") as Label;
                        //lbl_BuCode_HD.Visible = LoginInfo.BuInfo.IsHQ;
                    }


                    //if (e.Row.FindControl("lbl_BuCode_HD") != null)
                    //{
                    //    var lbl_BuCode_HD = (Label)e.Row.FindControl("lbl_BuCode_HD");
                    //    lbl_BuCode_HD.Visible = !IsExistInField("PC.PrDt.BuCode", controlHide);
                    //    lbl_BuCode_HD.Visible = LoginInfo.BuInfo.IsHQ;
                    //}

                    if (e.Row.FindControl("lbl_Store_HDG_Nm") != null)
                    {
                        var lbl_Store_HDG_Nm = (Label)e.Row.FindControl("lbl_Store_HDG_Nm");
                        lbl_Store_HDG_Nm.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                    }

                    if (e.Row.FindControl("lbl_SKU_HDG_Nm") != null)
                    {
                        var lbl_SKU_HDG_Nm = (Label)e.Row.FindControl("lbl_SKU_HDG_Nm");
                        lbl_SKU_HDG_Nm.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DescEn_HDG_Nm") != null)
                    {
                        var lbl_DescEn_HDG_Nm = (Label)e.Row.FindControl("lbl_DescEn_HDG_Nm");
                        lbl_DescEn_HDG_Nm.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                    }

                    if (e.Row.FindControl("lbl_Descll_HDG_Nm") != null)
                    {
                        var lbl_Descll_HDG_Nm = (Label)e.Row.FindControl("lbl_Descll_HDG_Nm");
                        lbl_Descll_HDG_Nm.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                    }

                    if (e.Row.FindControl("lbl_ReqQty_HDG_Nm") != null)
                    {
                        var lbl_ReqQty_HDG_Nm = (Label)e.Row.FindControl("lbl_ReqQty_HDG_Nm");
                        lbl_ReqQty_HDG_Nm.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                    }

                    // Added on: 10/08/2017, By: Fon
                    if (e.Row.FindControl("lbl_ApprQty_HDG_Nm") != null)
                    {
                        var lbl_ApprQty_HDG_Nm = (Label)e.Row.FindControl("lbl_ApprQty_HDG_Nm");
                        lbl_ApprQty_HDG_Nm.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                    }
                    // End Added.

                    if (e.Row.FindControl("lbl_Unit_HDG_Nm") != null)
                    {
                        var lbl_Unit_HDG_Nm = (Label)e.Row.FindControl("lbl_Unit_HDG_Nm");
                        lbl_Unit_HDG_Nm.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DeliDate_HDG_Nm") != null)
                    {
                        var lbl_DeliDate_HDG_Nm = (Label)e.Row.FindControl("lbl_DeliDate_HDG_Nm");
                        lbl_DeliDate_HDG_Nm.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                    }

                    if (e.Row.FindControl("lbl_DeliPoint_HDG_Nm") != null)
                    {
                        var lbl_DeliPoint_HDG_Nm = (Label)e.Row.FindControl("lbl_DeliPoint_HDG_Nm");
                        lbl_DeliPoint_HDG_Nm.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                    }

                    if (e.Row.FindControl("lbl_CurrCode_HDG_Nm") != null)
                    {
                        var lbl_CurrCode_HDG_Nm = (Label)e.Row.FindControl("lbl_CurrCode_HDG_Nm");
                        lbl_CurrCode_HDG_Nm.Visible = !IsExistInField("PC.PrDt.Currency", controlHide);
                    }

                    if (e.Row.FindControl("lbl_CurrRate_HDG_Nm") != null)
                    {
                        Label lbl_CurrRate_HDG_Nm = (Label)e.Row.FindControl("lbl_CurrRate_HDG_Nm");
                        lbl_CurrRate_HDG_Nm.Visible = !IsExistInField("PC.PrDt.Currency", controlHide);
                    }
                    #endregion
                }

                /*** Added on 2017/03/15: For control visibility from WFDt's condition. ***/


            }

            #endregion Header

            #region DataRow

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var buCode = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();
                var buConnStr = bu.GetConnectionString(buCode);
                var p_Issue = e.Row.FindControl("p_Issue") as Panel;
                var p_Issue_Expand = e.Row.FindControl("p_Issue_Expand") as Panel;
                var p_AllocateVendor = e.Row.FindControl("p_AllocateVendor") as Panel;
                var p_AllocateVendor_Expand = e.Row.FindControl("p_AllocateVendor_Expand") as Panel;

                var chk_Item = e.Row.FindControl("Chk_Item") as CheckBox;

                var lnkb_Delete = e.Row.FindControl("lnkb_Delete") as LinkButton;
                var lnkb_Edit = e.Row.FindControl("lnkb_Edit") as LinkButton;
                var lnkb_SaveNew = e.Row.FindControl("lnkb_SaveNew") as LinkButton;
                var lnkb_Update = e.Row.FindControl("lnkb_Update") as LinkButton;

                var status = DataBinder.Eval(e.Row.DataItem, "ApprStatus");
                bool isCurrentStepNoApproved = status.ToString().Substring((wfStep - 1) * 10, 10).Contains('_');
                bool isPreviousStepNoApproved = status.ToString().Substring(0, (wfStep - 1) * 10).Contains('_');
                bool isAllocateVendor = Convert.ToBoolean(drWFDt["IsAllocateVendor"]);


                p_Issue.Visible = false;
                p_Issue_Expand.Visible = false;
                p_AllocateVendor.Visible = false;
                p_AllocateVendor_Expand.Visible = false;
                chk_Item.Checked = false;

                // Display Button.

                if (isAllocateVendor)
                {

                    p_AllocateVendor.Visible = true;
                    p_AllocateVendor_Expand.Visible = true;
                    chk_Item.Checked = true;
                }
                else
                {

                    p_Issue.Visible = true;
                    p_Issue_Expand.Visible = true;
                    var Chk_Item = e.Row.FindControl("Chk_Item") as CheckBox;
                    Chk_Item.Visible = false;

                }

                #region <DataRow> Allocate Vendor

                #region Original ( without Currrency)
                // Bu
                if (e.Row.FindControl("lbl_BuCode_av") != null)
                {
                    var lbl_BuCode_av = e.Row.FindControl("lbl_BuCode_av") as Label;
                    lbl_BuCode_av.Text = buCode + " : " + bu.GetName(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString(), buConnStr);

                    lbl_BuCode_av.Visible = !IsExistInField("PC.PrDt.BuCode", controlHide);
                    lbl_BuCode_av.Visible = LoginInfo.BuInfo.IsHQ;
                    lbl_BuCode_av.ToolTip = lbl_BuCode_av.Text;
                }
                if (e.Row.FindControl("ddl_BuCode_av") != null)
                {
                    var ddl_BuCode_av = e.Row.FindControl("ddl_BuCode_av") as ASPxComboBox;
                    ddl_BuCode_av.Value = buCode;
                    ddl_BuCode_av.Visible = !IsExistInField("PC.PrDt.BuCode", controlHide);
                    ddl_BuCode_av.Visible = LoginInfo.BuInfo.IsHQ;

                    // Visible CheckBox & ImageButton when Create or Edit.
                    var Chk_Item = e.Row.FindControl("Chk_Item") as CheckBox;
                    Chk_Item.Visible = false;
                }

                // Vendor
                var vendorCode = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString().Trim();

                if (e.Row.FindControl("lbl_Vendor_av") != null)
                {
                    var lbl_Vendor_av = e.Row.FindControl("lbl_Vendor_av") as Label;
                    var vendorName = string.IsNullOrEmpty(vendorCode) ? string.Empty : vendor.GetName(vendorCode, buConnStr);

                    lbl_Vendor_av.Visible = true;
                    lbl_Vendor_av.Text = string.Format("{0} : {1}", vendorCode, vendorName);
                    lbl_Vendor_av.ToolTip = lbl_Vendor_av.Text;
                }


                if (e.Row.FindControl("ddl_Vendor_av") != null)
                {
                    var ddl_Vendor_av = e.Row.FindControl("ddl_Vendor_av") as ASPxComboBox;

                    ddl_Vendor_av.Value = vendorCode;
                    ddl_Vendor_av.ToolTip = ddl_Vendor_av.Text;
                }


                //if (e.Row.FindControl("ddl_Vendor_av") != null)
                //{
                //    var ddl_ProductCode_av = e.Row.FindControl("ddl_ProductCode_av") as ASPxComboBox;
                //    var ddl_Vendor_av = e.Row.FindControl("ddl_Vendor_av") as ASPxComboBox;

                //    ddl_ProductCode_av.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                //    ddl_Vendor_av.Value = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString();
                //    ddl_Vendor_av.ToolTip = ddl_Vendor_av.Text;
                //}

                // Location
                if (e.Row.FindControl("lbl_LocationCode_av") != null)
                {
                    var lbl_LocationCode_av = e.Row.FindControl("lbl_LocationCode_av") as Label;
                    lbl_LocationCode_av.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);

                    lbl_LocationCode_av.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode") + " : " +
                                               storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), buConnStr);
                    lbl_LocationCode_av.ToolTip = lbl_LocationCode_av.Text;
                }

                var productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString().Trim();


                if (e.Row.FindControl("ddl_LocationCode_av") != null)
                {
                    var ddl_LocationCode_av = e.Row.FindControl("ddl_LocationCode_av") as ASPxComboBox;

                    ddl_LocationCode_av.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                    ddl_LocationCode_av.Enabled = IsExistInField("PC.PrDt.LocationCode", controlEnable);
                    ddl_LocationCode_av.Value = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString().Trim();

                    if (DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() != string.Empty)
                    {
                        var ddl_ProductCode_av = e.Row.FindControl("ddl_ProductCode_av") as ASPxComboBox;

                        ddl_ProductCode_av.DataSource = product.GetLookUp_ByLocationCodeCateType(ddl_PrType.Value.ToString(), DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), buConnStr);
                        ddl_ProductCode_av.DataBind();

                        ddl_ProductCode_av.Value = productCode;
                        ddl_ProductCode_av.ToolTip = ddl_ProductCode_av.Text;

                        ddl_ProductCode_av.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                        ddl_ProductCode_av.Enabled = IsExistInField("PC.PrDt.ProductCode", controlEnable);
                    }

                }

                //if (e.Row.FindControl("ddl_ProductCode_av") != null)
                //{
                //    var ddl_ProductCode_av = e.Row.FindControl("ddl_ProductCode_av") as ASPxComboBox;

                //    if (Request.Params["ID"] != null)
                //    {
                //        ddl_ProductCode_av.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                //        ddl_ProductCode_av.Enabled = IsExistInField("PC.PrDt.ProductCode", controlEnable);

                //        ddl_ProductCode_av.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                //    }
                //}


                // Product
                if (e.Row.FindControl("lbl_SKU_av") != null)
                {
                    var lbl_SKU_av = e.Row.FindControl("lbl_SKU_av") as Label;

                    //if (Request.Params["ID"] != null)
                    //{
                    lbl_SKU_av.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                    lbl_SKU_av.Enabled = IsExistInField("PC.PrDt.ProductCode", controlEnable);

                    lbl_SKU_av.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_SKU_av.ToolTip = lbl_SKU_av.Text;
                    //}
                }



                // Description
                if (e.Row.FindControl("lbl_DescEN_av") != null)
                {
                    var lbl_DescEN_av = e.Row.FindControl("lbl_DescEN_av") as Label;
                    lbl_DescEN_av.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                    lbl_DescEN_av.Enabled = IsExistInField("PC.PrDt.Descen", controlEnable);

                    //lbl_DescEN_av.Text = DataBinder.Eval(e.Row.DataItem, "Descen").ToString();
                    lbl_DescEN_av.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), buConnStr);
                    lbl_DescEN_av.ToolTip = lbl_DescEN_av.Text;
                }
                if (e.Row.FindControl("txt_DescEN_av") != null)
                {
                    //var txt_DescEN_av = e.Row.FindControl("txt_DescEN_av") as TextBox;
                    var txt_DescEN_av = e.Row.FindControl("txt_DescEN_av") as Label;

                    txt_DescEN_av.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                    //txt_DescEN_av.ReadOnly = !IsExistInField("PC.PrDt.Descen", controlEnable);
                    //txt_DescEN_av.Text = DataBinder.Eval(e.Row.DataItem, "Descen").ToString();
                    txt_DescEN_av.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), buConnStr);
                    txt_DescEN_av.ToolTip = txt_DescEN_av.Text;
                }

                if (e.Row.FindControl("lbl_DescLL_av") != null)
                {
                    var lbl_DescLL_av = e.Row.FindControl("lbl_DescLL_av") as Label;
                    //lbl_DescLL_av.Text = DataBinder.Eval(e.Row.DataItem, "Descll").ToString();
                    lbl_DescLL_av.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), buConnStr);
                    lbl_DescLL_av.ToolTip = lbl_DescLL_av.Text;
                }
                if (e.Row.FindControl("txt_DescLL_av") != null)
                {
                    //var txt_DescLL_av = e.Row.FindControl("txt_DescLL_av") as TextBox;
                    var txt_DescLL_av = e.Row.FindControl("txt_DescLL_av") as Label;

                    txt_DescLL_av.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                    //txt_DescLL_av.Text = DataBinder.Eval(e.Row.DataItem, "Descll").ToString();
                    txt_DescLL_av.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), buConnStr);
                    txt_DescLL_av.ToolTip = txt_DescLL_av.Text;
                }

                // Delivery Date
                if (e.Row.FindControl("lbl_DeliDate_av") != null)
                {
                    var lbl_DeliDate_av = e.Row.FindControl("lbl_DeliDate_av") as Label;
                    lbl_DeliDate_av.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                    lbl_DeliDate_av.Enabled = IsExistInField("PC.PrDt.ReqDate", controlEnable);

                    lbl_DeliDate_av.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString()).ToString("dd/MM/yyyy");
                    lbl_DeliDate_av.ToolTip = lbl_DeliDate_av.Text;
                }
                if (e.Row.FindControl("dte_DeliDate_av") != null)
                {
                    var dte_DeliDate_av = e.Row.FindControl("dte_DeliDate_av") as ASPxDateEdit;
                    dte_DeliDate_av.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                    dte_DeliDate_av.Enabled = IsExistInField("PC.PrDt.ReqDate", controlEnable);

                    dte_DeliDate_av.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString());
                }

                // Delivery Point
                if (e.Row.FindControl("lbl_DeliPoint_av") != null)
                {
                    var lbl_DeliPoint_av = e.Row.FindControl("lbl_DeliPoint_av") as Label;
                    lbl_DeliPoint_av.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                    lbl_DeliPoint_av.Enabled = IsExistInField("PC.PrDt.DeliPoint", controlEnable);

                    lbl_DeliPoint_av.Text = DataBinder.Eval(e.Row.DataItem, "DeliPoint") + " : " +
                                            deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(),
                                                bu.GetConnectionString(
                                                    DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                    lbl_DeliPoint_av.ToolTip = lbl_DeliPoint_av.Text;
                }
                if (e.Row.FindControl("ddl_DeliPoint_av") != null)
                {
                    var ddl_DeliPoint_av = e.Row.FindControl("ddl_DeliPoint_av") as ASPxComboBox;

                    if (DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString() != string.Empty)
                    {
                        ddl_DeliPoint_av.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                        ddl_DeliPoint_av.Enabled = IsExistInField("PC.PrDt.DeliPoint", controlEnable);

                        ddl_DeliPoint_av.Value = DataBinder.Eval(e.Row.DataItem, "DeliPoint") + " : " +
                                                 deliPoint.GetName(
                                                     DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(),
                                                     bu.GetConnectionString(
                                                         DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                    }
                }

                // Qty Request
                if (e.Row.FindControl("lbl_ReqQty_av") != null)
                {
                    var lbl_ReqQty_av = e.Row.FindControl("lbl_ReqQty_av") as Label;
                    lbl_ReqQty_av.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                    lbl_ReqQty_av.Text = String.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "ReqQty"));
                    lbl_ReqQty_av.ToolTip = lbl_ReqQty_av.Text;
                }
                if (e.Row.FindControl("txt_ReqQty_av") != null)
                {
                    var txt_ReqQty_av = e.Row.FindControl("txt_ReqQty_av") as ASPxSpinEdit;
                    txt_ReqQty_av.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);

                    // 1. Check enable in EnableField then 2. Check is allow create - mean requestor.
                    txt_ReqQty_av.Enabled = IsExistInField("PC.PrDt.ReqQty", controlEnable);
                    txt_ReqQty_av.Enabled = wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr);
                    txt_ReqQty_av.Text = DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString();
                }

                // Request Approve Qty
                if (e.Row.FindControl("lbl_ApprQty_av") != null)
                {
                    var lbl_ApprQty_av = (Label)e.Row.FindControl("lbl_ApprQty_av");
                    lbl_ApprQty_av.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                    lbl_ApprQty_av.Text = string.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    //Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                }

                if (e.Row.FindControl("txt_ApprQty_av") != null)
                {
                    var txt_ApprQty_av = (ASPxSpinEdit)e.Row.FindControl("txt_ApprQty_av");
                    txt_ApprQty_av.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                    txt_ApprQty_av.Enabled = IsExistInField("PC.PrDt.ApprQty", controlEnable);
                    txt_ApprQty_av.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                }
                // End Added.

                // FOC
                if (e.Row.FindControl("lbl_FOC_av") != null)
                {
                    var lbl_FOC_av = e.Row.FindControl("lbl_FOC_av") as Label;
                    lbl_FOC_av.Visible = !IsExistInField("PC.PrDt.FOCQty", controlHide);
                    lbl_FOC_av.Enabled = IsExistInField("PC.PrDt.FOCQty", controlEnable);

                    lbl_FOC_av.Text = String.Format("{0:N3}", DataBinder.Eval(e.Row.DataItem, "FOCQty"));
                    lbl_FOC_av.ToolTip = lbl_FOC_av.Text;
                }
                if (e.Row.FindControl("txt_FOC_av") != null)
                {
                    var txt_FOC_av = e.Row.FindControl("txt_FOC_av") as ASPxSpinEdit;
                    txt_FOC_av.Visible = !IsExistInField("PC.PrDt.FOCQty", controlHide);
                    txt_FOC_av.Enabled = IsExistInField("PC.PrDt.FOCQty", controlEnable);

                    txt_FOC_av.Text = DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString();
                }

                // Order Unit
                if (e.Row.FindControl("lbl_OrderUnit_av") != null)
                {
                    var lbl_OrderUnit_av = e.Row.FindControl("lbl_OrderUnit_av") as Label;
                    lbl_OrderUnit_av.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                    lbl_OrderUnit_av.Enabled = IsExistInField("PC.PrDt.OrderUnit", controlEnable);

                    lbl_OrderUnit_av.Text = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                    lbl_OrderUnit_av.ToolTip = lbl_OrderUnit_av.Text;
                }
                if (e.Row.FindControl("ddl_Unit_av") != null)
                {
                    var ddl_Unit_av = e.Row.FindControl("ddl_Unit_av") as ASPxComboBox;
                    ddl_Unit_av.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                    //if (wfDt.IsEnableEdit(wfId, wfStep, "PC.PrDt.OrderUnit", hf_ConnStr.Value) &&
                    if (IsExistInField("PC.PrDt.OrderUnit", controlEnable) &&
                        bool.Parse(conf.GetValue("PC", "PR", "UnitChanged", hf_ConnStr.Value)))
                    {
                        ddl_Unit_av.Enabled = true;
                    }
                    else
                    {
                        ddl_Unit_av.Enabled = false;
                    }

                    ddl_Unit_av.Value = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                }

                // Price
                if (e.Row.FindControl("lbl_Price_av") != null)
                {
                    var lbl_Price_av = e.Row.FindControl("lbl_Price_av") as Label;
                    lbl_Price_av.Visible = !IsExistInField("PC.PrDt.Price", controlHide);
                    lbl_Price_av.Enabled = IsExistInField("PC.PrDt.Price", controlEnable);

                    lbl_Price_av.Text = String.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                    Price += decimal.Parse(lbl_Price_av.Text);
                    lbl_Price_av.ToolTip = lbl_Price_av.Text;
                }
                if (e.Row.FindControl("txt_Price_av") != null)
                {
                    var txt_Price_av = e.Row.FindControl("txt_Price_av") as ASPxSpinEdit;
                    txt_Price_av.Visible = !IsExistInField("PC.PrDt.Price", controlHide);
                    txt_Price_av.Enabled = IsExistInField("PC.PrDt.Price", controlEnable);

                    txt_Price_av.Text = String.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price"));
                }

                #endregion

                #region Other Currency
                if (e.Row.FindControl("lbl_CurrCurrDt") != null)
                {
                    Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                    lbl_CurrCurrDt.Text = string.Format("( {0} )", DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }
                if (e.Row.FindControl("lbl_BaseCurrDt") != null)
                {
                    Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                    lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
                }

                // CurrNetAmt
                if (e.Row.FindControl("lbl_CurrNetAmt_Grd_Av") != null)
                {
                    Label lbl_CurrNetAmt_Grd_Av = (Label)e.Row.FindControl("lbl_CurrNetAmt_Grd_Av");
                    lbl_CurrNetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }
                if (e.Row.FindControl("txt_CurrNetAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrNetAmt_Grd_Av = (TextBox)e.Row.FindControl("txt_CurrNetAmt_Grd_Av");
                    txt_CurrNetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }

                // CurrDiscAmt
                if (e.Row.FindControl("lbl_CurrDiscAmt_Grd_Av") != null)
                {
                    Label lbl_CurrDiscAmt_Grd_Av = (Label)e.Row.FindControl("lbl_CurrDiscAmt_Grd_Av");
                    lbl_CurrDiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }
                if (e.Row.FindControl("txt_CurrDiscAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)e.Row.FindControl("txt_CurrDiscAmt_Grd_Av");
                    txt_CurrDiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }

                // CurrTaxAmt
                if (e.Row.FindControl("lbl_CurrTaxAmt_Grd_Av") != null)
                {
                    Label lbl_CurrTaxAmt_Grd_Av = (Label)e.Row.FindControl("lbl_CurrTaxAmt_Grd_Av");
                    lbl_CurrTaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }
                if (e.Row.FindControl("txt_CurrTaxAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)e.Row.FindControl("txt_CurrTaxAmt_Grd_Av");
                    txt_CurrTaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }

                // CurrTotalAmt
                if (e.Row.FindControl("lbl_CurrTotalAmt_Grd_Av") != null)
                {
                    Label lbl_CurrTotalAmt_Grd_Av = (Label)e.Row.FindControl("lbl_CurrTotalAmt_Grd_Av");
                    lbl_CurrTotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                if (e.Row.FindControl("txt_CurrTotalAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrTotalAmt_Grd_Av = (TextBox)e.Row.FindControl("txt_CurrTotalAmt_Grd_Av");
                    txt_CurrTotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                #endregion

                if (e.Row.FindControl("lbl_NetAmt_Grd_Av") != null)
                {
                    var lbl_NetAmt_Grd_Av = e.Row.FindControl("lbl_NetAmt_Grd_Av") as Label;
                    lbl_NetAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                    lbl_NetAmt_Grd_Av.ToolTip = lbl_NetAmt_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_NetAmt_Grd_Av") != null)
                {
                    var txt_NetAmt_Grd_Av = e.Row.FindControl("txt_NetAmt_Grd_Av") as TextBox;
                    txt_NetAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                }

                if (e.Row.FindControl("lbl_Disc_Grd_Av") != null)
                {
                    var lbl_Disc_Grd_Av = e.Row.FindControl("lbl_Disc_Grd_Av") as Label;
                    lbl_Disc_Grd_Av.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "DiscPercent")) +
                                           " % ";
                    lbl_Disc_Grd_Av.ToolTip = lbl_Disc_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_DiscPercent_Grd_Av") != null)
                {
                    var txt_DiscPercent_Grd_Av = e.Row.FindControl("txt_DiscPercent_Grd_Av") as TextBox;
                    txt_DiscPercent_Grd_Av.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "DiscPercent"));
                }

                if (e.Row.FindControl("lbl_DiscAmt_Grd_Av") != null)
                {
                    var lbl_DiscAmt_Grd_Av = e.Row.FindControl("lbl_DiscAmt_Grd_Av") as Label;
                    lbl_DiscAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                    lbl_DiscAmt_Grd_Av.ToolTip = lbl_DiscAmt_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_DiscAmt_Grd_Av") != null)
                {
                    var txt_DiscAmt_Grd_Av = e.Row.FindControl("txt_DiscAmt_Grd_Av") as TextBox;
                    txt_DiscAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                }

                if (e.Row.FindControl("lbl_TaxRate_Grd_Av") != null)
                {
                    var lbl_TaxRate_Grd_Av = e.Row.FindControl("lbl_TaxRate_Grd_Av") as Label;
                    lbl_TaxRate_Grd_Av.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "TaxRate")) + " % ";
                    lbl_TaxRate_Grd_Av.ToolTip = lbl_TaxRate_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_TaxRate_Grd_av") != null)
                {
                    var txt_TaxRate_Grd_av = e.Row.FindControl("txt_TaxRate_Grd_av") as TextBox;
                    txt_TaxRate_Grd_av.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "TaxRate"));
                }

                if (e.Row.FindControl("lbl_TaxType_Grd_Av") != null)
                {
                    var lbl_TaxType_Grd_Av = e.Row.FindControl("lbl_TaxType_Grd_Av") as Label;
                    lbl_TaxType_Grd_Av.Text = GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                    lbl_TaxType_Grd_Av.ToolTip = lbl_TaxType_Grd_Av.Text;
                    //lbl_TaxType_Grd_Av.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }

                if (e.Row.FindControl("lbl_TaxAmt_Grd_Av") != null)
                {
                    var lbl_TaxAmt_Grd_Av = e.Row.FindControl("lbl_TaxAmt_Grd_Av") as Label;
                    lbl_TaxAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    lbl_TaxAmt_Grd_Av.ToolTip = lbl_TaxAmt_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_TaxAmt_Grd_Av") != null)
                {
                    var txt_TaxAmt_Grd_Av = e.Row.FindControl("txt_TaxAmt_Grd_Av") as TextBox;
                    txt_TaxAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                }

                if (e.Row.FindControl("lbl_TotalAmt_Grd_Av") != null)
                {
                    var lbl_TotalAmt_Grd_Av = e.Row.FindControl("lbl_TotalAmt_Grd_Av") as Label;
                    lbl_TotalAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_TotalAmt_Grd_Av.ToolTip = lbl_TotalAmt_Grd_Av.Text;
                }
                if (e.Row.FindControl("txt_TotalAmt_Grd_Av") != null)
                {
                    var txt_TotalAmt_Grd_Av = e.Row.FindControl("txt_TotalAmt_Grd_Av") as TextBox;
                    txt_TotalAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                }

                if (e.Row.FindControl("chk_Adj_Av") != null)
                {
                    var chk_Adj_Av = e.Row.FindControl("chk_Adj_Av") as CheckBox;
                    chk_Adj_Av.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj"));
                    chk_Adj_Av.Enabled = false;
                }

                if (e.Row.FindControl("chk_Adj_Grd_Av") != null)
                {
                    // Modified on: 30/08/2017, By: Fon
                    var chk_Adj_Grd_Av = e.Row.FindControl("chk_Adj_Grd_Av") as ASPxCheckBox;
                    var txt_TaxAmt_Grd_Av = e.Row.FindControl("txt_TaxAmt_Grd_Av") as TextBox;
                    TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)e.Row.FindControl("txt_CurrTaxAmt_Grd_Av");
                    TextBox txt_TaxRate_Grd_Av = (TextBox)e.Row.FindControl("txt_TaxRate_Grd_Av");
                    ASPxComboBox ddl_TaxType_Grd_Av = (ASPxComboBox)e.Row.FindControl("ddl_TaxType_Grd_Av");

                    chk_Adj_Grd_Av.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj"));
                    txt_TaxRate_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
                    txt_TaxAmt_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
                    txt_CurrTaxAmt_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
                    ddl_TaxType_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;

                    // End Modified.
                }

                if (e.Row.FindControl("ddl_TaxType_Grd_Av") != null)
                {
                    var ddl_TaxType_Grd_Av = e.Row.FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
                    ddl_TaxType_Grd_Av.Value = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                }
                //**********************************************************************************************************

                #endregion

                #region <DataRow> Issue
                // Business Unit Code
                if (e.Row.FindControl("lbl_BuCode") != null)
                {
                    var lbl_BuCode = e.Row.FindControl("lbl_BuCode") as Label;
                    lbl_BuCode.Text = DataBinder.Eval(e.Row.DataItem, "BuCode") + " : " + bu.GetName(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString(), buConnStr);
                    lbl_BuCode.ToolTip = lbl_BuCode.Text;
                }
                if (e.Row.FindControl("ddl_BuCode") != null)
                {
                    var ddl_BuCode = e.Row.FindControl("ddl_BuCode") as ASPxComboBox;
                    ddl_BuCode.Value = DataBinder.Eval(e.Row.DataItem, "BuCode").ToString();

                }

                // Location
                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode") + " : " + storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), buConnStr);
                    lbl_LocationCode.ToolTip = lbl_LocationCode.Text;

                    lbl_LocationCode.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                }
                if (e.Row.FindControl("ddl_LocationCode") != null)
                {
                    var ddl_LocationCode = e.Row.FindControl("ddl_LocationCode") as ASPxComboBox;
                    ddl_LocationCode.Value = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString().Trim();
                    ddl_LocationCode.ToolTip = ddl_LocationCode.Text;

                    ddl_LocationCode.Visible = !IsExistInField("PC.PrDt.LocationCode", controlHide);
                    ddl_LocationCode.Enabled = IsExistInField("PC.PrDt.LocationCode", controlEnable);
                }

                // Product
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;

                    lbl_ProductCode.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                }
                if (e.Row.FindControl("ddl_ProductCode") != null)
                {
                    var ddl_ProductCode = e.Row.FindControl("ddl_ProductCode") as ASPxComboBox;
                    ddl_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    ddl_ProductCode.ToolTip = ddl_ProductCode.Text;

                    ddl_ProductCode.Visible = !IsExistInField("PC.PrDt.ProductCode", controlHide);
                    ddl_ProductCode.Enabled = IsExistInField("PC.PrDt.ProductCode", controlEnable);

                }
                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf_ProductCode = e.Row.FindControl("hf_ProductCode") as HiddenField;
                    hf_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }

                // Description (en)
                if (e.Row.FindControl("lbl_DescEn") != null)
                {
                    var lbl_DescEn = e.Row.FindControl("lbl_DescEn") as Label;
                    lbl_DescEn.Text = DataBinder.Eval(e.Row.DataItem, "Descen").ToString();
                    lbl_DescEn.ToolTip = lbl_DescEn.Text;

                    lbl_DescEn.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                }
                if (e.Row.FindControl("txt_DescEn") != null)
                {
                    var txt_DescEn = e.Row.FindControl("txt_DescEn") as TextBox;
                    txt_DescEn.Text = DataBinder.Eval(e.Row.DataItem, "Descen").ToString();

                    txt_DescEn.Visible = !IsExistInField("PC.PrDt.Descen", controlHide);
                    txt_DescEn.Enabled = IsExistInField("PC.PrDt.Descen", controlEnable);
                }

                // Description (local)
                if (e.Row.FindControl("lbl_DescLL") != null)
                {
                    var lbl_DescLL = e.Row.FindControl("lbl_DescLL") as Label;
                    lbl_DescLL.Text = DataBinder.Eval(e.Row.DataItem, "Descll").ToString();
                    lbl_DescLL.ToolTip = lbl_DescLL.Text;

                    lbl_DescLL.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                }
                if (e.Row.FindControl("txt_DescLL") != null)
                {
                    var txt_DescLL = e.Row.FindControl("txt_DescLL") as TextBox;
                    txt_DescLL.Text = DataBinder.Eval(e.Row.DataItem, "Descll").ToString();

                    txt_DescLL.Visible = !IsExistInField("PC.PrDt.Descll", controlHide);
                    txt_DescLL.Enabled = IsExistInField("PC.PrDt.Descll", controlEnable);
                }

                // Request Quantity
                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lbl_ReqQty = e.Row.FindControl("lbl_ReqQty") as Label;
                    lbl_ReqQty.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty"));
                    lbl_ReqQty.ToolTip = lbl_ReqQty.Text;

                    lbl_ReqQty.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                }
                if (e.Row.FindControl("txt_QtyRequest") != null)
                {
                    var txt_QtyRequest = e.Row.FindControl("txt_QtyRequest") as ASPxSpinEdit;
                    txt_QtyRequest.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString());

                    txt_QtyRequest.Visible = !IsExistInField("PC.PrDt.ReqQty", controlHide);
                    txt_QtyRequest.Enabled = IsExistInField("PC.PrDt.ReqQty", controlEnable);
                    // if not create step, disable (disallow edit)
                    txt_QtyRequest.Enabled = wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr);

                }

                // Request Approve Qty
                if (e.Row.FindControl("lbl_ApprQty") != null)
                {
                    var lbl_ApprQty = (Label)e.Row.FindControl("lbl_ApprQty");
                    lbl_ApprQty.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));

                    lbl_ApprQty.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                }

                if (e.Row.FindControl("txt_ApprQty") != null)
                {
                    var txt_ApprQty = (ASPxSpinEdit)e.Row.FindControl("txt_ApprQty");
                    txt_ApprQty.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ApprQty"));

                    txt_ApprQty.Visible = !IsExistInField("PC.PrDt.ApprQty", controlHide);
                    txt_ApprQty.Enabled = IsExistInField("PC.PrDt.ApprQty", controlEnable);
                    // Not allow in create step
                    txt_ApprQty.Enabled = !wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr);

                }

                // Unit
                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;

                    lbl_Unit.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                }
                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl_Unit = e.Row.FindControl("ddl_Unit") as ASPxComboBox;
                    ddl_Unit.Value = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();

                    ddl_Unit.Visible = !IsExistInField("PC.PrDt.OrderUnit", controlHide);
                    ddl_Unit.Enabled = IsExistInField("PC.PrDt.OrderUnit", controlEnable);

                    //if (IsExistInField("PC.PrDt.OrderUnit", controlEnable) && bool.Parse(conf.GetValue("PC", "PR", "UnitChanged", hf_ConnStr.Value)))
                    //    ddl_Unit.Enabled = true;
                    //else
                    //    ddl_Unit.Enabled = false;

                }

                // Delivery Date
                if (e.Row.FindControl("lbl_DeliDate") != null)
                {
                    var lbl_DeliDate = e.Row.FindControl("lbl_DeliDate") as Label;
                    if (DataBinder.Eval(e.Row.DataItem, "ReqDate") != DBNull.Value)
                    {
                        lbl_DeliDate.Text = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString()).ToString("dd/MM/yyyy");
                        lbl_DeliDate.ToolTip = lbl_DeliDate.Text;

                        lbl_DeliDate.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                    }
                }
                if (e.Row.FindControl("dte_Date") != null)
                {
                    var dte_Date = e.Row.FindControl("dte_Date") as ASPxDateEdit;

                    if (DataBinder.Eval(e.Row.DataItem, "ReqDate") != DBNull.Value)
                    {
                        dte_Date.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString());

                        dte_Date.Visible = !IsExistInField("PC.PrDt.ReqDate", controlHide);
                        dte_Date.Enabled = IsExistInField("PC.PrDt.ReqDate", controlEnable);
                    }
                }

                // Delivery Point
                if (e.Row.FindControl("lbl_DePoint") != null)
                {
                    var lbl_DePoint = e.Row.FindControl("lbl_DePoint") as Label;
                    lbl_DePoint.Text = DataBinder.Eval(e.Row.DataItem, "DeliPoint") + " : " + deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), buConnStr);
                    lbl_DePoint.ToolTip = lbl_DePoint.Text;

                    lbl_DePoint.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                }
                if (e.Row.FindControl("ddl_DeliPoint") != null)
                {
                    var ddl_DeliPoint = e.Row.FindControl("ddl_DeliPoint") as ASPxComboBox;
                    var hf_DeliPoint = e.Row.FindControl("hf_DeliPoint") as HiddenField;

                    hf_DeliPoint.Value = DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString();
                    ddl_DeliPoint.Value = string.IsNullOrEmpty(hf_DeliPoint.Value) ? string.Empty : DataBinder.Eval(e.Row.DataItem, "DeliPoint") + " : " + deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), buConnStr);

                    ddl_DeliPoint.Visible = !IsExistInField("PC.PrDt.DeliPoint", controlHide);
                    ddl_DeliPoint.Enabled = IsExistInField("PC.PrDt.DeliPoint", controlEnable);
                }


                // Comment. Control is in Expand.
                if (e.Row.FindControl("lbl_Comment_Detail") != null)
                {
                    var lbl_Comment_Detail = e.Row.FindControl("lbl_Comment_Detail") as Label;
                    lbl_Comment_Detail.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment_Detail.ToolTip = lbl_Comment_Detail.Text;
                }
                if (e.Row.FindControl("txt_Comment_Detail") != null)
                {
                    var txt_Comment_Detail = e.Row.FindControl("txt_Comment_Detail") as TextBox;
                    //txt_Comment_Detail.Enabled = wfDt.IsEnableEdit(wfId, wfStep, "PC.PrDt.Comment", hf_ConnStr.Value);
                    txt_Comment_Detail.Visible = !IsExistInField("PC.PrDt.Comment", controlHide);
                    txt_Comment_Detail.Enabled = IsExistInField("PC.PrDt.Comment", controlEnable);

                    txt_Comment_Detail.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                //***************************************** Add New Issue ********************************************************
                if (e.Row.FindControl("lbl_TaxRate_Grd") != null)
                {
                    var lbl_TaxRate_Grd = e.Row.FindControl("lbl_TaxRate_Grd") as Label;
                    lbl_TaxRate_Grd.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "TaxRate")) + " % ";
                    lbl_TaxRate_Grd.ToolTip = lbl_TaxRate_Grd.Text;
                }

                if (e.Row.FindControl("lbl_TaxType_Grd") != null)
                {
                    var lbl_TaxType_Grd = e.Row.FindControl("lbl_TaxType_Grd") as Label;
                    lbl_TaxType_Grd.Text =
                        GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                    lbl_TaxType_Grd.ToolTip = lbl_TaxType_Grd.Text;
                }

                if (e.Row.FindControl("lbl_DiscPercent_Grd") != null)
                {
                    var lbl_DiscPercent_Grd = e.Row.FindControl("lbl_DiscPercent_Grd") as Label;
                    lbl_DiscPercent_Grd.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "DiscPercent")) +
                                               " % ";
                    lbl_DiscPercent_Grd.ToolTip = lbl_DiscPercent_Grd.Text;
                }

                #region Other Currency
                if (e.Row.FindControl("lbl_CurrCurrDt_Grd") != null)
                {
                    Label lbl_CurrCurrDt_Grd = (Label)e.Row.FindControl("lbl_CurrCurrDt_Grd");
                    lbl_CurrCurrDt_Grd.Text = string.Format("( {0} )", DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }

                if (e.Row.FindControl("lbl_CurrNetAmt_Grd") != null)
                {
                    Label lbl_CurrNetAmt_Grd = (Label)e.Row.FindControl("lbl_CurrNetAmt_Grd");
                    lbl_CurrNetAmt_Grd.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                }

                if (e.Row.FindControl("lbl_CurrDiscAmt_Grd") != null)
                {
                    Label lbl_CurrDiscAmt_Grd = (Label)e.Row.FindControl("lbl_CurrDiscAmt_Grd");
                    lbl_CurrDiscAmt_Grd.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt"));
                }

                if (e.Row.FindControl("lbl_CurrTaxAmt_Grd") != null)
                {
                    Label lbl_CurrTaxAmt_Grd = (Label)e.Row.FindControl("lbl_CurrTaxAmt_Grd");
                    lbl_CurrTaxAmt_Grd.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt_Grd") != null)
                {
                    Label lbl_CurrTotalAmt_Grd = (Label)e.Row.FindControl("lbl_CurrTotalAmt_Grd");
                    lbl_CurrTotalAmt_Grd.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                }
                #endregion

                #region Base Currency
                if (e.Row.FindControl("lbl_BaseCurrDt_Grd") != null)
                {
                    Label lbl_BaseCurrDt_Grd = (Label)e.Row.FindControl("lbl_BaseCurrDt_Grd");
                    lbl_BaseCurrDt_Grd.Text = string.Format("( {0} )", baseCurrency);
                }

                if (e.Row.FindControl("lbl_NetAmt_Grd") != null)
                {
                    var lbl_NetAmt_Grd = e.Row.FindControl("lbl_NetAmt_Grd") as Label;
                    lbl_NetAmt_Grd.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                    lbl_NetAmt_Grd.ToolTip = lbl_NetAmt_Grd.Text;
                }

                if (e.Row.FindControl("lbl_DiscAmt_Grd") != null)
                {
                    var lbl_DiscAmt_Grd = e.Row.FindControl("lbl_DiscAmt_Grd") as Label;
                    lbl_DiscAmt_Grd.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                    lbl_DiscAmt_Grd.ToolTip = lbl_DiscAmt_Grd.Text;
                }

                if (e.Row.FindControl("lbl_TaxAmt_Grd") != null)
                {
                    var lbl_TaxAmt_Grd = e.Row.FindControl("lbl_TaxAmt_Grd") as Label;
                    lbl_TaxAmt_Grd.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    lbl_TaxAmt_Grd.ToolTip = lbl_TaxAmt_Grd.Text;
                }

                if (e.Row.FindControl("lbl_TotalAmt_Grd") != null)
                {
                    var lbl_TotalAmt_Grd = e.Row.FindControl("lbl_TotalAmt_Grd") as Label;
                    lbl_TotalAmt_Grd.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_TotalAmt_Grd.ToolTip = lbl_TotalAmt_Grd.Text;
                }
                #endregion

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chk_Adj = e.Row.FindControl("chk_Adj") as CheckBox;
                    chk_Adj.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj"));
                    chk_Adj.Enabled = false;
                }

                // Added on: 07/08/2017, By: Fon, About: Currency.
                // State: View
                if (e.Row.FindControl("lbl_CurrCode") != null)
                {
                    Label lbl_CurrCode = (Label)e.Row.FindControl("lbl_CurrCode");
                    lbl_CurrCode.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                    lbl_CurrCode.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                }

                if (e.Row.FindControl("lbl_CurrRate") != null)
                {
                    Label lbl_CurrRate = (Label)e.Row.FindControl("lbl_CurrRate");
                    lbl_CurrRate.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));
                    lbl_CurrRate.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                }

                // State: Edit
                if (e.Row.FindControl("ddl_CurrCode") != null)
                {
                    ASPxComboBox ddl_CurrCode = (ASPxComboBox)e.Row.FindControl("ddl_CurrCode");
                    ddl_CurrCode.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));

                    ddl_CurrCode.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                    ddl_CurrCode.Enabled = false;
                    // Because: Only AllocateVendor can edit.

                }

                if (e.Row.FindControl("comb_CurrRate") != null)
                {
                    ASPxComboBox comb_CurrRate = (ASPxComboBox)e.Row.FindControl("comb_CurrRate");
                    comb_CurrRate.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));

                    comb_CurrRate.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                    comb_CurrRate.Enabled = false;
                    // Because: Only AllocateVendor can edit.
                }
                #endregion

                if (wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr)) // Create
                {
                    if (lnkb_Edit != null && lnkb_Delete != null)
                    {
                        lnkb_Edit.Enabled = true;
                        lnkb_Delete.Enabled = true;
                    }

                    // ไม่สามารถลบ Detail ได้ หากอยู่ใน header's status 'Patial'
                    if (dsPR.Tables[pr.TableName].Rows[0]["ApprStatus"].ToString().Contains('P'))
                    {
                        if (lnkb_Delete != null)
                            lnkb_Delete.Enabled = false;
                    }
                }
                else // Approve
                {
                    if (lnkb_Delete != null)
                        lnkb_Delete.Visible = false;

                    if (isPreviousStepNoApproved || !isCurrentStepNoApproved)
                    {
                        if (chk_Item != null)
                            chk_Item.Visible = false;
                        if (lnkb_Edit != null)
                            lnkb_Edit.Visible = false;
                        if (lnkb_Delete != null)
                            lnkb_Delete.Visible = false;
                    }
                }


                if (e.Row.FindControl("lbl_DeliPoint") != null)
                {
                    var lbl_DeliPoint = e.Row.FindControl("lbl_DeliPoint") as Label;
                    lbl_DeliPoint.Text = deliPoint.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(), hf_ConnStr.Value);
                }

                if (e.Row.FindControl("lbl_DeDate") != null)
                {
                    var lbl_DeDate = e.Row.FindControl("lbl_DeDate") as Label;
                    lbl_DeDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString()).ToString("dd/MM/yyyy");
                    lbl_DeDate.ToolTip = lbl_DeDate.Text;
                }


                if (e.Row.FindControl("lbl_DiscPercent") != null)
                {
                    var lbl_DiscPercent = e.Row.FindControl("lbl_DiscPercent") as Label;
                    lbl_DiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                    lbl_DiscPercent.ToolTip = lbl_DiscPercent.Text;
                }

                if (e.Row.FindControl("lbl_Buyer") != null)
                {
                    var lbl_Buyer = e.Row.FindControl("lbl_Buyer") as Label;
                    lbl_Buyer.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                    lbl_Buyer.ToolTip = lbl_Buyer.Text;
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                    lbl_Price.ToolTip = lbl_Price.Text;
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                    lbl_DiscAmt.ToolTip = lbl_DiscAmt.Text;
                }

                if (e.Row.FindControl("lbl_Receive") != null)
                {
                    var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                    lbl_Receive.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RcvQty").ToString());
                    lbl_Receive.ToolTip = lbl_Receive.Text;
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    string taxType = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString()[0].ToString();
                    switch (taxType)
                    {
                        case "A": lbl_TaxType.Text = "Add";
                            break;
                        case "I": lbl_TaxType.Text = "Include";
                            break;
                        default: lbl_TaxType.Text = "None";
                            break;

                    }

                    lbl_TaxType.ToolTip = lbl_TaxType.Text;
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_TotalAmt.ToolTip = lbl_TotalAmt.Text;
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                    lbl_TaxRate.ToolTip = lbl_TaxRate.Text;
                }

                if (e.Row.FindControl("lbl_Ref") != null)
                {
                    var lbl_Ref = e.Row.FindControl("lbl_Ref") as Label;
                    lbl_Ref.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                    lbl_Ref.ToolTip = lbl_Ref.Text;
                }

                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl_NetAmt = e.Row.FindControl("lbl_NetAmt") as Label;
                    lbl_NetAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                    lbl_NetAmt.ToolTip = lbl_NetAmt.Text;
                }

                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl_TaxAmt = e.Row.FindControl("lbl_TaxAmt") as Label;
                    lbl_TaxAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    lbl_TaxAmt.ToolTip = lbl_TaxAmt.Text;
                }

                if (e.Row.FindControl("lbl_Po") != null)
                {
                    var lbl_Po = e.Row.FindControl("lbl_Po") as Label;
                    lbl_Po.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                    lbl_Po.ToolTip = lbl_Po.Text;
                }

                if (e.Row.FindControl("lbl_Order") != null)
                {
                    var lbl_Order = e.Row.FindControl("lbl_Order") as Label;
                    lbl_Order.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    lbl_Order.ToolTip = lbl_Order.Text;
                }

                // Added on: 07/08/2017, By: Fon, About: Currency.
                // State: View
                if (e.Row.FindControl("lbl_CurrCode_av") != null)
                {
                    Label lbl_CurrCode_av = (Label)e.Row.FindControl("lbl_CurrCode_av");
                    lbl_CurrCode_av.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                    lbl_CurrCode_av.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                }

                if (e.Row.FindControl("lbl_CurrRate_Av") != null)
                {
                    Label lbl_CurrRate_Av = (Label)e.Row.FindControl("lbl_CurrRate_Av");
                    lbl_CurrRate_Av.Text = string.Format("{0:N6}", DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));
                    lbl_CurrRate_Av.Enabled = IsExistInField("PC.PrDt.CurrencyCode", controlEnable);
                }

                // State: Edit
                if (e.Row.FindControl("ddl_CurrCode_av") != null)
                {
                    ASPxComboBox ddl_CurrCode_av = (ASPxComboBox)e.Row.FindControl("ddl_CurrCode_av");
                    ddl_CurrCode_av.Visible = !IsExistInField("PC.PrDt.CurrencyCode", controlHide);
                    ddl_CurrCode_av.Enabled = IsExistInField("PC.PrDt.CurrencyCode", controlEnable);
                    ddl_CurrCode_av.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));

                    if (e.Row.FindControl("ddl_CurrRate_av") != null)
                    {
                        ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)e.Row.FindControl("ddl_CurrRate_av");
                        if (ddl_CurrCode_av.Text != string.Empty)
                        {
                            ddl_CurrRate_av.DataSource = Get_CurrencyHistory(ddl_CurrCode_av.Value.ToString());
                            ddl_CurrRate_av.DataBind();
                            ddl_CurrRate_av.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));
                            ddl_CurrRate_av.Enabled = IsExistInField("PC.PrDt.CurrencyCode", controlEnable);
                        }
                    }
                }
                // End Added.

                #region "Display Stock Summary"

                //****************** Display Stock Summary ******************
                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                    txt_PrDate.Text, hf_ConnStr.Value);

                if (getPrDtStockSum)
                {
                    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                    {
                        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        lbl_OnHand.Text = string.Format(DefaultQtyFmt, drStockSummary["OnHand"].ToString());
                        lbl_OnHand.ToolTip = lbl_OnHand.Text;

                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                        lbl_OnOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["OnOrder"].ToString());
                        lbl_OnOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_ReOrder = e.Row.FindControl("lbl_ReOrder") as Label;
                        lbl_ReOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["Reorder"].ToString());
                        lbl_ReOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;
                        lbl_Restock.Text = string.Format(DefaultQtyFmt, drStockSummary["Restock"].ToString());
                        lbl_Restock.ToolTip = lbl_Restock.Text;

                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;

                        //var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                        //lbl_LastPrice.Text = string.Format(DefaultAmtFmt, drStockSummary["LastPrice"].ToString());
                        //lbl_LastPrice.ToolTip = lbl_LastPrice.Text;

                        //var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        //lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                        //lbl_LastVendor.ToolTip = lbl_LastVendor.Text;

                        var lastPrice = DataBinder.Eval(e.Row.DataItem, "LastPrice").ToString();
                        var lastVendor = DataBinder.Eval(e.Row.DataItem, "VendorProdCode").ToString();

                        lbl_LastPrice.Text = string.IsNullOrEmpty(lastPrice) ? "0" : FormatAmt(Convert.ToDecimal(lastPrice));

                        if (string.IsNullOrEmpty(lastVendor))
                            lbl_LastVendor.Text = "";
                        else
                        {
                            var dt = prDt.DbExecuteQuery(string.Format("SELECT CONCAT(VendorCode,' : ', [Name]) as Vendor FROM AP.Vendor WHERE VendorCode=N'{0}'", lastVendor), null, hf_ConnStr.Value);

                            lbl_LastVendor.Text = dt.Rows.Count == 0 ? "" : dt.Rows[0][0].ToString();
                        }

                        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                    }
                }

                #endregion




                // Price Compare
                dsPriceCompare.Clear();

                // Display price compare  
                var grd_PriceCompare1 = e.Row.FindControl("grd_PriceCompare1") as GridView;

                if (DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() != string.Empty)
                {
                    var result = false;

                    if (ddl_PrType.SelectedItem.Value.ToString() == "1") //Marketlist
                    {
                        result = priceList.GetList(dsPriceCompare,
                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                            DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString()),
                            decimal.Parse(DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString()),
                            DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString(), LoginInfo.ConnStr);
                    }
                    else //Non market list
                    {
                        result = priceList.GetList(dsPriceCompare,
                            DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                            DateTime.Parse(txt_PrDate.Text),
                            decimal.Parse(DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString()),
                            DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString(), LoginInfo.ConnStr);
                    }
                }

                grd_PriceCompare1.DataSource = dsPriceCompare.Tables[priceList.TableName];
                grd_PriceCompare1.DataBind();

                if (!LoginInfo.BuInfo.IsHQ)
                {
                    grd_PriceCompare1.Columns[0].Visible = false;
                }

                if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]))
                {
                    grd_PriceCompare1.Columns[11].Visible = true;
                }
                else
                {
                    grd_PriceCompare1.Columns[11].Visible = false;
                }

            }

            #endregion DataRow

        }

        protected void grd_PrDt1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            #region Menu Visible
            // Disable Create and Delete When click edit.
            menu_GrdBar.Items[0].Visible = false;
            menu_GrdBar.Items[1].Visible = false;

            menu_CmdBar.Items.FindByName("Save").Visible = false;
            menu_CmdBar.Items.FindByName("Commit").Visible = false;
            menu_CmdBar.Items.FindByName("Back").Visible = false;

            txt_PrDate.Enabled = false;

            #endregion Menuvisible

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = e.NewEditIndex;
            grd_PrDt1.DataBind();

            // Expand button
            var Img_Btn1 = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("Img_Btn1") as ImageButton;
            Img_Btn1.Visible = false;

            // CheckBox
            var Chk_Item = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("Chk_Item") as CheckBox;
            Chk_Item.Visible = false;

            // Save and New button
            var lnkb_SaveNew = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("lnkb_SaveNew") as LinkButton;
            lnkb_SaveNew.Visible = wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr); //if allow create is true, then visible

            var grd_PriceCompare = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("grd_PriceCompare1") as GridView;
            grd_PriceCompare.Columns[11].Visible = false;

            // Added on: 18/01/2018
            bool isAllocateVendor = Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]);

            if (isAllocateVendor)
            {
                ASPxComboBox ddl_Vendor_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Vendor_av");
                //ddl_Vendor_av_OnSelectedIndexChanged(ddl_Vendor_av, e);
            }
            else
            {
                ASPxComboBox ddl_LocationCode = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode");
                ddl_LocationCode_SelectedIndexChanged(ddl_LocationCode, e);
            }
            // End Added.

            PRDtEditMode = "EDIT";
        }

        protected void grd_PrDt1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (MODE.ToUpper() == "NEW" || MODE.ToUpper() == "TEMPLATE")
            {
                if (PRDtEditMode == "NEW" || PRDtEditMode == "TEMPLATE")
                {
                    dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1].Delete();
                }

                if (PRDtEditMode == "EDIT")
                {
                    dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (MODE.ToUpper() == "EDIT")
            {
                if (PRDtEditMode == "NEW")
                {
                    dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1].Delete();
                }

                if (PRDtEditMode == "EDIT")
                {
                    dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();

            // Enable Create and Delete When click cancel.
            if (!wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr))
            {
                menu_GrdBar.Items[0].Visible = false; //Create
                menu_GrdBar.Items[1].Visible = false; //Delete

                menu_CmdBar.Items.FindByName("Commit").Visible = false;
            }
            else
            {
                menu_GrdBar.Items[0].Visible = true; //Create
                menu_GrdBar.Items[1].Visible = true; //Delete

                menu_CmdBar.Items.FindByName("Commit").Visible = true;
            }
            menu_CmdBar.Items.FindByName("Save").Visible = true;
            menu_CmdBar.Items.FindByName("Back").Visible = true;

            PRDtEditMode = string.Empty;
        }

        protected void grd_PrDt1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            bool isCreateStep = wfDt.GetAllowCreate(wfId, wfStep, hf_ConnStr.Value);

            var drUpdating = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.Rows[grd_PrDt1.EditIndex].DataItemIndex];


            var ddl_Vendor_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Vendor_av") as ASPxComboBox;
            var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
            var ddl_BuCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode_av") as ASPxComboBox;
            var ddl_LocationCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode_av") as ASPxComboBox;
            var ddl_ProductCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode_av") as ASPxComboBox;

            var row = grd_PrDt1.Rows[grd_PrDt1.EditIndex];

            var lbl_LastPrice = row.FindControl("lbl_LastPrice") as Label;
            var lbl_LastVendor = row.FindControl("lbl_LastVendor") as Label;


            DataRow drWFDt = dsWF.Tables["APPwfdt"].Rows[0];

            string locationCode = string.Empty;

            var isAllocateVendor = Convert.ToBoolean(drWFDt["IsAllocateVendor"]);

            if (isAllocateVendor)
            {
                #region IsAllocateVendor
                // Allocate Vendor.                
                if (ddl_Vendor_av.Value != null)
                {
                    drUpdating["VendorCode"] = ddl_Vendor_av.Value.ToString().Split(':')[0].Trim();
                }

                if (ddl_BuCode_av.Value != null)
                {
                    drUpdating["BuCode"] = ddl_BuCode_av.Value.ToString();
                }

                if (ddl_LocationCode_av.Value != null)
                {
                    drUpdating["LocationCode"] = ddl_LocationCode_av.Value.ToString();
                    locationCode = ddl_LocationCode_av.Value.ToString();
                }

                if (ddl_ProductCode_av.Value != null)
                {
                    drUpdating["ProductCode"] = ddl_ProductCode_av.Text.Split(':')[0].Trim();
                }

                var dte_DeliDate_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("dte_DeliDate_av") as ASPxDateEdit;
                if (!string.IsNullOrEmpty(dte_DeliDate_av.Text))
                {
                    drUpdating["ReqDate"] = DateTime.Parse(dte_DeliDate_av.Date.ToString("dd/MM/yyyy"));
                }

                var ddl_DeliPoint_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint_av") as ASPxComboBox;
                var DeliPoint = ddl_DeliPoint_av.Value.ToString().Split(':');
                if (ddl_DeliPoint_av.Value != null)
                {
                    drUpdating["DeliPoint"] = int.Parse(DeliPoint[0]);
                }

                var txt_ReqQty_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;

                if (!string.IsNullOrEmpty(txt_ReqQty_av.Text) && decimal.Parse(txt_ReqQty_av.Text) != 0)
                {
                    drUpdating["ReqQty"] = txt_ReqQty_av.Text;
                }
                else
                {
                    pop_AlertQty.ShowOnPageLoad = true;
                    return;
                }

                var txt_FOC_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_FOC_av") as ASPxSpinEdit;
                if (!string.IsNullOrEmpty(txt_FOC_av.Text))
                {
                    drUpdating["FOCQty"] = txt_FOC_av.Text;
                }
                else
                {
                    drUpdating["FOCQty"] = 0;
                }

                var ddl_Unit_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit_av") as ASPxComboBox;
                drUpdating["OrderUnit"] = ddl_Unit_av.Value;

                var txt_NetAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_NetAmt_Grd_Av") as TextBox;
                drUpdating["NetAmt"] = txt_NetAmt_Grd_Av.Text;

                var txt_TaxAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxAmt_Grd_Av") as TextBox;
                drUpdating["TaxAmt"] = txt_TaxAmt_Grd_Av.Text;

                var txt_TotalAmt_Grd_Av =
                    grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TotalAmt_Grd_Av") as TextBox;
                drUpdating["TotalAmt"] = txt_TotalAmt_Grd_Av.Text;


                var txt_Price_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;
                if (Convert.ToDecimal(txt_Price_av.Text) > 0)
                {
                    drUpdating["Price"] = txt_Price_av.Text;
                }
                else if (Convert.ToDecimal(txt_FOC_av.Text) > 0)
                {
                    drUpdating["Price"] = txt_Price_av.Text;
                }
                else
                {
                    pop_AlertPrice.ShowOnPageLoad = true;
                    return;
                }

                // Added on: 08/08/2017, By: Fon
                ASPxComboBox ddl_CurrCode_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_CurrCode_av");
                ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_CurrRate_av");
                TextBox txt_CurrNetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrNetAmt_Grd_Av");
                TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrDiscAmt_Grd_Av");
                TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");
                TextBox txt_CurrTotalAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTotalAmt_Grd_Av");

                drUpdating["CurrencyCode"] = ddl_CurrCode_av.Value.ToString().Trim();
                drUpdating["CurrencyRate"] = ddl_CurrRate_av.Value.ToString();
                drUpdating["CurrNetAmt"] = txt_CurrNetAmt_Grd_Av.Text;
                drUpdating["CurrTaxAmt"] = txt_CurrTaxAmt_Grd_Av.Text;
                drUpdating["CurrTotalAmt"] = txt_CurrTotalAmt_Grd_Av.Text;
                // End Added.


                var chk_Adj_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("chk_Adj_Grd_Av") as ASPxCheckBox;

                drUpdating["TaxAdj"] = chk_Adj_Grd_Av.Checked;

                var ddl_TaxType_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
                var txt_TaxRate_Grd_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxRate_Grd_av") as TextBox;

                if (!chk_Adj_Grd_Av.Checked)
                {
                    //drUpdating["TaxType"] = product.GetTaxType(drUpdating["ProductCode"].ToString(), hf_ConnStr.Value);
                    //drUpdating["TaxRate"] = product.GetTaxRate(drUpdating["ProductCode"].ToString(), hf_ConnStr.Value).ToString();
                }
                else
                {

                    drUpdating["TaxType"] = ddl_TaxType_Grd_Av.Value;


                    if (!string.IsNullOrEmpty(txt_TaxRate_Grd_av.Text) && ddl_TaxType_Grd_Av.Value.ToString() == "N")
                    {
                        drUpdating["TaxRate"] = 0;
                    }
                    else if (decimal.Parse(txt_TaxRate_Grd_av.Text) > 0 && ddl_TaxType_Grd_Av.Value.ToString() != "N")
                    {
                        drUpdating["TaxRate"] = txt_TaxRate_Grd_av.Text;
                    }
                    else
                    {
                        pop_AlertTaxRate.ShowOnPageLoad = true;
                        return;
                    }
                }

                var txt_DiscPercent_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DiscPercent_Grd_Av") as TextBox;
                if (!string.IsNullOrEmpty(txt_DiscPercent_Grd_Av.Text) &&
                    decimal.Parse(txt_DiscPercent_Grd_Av.Text) <= 100)
                {
                    drUpdating["DiscPercent"] = txt_DiscPercent_Grd_Av.Text;
                }
                else if (string.IsNullOrEmpty(txt_DiscPercent_Grd_Av.Text))
                {
                    drUpdating["DiscPercent"] = 0;
                    txt_DiscPercent_Grd_Av.Text = String.Format("{0:N}", drUpdating["DiscPercent"]);
                }
                else if (decimal.Parse(txt_DiscPercent_Grd_Av.Text) > 100)
                {
                    pop_AlertDisc.ShowOnPageLoad = true;
                    return;
                }

                var txt_DiscAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DiscAmt_Grd_Av") as TextBox;

                if (!string.IsNullOrEmpty(txt_CurrDiscAmt_Grd_Av.Text) &&
                   (decimal.Parse(txt_CurrDiscAmt_Grd_Av.Text) / decimal.Parse(txt_ReqQty_av.Text)) < decimal.Parse(txt_Price_av.Text))
                {
                    drUpdating["CurrDiscAmt"] = txt_CurrDiscAmt_Grd_Av.Text;
                    drUpdating["DiscAmt"] = txt_DiscAmt_Grd_Av.Text;
                }
                else if ((decimal.Parse(txt_CurrDiscAmt_Grd_Av.Text) / decimal.Parse(txt_ReqQty_av.Text)) > decimal.Parse(txt_Price_av.Text))
                {
                    pop_AlertDiscAmt.ShowOnPageLoad = true;
                    return;
                }
                else
                {
                    drUpdating["CurrDiscAmt"] = 0;
                    drUpdating["DiscAmt"] = 0;
                    txt_CurrDiscAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, drUpdating["CurrDiscAmt"]);
                    txt_DiscAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, drUpdating["DiscAmt"]);
                }



                var txt_ApprQty_av = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ApprQty_av");
                if (!string.IsNullOrEmpty(txt_ApprQty_av.Text) && decimal.Parse(txt_ApprQty_av.Text) != 0)
                {
                    drUpdating["ApprQty"] = RoundQty(decimal.Parse(txt_ApprQty_av.Text));
                }
                else
                {
                    if (!isCreateStep)
                    {
                        pop_AlertApprQty.ShowOnPageLoad = true;
                        return;
                    }
                }

                var txt_Comment_Detail =
                    grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Comment_Detail") as TextBox;
                drUpdating["Comment"] = (txt_Comment_Detail.Text != null ? txt_Comment_Detail.Text : string.Empty);

                decimal DiscAmt = (decimal.Parse(drUpdating["Price"].ToString()) * decimal.Parse(drUpdating["DiscPercent"].ToString())) / 100;
                #endregion
            }
            else
            {
                #region Not Allocate Vendor
                var ddl_ProductCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;

                drUpdating["ProductCode"] = ddl_ProductCode.Value.ToString().Split(' ')[0].Trim();

                var ddl_LocationCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;

                drUpdating["LocationCode"] = ddl_LocationCode.Value.ToString();
                locationCode = ddl_LocationCode.Value.ToString();

                drUpdating["Descen"] = "";
                drUpdating["Descll"] = "";

                //drUpdating["Descen"] = product.GetName(ddl_ProductCode.Text.Split(':')[0].Trim(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));
                //drUpdating["Descll"] = product.GetName2(ddl_ProductCode.Text.Split(':')[0].Trim(), bu.GetConnectionString(ddl_BuCode.Value.ToString()));


                //TextBox txt_Unit = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Unit") as TextBox;
                var ddl_Unit = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
                if (ddl_Unit != null && ddl_Unit.Value.ToString() != string.Empty)
                    drUpdating["OrderUnit"] = ddl_Unit.Value;
                else
                {
                    lbl_Pop_AlertBox.Text = "Unit is required.";
                    pop_AlertBox.ShowOnPageLoad = true;
                    return;
                }


                var chk_Adj = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("chk_Adj") as CheckBox;
                drUpdating["TaxAdj"] = chk_Adj.Checked;

                if (!chk_Adj.Checked)
                {
                    drUpdating["TaxType"] = product.GetTaxType(drUpdating["ProductCode"].ToString(), hf_ConnStr.Value);
                    drUpdating["TaxRate"] = product.GetTaxRate(drUpdating["ProductCode"].ToString(), hf_ConnStr.Value).ToString();
                }


                var txt_QtyRequest = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_QtyRequest") as ASPxSpinEdit;
                decimal reqQty = 0;
                decimal.TryParse(txt_QtyRequest.Text, out reqQty);
                if (reqQty > 0)
                {
                    drUpdating["ReqQty"] = reqQty;
                    drUpdating["ApprQty"] = reqQty;
                }
                else
                {
                    pop_AlertQty.ShowOnPageLoad = true;
                    return;
                }

                if (!isCreateStep)
                {
                    var txt_ApprQty = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ApprQty");
                    decimal apprQty = 0;
                    decimal.TryParse(txt_ApprQty.Text, out apprQty);
                    if (apprQty > 0)
                        drUpdating["ApprQty"] = Convert.ToDecimal(txt_ApprQty.Text);
                    else
                    {
                        pop_AlertApprQty.ShowOnPageLoad = true;
                        return;
                    }
                }

                // Modified on: 30/03/2018, By:Fon
                //drUpdating["BuCode"] = LoginInfo.BuInfo.BuCode;
                if (ddl_BuCode != null)
                {
                    drUpdating["BuCode"] = (LoginInfo.BuInfo.IsHQ)
                                ? ddl_BuCode.Value.ToString() : LoginInfo.BuInfo.BuCode;
                }

                var dte_Date = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("dte_Date") as ASPxDateEdit;
                if (!string.IsNullOrEmpty(dte_Date.Text))
                {
                    drUpdating["ReqDate"] = DateTime.Parse(dte_Date.Date.ToString("dd/MM/yyyy"));
                }

                var ddl_DeliPoint = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
                var DeliPoint = ddl_DeliPoint.Value.ToString().Split(':');
                if (ddl_DeliPoint.Value != null)
                {
                    drUpdating["DeliPoint"] = int.Parse(DeliPoint[0]);
                }

                var txt_Comment_Detail =
                    grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Comment_Detail") as TextBox;
                drUpdating["Comment"] = (txt_Comment_Detail.Text != null ? txt_Comment_Detail.Text : string.Empty);

                // Modified on: 01/09/2017, By: Fon
                var ddl_CurrCode_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_CurrCode_av");
                var comb_CurrRate_Grd_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("comb_CurrRate_Grd_av");


                if (ddl_CurrCode_av.Value != null)
                {
                    drUpdating["CurrencyCode"] = ddl_CurrCode_av.Value.ToString().Trim();
                }
                if (comb_CurrRate_Grd_av != null)
                {
                    if (comb_CurrRate_Grd_av.Text != string.Empty)
                        drUpdating["CurrencyRate"] = comb_CurrRate_Grd_av.Value.ToString();
                }

                // Because they don't have txt_Price in Issue panel.
                decimal qty = 0, price = 0, discRate = 0, currRate = 0;

                var lastPrice = string.IsNullOrEmpty(lbl_LastPrice.Text) ? 0m : Convert.ToDecimal(lbl_LastPrice.Text);
                var lastVendorCode = string.IsNullOrEmpty(lbl_LastVendor.Text) ? "" : lbl_LastVendor.Text.Split(':').Select(x => x.Trim()).First();

                // Last Price
                drUpdating["LastPrice"] = lastPrice;
                // Last Vendor
                drUpdating["VendorProdCode"] = lastVendorCode;

                if (wfStep == 1)
                {
                    drUpdating["VendorCode"] = lastVendorCode; 
                    drUpdating["Price"] = lastPrice;
                }

                decimal.TryParse(drUpdating["ApprQty"].ToString(), out qty);
                decimal.TryParse(drUpdating["Price"].ToString(), out price);
                decimal.TryParse(drUpdating["DiscPercent"].ToString(), out discRate);
                decimal.TryParse(drUpdating["CurrencyRate"].ToString(), out currRate);
                if (currRate == 0)
                    currRate = 1;

                string taxType = drUpdating["TaxType"].ToString();
                decimal taxRate = Convert.ToDecimal(drUpdating["TaxRate"]);

                decimal currDiscAmt = RoundAmt(((price * qty) * discRate) / 100);
                decimal discAmt = RoundAmt(currDiscAmt * currRate);

                drUpdating["CurrNetAmt"] = NetAmt(taxType, taxRate, price * qty, currDiscAmt, 1);
                drUpdating["CurrTaxAmt"] = TaxAmt(taxType, taxRate, price * qty, currDiscAmt, 1);
                drUpdating["CurrTotalAmt"] = Amount(taxType, taxRate, price * qty, currDiscAmt, 1);

                drUpdating["NetAmt"] = RoundAmt(Convert.ToDecimal(drUpdating["CurrNetAmt"]) * currRate);
                drUpdating["DiscAmt"] = RoundAmt(Convert.ToDecimal(drUpdating["CurrDiscAmt"]) * currRate);
                drUpdating["TaxAmt"] = RoundAmt(Convert.ToDecimal(drUpdating["CurrTaxAmt"]) * currRate);
                drUpdating["TotalAmt"] = RoundAmt(Convert.ToDecimal(drUpdating["CurrTotalAmt"]) * currRate);


                // End Modified
                #endregion
            }

            if (ddl_JobCode.Value != null)
            {
                drUpdating["AddField1"] = ddl_JobCode.Value.ToString();
                dsPR.Tables[pr.TableName].Rows[0]["AddField1"] = ddl_JobCode.Value.ToString();
                //dsPR.Tables[pr.TableName].Rows[grd_PrDt1.EditIndex]["AddField1"] = ddl_JobCode.Value.ToString();
            }

            if (txt_Desc.Text != "")
            {
                dsPR.Tables[pr.TableName].Rows[0]["Description"] = txt_Desc.Text;
                //dsPR.Tables[pr.TableName].Rows[grd_PrDt1.EditIndex]["Description"] = txt_Desc.Text;
            }

            // Enable Pr Type & Pr Date
            //ddl_PrType.Enabled = true;
            //txt_PrDate.Enabled = true;

            if (wfDt.GetAllowCreate(wfId, wfStep, hf_ConnStr.Value))
            {
                //menu_CmdBar.Items[0].Visible = true; //Save
                //menu_CmdBar.Items[1].Visible = true; //Back

                menu_GrdBar.Items[0].Visible = true; //Create
                menu_GrdBar.Items[1].Visible = true; //Delete

                menu_CmdBar.Items.FindByName("Save").Visible = true;
                menu_CmdBar.Items.FindByName("Commit").Visible = true;
                menu_CmdBar.Items.FindByName("Back").Visible = true;
            }
            else
            {
                menu_GrdBar.Items[0].Visible = false; //Create
                menu_GrdBar.Items[1].Visible = false; //Delete

                menu_CmdBar.Items.FindByName("Save").Visible = true;
                menu_CmdBar.Items.FindByName("Commit").Visible = false;
                menu_CmdBar.Items.FindByName("Back").Visible = true;
            }

            //ddl_JobCode.Enabled = true;
            PRDtEditMode = string.Empty;

            // Modified on: 10/11/2017, By: Fon
            /*grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();*/

            Control_Location(locationCode, dsPR.Tables[prDt.TableName]);
            // End Modified.

            Session["dsPR"] = dsPR;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    //if (ddl_JobCode.Value == null)
                    //{
                    //    lbl_WarningPeriod.Text = @"Please select 'JobCode'.";
                    //    pop_WarningPeriod.ShowOnPageLoad = true;
                    //    pop_WarningPeriod.Width = Unit.Pixel(300);
                    //    return;
                    //}

                    if (wfDt.GetAllowCreate(wfId, wfStep, hf_ConnStr.Value))
                        Save(e.Item.Name);
                    else
                        Save("Commit");
                    break;

                case "COMMIT":
                    if (ddl_JobCode.Value == null)
                    {
                        lbl_WarningPeriod.Text = @"Please select 'JobCode'.";
                        pop_WarningPeriod.ShowOnPageLoad = true;
                        pop_WarningPeriod.Width = Unit.Pixel(300);
                        return;
                    }
                    Save(e.Item.Name);
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        protected void menu_GrdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateDetail();
                    break;

                case "DELETE":
                    DeleteDetail();
                    break;
            }
        }

        protected void btn_OK_ApprQty_Click(object sender, EventArgs e)
        {
            pop_AlertApprQty.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);

        }

        protected void menu_OKBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "OK":
                    Ok_DeliDate();
                    break;
            }
        }

        protected void btn_Warning_OK_Click(object sender, EventArgs e)
        {
            pop_AlertProductSame.ShowOnPageLoad = false;

            //ddl_PrType.SelectedValue = hf_PrType.Value;
            ddl_PrType.Value = hf_PrType.Value;
        }

        // ********************************* 22/09/2011 ********************************************
        protected void btn_WarningProdCateType_OK_Click(object sender, EventArgs e)
        {
            pop_AlertProdCateType.ShowOnPageLoad = false;
        }

        // *****************************************************************************************

        protected void grd_PrDt1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //pop_ConfirmDelete_Grd.ShowOnPageLoad = true;

            dsPR.Tables[prDt.TableName].Rows[e.RowIndex].Delete();

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();

            PRDtEditMode = string.Empty;
            //ddl_JobCode.Enabled = true;
            Session["dsPR"] = dsPR;
        }

        protected void grd_PrDt1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Save New
            if (e.CommandName.ToUpper() == "SAVENEW")
            {

                var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
                var ddl_BuCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode_av") as ASPxComboBox;

                var ddl_Vendor_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Vendor_av") as ASPxComboBox;


                var ddl_LocationCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode_av") as ASPxComboBox;

                var ddl_ProductCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode_av") as ASPxComboBox;
                var ddlProductCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;


                if (ddlProductCode.Value == null || string.IsNullOrEmpty(ddlProductCode.Value.ToString()))
                {
                    return;
                }

                var drUpdating = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.Rows[grd_PrDt1.EditIndex].DataItemIndex];

                drUpdating["HOD"] = HOD;

                var dte_DeliDate_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("dte_DeliDate_av") as ASPxDateEdit;
                drUpdating["ReqDate"] = dte_DeliDate_av.Date;


                #region Allocate Vendor.
                if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]))
                {
                    // Allocate Vendor.
                    drUpdating["BuCode"] = ddl_BuCode_av.Value == null ? BuCode : ddl_BuCode_av.Value.ToString();
                    drUpdating["VendorCode"] = ddl_Vendor_av.Value == null ? string.Empty : ddl_Vendor_av.Value.ToString();
                    drUpdating["LocationCode"] = ddl_LocationCode_av.Value == null ? string.Empty : ddl_LocationCode_av.Value.ToString();
                    drUpdating["ProductCode"] = ddl_ProductCode_av.Value == null ? string.Empty : ddl_ProductCode_av.Value.ToString();

                    //var txt_DescEN_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("Txt_DescEN_av") as TextBox;
                    //var txt_DescEN_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("Txt_DescEN_av") as Label;
                    //drUpdating["Descen"] = txt_DescEN_av.Text;
                    //var txt_DescLL_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescLL_av") as TextBox;
                    //var txt_DescLL_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescLL_av") as Label;
                    //drUpdating["Descll"] = txt_DescLL_av.Text;


                    var ddl_DeliPoint_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint_av") as ASPxComboBox;
                    var DeliPoint = ddl_DeliPoint_av.Value.ToString().Split(':');
                    if (ddl_DeliPoint_av.Value != null)
                    {
                        drUpdating["DeliPoint"] = int.Parse(DeliPoint[0]);
                    }

                    var txt_ReqQty_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;

                    if (!string.IsNullOrEmpty(txt_ReqQty_av.Text) && decimal.Parse(txt_ReqQty_av.Text) != 0)
                    {
                        drUpdating["ReqQty"] = txt_ReqQty_av.Text;
                    }
                    else
                    {
                        pop_AlertQty.ShowOnPageLoad = true;
                        return;
                    }

                    var txt_FOC_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_FOC_av") as ASPxSpinEdit;

                    if (!string.IsNullOrEmpty(txt_FOC_av.Text))
                    {
                        drUpdating["FOCQty"] = txt_FOC_av.Text;
                    }
                    else
                    {
                        drUpdating["FOCQty"] = 0;
                    }

                    var ddl_Unit_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit_av") as ASPxComboBox;
                    drUpdating["OrderUnit"] = ddl_Unit_av.Value;

                    var txt_CurrNetAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrNetAmt_Grd_Av") as TextBox;
                    drUpdating["CurrNetAmt"] = txt_CurrNetAmt_Grd_Av.Text;

                    var txt_CurrTaxAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTaxAmt_Grd_Av") as TextBox;
                    drUpdating["CurrTaxAmt"] = txt_CurrTaxAmt_Grd_Av.Text;

                    var txt_CurrTotalAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTotalAmt_Grd_Av") as TextBox;
                    drUpdating["CurrTotalAmt"] = txt_CurrTotalAmt_Grd_Av.Text;


                    var txt_NetAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_NetAmt_Grd_Av") as TextBox;
                    drUpdating["NetAmt"] = txt_NetAmt_Grd_Av.Text;

                    var txt_TaxAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxAmt_Grd_Av") as TextBox;
                    drUpdating["TaxAmt"] = txt_TaxAmt_Grd_Av.Text;

                    var txt_TotalAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TotalAmt_Grd_Av") as TextBox;
                    drUpdating["TotalAmt"] = txt_TotalAmt_Grd_Av.Text;

                    var txt_Price_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;
                    if (Convert.ToDecimal(txt_Price_av.Text) > 0)
                    {
                        drUpdating["Price"] = txt_Price_av.Text;
                    }
                    else if (Convert.ToDecimal(txt_FOC_av.Text) > 0)
                    {
                        drUpdating["Price"] = txt_Price_av.Text;
                    }
                    else
                    {
                        pop_AlertPrice.ShowOnPageLoad = true;
                        return;
                    }

                    string taxType = string.Empty;
                    var ddl_TaxType_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
                    if (ddl_TaxType_Grd_Av.Value != null)
                    {
                        taxType = ddl_TaxType_Grd_Av.Value.ToString();
                        drUpdating["TaxType"] = ddl_TaxType_Grd_Av.Value;
                    }

                    var txt_TaxRate_Grd_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxRate_Grd_av") as TextBox;
                    if (ddl_TaxType_Grd_Av.Value != null)
                    {
                        if (!string.IsNullOrEmpty(txt_TaxRate_Grd_av.Text) && ddl_TaxType_Grd_Av.Value.ToString() == "N")
                        {
                            drUpdating["TaxRate"] = 0;
                        }
                        else if (txt_TaxRate_Grd_av.Text != "0" && ddl_TaxType_Grd_Av.Value.ToString() != "N")
                        {
                            drUpdating["TaxRate"] = txt_TaxRate_Grd_av.Text;
                        }
                        else
                        {
                            pop_AlertTaxRate.ShowOnPageLoad = true;
                            return;
                        }
                    }
                    else
                    {
                        drUpdating["TaxType"] = "N";
                        drUpdating["TaxRate"] = 0;
                    }

                    var txt_DiscPercent_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DiscPercent_Grd_Av") as TextBox;

                    if (!string.IsNullOrEmpty(txt_DiscPercent_Grd_Av.Text) && decimal.Parse(txt_DiscPercent_Grd_Av.Text) <= 100)
                    {
                        drUpdating["DiscPercent"] = txt_DiscPercent_Grd_Av.Text;
                    }
                    else if (string.IsNullOrEmpty(txt_DiscPercent_Grd_Av.Text))
                    {
                        drUpdating["DiscPercent"] = 0;
                        txt_DiscPercent_Grd_Av.Text = String.Format("{0:N}", drUpdating["DiscPercent"]);
                    }
                    else if (decimal.Parse(txt_DiscPercent_Grd_Av.Text) > 100)
                    {
                        pop_AlertDisc.ShowOnPageLoad = true;
                        return;
                    }

                    var txt_DiscAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DiscAmt_Grd_Av") as TextBox;

                    if (!string.IsNullOrEmpty(txt_DiscAmt_Grd_Av.Text) && (decimal.Parse(txt_DiscAmt_Grd_Av.Text) / decimal.Parse(txt_ReqQty_av.Text)) < decimal.Parse(txt_Price_av.Text))
                    {
                        drUpdating["DiscAmt"] = txt_DiscAmt_Grd_Av.Text;
                    }
                    else if ((decimal.Parse(txt_DiscAmt_Grd_Av.Text) / decimal.Parse(txt_ReqQty_av.Text)) > decimal.Parse(txt_Price_av.Text))
                    {
                        pop_AlertDiscAmt.ShowOnPageLoad = true;
                        return;
                    }
                    else
                    {
                        drUpdating["DiscAmt"] = 0;
                        txt_DiscAmt_Grd_Av.Text = String.Format(DefaultAmtFmt, drUpdating["DiscAmt"]);
                    }

                    var chk_Adj_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("chk_Adj_Grd_Av") as ASPxCheckBox;
                    drUpdating["TaxAdj"] = chk_Adj_Grd_Av.Checked;

                    if (!string.IsNullOrEmpty(txt_ReqQty_av.Text) && decimal.Parse(txt_ReqQty_av.Text) != 0)
                    {
                        drUpdating["ApprQty"] = txt_ReqQty_av.Text;
                    }

                    var txt_Comment_Detail = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Comment_Detail") as TextBox;
                    drUpdating["Comment"] = (txt_Comment_Detail.Text != null ? txt_Comment_Detail.Text : string.Empty);


                    //drUpdating["DiscAmt"] = decimal.Parse(txt_DiscAmt_Grd_Av.Text);
                    //drUpdating["DiscPerCent"] = decimal.Parse(txt_DiscPercent_Grd_Av.Text);


                    //if (decimal.Parse(txt_DiscAmt_Grd_Av.Text) == 0 && decimal.Parse(txt_DiscPercent_Grd_Av.Text) != 0)
                    //{
                    //    drUpdating["DiscAmt"] = (decimal.Parse(drUpdating["Price"].ToString()) * decimal.Parse(drUpdating["DiscPercent"].ToString())) / 100;
                    //}
                    //else if (decimal.Parse(txt_DiscAmt_Grd_Av.Text) > 0 && decimal.Parse(txt_DiscPercent_Grd_Av.Text) > 0)
                    //{
                    //    drUpdating["DiscAmt"] = ((decimal.Parse(drUpdating["Price"].ToString()) *
                    //                              decimal.Parse(drUpdating["DiscPercent"].ToString())) / 100)
                    //                            * decimal.Parse(drUpdating["ApprQty"].ToString());
                    //}
                    //else
                    //{
                    //    drUpdating["DiscPerCent"] = (decimal.Parse(drUpdating["DiscAmt"].ToString()) * 100) /
                    //                                decimal.Parse(drUpdating["Price"].ToString());
                    //}

                    //decimal DiscAmt;
                    //DiscAmt = (decimal.Parse(drUpdating["Price"].ToString()) * decimal.Parse(drUpdating["DiscPercent"].ToString())) / 100;

                    ////UpdateNetAmount(); 
                    //drUpdating["NetAmt"] = NetAmt(drUpdating["TaxType"].ToString(),
                    //    decimal.Parse(drUpdating["TaxRate"].ToString()),
                    //    decimal.Parse(drUpdating["Price"].ToString()), DiscAmt,
                    //    decimal.Parse(drUpdating["ApprQty"].ToString()));
                    //drUpdating["TaxAmt"] = TaxAmt(drUpdating["TaxType"].ToString(),
                    //    decimal.Parse(drUpdating["TaxRate"].ToString()),
                    //    decimal.Parse(drUpdating["Price"].ToString()), DiscAmt,
                    //    decimal.Parse(drUpdating["ApprQty"].ToString()));
                    //drUpdating["TotalAmt"] = Amount(drUpdating["TaxType"].ToString(),
                    //    decimal.Parse(drUpdating["TaxRate"].ToString()),
                    //    decimal.Parse(drUpdating["Price"].ToString()), DiscAmt,
                    //    decimal.Parse(drUpdating["ApprQty"].ToString()));
                }
                #endregion
                #region Not Allocate Vendor
                else
                {
                    {

                        var ddl_LocationCode =
                            grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_LocationCode") as ASPxComboBox;
                        var LocationCode = ddl_LocationCode.Value.ToString().Split(':');
                        drUpdating["LocationCode"] = LocationCode[0];

                        var ddl_ProductCode =
                            grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;
                        var ProductCode = ddl_ProductCode.Value.ToString().Split(':');
                        drUpdating["ProductCode"] = ProductCode[0];

                        // Modified on: 17/01/2018, By: Fon, For: 
                        //Issue Local
                        //drUpdating["TaxAdj"] = false;
                        if (wfDt.GetAllowCreate(wfId, wfStep, hf_ConnStr.Value))
                        {
                            drUpdating["TaxAdj"] = (product.GetTaxType(ProductCode[0], hf_ConnStr.Value) != "N")
                                ? true : false;
                        }
                        else
                        {
                            drUpdating["TaxAdj"] = false;
                        }
                        // End Modified.

                        var txt_DescEn = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescEn") as TextBox;
                        drUpdating["Descen"] = txt_DescEn.Text;

                        var txt_DescLL = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_DescLL") as TextBox;
                        drUpdating["Descll"] = txt_DescLL.Text;

                        //TextBox txt_Unit = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Unit") as TextBox;
                        var ddl_Unit = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
                        drUpdating["OrderUnit"] = ddl_Unit.Value;

                        drUpdating["TaxType"] = product.GetTaxType(drUpdating["ProductCode"].ToString(),
                             hf_ConnStr.Value);
                        drUpdating["TaxRate"] =
                            product.GetTaxRate(drUpdating["ProductCode"].ToString(), LoginInfo.ConnStr).ToString();

                        //ASPxSpinEdit txt_QtyRequest = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_QtyRequest") as ASPxSpinEdit;
                        var txt_QtyRequest = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_QtyRequest") as ASPxSpinEdit;
                        if (txt_QtyRequest != null && decimal.Parse(txt_QtyRequest.Text) != 0)
                        {
                            drUpdating["ReqQty"] = txt_QtyRequest.Text;
                        }
                        else
                        {
                            pop_AlertQty.ShowOnPageLoad = true;
                            return;
                        }

                        var txt_ApprQty = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ApprQty") as ASPxSpinEdit;

                        if (!string.IsNullOrEmpty(txt_ApprQty.Text) && decimal.Parse(txt_ApprQty.Text) != 0)
                        {
                            drUpdating["ApprQty"] = txt_ApprQty.Text;
                        }

                        if (ddl_BuCode != null)
                        {
                            drUpdating["BuCode"] = (LoginInfo.BuInfo.IsHQ) ? ddl_BuCode.Value.ToString() : LoginInfo.BuInfo.BuCode;
                        }

                        var dte_Date = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("dte_Date") as ASPxDateEdit;
                        if (!string.IsNullOrEmpty(dte_Date.Text))
                        {
                            drUpdating["ReqDate"] = DateTime.Parse(dte_Date.Date.ToString("dd/MM/yyyy"));
                        }

                        var ddl_DeliPoint =
                            grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_DeliPoint") as ASPxComboBox;
                        var DeliPoint = ddl_DeliPoint.Value.ToString().Split(':');
                        if (ddl_DeliPoint.Value != null)
                        {
                            drUpdating["DeliPoint"] = int.Parse(DeliPoint[0]);
                        }

                        var txt_Comment_Detail =
                            grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Comment_Detail") as TextBox;

                        drUpdating["Comment"] = (txt_Comment_Detail.Text != null
                            ? txt_Comment_Detail.Text
                            : string.Empty);

                        drUpdating["NetAmt"] = NetAmt(drUpdating["TaxType"].ToString(),
                            decimal.Parse(drUpdating["TaxRate"].ToString()),
                            decimal.Parse(drUpdating["Price"].ToString()),
                            decimal.Parse(drUpdating["DiscAmt"].ToString()),
                            decimal.Parse(drUpdating["ReqQty"].ToString()));
                        drUpdating["TaxAmt"] = TaxAmt(drUpdating["TaxType"].ToString(),
                            decimal.Parse(drUpdating["TaxRate"].ToString()),
                            decimal.Parse(drUpdating["Price"].ToString()),
                            decimal.Parse(drUpdating["DiscAmt"].ToString()),
                            decimal.Parse(drUpdating["ReqQty"].ToString()));
                        drUpdating["TotalAmt"] = Amount(drUpdating["TaxType"].ToString(),
                            decimal.Parse(drUpdating["TaxRate"].ToString()),
                            decimal.Parse(drUpdating["Price"].ToString()),
                            decimal.Parse(drUpdating["DiscAmt"].ToString()),
                            decimal.Parse(drUpdating["ReqQty"].ToString()));
                    }
                }
                #endregion

                bool isSaveNew = Control_Location(drUpdating["LocationCode"].ToString(), dsPR.Tables[prDt.TableName]);

                if (wfDt.GetAllowCreate(wfId, wfStep, LoginInfo.ConnStr))
                    menu_CmdBar.Items.FindByName("Commit").Visible = true;
                else
                    menu_CmdBar.Items.FindByName("Commit").Visible = false;

                menu_CmdBar.Items.FindByName("Save").Visible = true;
                menu_CmdBar.Items.FindByName("Back").Visible = true;

                PRDtEditMode = string.Empty;
                Session["dsPR"] = dsPR;


                if (isSaveNew)
                    CreateDetail();
            }
            #endregion

            if (e.CommandName.ToUpper() == "CREATE")
            {
                CreateDetail();
            }
        }

        protected void Img_Create_Issue_Click(object sender, ImageClickEventArgs e)
        {
            CreateDetail();
        }

        protected void Img_Create_Vendor_Click(object sender, ImageClickEventArgs e)
        {
            CreateDetail();
        }

        protected void ddl_Unit_Load(object sender, EventArgs e)
        {
            var ddl_Unit = sender as ASPxComboBox;
            var ProductCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_Unit.NamingContainer).DataItemIndex]["ProductCode"].ToString();
            var buCode = dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_Unit.NamingContainer).DataItemIndex]["BuCode"].ToString();

            if (ProductCode != string.Empty)
            {
                ddl_Unit.DataSource = prodUnit.GetLookUp_OrderUnitByProductCode(ProductCode,
                    bu.GetConnectionString(buCode));
                ddl_Unit.DataBind();
            }
        }

        protected void ddl_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var row = ddl.NamingContainer;

            var ddl_ProductCode = row.FindControl("ddl_ProductCode") as ASPxComboBox;
            var lbl_LastPrice = row.FindControl("lbl_LastPrice") as Label;
            var lbl_LastVendor = row.FindControl("lbl_LastVendor") as Label;




            // Get Product Information
            var items = txt_PrDate.Text.Split('/');
            var yyyy = Convert.ToInt32(items[2]);
            var mm = Convert.ToInt32(items[1]);
            var dd = Convert.ToInt32(items[0]);
            var prDate = new DateTime(yyyy, mm, dd);

            var productCode = ddl_ProductCode.Text.Split(' ').Select(x => x.Trim()).FirstOrDefault();
            var unitCode = ddl.Text.Trim();

            var info = GetProductInfo(productCode, unitCode, prDate);

            lbl_LastPrice.Text = FormatAmt(info.LastPrice);
            lbl_LastVendor.Text = info.LastVendorCode + " : " + info.LastVendorName;
        }

        protected void ddl_Unit_av_Load(object sender, EventArgs e)
        {
            var ddl_Unit_av = sender as ASPxComboBox;
            var buCode =
                dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_Unit_av.NamingContainer).DataItemIndex]["BuCode"]
                    .ToString();
            var ProductCode =
                dsPR.Tables[prDt.TableName].Rows[((GridViewRow)ddl_Unit_av.NamingContainer).DataItemIndex][
                    "ProductCode"].ToString();

            if (ProductCode != string.Empty && buCode != string.Empty)
            {
                ddl_Unit_av.DataSource = prodUnit.GetLookUp_OrderUnitByProductCode(ProductCode,
                    bu.GetConnectionString(buCode));
                ddl_Unit_av.DataBind();
            }
        }

        protected void ddl_Unit_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Unit_av = sender as ASPxComboBox;
            var txt_ReqQty_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            var hf_UnitCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("hf_UnitCode_av") as HiddenField;

            var product = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.EditIndex]["ProductCode"].ToString();
            var unit = string.Empty;
            decimal qty = 0;

            var oldUnit = string.Empty;
            var newUnit = ddl_Unit_av.Value.ToString();

            if (hf_UnitCode_av.Value != string.Empty)
            {
                if (ddl_Unit_av.Value.ToString() != hf_UnitCode_av.Value)
                {
                    oldUnit = hf_UnitCode_av.Value;
                }
            }
            else
            {
                oldUnit = dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.EditIndex]["OrderUnit"].ToString();
            }

            qty = prodUnit.GetQtyAfterChangeUnit(product, newUnit, oldUnit, Convert.ToDecimal(txt_ReqQty_av.Text),
                hf_ConnStr.Value);

            if (qty > 0)
            {
                txt_ReqQty_av.Text = qty.ToString();
            }

            hf_UnitCode_av.Value = ddl_Unit_av.Value.ToString();
        }

        protected void chk_Adj_Grd_Av_CheckedChanged(object sender, EventArgs e)
        {
            var chk_Adj_Grd_Av = sender as ASPxCheckBox;
            var ddl_TaxType_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
            var txt_TaxRate_Grd_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxRate_Grd_av") as TextBox;

            if (!chk_Adj_Grd_Av.Checked) // return to default value
            {
                var ddl_BuCode = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_BuCode") as ASPxComboBox;
                var ddl_ProductCode_av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_ProductCode_av") as ASPxComboBox;

                var connStr = bu.GetConnectionString(ddl_BuCode.Value.ToString());
                var productCode = ddl_ProductCode_av.Value.ToString();
                var dtProduct = bu.DbExecuteQuery("SELECT TOP(1) * FROM [IN].Product WHERE ProductCode = '" + productCode + "'", null, connStr);
                var taxType = dtProduct.Rows.Count > 0 ? dtProduct.Rows[0]["TaxType"].ToString() : "N";
                var taxRate = dtProduct.Rows.Count > 0 ? Convert.ToDecimal(dtProduct.Rows[0]["TaxRate"].ToString()) : 0m;

                ddl_TaxType_Grd_Av.Value = taxType;
                txt_TaxRate_Grd_av.Text = string.Format(DefaultAmtFmt, taxRate);
                CalculationItem_av(grd_PrDt1.EditIndex);
            }

            ddl_TaxType_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
            txt_TaxRate_Grd_av.Enabled = chk_Adj_Grd_Av.Checked;

            //CalculationItem_av(grd_PrDt1.EditIndex);

            // Modified on: 30/08/2017, By: Fon
            //var chk_Adj_Grd_Av = sender as ASPxCheckBox;
            //var txt_TaxAmt_Grd_Av = grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxAmt_Grd_Av") as TextBox;
            //TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");
            //TextBox txt_TaxRate_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxRate_Grd_av");
            //ASPxComboBox ddl_TaxType_Grd_Av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_TaxType_Grd_Av");

            //txt_TaxAmt_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
            //txt_CurrTaxAmt_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
            //txt_TaxRate_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
            //ddl_TaxType_Grd_Av.Enabled = chk_Adj_Grd_Av.Checked;
            //if (!chk_Adj_Grd_Av.Checked)
            //{
            //}
            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);


        }

        protected void btn_ConfirmDelete_Grd_Click(object sender, EventArgs e)
        {
        }

        protected void btn_CancelDelete_Grd_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete_Grd.ShowOnPageLoad = false;
            //Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
        }

        // Req Quantiy > 0
        protected void btn_WarningQty_Click(object sender, EventArgs e)
        {
            pop_AlertQty.ShowOnPageLoad = false;
        }

        // DiscPercent > 100
        protected void btn_WarningDisc_Click(object sender, EventArgs e)
        {
            pop_AlertDisc.ShowOnPageLoad = false;
        }

        // Disc. Amount > Price
        protected void btn_WarningDiscAmt_Click(object sender, EventArgs e)
        {
            pop_AlertDiscAmt.ShowOnPageLoad = false;
        }

        protected void ddl_TaxType_Grd_Av_SelectedIndexChanged(object sender, EventArgs e)
        {
            ASPxComboBox ddl_TaxType_Grd_Av = (ASPxComboBox)sender;
            TextBox txt_TaxRate_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxRate_Grd_Av");
            TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");
            TextBox txt_TaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_TaxAmt_Grd_Av");

            if (txt_TaxRate_Grd_Av != null)
            {
                txt_TaxRate_Grd_Av.Enabled = true;
                txt_CurrTaxAmt_Grd_Av.Enabled = true;
                txt_TaxAmt_Grd_Av.Enabled = true;
            }

            if (ddl_TaxType_Grd_Av.SelectedItem.Value.ToString() == "N")
            {
                if (txt_TaxRate_Grd_Av != null)
                {
                    txt_TaxRate_Grd_Av.Text = string.Format("{0:N}", 0);
                    txt_TaxRate_Grd_Av.Enabled = false;
                    txt_CurrTaxAmt_Grd_Av.Enabled = false;
                    txt_TaxAmt_Grd_Av.Enabled = false;
                }
            }

            CalculationItem_av(grd_PrDt1.EditIndex);
            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        // Check date for period.
        protected void txt_PrDate_TextChanged(object sender, EventArgs e)
        {
            // เช็คว่า Period นั้นถูกปิดแล้วหรือยัง
            if (grd_PrDt1.Rows.Count == 0)
            {
                //if (!period.GetIsValidDate(DateTime.Parse(txt_PrDate.Text), string.Empty, hf_ConnStr.Value))
                if (DateTime.Parse(txt_PrDate.Text) < period.GetLatestOpenStartDate(hf_ConnStr.Value))
                {
                    lbl_WarningPeriod.Text = "Date is in closed period.";
                    pop_WarningPeriod.ShowOnPageLoad = true;
                    //txt_PrDate.Text = ServerDateTime.ToShortDateString();
                    return;
                }
            }
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }

        protected void ddl_JobCode_Validation(object sender, ValidationEventArgs e)
        {
            //var jc = sender as ASPxComboBox;
            //var s = jc.Text.Split(':')[0].Trim();
            //var ds = new JobCodeLookup().GetRecord(s, LoginInfo.ConnStr);

            //var result = ds.Tables[0].Rows.Count > 0;
            //e.IsValid = result;
            //e.ErrorText = "JobCode not found.";
            //if (!result) e.Value = "";
        }

        protected void ddl_JobCode_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var ddl_JobCode = source as ASPxComboBox;

            string sqlText = string.Empty;

            sqlText = "SELECT Code, [Description]";
            sqlText += " FROM (";
            sqlText += "        SELECT Code, [Description], ROW_NUMBER() OVER(order By Code) as rn";
            sqlText += "        FROM [IMPORT].JobCode";
            sqlText += "        WHERE Code + ' : '+ [Description] LIKE @filter";
            sqlText += "          AND IsActive = 1";
            sqlText += "     ) as st";
            sqlText += " WHERE st.rn BETWEEN @startIndex AND @endIndex";

            SqlDataSource1.ConnectionString = hf_ConnStr.Value;
            SqlDataSource1.SelectCommand = @sqlText;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());

            ddl_JobCode.DataSource = SqlDataSource1;
            ddl_JobCode.DataBind();

            if (ddl_JobCode.Items.Count == 1)
                ddl_JobCode.SelectedIndex = 0;

            ddl_JobCode.ToolTip = ddl_JobCode.Text;

        }


        protected void ddl_JobCode_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
                return;
            var ComboBox = (ASPxComboBox)source;
            try
            {

                SqlDataSource1.ConnectionString = hf_ConnStr.Value;

                SqlDataSource1.SelectCommand = @"SELECT * FROM [IMPORT].[JobCode] WHERE Code = @Code";
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("Code", TypeCode.String, e.Value.ToString());

                ComboBox.DataSource = SqlDataSource1;
                ComboBox.DataBind();
                ComboBox.ToolTip = ComboBox.Text;
            }
            catch
            {
                ComboBox.ToolTip = "Found Exception.";
            }
        }


        #region ddl_Vendor_av


        protected void ddl_Vendor_av_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var dt = bu.DbExecuteQuery("SELECT VendorCode, [Name], [Description] AS Branch FROM AP.Vendor WHERE IsActive = 1 ORDER BY VendorCode", null, hf_ConnStr.Value);

            ddl.DataSource = dt;
            ddl.ValueField = "VendorCode";
            ddl.DataBind();


        }


        protected void ddl_Vendor_av_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var comboBox = (ASPxComboBox)source;

            SqlDataSource1.ConnectionString = LoginInfo.ConnStr;
            SqlDataSource1.SelectCommand =
@"SELECT 
    VendorCode, 
    Name, 
    [Description] AS Branch
FROM 
    (
        SELECT 
            VendorCode, 
            Name, 
            [Description], 
            ROW_NUMBER() OVER(ORDER BY VendorCode) as rn 
        FROM 
            [AP].[Vendor] 
        WHERE 
            IsActive = 1
            AND (LTRIM(RTRIM(VendorCode)) LIKE @filter OR [Name] LIKE @filter)
    ) as st
WHERE 
    st.[rn] BETWEEN @startIndex AND @endIndex";

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter.Trim()));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());

            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;

        }

        protected void ddl_Vendor_av_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
                return;

            var comboBox = (ASPxComboBox)source;

            SqlDataSource1.ConnectionString = LoginInfo.ConnStr;
            SqlDataSource1.SelectCommand = @"SELECT VendorCode, Name , [Description] AS Branch FROM [AP].[Vendor] WHERE LTRIM(RTRIM(VendorCode)) = @VendorCode ";
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString().Trim());

            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;


        }

        #endregion

        protected void btn_Pop_AlertBox_Ok_Click(object sender, EventArgs e)
        {
            lbl_Pop_AlertBox.Text = string.Empty;
            pop_AlertBox.ShowOnPageLoad = false;
        }

        protected void btn_PopupAlert_Click(object sender, EventArgs e)
        {
            if (lbl_hide_action.Text == "Redirect".ToUpper())
            {
                if (Convert.ToBoolean(lbl_hide_value.Text))
                {
                    string prNo = lbl_PrNo.Text;
                    Response.Redirect("Pr.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + prNo + "&VID=" + Request.Params["VID"]);
                }
                else
                {
                    Page_Retrieve();
                }
            }

            lbl_PrNo.Text = string.Empty;
            lbl_hide_action.Text = string.Empty;
            lbl_hide_value.Text = string.Empty;
            pop_Alert.HeaderText = string.Empty;
            pop_Alert.ShowOnPageLoad = false;
        }

        #region Email Process
        //Added on: 2017/05/25, By: Fon
        protected bool Control_SentEmail(string action)
        {
            string errorMessage = SentEmail(action.ToUpper());
            if (errorMessage != string.Empty)
            {
                lbl_PopupAlert.Text = errorMessage;
                pop_Alert.ShowOnPageLoad = true;
                return false;
            }
            else
                return true;
        }

        //protected object[] SentEmail(string action)
        private string SentEmail(string action)
        {
            //Modified on: 2017/05/22, By: Fon
            //object[] obj = new object[2];
            //List<string> result = new List<string>();

            string errorMessage = string.Empty;
            //DataSet dsEmail = Get_LastInboxRecord();
            //dsEmail.Tables[0].TableName = "dtLastInbox";
            //dsEmail.Tables[1].TableName = "dtSMTP";

            //if (dsEmail.Tables["dtLastInbox"].Rows.Count > 0)
            //{
            //    string serverValue = dsEmail.Tables["dtSMTP"].Rows[0]["Value"].ToString();
            //    string de_Value = GnxLib.EnDecryptString(serverValue, GnxLib.EnDeCryptor.DeCrypt);
            //    string[] serverStr = de_Value.Split(';');
            //    List<string> smtpList = new List<string>(serverStr);

            //    string smtpName = string.Empty;
            //    int port = 0;
            //    string from = string.Empty;
            //    string to = string.Empty;
            //    string name = string.Empty;
            //    bool enabledSSL = false;
            //    bool isAuth = true;
            //    string username = string.Empty;
            //    string password = string.Empty;
            //    for (int i = 0; i < smtpList.Count; i++)
            //    {
            //        #region PrepareValue
            //        if (smtpList[i].ToString() != string.Empty)
            //        {
            //            string value = smtpList[i].Split('=')[1].Trim(new char[] { '\'' });
            //            if (smtpList[i].Contains("smtp"))
            //            {
            //                smtpName = value;
            //            }
            //            else if (smtpList[i].Contains("port"))
            //            {
            //                port = Convert.ToInt32(value);
            //            }
            //            else if (smtpList[i].Contains("ssl"))
            //            {
            //                enabledSSL = Convert.ToBoolean(value);
            //            }
            //            else if (smtpList[i].Contains("Name")) // Becareful "Name" & "username"
            //            {
            //                name = value;
            //            }
            //            else if (smtpList[i].Contains("from"))
            //            {
            //                from = value;
            //            }
            //            else if (smtpList[i].Contains("authenticate"))
            //            {
            //                isAuth = Convert.ToBoolean(value);
            //            }
            //            else if (smtpList[i].Contains("username"))
            //            {
            //                username = value;
            //            }
            //            else if (smtpList[i].Contains("password"))
            //            {
            //                password = value;
            //            }
            //        }
            //        #endregion
            //    }

            //    string inboxNo = dsEmail.Tables["dtLastInbox"].Rows[0]["InboxNo"].ToString();
            //    string subject = dsEmail.Tables["dtLastInbox"].Rows[0]["Subject"].ToString();
            //    string body = GnxLib.EnDecryptString(dsEmail.Tables["dtLastInbox"].Rows[0]["Message"].ToString(),
            //         GnxLib.EnDeCryptor.DeCrypt);
            //    string toStep = dsEmail.Tables["dtLastInbox"].Rows[0]["StepTo"].ToString();

            //    if (action.ToUpper() == "A")
            //    {
            //        string receiver = dsEmail.Tables["dtLastInbox"].Rows[0]["Reciever"].ToString();

            //         Modified on: 25/01/2018, By: Fon, For: Short Time
            //        to = receiver;
            //        to = Get_InvolvedLoginFromApprovals(Convert.ToInt32(toStep));
            //         End Modified.
            //    }

            //    if ((!to.Contains("@") && !to.Contains(".com"))
            //       || to.Contains("demo@blueledgers.com"))
            //    {
            //    to = ConvertLoginName_ToEmail(to);

            //    }
            //    if (to != string.Empty)
            //    {
            //        Mail email = new Mail();
            //        email.SmtpServer = smtpName;
            //        if (isAuth)
            //        {
            //            email.Name = name;
            //            email.UserName = username;
            //            email.Password = password;
            //        }
            //        else
            //        {
            //            email.From = from;
            //        }

            //        email.Port = port;
            //        email.EnableSsl = enabledSSL;
            //        email.IsAuthentication = isAuth;
            //        email.To = to;
            //        email.Subject = subject;
            //        email.Body = body;

            //        errorMessage = email.Send();

            //        if (errorMessage != string.Empty)
            //            result.Add("InboxNo: " + inboxNo + ", To: " + to + ", " + errorMessage);
            //            errorMessage = "InboxNo: " + inboxNo + ", To: " + to + ", " + errorMessage;

            //        if (result.Count > 0)
            //        {
            //            //obj[0] = false;
            //            //obj[1] = result;
            //            errorMessage = 
            //        }
            //        else
            //        {
            //            obj[0] = true;
            //            obj[1] = string.Empty;
            //        }
            //    }
            //    else
            //    {
            //        obj[0] = false;
            //        obj[1] = "Process successfully but email does not sent beacuase no recipient's email found.";
            //        errorMessage = "Process successfully. <br /> But email is not sent beacuase no recipient's email found.";
            //    }

            //    return obj;
            //}
            //else
            //{
            //    obj[0] = true;
            //    obj[1] = "Data Empty";
            //    return obj;
            //    errorMessage = "Data empty";
            //}

            return errorMessage;
        }

        protected DataSet Get_LastInboxRecord()
        {
            string cmdStr = string.Format("DECLARE @Email nvarchar(50)");
            cmdStr += string.Format(" SET @Email = (SELECT Email FROM [ADMIN].[vUser] WHERE [LoginName] = '{0}')", LoginInfo.LoginName);
            cmdStr += string.Format(" ;WITH rs AS");
            cmdStr += string.Format(" (SELECT TOP(1) * FROM [IM].[Inbox]");
            cmdStr += string.Format(" WHERE [RefNo] = '{0}'", lbl_PrNo.Text);
            cmdStr += string.Format(" AND [Sender] = ");
            cmdStr += string.Format(" CASE ");
            cmdStr += string.Format("   WHEN ([Sender] = 'demo@blueledgers.com') THEN '{0}'", LoginInfo.LoginName);
            cmdStr += string.Format("   WHEN ([Sender] LIKE '%@%' + '%.com%') THEN @Email");
            cmdStr += string.Format("   ELSE '{0}'", LoginInfo.LoginName);
            cmdStr += string.Format(" END ");
            cmdStr += string.Format(" ORDER BY [InboxNo] DESC )");
            cmdStr += string.Format(" SELECT * FROM rs");

            cmdStr += string.Format(" SELECT [Value] FROM [APP].[Config]");
            cmdStr += string.Format(" WHERE [SubModule] = '{0}'", "Mail");
            cmdStr += string.Format(" AND [Key] = '{0}';", "ServerString");

            DataSet dsInbox = new DataSet();
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            con.Open();

            SqlCommand cmd = new SqlCommand(cmdStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsInbox);
            con.Close();

            return dsInbox;
        }

        protected string ConvertLoginName_ToEmail(string strLogin)
        {
            string strEmail = string.Empty;
            if (strLogin != null)
            {

                List<string> login = strLogin.Split(';').ToList();
                SqlConnection con = new SqlConnection(LoginInfo.ConnStr);

                for (int i = 0; i < login.Count; i++)
                {
                    if (login[i] != string.Empty)
                    {
                        string strCmd = "SELECT [Email] FROM [ADMIN].[vUser]";
                        strCmd += string.Format(" WHERE [LoginName] = '{0}'", login[i]);

                        con.Open();
                        SqlCommand cmd = new SqlCommand(strCmd, con);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        con.Close();

                        if (dt.Rows.Count > 0)
                            strEmail += dt.Rows[0][0].ToString() + ";";
                    }
                }
            }

            return strEmail;
        }

        //        protected string Get_InvolvedLoginFromApprovals(int toStep)
        //        {
        //            string prNo = Request.Params["ID"];
        //            #region
        //            string sql = string.Format(

        //@"DECLARE @list TABLE ( [LoginName] NVARCHAR(100), [Email] NVARCHAR(MAX) )
        //DECLARE @approvals NVARCHAR(MAX) = 
        //	( SELECT [Approvals] FROM [APP].[WFDt] 
        //      WHERE [WFId] = @wfId AND [Step] = @wfStep )
        //               
        //
        //IF ISNULL((SELECT IsHOD FROM [APP].[WFDt] WHERE [WFId] = @wfId AND [Step] = @wfStep), 0) = 1
        //BEGIN
        //	DECLARE @CreateBy NVARCHAR(20) = (SELECT CreatedBy FROM PC.PR WHERE PrNo = '" + prNo + @"');
        //	DECLARE @DeptCode NVARCHAR(20) = (SELECT DepartmentCode FROM [ADMIN].vUser WHERE LoginName = @CreateBy)
        //	DECLARE @HOD nvarchar(20) = (SELECT TOP(1) LoginName FROM [ADMIN].vHeadOfDepartment WHERE DepCode = @DeptCode)
        //	DECLARE @To nvarchar(200) = (SELECT email FROM [ADMIN].vUser WHERE LoginName = @HOD)
        //	INSERT INTO @list ( [LoginName], Email )
        //	VALUES (@HOD, @To)
        //	
        //END
        //ELSE
        //BEGIN
        //                WHILE LEN(@approvals) > 0
        //                BEGIN
        //	                DECLARE @subAppr NVARCHAR(100) = SUBSTRING(@approvals, 1, CHARINDEX(',', @approvals) - 1)
        //	                IF(@subAppr LIKE '#%')
        //	                BEGIN
        //		                INSERT INTO @list ( [LoginName], Email )
        //		                SELECT vU.[LoginName], vU.[Email]
        //		                FROM [ADMIN].[vUser] AS vU 
        //		                LEFT JOIN @list AS l ON l.LoginName = vU.[LoginName]
        //		                WHERE vU.[LoginName] = REPLACE(@subAppr, '#', '') AND l.LoginName IS NULL 
        //	                END
        //	                ELSE
        //	                BEGIN
        //		                INSERT INTO @list ( [LoginName], Email )
        //		                SELECT ur.[LoginName], vU.[Email]
        //		                FROM [ADMIN].[UserRole] AS ur
        //		                LEFT JOIN @list AS l ON l.[LoginName] = ur.[LoginName]
        //		                JOIN [ADMIN].[vUser] AS vU ON vU.[LoginName] = ur.[LoginName]
        //		                WHERE ur.[RoleName] = @subAppr AND ur.[IsActive] = 1 AND l.LoginName IS NULL
        //	                END
        //	
        //	                SET @approvals = STUFF(@approvals, 1, LEN(@subAppr)+1, '')
        //                END
        //
        //END
        //SELECT * FROM @list
        //
        //");
        //            #endregion
        //            string login = string.Empty;
        //            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
        //            DataTable dt = new DataTable();
        //            try
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand(sql, con);
        //                cmd.Parameters.AddWithValue("@wfId", wfId);
        //                cmd.Parameters.AddWithValue("@wfStep", toStep);

        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dt);
        //            }
        //            catch (Exception ex)
        //            {
        //                return null;
        //            }

        //            con.Close();

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                login += dr["LoginName"].ToString() + ";";
        //            }
        //            return login;
        //        }

        #endregion

        #region Currency

        // Added on: 07/08/2017, By: Fon.
        // Note: About tax, disc, discAmt are open on allocatevendor.
        #region Currency Field
        protected void ddl_CurrCode_Load(object sender, EventArgs e)
        {
            var ddl_CurrCode = (ASPxComboBox)sender;

            ddl_CurrCode.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_CurrCode.DataBind();
        }

        protected void ddl_CurrCode_av_OnInit(object sender, EventArgs e)
        {
            var ddl_CurrCode_av = (ASPxComboBox)sender;
            ddl_CurrCode_av.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            ddl_CurrCode_av.DataBind();
        }

        protected void ddl_CurrCode_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ASPxComboBox ddl_CurrCode_av = (ASPxComboBox)sender;
            //GridViewRow gvr = (GridViewRow)ddl_CurrCode_av.NamingContainer;
            //ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)gvr.FindControl("ddl_CurrRate_av");
            //ddl_CurrRate_av.DataSource = Get_CurrencyHistory(ddl_CurrCode_av.Value.ToString());
            //ddl_CurrRate_av.DataBind();
            //ddl_CurrRate_av.Value = currency.GetLastCurrencyRate(ddl_CurrCode_av.Value.ToString(), Convert.ToDateTime(txt_PrDate.Text), LoginInfo.ConnStr);

            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
            var gvr = (sender as ASPxComboBox).NamingContainer as GridViewRow;
            var ddl_CurrRate_av = gvr.FindControl("ddl_CurrRate_av") as ASPxComboBox;
            ddl_CurrRate_av.DataSource = currency.GetLastCurrencyRate((sender as ASPxComboBox).Value.ToString(), Convert.ToDateTime(txt_PrDate.Text), hf_ConnStr.Value);
        }

        protected void ddl_CurrRate_av_Load(object sender, EventArgs e)
        {
            //var gvr = (sender as ASPxComboBox).NamingContainer as GridViewRow;
            //var ddl_CurrCode_av = gvr.FindControl("ddl_CurrCode_av") as ASPxComboBox;
            //if (ddl_CurrCode_av != null && ddl_CurrCode_av.SelectedIndex > -1)
            //{
            //    var ddl_CurrRate_av = gvr.FindControl("ddl_CurrRate_av") as ASPxComboBox;
            //    ddl_CurrRate_av.DataSource = currency.GetLastCurrencyRate(ddl_CurrCode_av.Value.ToString(), Convert.ToDateTime(txt_PrDate.Text), hf_ConnStr.Value);
            //}
        }

        protected void ddl_CurrRate_av_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)sender;
            //string rateValue = ddl_CurrRate_av.Value.ToString();

            //GridViewRow gvr = (GridViewRow)ddl_CurrRate_av.NamingContainer;
            //ASPxComboBox ddl_CurrCode_av = (ASPxComboBox)gvr.FindControl("ddl_CurrCode_av");
            //ddl_CurrRate_av.DataSource = Get_CurrencyHistory(ddl_CurrCode_av.Value.ToString());
            //ddl_CurrRate_av.DataBind();
            //ddl_CurrRate_av.Value = rateValue;

            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        protected DataTable Get_CurrencyHistory(string currCode)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format("SELECT [CurrencyRate], CONVERT(VARCHAR(10), [InputDate], 103) AS InputDate");
            sqlStr += string.Format(" FROM [Ref].[CurrencyExchange]");
            sqlStr += string.Format(" WHERE [CurrencyCode] = '{0}'", currCode);
            sqlStr += string.Format(" ORDER BY [InputDate] DESC");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                conn.Close();
                return dt;
            }
            catch
            {
                conn.Close();
            }

            return dt;
        }


        #endregion

        protected void txt_DiscPercent_grd_av_TextChanged(object sender, EventArgs e)
        {
            TextBox txt_DiscPercent_grd_av = (TextBox)sender;
            TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrDiscAmt_Grd_Av");
            ASPxSpinEdit txt_Price_Av = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Price_Av");
            ASPxSpinEdit txt_ApprQty_av = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ApprQty_av");

            // Follow from old method at grd_Prdt1_updating
            decimal price = 0, qty = 0, discRate = 0, currDiscAmt = 0;
            decimal.TryParse(txt_DiscPercent_grd_av.Text, out discRate);
            decimal.TryParse(txt_Price_Av.Text, out price);
            decimal.TryParse(txt_ApprQty_av.Text, out qty);
            if (qty == 0) decimal.TryParse(dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.EditIndex]["ApprQty"].ToString(), out qty);
            if (txt_DiscPercent_grd_av.Text == string.Empty) txt_DiscPercent_grd_av.Text = string.Format("{0:N}", 0);
            currDiscAmt = ((price * qty) * discRate) / 100;
            txt_CurrDiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, currDiscAmt);

            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
            CalculationItem_av(grd_PrDt1.EditIndex);

        }

        protected void txt_DiscAmt_Grd_av_TextChanged(object sender, EventArgs e)
        {
            TextBox txt_DiscAmt_Grd_av = (TextBox)sender;
            TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrDiscAmt_Grd_Av");
            ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("ddl_CurrRate_av");

            if (txt_DiscAmt_Grd_av.Text == string.Empty)
            {
                txt_DiscAmt_Grd_av.Text = string.Format("{0:N}", 0);
            }

            decimal currRate = 0;
            decimal.TryParse(ddl_CurrRate_av.Text, out currRate);
            decimal discAmt = decimal.Parse(txt_DiscAmt_Grd_av.Text);
            txt_CurrDiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, discAmt / currRate);

            //CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
            CalculationItem_av(grd_PrDt1.EditIndex);
        }

        protected void txt_CurrDiscAmt_Grd_Av_TextChanged(object sender, EventArgs e)
        {
            TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)sender;
            TextBox txt_CurrNetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrNetAmt_Grd_Av");

            if (txt_CurrDiscAmt_Grd_Av.Text == string.Empty) txt_CurrDiscAmt_Grd_Av.Text = string.Format("{0:N}", 0);
            CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        protected void txt_CurrTaxAmt_Grd_Av_TextChanged(object sender, EventArgs e)
        {
            CostContent_ValueChanged(grd_PrDt1.EditIndex, true);
        }

        protected void txt_TaxAmt_Grd_Av_TextChanged(object sender, EventArgs e)
        {
            TextBox txt_TaxAmt_Grd_Av = (TextBox)sender;
            TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");

            if (txt_TaxAmt_Grd_Av.Text == string.Empty) txt_TaxAmt_Grd_Av.Text = string.Format("{0:N}", 0);
        }

        protected decimal Get_TaxRate(string taxType, decimal taxAmt, decimal price, decimal discPerUnit, decimal qty)
        {
            // Use before Calculate_Cost()
            decimal calAmt = CalAmt(price, discPerUnit, qty);
            decimal taxRate = 0;

            if (taxType.ToUpper() == "I")
                taxRate = taxAmt / (calAmt - taxAmt);

            else
                taxRate = taxAmt / calAmt;

            taxRate = taxRate * 100;
            return taxRate;
        }

        protected decimal Get_Disc_Percent(bool isAllocateVendor, decimal currDiscAmt)
        {
            decimal discRate = 0;
            if (isAllocateVendor)
            {
                // Follow from old method at grd_Prdt1_updating
                ASPxSpinEdit txt_Price_Av = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_Price_Av");
                ASPxSpinEdit txt_ApprQty_av = (ASPxSpinEdit)grd_PrDt1.Rows[grd_PrDt1.EditIndex].FindControl("txt_ApprQty_av");

                decimal price = 0, qty = 0;
                decimal.TryParse(txt_Price_Av.Text, out price);
                decimal.TryParse(txt_ApprQty_av.Text, out qty);
                decimal.TryParse(dsPR.Tables[prDt.TableName].Rows[grd_PrDt1.EditIndex]["ApprQty"].ToString(), out qty);

                if (price * qty > 0)
                    discRate = (currDiscAmt * 100) / (price * qty);
            }
            return discRate;
        }

        protected void CostContent_ValueChanged(int grdEditIndex, bool isAllocateVandor)
        {
            decimal qty = 0, price = 0, price_Qty = 0;
            decimal netAmt = 0, discAmt = 0, taxAmt = 0, totalAmt = 0;
            decimal currNetAmt = 0, currDiscAmt = 0, currTaxAmt = 0, currTotalAmt = 0;
            decimal discPercent = 0, taxRate = 0;
            decimal currRate = 1;
            string taxType = string.Empty;

            if (isAllocateVandor)
            {
                #region Get value
                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ApprQty_av") != null)
                {
                    ASPxSpinEdit txt_ApprQty_av = (ASPxSpinEdit)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ApprQty_av");
                    decimal.TryParse(txt_ApprQty_av.Text, out qty);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_Price_av") != null)
                {
                    ASPxSpinEdit txt_Price_av = (ASPxSpinEdit)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_Price_av");
                    decimal.TryParse(txt_Price_av.Text, out price);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("ddl_CurrRate_av") != null)
                {
                    ASPxComboBox ddl_CurrRate_av = (ASPxComboBox)grd_PrDt1.Rows[grdEditIndex].FindControl("ddl_CurrRate_av");
                    decimal.TryParse(ddl_CurrRate_av.Text, out currRate);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av") != null)
                {
                    TextBox txt_DiscPercent_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av");
                    decimal.TryParse(txt_DiscPercent_Grd_Av.Text, out discPercent);
                }

                // ddl_CurrRate_av
                if (grd_PrDt1.Rows[grdEditIndex].FindControl("chk_Adj_Grd_Av") != null)
                {
                    ASPxCheckBox chk_Adj_Grd_Av = (ASPxCheckBox)grd_PrDt1.Rows[grdEditIndex].FindControl("chk_Adj_Grd_Av");
                    //if (chk_Adj_Grd_Av.Checked)
                    {
                        ASPxComboBox ddl_TaxType_Grd_Av = (ASPxComboBox)grd_PrDt1.Rows[grdEditIndex].FindControl("ddl_TaxType_Grd_Av");
                        TextBox txt_TaxRate_Grd_av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TaxRate_Grd_av");

                        decimal.TryParse(txt_TaxRate_Grd_av.Text, out taxRate);
                        taxType = ddl_TaxType_Grd_Av.Value.ToString().ToUpper();
                    }
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av");
                    decimal.TryParse(txt_CurrDiscAmt_Grd_Av.Text, out currDiscAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av") != null)
                {
                    TextBox txt_DiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av");
                    discAmt = currDiscAmt * currRate;
                }
                #endregion
                price_Qty = price * qty;

                currNetAmt = NetAmt(taxType, taxRate, price * qty, currDiscAmt, 1);
                currTaxAmt = TaxAmt(taxType, taxRate, price * qty, currDiscAmt, 1);
                currTotalAmt = Amount(taxType, taxRate, price * qty, currDiscAmt, 1);

                //netAmt = NetAmt(taxType, taxRate, (price * currRate) * qty, discAmt, 1);
                //taxAmt = TaxAmt(taxType, taxRate, (price * currRate) * qty, discAmt, 1);
                //totalAmt = Amount(taxType, taxRate, (price * currRate) * qty, discAmt, 1);
                netAmt = currNetAmt * currRate;
                taxAmt = currTaxAmt * currRate;
                totalAmt = currTotalAmt * currRate;

                #region Set value
                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av") != null)
                {
                    TextBox txt_DiscPercent_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av");
                    txt_DiscPercent_Grd_Av.Text = string.Format("{0:N}", Get_Disc_Percent(true, currDiscAmt));
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrNetAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrNetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrNetAmt_Grd_Av");
                    txt_CurrNetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, currNetAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrDiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av");
                    txt_CurrDiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, currDiscAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTaxAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");
                    txt_CurrTaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, currTaxAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTotalAmt_Grd_Av") != null)
                {
                    TextBox txt_CurrTotalAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTotalAmt_Grd_Av");
                    txt_CurrTotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, currTotalAmt);
                }

                // Base Currency
                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_NetAmt_Grd_Av") != null)
                {
                    TextBox txt_NetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_NetAmt_Grd_Av");
                    txt_NetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, netAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av") != null)
                {
                    TextBox txt_DiscAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av");
                    txt_DiscAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, discAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TaxAmt_Grd_Av") != null)
                {
                    TextBox txt_TaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TaxAmt_Grd_Av");
                    txt_TaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, taxAmt);
                }

                if (grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TotalAmt_Grd_Av") != null)
                {
                    TextBox txt_TotalAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TotalAmt_Grd_Av");
                    txt_TotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, totalAmt);
                }
                #endregion
            }
        }



        private void CalculationItem_av(int grdEditIndex, string connStr = null)
        {
            // Controls
            //var txt_ReqQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            //var txt_ApprQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ApprQty_av") as ASPxSpinEdit;
            //var txt_Price_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;
            var ddl_CurrRate_av = grd_PrDt1.Rows[grdEditIndex].FindControl("ddl_CurrRate_av") as ASPxComboBox;
            //var txt_DiscPercent_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av") as TextBox;
            var txt_CurrDiscAmt_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av") as TextBox;
            //var txt_DiscAmt_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av") as TextBox;
            var chk_Adj_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("chk_Adj_Grd_Av") as ASPxCheckBox;
            var ddl_TaxType_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("ddl_TaxType_Grd_Av") as ASPxComboBox;
            var txt_TaxRate_Grd_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TaxRate_Grd_av") as TextBox;

            #region -- Varaible(s)

            //decimal reqQty = txt_ReqQty_av == null ? 0 : Convert.ToDecimal(txt_ReqQty_av.Value);
            //decimal qty = txt_ApprQty_av == null ? 0 : Convert.ToDecimal(txt_ApprQty_av.Value);
            //decimal price = txt_Price_av == null ? 0 : Convert.ToDecimal(txt_Price_av.Value);
            decimal curRate = ddl_CurrRate_av == null ? 0 : Convert.ToDecimal(ddl_CurrRate_av.Value);
            decimal curDiscAmt = txt_CurrDiscAmt_Grd_Av == null ? 0 : Convert.ToDecimal(txt_CurrDiscAmt_Grd_Av.Text);

            bool isAdjTax = chk_Adj_Grd_Av.Checked;
            string taxType = ddl_TaxType_Grd_Av == null ? "N" : ddl_TaxType_Grd_Av.Value.ToString();
            decimal taxRate = txt_TaxRate_Grd_av == null ? 0 : Convert.ToDecimal(txt_TaxRate_Grd_av.Text);

            var amount = GetAmount_av(grdEditIndex, connStr) - curDiscAmt;
            decimal curNetAmt = amount; //RoundAmt(amount, connStr) - curDiscAmt;
            decimal curTaxAmt = 0;

            if (taxRate != 0)
            {
                if (taxType == "A")
                {
                    curTaxAmt = (amount * taxRate / 100);
                }
                else if (taxType == "I")
                {
                    curNetAmt = RoundAmt(amount / ((taxRate / 100) + 1));
                    curTaxAmt = amount - curNetAmt;
                }
            };

            decimal curTotalAmt = curNetAmt + curTaxAmt;

            decimal totalAmt = RoundAmt(curTotalAmt * curRate);
            decimal taxAmt = RoundAmt(curTaxAmt * curRate);
            decimal netAmt = totalAmt - taxAmt;

            #endregion

            // Assign to control(s)

            var txt_CurrNetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrNetAmt_Grd_Av");
            var txt_CurrTaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTaxAmt_Grd_Av");
            var txt_CurrTotalAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrTotalAmt_Grd_Av");
            var txt_NetAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_NetAmt_Grd_Av");
            var txt_TaxAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TaxAmt_Grd_Av");
            var txt_TotalAmt_Grd_Av = (TextBox)grd_PrDt1.Rows[grdEditIndex].FindControl("txt_TotalAmt_Grd_Av");


            txt_CurrNetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, curNetAmt);
            txt_CurrTaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, curTaxAmt);
            txt_CurrTotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, curTotalAmt);

            txt_NetAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, netAmt);
            txt_TaxAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, taxAmt);
            txt_TotalAmt_Grd_Av.Text = string.Format(DefaultAmtFmt, totalAmt);
        }


        private void CalculateDiscount_av(int grdEditIndex, string connStr = null)
        {
            var txt_ReqQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            var txt_ApprQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ApprQty_av") as ASPxSpinEdit;
            var txt_Price_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;

            var txt_DiscPercent_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscPercent_Grd_Av") as TextBox;
            var txt_CurrDiscAmt_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_CurrDiscAmt_Grd_Av") as TextBox;
            var txt_DiscAmt_Grd_Av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_DiscAmt_Grd_Av") as TextBox;

            decimal reqQty = txt_ReqQty_av == null ? 0 : Convert.ToDecimal(txt_ReqQty_av.Value);
            decimal qty = txt_ApprQty_av == null ? 0 : Convert.ToDecimal(txt_ApprQty_av.Value);
            decimal price = txt_Price_av == null ? 0 : Convert.ToDecimal(txt_Price_av.Value);

            qty = qty == 0 ? reqQty : qty;
            var amount = RoundAmt(price * qty, connStr);

        }

        private decimal GetAmount_av(int grdEditIndex, string connStr = "")
        {
            var txt_ReqQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ReqQty_av") as ASPxSpinEdit;
            var txt_ApprQty_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_ApprQty_av") as ASPxSpinEdit;
            var txt_Price_av = grd_PrDt1.Rows[grdEditIndex].FindControl("txt_Price_av") as ASPxSpinEdit;
            decimal reqQty = txt_ReqQty_av == null ? 0 : Convert.ToDecimal(txt_ReqQty_av.Value);
            decimal qty = txt_ApprQty_av == null ? 0 : Convert.ToDecimal(txt_ApprQty_av.Value);
            decimal price = txt_Price_av == null ? 0 : Convert.ToDecimal(txt_Price_av.Value);

            qty = qty == 0 ? reqQty : qty;
            return RoundAmt(price * qty, connStr);
        }

        #endregion

        #region Single-Multi Location
        // Added on: 10/11/2017, By: Fon
        // I will do it when user click save, save new
        protected bool Control_Location(string locationCode, DataTable dt)
        {
            bool productMatchLocate = true;
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            // Single Location.
            if (isSingle)
            {
                int rowIndex = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    //if (!IsProduct_InThisLocation(dr["productCode"].ToString(), locationCode))
                    if (!IsSame_Location(dt, rowIndex, locationCode))
                    {
                        lbl_Warning_ProductLocate.Text = string.Format(@"Some products in list do not exist in this location.
                                <br/> Do you want to change to selected location? ");
                        productMatchLocate = false;
                        pop_Product_Location.ShowOnPageLoad = true;
                        return productMatchLocate;
                    }
                    rowIndex++;
                }
            }

            Session["dsPR"] = dsPR;
            if (productMatchLocate)
            {
                grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
                grd_PrDt1.EditIndex = -1;
                grd_PrDt1.DataBind();
            }
            return productMatchLocate;
        }

        protected bool IsSame_Location(DataTable dt, int rowIndex, string location)
        {
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                string oldLocation = string.Empty;
                if (rowIndex > 0 && rowIndex <= dt.Rows.Count)
                {
                    // Mean last detail.
                    oldLocation = dt.Rows[rowIndex - 1]["LocationCode"].ToString();
                }
                else if (rowIndex == 0 && dt.Rows.Count >= 1)
                {
                    oldLocation = dt.Rows[rowIndex + 1]["LocationCode"].ToString();
                }
                return (oldLocation == location) ? true : false;
            }
        }

        protected bool IsProduct_InThisLocation(string productCode, string locationCode)
        {
            string strSelect = string.Format("[ProductCode] = '{0}'", productCode);
            DataTable dtProdLocate = product.GetLookUp_LocationCode(locationCode, LoginInfo.ConnStr);
            DataRow[] drSelect = dtProdLocate.Select(strSelect);
            return (drSelect.Length > 0) ? true : false;
        }

        protected void Clear_Product(DataTable dt, int rowIndex)
        {
            DataRow dr = dt.Rows[rowIndex];
            dr["ProductCode"] = string.Empty;
            dr["ReqQty"] = DBNull.Value;
            dr["ApprQty"] = DBNull.Value;
            dr["Descen"] = string.Empty;
            dr["Descll"] = string.Empty;
            dr["OrderUnit"] = string.Empty;
            dr["DeliPoint"] = DBNull.Value;

            dr["Price"] = 0.00;
            dr["DiscAmt"] = 0.00;
            dr["TaxAmt"] = 0.00;
            dr["NetAmt"] = 0.00;
            dr["TotalAmt"] = 0.00;

            dr["CurrNetAmt"] = 0.00;
            dr["CurrDiscAmt"] = 0.00;
            dr["CurrTaxAmt"] = 0.00;
            dr["CurrTotalAmt"] = 0.00;
            Session["dsPR"] = dsPR;
        }

        protected void btn_Yes_popPL_Click(object sender, EventArgs e)
        {
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            int editIndex = grd_PrDt1.EditIndex;
            ASPxComboBox ddlLocation = new ASPxComboBox();
            if (grd_PrDt1.Rows[editIndex].FindControl("ddl_LocationCode") != null)
            { ddlLocation = (ASPxComboBox)grd_PrDt1.Rows[editIndex].FindControl("ddl_LocationCode"); }
            else
            { ddlLocation = (ASPxComboBox)grd_PrDt1.Rows[editIndex].FindControl("ddl_LocationCode_av"); }

            DataTable dt = dsPR.Tables[prDt.TableName];
            string locationChanged = ddlLocation.Value.ToString();
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (isSingle)
                {
                    dr["LocationCode"] = locationChanged;
                    if (!IsProduct_InThisLocation(dr["ProductCode"].ToString(), dr["LocationCode"].ToString()))
                    {
                        Clear_Product(dt, rowIndex);
                    }
                }
                rowIndex++;
            }

            Session["dsPR"] = dsPR;
            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();
            pop_Product_Location.ShowOnPageLoad = false;
        }

        protected void btn_No_popPL_Click(object sender, EventArgs e)
        {
            int editIndex = grd_PrDt1.EditIndex;
            bool isSingle = false;
            if (IsSingleLocation != string.Empty)
            { isSingle = Convert.ToBoolean(IsSingleLocation); }

            if (isSingle)
            {
                DataTable dt = dsPR.Tables[prDt.TableName];
                string locate = string.Empty;
                string delipoint = string.Empty;
                if (editIndex > 0 && editIndex <= dt.Rows.Count)
                {
                    // Mean last detail.
                    locate = dt.Rows[editIndex - 1]["LocationCode"].ToString();
                    delipoint = dt.Rows[editIndex - 1]["DeliPoint"].ToString();
                }
                else if (editIndex == 0 && dt.Rows.Count >= 1)
                {
                    locate = dt.Rows[editIndex + 1]["LocationCode"].ToString();
                    delipoint = dt.Rows[editIndex + 1]["DeliPoint"].ToString();
                }

                dt.Rows[editIndex]["LocationCode"] = locate;
                dt.Rows[editIndex]["DeliPoint"] = delipoint;
                if (!IsProduct_InThisLocation(dt.Rows[editIndex]["ProductCode"].ToString()
                        , dt.Rows[editIndex]["LocationCode"].ToString()))
                { Clear_Product(dt, editIndex); }
            }

            grd_PrDt1.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt1.EditIndex = -1;
            grd_PrDt1.DataBind();

            Session["dsPR"] = dsPR;
            pop_Product_Location.ShowOnPageLoad = false;
        }
        #endregion


        protected void ddl_Vendor_av1_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {

            var comboBox = (ASPxComboBox)source;
            SqlDataSource1.ConnectionString = hf_ConnStr.Value.ToString();
            string sqlText = string.Empty;

            sqlText = "SELECT VendorCode, Name";
            sqlText += " FROM (";
            sqlText += "        SELECT VendorCode, Name, ROW_NUMBER() OVER(order by VendorCode) as rn";
            sqlText += "        FROM [AP].Vendor";
            sqlText += "        WHERE IsActive = 1";
            sqlText += "          AND (ISNULL(VendorCode,'') + ' ' + ISNULL(Name,'') LIKE @filter)";
            sqlText += "     ) as st";
            sqlText += " WHERE st.rn BETWEEN @startIndex AND @endIndex";

            SqlDataSource1.SelectCommand = @sqlText;

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();

            comboBox.ToolTip = comboBox.Text;
        }

        protected void ddl_Vendor_av1_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null || e.Value == string.Empty)
                return;

            ASPxComboBox comboBox = (ASPxComboBox)source;

            SqlDataSource1.ConnectionString = hf_ConnStr.Value.ToString();

            string sqlText = "SELECT VendorCode, Name";
            sqlText += " FROM [AP].Vendor";
            sqlText += " WHERE IsActive = 1";
            sqlText += " AND VendorCode = @VendorCode";
            sqlText += " ORDER BY VendorCode";

            SqlDataSource1.SelectCommand = @sqlText;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSource1;
            comboBox.DataBind();
            comboBox.ToolTip = comboBox.Text;


        }

        private void CheckUserRequired()
        {
            var connStr = bu.GetConnectionString(Request.Params["BuCode"]);
            if (!string.IsNullOrEmpty(connStr))
            {
                var sql = string.Format("SELECT TOP(1) DepartmentCode FROM [ADMIN].vUser WHERE LoginName = '{0}'", LoginInfo.LoginName);
                var dt = pr.DbExecuteQuery(sql, null, connStr);

                if (dt != null && dt.Rows[0][0].ToString() == "")
                {
                    lbl_UserRequired.Text = "Department is required. Please assign Department to this user.";
                    pop_UserRequired.ShowOnPageLoad = true;
                    return;
                }

                HOD = dt.Rows[0][0].ToString();

                var sql1 = string.Format("SELECT COUNT(*) as RN FROM [ADMIN].UserRole ur JOIN [IN].RoleType rt ON ur.RoleName = rt.RoleName WHERE ur.IsActive = 1 AND LoginName = '{0}'", LoginInfo.LoginName);
                var dt1 = pr.DbExecuteQuery(sql1, null, connStr);

                if (dt1 != null && Convert.ToInt32(dt1.Rows[0][0].ToString()) == 0)
                {
                    lbl_UserRequired.Text = "PR Type must be assigned to your role(s). Please assign PR Type to your role(s)";
                    pop_UserRequired.ShowOnPageLoad = true;
                    return;
                }
            }

        }

        protected void btn_OK_UserRequired_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PC/PR/PrList.aspx");
        }

        protected string FormatQty(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultQtyDigit));
        }

        protected string FormatAmt(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultAmtDigit));
        }

        private ProductInfo GetProductInfo(string productCode, string unitCode, DateTime date)
        {
            var info = new ProductInfo();

            info.ProductCode = productCode;
            info.UnitCode = unitCode;

            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(unitCode))
                return info;


            var query = @"
SELECT 
	TOP(1)
	RecDate,
	rec.RecNo,
	rec.VendorCode,
	v.[Name] as VendorName,
	Price
FROM 
	PC.REC
	JOIN PC.RECDt ON rec.RecNo=recdt.RecNo
	LEFT JOIN AP.Vendor v ON v.VendorCode=rec.VendorCode
WHERE
	rec.DocStatus = 'Committed'
	AND RecDate <= @Date
	AND ProductCode = @ProductCode
	AND RcvUnit =@UnitCode
ORDER BY
	RecDate DESC,
	RecNo DESC
";
            var parameters = new Blue.DAL.DbParameter[]
            {
                new Blue.DAL.DbParameter("@Date", date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                new Blue.DAL.DbParameter("@ProductCode", productCode),
                new Blue.DAL.DbParameter("@UnitCode", unitCode)
            };
            var dt = prDt.DbExecuteQuery(query, parameters, hf_ConnStr.Value);

            if (dt != null && dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];

                info.LastRecDate = Convert.ToDateTime(dr["RecDate"]);
                info.LastRecNo = dr["RecNo"].ToString();
                info.LastVendorCode = dr["VendorCode"].ToString();
                info.LastVendorName = dr["VendorName"].ToString();
                info.LastPrice = Convert.ToDecimal(dr["Price"]);
            }


            return info;
        }

        public class ProductInfo
        {
            public ProductInfo()
            {
                LastPrice = 0;
            }

            public string ProductCode { get; set; }
            public string UnitCode { get; set; }
            public DateTime LastRecDate { get; set; }
            public string LastRecNo { get; set; }
            public string LastVendorCode { get; set; }
            public string LastVendorName { get; set; }
            public decimal LastPrice { get; set; }
        }

        public class TaxCalculation
        {
            private decimal taxAmt { get; set; }
            private decimal netAmt { get; set; }
            private decimal totalAmt { get; set; }

            public TaxCalculation(decimal qty, decimal price, string taxType, decimal taxRate, int digitAmt = 2)
            {
                Qty = qty;
                Price = price;
                TaxType = taxType;
                TaxRate = taxRate;

                var amount = Math.Round(qty * price, digitAmt, MidpointRounding.AwayFromZero);

                taxType = taxType.ToLower();

                if (taxType.StartsWith("a"))
                {
                    netAmt = amount;
                    taxAmt = Math.Round(amount * (taxRate / 100), digitAmt, MidpointRounding.AwayFromZero);
                    totalAmt = netAmt + taxAmt;
                }
                else if (taxType.StartsWith("i"))
                {
                    totalAmt = amount;
                    netAmt = Math.Round(amount * (100 / 107), digitAmt, MidpointRounding.AwayFromZero);
                    taxAmt = totalAmt - netAmt;
                }
                else
                {
                    netAmt = amount;
                    taxAmt = 0;
                    totalAmt = amount;
                }



            }

            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public string TaxType { get; set; }
            public decimal TaxRate { get; set; }

            public decimal TaxAmt { get { return taxAmt; } }
            public decimal NetAmt { get { return netAmt; } }
            public decimal TotalAmt { get { return totalAmt; } }

        }

    }
}