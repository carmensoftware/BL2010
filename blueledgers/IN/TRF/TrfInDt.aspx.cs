using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.TRF
{
    public partial class TrfInDt : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.TransferIn trfIn = new Blue.BL.IN.TransferIn();
        private readonly Blue.BL.IN.TransferInDt trfInDt = new Blue.BL.IN.TransferInDt();
        private DataSet dsTrfIn = new DataSet();
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
                dsTrfIn = (DataSet) Session["dsTrfIn"];
            }
        }

        private void Page_Retrieve()
        {
            trfIn.GetList(dsTrfIn, Request.Params["ID"], LoginInfo.ConnStr);
            trfInDt.GetList(dsTrfIn, Request.Params["ID"], LoginInfo.ConnStr);

            Session["dsTrfIn"] = dsTrfIn;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drTrfIn = dsTrfIn.Tables[trfIn.TableName].Rows[0];

            //Disable buttons when document status is 'Voided' or 'Committed'.
            if (drTrfIn["Status"].ToString().ToUpper() == "VOIDED" ||
                drTrfIn["Status"].ToString().ToUpper() == "COMMITTED")
            {
                btn_Edit.Enabled = false;
                btn_Void.Enabled = false;
            }

            //Show Info.
            lbl_Date.Text = DateTime.Parse(drTrfIn["CreateDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            lbl_Ref.Text = drTrfIn["RefId"].ToString();
            lbl_Status.Text = drTrfIn["Status"].ToString();
            lbl_CommitDate.Text = drTrfIn["CommitDate"] == DBNull.Value
                ? string.Empty
                : DateTime.Parse(drTrfIn["CommitDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //lbl_FromLocationCode.Text   = drTrfIn["FromStoreId"].ToString();
            lbl_FromLocationName.Text = drTrfIn["FromStoreId"] + " : " +
                                        locat.GetName(drTrfIn["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfIn["FromStoreId"].ToString(), LoginInfo.ConnStr);
            //lbl_DeliveryDate.Text       = DateTime.Parse(drTrfIn["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            //lbl_ToLocationCode.Text     = drTrfIn["ToStoreId"].ToString();
            lbl_ToLocationName.Text = drTrfIn["ToStoreId"] + " : " +
                                      locat.GetName(drTrfIn["ToStoreId"].ToString(), LoginInfo.ConnStr);
            //----02/03/2012----locat.GetName2(drTrfIn["ToStoreId"].ToString(), LoginInfo.ConnStr);
            lbl_Desc.Text = drTrfIn["Description"].ToString();

            grd_TrfInDt.DataSource = dsTrfIn.Tables[trfInDt.TableName];
            grd_TrfInDt.DataBind();

            // Display Comment
            var comment = (PL.UserControls.Comment2) Master.FindControl("Comment");
            comment.Module = "IN";
            comment.SubModule = "TRNIN";
            comment.RefNo = "1";
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2) Master.FindControl("Attach");
            attach.ModuleName = "TRNIN";
            attach.RefNo = "1";
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2) Master.FindControl("Log");
            log.Module = "IN";
            log.SubModule = "TRNIN";
            log.RefNo = "1";
            log.Visible = true;
            log.DataBind();
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfInEdit.aspx?MODE=Edit&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrfInLst.aspx");
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

        //protected void grd_StoreReqDt_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        //{
        //    if (!e.Expanded)
        //    {
        //        return;
        //    }
        //    grd_StoreReqDt.DetailRows.CollapseAllRows();
        //    grd_StoreReqDt.DetailRows.ExpandRow(e.VisibleIndex);

        //    DataRow drExpand    = dsStoreReq.Tables[storeReqDt.TableName].Rows[e.VisibleIndex];
        //    string errorMsg     = string.Empty;

        //    // Display Transaction Detail ---------------------------------------------------------

        //    Label lbl_Debit = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Debit") as Label;
        //    lbl_Debit.Text  = drExpand["DebitACCode"].ToString();

        //    Label lbl_DebitName = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_DebitName") as Label;
        //    //lbl_DebitName.Text = drExpand["DiccountAmt"].ToString();

        //    Label lbl_Credit    = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Credit") as Label;
        //    lbl_Credit.Text     = drExpand["CreditACCode"].ToString();

        //    Label lbl_CreditName    = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_CreditName") as Label;
        //    //lbl_CreditName.Text = drExpand["TaxRate"].ToString();


        //    // Display Comment --------------------------------------------------------------     

        //    Label lbl_Comment   = grd_StoreReqDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Comment") as Label;
        //    lbl_Comment.Text    = drExpand["Comment"].ToString();

        //}

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            var drTrfIn = dsTrfIn.Tables[trfIn.TableName].Rows[0];
            drTrfIn["Status"] = "Voided";

            var Save = trfIn.Save(dsTrfIn, LoginInfo.ConnStr);

            if (Save)
            {
                pop_ConfirmVoid.ShowOnPageLoad = false;
                Response.Redirect("TrfInLst.aspx");
            }
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        //protected void btn_Create_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("StkInEdit.aspx?MODE=New");
        //}

        //protected void grd_TrfInDt_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        //{
        //    if (!e.Expanded)
        //    {
        //        return;
        //    }

        //    grd_TrfInDt.DetailRows.CollapseAllRows();
        //    grd_TrfInDt.DetailRows.ExpandRow(e.VisibleIndex);

        //    DataRow drExpand    = dsTrfIn.Tables[trfInDt.TableName].Rows[e.VisibleIndex];
        //    string errorMsg     = string.Empty;

        //    // Display Transaction Detail ---------------------------------------------------------

        //    Label lbl_Debit = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Debit") as Label;
        //    //lbl_Debit.Text  = drExpand["DebitACCode"].ToString();

        //    Label lbl_DebitName = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_DebitName") as Label;
        //    //lbl_DebitName.Text = drExpand["DiccountAmt"].ToString();

        //    Label lbl_Credit = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Credit") as Label;
        //    //lbl_Credit.Text     = drExpand["CreditACCode"].ToString();

        //    Label lbl_CreditName = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_CreditName") as Label;
        //    //lbl_CreditName.Text = drExpand["TaxRate"].ToString();

        //    // Display Comment --------------------------------------------------------------     

        //    //Label lbl_Comment   = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_Comment") as Label;
        //    //lbl_Comment.Text    = drExpand["Comment"].ToString();
        //    TextBox txt_Comment = grd_TrfInDt.FindDetailRowTemplateControl(e.VisibleIndex, "txt_Comment") as TextBox;
        //    txt_Comment.Text    = drExpand["Comment"].ToString();
        //}

        //protected void grd_TrfInDt_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        GridViewDataColumn colProductEngName    = (GridViewDataColumn)grd_TrfInDt.Columns["English Name"];
        //        ASPxLabel lbl_EnglishName               = (ASPxLabel)grd_TrfInDt.FindRowCellTemplateControl(e.VisibleIndex, colProductEngName, "lbl_EnglishName");

        //        if (lbl_EnglishName != null)
        //        {
        //            lbl_EnglishName.Text = product.GetName(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }

        //        GridViewDataColumn colProductLocalName  = (GridViewDataColumn)grd_TrfInDt.Columns["Local Name"];
        //        ASPxLabel lbl_LocalName                 = (ASPxLabel)grd_TrfInDt.FindRowCellTemplateControl(e.VisibleIndex, colProductLocalName, "lbl_LocalName");

        //        if (lbl_LocalName != null)
        //        {
        //            lbl_LocalName.Text = product.GetName2(e.GetValue("ProductCode").ToString(), LoginInfo.ConnStr);
        //        }
        //    }
        //}

        protected void grd_TrfInDt_Load(object sender, EventArgs e)
        {
            grd_TrfInDt.DataSource = dsTrfIn.Tables[trfInDt.TableName];
            grd_TrfInDt.DataBind();
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void grd_TrfInDt_RowDataBound(object sender, GridViewRowEventArgs e)
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
                                               LoginInfo.ConnStr) + " : " +
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

                if (e.Row.FindControl("lbl_QtyTrf") != null)
                {
                    var lbl_QtyTrf = e.Row.FindControl("lbl_QtyTrf") as Label;
                    lbl_QtyTrf.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "QtyOut"));
                    lbl_QtyTrf.ToolTip = lbl_QtyTrf.Text;
                    total += decimal.Parse(lbl_QtyTrf.Text);
                }

                if (e.Row.FindControl("lbl_QtyIn") != null)
                {
                    var lbl_QtyIn = e.Row.FindControl("lbl_QtyIn") as Label;
                    lbl_QtyIn.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "QtyIn"));
                    lbl_QtyIn.ToolTip = lbl_QtyIn.Text;
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

        protected void grd_TrfInDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                var Img_Btn =
                    grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as
                        ImageButton;

                if (Img_Btn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
                {
                    var p_DetailRows =
                        grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
                    p_DetailRows.Visible = true;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

                    var drExpand =
                        dsTrfIn.Tables[trfInDt.TableName].Rows[
                            grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];

                    var errorMsg = string.Empty;

                    //Label lbl_Debit = grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Debit") as Label;
                    //lbl_Debit.Text = drExpand["DebitAcc"].ToString();

                    //Label lbl_Credit = grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Credit") as Label;
                    //lbl_Credit.Text = drExpand["CreditAcc"].ToString();

                    //Label lbl_OnHand = grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_OnHand") as Label;

                    //TextBox txt_Comment = grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("txt_Comment") as TextBox;
                    //txt_Comment.Text = drExpand["Comment"].ToString();

                    var lbl_Comment =
                        grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = drExpand["Comment"].ToString();
                }
                else
                {
                    var p_DetailRows =
                        grd_TrfInDt.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

                    p_DetailRows.Visible = false;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                }
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            var objArrList = new ArrayList();
            objArrList.Add("'" + dsTrfIn.Tables[trfInDt.TableName].Rows[0]["RefID"] + "'");
            Session["s_arrNo"] = objArrList;
            var reportLink1 = "../../RPT/ReportCriteria.aspx?category=012&reportid=329" + "&BuCode=" +
                              Request.Params["BuCode"];
            ClientScript.RegisterStartupScript(GetType(), "newWindow",
                "<script>window.open('" + reportLink1 + "','_blank')</script>");
        }
    }
}