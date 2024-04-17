using System;
using System.Collections.Generic;

using BlueLedger.PL.BaseClass;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace BlueLedger.PL.UserControls
{
    public partial class TotalSummary : BaseUserControl
    {
        private readonly BasePage basePage = new BasePage();

        #region --Attributes--

        // Private
        private decimal _currNetAmt;
        private decimal _currTaxAmt;
        private decimal _currTotalAmt;
        private decimal _netAmt;
        private decimal _taxAmt;
        private decimal _totalAmt;

        // Public
        public decimal CurrencyNetAmount
        {
            set
            {
                lb_CurrNetAmt.Text = FormatAmt(value);
            }
        }
        public decimal CurrencyTaxAmount
        {
            set
            {
                lb_CurrTaxAmt.Text = FormatAmt(value);
            }
        }

        public decimal CurrencyTotalAmount
        {
            set
            {
                lb_CurrTotalAmt.Text = FormatAmt(value);
            }
        }

        public decimal NetAmount
        {
            set
            {
                lb_NetAmt.Text = FormatAmt(value);
            }
        }

        public decimal TaxAmount
        {
            set
            {
                lb_TaxAmt.Text = FormatAmt(value);
            }
        }

        public decimal TotalAmount
        {
            set
            {
                lb_TotalAmt.Text = FormatAmt(value);
            }
        }


        #endregion

        private string FormatAmt(decimal value)
        {
            return string.Format("{0:N" + basePage.DefaultAmtDigit.ToString() + "}", value);
        }


       
    }
}

