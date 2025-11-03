using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Blue.BL;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STOREREQ
{
    public partial class StoreReqDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.IN.AdjType adjType = new Blue.BL.IN.AdjType();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.storeRequisition storeReq = new Blue.BL.IN.storeRequisition();
        private readonly Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();
        private readonly Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        private readonly Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();

        private DataSet dsStoreReq = new DataSet();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.1";

        private decimal sumTotal = 0;

        private bool WorkFlowEnable
        {
            get { return workFlow.GetIsActive("IN", "STOREREQ", LoginInfo.ConnStr); }
        }

        private int WfId
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFId(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr);
                }
                var httpCookie = Request.Cookies["[IN].[vStoreRequisition]"];
                if (httpCookie != null)
                    return viewHandler.GetWFId(int.Parse(httpCookie.Value),
                        LoginInfo.ConnStr);
                return 0;
            }
        }

        private int WfStep
        {
            get
            {
                if (Request.Params["VID"] != null)
                {
                    return viewHandler.GetWFStep(int.Parse(Request.Params["VID"]), LoginInfo.ConnStr);
                }
                var httpCookie = Request.Cookies["[IN].[vStoreRequisition]"];
                if (httpCookie != null)
                    return viewHandler.GetWFStep(
                        int.Parse(httpCookie.Value), LoginInfo.ConnStr);
                return 0;
            }
        }

        private int WfStepCount
        {
            get
            {
                DataSet ds = new DataSet();
                workFlow.Get(ds, "IN", "SR", LoginInfo.ConnStr);

                return Convert.ToInt32(ds.Tables[0].Rows[0]["StepNo"]);
            }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsStoreReq = (DataSet)Session["dsStoreReq"];
            }
        }

        private void Page_Retrieve()
        {
            storeReq.GetListById(dsStoreReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
            storeReqDt.GetListByHeaderId(dsStoreReq, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            Session["dsStoreReq"] = dsStoreReq;


            dl_ProcessStatus.DataSource = workFlowDt.GetList(workFlow.GetWFId("IN", "SR", LoginInfo.ConnStr), LoginInfo.ConnStr);
            dl_ProcessStatus.DataBind();

            Page_Setting();
        }

        private void Page_Setting()
        {
            // Display current process description
            var v = Request.Cookies["[IN].[vStoreRequisition]"];
            if ((v != null) && (v.Value != string.Empty))
            {
                lbl_Process.Text = viewHandler.GetDesc(int.Parse(v.Value), LoginInfo.ConnStr);
            }

            var drStoreReq = dsStoreReq.Tables[storeReq.TableName].Rows[0];
            lbl_Date.Text = DateTime.Parse(drStoreReq["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drStoreReq["RequestCode"].ToString();
            lbl_Status.Text = drStoreReq["DocStatus"].ToString();

            //Movement Type
            lbl_Move_Type.Text = adjType.GetName(drStoreReq["AdjId"].ToString(), LoginInfo.ConnStr);
            //Project Ref
            //lbl_Project_Ref.Text = drStoreReq["ProjectRef"].ToString();
            string jobCode = drStoreReq["ProjectRef"].ToString();
            var ds = new JobCodeLookup().GetRecord(jobCode, LoginInfo.ConnStr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lbl_Project_Ref.Text = string.Format("{0} : {1}", ds.Tables[0].Rows[0]["Code"], ds.Tables[0].Rows[0]["Description"]);
                lbl_Project_Ref.ToolTip = lbl_Project_Ref.Text;
            }
            else
            {
                lbl_Project_Ref.Text = "";
            }


            // Check Workflow Step

            if (int.Parse(drStoreReq["WFStep"].ToString()) != 1)
            {
                btn_Create.Visible = false;
            }

            if (drStoreReq["ApprStatus"].ToString().StartsWith("_"))
            {
                btn_Void.Visible = true;
            }

            //if (int.Parse(drStoreReq["WFStep"].ToString()) != 3)
            //{
            //    btn_Void.Visible = false;

            //}

            //if ((int.Parse(drStoreReq["WFStep"].ToString()) == 1) && (drStoreReq["ApprStatus"].ToString() == "___"))
            //{
            //    btn_Void.Visible = true;
            //}

            bool allowCreate = workFlowDt.GetAllowCreate(WfId, WfStep, LoginInfo.ConnStr);

            btn_Create.Visible = allowCreate;
            btn_Edit.Visible = WfStep > 0;

            if (WfStep == WfStepCount) // last step
            {
				lbl_ApproveMessage.Text = string.Empty;
                panel_WFControl.Enabled = true;

                // check if the current date is outside the opening period. No approval is allowed.
                // DateTime today = DateTime.Today;
                // DateTime openPeriod = period.GetLatestOpenEndDate(LoginInfo.ConnStr);
                // if (today > openPeriod)
                // {
                    // lbl_ApproveMessage.Text = "Not Allowed to Issue SR Document until closed peroid./ไม่อนุญาตให้ Issue จนกว่าจะปิดเดือนเรียบร้อย";
                    // panel_WFControl.Enabled = false;

                // }
                // else
                // {
                    // lbl_ApproveMessage.Text = string.Empty;
                    // panel_WFControl.Enabled = true;
                // }
            }

            // Check normal operations

            if (drStoreReq["DocStatus"].ToString().ToUpper() == "VOIDED")
            {
                btn_Edit.Visible = false;
                btn_Void.Visible = false;
                WFControlPanel.Visible = false;
            }

            if (drStoreReq["DocStatus"].ToString().ToUpper() == "COMPLETE")
            {
                btn_Edit.Visible = false;
                btn_Void.Visible = false;
            }



            lbl_RequestTo.Text = drStoreReq["LocationCode"].ToString();
            lbl_StoreName.Text = " : " + locat.GetName(drStoreReq["LocationCode"].ToString(), LoginInfo.ConnStr);
            lbl_Desc.Text = drStoreReq["Description"].ToString();
            ProcessStatus.ApprStatus = drStoreReq["ApprStatus"].ToString();

            grd_StoreReqDt.DataSource = dsStoreReq.Tables[storeReqDt.TableName];
            grd_StoreReqDt.DataBind();

            for (var i = 0; i < dsStoreReq.Tables[storeReqDt.TableName].Rows.Count; i++)
            {
                var chkItem = grd_StoreReqDt.Rows[i].FindControl("chk_Item") as CheckBox;
                var s = dsStoreReq.Tables[storeReqDt.TableName].Rows[i]["ApprStatus"].ToString();
                var j = WfStep < 1 ? 1 : WfStep;

                if (chkItem != null)
                    chkItem.Checked = s.Substring((j - 1) * 10, 10).Contains('_');
            }

            // WF Control Setting
            if (WFControlPanel.Visible)
            {
                WFControlPanel.ConnStr = bu.GetConnectionString(LoginInfo.BuInfo.BuCode);
                WFControlPanel.TableSchema = "StoreRequisition";
                WFControlPanel.TableDtSchema = "StoreRequisitionDetail";
                WFControlPanel.Bu = LoginInfo.BuInfo.BuCode;
                WFControlPanel.ColumnName = "RefId";
                WFControlPanel.DBParam = new[] { "@DocNo", "@DocDtNo" };
                WFControlPanel.ControlID = "grd_StoreReqDt";
                WFControlPanel.Ds = dsStoreReq;
                WFControlPanel.WfStep = WfStep;
                WFControlPanel.WFId = WfId;
                WFControlPanel.Module = "IN";
                WFControlPanel.SubModule = "SR";
                WFControlPanel.RedirectTarget = "StoreReqLst.aspx";
                // this.WFControlPanel.PageCode = "[IN].[vStoreRequisition]";
                WFControlPanel.ViewNo = Request.Params["VID"];
                WFControlPanel.DataBind();
            }

            // Modified on: 20/11/2017, By: Fon, About SubModule "STOREREQ" to "SR"
            // Display Comment
            //var masterPage = Master;
            //if (masterPage == null) return;

            var comment = (PL.UserControls.Comment2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "SR";
            comment.RefNo = drStoreReq["RequestCode"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Attach");

            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "SR";
            attach.RefNo = drStoreReq["RequestCode"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)((BlueLedger.PL.Master.In.MasterDefault)Master).FindControl("Log");
            log.Module = "IN";
            log.SubModule = "SR";
            log.RefNo = drStoreReq["RequestCode"].ToString();
            log.Visible = true;
            log.DataBind();
            // End Modified.

            var ii = WfStep < 1 ? 1 : WfStep;
             if (drStoreReq["ApprStatus"].ToString().Substring(ii-1, 1) != "_")
             {
                 btn_Edit.Visible = false;
             }

            // Set User Authorize for Step 1

            if (drStoreReq["WfStep"].ToString() == "1" && drStoreReq["WfStep"].ToString()[0] == '_')
            {
                string createdUsername = drStoreReq["CreateBy"].ToString();
                btn_Void.Visible = createdUsername.ToUpper() == LoginInfo.LoginName.ToUpper();
            }

            Control_HeaderMenuBar();
        }

        // Added on: 03/10/2017, By: Fon
        private void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            btn_Create.Visible = (pagePermiss >= 3) ? btn_Create.Visible : false;
            btn_Edit.Visible = (pagePermiss >= 3) ? btn_Edit.Visible : false;
            btn_Void.Visible = (pagePermiss >= 7) ? btn_Void.Visible : false;
        }
        //  End Added.

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Session["Page"] = Request.Params["Page"];
            Response.Redirect("StoreReqEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            string pageIndex = string.Empty;
            string filterText = string.Empty;
            if (Request.Params["Page"] != null)
                pageIndex = Request.Params["Page"];
            else
            {
                if (Session["Page"] != null)
                {
                    pageIndex = (string)Session["Page"];
                    Session.Remove("Page");
                }
            }
            if (Request.Params["Filter"] != null)
                filterText = Request.Params["Filter"];


            Response.Redirect("StoreReqLst.aspx?Page=" + pageIndex + "&Filter=" + filterText);
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            var drStoreReq = dsStoreReq.Tables[storeReq.TableName].Rows[0];
            drStoreReq["DocStatus"] = "Voided";
            drStoreReq["IsVoid"] = 1;

            var save = storeReq.Save(dsStoreReq, LoginInfo.ConnStr);
            if (!save) return;

            string requestCode = drStoreReq["RequestCode"].ToString();
            _transLog.Save("IN", "SR", requestCode, "VOID", string.Empty, LoginInfo.LoginName, LoginInfo.ConnStr);

            pop_ConfirmVoid.ShowOnPageLoad = false;
            pop_VoidSeccess.ShowOnPageLoad = true;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_VoidSeccess.ShowOnPageLoad = false;
            Response.Redirect("StoreReqLst.aspx");
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreReqEdit.aspx?MODE=New&VID=" + Request.Params["VID"]);
        }


        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void grd_StoreReqDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                #region "Header"

                var lbl_AllocateQty = (Label)e.Row.FindControl("lbl_Allocate_Nm");
                //if (lbl_AllocateQty != null)
                //{
                //    lbl_AllocateQty.Visible = WfStep == WfStepCount; // last step
                //}


                #endregion

            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdrNo = lbl_Ref.Text; // RequestCode
                string dtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                string productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                string toLocationCode = DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString();
                decimal onhand = 0;
                decimal reOrder = 0;
                decimal onOrder = 0;
                decimal lastPrice = 0;
                decimal lastCost = 0;
                decimal reStock = 0;
                string lastVendor = string.Empty;
                DateTime atDate = DateTime.Now;

                DataTable dt = new DataTable();
                dt = bu.DbExecuteQuery(string.Format("SELECT TOP(1) CommittedDate FROM [IN].Inventory WHERE HdrNo = '{0}' AND DtNo = {1} AND ProductCode = '{2}'", hdrNo, dtNo, productCode), null, LoginInfo.ConnStr);
                if (dt.Rows.Count > 0)
                    atDate = (DateTime)dt.Rows[0]["CommittedDate"];


                if (dsStoreReq.Tables[prDt.TableName] != null)
                    dsStoreReq.Tables[prDt.TableName].Clear();
                var getStock = prDt.GetStockSummary(dsStoreReq, productCode, lbl_RequestTo.Text, atDate.ToShortDateString(), LoginInfo.ConnStr);
                if (getStock)
                {
                    DataRow dr = dsStoreReq.Tables[prDt.TableName].Rows[0];

                    onhand = dr["OnHand"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["OnHand"]);
                    onOrder = dr["OnOrder"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["OnOrder"]);
                    reOrder = dr["ReOrder"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["ReOrder"]);
                    lastPrice = dr["LastPrice"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["LastPrice"]);
                    lastCost = dr["LastCost"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["LastCost"]);
                    reStock = dr["ReStock"].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr["ReStock"]);
                    lastVendor = dr["LastVendor"].ToString();
                }


                var status = lbl_Status.Text.Trim();

                if (status.StartsWith("C"))
                {
                    var dtno = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    var query = string.Format("SELECT TOP(1) Amount FROM [IN].Inventory WHERE DtNo={0} AND [Type] IN ('SR','TR')", dtno );
                    var dtCost = bu.DbExecuteQuery(query, null, LoginInfo.ConnStr);

                    lastCost = dtCost.Rows.Count == 0 ? lastCost : Convert.ToDecimal(dtCost.Rows[0][0]);
                }


                // Check Box
                var chkItem = e.Row.FindControl("chk_Item") as CheckBox;
                if (chkItem != null)
                {
                    chkItem.Visible = true;

                    // ********** Display Button. **********
                    string apprStatus = DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString();
                    var j = WfStep < 1 ? 1 : WfStep;
                     if (WfStep == 1 && apprStatus.ToString().Substring((j - 1), 10).Contains('_'))
                     {
                         chkItem.Visible = true;
                     }
                     else if (!apprStatus.ToString().Substring((j - 1) * 10, 10).Contains('_')
                         /*current status ที่เปลี่ยนสถานะไปแล้ว*/
                             // || apprStatus.ToString().Substring(0, (j - 1) * 10).Contains('_')
                         /*previous status ที่ยังทำไม่เสร็จ*/)
                     {
                     chkItem.Visible = false;
                     }

                }

                // ----------------------------------------------------------------------------------------------------
                // Main Information
                // ----------------------------------------------------------------------------------------------------
                #region Main Information

                //  lbl_StoreName_Issue
                var lblStoreNameIssue = e.Row.FindControl("lbl_StoreName_Issue") as Label;
                if (lblStoreNameIssue != null)
                {
                    var loc = DataBinder.Eval(e.Row.DataItem, "ToLocationCode");
                    lblStoreNameIssue.Text = loc + @" : " + locat.GetName(loc.ToString(), LoginInfo.ConnStr);
                    lblStoreNameIssue.ToolTip = lblStoreNameIssue.Text;
                }

                var lblEnglishNameIssue = e.Row.FindControl("lbl_EnglishName_Issue") as Label;
                if (lblEnglishNameIssue != null)
                {
                    var prc = DataBinder.Eval(e.Row.DataItem, "ProductCode");
                    lblEnglishNameIssue.Text = prc + @" : " + product.GetName(prc.ToString(), LoginInfo.ConnStr) + @" : " + product.GetName2(prc.ToString(), LoginInfo.ConnStr);
                    lblEnglishNameIssue.ToolTip = lblEnglishNameIssue.Text;
                }

                var lblUnitIssue = e.Row.FindControl("lbl_Unit_Issue") as Label;
                if (lblUnitIssue != null)
                {
                    var reqU = DataBinder.Eval(e.Row.DataItem, "RequestUnit");
                    lblUnitIssue.Text = reqU.ToString();
                    lblUnitIssue.ToolTip = lblUnitIssue.Text;
                }


                var lblQtyRequestIssue = e.Row.FindControl("lbl_QtyRequest_Issue") as Label;
                if (lblQtyRequestIssue != null)
                {
                    lblQtyRequestIssue.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RequestQty"));
                    lblQtyRequestIssue.ToolTip = lblQtyRequestIssue.Text;
                }


                bool isCreateStep = workFlowDt.GetAllowCreate(WfId, WfStep, LoginInfo.ConnStr);

                var lblQtyApprAppr = e.Row.FindControl("lbl_QtyAppr_Appr") as Label;
                if (lblQtyApprAppr != null)
                {

                    lblQtyApprAppr.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));

                    if (isCreateStep) // First or create step, hide
                        lblQtyApprAppr.Text = string.Empty;

                    lblQtyApprAppr.ToolTip = lblQtyApprAppr.Text;
                }

                var lblQtyAllocated = e.Row.FindControl("lbl_QtyAllocated") as Label;
                if (lblQtyAllocated != null)
                {
                    lblQtyAllocated.Text = string.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "AllocateQty"));
                    lblQtyAllocated.ToolTip = lblQtyAllocated.Text;

                    //lblQtyAllocated.Visible = WfStep == WfStepCount;
                }



                var lblUnitCost = e.Row.FindControl("lbl_UnitCost") as Label;
                if (lblUnitCost != null)
                {
                    lblUnitCost.Text = string.Format(DefaultAmtFmt, lastCost);
                    lblUnitCost.ToolTip = lblUnitCost.Text;
                }

                var lblTotal = e.Row.FindControl("lbl_Total") as Label;
                if (lblTotal != null)
                {
                    decimal qty = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RequestQty"));
                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AllocateQty").ToString()))
                        qty = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AllocateQty"));

                    decimal total = RoundAmt(qty * lastCost);

                    sumTotal += total;
                    lblTotal.Text = string.Format(DefaultAmtFmt, total);
                    lblTotal.ToolTip = lblTotal.Text;
                }

                var lblDeliveryDateIssue = e.Row.FindControl("lbl_DeliveryDate_Issue") as Label;
                if (lblDeliveryDateIssue != null)
                {
                    lblDeliveryDateIssue.Text = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                var processStatusDt = e.Row.FindControl("ProcessStatusDt") as PL.UserControls.workflow.ProcessStatusDt;
                if (processStatusDt != null)
                {
                    processStatusDt.ApprStatus = DataBinder.Eval(e.Row.DataItem, "ApprStatus").ToString();
                    processStatusDt.RefNo = DataBinder.Eval(e.Row.DataItem, "DocumentId").ToString();
                    processStatusDt.RefDtNo = int.Parse(DataBinder.Eval(e.Row.DataItem, "RefId").ToString());
                    processStatusDt.ConnString = LoginInfo.ConnStr;
                    processStatusDt.DataBind();
                }


                #endregion
                // ----------------------------------------------------------------------------------------------------
                // Summary Information
                // ----------------------------------------------------------------------------------------------------
                #region Summary
                //var lblDebitName = e.Row.FindControl("lbl_DebitName") as Label;
                //if (lblDebitName != null)
                //{
                //    lblDebitName.Text = DataBinder.Eval(e.Row.DataItem, "DebitACCode").ToString();
                //    lblDebitName.ToolTip = lblDebitName.Text;
                //}


                //var lblCreditName = e.Row.FindControl("lbl_CreditName") as Label;
                //if (lblCreditName != null)
                //{
                //    lblCreditName.Text = DataBinder.Eval(e.Row.DataItem, "CreditACCode").ToString();
                //    lblCreditName.ToolTip = lblCreditName.Text;
                //}


                var lblItemGroup = e.Row.FindControl("lbl_ItemGroup") as Label;
                if (lblItemGroup != null)
                {
                    lblItemGroup.Text = prodCat.GetName(product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblItemGroup.ToolTip = lblItemGroup.Text;
                }

                var lblSubCate = e.Row.FindControl("lbl_SubCate") as Label;
                if (lblSubCate != null)
                {
                    lblSubCate.Text = prodCat.GetName(product.GetParentNoByCategoryCode(product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblSubCate.ToolTip = lblSubCate.Text;
                }

                var lblCategory = e.Row.FindControl("lbl_Category") as Label;
                if (lblCategory != null)
                {
                    lblCategory.Text = prodCat.GetName(product.GetParentNoByCategoryCode(product.GetParentNoByCategoryCode(product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lblCategory.ToolTip = lblCategory.Text;
                }



                var lblOnHand = e.Row.FindControl("lbl_OnHand") as Label;
                if (lblOnHand != null)
                {
                    lblOnHand.Text = String.Format(DefaultQtyFmt, onhand);
                    lblOnHand.ToolTip = lblOnHand.Text;
                }

                var lblOnHandReq = e.Row.FindControl("lbl_OnHand_Req") as Label;
                if (lblOnHandReq != null)
                {
                    //lblOnHandReq.Text = String.Format("{0:N}", onhand);
                    lblOnHandReq.ToolTip = lblOnHand.Text;
                }

                var lblOnOrder = e.Row.FindControl("lbl_OnOrder") as Label;
                if (lblOnOrder != null)
                {
                    lblOnOrder.Text = String.Format(DefaultQtyFmt, onOrder);
                    lblOnOrder.ToolTip = lblOnOrder.Text;
                }

                var lblReorder = e.Row.FindControl("lbl_Reorder") as Label;
                if (lblReorder != null)
                {
                    lblReorder.Text = String.Format(DefaultQtyFmt, reOrder);
                    lblReorder.ToolTip = lblReorder.Text;
                }

                var lblRestock = e.Row.FindControl("lbl_Restock") as Label;
                if (lblRestock != null)
                {
                    lblRestock.Text = String.Format(DefaultQtyFmt, reStock);
                    lblRestock.ToolTip = lblRestock.Text;
                }

                var lblLastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                if (lblLastPrice != null)
                {
                    lblLastPrice.Text = String.Format(DefaultAmtFmt, lastPrice);
                    lblLastPrice.ToolTip = lblLastPrice.Text;
                }

                var lblLastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                if (lblLastVendor != null)
                {
                    lblLastVendor.Text = lastVendor;
                    lblLastVendor.ToolTip = lblLastVendor.Text;
                }

                //var lblOnHandReq = e.Row.FindControl("lbl_OnHand_Req") as Label;
                //if (lblOnHandReq != null)
                //{
                //    if (dsStoreReq.Tables[prDt.TableName] != null)
                //    {
                //        dsStoreReq.Tables[prDt.TableName].Clear();
                //    }

                //    var getOnHand = prDt.GetStockSummary(dsStoreReq,
                //        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                //        DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                //        DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString(), LoginInfo.ConnStr);

                //    if (!getOnHand) return;
                //    var drStock0 = dsStoreReq.Tables[prDt.TableName].Rows[0];

                //    lblOnHandReq.Text = drStock0["OnHand"].ToString() != string.Empty && drStock0["OnHand"] != null
                //        ? String.Format("{0:N}", drStock0["OnHand"])
                //        : @"0.00";

                //    lblOnHandReq.ToolTip = lblOnHandReq.Text;
                //}

                var lblComment = e.Row.FindControl("lbl_Comment") as Label;
                if (lblComment != null)
                {
                    lblComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lblComment.ToolTip = lblComment.Text;
                }

                // ******************************** Display Stock Movement *****************************
                var ucStockMovement = e.Row.FindControl("uc_StockMovement") as PL.PC.StockMovement;
                if (ucStockMovement != null)
                {
                    ucStockMovement.HdrNo = dsStoreReq.Tables[storeReq.TableName].Rows[0]["RequestCode"].ToString();
                    ucStockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    ucStockMovement.ConnStr = LoginInfo.ConnStr;
                    ucStockMovement.DataBind();
                }

                #endregion
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var lblTotal = e.Row.FindControl("lbl_Total") as Label;
                if (lblTotal != null)
                {
                    lblTotal.Text = string.Format(DefaultAmtFmt, sumTotal);
                    lblTotal.ToolTip = lblTotal.Text;
                }


            }
        }


        protected void grd_StoreReqDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "ShowDetail")
            //{
            //    var imgBtn = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as ImageButton;

            //    if (imgBtn != null
            //        && imgBtn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
            //    {
            //        var pDetailRows = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
            //        if (pDetailRows != null) pDetailRows.Visible = true;
            //        imgBtn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

            //        var drExpand =
            //            dsStoreReq.Tables[storeReqDt.TableName].Rows[
            //                grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];


            //        var lblComment = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Comment") as Label;
            //        if (lblComment != null)
            //        {
            //            lblComment.Text = drExpand["Comment"].ToString();
            //            lblComment.ToolTip = lblComment.Text;
            //        }

            //        var lblItemGroup = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_ItemGroup") as
            //                Label;
            //        if (lblItemGroup != null)
            //        {
            //            lblItemGroup.Text = prodCat.GetName(product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr), LoginInfo.ConnStr);
            //            lblItemGroup.ToolTip = lblItemGroup.Text;
            //        }

            //        var lblSubCate = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_SubCate") as Label;
            //        if (lblSubCate != null)
            //        {
            //            lblSubCate.Text = prodCat.GetName(product.GetParentNoByCategoryCode(product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr),
            //                        LoginInfo.ConnStr), LoginInfo.ConnStr);
            //            lblSubCate.ToolTip = lblSubCate.Text;
            //        }

            //        var lblCategory = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Category") as
            //                Label;
            //        if (lblCategory != null)
            //        {
            //            lblCategory.Text = prodCat.GetName(
            //                product.GetParentNoByCategoryCode(
            //                product.GetParentNoByCategoryCode(
            //                product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr),
            //                LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
            //            lblCategory.ToolTip = lblCategory.Text;
            //        }

            //        var lblProductCode = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_ProductCode") as
            //                Label;
            //        if (lblProductCode != null)
            //        {
            //            lblProductCode.Text = drExpand["ProductCode"].ToString();
            //            lblProductCode.ToolTip = lblProductCode.Text;
            //        }

            //        var lblEnglishName =
            //            grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_EnglishName") as
            //                Label;
            //        if (lblEnglishName != null)
            //        {
            //            lblEnglishName.Text = product.GetName(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr);
            //            lblEnglishName.ToolTip = lblEnglishName.Text;
            //        }

            //        var lblLocalName =
            //            grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_LocalName") as
            //                Label;
            //        if (lblLocalName != null)
            //        {
            //            lblLocalName.Text = product.GetName2(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr);
            //            lblLocalName.ToolTip = lblLocalName.Text;
            //        }

            //        // Display Stock Summary --------------------------------------------------------------  

            //        //****************** Display Stock Summary ******************
            //        if (grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") ==
            //            null) return;
            //        var ucStockSummary = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") as PL.PC.StockSummary;
            //        if (ucStockSummary == null) return;
            //        ucStockSummary.ProductCode = drExpand["ProductCode"].ToString();
            //        ucStockSummary.LocationCode = drExpand["ToLocationCode"].ToString();
            //        ucStockSummary.ConnStr = LoginInfo.ConnStr;
            //        ucStockSummary.DataBind();
            //    }
            //    else
            //    {
            //        var pDetailRows = grd_StoreReqDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

            //        if (pDetailRows != null) pDetailRows.Visible = false;
            //        if (imgBtn != null) imgBtn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            //    }
            //}
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            Report rpt = new Report();
            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", Request.Params["ID"].ToString(), "StoreRequisitionForm");
        }
    }
}