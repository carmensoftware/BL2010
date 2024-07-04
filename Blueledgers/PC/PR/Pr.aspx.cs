using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.PC.PR
{
    public partial class Pr : BasePage
    {
        // Set Command Agrument for Image Button for Expand Grid.
        private int index = 0;
        private decimal Total = 0;

        #region "Attributes"
        private DataSet dsWF = new DataSet();
        private DataSet dsPR = new DataSet();
        private DataSet dsPriceCompare = new DataSet();

        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.ProdCat productCate = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ApprLv apprLv = new Blue.BL.Option.Inventory.ApprLv();
        private Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private Blue.BL.dbo.User buyer = new Blue.BL.dbo.User();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private Blue.BL.IN.PriceList priceList = new Blue.BL.IN.PriceList();
        private Blue.BL.Option.Inventory.DeliveryPoint deli = new Blue.BL.Option.Inventory.DeliveryPoint();
        private Blue.BL.Option.Inventory.ProdCateType prodCateType = new Blue.BL.Option.Inventory.ProdCateType();
        private Blue.BL.GnxLib GnxLib = new Blue.BL.GnxLib();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private Blue.BL.APP.WF wf = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();
        private string BuCode = string.Empty;
        private string _module;

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.1";

        [Category("Misc")]
        [Description("Gets or set a module of view list")]
        [Browsable(true)]

        public string Module
        {
            get { return this._module; }
            set { this._module = value; }
        }

        private string _subModule;
        [Category("Misc")]
        [Description("Gets or set a sub module for get the work flow option")]
        [Browsable(true)]
        public string SubModule
        {
            get { return this._subModule; }
            set { this._subModule = value; }
        }

        private bool WorkFlowEnable
        {
            get { return workFlow.GetIsActive("PC", "PR", hf_ConnStr.Value); }
        }

        private int wfId
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFId(int.Parse(Request.Params["VID"].ToString()), hf_ConnStr.Value);
                }
                else
                {
                    return viewHandler.GetWFId(int.Parse(Request.Cookies["[PC].[vPrList]"].Value.ToString()),
                        hf_ConnStr.Value);
                }
            }
        }

        private int wfStep
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFStep(int.Parse(Request.Params["VID"].ToString()), hf_ConnStr.Value);
                }
                else
                {
                    return viewHandler.GetWFStep(int.Parse(Request.Cookies["[PC].[vPrList]"].Value.ToString()),
                        hf_ConnStr.Value);
                }
            }
        }

        // Added By: Fon
        // Added on: 31/08/2017
        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }

        // Modified on: 18/09/2017
        //private string prNo = string.Empty;
        private string prNo
        {
            get
            {
                if (Request.Params["ID"] != null)
                    return Request.Params["ID"].ToString(); ;
                return string.Empty;
            }
        }
        // End Added.

        #endregion

        #region "Page load"

        /// <summary>
        ///     Initial application variable value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_BuGrpCode.Value = LoginInfo.BuInfo.BuGrpCode;
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"].ToString());
        }

        /// <summary>
        ///     Get PR & PRDt data and display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["NoShowMessage_PR_Approve"] != null)
                    chk_Approve_NoShowMessage.Checked = Request.Cookies["NoShowMessage_PR_Approve"].Value.ToString() == "1";

                Page_Retrieve();


            }
            else
            {
                dsWF = (DataSet)Session["dsWF"];
                dsPR = (DataSet)Session["dsPR"];
            }

            // Approve function(s)
            ApproveBar.Visible = wfDt.GetAppr(wfId, wfStep, hf_ConnStr.Value) > 0;
            SetControls();


            ods_SendBack.SelectParameters[0].DefaultValue = wfId.ToString();
            ods_SendBack.SelectParameters[1].DefaultValue = wfStep.ToString();
            ods_SendBack.SelectParameters[2].DefaultValue = prNo;



        }


        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            dsPR.Clear();

            var MsgError = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                //prNo = Request.Params["ID"].ToString();
                BuCode = Request.Params["BuCode"].ToString();

                var getPr = pr.GetListByPrNo(dsPR, ref MsgError, prNo, hf_ConnStr.Value);

                if (!getPr)
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }

                var getPrdt = prDt.GetList(dsPR, prNo, hf_ConnStr.Value);

                if (!getPrdt)
                {
                    return;
                }

                GetAppWFdt();
                Session["dsWF"] = dsWF;
                Session["dsPR"] = dsPR;

                Page_Setting();
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            // --------------------------------------------------------------------------------------------------------
            // Data
            // --------------------------------------------------------------------------------------------------------
            #region "DATA"
            // Display current process description
            if (Request.Cookies["[PC].[vPrList]"] != null)
            {
                if (Request.Cookies["[PC].[vPrList]"].Value != string.Empty)
                {
                    lbl_Process.Text = viewHandler.GetDesc(int.Parse(Request.Params["VID"].ToString()), hf_ConnStr.Value);
                }
            }

            var drPr = dsPR.Tables[pr.TableName].Rows[0];
            //DataRow drPrDt = dsPR.Tables[prDt.TableName].Rows[0];

            lbl_PRNo.Text = drPr["PrNo"].ToString();
            lbl_PRNo.ToolTip = lbl_PRNo.Text;

            lbl_PRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", drPr["PrDate"]);
            lbl_PRDate.ToolTip = lbl_PRDate.Text;

            lbl_Status.Text = drPr["DocStatus"].ToString() + (drPr["DocStatus"].ToString().ToUpper() == "VOIDED" ? "(" + drPr["UpdatedBy"].ToString() + ")" : string.Empty);
            lbl_Status.ToolTip = lbl_Status.Text;

            lbl_Desc.Text = drPr["Description"].ToString();
            lbl_Desc.ToolTip = lbl_Desc.Text;

            lbl_PrType.Text = prodCateType.GetName(drPr["PrType"].ToString(), bu.GetConnectionString(Request.Params["BuCode"].ToString()));
            lbl_PrType.ToolTip = lbl_PrType.Text;

            // Job Code
            //lbl_jobCode.Text = drPr["AddField1"].ToString();
            //lbl_jobCode.ToolTip = lbl_jobCode.Text;
            string jobCode = drPr["AddField1"].ToString();
            var ds = new JobCodeLookup().GetRecord(jobCode, LoginInfo.ConnStr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_jobCode.Text = string.Format("{0} : {1}", ds.Tables[0].Rows[0]["Code"], ds.Tables[0].Rows[0]["Description"]);
                lbl_jobCode.ToolTip = lbl_jobCode.Text;
            }
            else
            {
                lbl_jobCode.Text = "";
            }


            lbl_Requestor.Text = drPr["CreatedBy"].ToString();
            lbl_Requestor.ToolTip = lbl_Requestor.Text;

            lbl_HOD.Text = drPr["HOD"].ToString();
            lbl_HOD.ToolTip = lbl_HOD.Text;

            lbl_HODRE.Text = drPr["HODRE"].ToString();
            lbl_HODRE.ToolTip = lbl_HODRE.Text;

            ProcessStatus.ApprStatus = drPr["ApprStatus"].ToString();


            // WorkFlow information setting -------------------------------------------------------
            pnl_WFLegend.Visible = WorkFlowEnable;
            if (pnl_WFLegend.Visible)
            {
                dl_ProcessStatus.DataSource = wfDt.GetList(wf.GetWFId("PC", "PR", LoginInfo.ConnStr), LoginInfo.ConnStr);
                dl_ProcessStatus.DataBind();
            }

            // WorkFlow Setting
            if (WorkFlowEnable)
            {
                ddl_Buyer.Visible = true;
                ddl_Buyer.BackColor = System.Drawing.Color.Red;
                grd_PRDt1.Columns[2].Visible = !IsExistInField("PC.PrDt.VendorCode", "HideField");
                grd_PRDt1.Columns[3].Visible = !IsExistInField("PC.PrDt.LocationCode", "HideField");
                grd_PRDt1.Columns[4].Visible = !IsExistInField("PC.PrDt.ProductCode", "HideField");
                grd_PRDt1.Columns[5].Visible = !IsExistInField("PC.PrDt.OrderUnit", "HideField");
                grd_PRDt1.Columns[6].Visible = !IsExistInField("PC.PrDt.Price", "HideField");
                grd_PRDt1.Columns[7].Visible = !IsExistInField("PC.PrDt.ReqQty", "HideField");
                grd_PRDt1.Columns[8].Visible = !IsExistInField("PC.PrDt.ApprQty", "HideField");
                grd_PRDt1.Columns[9].Visible = !IsExistInField("PC.PrDt.FOCQty", "HideField");
                grd_PRDt1.Columns[10].Visible = !IsExistInField("PC.PrDt.TotalAmt", "HideField");
                grd_PRDt1.Columns[11].Visible = !IsExistInField("PC.PrDt.ReqDate", "HideField");
                grd_PRDt1.Columns[12].Visible = !IsExistInField("PC.PrDt.DeliPoint", "HideField");
                grd_PRDt1.Columns[13].Visible = !IsExistInField("PC.PrDt.ApprStatus", "HideField");

                grd_PRDt1.DataSource = dsPR.Tables[prDt.TableName];
                grd_PRDt1.DataBind();

                //grd_PRDt2.DataSource = dsPR.Tables[prDt.TableName];
                //grd_PRDt2.DataBind();
            }

            #endregion // DATA

            #region "CONTROLS"
            var dt = dsPR.Tables[prDt.TableName].AsEnumerable();


            TotalSummary.CurrencyNetAmount = dt.Sum(x => x.Field<decimal>("CurrNetAmt"));
            TotalSummary.CurrencyTaxAmount = dt.Sum(x => x.Field<decimal>("CurrTaxAmt"));
            TotalSummary.CurrencyTotalAmount = dt.Sum(x => x.Field<decimal>("CurrTotalAmt"));

            TotalSummary.NetAmount = dt.Sum(x => x.Field<decimal>("NetAmt"));
            TotalSummary.TaxAmount = dt.Sum(x => x.Field<decimal>("TaxAmt"));
            TotalSummary.TotalAmount = dt.Sum(x => x.Field<decimal>("TotalAmt"));

            //TotalSummary.DataBind();
            #endregion // "CONTROLS"

            #region "OTHERS"

            // --------------------------------------------------------------------------------------------------------
            // Others
            // --------------------------------------------------------------------------------------------------------

            // Display Comment.
            var comment = (PL.UserControls.Comment2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Comment");
            comment.Module = "PC";
            comment.SubModule = "PR";
            comment.RefNo = dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString();
            comment.BuCode = Request.Params["BuCode"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"].ToString();
            attach.ModuleName = "PC";
            attach.RefNo = dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Log");
            log.Module = "PC";
            log.SubModule = "PR";
            log.RefNo = dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString();
            log.Visible = true;
            log.DataBind();

            #endregion // "OTHERS"
        }

        // Modified on: 03/10/2017, By: Fon
        //protected void Control_HeaderMenuBar()
        //{
        //    int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
        //    menu_CmdBar.Items.FindByName("Commit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Commit").Visible : false;
        //    menu_CmdBar.Items.FindByName("Edit").Visible = (pagePermiss >= 3) ? menu_CmdBar.Items.FindByName("Edit").Visible : false;
        //    menu_CmdBar.Items.FindByName("Void").Visible = (pagePermiss >= 7) ? menu_CmdBar.Items.FindByName("Void").Visible : false;
        //}
        //// End Modified.

        private void SetControls()
        {
            var drPr = dsPR.Tables[pr.TableName].Rows[0];
            DataSet dsWfDt = new DataSet();
            workFlowDt.Get(dsWfDt, wfId, wfStep, LoginInfo.ConnStr);

            // --------------------------------------------------------------------------------------------------------
            // Controls
            // --------------------------------------------------------------------------------------------------------

            var vid = Request.QueryString["VID"].ToString();
            var page = Request.QueryString["page"]==null ? "1" : Request.QueryString["page"].ToString();

            // Modified on: 05/09/2017, By: Fon
            menu_CmdBar.Items.FindByName("Commit").ToolTip = "Commit";
            menu_CmdBar.Items.FindByName("Edit").ToolTip = "Edit";
            menu_CmdBar.Items.FindByName("Void").ToolTip = "Void";
            menu_CmdBar.Items.FindByName("Print").ToolTip = "Print";
            menu_CmdBar.Items.FindByName("Back").ToolTip = "Back";
            menu_CmdBar.Items.FindByName("Back").NavigateUrl = string.Format("PrList.aspx?VID={0}&page={1}", vid, page);

            bool visibleCommit = false;
            bool visibleEdit = false;
            bool visibleVoid = false;
            bool visiblePrint = true;

            //bool isAllocateVendor = false;
            //try
            //{
            //    isAllocateVendor = Convert.ToBoolean(workFlowDt.Get(wfId, wfStep, LoginInfo.ConnStr).Rows[0]["IsAllocateVendor"].ToString());
            //}
            //catch
            //{
            //}

            //if ((drPr["IsVoid"].ToString() != string.Empty && bool.Parse(drPr["IsVoid"].ToString()))) // Void
            //{
            //    // hide all, show only "Back"
            //    // using default
            //}
            //else if (drPr["DocStatus"].ToString().ToUpper() == "COMPLETE") // Complete
            //{
            //    // hide all, show only "Back"
            //    // using default
            //}
            //else if (drPr["DocStatus"].ToString().ToUpper() == "REJECTED") // Rejected
            //{
            //    // hide all, show only "Back"
            //    // using default
            //}
            if (wfStep > 0)
            {
                if (drPr["ApprStatus"].ToString()[0] == '_')
                {
                    visibleCommit = true;
                    visibleEdit = true;
                    visibleVoid = true;
                }
                else if (wfStep > 1 && drPr["ApprStatus"].ToString()[0] != '_')
                {
                    visibleEdit = true;
                }

                //else if (dsWfDt.Tables[0].Rows[0]["EnableField"].ToString() != string.Empty)
                //{
                //    visibleEdit = true;
                //    visiblePrint = true;
                //}


                btn_Split.Visible = Convert.ToBoolean(workFlowDt.Get(wfId, wfStep, LoginInfo.ConnStr).Rows[0]["IsAllocateVendor"].ToString());
            }


            int pagePermission = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);

            // Check Permission
            switch (pagePermission)
            {
                // Read Only
                case 1:
                    visibleCommit = visibleCommit && false;
                    visibleEdit = visibleEdit && false;
                    visibleVoid = visibleVoid && false;
                    visiblePrint = visiblePrint && true;
                    break;
                // Read + Create/Edit
                case 3:
                    visibleCommit = visibleCommit && true;
                    visibleEdit = visibleEdit && true;
                    visibleVoid = visibleVoid && false;
                    visiblePrint = visiblePrint && true;
                    break;
                // Read + Create/Edit + Delete
                case 7:
                    visibleCommit = visibleCommit && true;
                    visibleEdit = visibleEdit && true;
                    visibleVoid = visibleVoid && true;
                    visiblePrint = visiblePrint && true;
                    break;
            }


            menu_CmdBar.Items.FindByName("Commit").Visible = visibleCommit;
            menu_CmdBar.Items.FindByName("Edit").Visible = visibleEdit;
            menu_CmdBar.Items.FindByName("Void").Visible = visibleVoid;
            menu_CmdBar.Items.FindByName("Print").Visible = visiblePrint;
            menu_CmdBar.Items.FindByName("Back").Visible = true;



        }


        #endregion

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

        #region "Pr Menu Bar"

        /// <summary>
        ///     Menu click event.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            var objArrList = new ArrayList();
            var pageFile = string.Empty;

            switch (e.Item.Name.ToUpper())
            {
                case "COMMIT":
                    // Added on: 06/09/2017, By: Fon.
                    Response.Redirect("PrEdit.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                      "&MODE=EDIT&ID=" + dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString() +
                                      "&VID=" + Request.Params["VID"].ToString() + "&Type=E"
                                      + "&ACTION=" + "C"
                                      );
                    break;

                case "EDIT":
                    // Check PrType of user (loginname)

                    DataTable dt = bu.DbExecuteQuery(string.Format("EXEC [IN].[ProdCateType_GetActivedListByLoginName] '{0}'", LoginInfo.LoginName), null, hf_ConnStr.Value);
                    if (dt.Rows.Count > 0)
                    {


                        Response.Redirect("PrEdit.aspx?BuCode=" + Request.Params["BuCode"].ToString() +
                                          "&MODE=EDIT&ID=" + dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString() +
                                          "&VID=" + Request.Params["VID"].ToString() + "&Type=E");
                    }
                    else
                    {
                        lbl_PopupAlert.Text = "No PR Type is assigned to this user.";
                        pop_Alert.ShowOnPageLoad = true;
                    }
                    break;

                case "VOID":

                    pop_ConfirmVoid.ShowOnPageLoad = true;
                    break;
                case "DETAIL":

                    //objArrList.Add(dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString());
                    //Session["s_arrNo"] = objArrList;


                    //var paramField = new string[1, 2];
                    //paramField[0, 0] = "BU";
                    //paramField[0, 1] = LoginInfo.BuInfo.BuName.ToString();
                    ////paramField[1, 0] = "Pm-?BU";
                    ////paramField[1, 1] = LoginInfo.BuInfo.BuName.ToString();
                    //Session["paramField"] = paramField;

                    //Session.Remove("SubReportName");
                    //Session["SubReportName"] = "prsign";

                    //Blue.BL.APP.Config config = new Blue.BL.APP.Config();
                    //pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseRequestDetailForm";
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    //break;
                    pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseRequestDetailForm";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    break;
                case "SUMMARY":
                    //objArrList.Add(dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString());
                    //Session["s_arrNo"] = objArrList;

                    //Blue.BL.APP.Config config2 = new Blue.BL.APP.Config();
                    //pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseRequestSummaryForm";
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    //break;
                    pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseRequestForm";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    break;

                case "PRINT":
                    //objArrList.Add(dsPR.Tables[pr.TableName].Rows[0]["PrNo"].ToString());
                    //Session["s_arrNo"] = objArrList;

                    //Blue.BL.APP.Config config3= new Blue.BL.APP.Config();
                    pageFile = "../../RPT/PrintForm.aspx?ID=" + Request.Params["ID"] + "&Report=PurchaseRequestForm";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow", "<script>window.open('" + pageFile + "','_blank')</script>");
                    break;

            }
        }

        #endregion

        #region "Pr Detail"

        /// <summary>
        ///     Vendor Changed, Assign RefNo, Price, FOCQty, DiscPercent, DiscAmt, VendorProdCode,
        ///     TaxType, TaxRate and TaxAdj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddl_VendorCode_Callback(object sender, CallbackEventArgsBase e)
        //{

        //}

        /// <summary>
        ///     Assing OrderUnit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddl_ProductCode_Callback(object sender, CallbackEventArgsBase e)
        //{

        //}

        /// <summary>
        ///     Calculate Net, Tax and Total
        /// </summary>
        /// <param name="drPrDtEditRow"></param>
        private void UpdateAllAmount(DataRow drPrDtEditRow)
        {
            decimal Price = 0;
            decimal DiscAmt = 0;

            if (string.IsNullOrEmpty(drPrDtEditRow["TaxType"].ToString()))
            {
                return;
            }

            if (!string.IsNullOrEmpty(drPrDtEditRow["Price"].ToString()))
            {
                Price = decimal.Parse(drPrDtEditRow["Price"].ToString());
            }

            if (!string.IsNullOrEmpty(drPrDtEditRow["DiscAmt"].ToString()))
            {
                DiscAmt = decimal.Parse(drPrDtEditRow["DiscAmt"].ToString());
            }

            // Net, Tax, Total
            var calAmt = (Price - DiscAmt) * decimal.Parse(drPrDtEditRow["ApprQty"].ToString());

            switch (drPrDtEditRow["TaxType"].ToString().ToUpper())
            {
                case "N":
                    drPrDtEditRow["NetAmt"] = calAmt.ToString();
                    drPrDtEditRow["TaxAmt"] = 0;
                    //txt_NetAmt.Text = calAmt.ToString();
                    //txt_TaxAmt.Text = "0";
                    break;

                case "A":
                    drPrDtEditRow["NetAmt"] = calAmt.ToString();
                    drPrDtEditRow["TaxAmt"] = (calAmt * decimal.Parse(drPrDtEditRow["TaxRate"].ToString())) / 100;
                    //txt_NetAmt.Text = calAmt.ToString();
                    //txt_TaxAmt.Text = ((calAmt * decimal.Parse(drPrDtEditRow["TaxRate"].ToString())) / 100).ToString();
                    break;

                case "I":
                    drPrDtEditRow["TaxAmt"] = (calAmt * decimal.Parse(drPrDtEditRow["TaxRate"].ToString())) /
                                              (100 + decimal.Parse(drPrDtEditRow["TaxRate"].ToString()));
                    drPrDtEditRow["NetAmt"] = calAmt - decimal.Parse(drPrDtEditRow["TaxAmt"].ToString());
                    //txt_TaxAmt.Text = ((calAmt * decimal.Parse(drPrDtEditRow["TaxRate"].ToString())) /
                    //    (100 + decimal.Parse(drPrDtEditRow["TaxRate"].ToString()))).ToString();
                    //txt_NetAmt.Text = (calAmt - txt_TaxAmt.Number).ToString(); 
                    break;
            }

            drPrDtEditRow["TotalAmt"] = decimal.Parse(drPrDtEditRow["NetAmt"].ToString()) +
                                        decimal.Parse(drPrDtEditRow["TaxAmt"].ToString());
            //txt_TotalAmt.Text = (txt_NetAmt.Number + txt_TaxAmt.Number).ToString();
        }

        #region "Edit Form"

        /// <summary>
        ///     Prepare Data for Display Price List before Row Expand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        ///     TabPage load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pc_Prdt_Load(object sender, EventArgs e)
        {
            //if (viewHandler.GetWFStep(int.Parse(Request.Cookies["[PC].[vPrList]"].Value.ToString()), hf_ConnStr.Value) < 2)
            //{
            //    pc_Prdt.TabPages[1].Enabled = false; // Hide Allocate Buyer
            //}

            //if (viewHandler.GetWFStep(int.Parse(Request.Cookies["[PC].[vPrList]"].Value.ToString()), hf_ConnStr.Value) < 3)
            //{
            //    pc_Prdt.TabPages[2].Enabled = false; // Hide Allocate Vendor
            //}
        }

        /// <summary>
        ///     Rebinding Vendor List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Vendor_Load(object sender, EventArgs e)
        {

        }

        protected void ddl_Vendor_OnItemsRequestedByFilterCondition_SQL(object source,
            ListEditItemsRequestedByFilterConditionEventArgs e)
        {
        }

        protected void ddl_Vendor_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var ddl_Vendor = (ASPxComboBox)source;

            try
            {
                long value = 0;
                if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                    return;

                SqlDataSource1.SelectCommand =
                    @"SELECT VendorCode,Name FROM [AP].[Vendor] WHERE (VendorCode = @VendorCode) ORDER BY VendorCode";

                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectParameters.Add("VendorCode", TypeCode.String, e.Value.ToString());
                ddl_Vendor.DataSource = SqlDataSource1;
                ddl_Vendor.DataBind();
                ddl_Vendor.ToolTip = ddl_Vendor.Text;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        /// <summary>
        ///     Rebinding Product, Unit and Delivery Point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Store_Pop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Store_Pop.Value == null)
            {
                return;
            }

            // Fill Product related to selected Store
            ods_Product_Pop.SelectParameters[0].DefaultValue = ddl_Store_Pop.Value.ToString();
            ddl_Product_Pop.DataBind();
            ddl_Product_Pop.Value = string.Empty;

            // Clear unit
            ddl_Unit_Pop.Value = string.Empty;

            // Assign Default Delivery Point related to selected store
            ddl_DeliPoint_Pop.Value = storeLct.GetDeliveryPoint(ddl_Store_Pop.Value.ToString(), hf_ConnStr.Value);
        }

        /// <summary>
        ///     Assing OrderUnit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Product_Pop.Value == null)
            {
                return;
            }

            ddl_Unit_Pop.Value = product.GetOrderUnit(ddl_Product_Pop.Value.ToString(), hf_ConnStr.Value);
        }

        /// <summary>
        ///     Change Approve Quantity, update all amount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_ApprQty_NumberChanged(object sender, EventArgs e)
        {
            // Same change vendor
            UpdateAllAmount();
        }

        /// <summary>
        ///     Change Discount Percent, update all amount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_Discount_NumberChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     Change TaxType, update all amount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.UpdateAllAmount();
        }

        /// <summary>
        ///     Change TaxRate, update all amount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_TaxRate_NumberChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     Enabled/Disabled Net, Tax anf Amount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chk_IsAdj_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     Allocate Vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddl_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //[System.Web.Services.WebMethod]
        protected void VenderChange(object sender, CallbackEventArgsBase e)
        {

        }

        /// <summary>
        ///     Update Net, Tax and TotalAmount
        /// </summary>
        private void UpdateAllAmount()
        {
            decimal Price = 0;
            decimal DiscAmt = 0;

            if (ddl_TaxType.Value == null)
            {
                return;
            }

            if (txt_Price.Text != string.Empty)
            {
                Price = decimal.Parse(txt_Price.Text);
            }

            if (txt_DiscountAmt.Text != string.Empty)
            {
                DiscAmt = decimal.Parse(txt_DiscountAmt.Text);
            }

            // Net, Tax, Total
            var calAmt = (Price - DiscAmt) * decimal.Parse(txt_ApprQty.Text);

            switch (ddl_TaxType.Value.ToString().ToUpper())
            {
                case "N":
                    txt_Net.Text = calAmt.ToString();
                    txt_TaxAmt.Text = "0";
                    txt_Amount.Text = (decimal.Parse(txt_Net.Text.ToString()) +
                                       decimal.Parse(txt_TaxAmt.Text.ToString())).ToString();
                    break;

                case "A":
                    txt_Net.Text = calAmt.ToString();
                    txt_TaxAmt.Text = ((calAmt * decimal.Parse(txt_TaxRate.Text)) / 100).ToString();
                    txt_Amount.Text = (decimal.Parse(txt_Net.Text.ToString()) +
                                       decimal.Parse(txt_TaxAmt.Text.ToString())).ToString();
                    break;

                case "I":
                    txt_TaxAmt.Text =
                        ((calAmt * decimal.Parse(txt_TaxRate.Text)) / (100 + decimal.Parse(txt_TaxRate.Text))).ToString();
                    txt_Net.Text = (calAmt - decimal.Parse(txt_TaxAmt.Text)).ToString();
                    txt_Amount.Text = (decimal.Parse(txt_Net.Text.ToString()) +
                                       decimal.Parse(txt_TaxAmt.Text.ToString())).ToString();
                    break;
            }
        }

        /// <summary>
        ///     Save to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Update_Pop_Click(object sender, EventArgs e)
        {
            #region Command: because it look like useless.
            //if (Session["PrDtNoEdit"] != null)
            //{
            //    var PrDtNo = Session["PrDtNoEdit"].ToString();

            //    // Update exist PrDt
            //    foreach (DataRow drUpdating in dsPR.Tables[prDt.TableName].Rows)
            //    {
            //        if (drUpdating.RowState == DataRowState.Deleted)
            //        {
            //            continue;
            //        }

            //        if (drUpdating["PrDtNo"].ToString() == PrDtNo)
            //        {
            //            drUpdating["LocationCode"] = ddl_Store_Pop.Value.ToString();
            //            drUpdating["ProductCode"] = ddl_Product_Pop.Value.ToString();
            //            drUpdating["OrderUnit"] = ddl_Unit_Pop.Value.ToString();
            //            drUpdating["ReqQty"] = txt_ReqQty.Text;

            //            if (!string.IsNullOrEmpty(txt_ReqDate.Text))
            //            {
            //                drUpdating["ReqDate"] = txt_ReqDate.Date;
            //            }

            //            drUpdating["ApprQty"] = txt_ApprQty.Text;
            //            drUpdating["OrderQty"] = txt_OrderQty.Text;
            //            drUpdating["RcvQty"] = txt_RcvQty.Text;

            //            if (ddl_DeliPoint_Pop.Value != null)
            //            {
            //                drUpdating["DeliPoint"] = int.Parse(ddl_DeliPoint_Pop.Value.ToString());
            //            }

            //            drUpdating["Comment"] = txt_Comment.Text.Trim();

            //            // Allocate Buyer
            //            if (pc_Prdt.TabPages[1].Enabled)
            //            {
            //                drUpdating["Buyer"] = ddl_Buyer.Value;
            //            }

            //            if (pc_Prdt.TabPages[2].Enabled)
            //            {
            //                if (ddl_Vendor.Value == null)
            //                {
            //                    pop_ReqVendor.ShowOnPageLoad = true;
            //                    return;
            //                }

            //                // Allocate Vendor                        
            //                drUpdating["VendorCode"] = ddl_Vendor.Value.ToString();
            //                drUpdating["RefNo"] = txt_RefNo.Text;
            //                drUpdating["Price"] = txt_Price.Text;
            //                drUpdating["FOCQty"] = (string.IsNullOrEmpty(txt_FOCQty.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_FOCQty.Text));
            //                drUpdating["DiscPercent"] = (string.IsNullOrEmpty(txt_Discount.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_Discount.Text));
            //                drUpdating["DiscAmt"] = (string.IsNullOrEmpty(txt_DiscountAmt.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_DiscountAmt.Text));
            //                drUpdating["VendorProdCode"] = txt_VendorProdCode.Text;
            //                drUpdating["TaxType"] = ddl_TaxType.Value.ToString();
            //                drUpdating["TaxRate"] = (string.IsNullOrEmpty(txt_TaxRate.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_TaxRate.Text));
            //                drUpdating["PONo"] = txt_PONo.Text;
            //                drUpdating["NetAmt"] = (string.IsNullOrEmpty(txt_Net.Text) ? 0 : decimal.Parse(txt_Net.Text));
            //                drUpdating["TaxAdj"] = chk_IsAdj.Checked;
            //                drUpdating["TaxAmt"] = (string.IsNullOrEmpty(txt_TaxAmt.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_TaxAmt.Text));
            //                drUpdating["TotalAmt"] = (string.IsNullOrEmpty(txt_Amount.Text)
            //                    ? 0
            //                    : decimal.Parse(txt_Amount.Text));
            //            }

            //            if (wfStep <= 3)
            //            {
            //                var result = pr.Save(dsPR, hf_ConnStr.Value);

            //                if (result)
            //                {
            //                    pop_PrDtEdit.ShowOnPageLoad = false;
            //                    Session["PrDtNoEdit"] = null;

            //                    if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]))    //if (wfStep == 3)
            //                    {
            //                        // Update Allocate Vendor ApprStatus
            //                        UpdateAllocateVendorApprStatus(drUpdating["PrNo"].ToString(),
            //                            int.Parse(drUpdating["PrDtNo"].ToString()));
            //                    }

            //                    Page_Retrieve();
            //                }
            //            }
            //            else
            //            {
            //                //grd_PrDt.DataSource = dsPR.Tables[prDt.TableName];
            //                //grd_PrDt.DataBind();

            //                grd_PRDt1.DataSource = dsPR.Tables[prDt.TableName];
            //                grd_PRDt1.DataBind();
            //            }

            //            break;
            //        }
            //    }
            //}
            #endregion
        }

        #endregion

        #endregion

        #region "Work Flow Process"

        /// <summary>
        ///     Auto assign Vendor and other data to selected Pr Items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_AutoAllocateVd_Click(object sender, EventArgs e)
        {
            pop_ConfirmAutoAlloVd.ShowOnPageLoad = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="PRNo"></param>
        /// <param name="PRDtNo"></param>
        public void UpdateAllocateVendorApprStatus(string PRNo, int PRDtNo)
        {
            var dtWFDt = workFlowDt.Get(wfId, wfStep, hf_ConnStr.Value);

            var dbParams = new Blue.DAL.DbParameter[3];
            dbParams[0] = new Blue.DAL.DbParameter("@PrNo", PRNo);
            dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", PRDtNo.ToString());
            dbParams[2] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

            workFlowDt.ExcecuteApprRule(dtWFDt.Rows[0]["ApprRule"].ToString(), dbParams, hf_ConnStr.Value);
        }

        /// <summary>
        ///     Split Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Split_Click(object sender, EventArgs e)
        {
            SplitAndRejectItems();
        }

        /// <summary>
        ///     Approve Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Appr_Click(object sender, EventArgs e)
        {
            string message = ValidateApproval();

            if (message == string.Empty)
                pop_ConfirmApprove.ShowOnPageLoad = true;
            else
            {
                lbl_PopupAlert.Text = message;
                pop_Alert.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        ///     Redirect to ListPage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OK_PopApprClose_Click(object sender, EventArgs e)
        {
            pop_Approve.ShowOnPageLoad = false;
        }

        protected void btn_OK_PopApprFunction_Click(object sender, EventArgs e)
        {
            pop_Approve.ShowOnPageLoad = false;

            Response.Redirect("PrList.aspx"); // Just Commnet For Test on: 19/01/2018
        }

        /// <summary>
        ///     Reject on selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Reject_OK_Click(object sender, EventArgs e)
        {
            pop_ConfirmReject.ShowOnPageLoad = true;
        }

        protected void btn_ConfirmReject_Click(object sender, EventArgs e)
        {
            RejectItems(true);
        }

        private void RejectItems(bool sendMail)
        {
            var rejectItemCount = 0;

            var sbPrDtNo = new StringBuilder();

            for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
            {
                var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                if (Chk_Item.Checked)
                {
                    var drReject = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];

                    if (drReject["ApprStatus"].ToString().Contains('R'))
                    {
                        continue;
                    }

                    var Step = (wfStep - 1) * 10;

                    if (wfStep > 1)
                    {
                        if (!drReject["ApprStatus"].ToString().Substring(Step).StartsWith("_"))
                        {
                            continue;
                        }
                        else
                        {
                            rejectItemCount++;
                        }
                    }

                    sbPrDtNo.Append((sbPrDtNo.Length > 0
                        ? ", " + drReject["PRDtNo"].ToString() + " "
                        : drReject["PRDtNo"].ToString()));

                    var dbParams = new Blue.DAL.DbParameter[5];
                    dbParams[0] = new Blue.DAL.DbParameter("@PrNo", dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString());
                    //dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", sbPrDtNo.ToString());
                    dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", drReject["PrDtNo"].ToString());
                    dbParams[2] = new Blue.DAL.DbParameter("@Step", wfStep.ToString());
                    dbParams[3] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);
                    dbParams[4] = new Blue.DAL.DbParameter("@Comment", txt_RejectMessage.Text.Trim());

                    // Modified on: 10/10/2017, By: Fon
                    //workFlowDt.ExcecuteApprRule("APP.WF_PR_REJECT", dbParams, hf_ConnStr.Value);
                    string spRej = workFlowDt.GetRejRule(wfId, wfStep, LoginInfo.ConnStr);
                    if (spRej != string.Empty)
                        workFlowDt.ExcecuteApprRule(spRej, dbParams, hf_ConnStr.Value);
                    else
                        return;
                    // End Modified.
                }
            }


            pop_Reject.ShowOnPageLoad = false;
            pop_ConfirmReject.ShowOnPageLoad = false;

            // Added on: 2017/05/23, By: Fon
            // Send email

            if (sendMail)
            {

                bool sentComplete = true;
                bool isSentEmail = Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["SentEmail"]);

                if (isSentEmail)
                {
                    lbl_hide_action.Text = "Redirect".ToUpper();
                    lbl_hide_value.Text = rejectItemCount == dsPR.Tables[prDt.TableName].Rows.Count ? "true" : "false";

                    sentComplete = SendEmailWorkflow.Send("R", prNo, wfId, wfStep, LoginInfo.LoginName, hf_ConnStr.Value);
                }

                if (sentComplete)
                {
                    Response.Redirect("PrList.aspx");
                    //// Origin ver.
                    //if (rejectItemCount == dsPR.Tables[prDt.TableName].Rows.Count)
                    //{
                    //    Response.Redirect("PrList.aspx");
                    //}
                    //else
                    //{
                    //    Page_Retrieve();
                    //}
                }
            }
        }

        protected void btn_CancelReject_Click(object sender, EventArgs e)
        {
            pop_ConfirmReject.ShowOnPageLoad = false;
            pop_Reject.ShowOnPageLoad = false;
        }

        /// <summary>
        ///     Send PR Back to selected step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SandBack_OK_Click(object sender, EventArgs e)
        {
            pop_ConfirmSendback.ShowOnPageLoad = true;
            lbl_Sendback.Text = "Send back to " + ddl_SendBack.Text;
        }

        protected void btn_ConfirmAutoAlloVd_Click(object sender, EventArgs e)
        {

        }

        protected void btn_CancelAutoAlloVd_Click(object sender, EventArgs e)
        {
            pop_ConfirmAutoAlloVd.ShowOnPageLoad = false;
            pop_AutoAlloVd.ShowOnPageLoad = false;
        }

        protected void btn_OK_PopAutoAlloVd_Click(object sender, EventArgs e)
        {
            pop_AutoAlloVd.ShowOnPageLoad = false;
            //Response.Redirect("PrList.aspx");
        }

        protected void btn_ConfirmApprove_Click(object sender, EventArgs e)
        {
            ConfirmApprove();
        }

        private void SplitAndRejectItems()
        {
            if (grd_PRDt1.Rows.Count <= 1)
            {
                lbl_PopupAlert.Text = "Not support only one item.";
                pop_Alert.ShowOnPageLoad = true;
            }
            else
            {
                int selectedItems = 0;
                string oldPrNo = string.Empty;
                var prDtNoList = new List<string>();

                for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
                {
                    var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                    if (Chk_Item.Visible && Chk_Item.Checked)
                    {
                        DataRow dr = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];

                        oldPrNo = dr["PrNo"].ToString();
                        prDtNoList.Add(dr["PrDtNo"].ToString());
                        selectedItems++;
                    }
                }


                if (selectedItems > 0)
                {
                    string buConnection = bu.GetConnectionString(Request.Params["BuCode"].ToString());
                    DateTime oldPrDate = Convert.ToDateTime(lbl_PRDate.Text);
                    string newPrNo = pr.GetNewID(oldPrDate, buConnection);
                    string newDesc = lbl_Desc.Text + string.Format(" *splitted from {0}", lbl_PRNo.Text);
                    string sql = string.Empty;

                    // Create Header(PC.Pr)
                    sql = "INSERT INTO PC.Pr (PrNo, PrDate, Location, PrType, DeliPoint, [Description], Discount, Buyer, HOD, HODRE, GrpBuyer,";
                    sql += " ApprStatus, DocStatus, IsVoid, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy,";
                    sql += " AddField1, AddField2, AddField3, AddField4, AddField5, AddField6, AddField7, AddField8, AddField9, AddField10)";

                    sql += string.Format(" SELECT '{0}', PrDate, Location, PrType, DeliPoint, '{1}', Discount, Buyer, HOD, HODRE, GrpBuyer,", newPrNo, newDesc);
                    sql += " ApprStatus, DocStatus, IsVoid, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy,";
                    sql += string.Format(" AddField1, AddField2, AddField3, AddField4, AddField5, AddField6, AddField7, AddField8, AddField9, '{0}'", lbl_PRNo.Text);
                    sql += " FROM PC.Pr";
                    sql += string.Format(" WHERE PrNo = '{0}'", oldPrNo);

                    bu.DbExecuteQuery(@sql, null, buConnection);

                    // Create Detail(s)
                    sql = "INSERT INTO PC.PrDt (PrNo, PrDtNo,  BuCode, ProductCode, LocationCode, ReqQty, OrderUnit, OrderQty,  ApprQty, VendorCode, FOCQty, RcvQty,";
                    sql += " DiscPercent, DiscAmt, TaxAmt, NetAmt, TotalAmt, TaxType, TaxRate, TaxAdj, Price, GrandTotalAmt,";
                    sql += " Buyer, HOD, ReqDate, DeliPoint, Comment,  PoNo, PoDtNo, ApprStatus, PrStatus, RefNo, VendorProdCode, ";
                    sql += " AddField1, AddField2, AddField3, AddField4, DescEn, Descll, LastPrice, GrpBuyer, ";
                    sql += " CurrencyCode, CurrencyRate, CurrNetAmt, CurrDiscAmt, CurrTaxAmt, CurrTotalAmt)";

                    sql += string.Format(" SELECT '{0}', PrDtNo,  BuCode, ProductCode, LocationCode, ReqQty, OrderUnit, OrderQty,  ApprQty, VendorCode, FOCQty, RcvQty,", newPrNo);
                    sql += " DiscPercent, DiscAmt, TaxAmt, NetAmt, TotalAmt, TaxType, TaxRate, TaxAdj, Price, GrandTotalAmt,";
                    sql += " Buyer, HOD, ReqDate, DeliPoint, Comment,  PoNo, PoDtNo, ApprStatus, PrStatus, RefNo, VendorProdCode, ";
                    sql += " AddField1, AddField2, AddField3, AddField4, DescEn, Descll, LastPrice, GrpBuyer, ";
                    sql += " CurrencyCode, CurrencyRate, CurrNetAmt, CurrDiscAmt, CurrTaxAmt, CurrTotalAmt";
                    sql += " FROM PC.PrDt";
                    sql += string.Format(" WHERE PrNo = '{0}'", oldPrNo);
                    sql += string.Format(" AND PrDtNo IN ({0})", string.Join(",", prDtNoList));

                    bu.DbExecuteQuery(sql, null, buConnection);

                    bool isSendMail = false;
                    RejectItems(isSendMail);

                    // Insert into PC.PrWfHis
                    sql = "INSERT INTO PC.PrWfHis (PrNo, Appr1, Appr2, Appr3, Appr4, Appr5, Appr6, Appr7, Appr8, Appr9, Appr10,";
                    sql += " ApprName1, ApprName2, ApprName3, ApprName4, ApprName5, ApprName6, ApprName7, ApprName8, ApprName9, ApprName10,";
                    sql += " ApprPos1, ApprPos2, ApprPos3, ApprPos4, ApprPos5, ApprPos6, ApprPos7, ApprPos8, ApprPos9, ApprPos10,";
                    sql += " ApprDate1, ApprDate2, ApprDate3, ApprDate4, ApprDate5, ApprDate6, ApprDate7, ApprDate8, ApprDate9, ApprDate10)";

                    sql += string.Format(" SELECT '{0}', Appr1, Appr2, Appr3, Appr4, Appr5, Appr6, Appr7, Appr8, Appr9, Appr10,", newPrNo);
                    sql += " ApprName1, ApprName2, ApprName3, ApprName4, ApprName5, ApprName6, ApprName7, ApprName8, ApprName9, ApprName10,";
                    sql += " ApprPos1, ApprPos2, ApprPos3, ApprPos4, ApprPos5, ApprPos6, ApprPos7, ApprPos8, ApprPos9, ApprPos10,";
                    sql += " ApprDate1, ApprDate2, ApprDate3, ApprDate4, ApprDate5, ApprDate6, ApprDate7, ApprDate8, ApprDate9, ApprDate10";
                    sql += " FROM PC.PrWfHis";
                    sql += string.Format(" WHERE PrNo = '{0}'", oldPrNo);

                    bu.DbExecuteQuery(sql, null, buConnection);

                    newDesc = string.Format(" Splitted from {0}", lbl_PRNo.Text);
                    // Insert into App.WfHis
                    sql = "INSERT INTO APP.WFHis (Module, SubModule, RefNo, RefNoDt, Step, Process, ProcessBack, ProcessBy, ProcessDate, Comment)";
                    sql += string.Format(" SELECT Module, SubModule, '{0}', RefNoDt, Step, Process, ProcessBack, ProcessBy, ProcessDate, '{1}'", newPrNo, newDesc);
                    sql += " FROM APP.WFHis";
                    sql += string.Format(" WHERE Module='PC' AND SubModule='PR' AND RefNo = '{0}' AND Step < {1}", oldPrNo, wfStep.ToString());
                    sql += string.Format(" AND RefNoDt IN ({0})", string.Join(",", prDtNoList));

                    bu.DbExecuteQuery(sql, null, buConnection);

                    Response.Redirect(Request.RawUrl);


                }
            }

        }
        private string ValidateApproval() // retrun empty string if valid else return error message.
        {
            int selectedItems = 0;
            int noVendorAssign = 0;

            bool isAllocateVendor = Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]);

            for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
            {
                var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                if (Chk_Item.Visible && Chk_Item.Checked)
                {
                    selectedItems++;

                    DataRow dr = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];

                    bool isRejected = dr["ApprStatus"].ToString().Contains('R');
                    bool isSendBack = false;

                    // if the last step then skip
                    if (workFlow.GetHdrApprStatus("PC", "PR", hf_ConnStr.Value).Length < wfStep)
                    {
                        isSendBack = !dr["ApprStatus"].ToString().Substring(wfStep * 10, 10).Contains('_');
                    }

                    if (isRejected || isSendBack)
                        continue;

                    // Check if allocate vendor but no vendor assign
                    if (isAllocateVendor && dr["VendorCode"].ToString().Trim() == "")
                    {
                        noVendorAssign++;
                        break;
                    }

                }

            }

            if (selectedItems == 0) // No detail is selected
            {
                return "No detail is selected.";
            }
            else if (noVendorAssign > 0)
            {
                return "No vendor is assigned.";
            }
            else
                return string.Empty;
        }

        private void ConfirmApprove()
        {
            var step = (wfStep - 1) * 10;
            var confirmApproveCount = 0;
            var approveCount = 0;
            var noSelectedVendorCount = 0;

            bool isAllocateVendor = Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]);

            if (wfStep > 1)
            {
                if (isAllocateVendor)
                {
                    #region Allocated Vendor Process
                    for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
                    {
                        var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                        //if (Chk_Item.Checked)
                        if (Chk_Item.Checked && Chk_Item.Visible)
                        {
                            var drApprove = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];
                            var apprStatus = drApprove["ApprStatus"].ToString();

                            bool isRejected = apprStatus.Contains('R');
                            bool isSendBack = apprStatus.Length >= wfStep * 10 + 10 ? !apprStatus.Substring(wfStep * 10, 10).Contains('_') : false;

                            confirmApproveCount++;
                            if (isRejected || isSendBack)
                                continue;

                            if (drApprove["VendorCode"].ToString().Trim() == "")
                            {
                                //NotApprove++;
                                noSelectedVendorCount++;
                                continue;
                            }
                            else
                            {
                                approveCount++;
                            }

                            var dbParams = new Blue.DAL.DbParameter[3];
                            dbParams[0] = new Blue.DAL.DbParameter("@PrNo", dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString());
                            dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", drApprove["PRDtNo"].ToString());
                            dbParams[2] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

                            // Modified on: 10/10/2017, By: Fon
                            // workFlowDt.ExcecuteApprRule("APP.WF_PR_APPR_STEP_" + wfStep, dbParams, hf_ConnStr.Value);
                            string apprRules = wfDt.GetApprRule(wfId, wfStep, LoginInfo.ConnStr);
                            if (apprRules != string.Empty)
                                workFlowDt.ExcecuteApprRule(apprRules, dbParams, hf_ConnStr.Value);
                            else
                                return;
                            // End Mdoified     
                        }
                    }

                    if (noSelectedVendorCount == confirmApproveCount)
                    {
                        lbl_Approve_Chk.Text = "No vendor is assigned.";

                    }
                    //Count Approve.
                    //else if (Approve == 0 && NotApprove >= 0)
                    else if (approveCount == 0 && noSelectedVendorCount >= 0)
                    {
                        lbl_Approve_Chk.Text = "No item is approved.";
                    }
                    //else if (Approve > 0 && NotApprove == 0)
                    else if (approveCount > 0 && noSelectedVendorCount == 0)
                    {
                        lbl_Approve_Chk.Text = "Approval is successful.";
                    }
                    //else if (Approve > 0 && NotApprove > 0)
                    else if (approveCount > 0 && noSelectedVendorCount > 0)
                    {
                        //lbl_Approve_Chk.Text = Approve + " item(s) are approved.<br>" + NotApprove + " item(s) are not approved.";
                        lbl_Approve_Chk.Text = approveCount + " item(s) are approved.<br>" + noSelectedVendorCount + " item(s) are not approved.";
                    }
                    #endregion
                }
                else
                {
                    #region Standard Process
                    for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
                    {
                        var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");

                        //if (Chk_Item.Checked)
                        if (Chk_Item.Checked && Chk_Item.Visible)
                        {
                            var drApprove = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];

                            bool isRejected = drApprove["ApprStatus"].ToString().Contains('R');
                            bool isApproved = drApprove["ApprStatus"].ToString().Substring(step).StartsWith("A");
                            bool isSendBack = false;

                            if (drApprove["ApprStatus"].ToString().Length > (wfStep * 10))
                                if (!drApprove["ApprStatus"].ToString().Substring(wfStep * 10, 10).Contains('_'))
                                    isSendBack = true;

                            confirmApproveCount++;
                            if (isRejected || isApproved || isSendBack)
                                continue;

                            var dbParams = new Blue.DAL.DbParameter[3];
                            dbParams[0] = new Blue.DAL.DbParameter("@PrNo",
                                dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString());
                            dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", drApprove["PRDtNo"].ToString());
                            dbParams[2] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);

                            // Modified on: 10/10/2017, By: Fon
                            string apprRules = wfDt.GetApprRule(wfId, wfStep, LoginInfo.ConnStr);
                            if (apprRules != string.Empty)
                                workFlowDt.ExcecuteApprRule(apprRules, dbParams, hf_ConnStr.Value);
                            else
                                return;
                            // End Mdoified   

                        }
                    }

                    lbl_Approve_Chk.Text = confirmApproveCount + " item(s) are approved.";
                    #endregion
                }

            }

            btn_OK_PopApprClose.Visible = false;
            btn_OK_PopApprFunction.Visible = false;
            pop_ConfirmApprove.ShowOnPageLoad = false;

            if (confirmApproveCount == 0)
            {
                lbl_Approve_Chk.Text = "No detail is selected.";
                btn_OK_PopApprClose.Visible = true;
                pop_Approve.ShowOnPageLoad = true;
            }
            else
            {
                //Added on: 2017/05/23, By: Fon
                bool sentable = false;
                if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["SentEmail"]))
                {
                    lbl_hide_action.Text = "Redirect".ToUpper();
                    lbl_hide_value.Text = true.ToString();
                    //sentable = SendEmail("A");
                    sentable = SendEmailWorkflow.Send("A", prNo, wfId, wfStep, LoginInfo.LoginName, hf_ConnStr.Value);
                }


                if (chk_Approve_NoShowMessage.Checked)
                {
                    Response.Redirect("PrList.aspx");
                }
                else
                {
                    /* old ver.*/
                    //pop_ConfirmApprove.ShowOnPageLoad = false;
                    //pop_OKApprove_Succ.ShowOnPageLoad = true;

                    // Modified on: 2017/04, By: Fon
                    if (Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["IsAllocateVendor"]))
                    {
                        if (noSelectedVendorCount == confirmApproveCount || approveCount == 0 && noSelectedVendorCount >= 0 || approveCount > 0 && noSelectedVendorCount > 0)
                            btn_OK_PopApprClose.Visible = true;
                        else
                            btn_OK_PopApprFunction.Visible = true;

                    }
                    else
                    {
                        btn_OK_PopApprFunction.Visible = true;
                    }



                    pop_Approve.ShowOnPageLoad = true;
                }
            }



        }


        protected void btn_CancelApprove_Click(object sender, EventArgs e)
        {
            pop_ConfirmApprove.ShowOnPageLoad = false;
            pop_Approve.ShowOnPageLoad = false;
        }

        #endregion

        /// <summary>
        ///     Binding Buyer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Buyer_Load(object sender, EventArgs e)
        {
            ddl_Buyer.DataSource = buyer.GetLookUp(LoginInfo.BuInfo.BuCode);
            ddl_Buyer.DataBind();
        }

        protected void ddl_TaxType_Callback(object sender, CallbackEventArgsBase e)
        {
            //VenderChange(e.Parameter);
            //this.UpdateAllAmount();
        }

        protected void ddl_VendorCode_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btn_Reject_Cancel_Click(object sender, EventArgs e)
        {
            pop_Reject.ShowOnPageLoad = false;
        }

        protected void btn_ConfirmSendback_Click(object sender, EventArgs e)
        {
            if (grd_PRDt1.Rows.Count == 0)
            {
                return;
            }

            if (ddl_SendBack.Value == null)
            {
                return;
            }

            var sbPrDtNo = new StringBuilder();

            for (var i = 0; i <= grd_PRDt1.Rows.Count - 1; i++)
            {
                var Chk_Item = (CheckBox)grd_PRDt1.Rows[i].Cells[0].FindControl("Chk_Item");
                if (Chk_Item.Checked)
                {
                    var drSendBack = dsPR.Tables[prDt.TableName].Rows[grd_PRDt1.Rows[i].DataItemIndex];
                    var Step = (wfStep - 1) * 10;


                    // Modified on: 2017/05/22, By: Fon
                    if (wfStep > 1)
                    {
                        if (drSendBack["ApprStatus"].ToString().Contains('R'))
                        {
                            continue;
                        }

                        if (!drSendBack["ApprStatus"].ToString().Substring(Step).StartsWith("_"))
                        {
                            continue;
                        }
                    }

                    //sbPrDtNo.Append((sbPrDtNo.Length > 0 ? ", " + drSendBack["PRDtNo"].ToString() + " " : drSendBack["PRDtNo"].ToString()));

                    var dbParams = new Blue.DAL.DbParameter[6];
                    dbParams[0] = new Blue.DAL.DbParameter("@PrNo", dsPR.Tables[pr.TableName].Rows[0]["PRNo"].ToString());
                    dbParams[1] = new Blue.DAL.DbParameter("@PrDtNo", dsPR.Tables[prDt.TableName].Rows[i]["PRDtNo"].ToString());
                    dbParams[2] = new Blue.DAL.DbParameter("@CurrentStep", wfStep.ToString());
                    dbParams[3] = new Blue.DAL.DbParameter("@Step", ddl_SendBack.Value.ToString());
                    dbParams[4] = new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName);
                    dbParams[5] = new Blue.DAL.DbParameter("@Comment", txt_SendBackMessage.Text.Trim());

                    // Modified on: 10/10/2017, By: Fon
                    string sendBkRule = workFlowDt.GetSendBkRule(wfId, wfStep, LoginInfo.ConnStr);
                    if (sendBkRule != string.Empty) workFlowDt.ExcecuteApprRule(sendBkRule, dbParams, hf_ConnStr.Value);
                    else return;
                    // End Modified.

                }
            }

            var isSentEmail = Convert.ToBoolean(dsWF.Tables["APPwfdt"].Rows[0]["SentEmail"]);

            if (isSentEmail)
            {
                lbl_hide_action.Text = "Redirect".ToUpper();
                lbl_hide_value.Text = true.ToString();

                SendEmailWorkflow.Send("S", prNo, wfId, wfStep, LoginInfo.LoginName, hf_ConnStr.Value);
            }

            pop_SendBack.ShowOnPageLoad = false;
            pop_ConfirmSendback.ShowOnPageLoad = false;

            Response.Redirect("PrList.aspx");
        }

        protected void btn_CancelSendback_Click(object sender, EventArgs e)
        {
            pop_ConfirmSendback.ShowOnPageLoad = false;
            return;
        }

        protected void btn_Reject_Click(object sender, EventArgs e)
        {
            pop_Reject.ShowOnPageLoad = true;
        }

        protected void btn_SendBack_Cancel_Click(object sender, EventArgs e)
        {
            pop_SendBack.ShowOnPageLoad = false;
        }

        protected void btn_SendBack_Click(object sender, EventArgs e)
        {
            pop_SendBack.ShowOnPageLoad = true;
        }

        // test grd
        protected void grd_PRDt1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var chk_All = e.Row.FindControl("Chk_All") as CheckBox;
            var chk_Item = e.Row.FindControl("Chk_Item") as CheckBox;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                chk_All.Checked = true;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region if(DataRow)
                chk_Item.Checked = true;

                // ********** Display Button. **********
                var status = DataBinder.Eval(e.Row.DataItem, "ApprStatus");
                var j = wfStep;


                if (wfStep < 1) j = 1;

                if (status.ToString() != string.Empty)
                {
                    if (!status.ToString().Substring((j - 1) * 10, 10).Contains('_') /*current status ?????????????????????*/
                         || status.ToString().Substring(0, (j - 1) * 10).Contains('_') /*previous status ????????????????*/)
                    {
                        chk_Item.Visible = false;
                    }
                }


                // Item Template
                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString() + " : " +
                                            storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                                                bu.GetConnectionString(
                                                    DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                    lbl_LocationCode.ToolTip = lbl_LocationCode.Text;
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               bu.GetConnectionString(
                                                   DataBinder.Eval(e.Row.DataItem, "BuCode").ToString())) + " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               bu.GetConnectionString(
                                                   DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;

                    // Last 3 receiving
                    var lbl_LastRC = e.Row.FindControl("lbl_LastRC") as Label;
                    if (lbl_LastRC != null && wfStep > 1)
                    {
                        string sql = string.Format(@"DECLARE @prd nvarchar(20) = '{0}'
                                                    ;WITH rc AS(
	                                                    SELECT TOP(3)
		                                                    ROW_NUMBER() OVER(ORDER BY h.RecDate DESC, h.RecNo DESC) as RowId,
		                                                    CAST(h.RecDate AS DATE) RecDate, 
		                                                    h.RecNo, 
		                                                    d.UnitCode, 
		                                                    d.Price, 
		                                                    h.CurrencyCode
	                                                    FROM 
		                                                    PC.REC h
		                                                    JOIN PC.RECDt d ON d.RecNo = h.RecNo
		                                                    WHERE d.ProductCode = @prd
		                                                    ORDER BY h.RecDate DESC, h.RecNo DESC
	
                                                    )
                                                    SELECT TOP(1)
	                                                    STUFF(
		                                                    (
			                                                    SELECT ' ' + '<b>('+ CAST(RowId AS varchar(1)) + ') '+ CAST(RecDate AS nvarchar(10)) + '&ensp;' + RecNo + '</b>  (' + UnitCode + ') ' + CONVERT(varchar, CONVERT(decimal(18,3), Price)) + ' ' + CurrencyCode + '&emsp;&emsp;'b
			                                                    FROM rc
			                                                    FOR XML PATH(''), TYPE
		                                                    ).value('.[1]', 'nvarchar(max)'), 1, 1, '') as LastRC
                                                    FROM 
	                                                    rc", DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString());
                        DataTable dt = pr.DbExecuteQuery(sql, null, hf_ConnStr.Value);
                        if (dt.Rows.Count > 0)
                        {
                            lbl_LastRC.Text = "Last receiving: " + dt.Rows[0][0].ToString();

                        }
                        else
                            lbl_LastRC.Text = string.Empty;
                    }
                }

                if (e.Row.FindControl("lbl_ReqQty") != null)
                {
                    var lbl_ReqQty = e.Row.FindControl("lbl_ReqQty") as Label;
                    lbl_ReqQty.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty"));
                    lbl_ReqQty.ToolTip = lbl_ReqQty.Text;
                }

                if (e.Row.FindControl("lbl_ApprQty_HD") != null)
                {
                    var lbl_ApprQty_HD = e.Row.FindControl("lbl_ApprQty_HD") as Label;
                    lbl_ApprQty_HD.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    lbl_ApprQty_HD.ToolTip = lbl_ApprQty_HD.Text;
                }

                if (e.Row.FindControl("lbl_OrderUnit") != null)
                {
                    var lbl_OrderUnit = e.Row.FindControl("lbl_OrderUnit") as Label;
                    lbl_OrderUnit.Text = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                    lbl_OrderUnit.ToolTip = lbl_OrderUnit.Text;
                }

                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lbl_FOC = e.Row.FindControl("lbl_FOC") as Label;
                    lbl_FOC.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "FOCQTY"));
                    lbl_FOC.ToolTip = lbl_FOC.Text;
                }


                if (e.Row.FindControl("lbl_TotalAmt_HD") != null)
                {
                    var lbl_TotalAmt_HD = e.Row.FindControl("lbl_TotalAmt_HD") as Label;
                    lbl_TotalAmt_HD.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    Total += decimal.Parse(lbl_TotalAmt_HD.Text);
                    lbl_TotalAmt_HD.ToolTip = lbl_TotalAmt_HD.Text;
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt_HD") != null)
                {
                    var label = e.Row.FindControl("lbl_CurrTotalAmt_HD") as Label;
                    //decimal currTotalAmt = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                    //label.Text = String.Format("{0:N}", currTotalAmt);
                    label.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_Currency_HD") != null)
                {
                    var label = e.Row.FindControl("lbl_Currency_HD") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "CurrencyCode").ToString();
                    label.ToolTip = label.Text;
                }


                if (e.Row.FindControl("lbl_Buyer_HD") != null)
                {
                    var lbl_Buyer_HD = e.Row.FindControl("lbl_Buyer_HD") as Label;
                    lbl_Buyer_HD.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                    lbl_Buyer_HD.ToolTip = lbl_Buyer_HD.Text;
                }

                if (DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString() != string.Empty)
                {
                    if (e.Row.FindControl("lbl_VendorCode") != null)
                    {
                        var lbl_VendorCode = e.Row.FindControl("lbl_VendorCode") as Label;
                        lbl_VendorCode.Text = DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString() + " : " +
                                              vendor.GetName(DataBinder.Eval(e.Row.DataItem, "VendorCode").ToString(),
                                                  bu.GetConnectionString(
                                                      DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                        lbl_VendorCode.ToolTip = lbl_VendorCode.Text;
                    }
                }

                if (e.Row.FindControl("lbl_DeliDate") != null)
                {
                    var lbl_DeliDate = e.Row.FindControl("lbl_DeliDate") as Label;
                    lbl_DeliDate.Text =
                        lbl_PRDate.Text =
                            String.Format("{0:d/M/yyyy}", DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString());
                    lbl_DeliDate.ToolTip = lbl_DeliDate.Text;
                }

                if (e.Row.FindControl("lbl_DeliPointCode") != null)
                {
                    var lbl_DeliPointCode = e.Row.FindControl("lbl_DeliPointCode") as Label;
                    lbl_DeliPointCode.Text = deli.GetName(DataBinder.Eval(e.Row.DataItem, "DeliPoint").ToString(),
                        bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                    lbl_DeliPointCode.ToolTip = lbl_DeliPointCode.Text;
                }

                if (e.Row.FindControl("lbl_Price_Dt") != null)
                {
                    var lbl_Price_Dt = e.Row.FindControl("lbl_Price_Dt") as Label;
                    //lbl_Price_Dt.Text = String.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "Price").ToString());
                    lbl_Price_Dt.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                    lbl_Price_Dt.ToolTip = lbl_Price_Dt.Text;
                }

                //************************************************************ Edit At 04 Oct. 2011 At 15:02 *****************************************************
                // Expand 
                #region Other Currency
                if (e.Row.FindControl("lbl_CurrCurr_Grd") != null)
                {
                    Label lbl_CurrCurr_Grd = (Label)e.Row.FindControl("lbl_CurrCurr_Grd");
                    lbl_CurrCurr_Grd.Text = string.Format("( {0} )", DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
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

                if (e.Row.FindControl("lbl_Disc_Grd") != null)
                {
                    var lbl_Disc_Grd = e.Row.FindControl("lbl_Disc_Grd") as Label;
                    lbl_Disc_Grd.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscPercent")) + " % ";
                    lbl_Disc_Grd.ToolTip = lbl_Disc_Grd.Text;
                }

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
                        Blue.BL.GnxLib.GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                    lbl_TaxType_Grd.ToolTip = lbl_TaxType_Grd.Text;
                }

                if (e.Row.FindControl("chk_Adj") != null)
                {
                    var chk_Adj = e.Row.FindControl("chk_Adj") as CheckBox;
                    chk_Adj.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj"));
                    chk_Adj.Enabled = false;
                }

                if (e.Row.FindControl("lbl_Comment_Detail") != null)
                {
                    var lbl_Comment_Detail = e.Row.FindControl("lbl_Comment_Detail") as Label;
                    lbl_Comment_Detail.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment_Detail.ToolTip = lbl_Comment_Detail.Text;
                }

                // Display Stock Summary --------------------------------------------------------------  

                var dsPrDtStockSum = new DataSet();

                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum,
                    DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                    lbl_PRDate.Text, hf_ConnStr.Value);

                if (getPrDtStockSum)
                {
                    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                    {
                        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        //lbl_OnHand.Text = drStockSummary["OnHand"].ToString();
                        lbl_OnHand.Text = string.Format(DefaultQtyFmt, drStockSummary["OnHand"].ToString());
                        lbl_OnHand.ToolTip = lbl_OnHand.Text;

                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                        //lbl_OnOrder.Text = drStockSummary["OnOrder"].ToString();
                        lbl_OnOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["OnOrder"].ToString());
                        lbl_OnOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_ReOrder = e.Row.FindControl("lbl_ReOrder") as Label;
                        lbl_ReOrder.Text = string.Format(DefaultQtyFmt, drStockSummary["Reorder"].ToString());
                        lbl_ReOrder.ToolTip = lbl_OnHand.Text;

                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;
                        lbl_Restock.Text = string.Format(DefaultQtyFmt, drStockSummary["Restock"].ToString());
                        lbl_Restock.ToolTip = lbl_Restock.Text;

                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                        lbl_LastPrice.Text = string.Format(DefaultAmtFmt, drStockSummary["LastPrice"].ToString());
                        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;

                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                    }
                }

                if (e.Row.FindControl("lnk_Po") != null)
                {
                    var lnk_Po = e.Row.FindControl("lnk_Po") as HyperLink;
                    lnk_Po.Text = DataBinder.Eval(e.Row.DataItem, "PONo").ToString();
                    lnk_Po.ToolTip = lnk_Po.Text;
                    lnk_Po.NavigateUrl = "~/PC/PO/Po.aspx?BuCode=" + Request.Params["BuCode"].ToString() + "&ID=" +
                                         lnk_Po.Text + "&VID=3";
                }

                if (e.Row.FindControl("lbl_Ref") != null)
                {
                    var lbl_Ref = e.Row.FindControl("lbl_Ref") as Label;
                    lbl_Ref.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();
                    lbl_Ref.ToolTip = lbl_Ref.Text;
                }

                if (e.Row.FindControl("lbl_Buyer") != null)
                {
                    var lbl_Buyer = e.Row.FindControl("lbl_Buyer") as Label;
                    lbl_Buyer.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                    lbl_Buyer.ToolTip = lbl_Buyer.Text;
                }

                // Display Order ,Receive and Price From PoDt

                var MsgError = string.Empty;
                var dsPoDt = new DataSet();

                var getPoDt = poDt.GetPoDtByPoNo(dsPoDt, ref MsgError,
                    DataBinder.Eval(e.Row.DataItem, "PONo").ToString(),
                    bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));

                if (getPoDt)
                {
                    if (dsPoDt.Tables[poDt.TableName].Rows.Count > 0)
                    {
                        var drPoDt = dsPoDt.Tables[poDt.TableName].Rows[0];

                        if (e.Row.FindControl("lbl_Order") != null)
                        {
                            var lbl_Order = e.Row.FindControl("lbl_Order") as Label;
                            lbl_Order.Text = string.Format(DefaultQtyFmt, drPoDt["OrdQty"].ToString());
                            lbl_Order.ToolTip = lbl_Order.Text;
                        }

                        if (e.Row.FindControl("lbl_Receive") != null)
                        {
                            var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                            lbl_Receive.Text = string.Format(DefaultQtyFmt, drPoDt["RcvQty"].ToString());
                            lbl_Receive.ToolTip = lbl_Receive.Text;
                        }

                        if (e.Row.FindControl("lbl_Price") != null)
                        {
                            var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                            lbl_Price.Text = string.Format(DefaultAmtFmt, drPoDt["Price"].ToString());
                            lbl_Price.ToolTip = lbl_Price.Text;
                        }
                    }
                }



                // Display Grid Price Compare
                dsPriceCompare.Clear();

                var result = false;

                var grd_PriceCompare1 = e.Row.FindControl("grd_PriceCompare1") as GridView;

                /* if (DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString().StartsWith("6"))
                {
                    result = priceList.GetListHQ(dsPriceCompare, LoginInfo.BuInfo.BuGrpCode, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DateTime.Parse(lbl_PRDate.Text.ToString()), decimal.Parse(DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString()),
                        DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString());
                }
                else */

                if (dsPR.Tables[pr.TableName].Rows[0]["PrType"].ToString() == "1") // Market List
                //if (product.GetApprovalLevel(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                //    bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString())) == 2)
                {
                    //Use Delivery Date.
                    result = priceList.GetList(dsPriceCompare, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "ReqDate").ToString()),
                        decimal.Parse(DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString()),
                        DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString(),
                        bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                }
                else
                {
                    //Use Pr Date.
                    result = priceList.GetList(dsPriceCompare, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        DateTime.Parse(lbl_PRDate.Text.ToString()),
                        decimal.Parse(DataBinder.Eval(e.Row.DataItem, "ReqQty").ToString()),
                        DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString(),
                        bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BuCode").ToString()));
                }

                grd_PriceCompare1.DataSource = dsPriceCompare.Tables[priceList.TableName];
                grd_PriceCompare1.DataBind();

                if (!LoginInfo.BuInfo.IsHQ)
                {
                    grd_PriceCompare1.Columns[0].Visible = false;
                }

                if (e.Row.FindControl("ProcessStatusDt") != null)
                {
                    var processStatusDt = e.Row.FindControl("ProcessStatusDt") as PL.UserControls.workflow.ProcessStatusDt;
                    processStatusDt.ApprStatus = DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString();
                    processStatusDt.RefNo = DataBinder.Eval(e.Row.DataItem, "PrNo").ToString();
                    processStatusDt.RefDtNo = int.Parse(DataBinder.Eval(e.Row.DataItem, "PrDtNo").ToString());
                    processStatusDt.ConnString = hf_ConnStr.Value;
                    processStatusDt.DataBind();
                }
                #endregion
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = dsPR.Tables[prDt.TableName];

                object sumTotal = dt.Compute("SUM(CurrTotalAmt)", string.Empty);


                if (e.Row.FindControl("lbl_SumCurrTotalAmt_av") != null)
                {
                    var lbl = e.Row.FindControl("lbl_SumCurrTotalAmt_av") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, Convert.ToDecimal(sumTotal.ToString()));
                    lbl.ToolTip = lbl.Text;
                }

                sumTotal = dt.Compute("SUM(TotalAmt)", string.Empty);

                if (e.Row.FindControl("lbl_SumTotalAmt_av") != null)
                {
                    var lbl = e.Row.FindControl("lbl_SumTotalAmt_av") as Label;
                    //lbl.Text = String.Format("{0:N}", Total);
                    lbl.Text = String.Format(DefaultAmtFmt, Convert.ToDecimal(sumTotal.ToString()));
                    lbl.ToolTip = lbl.Text;
                }
            }
        }

        protected void btn_ProdLog_Click(object sender, EventArgs e)
        {
            pop_ProductLog.ShowOnPageLoad = true;
        }

        protected void btn_SendMsg_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = true;
        }

        protected void btn_Exit_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        protected void btn_Send_Click(object sender, EventArgs e)
        {
            pop_SendMsg.ShowOnPageLoad = false;
        }

        // Void Pr
        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
            return;
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            pop_Void.ShowOnPageLoad = true;
        }

        protected void btn_Void_Success_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
            pop_Void.ShowOnPageLoad = false;

            dsPR.Tables[pr.TableName].Rows[0]["IsVoid"] = true;
            dsPR.Tables[pr.TableName].Rows[0]["DocStatus"] = "Voided";
            dsPR.Tables[pr.TableName].Rows[0]["UpdatedDate"] = ServerDateTime;
            dsPR.Tables[pr.TableName].Rows[0]["UpdatedBy"] = LoginInfo.LoginName;

            var result = pr.Save(dsPR, hf_ConnStr.Value);

            if (result)
            {
                dsPR.Clear();

                // Added on: 21/09/2017, By: Fon
                ClassLogTool pctool = new ClassLogTool();
                pctool.SaveActionLog("PR", lbl_PRNo.Text, "Void");
                // End Added.

                Page_Retrieve();
            }
        }

        protected void btn_OKApprove_Succ_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrList.aspx");
        }

        protected void btn_PopupAlert_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;

            //Added on: 2017/05/25, By: Fon
            if (lbl_hide_action.Text == "Redirect".ToUpper())
            {
                if (Convert.ToBoolean(lbl_hide_value.Text))
                    Response.Redirect("PrList.aspx");
                else
                    Page_Retrieve();
            }
        }

        protected void chk_Approve_NoShowMessage_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk_Approve_NoShowResult = (sender as CheckBox);

            HttpCookie aCookie = new HttpCookie("NoShowMessage_PR_Approve");
            aCookie.Value = chk_Approve_NoShowResult.Checked ? "1" : "0";
            aCookie.Expires = DateTime.Now.AddDays(3650);
            Response.Cookies.Add(aCookie);
        }
    }
}