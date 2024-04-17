using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PL.Vendor
{
    public partial class Vd : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.PC.PL.PL pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt plDt = new Blue.BL.PC.PL.PLDt();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private DataSet dsPL = new DataSet();

        private string PLDetailEditMode
        {
            get { return Session["PLDetailEditMode"].ToString(); }
            set { Session["PLDetailEditMode"] = value; }
        }

        private int PLDtEditSeqNo
        {
            get { return int.Parse(Session["PLDtEditSeqNo"].ToString()); }
            set { Session["PLDtEditSeqNo"] = value; }
        }

        private string PriceLstNo
        {
            get
            {
                if (Request.Params["ID"] != null)
                {
                    return Request.Params["ID"];
                }

                return string.Empty;
            }
        }

        #endregion

        #region "Page Load"

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ID.Value = PriceLstNo;
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPL = (DataSet)Session["dsPL"];
            }

            base.Page_Load(sender, e);
        }

        private void Page_Retrieve()
        {
            dsPL.Clear();

            // Get PL Data
            var getPL = pl.Get(dsPL, int.Parse(PriceLstNo), LoginInfo.ConnStr);

            if (!getPL)
            {
                return;
            }

            // Get PLDt Data
            var getPLDt = plDt.GetList(dsPL, PriceLstNo, LoginInfo.ConnStr);

            if (!getPLDt)
            {
                return;
            }

            Session["dsPL"] = dsPL;

            Page_Setting();
        }

        private void Page_Setting()
        {
            var drPL = dsPL.Tables[pl.TableName].Rows[0];

            lbl_Vendor.Text = drPL["VendorCode"].ToString();
            lbl_VendorName.Text = vendor.GetName(drPL["VendorCode"].ToString(), LoginInfo.ConnStr);
            if (drPL["DateFrom"] != DBNull.Value)
            {
                lbl_DateFrom.Text = DateTime.Parse(drPL["DateFrom"].ToString()).ToString("dd/MM/yyyy");
            }
            else
            {
                lbl_DateFrom.Text = string.Empty;
            }

            if (drPL["DateTo"] != DBNull.Value)
            {
                lbl_DateTo.Text = DateTime.Parse(drPL["DateTo"].ToString()).ToString("dd/MM/yyyy");
            }
            else
            {
                lbl_DateTo.Text = string.Empty;
            }

            lbl_RefNo.Text = drPL["RefNo"].ToString();

            grd_PLDt.DataSource = dsPL.Tables[plDt.TableName];
            grd_PLDt.DataBind();
        }

        #endregion

        #region "Command Bar"

        protected void Create()
        {
            Response.Redirect("VdEdit.aspx?MODE=NEW");
        }

        protected void Edit()
        {
            Response.Redirect("VdEdit.aspx?MODE=EDIT&ID=" + dsPL.Tables[pl.TableName].Rows[0]["PriceLstNo"]);
        }

        /// <summary>
        ///     Delete Price List.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OK_Delete_PR_Click(object sender, EventArgs e)
        {
            // Delete Price List
            dsPL.Tables[pl.TableName].Rows[0].Delete();

            // Delete Price List Detail
            foreach (DataRow drPLDt in dsPL.Tables[plDt.TableName].Rows)
            {
                if (drPLDt.RowState != DataRowState.Deleted)
                {
                    drPLDt.Delete();
                }
            }

            // Delete From Database
            var result = pl.Delete(dsPL, LoginInfo.ConnStr);

            if (result)
            {
                Response.Redirect("VdList.aspx");
            }
        }

        protected void btn_Print_Click(object sender, EventArgs e)
        {
            var vCode = string.Empty;
            var rFDate = string.Empty;
            var rTDate = string.Empty;

            var drPL = dsPL.Tables[pl.TableName].Rows[0];

            vCode = drPL["VendorCode"].ToString();

            if (drPL["DateFrom"].ToString() != string.Empty)
            {
                rFDate = DateTime.Parse(drPL["DateFrom"].ToString()).ToString("dd/MM/yyyy");
            }
            else
            {
                rFDate = DBNull.Value.ToString();
            }

            if (drPL["DateTo"].ToString() != string.Empty)
            {
                rTDate = DateTime.Parse(drPL["DateTo"].ToString()).ToString("dd/MM/yyyy");
            }
            else
            {
                rTDate = DBNull.Value.ToString();
            }

            var reportLink = "../../../RPT/Default.aspx?page=plistv&rfdate=" + rFDate + " &rtdate=" + rTDate + " &vid=" +
                             vCode + "";
            Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
            //Response.Write("<script>");
            //Response.Write("window.open('" + reportLink + "','_blank'  )");
            //Response.Write("</script>");
            //Server.Transfer("Default.aspx?page=plistp&rfdate=" + rFDate.ToString() + " &rtdate=" + rTDate.ToString() + "");
        }

        protected void Back()
        {
            Response.Redirect("VdList.aspx");
        }

        #endregion

        #region "Price List Detail"

        protected void btn_Create_PLDt_Click(object sender, EventArgs e)
        {
            //this.PLDetailEditMode       = "NEW";
            //pop_PLDtEdit.ShowOnPageLoad = true;
        }

        /// <summary>
        ///     Delete Selected PLDt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn_OK_Click(object sender, EventArgs e)
        //{
        //    List<object> columnValues = grd_PLDt.GetSelectedFieldValues("SeqNo");
        //    foreach (int SeqNo in columnValues)
        //    {
        //        foreach (DataRow drPLDt in dsPL.Tables[plDt.TableName].Rows)
        //        {
        //            if (drPLDt.RowState == DataRowState.Deleted)
        //            {
        //                continue;
        //            }
        //            if (int.Parse(drPLDt["SeqNo"].ToString()) == SeqNo)
        //            {
        //                drPLDt.Delete();
        //                continue;
        //            }
        //        }
        //    }
        //    // Save Change to DataBase
        //    bool result = plDt.Save(dsPL, LoginInfo.ConnStr);
        //    if (result)
        //    {
        //        //pop_ConfirmDelete.ShowOnPageLoad = false;
        //        //this.Page_Retrieve();
        //    }
        //}
        protected void grd_PLDt_Load(object sender, EventArgs e)
        {
            grd_PLDt.DataSource = dsPL.Tables[plDt.TableName];
            grd_PLDt.DataBind();
        }

        protected void grd_PLDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                    lbl_ProductCode.ToolTip = lbl_ProductCode.Text;
                }

                if (e.Row.FindControl("lbl_ProductName") != null)
                {
                    var lbl_ProductName = e.Row.FindControl("lbl_ProductName") as Label;
                    lbl_ProductName.Text =
                        product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value) +
                        " : " +
                        product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), hf_ConnStr.Value);
                    lbl_ProductName.ToolTip = lbl_ProductName.Text;
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                }

                if (e.Row.FindControl("lbl_Rank") != null)
                {
                    var lbl_Rank = e.Row.FindControl("lbl_Rank") as Label;
                    lbl_Rank.Text = DataBinder.Eval(e.Row.DataItem, "VendorRank").ToString();
                }

                if (e.Row.FindControl("lbl_QtyFrom") != null)
                {
                    var lbl_QtyFrom = e.Row.FindControl("lbl_QtyFrom") as Label;
                    lbl_QtyFrom.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "QtyFrom"));
                }

                if (e.Row.FindControl("lbl_To") != null)
                {
                    var lbl_To = e.Row.FindControl("lbl_To") as Label;

                    lbl_To.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "QtyTo"));
                }

                if (e.Row.FindControl("lbl_Price") != null)
                {
                    var lbl_Price = e.Row.FindControl("lbl_Price") as Label;
                    //lbl_Price.Text = String.Format("{0:N4}", DataBinder.Eval(e.Row.DataItem, "QuotedPrice"));
                    lbl_Price.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "QuotedPrice"));
                }

                // Added on: 09/08/2017, By: Fon
                if (e.Row.FindControl("lbl_CurrCode") != null)
                {
                    var lbl_CurrCode = (Label)e.Row.FindControl("lbl_CurrCode");
                    lbl_CurrCode.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }
                // End Added.

                if (e.Row.FindControl("lbl_MarketPrice") != null)
                {
                    var lbl_MarketPrice = e.Row.FindControl("lbl_MarketPrice") as Label;
                    lbl_MarketPrice.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "MarketPrice"));
                }

                if (e.Row.FindControl("lbl_Foc") != null)
                {
                    var lbl_Foc = e.Row.FindControl("lbl_Foc") as Label;
                    //lbl_Foc.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "FOC"));
                    lbl_Foc.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "FOC"));
                }

                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment.ToolTip = lbl_Comment.Text;
                }

                if (e.Row.FindControl("lbl_Disc") != null)
                {
                    var lbl_Disc = e.Row.FindControl("lbl_Disc") as Label;
                    lbl_Disc.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }

                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "DiscAmt"));
                }

                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "Tax").ToString().ToUpper() == "A")
                    {
                        lbl_TaxType.Text = "Add";
                    }
                    else if (DataBinder.Eval(e.Row.DataItem, "Tax").ToString().ToUpper() == "I")
                    {
                        lbl_TaxType.Text = "Included";
                    }
                    else
                    {
                        lbl_TaxType.Text = "None";
                    }
                }

                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("lbl_VendorSKU") != null)
                {
                    var lbl_VendorSKU = e.Row.FindControl("lbl_VendorSKU") as Label;
                    lbl_VendorSKU.Text = DataBinder.Eval(e.Row.DataItem, "VendorProdCode").ToString();
                }

                if (e.Row.FindControl("lbl_PriceNet") != null)
                {
                    var lbl_PriceNet = e.Row.FindControl("lbl_PriceNet") as Label;
                    lbl_PriceNet.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                }

                if (e.Row.FindControl("lbl_Avg") != null)
                {
                    var lbl_Avg = e.Row.FindControl("lbl_Avg") as Label;
                    lbl_Avg.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "AvgPrice"));
                }

                if (e.Row.FindControl("lbl_Last") != null)
                {
                    var lbl_Last = e.Row.FindControl("lbl_Last") as Label;
                    lbl_Last.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "LastPrice"));
                }
            }
        }

        protected void grd_PLDt_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            //this.PLDetailEditMode   = "EDIT";
            //this.PLDtEditSeqNo      = int.Parse(e.EditingKeyValue.ToString());

            //foreach (DataRow drPLDt in dsPL.Tables[plDt.TableName].Rows)
            //{
            //    if (int.Parse(drPLDt["SeqNo"].ToString()) == this.PLDtEditSeqNo)
            //    {
            //        ddl_Product.Value       = drPLDt["ProductCode"].ToString();
            //        ddl_OrderUnit.Value     = drPLDt["OrderUnit"].ToString();
            //        txt_QtyFrom.Text        = drPLDt["QtyFrom"].ToString();
            //        txt_QtyTo.Text          = drPLDt["QtyTo"].ToString();
            //        txt_VendorRank.Text     = drPLDt["VendorRank"].ToString();
            //        txt_QuotedPrice.Text    = drPLDt["QuotedPrice"].ToString();
            //        txt_MarketPrice.Text    = drPLDt["MarketPrice"].ToString();
            //        txt_FOC.Text            = drPLDt["FOC"].ToString();
            //        txt_Comment.Text        = drPLDt["Comment"].ToString();
            //        txt_DiscPercent.Text    = drPLDt["DiscPercent"].ToString();
            //        txt_DiscAmt.Text        = drPLDt["DiscAmt"].ToString();

            //        if (drPLDt["Tax"] != null)
            //        {
            //            ddl_TaxType.Value = drPLDt["Tax"].ToString();
            //        }

            //        txt_TaxRate.Text        = drPLDt["TaxRate"].ToString();
            //        txt_VendorProdCode.Text = drPLDt["VendorProdCode"].ToString();
            //        txt_NetAmt.Text         = drPLDt["NetAmt"].ToString();
            //        txt_AvgPrice.Text       = drPLDt["AvgPrice"].ToString();
            //        txt_LastPrice.Text      = drPLDt["LastPrice"].ToString();

            //        break;
            //    }
            //}

            //// Disable Edit in GridView
            //pop_PLDtEdit.ShowOnPageLoad = true;
            //e.Cancel = true;
        }

        #endregion

        #region "Edit Form"

        protected void btn_Update_PLDt_Click(object sender, EventArgs e)
        {
            //if (this.PLDetailEditMode.ToUpper() == "NEW")
            //{
            //    DataRow drNew = dsPL.Tables[plDt.TableName].NewRow();

            //    drNew["PriceLstNo"] = dsPL.Tables[pl.TableName].Rows[0]["PriceLstNo"].ToString();

            //    if (dsPL.Tables[plDt.TableName].Rows.Count == 0)
            //    {
            //        drNew["SeqNo"] = 1;
            //    }
            //    else
            //    {
            //        int RowCount    = dsPL.Tables[plDt.TableName].Rows.Count;
            //        int LatestSeqNo = int.Parse(dsPL.Tables[plDt.TableName].Rows[RowCount - 1]["SeqNo"].ToString());
            //        drNew["SeqNo"]  = LatestSeqNo + 1;
            //    }
            //    drNew["ProductCode"]    = ddl_Product.Value.ToString();
            //    drNew["OrderUnit"]      = ddl_OrderUnit.Value.ToString();
            //    drNew["VendorRank"]     = txt_VendorRank.Text;
            //    drNew["QtyFrom"]        = txt_QtyFrom.Text;
            //    drNew["QtyTo"]          = txt_QtyTo.Text;
            //    drNew["QuotedPrice"]    = txt_QuotedPrice.Text;
            //    drNew["MarketPrice"]    = txt_MarketPrice.Text;
            //    drNew["FOC"]            = txt_FOC.Text;
            //    drNew["Comment"]        = txt_Comment.Text;
            //    drNew["DiscPercent"]    = txt_DiscPercent.Text;
            //    drNew["DiscAmt"]        = txt_DiscAmt.Text;

            //    if (ddl_TaxType.Value != null)
            //    {
            //        drNew["Tax"] = ddl_TaxType.Value;
            //    }

            //    drNew["TaxRate"]        = txt_TaxRate.Text;
            //    drNew["VendorProdCode"] = txt_VendorProdCode.Text;
            //    drNew["NetAmt"]         = txt_NetAmt.Text;
            //    drNew["AvgPrice"]       = txt_AvgPrice.Text;
            //    drNew["LastPrice"]      = txt_LastPrice.Text;

            //    dsPL.Tables[plDt.TableName].Rows.Add(drNew);
            //}
            //else
            //{
            //    foreach (DataRow drPLDt in dsPL.Tables[plDt.TableName].Rows)
            //    {
            //        if (int.Parse(drPLDt["SeqNo"].ToString()) == this.PLDtEditSeqNo)
            //        {
            //            drPLDt["ProductCode"]   = ddl_Product.Value.ToString();
            //            drPLDt["OrderUnit"]     = ddl_OrderUnit.Value.ToString();
            //            drPLDt["QtyFrom"]       = txt_QtyFrom.Text;
            //            drPLDt["QtyTo"]         = txt_QtyTo.Text;
            //            drPLDt["VendorRank"]    = txt_VendorRank.Text;
            //            drPLDt["QuotedPrice"]   = txt_QuotedPrice.Text;
            //            drPLDt["MarketPrice"]   = txt_MarketPrice.Text;
            //            drPLDt["FOC"]           = txt_FOC.Text;
            //            drPLDt["Comment"]       = txt_Comment.Text;
            //            drPLDt["DiscPercent"]   = txt_DiscPercent.Text;
            //            drPLDt["DiscAmt"]       = txt_DiscAmt.Text;

            //            if (ddl_TaxType.Value != null)
            //            {
            //                drPLDt["Tax"] = ddl_TaxType.Value.ToString();
            //            }

            //            drPLDt["TaxRate"]           = txt_TaxRate.Text;
            //            drPLDt["VendorProdCode"]    = txt_VendorProdCode.Text;
            //            drPLDt["NetAmt"]            = txt_NetAmt.Text;
            //            drPLDt["AvgPrice"]          = txt_AvgPrice.Text;
            //            drPLDt["LastPrice"]         = txt_LastPrice.Text;

            //            break;
            //        }
            //    }
            //}

            //// Save Change to DataBase
            //bool result = plDt.Save(dsPL, LoginInfo.ConnStr);

            //if (result)
            //{
            //    pop_PLDtEdit.ShowOnPageLoad = false;
            //    this.Page_Retrieve();
            //}
        }

        protected void btn_Cancel_PLDt_Click(object sender, EventArgs e)
        {
            //this.PLDetailEditMode       = "";
            //pop_PLDtEdit.ShowOnPageLoad = false;
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddl_OrderUnit.Value = product.GetOrderUnit(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
        }

        protected void txt_QuotedPrice_NumberChanged(object sender, EventArgs e)
        {
            //// Recalculate DiscountAmt
            //decimal Price       = 0;
            //decimal DiscPercent = 0;

            //if (!string.IsNullOrEmpty(txt_QuotedPrice.Text))
            //{
            //    Price = decimal.Parse(txt_QuotedPrice.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_DiscPercent.Text))
            //{
            //    DiscPercent = decimal.Parse(txt_DiscPercent.Text);
            //}

            //txt_DiscAmt.Text = ((Price * DiscPercent) / 100).ToString();

            //// Recalculate NetAmt
            //UpdateNetAmount();
        }

        protected void txt_DiscPercent_NumberChanged(object sender, EventArgs e)
        {
            //// Recalculate DiscountAmt
            //decimal Price = 0;
            //decimal DiscPercent = 0;

            //if (!string.IsNullOrEmpty(txt_QuotedPrice.Text))
            //{
            //    Price = decimal.Parse(txt_QuotedPrice.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_DiscPercent.Text))
            //{
            //    DiscPercent = decimal.Parse(txt_DiscPercent.Text);
            //}

            //txt_DiscAmt.Text = ((Price * DiscPercent) / 100).ToString();

            //// Recalculate NetAmt
            //UpdateNetAmount();
        }

        protected void ddl_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recalculate NetAmt
            UpdateNetAmount();
        }

        protected void txt_TaxRate_NumberChanged(object sender, EventArgs e)
        {
            // Recalculate NetAmt
            UpdateNetAmount();
        }

        private void UpdateNetAmount()
        {
            //decimal DiscAmt     = 0;
            //decimal TaxRate     = 0;
            //decimal Price       = 0;

            //if (!string.IsNullOrEmpty(txt_DiscAmt.Text))
            //{
            //    DiscAmt = decimal.Parse(txt_DiscAmt.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_TaxRate.Text))
            //{
            //    TaxRate = decimal.Parse(txt_TaxRate.Text);
            //}

            //if (!string.IsNullOrEmpty(txt_QuotedPrice.Text))
            //{
            //    Price = decimal.Parse(txt_QuotedPrice.Text);
            //}

            //// Recalculate NetAmt
            //if (ddl_TaxType.Value != null)
            //{
            //    switch (ddl_TaxType.Value.ToString().ToUpper())
            //    {
            //        case "A":
            //            txt_NetAmt.Text = ((Price - DiscAmt) + ((Price * TaxRate) / 100)).ToString();
            //            break;

            //        case "I":
            //            txt_NetAmt.Text = ((Price - DiscAmt) - ((Price * TaxRate) / (100 + TaxRate))).ToString();
            //            break;

            //        default:
            //            txt_NetAmt.Text = (Price - DiscAmt).ToString();
            //            break;
            //    }
            //}
            //else
            //{
            //    txt_NetAmt.Text = (Price - DiscAmt).ToString();
            //}
        }

        #endregion

        protected void btn_ConfirmDeletePL_Click(object sender, EventArgs e)
        {
            var PL = Request.Params["ID"];

            if (dsPL.Tables[plDt.TableName].Rows.Count > 0 &&
                prDt.CountByVendorCode(dsPL.Tables[pl.TableName].Rows[0]["VendorCode"].ToString(), dsPL.Tables[plDt.TableName].Rows[0]["ProductCode"].ToString(), LoginInfo.ConnStr) > 0)
            {
                pop_CannotDelete.ShowOnPageLoad = true;
                return;
            }

            foreach (DataRow drPLDt in dsPL.Tables[plDt.TableName].Rows)
            {
                drPLDt.Delete();
            }

            foreach (DataRow drPL in dsPL.Tables[pl.TableName].Rows)
            {
                drPL.Delete();
            }

            //Save To DataBase.
            var savePL = pl.Delete(dsPL, LoginInfo.ConnStr);

            if (savePL)
            {
                pop_ConfirmDeletePL.ShowOnPageLoad = false;
                Response.Redirect("VdList.aspx");
            }
        }

        protected void btn_CancelDeletePL_Click(object sender, EventArgs e)
        {
            pop_ConfirmDeletePL.ShowOnPageLoad = false;
        }

        protected void Delete()
        {
            pop_ConfirmDeletePL.ShowOnPageLoad = true;
        }

        protected void Print()
        {
            // Send VD  to Session            
            //var objArrList = new ArrayList();

            //objArrList.Add(PriceLstNo); //"'" + dsPL.Tables[pl.TableName].Rows[0]["VendorCode"].ToString() + "'"
            //Session["s_arrNo"] = objArrList;
            //var reportLink = "../../../RPT/ReportCriteria.aspx?rn=Plistv&category=009&reportid=146" + "&BuCode=" +
            //                 Request.Params["BuCode"];
            //ClientScript.RegisterStartupScript(GetType(), "newWindow",
            //    "<script>window.open('" + reportLink + "','_blank')</script>");
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
        }

        protected void btn_YES_Click(object sender, EventArgs e)
        {
            pop_ConfirmDeletePL.ShowOnPageLoad = false;
            pop_CannotDelete.ShowOnPageLoad = false;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    Create();
                    break;
                case "EDIT":
                    Edit();
                    break;
                case "DELETE":
                    Delete();
                    break;
                case "PRINT":
                    Print();
                    break;
                case "BACK":
                    Back();
                    break;
            }
        }
    }
}