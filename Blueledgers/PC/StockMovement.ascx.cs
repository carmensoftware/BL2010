using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxClasses.Internal;

namespace BlueLedger.PL.PC
{
    public partial class StockMovement : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet dsStockMovement = new DataSet();
        //BL.PC.PR.PRDt prDt      = new BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();

        private readonly BasePage basePage = new BasePage();

        private string _connStr;
        private string _dtNo;
        private string _hdrNo;
        private decimal _total;

        public string HdrNo
        {
            get
            {
                if (ViewState["HdrNo"] == null)
                {
                    return _hdrNo;
                }

                return ViewState["HdrNo"].ToString();
            }
            set
            {
                _hdrNo = value;
                ViewState["HdrNo"] = _hdrNo;
            }
        }

        public string DtNo
        {
            get
            {
                if (ViewState["DtNo"] == null)
                {
                    return _dtNo;
                }

                return ViewState["DtNo"].ToString();
            }
            set
            {
                _dtNo = value;
                ViewState["DtNo"] = _dtNo;
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

        public override void DataBind()
        {
            base.DataBind();

            Page_Retrieve();
        }

        private void Page_Retrieve()
        {
            //dsStockMovement.Clear();

            inv.GetStockMovement(dsStockMovement, HdrNo, DtNo, ConnStr);

            //Session["dsStockMovement"] = dsStockMovement;

            Page_Setting();
        }

        #region "Operations"

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            //dsStockMovement = (DataSet)Session["dsStockMovement"];

            basePage.SetDefault();

            // GridView
            ((BoundField)grd_StockMovement.Columns[4]).DataFormatString = basePage.DefaultQtyFmt;
            ((BoundField)grd_StockMovement.Columns[5]).DataFormatString = basePage.DefaultQtyFmt;
            // Get attachment data
            grd_StockMovement.DataSource = dsStockMovement.Tables[inv.TableName];
            grd_StockMovement.DataBind();
        }

        protected void grd_StockMovement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_CommitDate") != null)
                {
                    var lbl_CommitDate = e.Row.FindControl("lbl_CommitDate") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "CommittedDate").ToString() != string.Empty)
                    {
                        lbl_CommitDate.Text =
                            DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CommittedDate").ToString())
                                .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                        lbl_CommitDate.ToolTip = lbl_CommitDate.Text;
                    }
                }

                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl_Location = e.Row.FindControl("lbl_Location") as Label;
                    lbl_Location.Text = DataBinder.Eval(e.Row.DataItem, "Location") + " : "
                                        +
                                        storeLct.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(), ConnStr);
                    lbl_Location.ToolTip = lbl_Location.Text;
                }

                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl_Product = e.Row.FindControl("lbl_Product") as Label;
                    lbl_Product.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                       product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                           ConnStr) + " : " +
                                       product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                           ConnStr);
                    lbl_Product.ToolTip = lbl_Product.Text;
                }

                if (e.Row.FindControl("lbl_Amount") != null)
                {

                    decimal inQty = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "IN").ToString());
                    decimal outQty = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "OUT").ToString());
                    decimal cost = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "Amount").ToString());
                    decimal amount = basePage.RoundAmt((inQty + outQty) * cost);

                    var lbl_Amount = e.Row.FindControl("lbl_Amount") as Label;
                    lbl_Amount.Text = string.Format(basePage.DefaultAmtFmt, amount);
                    lbl_Amount.ToolTip = lbl_Amount.Text;

                    _total = _total + amount;
                }

                if (e.Row.FindControl("lbl_RefNo") != null)
                {
                    var label = e.Row.FindControl("lbl_RefNo") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "RefNo").ToString();

                    label.ToolTip = label.Text;
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int i = grd_StockMovement.Columns.Count - 2; // Last Column
                e.Row.Cells[i - 1].Text = "Total";
                e.Row.Cells[i].Text = string.Format(basePage.DefaultAmtFmt, _total); ;
                _total = 0;

            }
        }


        #endregion
    }
}