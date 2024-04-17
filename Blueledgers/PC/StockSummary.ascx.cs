using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC
{
    public partial class StockSummary : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet dsPrDtStockSum = new DataSet();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly BasePage basePage = new BasePage();

        private string _connStr;
        private string _currentDate;
        private string _locationCode;
        private string _productCode;

        public string ProductCode
        {
            get
            {
                if (ViewState["ProductCode"] == null)
                {
                    return _productCode;
                }

                return ViewState["ProductCode"].ToString();
            }
            set
            {
                _productCode = value;
                ViewState["ProductCode"] = _productCode;
            }
        }

        public string LocationCode
        {
            get
            {
                if (ViewState["LocationCode"] == null)
                {
                    return _locationCode;
                }

                return ViewState["LocationCode"].ToString();
            }
            set
            {
                _locationCode = value;
                ViewState["LocationCode"] = _locationCode;
            }
        }

        public string CurrentDate
        {
            get
            {
                if (ViewState["Date"] == null)
                {
                    return _currentDate;
                }

                return ViewState["Date"].ToString();
            }
            set
            {
                _currentDate = value;
                ViewState["Date"] = _currentDate;
            }
        }

        public string ConnStr
        {
            get
            {
                if (ViewState["ConnStr"] == null)
                {
                    return _connStr;
                    ;
                }

                return ViewState["ConnStr"].ToString();
            }
            set
            {
                _connStr = value;
                ViewState["ConnStr"] = _connStr;
                ;
            }
        }

        #endregion

        #region "Operations"

        /// <summary>
        /// </summary>
        public override void DataBind()
        {
            // Get attachment data
            if (ProductCode != null && LocationCode != null)
            {
                var getPrDtStockSum = prDt.GetStockSummary(dsPrDtStockSum, ProductCode, LocationCode, CurrentDate,
                    ConnStr);

                if (getPrDtStockSum)
                {
                    if (dsPrDtStockSum.Tables[prDt.TableName].Rows.Count > 0)
                    {
                        var drStockSummary = dsPrDtStockSum.Tables[prDt.TableName].Rows[0];

                        lbl_OnHand.Text = string.Format(basePage.DefaultQtyFmt, drStockSummary["OnHand"]);
                        lbl_OnOrder.Text = String.Format(basePage.DefaultQtyFmt, drStockSummary["OnOrder"]);
                        lbl_Reorder.Text = String.Format(basePage.DefaultQtyFmt, drStockSummary["Reorder"]);
                        lbl_Restock.Text = string.Format(basePage.DefaultQtyFmt, drStockSummary["Restock"]);
                        lbl_LastPrice.Text = String.Format(basePage.DefaultAmtFmt, drStockSummary["LastPrice"]);
                        lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                    }
                }
            }
        }

        #endregion
    }
}