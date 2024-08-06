using System;
using System.Drawing;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PT.RCP
{

    public partial class RecipeEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        private readonly Blue.BL.PT.RCP.Rcp rcp = new Blue.BL.PT.RCP.Rcp();
        private readonly Blue.BL.PT.RCP.RcpDt rcpDt = new Blue.BL.PT.RCP.RcpDt();

        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private readonly Blue.BL.Option.Inventory.StoreLct stoLocate = new Blue.BL.Option.Inventory.StoreLct();

        private DataSet dsRecipe = new DataSet();
        private decimal vatRate = 0;
        private decimal svcRate = 0;
        //private Byte[] imgBytes;

        private string RcpEditMode
        {
            get { return Session["RcpEditMode"].ToString(); }
            set { Session["RcpEditMode"] = value; }
        }
        private string lastProductCode
        {
            get
            {
                if (Session["lastProductCode"] == null)
                    return string.Empty;
                else
                    return Session["lastProductCode"].ToString();
            }
            set { Session["lastProductCode"] = value; }
        }


        private string rcpID = string.Empty;

        #endregion

        // -----------------------------------------------------------------------------------------------------------

        protected void Page_Init(object sender, EventArgs e)
        {
            //hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
            hf_ConnStr.Value = LoginInfo.ConnStr;

            Decimal.TryParse(config.GetValue("IN", "IN", "TaxRate", LoginInfo.ConnStr), out vatRate);
            Decimal.TryParse(config.GetValue("IN", "IN", "TaxRate", LoginInfo.ConnStr), out svcRate);


            hf_DefaultAmtDigit.Value = DefaultAmtDigit.ToString();
            hf_DefaultSvcRate.Value = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);
            hf_DefaultTaxRate.Value = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);

            hf_DefaultSvcRate.Value = string.IsNullOrEmpty(hf_DefaultSvcRate.Value) ? "0" : hf_DefaultSvcRate.Value;
            hf_DefaultTaxRate.Value = string.IsNullOrEmpty(hf_DefaultTaxRate.Value) ? "0" : hf_DefaultTaxRate.Value;

            lbl_TaxSvcRate.Text = string.Format("Service charge: {0}% , Tax Rate: {1}%", hf_DefaultSvcRate.Value.ToString(), hf_DefaultTaxRate.Value.ToString());
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            // Check login
            //base.Page_Load(sender, e);
            //fileUploadImg.Attributes.Add("onchange", "FileUploadControl_Change(this)");
            if (!IsPostBack)
            {
                Page_Retrieve();

                Session.Remove("lastProductCode");
                Session.Remove("RcpEditMode");
                Session["ImgBytes"] = null;
            }
            else
            {
                dsRecipe = (DataSet)Session["dsRecipe"];

                //UpdatePortionCost();
            }
        }

        private void Page_Retrieve()
        {
            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                var msgError = string.Empty;

                // Get invoice no from HTTP query string
                var rcpNo = Request.QueryString["ID"];

                rcp.GetList(dsRecipe, rcpNo, hf_ConnStr.Value);
                rcpDt.GetList(dsRecipe, rcpNo, hf_ConnStr.Value);
            }
            else // NEW (Insert)
            {
                // Get Schema.
                rcp.GetStructure(dsRecipe, hf_ConnStr.Value);
                rcpDt.GetStructure(dsRecipe, hf_ConnStr.Value);
            }

            Session["dsRecipe"] = dsRecipe;

            Page_Setting();

        }

        private void Page_Setting()
        {


            var MODE = Request.QueryString["MODE"];

            if (MODE.ToUpper() == "EDIT")
            {
                // Set header information.
                var drRecipe = dsRecipe.Tables[rcp.TableName].Rows[0];

                txt_RcpCode.Text = drRecipe["RcpCode"].ToString();
                txt_RcpCode.Enabled = false;
                txt_RcpDesc1.Text = drRecipe["RcpDesc1"].ToString();
                txt_RcpDesc2.Text = drRecipe["RcpDesc2"].ToString();
                ddl_RcpCateCode.Value = drRecipe["RcpCateCode"].ToString();
                ddl_RcpUnit.Value = drRecipe["RcpUnitCode"].ToString();
                txt_Preparation.Text = drRecipe["Preparation"].ToString();
                txt_PrepTime.Text = drRecipe["PrepTime"].ToString();
                txt_TotalTime.Text = drRecipe["TotalTime"].ToString();
                txt_PortionSize.Text = BuNumericFormat(drRecipe["PortionSize"]);
                comb_locate.Value = drRecipe["LocationCode"].ToString();

                lblCostOfPortion.Text = drRecipe["PortionCost"].ToString();
                if (drRecipe["RcpCost"] != DBNull.Value) { txt_TotalCost.Text = drRecipe["RcpCost"].ToString(); }
                if (drRecipe["MixRatio"] != DBNull.Value) { txt_TotalMix.Text = drRecipe["MixRatio"].ToString(); }
                if (drRecipe["MixCost"] != DBNull.Value) { txt_CostTotalMix.Text = drRecipe["MixCost"].ToString(); }
                if (drRecipe["NetPrice"] != DBNull.Value) { txt_NetPrice.Text = drRecipe["NetPrice"].ToString(); }
                if (drRecipe["GrossPrice"] != DBNull.Value) { txt_GrossPrice.Text = drRecipe["GrossPrice"].ToString(); }
                if (drRecipe["NetCost"] != DBNull.Value) { txt_NetCost.Text = drRecipe["NetCost"].ToString(); }
                if (drRecipe["GrossCost"] != DBNull.Value) { txt_GrossCost.Text = drRecipe["GrossCost"].ToString(); }
                txt_RcpRemark.Text = drRecipe["Remark"].ToString();
                // Add on 2016-11-17
                if (drRecipe["RcpImage"] != DBNull.Value && drRecipe["RcpImage"] != string.Empty)
                    img01.ImageUrl = (string)drRecipe["RcpImage"];
                else
                    img01.Visible = false;
            }
            else if (MODE.ToUpper() == "NEW")
            {
                img01.Visible = false;
                lblCostOfPortion.Text = string.Format("{0:N}", 0);
            }

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.DataBind();

            RcpEditMode = string.Empty;
            //calculateCost();
        }

        #region "Actions"

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "UPDATE":
                    var dbParams = new Blue.DAL.DbParameter[2];
                    dbParams[0] = new Blue.DAL.DbParameter("@RcpCode", txt_RcpCode.Text);
                    dbParams[1] = new Blue.DAL.DbParameter("@updatedBy", LoginInfo.LoginName);

                    bool result = rcpDt.UpdateCostOfRecipe(dbParams, LoginInfo.ConnStr);
                    if (result)
                    {
                        Response.Redirect("RecipeEdit.aspx?BuCode=" + Request.Params["BuCode"] +
                                         "&MODE=EDIT&ID=" + Request.Params["ID"] +
                                         "&VID=" + Request.Params["VID"]);
                    }

                    break;

                case "SAVE":
                    string message = checkHeaderRequired();
                    if (message == string.Empty)
                    {
                        pop_ConfirmSave.ShowOnPageLoad = true;
                    }
                    else
                    {
                        lbl_Warning.Text = message;
                        pop_Warning.ShowOnPageLoad = true;
                    }
                    break;

                case "BACK":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("RecipeDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                    }
                    else
                    {
                        Response.Redirect("RecipeList.aspx");
                    }
                    break;
            }
        }

        protected void menu_CmdGrd_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateDetail();
                    break;

                case "DELETE":
                    DeleteDetail();
                    break;
            }
        }

        // ---------------------------------------------------------------------------------------

        private int GetRcpDtID(DataTable dt)
        {
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["RcpDtID"].ToString()) + 1;
            else
                return 1;
        }

        private void CreateDetail()
        {
            var drNew = dsRecipe.Tables[rcpDt.TableName].NewRow();

            drNew["RcpCode"] = ""; // dsRecipe.Tables[recipeHeader].Rows[0]["RcpCode"];
            drNew["RcpDtID"] = GetRcpDtID(dsRecipe.Tables[rcpDt.TableName]);
            drNew["IngredientCode"] = string.Empty;
            drNew["IngredientType"] = string.Empty;
            //drNew["Qty"] = 0;
            drNew["Unit"] = string.Empty;
            drNew["UnitRate"] = 0;
            drNew["BaseUnit"] = string.Empty;
            drNew["BaseCost"] = 0;
            drNew["NetCost"] = 0;
            drNew["SpoilRate"] = 0;
            drNew["SpoilCost"] = 0;
            drNew["TotalCost"] = 0;

            dsRecipe.Tables[rcpDt.TableName].Rows.Add(drNew);

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = dsRecipe.Tables[rcpDt.TableName].Rows.Count - 1;
            grd_RecipeDt.DataBind();

            RcpEditMode = "NEW";
            //Session["lastProductCode"] = string.Empty;
            lastProductCode = string.Empty;


        }

        private void DeleteDetail()
        {
            for (var i = grd_RecipeDt.Rows.Count - 1; i >= 0; i--)
            {
                var chk_Item = grd_RecipeDt.Rows[i].Cells[0].FindControl("chk_Item") as CheckBox;
                if (chk_Item.Checked)
                {
                    var drRcpDt = dsRecipe.Tables[rcpDt.TableName].Rows[i];

                    if (drRcpDt.RowState != DataRowState.Deleted)
                        drRcpDt.Delete();
                }
            }

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = -1;
            grd_RecipeDt.DataBind();

            CalculateSummary();

        }

        private string checkHeaderRequired()
        {
            string message = string.Empty;

            bool isEditMode = Request.Params["MODE"].ToString().ToUpper() == "EDIT";

            if (txt_RcpCode.Text == string.Empty)
            {
                message = "Recipe Code is required.";
            }
            else if (!isEditMode && rcp.IsExistCode(txt_RcpCode.Text, LoginInfo.ConnStr))
            {
                message = "Recipe code is not available. It has already exists in recpipe.";
            }
            else if (ddl_RcpCateCode.Value == null)
            {
                message = "Category is required.";
            }
            else if (ddl_RcpUnit.Value == null)
            {
                message = "Unit is required.";
            }
            else
            {
                message = string.Empty;
            }

            return message;
        }

        //private decimal UpdatePortionCost()
        //{
        //    decimal total = 0;

        //    foreach (DataRow row in dsRecipe.Tables[rcpDt.TableName].Rows)
        //    {
        //        if (row.RowState != DataRowState.Deleted)
        //            total += Convert.ToDecimal(row["TotalCost"]);
        //    }
        //    return total;
        //}

        private decimal GetTotalCost()
        {
            decimal total = 0;

            foreach (DataRow row in dsRecipe.Tables[rcpDt.TableName].Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    total += Convert.ToDecimal(row["TotalCost"]);
            }
            return total;
        }


        private int SaveRcp()
        {
            Page.Validate();
            if (Page.IsValid)
            {
                //UpdatePortionCost();
                //UpdateRcpPrice();

                txt_TotalCost.Text = string.IsNullOrEmpty(txt_TotalCost.Text) ? "0" : txt_TotalCost.Text;
                txt_TotalMix.Text = string.IsNullOrEmpty(txt_TotalMix.Text) ? "0" : txt_TotalMix.Text;
                txt_CostTotalMix.Text = string.IsNullOrEmpty(txt_CostTotalMix.Text) ? "0" : txt_CostTotalMix.Text;
                txt_NetPrice.Text = string.IsNullOrEmpty(txt_NetPrice.Text) ? "0" : txt_NetPrice.Text;
                txt_GrossPrice.Text = string.IsNullOrEmpty(txt_GrossPrice.Text) ? "0" : txt_GrossPrice.Text;
                txt_NetCost.Text = string.IsNullOrEmpty(txt_NetCost.Text) ? "0" : txt_NetCost.Text;
                txt_GrossCost.Text = string.IsNullOrEmpty(txt_GrossCost.Text) ? "0" : txt_GrossCost.Text;


                var MODE = Request.Params["MODE"];
                if (MODE.ToUpper() == "EDIT")
                {
                    var drRecipe = dsRecipe.Tables[rcp.TableName].Rows[0];

                    drRecipe["RcpDesc1"] = txt_RcpDesc1.Text;
                    drRecipe["RcpDesc2"] = txt_RcpDesc2.Text;
                    drRecipe["RcpCateCode"] = ddl_RcpCateCode.SelectedItem.Value.ToString();
                    drRecipe["RcpUnitCode"] = ddl_RcpUnit.SelectedItem.Value.ToString();
                    drRecipe["Preparation"] = txt_Preparation.Text;
                    drRecipe["PrepTime"] = txt_PrepTime.Text;
                    drRecipe["TotalTime"] = txt_TotalTime.Text;
                    drRecipe["PortionSize"] = txt_PortionSize.Number;
                    drRecipe["PortionCost"] = Convert.ToDecimal(lblCostOfPortion.Text);

                    drRecipe["RcpCost"] = Convert.ToDecimal(txt_TotalCost.Text);
                    drRecipe["MixRatio"] = Convert.ToDecimal(txt_TotalMix.Text);
                    drRecipe["MixCost"] = Convert.ToDecimal(txt_CostTotalMix.Text);
                    drRecipe["NetPrice"] = Convert.ToDecimal(txt_NetPrice.Text);
                    drRecipe["GrossPrice"] = Convert.ToDecimal(txt_GrossPrice.Text);
                    drRecipe["NetCost"] = Convert.ToDecimal(txt_NetCost.Text);
                    drRecipe["GrossCost"] = Convert.ToDecimal(txt_GrossCost.Text);
                    drRecipe["Remark"] = txt_RcpRemark.Text;

                    if (comb_locate.SelectedIndex > 0)
                        drRecipe["LocationCode"] = comb_locate.SelectedItem.Value;
                    else
                        drRecipe["LocationCode"] = string.Empty;

                    if (img01.ImageUrl.ToString() != string.Empty)
                        drRecipe["RcpImage"] = img01.ImageUrl.ToString();

                    drRecipe["Attachment"] = DBNull.Value;
                    drRecipe["IsActived"] = ddl_IsActive.SelectedIndex;
                    drRecipe["IsVoid"] = 0;
                    drRecipe["UpdatedDate"] = ServerDateTime;
                    drRecipe["UpdatedBy"] = LoginInfo.LoginName;
                }
                else // "NEW"
                {
                    rcp.GetStructure(dsRecipe, hf_ConnStr.Value);

                    var drRecipe = dsRecipe.Tables[rcp.TableName].NewRow();

                    // Modified on: 19/09/2017, By: Fon
                    drRecipe["RcpCode"] = (txt_RcpCode.Text != string.Empty)
                        ? txt_RcpCode.Text.ToUpper()
                        : rcpDt.GetNewRcpCode(LoginInfo.ConnStr);
                    // End Modified.



                    drRecipe["RcpDesc1"] = txt_RcpDesc1.Text;
                    drRecipe["RcpDesc2"] = txt_RcpDesc2.Text;
                    drRecipe["RcpCateCode"] = ddl_RcpCateCode.SelectedItem.Value.ToString();
                    drRecipe["RcpUnitCode"] = ddl_RcpUnit.SelectedItem.Value.ToString();
                    drRecipe["Preparation"] = txt_Preparation.Text;
                    drRecipe["PrepTime"] = txt_PrepTime.Text;
                    drRecipe["TotalTime"] = txt_TotalTime.Text;
                    drRecipe["PortionSize"] = txt_PortionSize.Number;
                    drRecipe["PortionCost"] = Convert.ToDecimal(lblCostOfPortion.Text);

                    drRecipe["RcpCost"] = Convert.ToDecimal(txt_TotalCost.Text);
                    drRecipe["MixRatio"] = Convert.ToDecimal(txt_TotalMix.Text);
                    drRecipe["MixCost"] = Convert.ToDecimal(txt_CostTotalMix.Text);
                    drRecipe["NetPrice"] = Convert.ToDecimal(txt_NetPrice.Text);
                    drRecipe["GrossPrice"] = Convert.ToDecimal(txt_GrossPrice.Text);
                    drRecipe["NetCost"] = Convert.ToDecimal(txt_NetCost.Text);
                    drRecipe["GrossCost"] = Convert.ToDecimal(txt_GrossCost.Text);
                    drRecipe["Remark"] = txt_RcpRemark.Text;
                    if (comb_locate.SelectedIndex > 0)
                        drRecipe["LocationCode"] = comb_locate.SelectedItem.Value;
                    else
                        drRecipe["LocationCode"] = string.Empty;

                    if (img01.ImageUrl.ToString() != string.Empty)
                        drRecipe["RcpImage"] = img01.ImageUrl.ToString();

                    drRecipe["Attachment"] = DBNull.Value;
                    drRecipe["IsActived"] = ddl_IsActive.SelectedIndex;
                    drRecipe["IsVoid"] = 0;
                    drRecipe["CreatedDate"] = ServerDateTime;
                    drRecipe["CreatedBy"] = LoginInfo.LoginName;
                    drRecipe["UpdatedDate"] = ServerDateTime;
                    drRecipe["UpdatedBy"] = LoginInfo.LoginName;

                    dsRecipe.Tables[rcp.TableName].Rows.Add(drRecipe);
                }

                var dt = dsRecipe.Tables[rcpDt.TableName];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].RowState != DataRowState.Deleted && dt.Rows[i]["RcpCode"] == string.Empty)
                    {
                        dsRecipe.Tables[rcpDt.TableName].Rows[i]["RcpCode"] = txt_RcpCode.Text;
                    }
                }

                // var result = cn.SaveToCnCnDtAndInv(dsRecipe, hf_ConnStr.Value);
                var result = rcp.Save(dsRecipe, hf_ConnStr.Value);

                if (result)
                {
                    var rcpCode = string.Empty;

                    if (MODE == "EDIT")
                        rcpCode = txt_RcpCode.Text.ToUpper();
                    else
                        rcpCode = dsRecipe.Tables[rcp.TableName].Rows[0]["RcpCode"].ToString();
                    Response.Redirect("RecipeDt.aspx?ID=" + rcpCode + "&BuCode=" + Request.Params["BuCode"]);
                }

            }

            return 0;
        }

        private void SaveRcpDt()
        {
            var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];

            var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
            var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;

            var lbl_ItemType = grd.FindControl("lbl_ItemType") as Label;
            var lbl_NetCost = grd.FindControl("lbl_NetCost") as Label;
            var lbl_WastageCost = grd.FindControl("lbl_WastageCost") as Label;
            var lbl_TotalCost = grd.FindControl("lbl_TotalCost") as Label;
            var lbl_BaseUnit_Nm = grd.FindControl("lbl_BaseUnit_Nm") as Label;
            var lbl_UnitRate_Nm = grd.FindControl("lbl_UnitRate_Nm") as Label;
            var lbl_UnitCost_Nm = grd.FindControl("lbl_BaseCost_Nm") as Label;

            var txt_Qty = grd.FindControl("txt_Qty") as ASPxSpinEdit;
            var txt_BaseCost = grd.FindControl("txt_BaseCost") as ASPxSpinEdit;
            var txt_WastageRate = grd.FindControl("txt_WastageRate") as ASPxSpinEdit;

            // Required field(s)
            if (ddl_ItemCode != null)
            {
                if (ddl_ItemCode.Value == null)
                {
                    lbl_Warning.Text = "Item is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }


            if (txt_Qty != null)
            {
                if (txt_Qty.Number <= 0)
                {
                    lbl_Warning.Text = "Qantity is invalid";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            if (ddl_Unit != null)
            {
                if (ddl_Unit.Text == string.Empty)
                {
                    lbl_Warning.Text = "Unit is required.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var MODE = Request.Params["MODE"];

            if (ddl_ItemCode != null && MODE.ToUpper() == "EDIT")
            {
                string ingredientCode = ddl_ItemCode.Value.ToString();
                if (rcpDt.IsRecursiveRecipe(txt_RcpCode.Text, ref ingredientCode, hf_ConnStr.Value))
                {
                    lbl_Warning.Text = "Item is invalid. It is recursive recipe.";
                    pop_Warning.ShowOnPageLoad = true;
                    return;
                }
            }

            var drRecipeDt = dsRecipe.Tables[rcpDt.TableName].Rows[grd.DataItemIndex];

            string itemType = lbl_ItemType.Text[0].ToString().ToUpper();
            string itemCode = ddl_ItemCode.Value.ToString();
            CalculationRcpDt(itemCode);

            drRecipeDt["RcpCode"] = txt_RcpCode.Text.ToUpper();
            drRecipeDt["IngredientCode"] = itemCode;
            drRecipeDt["IngredientType"] = itemType;

            decimal qty = txt_Qty.Number;
            decimal baseCost = txt_BaseCost.Number;
            decimal netCost = Convert.ToDecimal(lbl_NetCost.Text);
            decimal wastageCost = Convert.ToDecimal(lbl_WastageCost.Text);
            decimal totalCost = Convert.ToDecimal(lbl_TotalCost.Text);

            drRecipeDt["Qty"] = qty;
            drRecipeDt["Unit"] = ddl_Unit.Value;
            drRecipeDt["UnitRate"] = Convert.ToDecimal(lbl_UnitRate_Nm.Text);
            drRecipeDt["BaseUnit"] = lbl_BaseUnit_Nm.Text;
            drRecipeDt["BaseCost"] = baseCost;
            drRecipeDt["NetCost"] = netCost;
            drRecipeDt["SpoilRate"] = txt_WastageRate.Number;
            drRecipeDt["SpoilCost"] = wastageCost;
            drRecipeDt["TotalCost"] = totalCost;
            drRecipeDt["UpdatedDate"] = ServerDateTime;

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = -1;
            grd_RecipeDt.DataBind();

            Session["dsRecipe"] = dsRecipe;

            //UpdatePortionCost();
            //txt_TotalCost.Text = string.Format(DefaultAmtFmt, GetTotalCost());
            CalculateSummary();

        }
        // ---------------------------------------------------------------------------------------

        #endregion

        #region "Header"
        // Header
        protected void ddl_RcpCateCode_Load(object sender, EventArgs e)
        {
            var comboBox = (ASPxComboBox)sender;
            comboBox.DataSource = rcp.GetListCategory(hf_ConnStr.Value); //recipe.GetCategoryList(LoginInfo.ConnStr);
            comboBox.ValueField = "RcpCateCode";
            comboBox.DataBind();
        }

        protected void ddl_RcpUnit_Load(object sender, EventArgs e)
        {
            var comboBox = (ASPxComboBox)sender;
            comboBox.DataSource = rcp.GetListUnit(LoginInfo.ConnStr);
            comboBox.ValueField = "RcpUnitCode";
            comboBox.DataBind();
        }

        protected void txt_PortionSize_ValueChanged(object sender, EventArgs e)
        {
        }

        protected void txt_ProfitRate_ValueChanged(object sender, EventArgs e)
        {
            //UpdateRcpPrice();
        }

        // Added on: 25/09/2017, By: Fon
        protected void comb_locate_Init(object sender, EventArgs e)
        {
            ASPxComboBox comb = (ASPxComboBox)sender;
            comb.DataSource = stoLocate.GetList(1, LoginInfo.LoginName, LoginInfo.ConnStr);
            comb.ValueField = "LocationCode";
            comb.TextField = "LocationName";
            comb.DataBind();
            comb.Items.Insert(0, new ListEditItem());
        }
        // End Added;

        #endregion

        #region "Detail"
        // Detail
        protected void ddl_IngredientCode_Load(object sender, EventArgs e)
        {
            var ddl_ItemCode = sender as ASPxComboBox;
            string rcpCode = txt_RcpCode.Text;
            ddl_ItemCode.DataSource = rcpDt.GetListItem(rcpCode, LoginInfo.ConnStr);
            ddl_ItemCode.ValueField = "IngredientCode";
            ddl_ItemCode.DataBind();
        }

        protected void ddl_IngredientCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string productCode = (grd_RecipeDt.Rows[grd_RecipeDt.EditIndex].FindControl("ddl_ItemCode") as ASPxComboBox).ClientValue.ToString();
            //if (productCode != lastProductCode)
            //{
            //    ItemCalculation();
            //}
            ////Session["lastProductCode"] = productCode;
            //lastProductCode = productCode;


            var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];

            var lbl_ItemType = grd.FindControl("lbl_ItemType") as Label;

            //var lbl_NetCost = grd.FindControl("lbl_NetCost") as Label;
            //var lbl_WastageCost = grd.FindControl("lbl_WastageCost") as Label;
            //var lbl_TotalCost = grd.FindControl("lbl_TotalCost") as Label;

            var txt_Qty = grd.FindControl("txt_Qty") as ASPxSpinEdit;
            var txt_BaseCost = grd.FindControl("txt_BaseCost") as ASPxSpinEdit;
            //var txt_WastageRate = grd.FindControl("txt_WastageRate") as ASPxSpinEdit;

            var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
            var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;

            var lbl_BaseUnit_Nm = grd.FindControl("lbl_BaseUnit_Nm") as Label;
            var lbl_UnitRate_Nm = grd.FindControl("lbl_UnitRate_Nm") as Label;
            var lbl_BaseCost_Nm = grd.FindControl("lbl_BaseCost_Nm") as Label;
            //var lbl_CostUpdated = grd.FindControl("lbl_CostUpdated") as Label;


            string itemType = lbl_ItemType.Text[0].ToString();
            string itemCode = ddl_ItemCode.ClientValue.ToString();

            if (itemCode != lastProductCode)
            {
                ddl_Unit.DataSource = rcpDt.GetListUnit(itemCode, hf_ConnStr.Value);
                ddl_Unit.DataBind();
                ddl_Unit.Text = rcpDt.GetUnitDefault(itemCode, hf_ConnStr.Value);
                lbl_BaseUnit_Nm.Text = rcpDt.GetBaseUnit(itemCode, hf_ConnStr.Value);

                decimal unitRate = rcpDt.GetUnitConvRate(itemCode, ddl_Unit.Text, hf_ConnStr.Value);
                decimal baseCost = rcpDt.GetLastCost(itemType, itemCode, DateTime.Now, LoginInfo.ConnStr);
                //decimal qty = txt_Qty.Number;
                //decimal netCost = RoundAmt(qty * baseCost / unitRate);
                //decimal wastageRate = txt_WastageRate.Number;
                //decimal wastageCost = netCost + RoundAmt(netCost * wastageRate / 100);
                //decimal totalCost = netCost + wastageCost;

                lbl_UnitRate_Nm.Text = String.Format("{0:N3}", unitRate);
                //txt_BaseCost.Text = string.Format(DefaultAmtFmt, baseCost);
                //lbl_NetCost.Text = string.Format(DefaultAmtFmt, netCost);
                //lbl_WastageCost.Text = string.Format(DefaultAmtFmt, wastageCost);
                //lbl_TotalCost.Text = string.Format(DefaultAmtFmt, totalCost);

                //Session["lastProductCode"] = ddl_ItemCode.ClientValue;
                lastProductCode = itemCode;
            }

            //if (ddl_ItemCode.SelectedItem.GetValue("IngredientType").ToString().ToUpper() == "P")
            //    ddl_IngredientType.SelectedIndex = 0;
            //else
            //    ddl_IngredientType.SelectedIndex = 1;

            if (ddl_ItemCode.SelectedItem != null)
            {
                // ItemType
                lbl_ItemType.Text = ddl_ItemCode.SelectedItem.GetValue("IngredientType").ToString().ToUpper() == "P" ? "Product" : "Recipe";
                txt_Qty.Focus();
            }
        }

        protected void ddl_IngredientUnit_Load(object sender, EventArgs e)
        {
            var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];
            var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
            var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;


            ddl_Unit.DataSource = rcpDt.GetListUnit(ddl_ItemCode.ClientValue, hf_ConnStr.Value);
            ddl_Unit.DataBind();

        }

        protected void ddl_IngredientUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];

            var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
            var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;
            var lbl_UnitRate_Nm = grd.FindControl("lbl_UnitRate_Nm") as Label;

            if (ddl_Unit.Value != null)
            {
                lbl_UnitRate_Nm.Text = string.Format("{0:N}", rcpDt.GetUnitConvRate(ddl_ItemCode.ClientValue, ddl_Unit.ClientValue, hf_ConnStr.Value));
            }
        }


        protected void btn_Saved_Click(object sender, EventArgs e)
        {
            pop_Saved.ShowOnPageLoad = false;
            pop_ConfirmSave.ShowOnPageLoad = true;
        }


        protected void btn_ConfirmSave_Click(object sender, EventArgs e)
        {
            int result = SaveRcp();
            pop_ConfirmSave.ShowOnPageLoad = false;

        }

        protected void btn_CancelSave_Click(object sender, EventArgs e)
        {
            pop_ConfirmSave.ShowOnPageLoad = false;
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            DeleteDetail();
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        protected void btn_Error_Click(object sender, EventArgs e)
        {
            pop_Error.ShowOnPageLoad = false;
        }


        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }


        #endregion


        protected void grd_RecipeDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //// Hide buttons when editting
            //if (grd_RecipeDt.EditIndex != -1)
            //{
            //    menu_CmdBar.Visible = false; //SaveRecipe, Commit, Back
            //    menu_CmdGrd.Visible = false; //Create, Delete
            //}
            //else
            //{
            //    menu_CmdBar.Visible = true; //SaveRecipe, Commit, Back
            //    menu_CmdGrd.Visible = true; //Create, Delete
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Type
                if (e.Row.FindControl("lbl_ItemType") != null)
                {
                    var label = e.Row.FindControl("lbl_ItemType") as Label;
                    if (DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString() == "P")
                        label.Text = "Product";
                    else
                        label.Text = "Recipe";
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("ddl_IngredientType") != null)
                {
                    var ddl = e.Row.FindControl("ddl_IngredientType") as DropDownList;
                    if (DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString() == "P")
                        ddl.SelectedIndex = 0;
                    else if (DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString() == "R")
                        ddl.SelectedIndex = 1;
                }

                // Item
                if (e.Row.FindControl("lbl_ItemCode") != null)
                {
                    var label = e.Row.FindControl("lbl_ItemCode") as Label;
                    label.Text = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString() + " : " + rcpDt.GetItemDesc1(DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString(), hf_ConnStr.Value);
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("ddl_ItemCode") != null)
                {
                    var ddl = e.Row.FindControl("ddl_ItemCode") as ASPxComboBox;
                    ddl.Value = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString();
                    ddl.ToolTip = ddl.Text;
                }

                // Base Cost
                if (e.Row.FindControl("lbl_BaseCost") != null)
                {
                    var label = e.Row.FindControl("lbl_BaseCost") as Label;
                    label.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "BaseCost"));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("txt_BaseCost") != null)
                {
                    var txt = e.Row.FindControl("txt_BaseCost") as ASPxSpinEdit;
                    txt.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "BaseCost"));
                    txt.ToolTip = txt.Text;
                }


                // Qty
                if (e.Row.FindControl("lbl_Qty") != null)
                {
                    var lable = e.Row.FindControl("lbl_Qty") as Label;
                    lable.Text = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));
                    lable.ToolTip = lable.Text;
                }

                if (e.Row.FindControl("txt_Qty") != null)
                {
                    var txt = e.Row.FindControl("txt_Qty") as ASPxSpinEdit;
                    txt.Value = String.Format(DefaultQtyFmt, DataBinder.Eval(e.Row.DataItem, "Qty"));
                    txt.ToolTip = txt.Text;
                }

                // Unit
                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lable = e.Row.FindControl("lbl_Unit") as Label;
                    lable.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lable.ToolTip = lable.Text;
                }

                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Unit") as ASPxComboBox;

                    string IngredientCode = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString();
                    ddl.DataSource = rcpDt.GetListUnit(IngredientCode, hf_ConnStr.Value);
                    ddl.DataBind();

                    ddl.Value = DataBinder.Eval(e.Row.DataItem, "Unit") == DBNull.Value
                        ? null
                        : DataBinder.Eval(e.Row.DataItem, "Unit");
                }

                // Net Cost
                if (e.Row.FindControl("lbl_NetCost") != null)
                {
                    var label = e.Row.FindControl("lbl_NetCost") as Label;
                    label.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "NetCost"));
                    label.ToolTip = label.Text;
                }


                // SpoilRate
                if (e.Row.FindControl("lbl_WastageRate") != null)
                {
                    var label = e.Row.FindControl("lbl_WastageRate") as Label;
                    label.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "SpoilRate"));
                    label.ToolTip = label.Text;
                }

                if (e.Row.FindControl("txt_WastageRate") != null)
                {
                    var txt = e.Row.FindControl("txt_WastageRate") as ASPxSpinEdit;
                    txt.Value = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "SpoilRate"));
                    txt.ToolTip = txt.Text;
                }

                // Spoil Cost
                if (e.Row.FindControl("lbl_WastageCost") != null)
                {
                    var label = e.Row.FindControl("lbl_WastageCost") as Label;
                    label.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "SpoilCost"));
                    label.ToolTip = label.Text;
                }


                // Total Cost
                if (e.Row.FindControl("lbl_TotalCost") != null)
                {
                    var label = e.Row.FindControl("lbl_TotalCost") as Label;
                    label.Text = String.Format(DefaultAmtFmt, DataBinder.Eval(e.Row.DataItem, "TotalCost"));
                    label.ToolTip = label.Text;
                }

                // -- Summary Section
                // Product Description 2
                if (e.Row.FindControl("lbl_PrdDesc2") != null)
                {
                    var lable = e.Row.FindControl("lbl_PrdDesc2") as Label;
                    lable.Text = rcpDt.GetItemDesc2(DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString(), hf_ConnStr.Value);
                    lable.ToolTip = lable.Text;
                }


                // Base Unit
                if (e.Row.FindControl("lbl_BaseUnit_Nm") != null)
                {
                    var lable = e.Row.FindControl("lbl_BaseUnit_Nm") as Label;
                    lable.Text = DataBinder.Eval(e.Row.DataItem, "BaseUnit").ToString();
                    lable.ToolTip = lable.Text;
                }

                // Unit Rate
                if (e.Row.FindControl("lbl_UnitRate_Nm") != null)
                {
                    var lable = e.Row.FindControl("lbl_UnitRate_Nm") as Label;
                    lable.Text = String.Format("{0:N}", DataBinder.Eval(e.Row.DataItem, "UnitRate"));
                    lable.ToolTip = lable.Text;
                }

                if (e.Row.FindControl("lbl_UnitRcp") != null)
                {
                    var lable = e.Row.FindControl("lbl_UnitRcp") as Label;
                    lable.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lable.ToolTip = lable.Text;
                }


                //    // BaseCost
                //    if (e.Row.FindControl("lbl_BaseCost_Nm") != null)
                //    {
                //        var label = e.Row.FindControl("lbl_BaseCost_Nm") as Label;
                //        decimal baseCost = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "BaseCost").ToString());
                //        label.Text = String.Format("{0:0.00}", baseCost);
                //        label.ToolTip = label.Text;

                //        // Added on: 07/09/2017, By: Fon
                //        if (e.Row.FindControl("txt_NetCost") != null)
                //        {
                //            txt_NetCost = (TextBox)e.Row.FindControl("txt_NetCost");
                //            txt_NetCost.Visible = false;
                //            if (baseCost <= 0)
                //            {
                //                txt_NetCost.Text = string.Format("{0:N}", totalCost);
                //                txt_NetCost.Visible = true;
                //                lbl_NetCost.Visible = false;
                //            }
                //        }
                //        // End Added.
                //    }

                //    // Net Cost
                //    if (e.Row.FindControl("lbl_NetCost_Nm") != null)
                //    {
                //        var label = e.Row.FindControl("lbl_NetCost_Nm") as Label;
                //        label.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "NetCost").ToString());
                //        label.ToolTip = label.Text;
                //    }

                //    // Spoil Cost
                //    if (e.Row.FindControl("lbl_SpoilCost_Nm") != null)
                //    {
                //        var label = e.Row.FindControl("lbl_SpoilCost_Nm") as Label;
                //        label.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "SpoilCost").ToString());
                //        label.ToolTip = label.Text;
                //    }

                //    // Total Cost
                //    //if (e.Row.FindControl("lbl_TotalCost_Nm") != null)
                //    //{
                //    //    var label = e.Row.FindControl("lbl_TotalCost_Nm") as Label;
                //    //    label.Text = String.Format("{0:0.00}", DataBinder.Eval(e.Row.DataItem, "TotalCost").ToString());
                //    //    label.ToolTip = label.Text;
                //    //}


            }
        }

        protected void grd_RecipeDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "SAVENEW" || e.CommandName.ToUpper() == "SAVE")
            {
                SaveRcpDt();

                RcpEditMode = string.Empty;
                if (e.CommandName.ToUpper() == "SAVENEW")
                    CreateDetail();
            }


        }

        protected void grd_RecipeDt_RowEditing(object sender, GridViewEditEventArgs e)
        {

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = e.NewEditIndex;
            grd_RecipeDt.DataBind();

            RcpEditMode = "EDIT";

        }

        protected void grd_RecipeDt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                if (RcpEditMode.ToUpper() == "NEW")
                {
                    dsRecipe.Tables[rcpDt.TableName].Rows[dsRecipe.Tables[rcpDt.TableName].Rows.Count - 1].Delete();
                }
                if (RcpEditMode.ToUpper() == "EDIT")
                {
                    dsRecipe.Tables[rcpDt.TableName].Rows[dsRecipe.Tables[rcpDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            if (Request.Params["MODE"].ToUpper() == "EDIT")
            {
                if (RcpEditMode.ToUpper() == "NEW")
                {
                    dsRecipe.Tables[rcpDt.TableName].Rows[dsRecipe.Tables[rcpDt.TableName].Rows.Count - 1].Delete();
                }
                if (RcpEditMode.ToUpper() == "EDIT")
                {
                    dsRecipe.Tables[rcpDt.TableName].Rows[dsRecipe.Tables[rcpDt.TableName].Rows.Count - 1].CancelEdit();
                }
            }

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = -1;
            grd_RecipeDt.DataBind();
        }

        protected void grd_RecipeDt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dsRecipe.Tables[rcpDt.TableName].Rows[e.RowIndex].Delete();
            dsRecipe.Tables[rcpDt.TableName].AcceptChanges();

            grd_RecipeDt.DataSource = dsRecipe.Tables[rcpDt.TableName];
            grd_RecipeDt.EditIndex = -1;
            grd_RecipeDt.DataBind();

            Session["dsRecipe"] = dsRecipe;

            e.Cancel = true;

        }

        private void CalculationRcpDt(string itemCode)
        {
            var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];

            var lbl_Type = grd.FindControl("lbl_ItemType") as Label;

            var lbl_NetCost = grd.FindControl("lbl_NetCost") as Label;
            var lbl_WastageCost = grd.FindControl("lbl_WastageCost") as Label;
            var lbl_TotalCost = grd.FindControl("lbl_TotalCost") as Label;

            var txt_Qty = grd.FindControl("txt_Qty") as ASPxSpinEdit;
            var txt_BaseCost = grd.FindControl("txt_BaseCost") as ASPxSpinEdit;
            var txt_WastageRate = grd.FindControl("txt_WastageRate") as ASPxSpinEdit;

            //var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
            var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;

            var lbl_BaseUnit_Nm = grd.FindControl("lbl_BaseUnit_Nm") as Label;
            var lbl_UnitRate_Nm = grd.FindControl("lbl_UnitRate_Nm") as Label;
            var lbl_UnitCost_Nm = grd.FindControl("lbl_BaseCost_Nm") as Label;
            //var lbl_CostUpdated = grd.FindControl("lbl_CostUpdated") as Label;


            //ddl_Unit.DataSource = rcpDt.GetListUnit(itemCode, hf_ConnStr.Value);
            //ddl_Unit.DataBind();
            //ddl_Unit.Text = rcpDt.GetUnitDefault(itemCode, hf_ConnStr.Value);
            lbl_BaseUnit_Nm.Text = rcpDt.GetBaseUnit(itemCode, hf_ConnStr.Value);

            string itemType = lbl_Type.Text[0].ToString();
            decimal unitRate = Convert.ToDecimal(lbl_UnitRate_Nm.Text);
            decimal baseCost = txt_BaseCost.Number;
            decimal qty = txt_Qty.Number;
            decimal netCost = RoundAmt(qty * baseCost / unitRate);
            decimal wastageRate = txt_WastageRate.Number;
            decimal wastageCost = RoundAmt(netCost * wastageRate / 100);
            decimal totalCost = netCost + wastageCost;

            txt_BaseCost.Text = string.Format(DefaultAmtFmt, baseCost);
            lbl_NetCost.Text = string.Format(DefaultAmtFmt, netCost);
            lbl_WastageCost.Text = string.Format(DefaultAmtFmt, wastageCost);
            lbl_TotalCost.Text = string.Format(DefaultAmtFmt, totalCost);

        }


        //private void ItemCalculation(decimal baseCost = 0m)
        //{
        //    var grd = grd_RecipeDt.Rows[grd_RecipeDt.EditIndex];

        //    var lbl_ItemType = grd.FindControl("lbl_ItemType") as Label;

        //    var lbl_NetCost = grd.FindControl("lbl_NetCost") as Label;
        //    var lbl_WastageCost = grd.FindControl("lbl_WastageCost") as Label;
        //    var lbl_TotalCost = grd.FindControl("lbl_TotalCost") as Label;

        //    var txt_Qty = grd.FindControl("txt_Qty") as ASPxSpinEdit;
        //    var txt_BaseCost = grd.FindControl("txt_BaseCost") as ASPxSpinEdit;
        //    var txt_WastageRate = grd.FindControl("txt_WastageRate") as ASPxSpinEdit;

        //    var ddl_ItemCode = grd.FindControl("ddl_ItemCode") as ASPxComboBox;
        //    var ddl_Unit = grd.FindControl("ddl_Unit") as ASPxComboBox;

        //    var lbl_BaseUnit_Nm = grd.FindControl("lbl_BaseUnit_Nm") as Label;
        //    var lbl_UnitRate_Nm = grd.FindControl("lbl_UnitRate_Nm") as Label;
        //    var lbl_UnitCost_Nm = grd.FindControl("lbl_BaseCost_Nm") as Label;
        //    //var lbl_CostUpdated = grd.FindControl("lbl_CostUpdated") as Label;

        //    string itemType = lbl_ItemType.Text[0].ToString();
        //    string itemCode = ddl_ItemCode.ClientValue.ToString();

        //    ddl_Unit.DataSource = rcpDt.GetListUnit(itemCode, hf_ConnStr.Value);
        //    ddl_Unit.DataBind();
        //    ddl_Unit.Text = rcpDt.GetUnitDefault(itemCode, hf_ConnStr.Value);
        //    lbl_BaseUnit_Nm.Text = rcpDt.GetBaseUnit(itemCode, hf_ConnStr.Value);

        //    decimal unitRate = rcpDt.GetUnitConvRate(itemCode, ddl_Unit.Text, hf_ConnStr.Value);
        //    if (baseCost == 0m)
        //        baseCost = rcpDt.GetLastCost(itemType, itemCode, DateTime.Now, LoginInfo.ConnStr);
        //    decimal qty = txt_Qty.Number;
        //    decimal netCost = RoundAmt(qty * baseCost / unitRate);
        //    decimal wastageRate = txt_WastageRate.Number;
        //    decimal wastageCost = RoundAmt(netCost * wastageRate / 100);
        //    decimal totalCost = netCost + wastageCost;

        //    lbl_UnitRate_Nm.Text = String.Format("{0:N3}", unitRate);
        //    txt_BaseCost.Text = string.Format(DefaultAmtFmt, baseCost);
        //    lbl_NetCost.Text = string.Format(DefaultAmtFmt, netCost);
        //    lbl_WastageCost.Text = string.Format(DefaultAmtFmt, wastageCost);
        //    lbl_TotalCost.Text = string.Format(DefaultAmtFmt, totalCost);

        //}

        #region "Recipe Cost"

        protected void CalculateSummary()
        {
            DataTable dtRcpDt = dsRecipe.Tables[rcpDt.TableName];

            decimal totalCost = 0;

            foreach (DataRow row in dtRcpDt.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    totalCost += Convert.ToDecimal(row["TotalCost"]);
            }

            decimal totalMix = string.IsNullOrEmpty(txt_TotalMix.Text) || Convert.ToDecimal(txt_TotalMix.Text) == 0 ? 0 : RoundAmt(Convert.ToDecimal(txt_TotalMix.Text));
            decimal costTotalMix = RoundAmt(totalCost + (totalCost * totalMix / 100));
            decimal costOfPortion = string.IsNullOrEmpty(txt_PortionSize.Text) || Convert.ToDecimal(txt_PortionSize.Text) == 0 ? 0 : totalCost / Convert.ToDecimal(txt_PortionSize.Text);

            decimal svcRate = Convert.ToDecimal(hf_DefaultSvcRate.Value);
            decimal taxRate = Convert.ToDecimal(hf_DefaultTaxRate.Value);



            decimal netCost = string.IsNullOrEmpty(txt_NetCost.Text) || Convert.ToDecimal(txt_NetCost.Text) == 0 ? 0 : Convert.ToDecimal(txt_NetCost.Text);
            decimal netPrice = netCost * costTotalMix / 100; ;

            decimal svcAmt = RoundAmt(netPrice * svcRate / 100);
            decimal taxAmt = RoundAmt((netPrice + svcAmt) * taxRate / 100);
            // grossPrice = netPrice + svcAmt + taxAmt
            decimal grossPrice = netPrice + svcAmt + taxAmt;
            decimal grossCost = costTotalMix == 0 ? 0 : RoundAmt(grossPrice * 100 / costTotalMix);


            txt_TotalCost.Text = string.Format(DefaultAmtFmt, totalCost);
            txt_TotalMixAmt.Text = String.Format(DefaultAmtFmt, totalMix);
            txt_CostTotalMix.Text = String.Format(DefaultAmtFmt, costTotalMix);
            lblCostOfPortion.Text = String.Format(DefaultAmtFmt, costOfPortion);

            txt_NetPrice.Text = string.Format(DefaultAmtFmt, netPrice);
            txt_GrossPrice.Text = string.Format(DefaultAmtFmt, grossPrice);

            txt_NetCost.Text = string.Format(DefaultAmtFmt, netCost);
            txt_GrossCost.Text = string.Format(DefaultAmtFmt, grossCost);

        }

        //protected void calculatePrice()
        //{
        //    decimal serviceCharge = (svcRate / 100);
        //    decimal vat = (vatRate / 100);
        //    decimal netPrice = 0;
        //    decimal CostTotalMix = Convert.ToDecimal(txt_CostTotalMix.Text);
        //    if (txt_NetPrice.Text != "0.00") { netPrice = Convert.ToDecimal(txt_NetPrice.Text); }
        //    else { return; }

        //    decimal plusCharge = ((netPrice * serviceCharge) + netPrice);
        //    decimal grossPrice = ((plusCharge * vat) + plusCharge);
        //    decimal netCost = ((CostTotalMix / netPrice) * 100);
        //    decimal grossCost = ((CostTotalMix / grossPrice) * 100);

        //    txt_GrossPrice.Text = Convert.ToDecimal(grossPrice).ToString("#,##0.00");
        //    txt_NetCost.Text = String.Format("{0:0.00}", netCost).ToString();
        //    txt_GrossCost.Text = String.Format("{0:0.00}", grossCost).ToString();

        //    Session["Old_netCost"] = netCost;
        //}
        #endregion

        #region "Image Upload"
        protected void btnUploadHide_Click(object sender, EventArgs e)
        {
            pop_FileUpload.ShowOnPageLoad = false;
            img01.Visible = true;
            img01.ImageUrl = (string)Session["ImgBytes"];
        }

        protected void btnUploadPopup_Click(object sender, EventArgs e)
        {
            pop_FileUpload.ShowOnPageLoad = true;
        }

        protected byte[] convertImageToByte()
        {
            ImageConverter imgCon = new ImageConverter();
            return (byte[])imgCon.ConvertTo(img01, typeof(byte[]));
        }
        #endregion


    }
}