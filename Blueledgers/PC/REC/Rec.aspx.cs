using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.REC
{
    public partial class REC : BasePage
    {
        #region "Attributes"

        const string recExtCost = "PC.RecExtCost";

        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private readonly Blue.BL.Option.Inventory.DeliveryPoint deliveryPoint = new Blue.BL.Option.Inventory.DeliveryPoint();

        private readonly DataSet dsPO = new DataSet();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        private readonly Blue.BL.PC.PO.PO po = new Blue.BL.PC.PO.PO();
        private readonly Blue.BL.PC.PO.PoDt poDt = new Blue.BL.PC.PO.PoDt();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private string MsgError = string.Empty;
        private Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private DataSet dsPrDtStockSum = new DataSet();
        private DataSet dsRec = new DataSet();
        private Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private decimal grandAmt;
        private decimal grandNetAmt;
        private decimal grandRecAmt;
        private decimal grandTaxAmt;
        private int index;
        private string recNo = string.Empty;

        // Added on: 25/08/2017, By: Fon
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "2.3";
        private decimal currGrandNetAmt;
        private decimal currGrandTaxAmt;
        private decimal currGrandTotal;
        private string baseCurrency
        {
            get { return config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value); }
        }
        // End Added.
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
        }

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
                dsRec = (DataSet)Session["dsRec"];
            }

            //this.Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                recNo = Request.Params["ID"];

                var getStrLct = rec.GetListByRecNo(dsTmp, ref MsgError, recNo, hf_ConnStr.Value);

                if (getStrLct)
                {
                    dsRec = dsTmp;

                    recDt.GetListByRecNo(dsRec, recNo, hf_ConnStr.Value);
                }
                else
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }


                // Create Schema for ExtraCost
                string sql = "SELECT d.TypeName, h.Amount";
                sql += " FROM PC.RecExtCost h";
                sql += " JOIN PC.ExtCostType d ON d.TypeId = h.TypeId";
                sql += string.Format(" WHERE h.RecNo = '{0}'", recNo.ToString());
                sql += " ORDER BY h.DtNo";

                DataTable dt = new DataTable();
                dt = rec.DbExecuteQuery(sql, null, hf_ConnStr.Value);
                dt.TableName = recExtCost;
                dsRec.Tables.Add(dt);
            }

            Session["dsRec"] = dsRec;

            Page_Setting();

        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {


            if (dsRec.Tables[rec.TableName] != null)
            {
                var drRec = dsRec.Tables[rec.TableName].Rows[0];
                var recDate = Convert.ToDateTime(drRec["RecDate"]).Date;

                lbl_RecNo.Text = drRec["RecNo"].ToString();
                lbl_RecDate.Text = recDate.ToString(LoginInfo.BuFmtInfo.FmtSDate);
                //if (drRec["RecDate"].ToString() != string.Empty)
                //{
                //    lbl_RecDate.Text = DateTime.Parse(drRec["RecDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                //}

                lbl_DocStatus.Text = drRec["DocStatus"].ToString();

                var docStatus = lbl_DocStatus.Text.ToUpper();

                if (docStatus == "COMMITTED" || docStatus == "VOIDED")
                {
                    btn_Edit.Visible = false;
                    btn_Void.Visible = false;
                }

                btn_EditInvoice.Visible = docStatus == "COMMITTED";

                // Enable edit commit
                //var isAverage = config.GetValue("IN", "SYS", "COST", LoginInfo.ConnStr).ToUpper() == "AVCO";
                //var enableEditCommit = config.GetValue("APP", "SYS", "EnableEditCommit", LoginInfo.ConnStr) == "1";
                //var startPeriodDate = period.GetLatestOpenStartDate(LoginInfo.ConnStr);
                //var isValidDate = recDate >= startPeriodDate;

                //if (isValidDate && isAverage && enableEditCommit)
                //{
                //    btn_EditInvoice.Visible = false;

                //    btn_Edit.Visible = true;
                //    btn_Void.Visible = true;
                //}

                var isEdit = Helpers.Config.IsEnableEditCommit(recDate.Date, LoginInfo.ConnStr);

                if (isEdit)
                {
                    btn_EditInvoice.Visible = false;

                    btn_Edit.Visible = true;
                    btn_Void.Visible = true;
                }


                lbl_InvNo.Text = drRec["InvoiceNo"].ToString();

                if (drRec["InvoiceDate"].ToString() != string.Empty)
                {
                    lbl_InvDate.Text = DateTime.Parse(drRec["InvoiceDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                lbl_DeliPoint.Text = drRec["DeliPoint"] + " : " + deliveryPoint.GetName(drRec["DeliPoint"].ToString(), hf_ConnStr.Value);
                lbl_VendorCode.Text = drRec["VendorCode"] + " : ";
                lbl_VendorNm.Text = vendor.GetName(drRec["VendorCode"].ToString(), hf_ConnStr.Value);
                lbl_Currency.Text = drRec["CurrencyCode"].ToString();
                chk_IsCashConsign.Checked = drRec["IsCashConsign"].ToString() == "1";


                lbl_ExRateAudit.Text = drRec["CurrencyRate"].ToString();
                lbl_CommitDate.Text = (drRec["CommitDate"] != DBNull.Value)
                    ? Convert.ToDateTime(drRec["CommitDate"]).ToString("dd/MM/yyyy") 
                    : string.Empty;

                lbl_Desc.Text = drRec["Description"].ToString();
                lbl_Receiver.Text = drRec["CreatedBy"].ToString();

                if (drRec.Table.Columns.Contains("TotalExtraCost"))
                {
                    lbl_TotalExtraCost.Text = string.Format(DefaultAmtFmt, drRec["TotalExtraCost"]);
                    if (drRec["ExtraCostBy"].ToString().ToUpper() == "A")
                        lbl_ExtraCostBy.Text = "*Calculated by Amount";
                    else
                        lbl_ExtraCostBy.Text = "*Calculated by Quantity";
                }

                //Display Gird
                grd_RecEdit.DataSource = dsRec.Tables[recDt.TableName];
                grd_RecEdit.DataBind();


                //// Display ExtraCost
                grd_ExtraCost.DataSource = dsRec.Tables[recExtCost];
                grd_ExtraCost.DataBind();

            }

            // Display Total
            var dt = dsRec.Tables[recDt.TableName].AsEnumerable();


            TotalSummary.CurrencyNetAmount = dt.Sum(x => x.Field<decimal>("CurrNetAmt"));
            TotalSummary.CurrencyTaxAmount = dt.Sum(x => x.Field<decimal>("CurrTaxAmt"));
            TotalSummary.CurrencyTotalAmount = dt.Sum(x => x.Field<decimal>("CurrTotalAmt"));

            TotalSummary.NetAmount = dt.Sum(x => x.Field<decimal>("NetAmt"));
            TotalSummary.TaxAmount = dt.Sum(x => x.Field<decimal>("TaxAmt"));
            TotalSummary.TotalAmount = dt.Sum(x => x.Field<decimal>("TotalAmt"));


            // Display Comment.           
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "PC";
            comment.SubModule = "Rec";
            comment.RefNo = dsRec.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "PC";
            attach.RefNo = dsRec.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "PC";
            log.SubModule = "REC";
            log.RefNo = dsRec.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
            log.Visible = true;
            log.DataBind();

            Control_HeaderMenuBar();
        }

        // Added on: 03/10/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            btn_Edit.Visible = (pagePermiss >= 3) ? btn_Edit.Visible : false;
            btn_Void.Visible = (pagePermiss >= 7) ? btn_Void.Visible : false;
        }
        // End Added.

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RecEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find Control Image Button
                var Img_Btn = e.Row.FindControl("Img_Btn") as ImageButton;

                var productCode = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString().Trim();

                //set CommandArgument For ImageButton
                if (e.Row.FindControl("Img_Btn") != null)
                {
                    Img_Btn.CommandArgument = index.ToString();
                    index = index + 1;
                }

                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl_Location = e.Row.FindControl("lbl_Location") as Label;
                    lbl_Location.Text = locat.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                        hf_ConnStr.Value);

                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), hf_ConnStr.Value);
                    lbl_Location.ToolTip = DataBinder.Eval(e.Row.DataItem, "LocationCode") + " : " + lbl_Location.Text;
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = productCode + " : " + product.GetName(productCode, hf_ConnStr.Value) + " : "
                                           + product.GetName2(productCode, hf_ConnStr.Value);
                    lbl_ProductCode.ToolTip = productCode + " : " + lbl_ProductCode.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("lbl_RcvUnit") != null)
                {
                    var lbl_RcvUnit = e.Row.FindControl("lbl_RcvUnit") as Label;
                    lbl_RcvUnit.Text = DataBinder.Eval(e.Row.DataItem, "RcvUnit").ToString();
                    lbl_RcvUnit.ToolTip = lbl_RcvUnit.Text;
                }

                if (e.Row.FindControl("lbl_RcvUnit_Expand") != null)
                {
                    var lbl_RcvUnit_Expand = e.Row.FindControl("lbl_RcvUnit_Expand") as Label;
                    lbl_RcvUnit_Expand.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                    lbl_RcvUnit_Expand.ToolTip = lbl_RcvUnit_Expand.Text;
                }

                if (e.Row.FindControl("lbl_OrderQty") != null)
                {
                    var lbl_OrderQty = e.Row.FindControl("lbl_OrderQty") as Label;
                    lbl_OrderQty.Text = String.Format(DefaultQtyFmt,
                        (DataBinder.Eval(e.Row.DataItem, "OrderQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OrderQty").ToString())));
                    lbl_OrderQty.ToolTip = lbl_OrderQty.Text;
                }

                if (e.Row.FindControl("lbl_RecQty") != null)
                {
                    var lbl_RecQty = e.Row.FindControl("lbl_RecQty") as Label;
                    lbl_RecQty.Text = String.Format(DefaultQtyFmt,
                        (DataBinder.Eval(e.Row.DataItem, "RecQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString())));
                    lbl_RecQty.ToolTip = lbl_RecQty.Text;
                    grandRecAmt += Convert.ToDecimal(lbl_RecQty.Text);
                }

                if (e.Row.FindControl("lbl_FocQty") != null)
                {
                    var lbl_FocQty = e.Row.FindControl("lbl_FocQty") as Label;
                    lbl_FocQty.Text = String.Format(DefaultQtyFmt,
                        (DataBinder.Eval(e.Row.DataItem, "FOCQty") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FOCQty").ToString())));
                    lbl_FocQty.ToolTip = lbl_FocQty.Text;
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    lbl_Price.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "Price") == DBNull.Value
                            ? 0
                            : DataBinder.Eval(e.Row.DataItem, "Price")));
                    lbl_Price.ToolTip = lbl_Price.Text;
                }

                if (e.Row.FindControl("lbl_ExtraCost") != null)
                {
                    var lbl_ExtraCost = e.Row.FindControl("lbl_ExtraCost") as Label;
                    lbl_ExtraCost.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "ExtraCost") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ExtraCost").ToString())));
                    lbl_ExtraCost.ToolTip = lbl_ExtraCost.Text;
                }
                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl_TaxAmt = e.Row.FindControl("lbl_TaxAmt") as Label;
                    lbl_TaxAmt.Text = String.Format(DefaultAmtFmt,
                        (DataBinder.Eval(e.Row.DataItem, "TaxAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt").ToString())));
                    lbl_TaxAmt.ToolTip = lbl_TaxAmt.Text;
                    grandTaxAmt += Convert.ToDecimal(lbl_TaxAmt.Text);
                }


                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format(DefaultAmtFmt,
                        // Modified on: 28/08/2017
                        //(DataBinder.Eval(e.Row.DataItem, "TotalAmt") == DBNull.Value
                        //    ? 0
                        //    : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt").ToString())));
                        (DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt") == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString())));

                    lbl_TotalAmt.ToolTip = lbl_TotalAmt.Text;
                    //grandAmt += Convert.ToDecimal(lbl_TotalAmt.Text) * Convert.ToDecimal(lbl_ExRateAudit.Text);
                }

                if (e.Row.FindControl("chk_TaxAdj") != null)
                {
                    var chk_TaxAdj = e.Row.FindControl("chk_TaxAdj") as CheckBox;
                    chk_TaxAdj.Checked =
                        Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj") == DBNull.Value
                            ? false
                            : Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "TaxAdj")));
                }

                if (e.Row.FindControl("lbl_TaxType_Row") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType_Row") as Label;
                    lbl_TaxType.Text = "(" +
                                       Blue.BL.GnxLib.GetTaxTypeName(
                                           DataBinder.Eval(e.Row.DataItem, "TaxType").ToString()) + ")";
                    lbl_TaxType.ToolTip = lbl_TaxType.Text;
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = e.Row.FindControl("lbl_Status") as Label;
                    lbl_Status.Text = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                    lbl_Status.ToolTip = lbl_Status.Text;
                }

                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    //Label lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    //lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                //-------------------------------Expand----------------------------------------
                if (e.Row.FindControl("lbl_Receive") != null)
                {
                    var lbl_Receive = e.Row.FindControl("lbl_Receive") as Label;
                    lbl_Receive.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "RecQty"));
                    lbl_Receive.ToolTip = lbl_Receive.Text;
                }

                if (e.Row.FindControl("chk_DiscAdj") != null)
                {
                    var chk_DiscAdj = e.Row.FindControl("chk_DiscAdj") as CheckBox;
                    chk_DiscAdj.Checked =
                        Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DiscAdj") == DBNull.Value
                            ? false
                            : Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DiscAdj")));
                }

                if (e.Row.FindControl("lbl_Disc") != null)
                {
                    var lbl_Disc = e.Row.FindControl("lbl_Disc") as Label;
                    lbl_Disc.Text = DataBinder.Eval(e.Row.DataItem, "Discount").ToString();
                    lbl_Disc.ToolTip = lbl_Disc.Text;
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiccountAmt"));
                    lbl_DiscAmt.ToolTip = lbl_DiscAmt.Text;
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text =
                        Blue.BL.GnxLib.GetTaxTypeName(DataBinder.Eval(e.Row.DataItem, "TaxType").ToString());
                    lbl_TaxType.ToolTip = lbl_TaxType.Text;
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                    lbl_TaxRate.ToolTip = lbl_TaxRate.Text;
                }

                if (e.Row.FindControl("lbl_PrRef") != null)
                {
                    var lbl_PrRef = e.Row.FindControl("lbl_PrRef") as Label;

                    var dtPr = new DataTable();

                    if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "PoNo").ToString()))
                        dtPr = prDt.GetByPONoPODt(DataBinder.Eval(e.Row.DataItem, "PoNo").ToString(), int.Parse(DataBinder.Eval(e.Row.DataItem, "PoDtNo").ToString()), hf_ConnStr.Value);

                    if (dtPr.Rows.Count > 0)
                    {
                        foreach (DataRow drPr in dtPr.Rows)
                        {
                            if (lbl_PrRef.Text == string.Empty)
                            {
                                lbl_PrRef.Text = drPr["PRNo1"].ToString();
                            }
                            else
                            {
                                lbl_PrRef.Text = lbl_PrRef.Text + ", " + drPr["PRNo1"];
                            }
                        }
                    }

                    lbl_PrRef.ToolTip = lbl_PrRef.Text;

                    if (e.Row.FindControl("grd_PR") != null)
                    {
                        var grd_PR = e.Row.FindControl("grd_PR") as GridView;
                        grd_PR.DataSource = dtPr;
                        grd_PR.DataBind();
                    }
                }

                if (e.Row.FindControl("hpl_PoRef") != null)
                {
                    var hpl_PoRef = e.Row.FindControl("hpl_PoRef") as HyperLink;
                    hpl_PoRef.NavigateUrl = "~/PC/PO/PO.aspx?BuCode=" + dsRec.Tables[rec.TableName].Rows[0]["PoSource"] +
                                            "&ID=" + DataBinder.Eval(e.Row.DataItem, "PoNo");
                    hpl_PoRef.Text = DataBinder.Eval(e.Row.DataItem, "PoNo").ToString();
                    hpl_PoRef.ToolTip = hpl_PoRef.Text;
                }

                if (e.Row.FindControl("lbl_Avg") != null)
                {
                    var startDate = period.GetStartDate(ServerDateTime.Date, hf_ConnStr.Value);
                    var endDate = period.GetEndDate(ServerDateTime.Date, hf_ConnStr.Value);

                    var lbl_Avg = e.Row.FindControl("lbl_Avg") as Label;
                    lbl_Avg.Text = String.Format(DefaultAmtFmt,
                        inv.GetPAvgAudit(startDate, endDate, DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                            productCode, hf_ConnStr.Value));
                    lbl_Avg.ToolTip = lbl_Avg.Text;
                }

                if (dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode &&
                    dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != null)
                {
                    if (e.Row.FindControl("lbl_BuCodeText") != null)
                    {
                        var lbl_BuCodeText = e.Row.FindControl("lbl_BuCodeText") as Label;
                        lbl_BuCodeText.Visible = true;
                    }

                    if (e.Row.FindControl("lbl_BuCode") != null)
                    {
                        var lbl_BuCode = e.Row.FindControl("lbl_BuCode") as Label;
                        lbl_BuCode.Text = LoginInfo.BuInfo.BuCode;
                        lbl_BuCode.Visible = true;
                    }
                }

                if (e.Row.FindControl("lbl_NetAmtDt") != null)
                {
                    var lbl_NetAmtDt = e.Row.FindControl("lbl_NetAmtDt") as Label;
                    lbl_NetAmtDt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                    lbl_NetAmtDt.ToolTip = lbl_NetAmtDt.Text;
                    grandNetAmt += Convert.ToDecimal(lbl_NetAmtDt.Text);
                }

                if (e.Row.FindControl("lbl_TaxAmtDt") != null)
                {
                    var lbl_TaxAmtDt = e.Row.FindControl("lbl_TaxAmtDt") as Label;
                    lbl_TaxAmtDt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    lbl_TaxAmtDt.ToolTip = lbl_TaxAmtDt.Text;
                    grandTaxAmt += Convert.ToDecimal(lbl_TaxAmtDt.Text);
                }

                if (e.Row.FindControl("lbl_TotalAmtDt") != null)
                {
                    var lbl_TotalAmtDt = e.Row.FindControl("lbl_TotalAmtDt") as Label;
                    lbl_TotalAmtDt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_TotalAmtDt.ToolTip = lbl_TotalAmtDt.Text;
                }

                if (e.Row.FindControl("lbl_Receiver") != null)
                {
                    var lbl_Receiver = e.Row.FindControl("lbl_Receiver") as Label;
                    lbl_Receiver.Text = dsRec.Tables[rec.TableName].Rows[0]["CreatedBy"].ToString();
                    lbl_Receiver.ToolTip = lbl_Receiver.Text;
                }

                if (e.Row.FindControl("lbl_ConvertRate") != null)
                {
                    var lbl_ConvertRate = e.Row.FindControl("lbl_ConvertRate") as Label;
                    lbl_ConvertRate.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Rate"));
                    lbl_ConvertRate.ToolTip = lbl_ConvertRate.Text;
                }

                if (e.Row.FindControl("lbl_BaseUnit") != null)
                {
                    var lbl_BaseUnit = e.Row.FindControl("lbl_BaseUnit") as Label;
                    lbl_BaseUnit.Text = prodUnit.GetInvenUnit(productCode, hf_ConnStr.Value);
                    lbl_BaseUnit.ToolTip = lbl_BaseUnit.Text;
                }

                if (e.Row.FindControl("lbl_BaseQty") != null)
                {
                    var lbl_BaseQty = e.Row.FindControl("lbl_BaseQty") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "Rate").ToString() != string.Empty)
                    {
                        lbl_BaseQty.Text = String.Format(DefaultQtyFmt, prodUnit.GetQtyAfterChangeUnit(productCode,
                            prodUnit.GetInvenUnit(productCode, hf_ConnStr.Value),
                            DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString(),
                            Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty").ToString()),
                            hf_ConnStr.Value));
                    }

                    lbl_BaseQty.ToolTip = lbl_BaseQty.Text;
                }

                if (e.Row.FindControl("lbl_DtrComment") != null)
                {
                    var lbl_DtrComment = e.Row.FindControl("lbl_DtrComment") as Label;
                    lbl_DtrComment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_DtrComment.ToolTip = lbl_DtrComment.Text;
                }

                //if (e.Row.FindControl("lbl_NetAcc") != null)
                //{
                //    var lbl_NetAcc = e.Row.FindControl("lbl_NetAcc") as Label;
                //    lbl_NetAcc.Text = DataBinder.Eval(e.Row.DataItem, "NetDrAcc").ToString();
                //}

                //if (e.Row.FindControl("lbl_TaxAcc") != null)
                //{
                //    var lbl_TaxAcc = e.Row.FindControl("lbl_TaxAcc") as Label;
                //    lbl_TaxAcc.Text = DataBinder.Eval(e.Row.DataItem, "TaxDrAcc").ToString();
                //}

                #region About Currency
                decimal currNetAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrNetAmt") != DBNull.Value)
                    currNetAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt").ToString());

                decimal currDiscAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt") != DBNull.Value)
                    currDiscAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrDiscAmt").ToString());

                decimal currTaxAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt") != DBNull.Value)
                    currTaxAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt").ToString());

                decimal currTotalAmt = 0;
                if (DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt") != DBNull.Value)
                    currTotalAmt = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt").ToString());

                // Title
                if (e.Row.FindControl("lbl_CurrCurrDt") != null)
                {
                    Label lbl_CurrCurrDt = (Label)e.Row.FindControl("lbl_CurrCurrDt");
                    lbl_CurrCurrDt.Text = string.Format("( {0} )", lbl_Currency.Text);
                }

                if (e.Row.FindControl("lbl_BaseCurrDt") != null)
                {
                    Label lbl_BaseCurrDt = (Label)e.Row.FindControl("lbl_BaseCurrDt");
                    lbl_BaseCurrDt.Text = string.Format("( {0} )", baseCurrency);
                }

                // Net Amount
                currGrandNetAmt += currNetAmt;
                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    Label lbl_CurrNetAmt = (Label)e.Row.FindControl("lbl_CurrNetAmt");
                    lbl_CurrNetAmt.Text = string.Format(DefaultAmtFmt, currNetAmt);
                }

                // Disc Amount
                if (e.Row.FindControl("lbl_CurrDiscAmt") != null)
                {
                    Label lbl_CurrDiscAmt = (Label)e.Row.FindControl("lbl_CurrDiscAmt");
                    lbl_CurrDiscAmt.Text = string.Format(DefaultAmtFmt, currDiscAmt);
                }

                //  Tax Amount
                currGrandTaxAmt += currTaxAmt;
                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    Label lbl_CurrTaxAmt = (Label)e.Row.FindControl("lbl_CurrTaxAmt");
                    lbl_CurrTaxAmt.Text = string.Format(DefaultAmtFmt, currTaxAmt);
                }

                // Total Amount
                currGrandTotal = currGrandNetAmt + currGrandTaxAmt;
                if (e.Row.FindControl("lbl_CurrTotalAmtDt") != null)
                {
                    Label lbl_CurrTotalAmtDt = (Label)e.Row.FindControl("lbl_CurrTotalAmtDt");
                    lbl_CurrTotalAmtDt.Text = string.Format(DefaultAmtFmt, currTotalAmt);
                }

                if (e.Row.FindControl("lbl_ExpiryDate") != null)
                {
                    var lbl_ExpiryDate = e.Row.FindControl("lbl_ExpiryDate") as Label;
                    if (string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "ExpiryDate").ToString()))
                        lbl_ExpiryDate.Text = string.Empty;
                    else
                        lbl_ExpiryDate.Text = string.Format("{0:d}", (DateTime)DataBinder.Eval(e.Row.DataItem, "ExpiryDate"));

                    lbl_ExpiryDate.ToolTip = lbl_ExpiryDate.Text;
                }


                #endregion




                //****************** Display Stock Movement *****************
                if (e.Row.FindControl("uc_StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("uc_StockMovement") as PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RecDtNo").ToString();
                    uc_StockMovement.ConnStr = hf_ConnStr.Value;
                    uc_StockMovement.DataBind();
                }

                if (dsRec.Tables[prDt.TableName] != null)
                {
                    dsRec.Tables[prDt.TableName].Clear();
                }

                var get = prDt.GetStockSummary(dsRec, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(),
                    lbl_RecDate.Text, LoginInfo.ConnStr);
                if (get)
                {
                    var drStockSummary = dsRec.Tables[prDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;
                        if (drStockSummary["OnHand"].ToString() != string.Empty)
                        {
                            lbl_OnHand.Text = String.Format(DefaultQtyFmt, decimal.Parse(drStockSummary["OnHand"].ToString()));
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
                            lbl_OnOrder.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["OnOrder"].ToString()));
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
                            lbl_Reorder.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["Reorder"].ToString()));
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

                        if (drStockSummary["ReStock"].ToString() != string.Empty && drStockSummary["ReStock"] != null)
                        {
                            lbl_Restock.Text = String.Format(DefaultQtyFmt,
                                decimal.Parse(drStockSummary["ReStock"].ToString()));
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
                            lbl_LastPrice.Text = String.Format(DefaultAmtFmt,
                                decimal.Parse(drStockSummary["LastPrice"].ToString()));
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
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.FindControl("lbl_TotalTax") != null)
                {
                    var lbl_TotalTax = e.Row.FindControl("lbl_TotalTax") as Label;
                    lbl_TotalTax.Text = String.Format(DefaultAmtFmt, grandTaxAmt);
                }

                if (e.Row.FindControl("lbl_TotalRec") != null)
                {
                    var lbl_TotalRec = e.Row.FindControl("lbl_TotalRec") as Label;
                    lbl_TotalRec.Text = String.Format(DefaultAmtFmt, grandNetAmt);
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_TotalAmt = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_TotalAmt.Text = String.Format(DefaultAmtFmt, grandNetAmt + grandTaxAmt);
                }


                // Added on: 25/08/2017, By: Fon
                if (e.Row.FindControl("lbl_CurrGrandTitle") != null)
                {
                    Label lbl_CurrGrandTitle = (Label)e.Row.FindControl("lbl_CurrGrandTitle");
                    lbl_CurrGrandTitle.Text = string.Format("( {0} )", lbl_Currency.Text);
                }
                if (e.Row.FindControl("lbl_BaseGrandTitle") != null)
                {
                    Label lbl_BaseGrandTitle = (Label)e.Row.FindControl("lbl_BaseGrandTitle");
                    lbl_BaseGrandTitle.Text = string.Format("( {0} )", baseCurrency);
                }

                if (e.Row.FindControl("lbl_CurrGrandTotalNet") != null)
                {
                    Label lbl_CurrGrandTotalNet = (Label)e.Row.FindControl("lbl_CurrGrandTotalNet");
                    lbl_CurrGrandTotalNet.Text = string.Format(DefaultAmtFmt, currGrandNetAmt);
                }

                if (e.Row.FindControl("lbl_CurrGrandTotalTax") != null)
                {
                    Label lbl_CurrGrandTotalTax = (Label)e.Row.FindControl("lbl_CurrGrandTotalTax");
                    lbl_CurrGrandTotalTax.Text = string.Format(DefaultAmtFmt, currGrandTaxAmt);
                }

                if (e.Row.FindControl("lbl_CurrGrandTotalAmt") != null)
                {
                    Label lbl_CurrGrandTotalAmt = (Label)e.Row.FindControl("lbl_CurrGrandTotalAmt");
                    lbl_CurrGrandTotalAmt.Text = string.Format(DefaultAmtFmt, currGrandTotal);
                }
                // End Added.
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RecEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                var Img_Btn =
                    grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as
                        ImageButton;

                if (Img_Btn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
                {
                    var p_DetailRows =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
                    p_DetailRows.Visible = true;

                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

                    var drExpand =
                        dsRec.Tables[recDt.TableName].Rows[
                            grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];

                    var errorMsg = string.Empty;

                    // Display Transaction Detail ---------------------------------------------------------
                    var lbl_Disc =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Disc") as Label;
                    lbl_Disc.Text = drExpand["Discount"].ToString();

                    var lbl_DiscAmt =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format(DefaultAmtFmt, decimal.Parse(drExpand["DiccountAmt"].ToString()));

                    var lbl_TaxType =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text = Blue.BL.GnxLib.GetTaxTypeName(drExpand["TaxType"].ToString());

                    var lbl_TaxRate =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = string.Format("{0:N}", drExpand["TaxRate"].ToString());

                    var lbl_PrRef =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_PrRef") as Label;
                    lbl_PrRef.Text = drExpand["PrNo"].ToString();

                    var lbl_PoRef =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_PoRef") as Label;
                    lbl_PoRef.Text = drExpand["PoNo"].ToString();

                    if (dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode &&
                        dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != null)
                    {
                        var lbl_BuCodeText =
                            grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_BuCodeText") as
                                Label;
                        lbl_BuCodeText.Visible = true;

                        var lbl_BuCode =
                            grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_BuCode") as Label;
                        lbl_BuCode.Text = LoginInfo.BuInfo.BuCode;
                        lbl_BuCode.Visible = true;
                    }

                    var lbl_NetAmtDt =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_NetAmtDt") as Label;
                    lbl_NetAmtDt.Text = String.Format(DefaultAmtFmt, decimal.Parse(drExpand["NetAmt"].ToString()));

                    var lbl_TaxAmtDt =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxAmtDt") as Label;
                    lbl_TaxAmtDt.Text = String.Format(DefaultAmtFmt, decimal.Parse(drExpand["TaxAmt"].ToString()));

                    var lbl_TotalAmtDt =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TotalAmtDt") as Label;
                    lbl_TotalAmtDt.Text = String.Format(DefaultAmtFmt, decimal.Parse(drExpand["TotalAmt"].ToString()));

                    var lbl_Receiver =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Receiver") as Label;
                    lbl_Receiver.Text = dsRec.Tables[rec.TableName].Rows[0]["CreatedBy"].ToString();

                    var lbl_DtrComment =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_DtrComment") as Label;
                    lbl_DtrComment.Text = drExpand["Comment"].ToString();

                    // Display Stock Summary --------------------------------------------------------------  

                    //****************** Display Stock Summary ******************
                    if (grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") != null)
                    {
                        var uc_StockSummary =
                            grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uc_StockSummary") as
                                BlueLedger.PL.PC.StockSummary;

                        uc_StockSummary.ProductCode = drExpand["ProductCode"].ToString();
                        uc_StockSummary.LocationCode = drExpand["LocationCode"].ToString();
                        uc_StockSummary.ConnStr = LoginInfo.ConnStr;
                        uc_StockSummary.DataBind();
                    }
                }
                else
                {
                    var p_DetailRows =
                        grd_RecEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

                    p_DetailRows.Visible = false;
                    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimVoid_Click(object sender, EventArgs e)
        {
            Void();

            pop_ConfrimVoid.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        private void Void()
        {
            //Using for updating inventory
            inv.GetListByHdrNo(dsRec, ref MsgError, lbl_RecNo.Text, hf_ConnStr.Value);

            var recNo = dsRec.Tables[rec.TableName].Rows[0]["RecNo"].ToString();


            foreach (DataRow drRecDt in dsRec.Tables[recDt.TableName].Rows)
            {
                //recNo = drRecDt["RecNo"].ToString();

                var OnHand = recDt.GetInventoryOnHandAfterVoid(drRecDt["RecNo"].ToString(),
                    drRecDt["RecDtNo"].ToString(), hf_ConnStr.Value);

                if (OnHand < 0)
                {
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }

                drRecDt["Status"] = "Voided";

                drRecDt["NetAmt"] = Convert.ToDecimal("0");
                drRecDt["TaxAmt"] = Convert.ToDecimal("0");
                drRecDt["DiccountAmt"] = Convert.ToDecimal("0");
                drRecDt["TotalAmt"] = Convert.ToDecimal("0");

                drRecDt["CurrNetAmt"] = Convert.ToDecimal("0");
                drRecDt["CurrTaxAmt"] = Convert.ToDecimal("0");
                drRecDt["CurrDiscAmt"] = Convert.ToDecimal("0");
                drRecDt["CurrTotalAmt"] = Convert.ToDecimal("0");

                drRecDt["ExtraCost"] = 0;


                //Update Inventory
                //for (var i = dsRec.Tables[inv.TableName].Rows.Count - 1; i >= 0; i--)
                //{
                //    if (dsRec.Tables[inv.TableName].Rows[i].RowState != DataRowState.Deleted)
                //    {
                //        if (drRecDt["RecNo"].ToString() == dsRec.Tables[inv.TableName].Rows[i]["HdrNo"].ToString()
                //            && drRecDt["RecDtNo"].ToString() == dsRec.Tables[inv.TableName].Rows[i]["DtNo"].ToString())
                //        {
                //            dsRec.Tables[inv.TableName].Rows[i].Delete();
                //        }
                //    }
                //}

            }

            dsRec.Tables[rec.TableName].Rows[0]["DocStatus"] = "Voided";

            var Save = rec.Save(dsRec, hf_ConnStr.Value);

            if (Save)
            {
                // Remove transaction in [IN].Inventory
                DeleteInventory(recNo);

                string _remark = string.Empty;

                decimal TotalRcvQty = 0;
                decimal TotalCancelQty = 0;

                if (dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != string.Empty &&
                    dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString() != LoginInfo.BuInfo.BuCode)
                {
                    hf_ConnStr.Value = bu.GetConnectionString(dsRec.Tables[rec.TableName].Rows[0]["PoSource"].ToString());
                }

                foreach (DataRow drRecDt in dsRec.Tables[recDt.TableName].Rows)
                {
                    string poNo = drRecDt["PoNo"].ToString();

                    po.GetListByPoNo2(dsPO, ref MsgError, poNo, hf_ConnStr.Value);
                    poDt.GetPoDtByPoNo(dsPO, ref MsgError, poNo, hf_ConnStr.Value);

                    foreach (DataRow drPoDt in dsPO.Tables[poDt.TableName].Rows)
                    {
                        if (drRecDt["PoNo"].ToString() == drPoDt["PoNo"].ToString() &&
                            drRecDt["PoDtNo"].ToString() == drPoDt["PoDt"].ToString())
                        {
                            drPoDt["RcvQty"] = Convert.ToDecimal(drPoDt["RcvQty"].ToString() == string.Empty ? "0.00" : drPoDt["RcvQty"].ToString()) - Convert.ToDecimal(drRecDt["RecQty"].ToString());
                        }
                    }

                    //Check quantity in PoDt. If void full PO, PO status will change to Printed.
                    foreach (DataRow drPoDtCheck in dsPO.Tables[poDt.TableName].Rows)
                    {
                        if (drPoDtCheck.RowState != DataRowState.Deleted)
                        {
                            TotalRcvQty += Convert.ToDecimal(drPoDtCheck["RcvQty"].ToString());
                            TotalCancelQty += Convert.ToDecimal(drPoDtCheck["CancelQty"].ToString());
                        }
                    }

                    if (drRecDt["PoNo"].ToString() != "")
                    {
                        if (TotalCancelQty == 0 && TotalRcvQty == 0)
                        {
                            dsPO.Tables[po.TableName].Rows[0]["DocStatus"] = "Printed";
                        }
                        else
                        {
                            dsPO.Tables[po.TableName].Rows[0]["DocStatus"] = "Partial";
                        }
                    }

                    po.Save(dsPO, hf_ConnStr.Value);

                    TotalRcvQty = 0;
                    TotalCancelQty = 0;
                    dsPO.Clear();

                }

                hf_ConnStr.Value = bu.GetConnectionString(LoginInfo.BuInfo.BuCode);

                _transLog.Save("PC", "REC", recNo, "VOIDED", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

                Response.Redirect("~/PC/REC/RecLst.aspx");
            }
        }


        protected void btn_EditInvoice_Click(object sender, EventArgs e)
        {
            //de_InvDateEdit.Date = DateTime.Today;
            de_InvDateEdit.Date = Convert.ToDateTime(lbl_InvDate.Text);
            txt_InvNoEdit.Text = lbl_InvNo.Text;
            pop_EditInvoice.ShowOnPageLoad = true;
        }

        protected void btn_pop_EditInvoice_Save_Click(object sender, EventArgs e)
        {

            var drRec = dsRec.Tables[rec.TableName].Rows[0];

            DateTime old_InvoiceDate = (DateTime)drRec["InvoiceDate"];
            DateTime new_InvoiceDate = de_InvDateEdit.Date;

            string old_InvoiceNo = drRec["InvoiceNo"].ToString();
            string new_InvoiceNo = txt_InvNoEdit.Text;

            if (old_InvoiceDate != new_InvoiceDate || old_InvoiceNo != new_InvoiceNo)
            {
                string recNo = drRec["RecNo"].ToString();
                string sql = string.Format("UPDATE PC.REC SET InvoiceDate = '{0}', InvoiceNo = '{1}' WHERE RecNo = '{2}'", String.Format("{0:yyyy-MM-dd}", new_InvoiceDate), new_InvoiceNo, recNo);
                AlertMessageBox(sql);
                rec.DbParseQuery(sql, null, LoginInfo.ConnStr);
                //AlertMessageBox("Invoice is saved.");

                string comment = string.Format("Change Invoice date from {0} to {1} and Invoice No. from {2} to {3}", old_InvoiceDate.ToString("yyyy-MM-dd"), new_InvoiceDate.ToString("yyyy-MM-dd"), old_InvoiceNo, new_InvoiceNo);
                _transLog.Save("PC", "REC", recNo, "MODIFY", comment, LoginInfo.LoginName, hf_ConnStr.Value);

                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            pop_EditInvoice.ShowOnPageLoad = false;


        }
        protected void btn_pop_EditInvoice_Cancel_Click(object sender, EventArgs e)
        {
            pop_EditInvoice.ShowOnPageLoad = false;
        }


        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            var poNo = dsRec.Tables[rec.TableName].Rows[0]["PoSource"];
            var rcvNo = dsRec.Tables[rec.TableName].Rows[0]["RecNo"].ToString();
            var buCode = Request.Params["BuCode"];
            var vid = Request.Params["VID"];

            Response.Redirect(
                new StringBuilder().AppendFormat("RecEdit.aspx?MODE=EDIT&ID={0}&BuCode={1}&VID={2}&Po={3}",
                    rcvNo, buCode, vid, poNo).ToString());
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfrimVoid.ShowOnPageLoad = true;
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            var vid = Request.QueryString["VID"].ToString();
            var page = Request.QueryString["page"].ToString();

            Response.Redirect(string.Format("~/PC/REC/RecLst.aspx?VID={0}&page={1}", vid, page));
        }

        // -----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            //var objArrList = new ArrayList();
            //objArrList.Add(Request.Params["ID"]);
            //Session["s_arrNo"] = objArrList;

            Report rpt = new Report();
            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", Request.Params["ID"].ToString(), "ReceivingForm");
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimVoid.ShowOnPageLoad = false;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
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

        protected void grd_PR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var strConnStr = bu.GetConnectionString(DataBinder.Eval(e.Row.DataItem, "BUCode").ToString());

                if (e.Row.FindControl("lbl_BU") != null)
                {
                    var lbl_BU = e.Row.FindControl("lbl_BU") as Label;
                    lbl_BU.Text = DataBinder.Eval(e.Row.DataItem, "BUCode").ToString();
                }

                if (e.Row.FindControl("lbl_Store") != null)
                {
                    var lbl_Store = e.Row.FindControl("lbl_Store") as Label;
                    lbl_Store.Text = locat.GetName(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), strConnStr);
                    //----02/03/2012----locat.GetName2(DataBinder.Eval(e.Row.DataItem, "LocationCode").ToString(), strConnStr);
                }

                if (e.Row.FindControl("hpl_PRRef") != null)
                {
                    //Label lbl_PRRef = e.Row.FindControl("lbl_PRRef") as Label;
                    //lbl_PRRef.Text  = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                    var hpl_PRRef = e.Row.FindControl("hpl_PRRef") as HyperLink;
                    hpl_PRRef.NavigateUrl = "~/PC/PR/PR.aspx?VID=28&BuCode=" + DataBinder.Eval(e.Row.DataItem, "BUCode") +
                                            "&ID=" + DataBinder.Eval(e.Row.DataItem, "PRNo1");
                    hpl_PRRef.Text = DataBinder.Eval(e.Row.DataItem, "PRNo1").ToString();
                }

                if (e.Row.FindControl("lbl_PRDate") != null)
                {
                    var lbl_PRDate = e.Row.FindControl("lbl_PRDate") as Label;
                    lbl_PRDate.Text = String.Format("{0:d/M/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "PRDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "PRDate").ToString()).ToString();
                }

                if (e.Row.FindControl("lbl_QtyReq") != null)
                {
                    var lbl_QtyReq = e.Row.FindControl("lbl_QtyReq") as Label;
                    lbl_QtyReq.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ReqQty")) + " " +
                                      DataBinder.Eval(e.Row.DataItem, "Unit");
                    //dtPrDt.Rows[0]["ReqQty"].ToString() + " " + dtPrDt.Rows[0]["Unit"].ToString();
                }

                if (e.Row.FindControl("lbl_PricePR") != null)
                {
                    var lbl_PricePR = e.Row.FindControl("lbl_PricePR") as Label;
                    lbl_PricePR.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                    //String.Format("{0:n}", decPrice); //String.Format("{0:n}", dtPrDt.Rows[0]["Price"]);
                }

                if (e.Row.FindControl("lbl_Approve") != null)
                {
                    var lbl_Approve = e.Row.FindControl("lbl_Approve") as Label;
                    lbl_Approve.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "ApprQty"));
                    //String.Format("{0:n}", dtPrDt.Rows[0]["ApprQty"]);
                }

                if (e.Row.FindControl("lbl_Buyer") != null)
                {
                    var lbl_Buyer = e.Row.FindControl("lbl_Buyer") as Label;
                    lbl_Buyer.Text = DataBinder.Eval(e.Row.DataItem, "Buyer").ToString();
                }
            }
        }

        protected void btn_ExtraCostDetail_Click(object sender, EventArgs e)
        {
            pop_ExtraCostDetail.ShowOnPageLoad = true;

        }

        private void DeleteInventory(string recNo)
        {
            var sql = string.Format("DELETE FROM [IN].Inventory WHERE [Type]='RC' AND HdrNo='{0}'", recNo);

            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

        }

    }
}