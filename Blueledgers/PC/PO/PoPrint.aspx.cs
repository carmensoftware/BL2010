using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxGridView;

namespace BlueLedger.PL.PC.PO
{
    public partial class PoPrint : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.PC.PO.PoDt PoDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private string PoNo = string.Empty;
        private DataSet dsPo = new DataSet();

        #endregion

        #region "Attributes"

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
                dsPo = (DataSet) Session["dsPo"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                PoNo = Request.Params["ID"];

                var getStrLct = po.GetListByPoNo2(dsTmp, ref MsgError, PoNo, LoginInfo.ConnStr);

                if (getStrLct)
                {
                    dsPo = dsTmp;

                    PoDt.GetListByPoNo(dsPo, ref MsgError, PoNo, LoginInfo.ConnStr);
                }
                else
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }
            }

            Page_Setting();

            Session["dsPo"] = dsPo;
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            if (dsPo.Tables[po.TableName] != null)
            {
                var drPo = dsPo.Tables[po.TableName].Rows[0];

                var dtVendor = new DataTable();
                dtVendor = vendor.GetVendor(drPo["Vendor"].ToString(), LoginInfo.ConnStr);

                lbl_PONumber.Text = drPo["PoNo"].ToString();
                lbl_Status.Text = drPo["DocStatus"].ToString();
                lbl_PODate.Text = DateTime.Parse(drPo["PoDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                lbl_Buyer.Text = drPo["Buyer"].ToString();
                lbl_Location.Text = storeLct.GetName(drPo["Location"].ToString(), LoginInfo.ConnStr);
                //----02/03/2012----storeLct.GetName2(drPo["Location"].ToString(), LoginInfo.ConnStr);
                lbl_DeliveryPoint.Text = deliPoint.GetName(drPo["DeliPoint"].ToString(), LoginInfo.ConnStr);
                lbl_Description.Text = drPo["Description"].ToString();
                lbl_DeliveryDate.Text =
                    DateTime.Parse(drPo["DeliDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                lbl_CreditTerm.Text = drPo["CreditTerm"].ToString();
                lbl_ExChangeRate.Text = drPo["ExchageRate"].ToString();
                lbl_VendorCode.Text = drPo["Vendor"].ToString();
                lbl_Currency.Text = curr.GetName(drPo["Currency"].ToString(), LoginInfo.ConnStr);

                lbl_VendorName.Text = dtVendor.Rows[0]["Name"].ToString();

                lbl_NetAmt.Text = "0.00";
                lbl_TaxAmt.Text = "0.00";
                lbl_Discount.Text = "0.00";
                lbl_TotalAmt.Text = "0.00";
            }

            grd_PoDt.DataSource = dsPo.Tables[PoDt.TableName];
            grd_PoDt.DataBind();

            decimal net = 0;
            decimal tax = 0;
            decimal discount = 0;

            foreach (DataRow drPoDt in dsPo.Tables[PoDt.TableName].Rows)
            {
                if (drPoDt["NetAmt"] != DBNull.Value)
                {
                    net += decimal.Parse(drPoDt["NetAmt"].ToString());
                }

                if (drPoDt["TaxAmt"] != DBNull.Value)
                {
                    tax += decimal.Parse(drPoDt["TaxAmt"].ToString());
                }

                if (drPoDt["Discount"] != DBNull.Value)
                {
                    discount += decimal.Parse(drPoDt["Discount"].ToString());
                }
            }

            lbl_NetAmt.Text = net.ToString("#,###,##");
            lbl_TaxAmt.Text = tax.ToString("#,###,##");
            lbl_Discount.Text = discount.ToString("#,###,##");
            lbl_TotalAmt.Text = ((net + tax) - discount).ToString("#,###,##");
        }

        /// <summary>
        /// </summary>
        protected void Total()
        {
            var iASPxSummaryItem = new ASPxSummaryItem();
            iASPxSummaryItem.FieldName = "NetAmt";
            iASPxSummaryItem.ShowInColumn = "NetAmt";
            iASPxSummaryItem.ShowInGroupFooterColumn = "NetAmt";
            iASPxSummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //grd_PoDts.TotalSummary.Add(iASPxSummaryItem);
            ////lbl_NetAmt.Text = grd_PoDt.GetTotalSummaryValue(iASPxSummaryItem).ToString();
            //grd_PoDts.TotalSummary.Remove(iASPxSummaryItem);

            iASPxSummaryItem.FieldName = "TaxAmt";
            iASPxSummaryItem.ShowInGroupFooterColumn = "TaxAmt";
            iASPxSummaryItem.ShowInColumn = "TaxAmt";
            //grd_PoDts.TotalSummary.Add(iASPxSummaryItem);
            iASPxSummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //lbl_TaxAmt.Text = grd_PoDts.GetTotalSummaryValue(iASPxSummaryItem).ToString();
            //grd_PoDts.TotalSummary.Remove(iASPxSummaryItem);

            if (lbl_Discount.Text == "")
            {
                lbl_Discount.Text = "0";
            }
            ;

            if (lbl_NetAmt.Text == "")
            {
                lbl_NetAmt.Text = "0";
            }
            ;

            if (lbl_TaxAmt.Text == "")
            {
                lbl_TaxAmt.Text = "0";
            }
            ;

            var discount = decimal.Parse(lbl_Discount.Text);
            var netamt = decimal.Parse(lbl_NetAmt.Text);
            var taxamt = decimal.Parse(lbl_TaxAmt.Text);
            lbl_TotalAmt.Text = Convert.ToString(netamt + taxamt - discount);
        }

        #endregion
    }
}