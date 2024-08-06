using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.IN.Transfer
{
    public partial class TrfEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.Option.Inventory.StoreLct locat = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.Transfer trf = new Blue.BL.IN.Transfer();
        private readonly Blue.BL.IN.TransferDt trfDt = new Blue.BL.IN.TransferDt();
        private int TrfId;
        private DataSet dsTrfEdit = new DataSet();
        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();

        private string TrfEditMode
        {
            get { return Session["TrfEditMode"].ToString(); }
            set { Session["TrfEditMode"] = value; }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsTrfEdit = (DataSet) Session["dsTrfEdit"];
            }
        }

        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                trf.GetStructure(dsTrfEdit, LoginInfo.ConnStr);
                trfDt.GetStructure(dsTrfEdit, LoginInfo.ConnStr);
            }
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                trf.GetListById(dsTrfEdit, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
                trfDt.GetListByHeaderId(dsTrfEdit, int.Parse(Request.Params["ID"]), LoginInfo.ConnStr);
            }
            else if (Request.Params["MODE"].ToUpper() == "SR")
            {
                dsTrfEdit = (DataSet) Session["dsStoreReqDt"];
            }

            locat.GetList(dsTrfEdit, LoginInfo.ConnStr);
            Session["dsTrfEdit"] = dsTrfEdit;

            Page_Setting();
        }

        private void Page_Setting()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                txt_DeliDate.Text = ServerDateTime.ToShortDateString();
                de_ReqDate.Date = ServerDateTime.Date.AddDays(1);
                td_Ref.Visible = false;
                td_RefName.Visible = false;
                td_Delete.Visible = false;
            }
            else if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                var drStoreReq = dsTrfEdit.Tables[trf.TableName].Rows[0];

                ddl_Store.Enabled = false;
                ddl_Store.DataSource = dsTrfEdit.Tables[locat.TableName];
                ddl_Store.ValueField = "LocationCode";
                ddl_Store.Value = drStoreReq["LocationCode"].ToString();
                ddl_Store.DataBind();

                txt_Desc.Text = drStoreReq["Description"].ToString();
                de_ReqDate.Date = ServerDateTime.AddDays(1);
                txt_DeliDate.Text =
                    DateTime.Parse(drStoreReq["DeliveryDate"].ToString()).ToString(LoginInfo.BuFmtInfo.FmtSDate);
                //txt_DeliDate.Enabled    = false;
                lbl_Ref.Text = drStoreReq["RequestCode"].ToString();
                lbl_Status.Text = drStoreReq["DocStatus"].ToString();
            }
            else if (Request.Params["MODE"].ToUpper() == "SR")
            {
                txt_Desc.Text = dsTrfEdit.Tables[trf.TableName].Rows[0]["Description"].ToString();
                de_ReqDate.Date = DateTime.Parse(dsTrfEdit.Tables[trf.TableName].Rows[0]["DeliveryDate"].ToString());
                td_Ref.Visible = false;
                td_RefName.Visible = false;
                ddl_Store.Enabled = false;
                ddl_Store.DataSource = dsTrfEdit.Tables[locat.TableName];
                ddl_Store.ValueField = "LocationCode";
                ddl_Store.DataBind();
                ddl_Store.Value = dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString();
                TrfEditMode = "Edit";
            }

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.DataBind();
        }

        protected void grd_TrfEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (TrfEditMode.ToUpper() == "NEW")
                {
                    dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1].Delete();
                }

                if (TrfEditMode.ToUpper() == "EDIT")
                {
                    dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (TrfEditMode.ToUpper() == "NEW")
                {
                    dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1].Delete();
                }

                if (TrfEditMode.ToUpper() == "EDIT")
                {
                    dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = -1;
            grd_TrfEdit.DataBind();

            de_ReqDate.Enabled = true;
            btn_Save.Enabled = true;
            TrfEditMode = string.Empty;
        }

        protected void grd_TrfEdit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            btn_Save.Enabled = false;
            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = e.NewEditIndex;
            grd_TrfEdit.DataBind();

            for (var i = grd_TrfEdit.Rows.Count - 1; i >= 0; i--)
            {
                var Chk_Item = grd_TrfEdit.Rows[i].Cells[0].FindControl("Chk_Item") as CheckBox;
                var Chk_All = grd_TrfEdit.HeaderRow.FindControl("Chk_All") as CheckBox;

                Chk_All.Enabled = false;
                Chk_Item.Enabled = false;
            }

            TrfEditMode = "Edit";
        }

        protected void grd_TrfEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var ddl_gStore = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;
            var ddl_Product = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_Debit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
            var ddl_Credit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
            var txt_QtyRequested = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("txt_QtyRequested") as TextBox;
            var txt_Comment = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("txt_Comment") as TextBox;
            var de_gReqDate = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;
            var lbl_Unit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("lbl_Unit") as Label;


            //Check field cannot empty
            if (ddl_gStore != null)
            {
                if (ddl_gStore.Value == null)
                {
                    lbl_Warning.Text = "Please select 'Request From'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
            }

            if (ddl_Product != null)
            {
                if (ddl_Product.Value == null)
                {
                    lbl_Warning.Text = "Please select 'SKU #'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(200);
                    return;
                }
            }

            if (txt_QtyRequested != null)
            {
                decimal number;

                if (txt_QtyRequested.Text == string.Empty)
                {
                    lbl_Warning.Text = "Please entry 'Qty Requested'";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
                if (!decimal.TryParse(txt_QtyRequested.Text, out number) || decimal.Parse(txt_QtyRequested.Text) <= 0)
                {
                    //check number
                    lbl_Warning.Text = "Please entry 'Qty Requested' only 1-9";
                    pop_Warning.ShowOnPageLoad = true;
                    pop_Warning.Width = Unit.Pixel(250);
                    return;
                }
            }

            //Update Date
            var drUpdating =
                dsTrfEdit.Tables[trfDt.TableName].Rows[grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].DataItemIndex];
            drUpdating["ToLocationCode"] = ddl_gStore.Value;
            drUpdating["DeliveryDate"] = de_gReqDate.Date;
            drUpdating["CategoryCode"] = product.GetProductCategory(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
            drUpdating["ProductCode"] = ddl_Product.Value;
            drUpdating["RequestQty"] = decimal.Parse(txt_QtyRequested.Text);
            drUpdating["RequestUnit"] = lbl_Unit.Text;
            drUpdating["DebitACCode"] = null;
            drUpdating["CreditACCode"] = null;
            drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = -1;
            grd_TrfEdit.DataBind();

            if (Request.Params["MODE"].ToUpper() == "EDIT" && TrfEditMode.ToUpper() == "NEW")
            {
                drUpdating["DocumentId"] = dsTrfEdit.Tables[trf.TableName].Rows[0]["RefId"].ToString();
            }

            btn_Save.Enabled = true;
            TrfEditMode = string.Empty;
            td_Delete.Visible = true;
            de_ReqDate.Enabled = true;
        }

        protected void grd_TrfEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW")
            {
                var ddl_gStore = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;
                var ddl_Product = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
                var ddl_Debit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Debit") as ASPxComboBox;
                var ddl_Credit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Credit") as ASPxComboBox;
                var txt_QtyRequested =
                    grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("txt_QtyRequested") as TextBox;
                var txt_Comment = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("txt_Comment") as TextBox;
                var de_gReqDate = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("de_gReqDate") as ASPxDateEdit;
                var lbl_Unit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("lbl_Unit") as Label;


                //Check field cannot empty
                if (ddl_gStore != null)
                {
                    if (ddl_gStore.Value == null)
                    {
                        lbl_Warning.Text = "Please select 'Request From'";
                        pop_Warning.ShowOnPageLoad = true;
                        pop_Warning.Width = Unit.Pixel(250);
                        return;
                    }
                }

                if (ddl_Product != null)
                {
                    if (ddl_Product.Value == null)
                    {
                        lbl_Warning.Text = "Please select 'SKU #'";
                        pop_Warning.ShowOnPageLoad = true;
                        pop_Warning.Width = Unit.Pixel(200);
                        return;
                    }
                }

                if (txt_QtyRequested != null)
                {
                    decimal number;

                    if (txt_QtyRequested.Text == string.Empty)
                    {
                        lbl_Warning.Text = "Please entry 'Qty Requested'";
                        pop_Warning.ShowOnPageLoad = true;
                        pop_Warning.Width = Unit.Pixel(250);
                        return;
                    }
                    if (!decimal.TryParse(txt_QtyRequested.Text, out number) ||
                        decimal.Parse(txt_QtyRequested.Text) <= 0)
                    {
                        //check number
                        lbl_Warning.Text = "Please entry 'Qty Requested' only 1-9";
                        pop_Warning.ShowOnPageLoad = true;
                        pop_Warning.Width = Unit.Pixel(250);
                        return;
                    }
                }

                //Update Date
                var drUpdating =
                    dsTrfEdit.Tables[trfDt.TableName].Rows[grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].DataItemIndex];
                drUpdating["ToLocationCode"] = ddl_gStore.Value;
                drUpdating["DeliveryDate"] = de_gReqDate.Date;
                drUpdating["CategoryCode"] = product.GetProductCategory(ddl_Product.Value.ToString(), LoginInfo.ConnStr);
                drUpdating["ProductCode"] = ddl_Product.Value;
                drUpdating["RequestQty"] = decimal.Parse(txt_QtyRequested.Text);
                drUpdating["RequestUnit"] = lbl_Unit.Text;
                drUpdating["DebitACCode"] = null;
                drUpdating["CreditACCode"] = null;
                drUpdating["Comment"] = txt_Comment.Text == string.Empty ? null : txt_Comment.Text;

                grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
                grd_TrfEdit.EditIndex = -1;
                grd_TrfEdit.DataBind();

                if (Request.Params["MODE"].ToUpper() == "EDIT" && TrfEditMode.ToUpper() == "NEW")
                {
                    drUpdating["DocumentId"] = dsTrfEdit.Tables[trf.TableName].Rows[0]["RefId"].ToString();
                }

                btn_Save.Enabled = true;
                TrfEditMode = string.Empty;
                td_Delete.Visible = true;

                Create();
            }
        }

        protected void grd_TrfEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dsTrfEdit.Tables[trfDt.TableName].Rows.Count > 0)
            {
                for (var i = 0; i < dsTrfEdit.Tables[trfDt.TableName].Rows.Count; i++)
                {
                    var drDelete = dsTrfEdit.Tables[trfDt.TableName].Rows[i];
                    if (drDelete.RowState != DataRowState.Deleted)
                    {
                        if (i == e.RowIndex)
                        {
                            drDelete.Delete();
                        }
                    }
                }
            }

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = -1;
            grd_TrfEdit.DataBind();

            Session["dsStoreReqEdit"] = dsTrfEdit;
            e.Cancel = true;
        }

        protected void grd_TrfEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("ddl_gStore") != null)
                {
                    var ddl_gStore = e.Row.FindControl("ddl_gStore") as ASPxComboBox;

                    ddl_gStore.DataSource = dsTrfEdit.Tables[locat.TableName];
                    ddl_gStore.ValueField = "LocationCode";
                    //ddl_gStore.Value        = DataBinder.Eval(e.Row.DataItem, "ToLocationCode");
                    ddl_gStore.DataBind();
                }

                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;
                    var ddl_gStore = e.Row.FindControl("ddl_gStore") as ASPxComboBox;

                    if (ddl_gStore != null && ddl_gStore.Value != null)
                    {
                        ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_gStore.Value.ToString(),
                            LoginInfo.ConnStr);
                        ddl_Product.ValueField = "ProductCode";
                        ddl_Product.Value = DataBinder.Eval(e.Row.DataItem, "ProductCode");
                        ddl_Product.DataBind();
                    }
                }

                if (e.Row.FindControl("txt_QtyRequested") != null)
                {
                    var txt_QtyRequested = e.Row.FindControl("txt_QtyRequested") as TextBox;
                    txt_QtyRequested.Text = DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString();
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                }

                if (e.Row.FindControl("de_gReqDate") != null)
                {
                    var de_gReqDate = e.Row.FindControl("de_gReqDate") as ASPxDateEdit;
                    de_gReqDate.Date = de_ReqDate.Date;
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var txt_Comment = e.Row.FindControl("txt_Comment") as TextBox;
                    txt_Comment.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                }

                if (e.Row.FindControl("lbl_LocationCode") != null)
                {
                    var lbl_LocationCode = e.Row.FindControl("lbl_LocationCode") as Label;
                    lbl_LocationCode.Text = DataBinder.Eval(e.Row.DataItem, "ToLocationCode")
                                            + " : " +
                                            locat.GetName(DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString(),
                                                LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode") +
                                           " : " +
                                           product.GetName(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr) +
                                           " : " +
                                           product.GetName2(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                               LoginInfo.ConnStr);
                }

                if (e.Row.FindControl("lbl_QtyRequested") != null)
                {
                    var lbl_QtyRequested = e.Row.FindControl("lbl_QtyRequested") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString() != string.Empty)
                    {
                        lbl_QtyRequested.Text = String.Format("{0:0.00}",
                            decimal.Parse(DataBinder.Eval(e.Row.DataItem, "RequestQty").ToString()));
                    }
                    else
                    {
                        lbl_QtyRequested.Text = "0.00";
                    }
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;
                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "RequestUnit").ToString();
                }

                if (e.Row.FindControl("lbl_ReqDate") != null)
                {
                    var lbl_ReqDate = e.Row.FindControl("lbl_ReqDate") as Label;
                    lbl_ReqDate.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "DeliveryDate").ToString())
                            .ToString(LoginInfo.BuFmtInfo.FmtSDate);
                }

                //******************************* Stock Summary ******************************************************
                if (dsTrfEdit.Tables[prDt.TableName] != null)
                {
                    dsTrfEdit.Tables[prDt.TableName].Clear();
                }

                var getStock = prDt.GetStockSummary(dsTrfEdit, DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                    ddl_Store.Value.ToString(), txt_DeliDate.Text, LoginInfo.ConnStr);
                    //DataBinder.Eval(e.Row.DataItem, "ToLocationCode").ToString()

                if (getStock)
                {
                    var drStockSummary = dsTrfEdit.Tables[prDt.TableName].Rows[0];

                    if (e.Row.FindControl("lbl_OnHand") != null)
                    {
                        var lbl_OnHand = e.Row.FindControl("lbl_OnHand") as Label;

                        if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                        {
                            lbl_OnHand.Text = String.Format("{0:N}", drStockSummary["OnHand"]);
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
                            lbl_OnOrder.Text = String.Format("{0:N}", drStockSummary["OnOrder"]);
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
                            lbl_Reorder.Text = String.Format("{0:N}", drStockSummary["Reorder"]);
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

                        if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                        {
                            lbl_Restock.Text = String.Format("{0:N}", drStockSummary["Restock"]);
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
                            lbl_LastPrice.Text = String.Format("{0:N}", drStockSummary["LastPrice"]);
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

                //******************************* Disyplay Stock Movement ********************************************
                if (e.Row.FindControl("uc_StockMovement") != null)
                {
                    var uc_StockMovement = e.Row.FindControl("uc_StockMovement") as BlueLedger.PL.PC.StockMovement;
                    uc_StockMovement.HdrNo = lbl_Ref.Text;
                    uc_StockMovement.DtNo = DataBinder.Eval(e.Row.DataItem, "RefId").ToString();
                    uc_StockMovement.ConnStr = LoginInfo.ConnStr;
                    uc_StockMovement.DataBind();
                }

                //****************************** Product Info **********************************************
                if (e.Row.FindControl("lbl_ItemGroup") != null)
                {
                    var lbl_ItemGroup = e.Row.FindControl("lbl_ItemGroup") as Label;
                    lbl_ItemGroup.Text =
                        prodCat.GetName(
                            product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_ItemGroup.ToolTip = lbl_ItemGroup.Text;
                }

                if (e.Row.FindControl("lbl_SubCate") != null)
                {
                    var lbl_SubCate = e.Row.FindControl("lbl_SubCate") as Label;
                    lbl_SubCate.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetProductCategory(DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_SubCate.ToolTip = lbl_SubCate.Text;
                }

                if (e.Row.FindControl("lbl_Category") != null)
                {
                    var lbl_Category = e.Row.FindControl("lbl_Category") as Label;
                    lbl_Category.Text =
                        prodCat.GetName(
                            product.GetParentNoByCategoryCode(
                                product.GetParentNoByCategoryCode(
                                    product.GetProductCategory(
                                        DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString(), LoginInfo.ConnStr),
                                    LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                    lbl_Category.ToolTip = lbl_Category.Text;
                }
            }
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;

            for (var i = grd_TrfEdit.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_TrfEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                chk_Item.Checked = false;
            }
        }

        protected void txt_QtyRequested_TextChanged(object sender, EventArgs e)
        {
            //Check onhand: Cannot request more than onhand.
            var txt_QtyRequested = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("txt_QtyRequested") as TextBox;
            var ddl_Product = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;

            if (ddl_Product.Value == null)
            {
                lbl_Warning.Text = "Please select product.";
                pop_Warning.ShowOnPageLoad = true;
                return;
            }

            var onHand = GetOnHand(ddl_Product.Value.ToString(), ddl_Store.Value.ToString());
            decimal totalReqQty = 0;

            foreach (GridViewRow grv_Row in grd_TrfEdit.Rows)
            {
                var lbl_QtyRequested = grv_Row.FindControl("lbl_QtyRequested") as Label;

                if (dsTrfEdit.Tables[trfDt.TableName].Rows[grv_Row.RowIndex]["ProductCode"].ToString() != null &&
                    lbl_QtyRequested != null)
                {
                    if (dsTrfEdit.Tables[trfDt.TableName].Rows[grv_Row.RowIndex]["ProductCode"].ToString() ==
                        ddl_Product.Value.ToString())
                    {
                        totalReqQty += Convert.ToDecimal(lbl_QtyRequested.Text);
                    }
                }
            }

            if (string.IsNullOrEmpty(txt_QtyRequested.Text))
            {
                txt_QtyRequested.Text = "0";
            }

            if (decimal.Parse(txt_QtyRequested.Text) + totalReqQty > onHand)
            {
                lbl_Warning.Text = "Can not request quantity more than on hand";
                pop_Warning.ShowOnPageLoad = true;

                txt_QtyRequested.Text = String.Format("{0:N}", onHand - totalReqQty);
                SetFocus(txt_QtyRequested);
            }
        }

        private decimal GetOnHand(string productCode, string locationCode)
        {
            var dsOnHand = new DataSet();

            var get = prDt.GetStockSummary(dsOnHand, productCode, locationCode, txt_DeliDate.Text, LoginInfo.ConnStr);

            if (get)
            {
                if (dsOnHand.Tables[prDt.TableName].Rows[0]["OnHand"].ToString() != string.Empty &&
                    dsOnHand.Tables[prDt.TableName].Rows[0]["OnHand"] != null)
                {
                    return decimal.Parse(dsOnHand.Tables[prDt.TableName].Rows[0]["OnHand"].ToString());
                }
            }

            return 0;
        }

        private void UpdateStock(string Product, string Location, GridViewRow grd_Row)
        {
            //******************************* Stock Summary ******************************************************
            if (dsTrfEdit.Tables[prDt.TableName] != null)
            {
                dsTrfEdit.Tables[prDt.TableName].Clear();
            }

            var getStock = prDt.GetStockSummary(dsTrfEdit, Product, Location, txt_DeliDate.Text, LoginInfo.ConnStr);

            if (getStock)
            {
                var drStockSummary = dsTrfEdit.Tables[prDt.TableName].Rows[0];

                if (grd_Row.FindControl("lbl_OnHand") != null)
                {
                    var lbl_OnHand = grd_Row.FindControl("lbl_OnHand") as Label;

                    if (drStockSummary["OnHand"].ToString() != string.Empty && drStockSummary["OnHand"] != null)
                    {
                        lbl_OnHand.Text = String.Format("{0:N}", drStockSummary["OnHand"]);
                    }
                    else
                    {
                        lbl_OnHand.Text = "0.00";
                    }

                    lbl_OnHand.ToolTip = lbl_OnHand.Text;
                }

                if (grd_Row.FindControl("lbl_OnOrder") != null)
                {
                    var lbl_OnOrder = grd_Row.FindControl("lbl_OnOrder") as Label;

                    if (drStockSummary["OnOrder"].ToString() != string.Empty && drStockSummary["OnOrder"] != null)
                    {
                        lbl_OnOrder.Text = String.Format("{0:N}", drStockSummary["OnOrder"]);
                    }
                    else
                    {
                        lbl_OnOrder.Text = "0.00";
                    }

                    lbl_OnOrder.ToolTip = lbl_OnOrder.Text;
                }

                if (grd_Row.FindControl("lbl_Reorder") != null)
                {
                    var lbl_Reorder = grd_Row.FindControl("lbl_Reorder") as Label;

                    if (drStockSummary["Reorder"].ToString() != string.Empty && drStockSummary["Reorder"] != null)
                    {
                        lbl_Reorder.Text = String.Format("{0:N}", drStockSummary["Reorder"]);
                    }
                    else
                    {
                        lbl_Reorder.Text = "0.00";
                    }

                    lbl_Reorder.ToolTip = lbl_Reorder.Text;
                }

                if (grd_Row.FindControl("lbl_Restock") != null)
                {
                    var lbl_Restock = grd_Row.FindControl("lbl_Restock") as Label;

                    if (drStockSummary["Restock"].ToString() != string.Empty && drStockSummary["Restock"] != null)
                    {
                        lbl_Restock.Text = String.Format("{0:N}", drStockSummary["Restock"]);
                    }
                    else
                    {
                        lbl_Restock.Text = "0.00";
                    }

                    lbl_Restock.ToolTip = lbl_Restock.Text;
                }

                if (grd_Row.FindControl("lbl_LastPrice") != null)
                {
                    var lbl_LastPrice = grd_Row.FindControl("lbl_LastPrice") as Label;

                    if (drStockSummary["LastPrice"].ToString() != string.Empty && drStockSummary["LastPrice"] != null)
                    {
                        lbl_LastPrice.Text = String.Format("{0:N}", drStockSummary["LastPrice"]);
                    }
                    else
                    {
                        lbl_LastPrice.Text = "0.00";
                    }

                    lbl_LastPrice.ToolTip = lbl_LastPrice.Text;
                }

                if (grd_Row.FindControl("lbl_LastVendor") != null)
                {
                    var lbl_LastVendor = grd_Row.FindControl("lbl_LastVendor") as Label;
                    lbl_LastVendor.Text = drStockSummary["LastVendor"].ToString();
                    lbl_LastVendor.ToolTip = lbl_LastVendor.Text;
                }
            }

            //****************************** Product Info **********************************************
            if (grd_Row.FindControl("lbl_ItemGroup") != null)
            {
                var lbl_ItemGroup = grd_Row.FindControl("lbl_ItemGroup") as Label;
                lbl_ItemGroup.Text = prodCat.GetName(product.GetProductCategory(Product, LoginInfo.ConnStr),
                    LoginInfo.ConnStr);
                lbl_ItemGroup.ToolTip = lbl_ItemGroup.Text;
            }

            if (grd_Row.FindControl("lbl_SubCate") != null)
            {
                var lbl_SubCate = grd_Row.FindControl("lbl_SubCate") as Label;
                lbl_SubCate.Text =
                    prodCat.GetName(
                        product.GetParentNoByCategoryCode(product.GetProductCategory(Product, LoginInfo.ConnStr),
                            LoginInfo.ConnStr), LoginInfo.ConnStr);
                lbl_SubCate.ToolTip = lbl_SubCate.Text;
            }

            if (grd_Row.FindControl("lbl_Category") != null)
            {
                var lbl_Category = grd_Row.FindControl("lbl_Category") as Label;
                lbl_Category.Text =
                    prodCat.GetName(
                        product.GetParentNoByCategoryCode(
                            product.GetParentNoByCategoryCode(product.GetProductCategory(Product, LoginInfo.ConnStr),
                                LoginInfo.ConnStr), LoginInfo.ConnStr), LoginInfo.ConnStr);
                lbl_Category.ToolTip = lbl_Category.Text;
            }
        }

        protected void btn_WarningPeriod_Click(object sender, EventArgs e)
        {
            pop_WarningPeriod.ShowOnPageLoad = false;
        }

        protected void txt_DeliDate_TextChanged(object sender, EventArgs e)
        {
            // เช็คว่า Period นั้นถูกปิดแล้วหรือยัง
            if (grd_TrfEdit.Rows.Count == 0)
            {
                if (!period.GetIsValidDate(DateTime.Parse(txt_DeliDate.Text), string.Empty, LoginInfo.ConnStr))
                {
                    lbl_WarningPeriod.Text = "Date in close period.";
                    pop_WarningPeriod.ShowOnPageLoad = true;
                    txt_DeliDate.Text = ServerDateTime.ToShortDateString();
                    return;
                }
            }

            // Check Period on Date & Store
            for (var i = 0; i < grd_TrfEdit.Rows.Count; i++)
            {
                var lbl_Location = grd_TrfEdit.Rows[0].FindControl("lbl_LocationCode") as Label;

                if (
                    !period.GetIsValidDate(DateTime.Parse(txt_DeliDate.Text), lbl_Location.Text.Split(':')[0].Trim(),
                        LoginInfo.ConnStr))
                {
                    lbl_WarningPeriod.Text = "Date in close period.";
                    pop_WarningPeriod.ShowOnPageLoad = true;
                    txt_DeliDate.Text = ServerDateTime.ToShortDateString();
                    return;
                }
            }
        }

        protected void grd_TrfEdit_DataBound(object sender, EventArgs e)
        {
            if (grd_TrfEdit.Rows.Count > 0)
            {
                ddl_Store.Enabled = false;
            }
            else
            {
                ddl_Store.Enabled = true;
            }
        }

        #region "Editors"

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                Response.Redirect("TrfDt.aspx?ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"] + "&BuCode=" +
                                  LoginInfo.BuInfo.BuCode);
            }
            else
            {
                Response.Redirect("TrfLst.aspx");
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            var ds = dsTrfEdit.Tables[locat.TableName];
            ds.DefaultView.RowFilter = "EOP = 1";
            ddl_Store.DataSource = ds;
            ddl_Store.ValueField = "LocationCode";
            ddl_Store.DataBind();
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            if (ddl_Store.Value != null)
            {
                Create();
            }
            else
            {
                lbl_Warning.Text = "Please select store before create detail.";
                pop_Warning.Width = Unit.Pixel(250);
                pop_Warning.ShowOnPageLoad = true;
            }
        }

        private void Create()
        {
            btn_Save.Enabled = false;

            var drNew = dsTrfEdit.Tables[trfDt.TableName].NewRow();
            drNew["DeliveryDate"] = de_ReqDate.Date;

            if (dsTrfEdit.Tables[trfDt.TableName].Rows.Count > 0)
            {
                drNew["ToLocationCode"] =
                    dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1][
                        "ToLocationCode"].ToString();
            }

            dsTrfEdit.Tables[trfDt.TableName].Rows.Add(drNew);

            de_ReqDate.Enabled = false;

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1;
            grd_TrfEdit.DataBind();

            TrfEditMode = "New";
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Product = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var lbl_Unit = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("lbl_Unit") as Label;
            lbl_Unit.Text = product.GetInvenUnit(ddl_Product.Value.ToString(), LoginInfo.ConnStr);

            UpdateStock(ddl_Product.Value.ToString(), ddl_Store.Value.ToString(),
                grd_TrfEdit.Rows[grd_TrfEdit.EditIndex]);
        }

        protected void ddl_Product_Load(object sender, EventArgs e)
        {
            var ddl_Product = sender as ASPxComboBox;
            var ddl_gStore = sender as ASPxComboBox;

            if (ddl_Product != null && ddl_gStore != null && ddl_gStore.Value != null)
            {
                ddl_Product.DataSource = product.GetLookUp_LocationCode(ddl_gStore.Value.ToString(), LoginInfo.ConnStr);
                ddl_Product.ValueField = "ProductCode";
                ddl_Product.DataBind();
            }
        }

        protected void ddl_gStore_Load(object sender, EventArgs e)
        {
            var ddl_gStore = sender as ASPxComboBox;

            if (ddl_gStore != null)
            {
                var ds = dsTrfEdit.Tables[locat.TableName];
                ds.DefaultView.RowFilter = "EOP = 1 and LocationCode <> '" + ddl_Store.Value + "'";
                ddl_gStore.DataSource = ds;
                ddl_gStore.ValueField = "LocationCode";
                ddl_gStore.DataBind();
            }
        }

        protected void ddl_gStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl_Product = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_Product") as ASPxComboBox;
            var ddl_gStore = grd_TrfEdit.Rows[grd_TrfEdit.EditIndex].FindControl("ddl_gStore") as ASPxComboBox;

            var StoreName = locat.GetName(ddl_gStore.Value.ToString(), LoginInfo.ConnStr);

            //Check Period
            if (period.GetIsValidDate(DateTime.Parse(txt_DeliDate.Text), ddl_Store.Value.ToString(), LoginInfo.ConnStr))
            {
                ddl_Product.DataSource = product.GetListByTwoLocation(ddl_Store.Value.ToString(),
                    ddl_gStore.Value.ToString(), LoginInfo.ConnStr);
                ddl_Product.ValueField = "ProductCode";
                ddl_Product.DataBind();
            }
            else
            {
                ddl_Store.Value = string.Empty;
                lbl_WarningPeriod.Text = "Store " + StoreName + " is in stock take process.";
                pop_WarningPeriod.ShowOnPageLoad = true;
            }

            //if (ddl_Product != null)
            //{
            //    ddl_Product.DataSource = product.GetListByTwoLocation(ddl_Store.Value.ToString(), ddl_gStore.Value.ToString(), LoginInfo.ConnStr);
            //    ddl_Product.ValueField = "ProductCode";
            //    ddl_Product.DataBind();
            //}
        }

        protected void btn_ReqDate_Ok_Click(object sender, EventArgs e)
        {
            foreach (DataRow drStoreReqEdit in dsTrfEdit.Tables[trfDt.TableName].Rows)
            {
                drStoreReqEdit["DeliveryDate"] = de_ReqDate.Date;
            }

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.DataBind();
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            //Check store requisition detail.It must have data before save.
            if (grd_TrfEdit.Rows.Count == 0)
            {
                lbl_Warning.Text = "Please clicks Create button to select item.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(300);
                return;
            }

            //Check request store location.
            if (ddl_Store.Value == null)
            {
                lbl_Warning.Text = "Please select 'Request To Store'.";
                pop_Warning.ShowOnPageLoad = true;
                pop_Warning.Width = Unit.Pixel(250);
                return;
            }

            //Check Store Location. Store Location in header and detail must not same.
            foreach (DataRow drStoreReqDetail in dsTrfEdit.Tables[trfDt.TableName].Rows)
            {
                if (drStoreReqDetail.RowState != DataRowState.Deleted)
                {
                    if (drStoreReqDetail["ToLocationCode"].ToString() == ddl_Store.Value.ToString())
                    {
                        lbl_Warning.Text = "Source and Destination Store could not select same. Please re-select";
                        pop_Warning.ShowOnPageLoad = true;
                        pop_Warning.Width = Unit.Pixel(440);
                        return;
                    }
                }
            }

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var drInserting = dsTrfEdit.Tables[trf.TableName].NewRow();
                drInserting["RequestCode"] = trf.GetNewRequestCode(LoginInfo.ConnStr);
                drInserting["LocationCode"] = ddl_Store.Value;
                drInserting["Description"] = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
                drInserting["DeliveryDate"] = DateTime.Parse(txt_DeliDate.Text);
                drInserting["Status"] = true;

                if (TrfEditMode.ToUpper() == "COMMIT")
                {
                    drInserting["DocStatus"] = "Committed";
                }
                else
                {
                    drInserting["DocStatus"] = "In Process";
                }

                drInserting["CreateBy"] = LoginInfo.LoginName;
                drInserting["CreateDate"] = ServerDateTime.Date;
                drInserting["UpdateBy"] = LoginInfo.LoginName;
                drInserting["UpdateDate"] = ServerDateTime.Date;

                dsTrfEdit.Tables[trf.TableName].Rows.Add(drInserting);
            }
            else
            {
                var drSave = dsTrfEdit.Tables[trf.TableName].Rows[0];
                drSave["LocationCode"] = ddl_Store.Value;
                drSave["Description"] = txt_Desc.Text == string.Empty ? null : txt_Desc.Text;
                drSave["DeliveryDate"] = DateTime.Parse(txt_DeliDate.Text);
                drSave["Status"] = true;

                if (TrfEditMode.ToUpper() == "COMMIT")
                {
                    drSave["DocStatus"] = "Committed";
                }
                else
                {
                    drSave["DocStatus"] = "In Process";
                }

                drSave["UpdateBy"] = LoginInfo.LoginName;
                drSave["UpdateDate"] = ServerDateTime.Date;
                TrfId = int.Parse(drSave["RefId"].ToString());
            }

            var save = trf.Save(dsTrfEdit, LoginInfo.ConnStr);

            if (save)
            {
                if (Request.Params["MODE"].ToUpper() == "NEW" || Request.Params["MODE"].ToUpper() == "SR")
                {
                    foreach (DataRow drStoreReqDt in dsTrfEdit.Tables[trfDt.TableName].Rows)
                    {
                        drStoreReqDt["DocumentId"] = trf.GetLastID(LoginInfo.ConnStr);
                        TrfId = int.Parse(drStoreReqDt["DocumentId"].ToString());
                    }
                }

                var saveStoreReqDt = trfDt.Save(dsTrfEdit, LoginInfo.ConnStr);
                TrfEditMode = string.Empty;

                if (saveStoreReqDt)
                {
                    Response.Redirect("TrfDt.aspx?ID=" + dsTrfEdit.Tables[trfDt.TableName].Rows[0]["DocumentId"] +
                                      "&VID=" + Request.Params["VID"] + "&BuCode=" + LoginInfo.BuInfo.BuCode);
                }
            }
        }

        protected void btn_Commit_Click(object sender, EventArgs e)
        {
            dsTrfEdit = (DataSet) Session["dsTrfEdit"];

            var drTrf = dsTrfEdit.Tables[trf.TableName].Rows[0];
            drTrf["DocStatus"] = "Committed";
            drTrf["UpdateDate"] =
                DateTime.Parse(txt_DeliDate.Text)
                    .AddHours(ServerDateTime.Hour)
                    .AddMinutes(ServerDateTime.Minute)
                    .AddSeconds(ServerDateTime.Second);
            drTrf["UpdateBy"] = LoginInfo.LoginName;

            foreach (DataRow drTrfDt in dsTrfEdit.Tables[trfDt.TableName].Rows)
            {
                var onHand = GetOnHand(drTrfDt["ProductCode"].ToString(), drTrf["LocationCode"].ToString());

                foreach (GridViewRow grv_Row in grd_TrfEdit.Rows)
                {
                    var lbl_QtyRequested = grv_Row.FindControl("lbl_QtyRequested") as Label;
                    var lbl_ProductCode = grv_Row.FindControl("lbl_ProductCode") as Label;

                    if (drTrfDt["ProductCode"].ToString() == lbl_ProductCode.Text.Split(':')[0].Trim())
                    {
                        if (decimal.Parse(lbl_QtyRequested.Text) > onHand)
                        {
                            lbl_Warning.Text = "Can not commit if OnHand less than Quantity.";
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }
                    }
                }
            }

            var saveTrf = trf.Save(dsTrfEdit, LoginInfo.ConnStr);

            if (saveTrf)
            {
                var startDate = period.GetStartDate(ServerDateTime.Date, LoginInfo.ConnStr);
                var endDate = period.GetEndDate(ServerDateTime.Date, LoginInfo.ConnStr);

                //Update Inventory
                inv.GetStructure(dsTrfEdit, LoginInfo.ConnStr);

                //Update Stock ==>Qty Out
                foreach (DataRow drTrfOutDt in dsTrfEdit.Tables[trfDt.TableName].Rows)
                {
                    var drInv = dsTrfEdit.Tables[inv.TableName].NewRow();
                    drInv["HdrNo"] = dsTrfEdit.Tables[trf.TableName].Rows[0]["RequestCode"].ToString();
                    drInv["DtNo"] = drTrfOutDt["RefId"].ToString();
                    drInv["InvNo"] = 1;
                    drInv["ProductCode"] = drTrfOutDt["ProductCode"].ToString();
                    drInv["Location"] = dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString();
                    drInv["IN"] = 0;
                    drInv["OUT"] = drTrfOutDt["RequestQty"];
                    drInv["Amount"] = inv.GetPAvgAudit(startDate, endDate,
                        dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString()
                        , drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["PAvgAudit"] = inv.GetPAvgAudit(startDate, endDate,
                        dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString()
                        , drTrfOutDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["CommittedDate"] = DateTime.Parse(txt_DeliDate.Text);
                    drInv["Type"] = "TF";

                    dsTrfEdit.Tables[inv.TableName].Rows.Add(drInv);
                }

                //Update Stock ==>Qty In
                foreach (DataRow drTrfInDt in dsTrfEdit.Tables[trfDt.TableName].Rows)
                {
                    var drInv = dsTrfEdit.Tables[inv.TableName].NewRow();
                    drInv["HdrNo"] = dsTrfEdit.Tables[trf.TableName].Rows[0]["RequestCode"].ToString();
                    drInv["DtNo"] = drTrfInDt["RefId"].ToString();
                    drInv["InvNo"] = 2;
                    drInv["ProductCode"] = drTrfInDt["ProductCode"].ToString();
                    drInv["Location"] = drTrfInDt["ToLocationCode"].ToString();
                    drInv["IN"] = drTrfInDt["RequestQty"];
                    drInv["OUT"] = 0;
                    drInv["Amount"] = inv.GetPAvgAudit(startDate, endDate,
                        dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString()
                        , drTrfInDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["PAvgAudit"] = inv.GetPAvgAudit(startDate, endDate,
                        dsTrfEdit.Tables[trf.TableName].Rows[0]["LocationCode"].ToString()
                        , drTrfInDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drInv["CommittedDate"] = DateTime.Parse(txt_DeliDate.Text);
                    drInv["Type"] = "TF";

                    dsTrfEdit.Tables[inv.TableName].Rows.Add(drInv);
                }

                var save = inv.Save(dsTrfEdit, LoginInfo.ConnStr);

                if (save)
                {
                    Response.Redirect("TrfDt.aspx?&BuCode=" + LoginInfo.BuInfo.BuCode + "&ID=" + drTrf["RefId"] +
                                      "&VID=" + Request.Params["VID"]);
                }
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = true;
        }

        protected void btn_ComfiremDelete_Click(object sender, EventArgs e)
        {
            for (var i = grd_TrfEdit.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_TrfEdit.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var drStoreReqDt = dsTrfEdit.Tables[trfDt.TableName].Rows[i];

                    if (drStoreReqDt.RowState != DataRowState.Deleted)
                    {
                        drStoreReqDt.Delete();
                    }
                }
            }

            grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            grd_TrfEdit.EditIndex = -1;
            grd_TrfEdit.DataBind();

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        #endregion
    }
}