using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.PC.PL.Vendor
{
    public partial class VdEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.PC.PL.PL pl = new Blue.BL.PC.PL.PL();
        private readonly Blue.BL.PC.PL.PLDt plDt = new Blue.BL.PC.PL.PLDt();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private DataSet dsPLEdit = new DataSet();
        private Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();

        private string MODE
        {
            get { return Request.Params["MODE"].ToUpper(); }
        }

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
            string getOldMode = MODE;
            string getOldID = PriceLstNo;

            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPLEdit = (DataSet)Session["dsPLEdit"];

            }

        }

        private void Page_Retrieve()
        {
            if (MODE.ToUpper() == "NEW")
            {
                // Get PL Schema
                var getPLSchema = pl.GetSchame(dsPLEdit, LoginInfo.ConnStr);

                if (!getPLSchema)
                {
                    return;
                }

                // Get PLDt Schema
                var getPLDtSchema = plDt.GetSchame(dsPLEdit, LoginInfo.ConnStr);

                if (!getPLDtSchema)
                {
                    return;
                }

                // Add new row to PL DataTable
                var drPL = dsPLEdit.Tables[pl.TableName].NewRow();

                drPL["PriceLstNo"] = pl.GetNewID(LoginInfo.ConnStr);
                drPL["VendorCode"] = string.Empty;
                drPL["DateFrom"] = ServerDateTime;
                drPL["DateTo"] = ServerDateTime.AddDays(30); // Fixed 30 Days : Get from parameter next relase
                drPL["CreatedDate"] = ServerDateTime;
                drPL["CreatedBy"] = LoginInfo.LoginName;
                drPL["UpdatedDate"] = ServerDateTime;
                drPL["UpdatedBy"] = LoginInfo.LoginName;

                dsPLEdit.Tables[pl.TableName].Rows.Add(drPL);
            }
            //else //Original
            else if (MODE.ToUpper() == "EDIT")
            {
                // Get PL Data
                var getPL = pl.Get(dsPLEdit, int.Parse(PriceLstNo), LoginInfo.ConnStr);

                if (!getPL)
                {
                    return;
                }

                // Get PLDt Data
                var getPLDt = plDt.GetList(dsPLEdit, PriceLstNo, LoginInfo.ConnStr);

                if (!getPLDt)
                {
                    return;
                }
            }

            if (Request.Params["VENDOR"] != null)
            {
                ddl_Vendor.Value = Request.Params["VENDOR"];
                getProductList(Request.Params["VENDOR"].ToString());
            }

            Session["dsPLEdit"] = dsPLEdit;
            Page_Setting();
        }

        private void Page_Setting()
        {
            var drPL = dsPLEdit.Tables[pl.TableName].Rows[0];

            ddl_Vendor.Value = drPL["VendorCode"].ToString();
            //txt_Vendor.Text     = vendor.GetName(drPL["VendorCode"].ToString(), LoginInfo.ConnStr);
            if (drPL["DateFrom"] != DBNull.Value)
            {
                txt_DateFrom.Date = DateTime.Parse(drPL["DateFrom"].ToString());
            }
            else
            {
                txt_DateFrom.Text = null;
            }

            if (drPL["DateTo"] != DBNull.Value)
            {
                txt_DateTo.Date = DateTime.Parse(drPL["DateTo"].ToString());
            }
            else
            {
                txt_DateTo.Text = null;
            }
            txt_RefNo.Text = drPL["RefNo"].ToString();

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.DataBind();
        }

        #endregion

        #region "Command Bar"

        protected void Save()
        {

            // Update Header Information
            var drPL = dsPLEdit.Tables[pl.TableName].Rows[0];

            if (MODE.ToUpper() == "NEW")
            {
                drPL["PriceLstNo"] = pl.GetNewID(LoginInfo.ConnStr);

                // Update Detail Information
                foreach (DataRow drPLDt in dsPLEdit.Tables[plDt.TableName].Rows)
                {
                    if (drPLDt.RowState != DataRowState.Deleted)
                    {
                        drPLDt["PriceLstNo"] = drPL["PriceLstNo"].ToString();
                    }
                }
            }

            drPL["RefNo"] = txt_RefNo.Text.Trim();

            if (txt_DateFrom.Text != string.Empty)
            {
                //DateTime.Parse(txt_DateFrom.Date.ToString("dd/MM/yyyy"));
                drPL["DateFrom"] = DateTime.Parse(txt_DateFrom.Date.ToShortDateString());
            }
            else
            {
                drPL["DateFrom"] = DBNull.Value;
            }

            if (txt_DateTo.Text != string.Empty)
            {
                //drPL["DateTo"] = txt_DateTo.Date;
                // DateTime.Parse(txt_DateTo.Date.ToString("dd/MM/yyyy"));
                drPL["DateTo"] = DateTime.Parse(txt_DateTo.Date.ToShortDateString());
            }
            else
            {
                drPL["DateTo"] = DBNull.Value;
            }

            drPL["VendorCode"] = ddl_Vendor.Value.ToString();
            drPL["UpdatedDate"] = ServerDateTime;
            drPL["UpdatedBy"] = LoginInfo.LoginName;


            // Commit to Database
            var result = pl.Save(dsPLEdit, LoginInfo.ConnStr);

            if (result)
            {
                Response.Redirect("Vd.aspx?ID=" + drPL["PriceLstNo"]);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Error in saving!');", true);
            }
        }

        protected void Back()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                Response.Redirect("VdList.aspx");
            }
            else
            {
                Response.Redirect("Vd.aspx?ID=" + Request.Params["ID"]);
            }
        }

        #endregion

        #region "Price List Header"

        #endregion

        #region "Price List Detail"

        protected void Create_PL()
        {
            //this.PLDetailEditMode       = "NEW";
            //pop_PLDtEdit.ShowOnPageLoad = true;

            var drNew = dsPLEdit.Tables[plDt.TableName].NewRow();

            drNew["PriceLstNo"] = dsPLEdit.Tables[pl.TableName].Rows[0]["PriceLstNo"].ToString();
            drNew["SeqNo"] = (dsPLEdit.Tables[plDt.TableName].Rows.Count == 0
                ? 1
                : int.Parse(
                    dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1]["SeqNo"]
                        .ToString()) + 1);
            drNew["ProductCode"] = string.Empty;
            drNew["OrderUnit"] = string.Empty;
            drNew["VendorRank"] = 0;
            drNew["QtyFrom"] = 0.00;
            drNew["QtyTo"] = 0.00;
            drNew["QuotedPrice"] = 0.00;
            drNew["MarketPrice"] = 0.00;
            drNew["FOC"] = 0.00;
            drNew["DiscPercent"] = 0.00;
            drNew["DiscAmt"] = 0.00;
            drNew["Tax"] = "N";
            drNew["TaxRate"] = 0.00;
            drNew["NetAmt"] = 0.00;
            drNew["AvgPrice"] = 0.00;
            drNew["LastPrice"] = 0.00;

            // Added on: 09/08/2017, By: Fon
            drNew["CurrencyCode"] = config.GetConfigValue("APP", "BU", "DefaultCurrency", hf_ConnStr.Value);
            // End Added.

            dsPLEdit.Tables[plDt.TableName].Rows.Add(drNew);

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = dsPLEdit.Tables[plDt.TableName].Rows.Count - 1;
            grd_PLDt.DataBind();

            PLDetailEditMode = "NEW";
            menu_CmdGrd.Items.FindByName("Create").Visible = false;
        }

        protected void DeletePL()
        {
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            for (var i = grd_PLDt.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_PLDt.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                if (Chk_Item.Checked)
                {
                    //dsPR.Tables[prDt.TableName].Rows[dsPR.Tables[prDt.TableName].Rows.Count - 1].Delete();
                    dsPLEdit.Tables[plDt.TableName].Rows[i].Delete();
                }
            }

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = -1;
            grd_PLDt.DataBind();

            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = -1;
            grd_PLDt.DataBind();
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }
        protected void grd_PLDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("ddl_ProductCode") != null)
                {
                    var ddl_ProductCode = e.Row.FindControl("ddl_ProductCode") as ASPxComboBox;

                    //ddl_ProductCode.DataSource = product.GetActiveList(LoginInfo.ConnStr);
                    //ddl_ProductCode.DataBind();

                    ddl_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();

                }


                if (e.Row.FindControl("hf_ProductCode") != null)
                {
                    var hf_ProductCode = e.Row.FindControl("hf_ProductCode") as HiddenField;
                    hf_ProductCode.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }




                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") + " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               hf_ConnStr.Value) + " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               hf_ConnStr.Value);
                }

                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl_Unit = e.Row.FindControl("ddl_Unit") as ASPxComboBox;
                    var ddl_ProductCode = e.Row.FindControl("ddl_ProductCode") as ASPxComboBox;

                    if (ddl_ProductCode != null && ddl_ProductCode.Value != null)
                    {
                        ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(ddl_ProductCode.Value.ToString(),
                            LoginInfo.ConnStr);
                        ddl_Unit.Value = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                        ddl_Unit.DataBind();
                    }
                }
                if (e.Row.FindControl("lbl_UnitCode") != null)
                {
                    var lbl_UnitCode = e.Row.FindControl("lbl_UnitCode") as Label;
                    lbl_UnitCode.Text = DataBinder.Eval(e.Row.DataItem, "OrderUnit").ToString();
                }

                if (e.Row.FindControl("txt_VendorRank") != null)
                {
                    var txt_VendorRank = e.Row.FindControl("txt_VendorRank") as ASPxSpinEdit;
                    txt_VendorRank.Text = DataBinder.Eval(e.Row.DataItem, "VendorRank").ToString();
                }
                if (e.Row.FindControl("lbl_VendorRank") != null)
                {
                    var lbl_VendorRank = e.Row.FindControl("lbl_VendorRank") as Label;
                    lbl_VendorRank.Text = DataBinder.Eval(e.Row.DataItem, "VendorRank").ToString();
                }

                if (e.Row.FindControl("txt_QtyFrom") != null)
                {
                    var txt_QtyFrom = e.Row.FindControl("txt_QtyFrom") as ASPxSpinEdit;
                    txt_QtyFrom.Text = DataBinder.Eval(e.Row.DataItem, "QtyFrom").ToString();
                }
                if (e.Row.FindControl("lbl_QtyFrom") != null)
                {
                    var lbl_QtyFrom = e.Row.FindControl("lbl_QtyFrom") as Label;
                    lbl_QtyFrom.Text = DataBinder.Eval(e.Row.DataItem, "QtyFrom").ToString();
                }

                if (e.Row.FindControl("txt_QtyTo") != null)
                {
                    var txt_QtyTo = e.Row.FindControl("txt_QtyTo") as ASPxSpinEdit;
                    txt_QtyTo.Text = DataBinder.Eval(e.Row.DataItem, "QtyTo").ToString();
                }
                if (e.Row.FindControl("lbl_QtyTo") != null)
                {
                    var lbl_QtyTo = e.Row.FindControl("lbl_QtyTo") as Label;
                    lbl_QtyTo.Text = DataBinder.Eval(e.Row.DataItem, "QtyTo").ToString();
                }

                if (e.Row.FindControl("txt_QuotedPrice") != null)
                {
                    var txt_QuotedPrice = e.Row.FindControl("txt_QuotedPrice") as ASPxSpinEdit;
                    txt_QuotedPrice.Text = DataBinder.Eval(e.Row.DataItem, "QuotedPrice").ToString();
                }
                if (e.Row.FindControl("lbl_QuotedPrice") != null)
                {
                    var lbl_QuotedPrice = e.Row.FindControl("lbl_QuotedPrice") as Label;
                    lbl_QuotedPrice.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "QuotedPrice")).ToString("#,0.0000");
                }

                if (e.Row.FindControl("txt_MarketPrice") != null)
                {
                    var txt_MarketPrice = e.Row.FindControl("txt_MarketPrice") as ASPxSpinEdit;
                    txt_MarketPrice.Text = DataBinder.Eval(e.Row.DataItem, "MarketPrice").ToString();
                }
                if (e.Row.FindControl("lbl_MarketPrice") != null)
                {
                    var lbl_MarketPrice = e.Row.FindControl("lbl_MarketPrice") as Label;
                    lbl_MarketPrice.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MarketPrice")).ToString("#,##0.00");
                }

                if (e.Row.FindControl("txt_FOC") != null)
                {
                    var txt_FOC = e.Row.FindControl("txt_FOC") as ASPxSpinEdit;
                    txt_FOC.Text = DataBinder.Eval(e.Row.DataItem, "FOC").ToString();
                }
                if (e.Row.FindControl("lbl_FOC") != null)
                {
                    var lbl_FOC = e.Row.FindControl("lbl_FOC") as Label;
                    lbl_FOC.Text = DataBinder.Eval(e.Row.DataItem, "FOC").ToString();
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }
                if (e.Row.FindControl("lbl_Comment") != null)
                {
                    var lbl_Comment = e.Row.FindControl("lbl_Comment") as Label;
                    lbl_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    lbl_Comment.ToolTip = lbl_Comment.Text;
                }

                if (e.Row.FindControl("txt_DiscPercent") != null)
                {
                    var txt_DiscPercent = e.Row.FindControl("txt_DiscPercent") as ASPxSpinEdit;
                    txt_DiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }
                if (e.Row.FindControl("lbl_DiscPercent") != null)
                {
                    var lbl_DiscPercent = e.Row.FindControl("lbl_DiscPercent") as Label;
                    lbl_DiscPercent.Text = DataBinder.Eval(e.Row.DataItem, "DiscPercent").ToString();
                }

                if (e.Row.FindControl("txt_DiscAmt") != null)
                {
                    var txt_DiscAmt = e.Row.FindControl("txt_DiscAmt") as ASPxSpinEdit;
                    txt_DiscAmt.Text = DataBinder.Eval(e.Row.DataItem, "DiscAmt").ToString();
                }
                if (e.Row.FindControl("lbl_DiscAmt") != null)
                {
                    var lbl_DiscAmt = e.Row.FindControl("lbl_DiscAmt") as Label;
                    lbl_DiscAmt.Text = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DiscAmt")).ToString("#,##0.00");
                }

                if (e.Row.FindControl("ddl_TaxType") != null)
                {
                    var txt_TaxType = e.Row.FindControl("ddl_TaxType") as ASPxComboBox;
                    txt_TaxType.Text = DataBinder.Eval(e.Row.DataItem, "Tax").ToString();
                }
                if (e.Row.FindControl("lbl_TaxType") != null)
                {
                    var lbl_TaxType = e.Row.FindControl("lbl_TaxType") as Label;
                    lbl_TaxType.Text = DataBinder.Eval(e.Row.DataItem, "Tax").ToString();
                }

                if (e.Row.FindControl("txt_TaxRate") != null)
                {
                    var txt_TaxRate = e.Row.FindControl("txt_TaxRate") as ASPxSpinEdit;
                    txt_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }
                if (e.Row.FindControl("lbl_TaxRate") != null)
                {
                    var lbl_TaxRate = e.Row.FindControl("lbl_TaxRate") as Label;
                    lbl_TaxRate.Text = DataBinder.Eval(e.Row.DataItem, "TaxRate").ToString();
                }

                if (e.Row.FindControl("txt_VendorProdCode") != null)
                {
                    var txt_VendorProdCode = e.Row.FindControl("txt_VendorProdCode") as TextBox;
                    txt_VendorProdCode.Text = DataBinder.Eval(e.Row.DataItem, "VendorProdCode").ToString();
                }
                if (e.Row.FindControl("lbl_VendorProdCode") != null)
                {
                    var lbl_VendorProdCode = e.Row.FindControl("lbl_VendorProdCode") as Label;
                    lbl_VendorProdCode.Text = DataBinder.Eval(e.Row.DataItem, "VendorProdCode").ToString();
                }

                // Added on: 09/08/2017, By: Fon
                if (e.Row.FindControl("lbl_CurrCode") != null)
                {
                    Label lbl_CurrCode = (Label)e.Row.FindControl("lbl_CurrCode");
                    lbl_CurrCode.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }

                if (e.Row.FindControl("comb_CurrCode") != null)
                {
                    var comb_CurrCode = (ASPxComboBox)e.Row.FindControl("comb_CurrCode");
                    comb_CurrCode.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
                    comb_CurrCode.DataBind();

                    comb_CurrCode.Value = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }
                // End Added

                if (e.Row.FindControl("txt_NetAmt") != null)
                {
                    var txt_NetAmt = e.Row.FindControl("txt_NetAmt") as ASPxSpinEdit;
                    txt_NetAmt.Text = DataBinder.Eval(e.Row.DataItem, "NetAmt").ToString();
                }
                if (e.Row.FindControl("lbl_NetAmt") != null)
                {
                    var lbl_NetAmt = e.Row.FindControl("lbl_NetAmt") as Label;
                    lbl_NetAmt.Text = String.Format("{0:#,##0.00}", DataBinder.Eval(e.Row.DataItem, "NetAmt"));
                }

                if (e.Row.FindControl("txt_AvgPrice") != null)
                {
                    var txt_AvgPrice = e.Row.FindControl("txt_AvgPrice") as ASPxSpinEdit;
                    txt_AvgPrice.Text = DataBinder.Eval(e.Row.DataItem, "AvgPrice").ToString();
                }
                if (e.Row.FindControl("lbl_AvgPrice") != null)
                {
                    var lbl_AvgPrice = e.Row.FindControl("lbl_AvgPrice") as Label;
                    lbl_AvgPrice.Text = String.Format("{0:#,##0.00}", DataBinder.Eval(e.Row.DataItem, "AvgPrice"));
                }

                if (e.Row.FindControl("txt_LastPrice") != null)
                {
                    var txt_LastPrice = e.Row.FindControl("txt_LastPrice") as ASPxSpinEdit;
                    txt_LastPrice.Text = DataBinder.Eval(e.Row.DataItem, "LastPrice").ToString();
                }
                if (e.Row.FindControl("lbl_LastPrice") != null)
                {
                    var lbl_LastPrice = e.Row.FindControl("lbl_LastPrice") as Label;
                    lbl_LastPrice.Text = String.Format("{0:#,##0.00}", DataBinder.Eval(e.Row.DataItem, "LastPrice"));
                }
            }
        }

        protected void grd_PLDt_RowEditing(object sender, GridViewEditEventArgs e)
        {

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = e.NewEditIndex;
            grd_PLDt.DataBind();

            PLDetailEditMode = "EDIT";

            hf_ProductCode.Value = (grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox).Value.ToString();

            // Added on: 09/08/2017, By: Fon
            menu_CmdGrd.Items.FindByName("Create").Visible = false;
            menu_CmdGrd.Items.FindByName("Delete").Visible = false;
            menu_CmdBar.Items.FindByName("Save").Visible = false;
            menu_CmdBar.Items.FindByName("Back").Visible = false;
        }

        protected void grd_PLDt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            var drInserting = dsPLEdit.Tables[plDt.TableName].Rows[grd_PLDt.Rows[grd_PLDt.EditIndex].DataItemIndex];

            var ddl_ProductCode = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_ProductCode") as ASPxComboBox;
            drInserting["ProductCode"] = ddl_ProductCode.Value.ToString();

            var ddl_Unit = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
            if (ddl_Unit.Value == null)
                drInserting["OrderUnit"] = ddl_Unit.Items[0].Value;
            else
                drInserting["OrderUnit"] = ddl_Unit.Text;

            var txt_VendorRank = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_VendorRank") as ASPxSpinEdit;
            drInserting["VendorRank"] = txt_VendorRank.Value;

            var txt_QtyFrom = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QtyFrom") as ASPxSpinEdit;
            drInserting["QtyFrom"] = txt_QtyFrom.Value;

            var txt_QtyTo = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QtyTo") as ASPxSpinEdit;
            drInserting["QtyTo"] = txt_QtyTo.Value;

            var txt_QuotedPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QuotedPrice") as ASPxSpinEdit;
            drInserting["QuotedPrice"] = txt_QuotedPrice.Value;

            var txt_MarketPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_MarketPrice") as ASPxSpinEdit;
            drInserting["MarketPrice"] = txt_MarketPrice.Value;

            var txt_FOC = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_FOC") as ASPxSpinEdit;
            drInserting["FOC"] = txt_FOC.Value;

            var txt_Comment = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_Comment") as TextBox;
            drInserting["Comment"] = txt_Comment.Text;

            var txt_DiscPercent = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscPercent") as ASPxSpinEdit;
            drInserting["DiscPercent"] = txt_DiscPercent.Value;

            var txt_DiscAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscAmt") as ASPxSpinEdit;
            drInserting["DiscAmt"] = txt_DiscAmt.Value;

            var ddl_TaxType = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_TaxType") as ASPxComboBox;
            if (ddl_TaxType.Value != null)
            {
                drInserting["Tax"] = ddl_TaxType.Value;
            }

            var txt_TaxRate = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_TaxRate") as ASPxSpinEdit;
            if (txt_TaxRate != null)
            {
                if (ddl_TaxType.Value.ToString() == "N")
                    drInserting["TaxRate"] = 0;
                else
                    drInserting["TaxRate"] = txt_TaxRate.Value;
            }
            else
            {
                pop_AlertTaxRate.ShowOnPageLoad = true;
                return;
            }

            var txt_VendorProdCode = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_VendorProdCode") as TextBox;
            drInserting["VendorProdCode"] = txt_VendorProdCode.Text;

            var txt_NetAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_NetAmt") as ASPxSpinEdit;
            drInserting["NetAmt"] = txt_NetAmt.Value;

            var txt_AvgPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_AvgPrice") as ASPxSpinEdit;
            drInserting["AvgPrice"] = txt_AvgPrice.Value == null ? 0m : txt_AvgPrice.Value;

            var txt_LastPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_LastPrice") as ASPxSpinEdit;
            drInserting["LastPrice"] = txt_LastPrice.Value;

            // Added on: 09/08/2017, By: Fon
            var comb_CurrCode = (ASPxComboBox)grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("comb_CurrCode");
            drInserting["CurrencyCode"] = comb_CurrCode.Value.ToString();
            // End Added.

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = -1;
            grd_PLDt.DataBind();

            PLDetailEditMode = string.Empty;
            menu_CmdGrd.Items.FindByName("Create").Visible = true;
            menu_CmdGrd.Items.FindByName("Delete").Visible = true;
            menu_CmdBar.Items.FindByName("Save").Visible = true;
            menu_CmdBar.Items.FindByName("Back").Visible = true;

        }

        protected void grd_PLDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            menu_CmdBar.Items.FindByName("Save").Visible = true;
            menu_CmdBar.Items.FindByName("Back").Visible = true;

            if (MODE.ToUpper() == "NEW")
            {
                if (PLDetailEditMode == "NEW")
                {
                    dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1].Delete();
                }
                if (PLDetailEditMode == "EDIT")
                {
                    dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (MODE.ToUpper() == "EDIT")
            {
                if (PLDetailEditMode == "NEW")
                {
                    dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1].Delete();
                }
                if (PLDetailEditMode == "EDIT")
                {
                    dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_PLDt.DataSource = dsPLEdit.Tables[plDt.TableName];
            grd_PLDt.EditIndex = -1;
            grd_PLDt.DataBind();

            PLDetailEditMode = string.Empty;
            menu_CmdGrd.Items.FindByName("Create").Visible = true;
            menu_CmdGrd.Items.FindByName("Delete").Visible = true;
        }

        protected void ddl_ProductCode_Load(object sender, EventArgs e)
        {
            var ddl_ProductCode = sender as ASPxComboBox;
            ddl_ProductCode.DataSource = product.GetList(LoginInfo.ConnStr);
            ddl_ProductCode.DataBind();
        }

        protected void ddl_ProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var hf_ProductCode = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("hf_ProductCode") as HiddenField;
            var ddl_ProductCode = sender as ASPxComboBox;

            if (hf_ProductCode.Value.ToString() == ddl_ProductCode.Value.ToString())
                return;


            // Order Unit
            var ddl_Unit = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_Unit") as ASPxComboBox;
            string oldUnit = string.Empty;
            if (ddl_Unit.Value != null)
                oldUnit = ddl_Unit.SelectedItem.Value.ToString();

            if (ddl_Unit != null)
            {
                ddl_Unit.DataSource = prodUnit.GetLookUp_ProductCode(ddl_ProductCode.Value.ToString(), LoginInfo.ConnStr);
                ddl_Unit.DataBind();
                if (ddl_Unit.Items.Count > 0)
                    ddl_Unit.SelectedIndex = 0;

                if (oldUnit != string.Empty)
                {
                    ListEditItem item = ddl_Unit.Items.FindByValue(oldUnit);
                    if (item != null)
                        ddl_Unit.SelectedIndex = item.Index;
                }


            }

            // Tax
            var ddl_TaxType = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_TaxType") as ASPxComboBox;
            if (ddl_TaxType != null)
            {
                ddl_TaxType.Value = product.GetTaxType(ddl_ProductCode.Value.ToString(), LoginInfo.ConnStr);
            }
            var txt_TaxRate = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_TaxRate") as ASPxSpinEdit;
            if (txt_TaxRate != null)
            {
                txt_TaxRate.Text = product.GetTaxRate(ddl_ProductCode.Value.ToString(), LoginInfo.ConnStr).ToString();
            }


            // Set value of hidden Fields
            hf_ProductCode.Value = ddl_ProductCode.Value.ToString();

        }
        

        protected void ddl_Unit_Load(object sender, EventArgs e)
        {
        }

        protected void comb_CurrCode_Init(object sender, EventArgs e)
        {
            var comb_CurrCode = (ASPxComboBox)sender;
            comb_CurrCode.DataSource = currency.GetLastCurrencyRate(LoginInfo.ConnStr);
            comb_CurrCode.DataBind();
        }
        #endregion

        #region "Edit Form"

        protected void btn_Cancel_PLDt_Click(object sender, EventArgs e)
        {
            PLDetailEditMode = "";
            //pop_PLDtEdit.ShowOnPageLoad = false;
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddl_OrderUnit.Value = product.GetOrderUnit(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
        }

        protected void txt_QuotedPrice_NumberChanged(object sender, EventArgs e)
        {
            // Recalculate NetAmt
            UpdateNetAmount();
        }

        protected void txt_DiscPercent_NumberChanged(object sender, EventArgs e)
        {
            // Recalculate DiscountAmt
            decimal Price = 0;
            decimal DiscPercent = 0;

            var txt_QuotedPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QuotedPrice") as ASPxSpinEdit;

            if (!string.IsNullOrEmpty(txt_QuotedPrice.Text))
            {
                Price = decimal.Parse(txt_QuotedPrice.Text);
            }

            var txt_DiscPercent = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscPercent") as ASPxSpinEdit;

            if (!string.IsNullOrEmpty(txt_DiscPercent.Text))
            {
                DiscPercent = decimal.Parse(txt_DiscPercent.Text);
            }

            var txt_DiscAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscAmt") as ASPxSpinEdit;
            txt_DiscAmt.Text = ((Price * DiscPercent) / 100).ToString();

            // Recalculate NetAmt
            UpdateNetAmount();
        }

        // Added on: 27/02/2018, By: Fon, For: Following from P'Oat request.
        protected void txt_DiscAmt_NumberChanged(object sender, EventArgs e)
        {
            decimal price = 0;
            decimal discPercent = 0;
            decimal discAmt = 0;

            var txt_QuotedPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QuotedPrice") as ASPxSpinEdit;
            var txt_DiscAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscAmt") as ASPxSpinEdit;
            var txt_DiscPercent = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscPercent") as ASPxSpinEdit;

            price = (!string.IsNullOrEmpty(txt_QuotedPrice.Text)) ? decimal.Parse(txt_QuotedPrice.Text) : 0;
            discAmt = (!string.IsNullOrEmpty(txt_DiscAmt.Text)) ? decimal.Parse(txt_DiscAmt.Text) : 0;

            if (price > 0)
            {
                discPercent = (discAmt / price) * 100;
                txt_DiscPercent.Text = string.Format("{0}", discPercent);

                // Just Try on: 27/02/2018
                lbl_PriceVendor_Nm.Text = string.Format("{0}", discPercent);
            }

            UpdateNetAmount();
        }
        // End Added.

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

        protected void comb_CurrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNetAmount();
        }

        private void UpdateNetAmount()
        {
            decimal DiscAmt = 0;
            decimal TaxRate = 0;
            decimal Price = 0;

            var txt_DiscAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscAmt") as ASPxSpinEdit;
            var txt_TaxRate = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_TaxRate") as ASPxSpinEdit;
            var txt_QuotedPrice = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_QuotedPrice") as ASPxSpinEdit;
            var ddl_TaxType = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("ddl_TaxType") as ASPxComboBox;
            var txt_NetAmt = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_NetAmt") as ASPxSpinEdit;

            // Added on: 02/02/2018, By: Fon
            decimal DiscPercent = 0;
            var txt_DiscPercent = grd_PLDt.Rows[grd_PLDt.EditIndex].FindControl("txt_DiscPercent") as ASPxSpinEdit;
            // End Added.

            if (!string.IsNullOrEmpty(txt_QuotedPrice.Text))
            {
                Price = decimal.Parse(txt_QuotedPrice.Text);
            }

            if (!string.IsNullOrEmpty(txt_DiscAmt.Text))
            {
                // Added on: 02/02/2018, By: Fon
                DiscPercent = decimal.Parse(txt_DiscPercent.Text);
                txt_DiscAmt.Text = ((Price * DiscPercent) / 100).ToString();
                // End Added.

                DiscAmt = decimal.Parse(txt_DiscAmt.Text);
            }

            //if (!string.IsNullOrEmpty(txt_TaxRate.Text))
            //{
            //    TaxRate = decimal.Parse(txt_TaxRate.Text);
            //}
            TaxRate = txt_TaxRate.Number;

            // Recalculate NetAmt
            if (ddl_TaxType.Value != null)
            {
                // Note: Now U have currency. So, I change @price to @priceRate
                switch (ddl_TaxType.Value.ToString().ToUpper())
                {
                    //case "A":
                    //    txt_NetAmt.Text = ((Price - DiscAmt) + (((Price - DiscAmt) * TaxRate) / 100)).ToString();
                    //    break;

                    case "I":
                        //txt_NetAmt.Text = ((Price - DiscAmt) - (((Price - DiscAmt) * TaxRate) / (100 + TaxRate))).ToString();
                        txt_NetAmt.Value = (Price - DiscAmt) - (((Price - DiscAmt) * TaxRate) / (100 + TaxRate));

                        break;

                    default:
                        //txt_NetAmt.Text = (Price - DiscAmt).ToString();
                        txt_NetAmt.Value = Price - DiscAmt;
                        break;
                }
            }
            else
            {
                txt_NetAmt.Value = Price - DiscAmt;
            }
        }

        #endregion

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;
                case "BACK":
                    Back();
                    break;
            }
        }

        protected void menu_CmdGrd_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    //lbl_W_vendorCode.Visible = false;
                    Create_PL();
                    break;

                //case "GETLIST":
                //    if (ddl_Vendor.SelectedIndex > 0)
                //    {
                //        getProductList(ddl_Vendor.SelectedItem.Value.ToString());
                //        //menu_CmdGrd.Items.FindByName("GetList").Visible = false;
                //    }
                //    else
                //    {
                //        lbl_W_vendorCode.Visible = true;
                //        //dialog.MessageBox("btnDialog", "Alert!", "Vendor is required.", "OK", "Warning");
                //        //dialog.ShowMessageBox();
                //    }
                //    break;

                case "DELETE":
                    DeletePL();
                    break;
            }
        }

        protected void btn_OK_Tax_Click(object sender, EventArgs e)
        {
            pop_AlertTaxRate.ShowOnPageLoad = false;
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {

        }

        #region "GetProductListByVendor"
        protected void getProductList(string vendorCode)
        {
            string connectionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connectionString);

            DataTable dtProductList = new DataTable();
            try
            {
                cnn.Open();
                string strSql = "select DISTINCT ProductCode from [IN].VendorProd";
                strSql += " WHERE VendorCode = '" + vendorCode + "'";

                SqlCommand cmd = new SqlCommand(strSql, cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dtProductList);
            }
            catch
            { cnn.Close(); }

            for (int i = 0; i < dtProductList.Rows.Count; i++)
            {
                var drNew = dsPLEdit.Tables[plDt.TableName].NewRow();

                drNew["PriceLstNo"] = dsPLEdit.Tables[pl.TableName].Rows[0]["PriceLstNo"].ToString();
                drNew["SeqNo"] = (dsPLEdit.Tables[plDt.TableName].Rows.Count == 0
                    ? 1
                    : int.Parse(
                        dsPLEdit.Tables[plDt.TableName].Rows[dsPLEdit.Tables[plDt.TableName].Rows.Count - 1]["SeqNo"]
                            .ToString()) + 1);
                drNew["ProductCode"] = dtProductList.Rows[i]["ProductCode"].ToString();
                drNew["OrderUnit"] = string.Empty;
                drNew["VendorRank"] = 0;
                drNew["QtyFrom"] = 0.00;
                drNew["QtyTo"] = 0.00;
                drNew["QuotedPrice"] = 0.00;
                drNew["MarketPrice"] = 0.00;
                drNew["FOC"] = 0.00;
                drNew["DiscPercent"] = 0.00;
                drNew["DiscAmt"] = 0.00;
                drNew["TaxRate"] = 0.00;
                drNew["NetAmt"] = 0.00;
                drNew["AvgPrice"] = 0.00;
                drNew["LastPrice"] = 0.00;

                // Added on: 20/03/2018
                drNew["CurrencyCode"] = config.GetValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr);
                // End Added.

                dsPLEdit.Tables[plDt.TableName].Rows.Add(drNew);

                PLDetailEditMode = "NEW";
                menu_CmdGrd.Items.FindByName("Create").Visible = false;
            }
        }
        #endregion


    }
}