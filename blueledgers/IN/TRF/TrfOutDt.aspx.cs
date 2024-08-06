using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfOutDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();
        private readonly Blue.BL.IN.TransferOut trfOut = new Blue.BL.IN.TransferOut();
        private readonly Blue.BL.IN.TransferOutDt trfOutDt = new Blue.BL.IN.TransferOutDt();
        private DataSet dsTrfOut = new DataSet();
        private int index;
        private decimal total;

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTrfOut = (DataSet) Session["dsTrfOut"];
            }
        }

        private void Page_Retrieve()
        {
            trfOut.GetList(dsTrfOut, Request.Params["ID"], LoginInfo.ConnStr);
            trfOutDt.GetList(dsTrfOut, Request.Params["ID"], LoginInfo.ConnStr);

            Session["dsTrfOut"] = dsTrfOut;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTrfOut = dsTrfOut.Tables[trfOut.TableName].Rows[0];

            //Disable buttons when document status is 'Voided' or 'Committed'.
            if (drTrfOut["Status"].ToString().ToUpper() == "VOIDED" ||
                drTrfOut["Status"].ToString().ToUpper() == "COMMITTED")
            {
                btn_Edit.Enabled = false;
                btn_Void.Enabled = false;
            }

            //Show Info.
            lbl_Date.Text = DateTime.Parse(drTrfOut["CreateDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drTrfOut["RefId"].ToString();
            lbl_Status.Text = drTrfOut["Status"].ToString();
            lbl_CommitDate.Text = drTrfOut["CommitDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drTrfOut["CommitDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //lbl_FromLocationCode.Text   = drTrfOut["FromStoreId"].ToString();
            lbl_FromLocationName.Text = drTrfOut["FromStoreId"] + " : " +
                                        locat.GetName(drTrfOut["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfOut["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //lbl_DeliveryDate.Text       = DateTime.Parse(drTrfOut["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //lbl_ToLocationCode.Text     = drTrfOut["ToStoreId"].ToString();
            lbl_ToLocationName.Text = drTrfOut["ToStoreId"] + " : " +
                                      locat.GetName(drTrfOut["ToStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfOut["ToStoreId"].ToString(), LoginInfo.ConnStr);
            lbl_Desc.Text = drTrfOut["Description"].ToString();

            grd_TrfOutDt.DataSource = dsTrfOut.Tables[trfOutDt.TableName];
            grd_TrfOutDt.DataBind();

            // Display Comment
            var comment = (PL.UserControls.Comment2) Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "TRNOUT";
            comment.RefNo = "1";
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2) Master.FindControl("Attach");
            attach.ModuleName = "TRNOUT";
            attach.RefNo = "1";
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2) Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "TRNOUT";
            log.RefNo = "1";
            log.Visible = true;
            log.DataBind();
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfOutEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfOutLst.aspx");
        }

        //protected void grd_StoreReqDt_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colLocationName = (GridViewDataColumn)grd_StoreReqDt.Columns["Store Name"];
        //        ASPxLabel lbl_gStoreName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colLocationName, "lbl_gStoreName");

        //        if (lbl_gStoreName != null)
        //        {
        //            lbl_gStoreName.Text = locat.GetName(e.GetValue("ToLocationCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductEngName = (GridViewDataColumn)grd_StoreReqDt.Columns["English Name"];
        //        ASPxLabel lbl_EnglishName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colProductEngName, "lbl_EnglishName");

        //        if (lbl_EnglishName != null)
        //        {
        //            lbl_EnglishName.Text = product.GetName(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductLocalName = (GridViewDataColumn)grd_StoreReqDt.Columns["Local Name"];
        //        ASPxLabel lbl_LocalName = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colProductLocalName, "lbl_LocalName");

        //        if (lbl_LocalName != null)
        //        {
        //            lbl_LocalName.Text = product.GetName2(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colDeliveryDate = (GridViewDataColumn)grd_StoreReqDt.Columns["Delivery Date"];
        //        ASPxLabel lbl_DeliveryDate = (ASPxLabel)grd_StoreReqDt.FindRowCellTemplateControl(e.VisibleIndex, colDeliveryDate, "lbl_DeliveryDate");

        //        if (lbl_DeliveryDate != null)
        //        {
        //            lbl_DeliveryDate.Text = DateTime.Parse(e.GetValue("DeliveryDate").ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
        //        }

        //    }
        //}

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            var drTrfOut = dsTrfOut.Tables[trfOut.TableName].Rows[0];
            drTrfOut["Status"] = "Voided";

            var Save = trfOut.Save(dsTrfOut, LoginInfo.ConnStr);

            if (Save)
            {
                pop_ConfirmVoid.ShowOnPageLoad = false;
                Response.Redirect("TrfOutLst.aspx");
            }
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfOutEdit.aspx?MODE=New&VID=" + Request.Params["VID"]);
        }

        //protected void grd_TrfOutDt_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        //{
        //    if (!e.Expanded)
        //    {
        //        return;
        //    }

        //    grd_TrfOutDt.DetailRows.CollapseAllRows();
        //    grd_TrfOutDt.DetailRows.ExpandRow(e.VisibleIndex);

        //    DataRow drExpand    = dsTrfOut.Tables[trfOutDt.TableName].Rows[e.VisibleIndex];
        //    string errorMsg     = string.Empty;

        //    // Display Transaction Detail ---------------------------------------------------------

        //    Label lbl_Debit = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Debit") as Label;
        //    //lbl_Debit.Text  = drExpand["DebitACCode"].ToString();

        //    Label lbl_DebitName = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_DebitName") as Label;
        //    //lbl_DebitName.Text = drExpand["DiccountAmt"].ToString();

        //    Label lbl_Credit    = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Credit") as Label;
        //    //lbl_Credit.Text     = drExpand["CreditACCode"].ToString();

        //    Label lbl_CreditName = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_CreditName") as Label;
        //    //lbl_CreditName.Text = drExpand["TaxRate"].ToString();


        //    // Display Comment --------------------------------------------------------------     

        //    //Label lbl_Comment   = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Comment") as Label;
        //    //lbl_Comment.Text    = drExpand["Comment"].ToString();
        //    TextBox txt_Comment     = grd_TrfOutDt.FindDetailRowTemplateControl(e.VisibleIndex, "txt_Comment") as TextBox;
        //    txt_Comment.Text        = drExpand["Comment"].ToString();
        //}

        //protected void grd_TrfOutDt_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colProductEngName    = (GridViewDataColumn)grd_TrfOutDt.Columns["English Name"];
        //        ASPxLabel lbl_EnglishName               = (ASPxLabel)grd_TrfOutDt.FindRowCellTemplateControl(e.VisibleIndex, colProductEngName, "lbl_EnglishName");

        //        if (lbl_EnglishName != null)
        //        {
        //            lbl_EnglishName.Text = product.GetName(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductLocalName  = (GridViewDataColumn)grd_TrfOutDt.Columns["Local Name"];
        //        ASPxLabel lbl_LocalName                 = (ASPxLabel)grd_TrfOutDt.FindRowCellTemplateControl(e.VisibleIndex, colProductLocalName, "lbl_LocalName");

        //        if (lbl_LocalName != null)
        //        {
        //            lbl_LocalName.Text = product.GetName2(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }
        //    }
        //}

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void grd_TrfOutDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find Control Image Button
                var Img_Btn = e.Row.FindControl("Img_Btn") as ImageButton;

                //set CommandArgument For ImageButton
                if (e.Row.FindControl("Img_Btn") != null)
                {
                    Img_Btn.CommandArgument = index.ToString();
                    index = index + 1;
                }

                //if (e.Row.FindControl("lbl_ProductCode") != null)
                //{
                //    Label lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                //    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                //}

                if (e.Row.FindControl("lbl_EnglishName") != null)
                {
                    var lbl_EnglishName = e.Row.FindControl("lbl_EnglishName") as Label;
                    lbl_EnglishName.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr) + " : "
                                           +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr);
                    lbl_EnglishName.ToolTip = lbl_EnglishName.Text;
                }

                //if (e.Row.FindControl("lbl_LocalName") != null)
                //{
                //    Label lbl_LocalName = e.Row.FindControl("lbl_LocalName") as Label;
                //    lbl_LocalName.Text = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr);
                //}

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("lbl_QtyAllocate") != null)
                {
                    var lbl_QtyAllocate = e.Row.FindControl("lbl_QtyAllocate") as Label;
                    lbl_QtyAllocate.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "QtyAllocate"));
                    lbl_QtyAllocate.ToolTip = lbl_QtyAllocate.Text;
                    total += decimal.Parse(lbl_QtyAllocate.Text);
                }

                if (e.Row.FindControl("lbl_QtyOut") != null)
                {
                    var lbl_QtyOut = e.Row.FindControl("lbl_QtyOut") as Label;
                    lbl_QtyOut.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "QtyOut"));
                    lbl_QtyOut.ToolTip = lbl_QtyOut.Text;
                }
            }

            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if (e.Row.FindControl("lbl_Total") != null)
            //    {
            //        Label lbl_Total = e.Row.FindControl("lbl_Total") as Label;
            //        lbl_Total.Text = String.Format("{0:N}", total);
            //    }
            //}
        }

        protected void grd_TrfOutDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                var Img_Btn =
                    grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as
                        ImageButton;

                if (Img_Btn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
                {
                    var p_DetailRows =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
                    p_DetailRows.Visible = true;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

                    var drExpand =
                        dsTrfOut.Tables[trfOutDt.TableName].Rows[
                            grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];
                    storeReqDt.GetListByHeaderId(dsTrfOut, int.Parse(drExpand["SRId"].ToString()), LoginInfo.ConnStr);

                    var errorMsg = string.Empty;

                    //Label lbl_Debit = grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Debit") as Label;
                    //lbl_Debit.Text = drExpand["DebitAcc"].ToString();

                    //Label lbl_DebitName = grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_DebitName") as Label;
                    //lbl_DebitName.Text = drExpand[""].ToString();

                    //Label lbl_Credit = grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Credit") as Label;
                    //lbl_Credit.Text = drExpand["CreditAcc"].ToString();

                    var lbl_QtyAppr =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_QtyAppr") as Label;
                    //lbl_QtyAppr.Text = String.Format("{0:N}", dsTrfOut.Tables[storeReqDt.TableName].Rows[int.Parse(e.CommandArgument.ToString())]["ApprQty"]);

                    var lbl_Sr =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Sr") as Label;
                    //lbl_Sr.Text = drExpand["SRId"].ToString();

                    var lbl_QtyReq =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_QtyReq") as Label;
                    //lbl_QtyReq.Text = String.Format("{0:N}", dsTrfOut.Tables[storeReqDt.TableName].Rows[int.Parse(e.CommandArgument.ToString())]["RequestQty"]);

                    var lbl_OnHand =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_OnHand") as Label;

                    //TextBox txt_Comment = grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("txt_Comment") as TextBox;
                    //txt_Comment.Text = drExpand["Comment"].ToString();
                    var lbl_Comment =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = drExpand["Comment"].ToString();
                }
                else
                {
                    var p_DetailRows =
                        grd_TrfOutDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

                    p_DetailRows.Visible = false;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                }
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            var objArrList = new ArrayList();
            objArrList.Add("'" + dsTrfOut.Tables[trfOut.TableName].Rows[0]["RefID"] + "'");
            Session["s_arrNo"] = objArrList;
            var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=330" + "&BuCode=" +
                              Request.Params["BuCode"];
            ClientScript.RegisterStartupScript(GetType(), "newWindow",
                "<script>window.open('" + reportLink1 + "','_blank')</script>");
        }
    }
}