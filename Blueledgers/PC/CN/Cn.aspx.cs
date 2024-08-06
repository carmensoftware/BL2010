using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;

namespace BlueLedger.PL.PC.CN
{
    public partial class CN : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly Blue.BL.PC.CN.CnDt cnDt = new Blue.BL.PC.CN.CnDt();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private string CnNo = string.Empty;
        private Blue.BL.Ref.Currency curr = new Blue.BL.Ref.Currency();
        private Blue.BL.Option.Inventory.DeliveryPoint deliveryPoint = new Blue.BL.Option.Inventory.DeliveryPoint();
        private DataSet dsCn = new DataSet();
        private Blue.BL.Ref.ExRate exRate = new Blue.BL.Ref.ExRate();
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private int index;
        private readonly string moduleID = "2.4";

        private decimal _cnCurrTotalAmt = 0m;
        private decimal _cnCurrTotalTax = 0m;
        private decimal _cnCurrTotalNet = 0m;
        private decimal _cnTotalAmt = 0m;
        private decimal _cnTotalTax = 0m;
        private decimal _cnTotalNet = 0m;

        private decimal _stTotalAmt = 0m;

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsCn = (DataSet)Session["dsCn"];
                //this.Page_Setting();
            }
        }

        private void Page_Retrieve()
        {
            var dsTmp = new DataSet();
            var MsgError = string.Empty;

            hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);

            if (!string.IsNullOrEmpty(Request.Params["ID"]))
            {
                CnNo = Request.Params["ID"];

                var getStrLct = cn.GetListByCnNo(dsTmp, ref MsgError, CnNo, hf_ConnStr.Value);

                if (getStrLct)
                {
                    dsCn = dsTmp;

                    cnDt.GetListByCnNo(dsCn, CnNo, hf_ConnStr.Value);
                }
                else
                {
                    var Error = Resources.MsgError.ResourceManager.GetString(MsgError);
                    return;
                }
            }

            Page_Setting();

            Session["dsCn"] = dsCn;
        }

        private void Page_Setting()
        {
            if (dsCn.Tables[cn.TableName] != null)
            {
                var drCn = dsCn.Tables[cn.TableName].Rows[0];

                lbl_CnNo.Text = drCn["CnNo"].ToString();

                if (drCn["CnDate"].ToString() != string.Empty)
                {
                    lbl_CnDate.Text = DateTime.Parse(drCn["CnDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                lbl_DocNo.Text = drCn["DocNo"].ToString();

                if (drCn["DocDate"].ToString() != string.Empty)
                {
                    lbl_DocDate.Text = DateTime.Parse(drCn["DocDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                if (drCn["DocStatus"].ToString().ToUpper() == "COMMITTED" ||
                    drCn["DocStatus"].ToString().ToUpper() == "VOIDED")
                {
                    btn_Edit.Visible = false;
                    btn_Void.Visible = false;
                }

                lbl_VendorCode.Text = drCn["VendorCode"] + " : ";
                lbl_VendorNm.Text = vendor.GetName(drCn["VendorCode"].ToString(), hf_ConnStr.Value);
                lbl_Currency.Text = drCn["CurrencyCode"].ToString();
                lbl_ExRateAudit.Text = drCn["ExRateAudit"].ToString();
                lbl_Status.Text = drCn["DocStatus"].ToString();
                lbl_Desc.Text = drCn["Description"].ToString();

                grd_CnEdit.DataSource = dsCn.Tables[cnDt.TableName];
                grd_CnEdit.DataBind();
            }

            // Display Total
            var dt = dsCn.Tables[cnDt.TableName].AsEnumerable();


            TotalSummary.CurrencyNetAmount = dt.Sum(x => x.Field<decimal>("CurrNetAmt"));
            TotalSummary.CurrencyTaxAmount = dt.Sum(x => x.Field<decimal>("CurrTaxAmt"));
            TotalSummary.CurrencyTotalAmount = dt.Sum(x => x.Field<decimal>("CurrTotalAmt"));

            TotalSummary.NetAmount = dt.Sum(x => x.Field<decimal>("NetAmt"));
            TotalSummary.TaxAmount = dt.Sum(x => x.Field<decimal>("TaxAmt"));
            TotalSummary.TotalAmount = dt.Sum(x => x.Field<decimal>("TotalAmt"));


            // Display Comment.
            var comment = (PL.UserControls.Comment2)Master.FindControl("Comment");
            comment.Module = "PC";
            comment.SubModule = "Cn";
            comment.RefNo = dsCn.Tables[cn.TableName].Rows[0]["CnNo"].ToString();
            comment.Visible = true;
            comment.DataBind();

            // Display Attach
            var attach = (PL.UserControls.Attach2)Master.FindControl("Attach");
            attach.BuCode = Request.Params["BuCode"];
            attach.ModuleName = "PC";
            attach.RefNo = dsCn.Tables[cn.TableName].Rows[0]["CnNo"].ToString();
            attach.Visible = true;
            attach.DataBind();

            // Display Log
            var log = (PL.UserControls.Log2)Master.FindControl("Log");
            log.Module = "PC";
            log.SubModule = "Cn";
            log.RefNo = dsCn.Tables[cn.TableName].Rows[0]["CnNo"].ToString();
            log.Visible = true;
            log.DataBind();

            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            btn_Edit.Visible = (pagePermiss >= 3) ? btn_Edit.Visible : false;
            btn_Void.Visible = (pagePermiss >= 7) ? btn_Void.Visible : false;
        }

        protected void btn_ConfirmVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
            Void();
        }

        private void Void()
        {
            string cnNo;

            var drCn = dsCn.Tables[cn.TableName].Rows[0];

            cnNo = drCn["CnNo"].ToString();

            drCn["DocStatus"] = "Voided";
            inv.GetListByHdrNo(dsCn, drCn["CnNo"].ToString(), hf_ConnStr.Value);

            foreach (DataRow drCnDt in dsCn.Tables[cnDt.TableName].Rows)
            {
                //drCnDt["Status"] = "Voided";

                foreach (DataRow drInv in dsCn.Tables[inv.TableName].Rows)
                {
                    if (drCnDt["CnNo"].ToString() == drInv["HdrNo"].ToString() &&
                        drCnDt["CnDtNo"].ToString() == drInv["DtNo"].ToString())
                    {
                        drInv["OUT"] = Convert.ToDecimal(drInv["OUT"].ToString()) -
                                       Convert.ToDecimal(drCnDt["RecQty"].ToString());
                    }
                }
            }

            var save = cn.SaveToCnCnDtAndInv(dsCn, hf_ConnStr.Value);
            if (save)
            {
                _transLog.Save("PC", "CN", cnNo, "VOIDED", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
                Response.Redirect("~/PC/Cn/CnList.aspx");
            }
        }

        protected void btn_Edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("CnEdit.aspx?MODE=EDIT&ID=" + dsCn.Tables[cn.TableName].Rows[0]["CnNo"] + "&BuCode=" +
                              Request.Params["BuCode"]);
        }

        protected void btn_Void_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = true;
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PC/Cn/CnList.aspx");
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            string cnNo = Request.Params["ID"].ToString();
            _transLog.Save("PC", "CN", cnNo, "PRINT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);

            Report rpt = new Report();
            rpt.PrintForm(this, "../../RPT/PrintForm.aspx", cnNo, "CreditNoteForm");

            //// Send CnNo  to Session            
            //var objArrList = new ArrayList();


            //objArrList.Add(Request.Params["ID"]);
            //Session["s_arrNo"] = objArrList;

            //var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=116" + "&BuCode=" +
            //                 Request.Params["BuCode"];
            ////Response.Write("<script>");
            ////Response.Write("window.open('" + reportLink + "','_blank'  )");
            ////Response.Write("</script>");
            //ClientScript.RegisterStartupScript(GetType(), "newWindow",
            //    "<script>window.open('" + reportLink + "','_blank')</script>");
        }

        protected void btn_CancelVoid_Click(object sender, EventArgs e)
        {
            pop_ConfirmVoid.ShowOnPageLoad = false;
        }

        protected void grd_CnEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string cnStatus = lbl_Status.Text;
                string cnNo = lbl_CnNo.Text;
                string cnDtNo = grd_CnEdit.DataKeys[e.Row.RowIndex].Value.ToString();

                // ------------------------------------------------------------------------------------------

                // Find Control Image Button
                var Img_Btn = e.Row.FindControl("Img_Btn") as ImageButton;

                //set CommandArgument For ImageButton
                if (e.Row.FindControl("Img_Btn") != null)
                {
                    Img_Btn.CommandArgument = index.ToString();
                    index = index + 1;
                }


                if (e.Row.FindControl("lbl_Rec") != null)
                {
                    var lbl_Rec = e.Row.FindControl("lbl_Rec") as Label;
                    lbl_Rec.Text = DataBinder.Eval(e.Row.DataItem, "RecNo").ToString();
                    lbl_Rec.ToolTip = lbl_Rec.Text;
                }

                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl_Product = e.Row.FindControl("lbl_Product") as Label;

                    var name = product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        hf_ConnStr.Value);
                    var desc = product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                        hf_ConnStr.Value);
                    //lbl_Product.Text = (DataBinder.Eval(e.Row.DataItem, "ProductCode") == DBNull.Value ? string.Empty : DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString() + " : " + name + " : " + desc);
                    lbl_Product.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " + name + " : " + desc;
                    lbl_Product.ToolTip = lbl_Product.Text;
                }

                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl_Location = e.Row.FindControl("lbl_Location") as Label;
                    lbl_Location.Text = DataBinder.Eval(e.Row.DataItem, "Location") + " : " +
                                        locat.GetName(DataBinder.Eval(e.Row.DataItem, "Location").ToString(),
                                            hf_ConnStr.Value);
                    lbl_Location.ToolTip = lbl_Location.Text;
                }

                if (e.Row.FindControl("lbl_CnType") != null)
                {
                    var label = e.Row.FindControl("lbl_CnType") as Label;
                    string cnType = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();
                    if (cnType.ToUpper() == "A")  // Amount
                        label.Text = "Amount";
                    else
                        label.Text = "Quantity";
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lbl_Qty = e.Row.FindControl("lbl_Qty") as Label;
                    lbl_Qty.Text = String.Format(DefaultQtyFmt, Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "RecQty")));
                    lbl_Qty.ToolTip = lbl_Qty.Text + " @" + string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));

                    string cnType = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();
                    lbl_Qty.Visible = cnType.ToUpper() != "A";


                }


                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "UnitCode").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;

                    string cnType = DataBinder.Eval(e.Row.DataItem, "CnType").ToString();
                    lbl_Unit.Visible = cnType.ToUpper() != "A";
                }

                if (e.Row.FindControl("lbl_CurrNet") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrNet") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("lbl_CurrTax") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTax") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_CurrTotal") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTotal") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                    lbl.ToolTip = lbl.Text;
                }

                // --------------------------------------------------------------------------------------------------------------------------
                // Summary
                // --------------------------------------------------------------------------------------------------------------------------

                // Information of Receiving
                string query = string.Empty;
                query = "SELECT TOP(1) CAST(rh.RecDate as DATE) RecDate, rh.[Description] as RecDesc FROM PC.REC rh";
                query += string.Format(" WHERE rh.RecNo = '{0}' ", DataBinder.Eval(e.Row.DataItem, "RecNo").ToString());
                DataTable dtRec = cn.DbExecuteQuery(query, null, hf_ConnStr.Value);

                if (dtRec.Rows.Count > 0)
                {
                    DateTime recDate = Convert.ToDateTime(dtRec.Rows[0]["RecDate"]);
                    string recDesc = dtRec.Rows[0]["RecDesc"].ToString();

                    if (e.Row.FindControl("lbl_RecDate") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecDate") as Label;
                        lbl.Text = string.Format("{0:M/d/yyyy}", recDate);
                        lbl.ToolTip = string.Format("{0:dddd, MMMM d, yyyy}", recDate);
                    }
                    if (e.Row.FindControl("lbl_RecDesc") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecDesc") as Label;
                        lbl.Text = recDesc;
                        lbl.ToolTip = lbl.Text;
                    }
                    if (e.Row.FindControl("lbl_RecPrice") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecPrice") as Label;
                        lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "Price"));
                        lbl.ToolTip = lbl.Text;
                    }

                    if (e.Row.FindControl("lbl_RecTaxType") != null)
                    {
                        var label = e.Row.FindControl("lbl_RecTaxType") as Label;

                        label.Text = DataBinder.Eval(e.Row.DataItem, "TaxType").ToString();
                        label.ToolTip = label.Text;
                    }


                    if (e.Row.FindControl("lbl_RecTaxRate") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecTaxRate") as Label;
                        lbl.Text = string.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "TaxRate"));
                        lbl.ToolTip = lbl.Text;
                    }

                    if (e.Row.FindControl("lbl_RecTaxAmt") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecTaxAmt") as Label;
                        lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                        lbl.ToolTip = lbl.Text;
                        //_cnTotalTax = _cnTotalTax + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    }

                    if (e.Row.FindControl("lbl_RecNetAmt") != null)
                    {
                        var lbl = e.Row.FindControl("lbl_RecNetAmt") as Label;
                        lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                        lbl.ToolTip = lbl.Text;

                    }
                } // Receiving

                if (e.Row.FindControl("lbl_Currency") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Currency") as Label;
                    lbl.Text = lbl_Currency.Text;
                    lbl.ToolTip = lbl.Text;

                }


                if (e.Row.FindControl("lbl_CurrNetAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrNetAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                    lbl.ToolTip = lbl.Text;

                }


                if (e.Row.FindControl("lbl_CurrTaxAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTaxAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_CurrTotalAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_CurrTotalAmt") as Label;
                    lbl.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                    lbl.ToolTip = lbl.Text;

                }

                if (e.Row.FindControl("lbl_BaseCurrency") != null)
                {
                    var lbl = e.Row.FindControl("lbl_BaseCurrency") as Label;
                    lbl.Text = config.GetValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
                    lbl.ToolTip = lbl.Text;

                }



                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_NetAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                    lbl.ToolTip = lbl.Text;

                }


                if (e.Row.FindControl("lbl_TaxAmt") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TaxAmt") as Label;
                    lbl.Text = string.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TaxAmt"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_TotalAmt") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_TotalAmt") as Label;
                    lbl_Price.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                    lbl_Price.ToolTip = lbl_Price.Text;

                }

                // Calcualtin Total of NetAmount, TaxAmount and Amount
                _cnCurrTotalAmt = _cnTotalAmt + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTotalAmt"));
                _cnCurrTotalNet = _cnTotalNet + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrNetAmt"));
                _cnCurrTotalTax = _cnTotalTax + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CurrTaxAmt"));

                _cnTotalAmt = _cnTotalAmt + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmt"));
                _cnTotalNet = _cnTotalNet + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                _cnTotalTax = _cnTotalTax + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TaxAmt"));



                //******************************* Stock Movement *****************************************************
                if (e.Row.FindControl("uc_StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = DataBinder.Eval(e.Row.DataItem, "CnNo").ToString();
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "CnDtNo").ToString();
                    uc_StockMovement.ConnStr = hf_ConnStr.Value;
                    uc_StockMovement.DataBind();
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {

                if (e.Row.FindControl("lbl_GrandCurrTotalNet") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandCurrTotalNet") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnCurrTotalNet);
                }

                if (e.Row.FindControl("lbl_GrandCurrTotalTax") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandCurrTotalTax") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnCurrTotalTax);
                }

                if (e.Row.FindControl("lbl_GrandCurrTotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandCurrTotalAmt") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnCurrTotalAmt);
                }


                if (e.Row.FindControl("lbl_GrandTotalNet") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandTotalNet") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnTotalNet);
                }

                if (e.Row.FindControl("lbl_GrandTotalTax") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandTotalTax") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnTotalTax);
                }

                if (e.Row.FindControl("lbl_GrandTotalAmt") != null)
                {
                    var label = e.Row.FindControl("lbl_GrandTotalAmt") as Label;
                    label.Text = String.Format(DefaultAmtFmt, _cnTotalAmt);
                }
            }
        }

        protected void grd_CnEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "ShowDetail")
            //{
            //var Img_Btn =
            //    grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Img_Btn") as
            //        ImageButton;

            //if (Img_Btn.ImageUrl == "~/App_Themes/Default/Images/master/in/Default/Plus.jpg")
            //{
            //    var p_DetailRows =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;
            //    p_DetailRows.Visible = true;
            //    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Minus.jpg";

            //    var drExpand =
            //        dsCn.Tables[cnDt.TableName].Rows[
            //            grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].DataItemIndex];

            //    var errorMsg = string.Empty;


            //    // Display Transaction Detail ---------------------------------------------------------
            //    var lbl_ProductCode =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_ProductCode") as Label;
            //    lbl_ProductCode.Text = drExpand["ProductCode"].ToString();

            //    var lbl_Descen =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Descen") as Label;
            //    lbl_Descen.Text = product.GetName(drExpand["ProductCode"].ToString(), hf_ConnStr.Value);
            //    lbl_Descen.ToolTip = lbl_Descen.Text;

            //    var lbl_Descll =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Descll") as Label;
            //    lbl_Descll.Text = product.GetName2(drExpand["ProductCode"].ToString(), hf_ConnStr.Value);
            //    lbl_Descll.ToolTip = lbl_Descll.Text;

            //    var lbl_Location =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Location") as Label;
            //    lbl_Location.Text = locat.GetName(drExpand["Location"].ToString(), hf_ConnStr.Value);
            //    //----02/03/2012----locat.GetName2(drExpand["Location"].ToString(), hf_ConnStr.Value);

            //    var lbl_Disc =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Disc") as Label;
            //    lbl_Disc.Text = drExpand["DiscPercent"].ToString();

            //    var lbl_DiscAmt =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_DiscAmt") as Label;
            //    lbl_DiscAmt.Text = String.Format("{0:N}", decimal.Parse(drExpand["DiscAmt"].ToString()));

            //    var lbl_TaxType =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxType") as Label;
            //    lbl_TaxType.Text = Blue.BL.GnxLib.GetTaxTypeName(drExpand["TaxType"].ToString());

            //    var lbl_TaxRate =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxRate") as Label;
            //    lbl_TaxRate.Text = drExpand["TaxRate"].ToString();

            //    //Label lbl_PrRef = grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_PrRef") as Label;
            //    //lbl_PrRef.Text = drExpand["PrNo"].ToString();

            //    //Label lbl_PoRef = grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_PoRef") as Label;
            //    //lbl_PoRef.Text = drExpand["PoNo"].ToString();


            //    //Label lbl_QtyRec = grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_QtyRec") as Label;
            //    //lbl_QtyRec.Text = drExpand["RecQty"].ToString();

            //    var lbl_PriceDt =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_PriceDt") as Label;
            //    lbl_PriceDt.Text = String.Format("{0:N}", decimal.Parse(drExpand["Price"].ToString()));

            //    var lbl_NetAmtDt =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_NetAmtDt") as Label;
            //    lbl_NetAmtDt.Text = String.Format("{0:N}", decimal.Parse(drExpand["NetAmt"].ToString()));

            //    var lbl_TaxAmtDt =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TaxAmtDt") as Label;
            //    lbl_TaxAmtDt.Text = String.Format("{0:N}", decimal.Parse(drExpand["TaxAmt"].ToString()));

            //    var lbl_TotalAmtDt =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_TotalAmtDt") as Label;
            //    lbl_TotalAmtDt.Text = String.Format("{0:N}", decimal.Parse(drExpand["TotalAmt"].ToString()));


            //    var lbl_Memorandum =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lbl_Memorandum") as Label;
            //    lbl_Memorandum.Text = drExpand["Comment"].ToString();
            //}
            //else
            //{
            //    var p_DetailRows =
            //        grd_CnEdit.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("p_DetailRows") as Panel;

            //    p_DetailRows.Visible = false;
            //    Img_Btn.ImageUrl = "~/App_Themes/Default/Images/master/in/Default/Plus.jpg";
            //}
            //}
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

    }
}