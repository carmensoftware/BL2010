using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.Transfer
{
    public partial class TrfDt : BasePage
    {
        #region "Attributes"

        //BL.IN.storeRequisition storeReq         = new BL.IN.storeRequisition();
        //BL.IN.StoreRequisitionDetail storeReqDt = new BL.IN.StoreRequisitionDetail();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        //BL.APP.WF workFlow                      = new BL.APP.WF();
        //BL.APP.WFDt workFlowDt                  = new BL.APP.WFDt();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.Transfer trf = new Blue.BL.IN.Transfer();
        private readonly Blue.BL.IN.TransferDt trfDt = new Blue.BL.IN.TransferDt();
        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private DataSet dsTrf = new DataSet();
        private int index;
        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();

        //private bool WorkFlowEnable
        //{
        //    get { return workFlow.GetIsActive("IN", "STOREREQ", LoginInfo.ConnStr); }
        //}

        //private int wfId
        //{
        //    get
        //    {
        //        if (Request.Params["VID"] != null)
        //        {
        //            return viewHandler.GetWFId(int.Parse(Request.Params["VID"].ToString()), LoginInfo.ConnStr);
        //        }
        //        else
        //        {
        //            return viewHandler.GetWFId(int.Parse(Request.Cookies["[IN].[vStoreRequisition]"].Value.ToString()), LoginInfo.ConnStr);
        //        }
        //    }
        //}

        //private int wfStep
        //{
        //    get
        //    {
        //        if (Request.Params["VID"] != null)
        //        {
        //            return viewHandler.GetWFStep(int.Parse(Request.Params["VID"].ToString()), LoginInfo.ConnStr);
        //        }
        //        else
        //        {
        //            return viewHandler.GetWFStep(int.Parse(Request.Cookies["[IN].[vStoreRequisition]"].Value.ToString()), LoginInfo.ConnStr);
        //        }
        //    }
        //}

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTrf = (DataSet)Session["dsTrf"];
            }
        }

        private void Page_Retrieve()
        {
            trf.GetListById(dsTrf, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
            trfDt.GetListByHeaderId(dsTrf, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);

            Session["dsTrf"] = dsTrf;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTrf = dsTrf.Tables[trf.TableName].Rows[0];
            lbl_Date.Text = DateTime.Parse(drTrf["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drTrf["RequestCode"].ToString();
            lbl_Status.Text = drTrf["DocStatus"].ToString();

            if (drTrf["DocStatus"].ToString().ToUpper() == "VOIDED" ||
                drTrf["DocStatus"].ToString().ToUpper() == "COMMITTED")
            {
                btn_Edit.Enabled = false;
                btn_Void.Enabled = false;
            }

            lbl_StoreName.Text = drTrf["LocationCode"] + " : " +
                                 locat.GetName(drTrf["LocationCode"].ToString(), LoginInfo.ConnStr);
            lbl_Desc.Text = drTrf["Description"].ToString();

            grd_TrfDt.DataSource = dsTrf.Tables[trfDt.TableName];
            grd_TrfDt.DataBind();

            // Display Comment
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "Transfer";
            comment.RefNo = drTrf["RequestCode"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "Transfer";
            attach.RefNo = drTrf["RequestCode"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "Transfer";
            log.RefNo = drTrf["RequestCode"].ToString();
            log.Visible = true;
            log.DataBind();
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfLst.aspx");
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            var drTrf = dsTrf.Tables[trf.TableName].Rows[0];
            drTrf["DocStatus"] = "Voided";

            var save = trf.Save(dsTrf, LoginInfo.ConnStr);

            if (save)
            {
                pop_ConfirmVoid.ShowOnPageLoad = false;
                pop_VoidSeccess.ShowOnPageLoad = true;
            }
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_VoidSeccess.ShowOnPageLoad = false;
            Response.Redirect("TrfLst.aspx");
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfEdit.aspx?MODE=New&VID=" + Request.Params["VID"]);
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void grd_TrfDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var p_HeaderIssue = e.Row.FindControl("p_HeaderIssue") as Panel;
                var p_HeaderApproval = e.Row.FindControl("p_HeaderApproval") as Panel;
                var p_HeaderAllocated = e.Row.FindControl("p_HeaderAllocated") as Panel;

                if (Request.Params["VID"] == "40")
                {
                    p_HeaderIssue.Visible = false;
                    p_HeaderApproval.Visible = true;
                    p_HeaderAllocated.Visible = false;
                }
                else if (Request.Params["VID"] == "41")
                {
                    p_HeaderIssue.Visible = false;
                    p_HeaderApproval.Visible = false;
                    p_HeaderAllocated.Visible = true;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var p_ItemIssue = e.Row.FindControl("p_ItemIssue") as Panel;
                var p_ItemApproval = e.Row.FindControl("p_ItemApproval") as Panel;
                var p_ItemAllocated = e.Row.FindControl("p_ItemAllocated") as Panel;

                if (Request.Params["VID"] == "40")
                {
                    p_ItemIssue.Visible = false;
                    p_ItemApproval.Visible = true;
                    p_ItemAllocated.Visible = false;
                }
                else if (Request.Params["VID"] == "41")
                {
                    p_ItemIssue.Visible = false;
                    p_ItemApproval.Visible = false;
                    p_ItemAllocated.Visible = true;
                }

                // Find Control Image Button
                var Img_Btn = e.Row.FindControl("Img_Btn") as ImageButton;

                //set CommandArgument For ImageButton
                if (e.Row.FindControl("Img_Btn") != null)
                {
                    Img_Btn.CommandArgument = index.ToString();
                    index = index + 1;
                }

                if (e.Row.FindControl("lbl_StoreName_Issue") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName_Issue") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                         locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                                             LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                            lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_StoreName_Appr") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName_Appr") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                         locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                                             LoginInfo.ConnStr);
                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(), LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                            lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_StoreName_Allocated") != null)
                {
                    var lbl_StoreName = e.Row.FindControl("lbl_StoreName_Allocated") as Label;
                    lbl_StoreName.Text = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                         locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                                             LoginInfo.ConnStr);
                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(), LoginInfo.ConnStr);
                    lbl_StoreName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ToLocationCode") + " : " +
                                            lbl_StoreName.Text;
                }

                if (e.Row.FindControl("lbl_EnglishName_Issue") != null)
                {
                    var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName_Issue") as Label;
                    lbl_EnglishName.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr) + " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr);
                    lbl_EnglishName.ToolTip = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                              lbl_EnglishName.Text;
                }

                if (e.Row.FindControl("lbl_Unit_Issue") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit_Issue") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("lbl_QtyRequest_Issue") != null)
                {
                    var lbl_QtyRequest = e.Row.FindControl("lbl_QtyRequest_Issue") as Label;
                    lbl_QtyRequest.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "RequestQty"));
                    lbl_QtyRequest.ToolTip = lbl_QtyRequest.Text;
                }

                if (e.Row.FindControl("lbl_DeliveryDate_Issue") != null)
                {
                    var lbl_DeliveryDate = e.Row.FindControl("lbl_DeliveryDate_Issue") as Label;
                    lbl_DeliveryDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (e.Row.FindControl("lbl_DebitName") != null)
                {
                    var lbl_DebitName = e.Row.FindControl("lbl_DebitName") as Label;
                    lbl_DebitName.Text = DataBinder.Eval(e.Row.DataItem, "DebitACCode").ToString();
                    lbl_DebitName.ToolTip = lbl_DebitName.Text;
                }

                if (e.Row.FindControl("lbl_CreditName") != null)
                {
                    var lbl_CreditName = e.Row.FindControl("lbl_CreditName") as Label;
                    lbl_CreditName.Text = DataBinder.Eval(e.Row.DataItem, "CreditACCode").ToString();
                    lbl_CreditName.ToolTip = lbl_CreditName.Text;
                }

                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment.ToolTip = lbl_Comment.Text;
                }

                if (e.Row.FindControl("lbl_ItemGroup") != null)
                {
                    var lbl_ItemGroup = e.Row.FindControl("lbl_ItemGroup") as Label;
                    lbl_ItemGroup.Text =
                        prodCat.GetName(
                            product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_ItemGroup.ToolTip = lbl_ItemGroup.Text;
                }

                if (e.Row.FindControl("lbl_SubCate") != null)
                {
                    var lbl_SubCate = e.Row.FindControl("lbl_SubCate") as Label;
                    lbl_SubCate.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_SubCate.ToolTip = lbl_SubCate.Text;
                }

                if (e.Row.FindControl("lbl_Category") != null)
                {
                    var lbl_Category = e.Row.FindControl("lbl_Category") as Label;
                    lbl_Category.Text =
                        prodCat.GetName(product.GetParentNoByCategoryCode(product.GetParentNoByCategoryCode(
                            product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr)
                            , LoginInfo.ConnStr);
                    lbl_Category.ToolTip = lbl_Category.Text;
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                if (e.Row.FindControl("lbl_EnglishName") != null)
                {
                    var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                    lbl_EnglishName.Text = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                    lbl_EnglishName.ToolTip = lbl_EnglishName.Text;
                }

                if (e.Row.FindControl("lbl_LocalName") != null)
                {
                    var lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                    lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        LoginInfo.ConnStr);
                    lbl_LocalName.ToolTip = lbl_LocalName.Text;
                }

                //******************************* Disyplay Stock Movement ********************************************
                if (e.Row.FindControl("uc_StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = dsTrf.Tables[trf.TableName].Rows[0]["RequestCode"].ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    uc_StockMovement.ConnStr = LoginInfo.ConnStr;
                    uc_StockMovement.DataBind();
                }

                if (dsTrf.Tables[prDt.TableName] != null)
                {
                    dsTrf.Tables[prDt.TableName].Clear();
                }

                var getStock = prDt.GetStockSummary(dsTrf, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    dsTrf.Tables[trf.TableName].Rows[0]["LocationCode"].ToString(), lbl_Date.Text, LoginInfo.ConnStr);
                //DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString()

                if (getStock)
                {
                    var drStockSummary = dsTrf.Tables[prDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;

                        if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                        {
                            lbl_OnHand.Text = String.Format("{0:N}", drStockSummary["OnHand"]);
                        }
                        else
                        {
                            lbl_OnHand.Text = "0.00";
                        }

                        lbl_OnHand.ToolTip = lbl_OnHand.Text;
                    }

                    if (e.Row.FindControl("lbl_OnOrder") != null)
                    {
                        var lbl_OnOrder = e.Row.FindControl("lbl_OnOrder") as Label;

                        if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                        {
                            lbl_OnOrder.Text = String.Format("{0:N}", drStockSummary["OnOrder"]);
                        }
                        else
                        {
                            lbl_OnOrder.Text = "0.00";
                        }

                        lbl_OnOrder.ToolTip = lbl_OnOrder.Text;
                    }

                    if (e.Row.FindControl("lbl_Reorder") != null)
                    {
                        var lbl_Reorder = e.Row.FindControl("lbl_Reorder") as Label;

                        if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                        {
                            lbl_Reorder.Text = String.Format("{0:N}", drStockSummary["Reorder"]);
                        }
                        else
                        {
                            lbl_Reorder.Text = "0.00";
                        }

                        lbl_Reorder.ToolTip = lbl_Reorder.Text;
                    }

                    if (e.Row.FindControl("lbl_Restock") != null)
                    {
                        var lbl_Restock = e.Row.FindControl("lbl_Restock") as Label;

                        if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                        {
                            lbl_Restock.Text = String.Format("{0:N}", drStockSummary["Restock"]);
                        }
                        else
                        {
                            lbl_Restock.Text = "0.00";
                        }

                        lbl_Restock.ToolTip = lbl_Restock.Text;
                    }

                    if (e.Row.FindControl("lbl_LastPrice") != null)
                    {
                        var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;

                        if (drStockSummary["LastPrice"].ToString() != string.Empty &&
                            drStockSummary["LastPrice"] != null)
                        {
                            lbl_LastPrice.Text = String.Format("{0:N}", drStockSummary["LastPrice"]);
                        }
                        else
                        {
                            lbl_LastPrice.Text = "0.00";
                        }

                        lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                    }

                    if (e.Row.FindControl("lbl_LastVendor") != null)
                    {
                        var lbl_LastVendor = e.Row.FindControl("lbl_LastVendor") as Label;
                        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                        lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                    }
                }
                //GridView grd_StoreReqDt = sender as GridView;
                //BoundField apprStatus = new BoundField();
                //apprStatus.DataField = "ApprStatus";

                //if (grd_TrfDt.Columns[grd_TrfDt.Columns.Count - 2].HeaderText.ToUpper() == "PROCESS STATUS"||
                //    grd_TrfDt.Columns[grd_TrfDt.Columns.Count - 2].HeaderText.ToUpper() == "สถานะ")
                //{
                //    string newStatus = string.Empty;

                //    for (int i = 0; i <= e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text.Length - 10; i += 10)
                //    {
                //        if (e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text.Substring(i, 10).Contains('R'))
                //        {
                //            newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/REJ.gif\" style=\"width: 8px; height: 16px\" />";
                //        }
                //        else if (e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text.Substring(i, 10).Contains('_') && e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text.Substring(i, 10).Contains('P'))
                //        {
                //            newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/PAR.gif\" style=\"width: 8px; height: 16px\" />";
                //        }
                //        else if (e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text.Substring(i, 10).Contains('_'))
                //        {
                //            newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/NA.gif\" style=\"width: 8px; height: 16px\" />";
                //        }
                //        else
                //        {
                //            newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/APP.gif\" style=\"width: 8px; height: 16px\" />";
                //        }
                //    }

                //    e.Row.Cells[grd_TrfDt.Columns.Count - 2].Text = newStatus;
                //}
            }
        }

        protected void grd_TrfDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                var Img_Btn =
                    grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as
                        ImageButton;

                if (Img_Btn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
                {
                    var p_DetailRows =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
                    p_DetailRows.Visible = true;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

                    var drExpand =
                        dsTrf.Tables[trfDt.TableName].Rows[
                            grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];

                    var errorMsg = string.Empty;

                    // Display Transaction Detail ---------------------------------------------------------

                    var lbl_DebitName =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_DebitName") as Label;
                    var lbl_CreditName =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_CreditName") as Label;
                    var lbl_Comment =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = drExpand["Comment"].ToString();
                    lbl_Comment.ToolTip = lbl_Comment.Text;

                    var lbl_ItemGroup =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_ItemGroup") as Label;
                    lbl_ItemGroup.Text =
                        prodCat.GetName(
                            product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr),
                            LoginInfo.ConnStr);
                    lbl_ItemGroup.ToolTip = lbl_ItemGroup.Text;

                    var lbl_SubCate =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_SubCate") as Label;
                    lbl_SubCate.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr),
                                LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_SubCate.ToolTip = lbl_SubCate.Text;

                    var lbl_Category =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Category") as Label;
                    lbl_Category.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetParentNoByCategoryCode(
                                    product.GetProductCategory(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_Category.ToolTip = lbl_Category.Text;

                    var lbl_ProductCode =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = drExpand["ProductCode"].ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;

                    var lbl_EnglishName =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_EnglishName") as Label;
                    lbl_EnglishName.Text = product.GetName(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr);
                    lbl_EnglishName.ToolTip = lbl_EnglishName.Text;

                    var lbl_LocalName =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_LocalName") as Label;
                    lbl_LocalName.Text = product.GetName2(drExpand["ProductCode"].ToString(), LoginInfo.ConnStr);
                    lbl_LocalName.ToolTip = lbl_LocalName.Text;

                    // Display Stock Summary --------------------------------------------------------------  

                    //****************** Display Stock Summary ******************
                    if (grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") != null)
                    {
                        var uc_StockSummary =
                            grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") as
                                BlueLedger.PL.PC.StockSummary;

                        uc_StockSummary.ProductCode = drExpand["ProductCode"].ToString();
                        uc_StockSummary.LocationCode = drExpand["ToLocationCode"].ToString();
                        uc_StockSummary.ConnStr = LoginInfo.ConnStr;
                        uc_StockSummary.DataBind();
                    }
                }
                else
                {
                    var p_DetailRows =
                        grd_TrfDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

                    p_DetailRows.Visible = false;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                }
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            //var objArrList = new ArrayList();
            //objArrList.Add("'" + dsTrf.Tables[trf.TableName].Rows[0]["RequestCode"] + "'");
            //Session["s_arrNo"] = objArrList;
            //var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=329" + "&BuCode=" +
            //                  Request.Params["BuCode"];
            //ClientScript.RegisterStartupScript(GetType(), "newWindow",
            //    "<script>window.open('" + reportLink1 + "','_blank')</script>");
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
        }
    }
}