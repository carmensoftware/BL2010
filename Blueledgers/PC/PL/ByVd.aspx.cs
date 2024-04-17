using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PL
{
    public partial class ByVd : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.IN.PriceList priceList = new Blue.BL.IN.PriceList();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();
        private readonly Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private DataSet dsPriceList = new DataSet();

        private string AddEditRowPK
        {
            get { return (string) Session["AddEditRowPK"]; }
            set { Session["AddEditRowPK"] = value; }
        }

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPriceList = (DataSet) Session["dsPriceList"];
            }
        }

        /// <summary>
        ///     Page retrieve
        /// </summary>
        private void Page_Retrieve()
        {
            var keys = Request.Params["ID"].Split(',');

            // Get Price List Data
            priceList.GetListByDateFromDateToVendor(dsPriceList, keys[0], keys[1], keys[2], LoginInfo.ConnStr);

            product.GetLookUp(dsPriceList, LoginInfo.ConnStr);
            vendor.GetList(dsPriceList, LoginInfo.ConnStr);
            //recrieve
            //unit.Get(dsPriceList, LoginInfo.ConnStr);

            Session["dsPriceList"] = dsPriceList;

            Page_Setting();
        }

        /// <summary>
        ///     Page setting
        /// </summary>
        private void Page_Setting()
        {
            // Display template
            var drPLT = dsPriceList.Tables[priceList.TableName].Rows[0];

            lbl_RefNo.Text = drPLT["RefNo"].ToString();
            lbl_VendorNo.Text = drPLT["VendorCode"].ToString();
            lbl_VendorName.Text = vendor.GetName(drPLT["VendorCode"].ToString(), LoginInfo.ConnStr);

            if (drPLT["DateFrom"].ToString() != string.Empty)
            {
                lbl_DateFrom.Text = DateTime.Parse(drPLT["DateFrom"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            }

            if (drPLT["DateTo"].ToString() != string.Empty)
            {
                lbl_DateTo.Text = DateTime.Parse(drPLT["DateTo"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
            }

            // Display template detail
            grd_Prl.DataSource = dsPriceList.Tables[priceList.TableName];
            grd_Prl.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":
                    Response.Redirect("ByVdEdit.aspx?MODE=new");
                    break;

                case "EDIT":
                    Response.Redirect("ByVdEdit.aspx?MODE=edit&ID=" + Request.Params["ID"]);
                    break;

                case "DELETE":
                    if (dsPriceList.Tables[priceList.TableName].Rows.Count > 0)
                    {
                        pop_ConfrimDeleteAll.ShowOnPageLoad = true;
                    }

                    break;

                case "PRINT":
                    // Call Crytal Report
                    break;

                case "BACK":
                    Response.Redirect("ByVdLst.aspx");
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_Detail_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Text.ToUpper())
            {
                case "CREATE":

                    ClearFields();

                    cmb_Product.DataSource = dsPriceList.Tables[product.TableName];
                    cmb_Product.DataBind();

                    cmb_TaxType.Value = vendor.GetTaxType(lbl_VendorNo.Text, LoginInfo.ConnStr);

                    ViewState["IsEdit"] = false;

                    // Display list data
                    pop_Dt.HeaderText = "Price List New Item";
                    pop_Dt.ShowOnPageLoad = true;

                    break;

                case "DELETE":
                    Delete();

                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Prl_Load(object sender, EventArgs e)
        {
            grd_Prl.DataSource = dsPriceList.Tables[priceList.TableName];
            grd_Prl.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Prl_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ClearFields();

            cmb_Product.DataSource = dsPriceList.Tables[product.TableName];
            cmb_Product.DataBind();

            ViewState["IsEdit"] = true;
            AddEditRowPK = e.EditingKeyValue.ToString();

            // Set data to popup.
            var drRowSelect = dsPriceList.Tables[priceList.TableName].Rows.Find(e.EditingKeyValue.ToString());
            cmb_Product.Value = drRowSelect["ProductCode"].ToString();

            if (drRowSelect["ProductCode"].ToString() != string.Empty)
            {
                txt_ProductDesc1.Text = product.GetName(drRowSelect["ProductCode"].ToString(), LoginInfo.ConnStr);
            }

            if (drRowSelect["OrderUnit"].ToString() != string.Empty)
            {
                txt_OrderUnit.Text = unit.GetName(drRowSelect["OrderUnit"].ToString(), LoginInfo.ConnStr);
            }

            if (drRowSelect["AvgPrice"].ToString() != string.Empty)
            {
                spe_AvgPrice.Number = decimal.Parse(drRowSelect["AvgPrice"].ToString());
            }

            if (drRowSelect["LastPrice"].ToString() != string.Empty)
            {
                spe_LastPrice.Number = decimal.Parse(drRowSelect["LastPrice"].ToString());
            }

            if (drRowSelect["QtyFrom"].ToString() != string.Empty)
            {
                spe_QtyFrom.Number = int.Parse(drRowSelect["QtyFrom"].ToString());
            }

            if (drRowSelect["QtyTo"].ToString() != string.Empty)
            {
                spe_QtyTo.Number = int.Parse(drRowSelect["QtyTo"].ToString());
            }

            if (drRowSelect["QuotedPrice"].ToString() != string.Empty)
            {
                spe_QuotePrice.Number = decimal.Parse(drRowSelect["QuotedPrice"].ToString());
            }

            if (drRowSelect["DiscountPercent"].ToString() != string.Empty)
            {
                spe_DiscPer.Number = decimal.Parse(drRowSelect["DiscountPercent"].ToString());
            }

            if (drRowSelect["DiscountAmt"].ToString() != string.Empty)
            {
                spe_DiscAmt.Number = decimal.Parse(drRowSelect["DiscountAmt"].ToString());
            }

            if (drRowSelect["Tax"].ToString() != string.Empty)
            {
                cmb_TaxType.Value = drRowSelect["Tax"].ToString();
            }

            if (drRowSelect["TaxRate"].ToString() != string.Empty)
            {
                spe_TaxRate.Number = decimal.Parse(drRowSelect["TaxRate"].ToString());
            }

            if (drRowSelect["NetAmt"].ToString() != string.Empty)
            {
                spe_NetAmt.Number = decimal.Parse(drRowSelect["NetAmt"].ToString());
            }

            if (drRowSelect["FreeOfCharge"].ToString() != string.Empty)
            {
                spe_FOC.Number = decimal.Parse(drRowSelect["FreeOfCharge"].ToString());
            }

            if (drRowSelect["MarketPrice"].ToString() != string.Empty)
            {
                spe_MarketPrice.Number = decimal.Parse(drRowSelect["MarketPrice"].ToString());
            }

            txt_VProdSKU.Text = drRowSelect["VendorProdCode"].ToString();
            txt_Comment.Text = drRowSelect["Comment"].ToString();

            // Display list data
            pop_Dt.HeaderText = "Edit Price List Item";
            pop_Dt.ShowOnPageLoad = true;
            e.Cancel = true;
        }

        /// <summary>
        ///     Delete selected Unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var columnValues = grd_Prl.GetSelectedFieldValues("PrlNo");

            foreach (Guid delUnitCode in columnValues)
            {
                foreach (DataRow drDeleting in dsPriceList.Tables[priceList.TableName].Rows)
                {
                    if (drDeleting.RowState != DataRowState.Deleted)
                    {
                        if (drDeleting["PrlNo"].ToString() == delUnitCode.ToString())
                        {
                            drDeleting.Delete();
                        }
                    }
                }
            }

            // Save to database
            var saveUnit = priceList.Save(dsPriceList, LoginInfo.ConnStr);

            if (saveUnit)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;

                //this.Page_Retrieve();
                Response.Redirect("ByPdLst.aspx");
            }
        }

        /// <summary>
        ///     Canceling Delete Delivery Point and Deselect all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            grd_Prl.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ConfrimDeleteAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow drDeleting in dsPriceList.Tables[priceList.TableName].Rows)
            {
                if (drDeleting.RowState != DataRowState.Deleted)
                {
                    drDeleting.Delete();
                }
            }

            // Save to database
            var saveUnit = priceList.Save(dsPriceList, LoginInfo.ConnStr);

            if (saveUnit)
            {
                pop_ConfrimDeleteAll.ShowOnPageLoad = false;

                //this.Page_Retrieve();
                Response.Redirect("ByVdLst.aspx");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_CancelDeleteAll_Click(object sender, EventArgs e)
        {
            pop_ConfrimDeleteAll.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        private void ClearFields()
        {
            //Clear Object.
            cmb_Product.Value = string.Empty;
            cmb_TaxType.Value = string.Empty;
            txt_ProductDesc1.Text = string.Empty;
            txt_OrderUnit.Text = string.Empty;
            txt_Comment.Text = string.Empty;
            txt_VProdSKU.Text = string.Empty;
            spe_QtyFrom.Value = string.Empty;
            spe_QtyTo.Value = string.Empty;
            spe_QuotePrice.Value = string.Empty;
            spe_MarketPrice.Value = string.Empty;
            spe_FOC.Value = string.Empty;
            spe_DiscPer.Value = string.Empty;
            spe_DiscAmt.Value = string.Empty;
            spe_NetAmt.Value = string.Empty;
            spe_TaxRate.Value = string.Empty;
            spe_AvgPrice.Value = string.Empty;
            spe_LastPrice.Value = string.Empty;
        }

        /// <summary>
        /// </summary>
        private void SaveData()
        {
            var drData = dsPriceList.Tables[priceList.TableName].Rows[0];
            DataRow drEditing;

            if (Boolean.Parse(ViewState["IsEdit"].ToString()))
            {
                //Edit Date
                drEditing = dsPriceList.Tables[priceList.TableName].Rows.Find(AddEditRowPK);

                drEditing["UpdatedDate"] = ServerDateTime;
                drEditing["UpdatedBy"] = LoginInfo.LoginName;
            }
            else
            {
                //New data
                drEditing = dsPriceList.Tables[priceList.TableName].NewRow();
                drEditing["PrlNo"] = Guid.NewGuid();

                drEditing["RefNo"] = drData["RefNo"].ToString();
                drEditing["VendorCode"] = drData["VendorCode"].ToString();
                drEditing["DateFrom"] = DateTime.Parse(drData["DateFrom"].ToString()).Date.Date;
                drEditing["DateTo"] = DateTime.Parse(drData["DateTo"].ToString()).Date.Date;
                drEditing["CreatedDate"] = ServerDateTime;
                drEditing["CreatedBy"] = LoginInfo.LoginName;
                drEditing["UpdatedDate"] = ServerDateTime;
                drEditing["UpdatedBy"] = LoginInfo.LoginName;
            }

            if (cmb_Product.Value != null)
            {
                if (cmb_Product.Value.ToString() != null)
                {
                    drEditing["ProductCode"] = cmb_Product.Value.ToString();
                    drEditing["OrderUnit"] = product.GetOrderUnit(cmb_Product.Value.ToString(), LoginInfo.ConnStr);
                }
            }

            drEditing["VendorProdCode"] = txt_VProdSKU.Text;

            if (spe_AvgPrice.Number >= 0)
            {
                drEditing["AvgPrice"] = spe_AvgPrice.Number;
            }
            else
            {
                drEditing["AvgPrice"] = DBNull.Value;
            }

            if (spe_LastPrice.Number >= 0)
            {
                drEditing["LastPrice"] = spe_LastPrice.Number;
            }
            else
            {
                drEditing["LastPrice"] = DBNull.Value;
            }

            if (spe_QtyFrom.Number >= 0)
            {
                drEditing["QtyFrom"] = spe_QtyFrom.Number;
            }
            else
            {
                drEditing["QtyFrom"] = DBNull.Value;
            }

            if (spe_QtyTo.Number >= 0)
            {
                drEditing["QtyTo"] = spe_QtyTo.Number;
            }
            else
            {
                drEditing["QtyTo"] = DBNull.Value;
            }

            if (spe_QuotePrice.Number >= 0)
            {
                drEditing["QuotedPrice"] = spe_QuotePrice.Number;
            }
            else
            {
                drEditing["QuotedPrice"] = DBNull.Value;
            }

            if (spe_NetAmt.Number >= 0)
            {
                drEditing["NetAmt"] = spe_NetAmt.Number;
            }
            else
            {
                drEditing["NetAmt"] = DBNull.Value;
            }

            if (spe_VRank.Number > 0)
            {
                drEditing["VendorRank"] = spe_VRank.Number;
            }
            else
            {
                drEditing["VendorRank"] = DBNull.Value;
            }

            if (spe_DiscPer.Number >= 0)
            {
                drEditing["DiscountPercent"] = spe_DiscPer.Number;
            }
            else
            {
                drEditing["DiscountPercent"] = DBNull.Value;
            }

            if (spe_DiscAmt.Number >= 0)
            {
                drEditing["DiscountAmt"] = spe_DiscAmt.Number;
            }
            else
            {
                drEditing["DiscountAmt"] = DBNull.Value;
            }

            if (cmb_TaxType.Value != null)
            {
                drEditing["Tax"] = cmb_TaxType.Value.ToString();
            }

            if (spe_TaxRate.Number >= 0)
            {
                drEditing["TaxRate"] = spe_TaxRate.Number;
            }
            else
            {
                drEditing["TaxRate"] = DBNull.Value;
            }

            if (spe_FOC.Number >= 0)
            {
                drEditing["FreeOfCharge"] = spe_FOC.Number;
            }
            else
            {
                drEditing["FreeOfCharge"] = DBNull.Value;
            }

            if (spe_MarketPrice.Number >= 0)
            {
                drEditing["MarketPrice"] = spe_MarketPrice.Number;
            }
            else
            {
                drEditing["MarketPrice"] = DBNull.Value;
            }

            drEditing["Comment"] = txt_Comment.Text;

            if (Boolean.Parse(ViewState["IsEdit"].ToString()) != true)
            {
                dsPriceList.Tables[priceList.TableName].Rows.Add(drEditing);
            }
        }

        /// <summary>
        ///     Display confrim delete Unit
        /// </summary>
        private void Delete()
        {
            if (grd_Prl.Selection.Count > 0)
            {
                pop_ConfrimDelete.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Update_Click(object sender, EventArgs e)
        {
            if (spe_VRank.Number <= 0)
            {
                lbl_CheckSaveNew.Text = "Vendor Rank must be greater than 0.";
                pop_CheckSaveNew.ShowOnPageLoad = true;
                return;
            }

            // Check request field.
            Page.Validate();

            if (Page.IsValid)
            {
                SaveData();

                var result = priceList.Save(dsPriceList, LoginInfo.ConnStr);

                if (result)
                {
                    pop_Dt.ShowOnPageLoad = false;
                    ViewState["IsEdit"] = false;

                    Session["dsPriceList"] = null;
                    Page_Retrieve();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_UpdateAndNew_Click(object sender, EventArgs e)
        {
            if (spe_VRank.Number <= 0)
            {
                lbl_CheckSaveNew.Text = "Vendor Rank must be greater than 0.";
                pop_CheckSaveNew.ShowOnPageLoad = true;
                return;
            }

            // Check request field.
            Page.Validate();

            if (Page.IsValid)
            {
                SaveData();

                var result = priceList.Save(dsPriceList, LoginInfo.ConnStr);

                if (result)
                {
                    ViewState["IsEdit"] = false;
                    Session["dsPriceList"] = dsPriceList;

                    grd_Prl.DataSource = dsPriceList.Tables[priceList.TableName];
                    grd_Prl.DataBind();
                }
            }

            ClearFields();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Close_Click(object sender, EventArgs e)
        {
            pop_Dt.ShowOnPageLoad = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Product_Load(object sender, EventArgs e)
        {
            cmb_Product.DataSource = dsPriceList.Tables[product.TableName];
            cmb_Product.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Product.Value != null)
            {
                var strCode = cmb_Product.Value.ToString();
                var orderUnit = product.GetOrderUnit(strCode, LoginInfo.ConnStr);
                var orderName = unit.GetName(orderUnit, LoginInfo.ConnStr);

                txt_ProductDesc1.Text = product.GetName(strCode, LoginInfo.ConnStr);
                txt_OrderUnit.Text = orderName;
                cmb_TaxType.Value = product.GetTaxType(strCode, LoginInfo.ConnStr);
                spe_TaxRate.Number = product.GetTaxRate(strCode, LoginInfo.ConnStr);
                spe_AvgPrice.Number = inventory.GetMAvgAudit(strCode, LoginInfo.ConnStr);
                spe_LastPrice.Number = recDt.GetPrice(strCode, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void spe_DiscPer_NumberChanged(object sender, EventArgs e)
        {
            if (spe_DiscPer.Number >= 0)
            {
                if (spe_QuotePrice.Number > 0)
                {
                    decimal chkDisPer;

                    spe_DiscAmt.Value = string.Empty;

                    chkDisPer = (spe_QuotePrice.Number*spe_DiscPer.Number)/100;
                    spe_NetAmt.Number = decimal.Parse((spe_QuotePrice.Number - chkDisPer).ToString());
                }
            }
            else
            {
                spe_NetAmt.Value = string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void spe_DiscAmt_NumberChanged(object sender, EventArgs e)
        {
            if (spe_DiscAmt.Number >= 0)
            {
                if (spe_QuotePrice.Number > 0)
                {
                    spe_DiscPer.Value = string.Empty;

                    spe_NetAmt.Number = decimal.Parse((spe_QuotePrice.Number - spe_DiscAmt.Number).ToString());
                }
            }
            else
            {
                spe_NetAmt.Value = string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void spe_QuotePrice_NumberChanged(object sender, EventArgs e)
        {
            if (spe_DiscPer.Number > 0)
            {
                decimal chkDisPer;

                spe_DiscAmt.Value = string.Empty;

                chkDisPer = (spe_QuotePrice.Number*spe_DiscPer.Number)/100;
                spe_NetAmt.Number = decimal.Parse((spe_QuotePrice.Number - chkDisPer).ToString());
            }
            else if (spe_DiscAmt.Number > 0)
            {
                spe_DiscPer.Value = string.Empty;
                spe_NetAmt.Number = decimal.Parse((spe_QuotePrice.Number - spe_DiscAmt.Number).ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_TaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_TaxType.Value != null)
            {
                var strTax = cmb_TaxType.Value.ToString();
                if (strTax.ToUpper() == "N")
                {
                    spe_TaxRate.Number = 0;
                }
                else
                {
                    spe_TaxRate.Value = string.Empty;
                }
            }
        }

        #endregion
    }
}