using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.PC.PR
{
    public partial class PrPrint : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private DataSet dsPR = new DataSet();
        private string prNo = string.Empty;
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();
        private Blue.BL.APP.WFDt workFlowDt = new Blue.BL.APP.WFDt();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPR = (DataSet) Session["dsPR"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            //DataSet dsTmp = new DataSet();
            var MsgError = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params["prno"]))
            {
                prNo = Request.Params["prno"];

                var getPr = pr.GetListByPrNo(dsPR, ref MsgError, prNo, LoginInfo.ConnStr);

                if (!getPr)
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }

                var getPrDt = prDt.GetListByPrNo(dsPR, ref MsgError, prNo, LoginInfo.ConnStr);

                if (!getPrDt)
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }

                Session["dsPR"] = dsPR;

                Page_Setting();
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            if (dsPR.Tables[pr.TableName] != null)
            {
                var drPR = dsPR.Tables[pr.TableName].Rows[0];

                lbl_PRNo.Text = drPR["PrNo"].ToString();
                //lbl_Status.Text        = drPR["ApprStatus"].ToString();                
                lbl_PRDate.Text = DateTime.Parse(drPR["PrDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                //lbl_Buyer.Text           = drPR["Buyer"].ToString();
                lbl_Location.Text = storeLct.GetName(drPR["Location"].ToString(), LoginInfo.ConnStr);
                //----02/03/2012----storeLct.GetName2(drPR["Location"].ToString(), LoginInfo.ConnStr);
                lbl_DeliPoint.Text = deliPoint.GetName(drPR["DeliPoint"].ToString(), LoginInfo.ConnStr);
                lbl_Desc.Text = drPR["Description"].ToString();
            }

            grd_PrDt.DataSource = dsPR.Tables[prDt.TableName];
            grd_PrDt.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PrDt_DetailRowExpandedChanged(object sender,
            DevExpress.Web.ASPxGridView.ASPxGridViewDetailRowEventArgs e)
        {
            if (e.Expanded)
            {
                //Label lbl_onhand        = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_onhand");
                //Label lbl_totalonhand   = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_totalonhand");
                //Label lbl_order         = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_order");
                //Label lbl_totalorder    = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_totalorder");
                //Label lbl_reorder       = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_reorder");
                //Label lbl_restock       = (Label)grd_PrDt.FindDetailRowTemplateControl(e.VisibleIndex, "lbl_restock");


                // Get Onhand 
                //decimal onhand = product.GetProductOnHand(dsPR.Tables[pr.TableName].Rows[0]["Location"].ToString(),dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Get OnOrder
                //decimal Order = product.GetProductOnOrder(dsPR.Tables[pr.TableName].Rows[0]["Location"].ToString(),dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Get Onhand Total
                //decimal Totalonhand = product.GetProductOnHandAll(dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Get OnOrder Total
                //decimal TotalOrder = product.GetProductOnOrderAll(dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Get ReOrder
                //decimal ReOrder = product.GetProductReOrder(dsPR.Tables[pr.TableName].Rows[0]["Location"].ToString(), dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Get ReStock
                //decimal ReStock = product.GetProductReStock(dsPR.Tables[pr.TableName].Rows[0]["Location"].ToString(), dsPR.Tables[prDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr);

                // Seeting 
                //lbl_onhand.Text = onhand.ToString();
                //lbl_totalonhand.Text = Totalonhand.ToString();
                //lbl_order.Text = Order.ToString();
                //lbl_totalorder.Text = TotalOrder.ToString();
                //lbl_reorder.Text = ReOrder.ToString();
                //lbl_restock.Text = ReStock.ToString();

                // Get Last Vendor
                // Get Price Source
                // Get Last Price
            }
        }

        protected void grd_PrDt_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var col1 = (GridViewDataColumn) grd_PrDt.Columns["VendorCode"];
                var hpl_Vendor = (HyperLink) grd_PrDt.FindRowCellTemplateControl(e.VisibleIndex, col1, "hpl_Vendor");

                if (hpl_Vendor != null)
                {
                    //hpl_Vendor.Text = vendor.GetName(e.GetValue("VendorCode").ToString(), LoginInfo.ConnStr).Substring(0, 20);
                    hpl_Vendor.Text = vendor.GetName(e.GetValue("VendorCode").ToString(), LoginInfo.ConnStr);
                    hpl_Vendor.NavigateUrl = "~/FC/AP/VD/Vendor.aspx?ID=" + e.GetValue("VendorCode");
                }

                var col2 = (GridViewDataColumn) grd_PrDt.Columns["PONo"];
                var hpl_PoNo = (HyperLink) grd_PrDt.FindRowCellTemplateControl(e.VisibleIndex, col2, "hpl_PoNo");

                if (hpl_PoNo != null)
                {
                    hpl_PoNo.Text = e.GetValue("PONo").ToString();
                    hpl_PoNo.NavigateUrl = "~/PC/PO/Po.aspx?ID=" + e.GetValue("PONo");
                }
            }
        }
    }
}