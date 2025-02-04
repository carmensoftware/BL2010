using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using System.Text;

namespace BlueLedger.PL.PT.RCP
{

    public partial class RecipeEdit : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        private readonly Blue.BL.PT.RCP.RcpDt rcpDt = new Blue.BL.PT.RCP.RcpDt();

        #region "Attributes"

        private string _BuCode { get { return Request.QueryString["BuCode"] == null ? "" : Request.QueryString["BuCode"].ToString(); } }
        private string _VID { get { return Request.QueryString["VID"] == null ? "" : Request.QueryString["VID"].ToString(); } }
        private string _MODE { get { return Request.QueryString["MODE"] == null ? "" : Request.QueryString["MODE"].ToString(); } }

        private string _ID { get { return Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString(); } }

        private decimal _serviceRate { get { return string.IsNullOrEmpty(hf_DefaultSvcRate.Value) ? 0m : Convert.ToDecimal(hf_DefaultSvcRate.Value); } }
        private decimal _taxRate { get { return string.IsNullOrEmpty(hf_DefaultTaxRate.Value) ? 0m : Convert.ToDecimal(hf_DefaultTaxRate.Value); } }


        private DataTable _dtRcp
        {
            get { return ViewState["_dtRcp"] as DataTable; }
            set { ViewState["_dtRcp"] = value; }
        }

        private DataTable _dtRcpDt
        {
            get { return ViewState["_dtRcpDt"] as DataTable; }
            set { ViewState["_dtRcpDt"] = value; }
        }

        private DataTable _dtItem
        {
            get { return ViewState["_dtItem"] as DataTable; }
            set { ViewState["_dtItem"] = value; }
        }

        private DataTable _dtUnit
        {
            get { return ViewState["_dtUnit"] as DataTable; }
            set { ViewState["_dtUnit"] = value; }
        }

        #endregion

        // -----------------------------------------------------------------------------------------------------------

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            var serviceRate = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);
            var taxRate = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);

            hf_DefaultSvcRate.Value = string.IsNullOrEmpty(serviceRate) ? "0" : serviceRate;
            hf_DefaultTaxRate.Value = string.IsNullOrEmpty(taxRate) ? "0" : taxRate;

            //lbl_TaxSvcRate.Text = string.Format("Service charge: {0}% , Tax Rate: {1}%", hf_DefaultSvcRate.Value.ToString(), hf_DefaultTaxRate.Value.ToString());
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
        }

        private void Page_Retrieve()
        {

            var sql = new Helpers.SQL(hf_ConnStr.Value);

            _dtRcp = sql.ExecuteQuery("SELECT  * FROM PT.Rcp WHERE RcpCode=@RcpCode", new SqlParameter[] { new SqlParameter("@RcpCode", _ID) });
            _dtRcpDt = sql.ExecuteQuery("SELECT NEWID() as RowId, * FROM PT.RcpDt WHERE RcpCode=@RcpCode", new SqlParameter[] { new SqlParameter("@RcpCode", _ID) });

            _dtItem = GetDataItem(_ID);

            Page_Setting();
        }

        private void Page_Setting()
        {
            // Header
            se_TotalMixRate.DecimalPlaces = DefaultAmtDigit;
            se_NetPrice.DecimalPlaces = DefaultAmtDigit;
            se_GrossPrice.DecimalPlaces = DefaultAmtDigit;
            se_NetCost.DecimalPlaces = DefaultAmtDigit;
            se_GrossCost.DecimalPlaces = DefaultAmtDigit;


            // Edit
            if (_dtRcp.Rows.Count > 0)
            {
                var dr = _dtRcp.Rows[0];

                txt_RcpCode.Text = dr["RcpCode"].ToString();
                txt_RcpCode.ReadOnly = true;

                txt_RcpDesc1.Text = dr["RcpDesc1"].ToString();
                txt_RcpDesc2.Text = dr["RcpDesc2"].ToString();
                ddl_Category.Value = dr["RcpCateCode"].ToString();
                ddl_Location.Value = dr["LocationCode"].ToString();
                ddl_RcpUnit.Value = dr["RcpUnitCode"].ToString();

                txt_Preparation.Text = dr["Preparation"].ToString();
                se_PortionSize.Text = string.IsNullOrEmpty(dr["PortionSize"].ToString()) ? "0" : dr["PortionSize"].ToString();
                se_PortionCost.Text = string.IsNullOrEmpty(dr["PortionCost"].ToString()) ? "0" : dr["PortionCost"].ToString();

                se_PrepTime.Text = string.IsNullOrEmpty(dr["PrepTime"].ToString()) ? "0" : dr["PrepTime"].ToString();
                se_TotalTime.Text = string.IsNullOrEmpty(dr["TotalTime"].ToString()) ? "0" : dr["TotalTime"].ToString();

                txt_Remark.Text = dr["Remark"].ToString();
                ddl_Status.SelectedValue = dr["IsActived"].ToString();

                var image = dr["RcpImage"].ToString();
                img_RcpImage.ImageUrl = image;


                se_TotalCost.Number = GetDecimal(dr["RcpCost"].ToString());
                se_TotalMixRate.Number = GetDecimal(dr["MixRatio"].ToString());
                se_TotalMix.Number = GetDecimal(dr["MixCost"].ToString());
                se_CostTotalMix.Number = se_TotalCost.Number + se_TotalMix.Number;
                se_NetPrice.Number = GetDecimal(dr["NetPrice"].ToString());
                se_GrossPrice.Number = GetDecimal(dr["GrossPrice"].ToString());
                se_NetCost.Number = GetDecimal(dr["NetCost"].ToString());
                se_GrossCost.Number = GetDecimal(dr["GrossCost"].ToString());

                lbl_ServiceRate.Text = FormatAmt(hf_DefaultSvcRate.Value);
                lbl_TaxRate.Text = FormatAmt(hf_DefaultTaxRate.Value);
            }

            // Details
            gv_Detail.DataSource = _dtRcpDt;
            gv_Detail.DataBind();

        }

        #region "Events"

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "UPDATE":
                    var toDate = DateTime.Today;

                    UpdateCost(toDate);

                    break;

                case "SAVE":
                    pop_ConfirmSave.ShowOnPageLoad = true;
                    break;


                case "BACK":
                    var url = string.IsNullOrEmpty(_ID)
                        ? "RecipeList.aspx"
                        : "RecipeDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"];

                    Response.Redirect(url);

                    break;
            }
        }

        protected void btn_ComfirmSave_Click(object sender, EventArgs e)
        {
            var rcpCode = Save();

            if (!string.IsNullOrEmpty(rcpCode))
            {
                Response.Redirect("RecipeDt.aspx?BuCode=" + _BuCode + "&ID=" + rcpCode + "&VID=" + _VID);
            }
        }

        // Header
        protected void btn_UploadImg_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                var fileName = fileUpload.FileName;

                var fs = fileUpload.PostedFile.InputStream;
                var br = new System.IO.BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                var base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                int index = fileName.LastIndexOf('.');
                string extension = (index >= 0) ? fileName.Substring(index + 1).ToLower() : "jpg";

                img_RcpImage.ImageUrl = "data:image/" + extension + ";base64," + base64String;
            }
            else
            {
                ShowAlert("Please choose a file.");
            }
        }

        protected void ddl_Category_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            ddl.Items.Clear();
            ddl.Items.AddRange(GetDropdownItem("SELECT RcpCateCode as [Value], CONCAT(RcpCateCode, ' : ', RcpCateDesc) as [Text] FROM PT.RcpCategory ORDER BY RcpCateCode"));
        }

        protected void ddl_Location_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var parameters = new SqlParameter[] { new SqlParameter("LoginName", LoginInfo.LoginName) };

            var query = @"SELECT 
	l.LocationCode as [Value],
	CONCAT(l.LocationCode, ' : ', l.LocationName) as [Text]
FROM 
	[IN].StoreLocation l 
	JOIN [ADMIN].UserStore us 
		ON us.LocationCode=l.LocationCode
WHERE
	l.EOP <> 2
	AND LoginName = @LoginName
ORDER BY
	l.LocationCode";
            ddl.Items.AddRange(GetDropdownItem(query, parameters));


        }

        protected void ddl_RcpUnit_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var query = @"SELECT UnitCode as [Value], CONCAT(UnitCode,' : ',[Name]) as [Text] FROM	[IN].Unit ORDER BY UnitCode";
            ddl.Items.AddRange(GetDropdownItem(query));
        }

        protected void se_PortionSize_NumberChanged(object sender, EventArgs e)
        {
            var se = sender as ASPxSpinEdit;

            se_PortionCost.Value = GetCostOfPortion();
        }

        // Summary

        protected void se_TotalMixRate_NumberChanged(object sender, EventArgs e)
        {
            CalculateSummary();
        }

        protected void se_NetPrice_NumberChanged(object sender, EventArgs e)
        {
            var netPrice = (sender as ASPxSpinEdit).Number;
            var costTotalMix = se_CostTotalMix.Number;

            var grossPrice = GetValueAdded(netPrice);
            var netCost = GetValueByCostTotalMix(netPrice);
            var grossCost = GetValueByCostTotalMix(grossPrice);


            se_GrossPrice.Number = grossPrice;
            se_NetCost.Number = netCost;
            se_GrossCost.Number = grossCost;
        }

        protected void se_GrossPrice_NumberChanged(object sender, EventArgs e)
        {
            var grossPrice = (sender as ASPxSpinEdit).Number;
            var costTotalMix = se_CostTotalMix.Number;

            var netPrice = GetValueIncluded(grossPrice);
            var netCost = GetValueByCostTotalMix(netPrice);
            var grossCost = GetValueByCostTotalMix(grossPrice);

            se_NetPrice.Number = netPrice;
            se_NetCost.Number = netCost;
            se_GrossCost.Number = grossCost;
        }

        protected void se_NetCost_NumberChanged(object sender, EventArgs e)
        {
            var netCost = (sender as ASPxSpinEdit).Number;
            var costTotalMix = se_CostTotalMix.Number;

            var netPrice = GetValueByCostTotalMix(netCost);
            var grossPrice = GetValueAdded(netPrice);
            var grossCost = GetValueByCostTotalMix(grossPrice);


            se_NetPrice.Number = netPrice;
            se_GrossPrice.Number = grossPrice;

            se_GrossCost.Number = grossCost;
        }

        protected void se_GrossCost_NumberChanged(object sender, EventArgs e)
        {
            var grossCost = (sender as ASPxSpinEdit).Number;

            var costTotalMix = se_CostTotalMix.Number;

            var grossPrice = GetValueByCostTotalMix(grossCost); //RoundAmt(RoundAmt(costTotalMix / grossCost) * 100);
            var netPrice = GetValueIncluded(grossPrice);
            var netCost = GetValueByCostTotalMix(netPrice); // RoundAmt(RoundAmt(costTotalMix / netPrice) * 100);


            se_NetPrice.Number = netPrice;
            se_GrossPrice.Number = grossPrice;

            se_NetCost.Number = netCost;
        }



        // ---------------------------------------------------------------------------------------

        protected void menu_CmdItem_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateDetail();
                    break;

                case "DELETE":
                    if (HasPendingItem())
                    {
                        return;
                    }


                    var items = new List<string>();
                    var gv = gv_Detail;
                    foreach (GridViewRow row in gv.Rows)
                    {
                        if (((CheckBox)row.FindControl("Chk_Item")).Checked)
                        {
                            var hf_RowId = row.FindControl("hf_RowId") as HiddenField;
                            items.Add(hf_RowId.Value);
                        }
                    }

                    if (items.Count > 0)
                    {

                        hf_DeleteItems.Value = JsonConvert.SerializeObject(items);

                        ShowConfirmDelete(string.Format("Do you want to delete {0} item(s)?", items.Count));

                        return;
                    }
                    else
                        ShowAlert("Please select any item.");


                    break;
            }
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            var values = string.IsNullOrEmpty(hf_DeleteItems.Value) ? "[]" : hf_DeleteItems.Value;
            var items = JsonConvert.DeserializeObject<IEnumerable<string>>(values);

            if (items.Count() > 0)
            {
                DeleteDetails(items);
            }
            pop_ConfirmDelete.ShowOnPageLoad = false;
        }

        // GrdiView
        protected void gv_Detail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var gv = sender as GridView;

            var rowId = e.CommandArgument.ToString();
            var commandName = e.CommandName.ToUpper();

            switch (commandName)
            {
                case "CREATE":
                    CreateDetail();

                    break;

                case "SAVE":
                case "SAVENEW":
                    var row = gv.Rows[gv.EditIndex];
                    var error = SaveDetail(row);

                    if (!string.IsNullOrEmpty(error))
                    {
                        ShowAlert(error);
                        return;
                    }

                    gv_Detail.EditIndex = -1;
                    BindDetails();

                    if (commandName == "SAVENEW")
                        CreateDetail();

                    break;

                case "DEL":
                    var items = new string[] { rowId };

                    var dr = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

                    var itemCode = dr == null ? "" : dr["IngredientCode"].ToString();


                    hf_DeleteItems.Value = JsonConvert.SerializeObject(items);

                    ShowConfirmDelete(string.Format("Do you want to delete this item '{0}'?", itemCode));
                    break;
            }
        }

        protected void gv_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //if (e.Row.FindControl("btn_Expand") != null)
                //{
                //    var item = e.Row.FindControl("btn_Expand") as ImageButton;

                //    item.Attributes.Add("data-id", DataBinder.Eval(e.Row.DataItem, "RowId").ToString());
                //}

                var itemType = DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString();
                var itemCode = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString();

                var names = GetitemName(itemType, itemCode);
                var name1 = names != null ? names["Name1"].ToString() : "";
                var name2 = names != null ? names["Name2"].ToString() : "";

                if (e.Row.FindControl("hf_RowId") != null)
                {
                    var item = e.Row.FindControl("hf_RowId") as HiddenField;

                    item.Value = DataBinder.Eval(e.Row.DataItem, "RowId").ToString();
                }

                if (e.Row.FindControl("lbl_ItemType") != null)
                {
                    var lbl = e.Row.FindControl("lbl_ItemType") as Label;
                    //var value = DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString();

                    lbl.Text = itemType == "P" ? "Product" : "Recipe";
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("ddl_ItemType") != null)
                {
                    var ddl = e.Row.FindControl("ddl_ItemType") as ASPxComboBox;

                    ddl.Value = itemType;
                }



                if (e.Row.FindControl("lbl_Item") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Item") as Label;

                    lbl.Text = string.Format("{0} : {1}", itemCode, name1);
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("ddl_Item") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Item") as ASPxComboBox;

                    ddl.Value = itemCode;
                }

                if (e.Row.FindControl("lbl_ItemDesc2") != null)
                {
                    var lbl = e.Row.FindControl("lbl_ItemDesc2") as Label;

                    lbl.Text = name2;
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("ddl_Unit") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Unit") as ASPxComboBox;

                    ddl.Value = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                }

                if (e.Row.FindControl("lbl_UnitRate") != null)
                {
                    var lbl = e.Row.FindControl("lbl_UnitRate") as Label;

                    lbl.Text = FormatUnitRate(DataBinder.Eval(e.Row.DataItem, "UnitRate"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_BaseUnit") != null)
                {
                    var lbl = e.Row.FindControl("lbl_BaseUnit") as Label;

                    lbl.Text = DataBinder.Eval(e.Row.DataItem, "BaseUnit").ToString();
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("num_Qty") != null)
                {
                    var num = e.Row.FindControl("num_Qty") as ASPxSpinEdit;
                    num.DecimalPlaces = DefaultQtyDigit;

                    num.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }

                if (e.Row.FindControl("num_BaseCost") != null)
                {
                    var num = e.Row.FindControl("num_BaseCost") as ASPxSpinEdit;
                    num.DecimalPlaces = DefaultQtyDigit;

                    num.Text = DataBinder.Eval(e.Row.DataItem, "BaseCost").ToString();
                }

                if (e.Row.FindControl("num_SpoilRate") != null)
                {
                    var num = e.Row.FindControl("num_SpoilRate") as ASPxSpinEdit;
                    num.DecimalPlaces = DefaultQtyDigit;

                    num.Text = DataBinder.Eval(e.Row.DataItem, "SpoilRate").ToString();
                }

                if (e.Row.FindControl("lbl_NetCost") != null)
                {
                    var lbl = e.Row.FindControl("lbl_NetCost") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "NetCost"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_SpoilCost") != null)
                {
                    var lbl = e.Row.FindControl("lbl_SpoilCost") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "SpoilCost"));
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_TotalCost") != null)
                {
                    var lbl = e.Row.FindControl("lbl_TotalCost") as Label;

                    lbl.Text = FormatAmt(DataBinder.Eval(e.Row.DataItem, "TotalCost"));
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("lbl_UpdatedDate") != null)
                {
                    var lbl = e.Row.FindControl("lbl_UpdatedDate") as Label;

                    if (DataBinder.Eval(e.Row.DataItem, "UpdatedDate") != DBNull.Value)
                        lbl.Text = "@" + Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "UpdatedDate")).ToShortDateString();

                    lbl.ToolTip = lbl.Text;
                }


                // Expand Information --
                //if (e.Row.FindControl("lbl_BaseUnit") != null)
                //{
                //    var lbl = e.Row.FindControl("lbl_BaseUnit") as Label;

                //    var unit = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                //    var baseUnit = DataBinder.Eval(e.Row.DataItem, "BaseUnit").ToString();
                //    //var unitRate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "UnitRate"));

                //    lbl.Text = string.Format("Base unit : {0}", baseUnit);
                //    lbl.ToolTip = lbl.Text;
                //}
            }
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditIndex = e.NewEditIndex;
            BindDetails();
        }

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;
            var row = gv.Rows[gv.EditIndex];

            var hf_RowId = row.FindControl("hf_RowId") as HiddenField;
            var item = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == hf_RowId.Value);

            if (item != null & item.Field<int>("RcpDtId") < 0)
            {
                item.Delete();

                _dtRcpDt.AcceptChanges();
            }

            gv.EditIndex = -1;
            BindDetails();
        }

        protected void ddl_Item_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            ddl.DataSource = _dtItem;
            ddl.ValueField = "Code";
            ddl.TextField = "Text";
            ddl.TextFormatString = "{0}";
            ddl.DataBind();
        }

        protected void ddl_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var hf_RowId = ddl.NamingContainer.FindControl("hf_RowId") as HiddenField;
            var ddl_Unit = ddl.NamingContainer.FindControl("ddl_Unit") as ASPxComboBox;
            var lbl_ItemType = ddl.NamingContainer.FindControl("lbl_ItemType") as Label;
            var lbl_BaseUnit = ddl.NamingContainer.FindControl("lbl_BaseUnit") as Label;
            var lbl_UnitRate = ddl.NamingContainer.FindControl("lbl_UnitRate") as Label;

            var rowId = hf_RowId.Value;
            var type = ddl.Value == null ? "" : ddl.SelectedItem.GetValue("Type").ToString();
            var code = ddl.Value == null ? "" : ddl.Value.ToString();
            var baseUnit = ddl.Value == null ? "" : ddl.SelectedItem.GetValue("BaseUnit").ToString();
            var unit = ddl_Unit.Value == null ? "" : ddl_Unit.Value.ToString();

            BindUnit(ddl, type, code);

            var drUnit = _dtUnit.AsEnumerable().FirstOrDefault(x => x.Field<string>("Code") == unit);

            var unitRate = 0m;

            if (drUnit == null)
            {
                ddl_Unit.SelectedIndex = 0;
                unitRate = GetDecimal(ddl_Unit.SelectedItem.GetValue("Rate").ToString());
            }
            else
            {
                unitRate = drUnit.Field<decimal>("Rate");
            }


            lbl_ItemType.Text = type;
            lbl_BaseUnit.Text = baseUnit;
            lbl_UnitRate.Text = ddl_Unit.Value == null ? "0" : FormatUnitRate(unitRate.ToString());
        }

        protected void ddl_Unit_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var ddl_Item = ddl.NamingContainer.FindControl("ddl_Item") as ASPxComboBox;
            var lbl_ItemType = ddl.NamingContainer.FindControl("lbl_ItemType") as Label;

            var type = lbl_ItemType.Text;
            var code = ddl_Item.Value == null ? "" : ddl_Item.Value.ToString();

            BindUnit(ddl, type, code);
        }

        protected void ddl_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var lbl_UnitRate = ddl.NamingContainer.FindControl("lbl_UnitRate") as Label;

            var rate = ddl.SelectedItem.GetValue("Rate").ToString();

            lbl_UnitRate.Text = FormatAmt(rate);

        }

        protected void items_NumberChanged(object sender, EventArgs e)
        {
            var se = sender as ASPxSpinEdit;

            var num_Qty = se.NamingContainer.FindControl("num_Qty") as ASPxSpinEdit;
            var num_BaseCost = se.NamingContainer.FindControl("num_BaseCost") as ASPxSpinEdit;
            var num_SpoilRate = se.NamingContainer.FindControl("num_SpoilRate") as ASPxSpinEdit;
            var lbl_UnitRate = se.NamingContainer.FindControl("lbl_UnitRate") as Label;

            var lbl_NetCost = se.NamingContainer.FindControl("lbl_NetCost") as Label;
            var lbl_SpoilCost = se.NamingContainer.FindControl("lbl_SpoilCost") as Label;
            var lbl_TotalCost = se.NamingContainer.FindControl("lbl_TotalCost") as Label;

            var qty = GetValue(num_Qty);
            var baseCost = GetValue(num_BaseCost);
            var spoilRate = GetValue(num_SpoilRate);
            var unitRate = GetDecimal(lbl_UnitRate.Text);

            //var netCost = FormatAmt(lbl_NetCost.Text);
            //var spoilCost = FormatAmt(lbl_SpoilCost.Text);
            //var totalCost = FormatAmt(lbl_TotalCost);

            // TotalCost = (Qty/UnitRate) * BaseCost
            var totalCost = GetTotalCost(qty, unitRate, baseCost);

            // SpoilCost = TotalCost * SpoilRate/100
            var spoilCost = GetSpoilCost(totalCost, spoilRate);
            var netCost = totalCost - spoilCost;

            lbl_NetCost.Text = FormatAmt(netCost);
            lbl_SpoilCost.Text = FormatAmt(spoilCost);
            lbl_TotalCost.Text = FormatAmt(totalCost);

        }


        #endregion

        #region -- Method(s) --

        private void ShowInfo(string text)
        {
            pop_Warning.HeaderText = "Information";
            txt_Error.Visible = false;

            lbl_Warning.Text = text.Trim();
            pop_Warning.ShowOnPageLoad = true;
        }

        private void ShowAlert(string text)
        {
            pop_Warning.HeaderText = "Warning";
            txt_Error.Visible = false;

            lbl_Warning.Text = text.Trim();
            pop_Warning.ShowOnPageLoad = true;
        }

        private void ShowError(string text, string error)
        {
            pop_Warning.HeaderText = "Error";

            txt_Error.Visible = true;
            lbl_Warning.Text = text.Trim();
            txt_Error.Text = error;
            pop_Warning.ShowOnPageLoad = true;
        }

        private void ShowConfirmDelete(string text)
        {
            lbl_ConfirmDelete.Text = text.Trim();
            pop_ConfirmDelete.ShowOnPageLoad = true;
        }

        protected string FormatQty(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultQtyDigit));
        }

        protected string FormatAmt(object value)
        {
            var number = string.IsNullOrEmpty(value.ToString()) ? 0m : Convert.ToDecimal(value);

            return number.ToString(string.Format("N{0}", DefaultAmtDigit));
        }

        private DataRow GetitemName(string type, string code)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var query = "";

            if (type == "P")
                query = "SELECT ProductDesc1 as Name1, ProductDesc2 as Name2 FROM [IN].Product WHERE ProductCode=@code";
            else
                query = "SELECT RcpDesc1 as Name1, RcpDesc2 as Name2 FROM [PT].Rcp WHERE RcpCode=@code";

            var dt = sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@code", code) });


            return dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        private ListEditItemCollection GetDropdownItem(string query, SqlParameter[] parameters = null)
        {
            var items = new ListEditItemCollection();

            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var dt = sql.ExecuteQuery(query, parameters);

            items.AddRange(dt.AsEnumerable()
                .Select(x => new ListEditItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                })
                .ToArray());




            return items;
        }

        private decimal GetValue(ASPxSpinEdit item)
        {
            return item.Text.Trim() == "" ? 0m : Convert.ToDecimal(item.Value);
        }

        private string GetValue(ASPxComboBox item)
        {
            return item.SelectedItem == null ? null : item.Value.ToString();
        }

        private decimal GetDecimal(string value)
        {
            return string.IsNullOrEmpty(value) ? 0m : Convert.ToDecimal(value);
        }

        private string FormatUnitRate(object value)
        {
            return Convert.ToDecimal(value).ToString("#,###0.##");
        }


        private DataTable GetRcp(string code)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var query = "SELECT TOP(1) * FROM PT.Rcp WHERE RcpCode=@RcpCode";
            var param = new SqlParameter[]
            {
                new SqlParameter("@RcpCode",code)
            };

            return sql.ExecuteQuery(query, param);
        }

        private DataTable GetRcpDt(string code)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var query = "SELECT  * FROM PT.RcpDt WHERE RcpCode=@RcpCode";
            var param = new SqlParameter[]
            {
                new SqlParameter("@RcpCode",code)
            };

            return sql.ExecuteQuery(query, param);
        }

        private DataTable GetDataItem(string rcpCode)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);

            var query = @"
SELECT 
	ProductCode as [Code], 
	CONCAT(ProductCode,' : ', ProductDesc1, ' | ', ProductDesc2) as [Text],
    InventoryUnit as BaseUnit,
	'Product' as [Type]
FROM 
	[IN].Product 
WHERE 
	IsActive=1 
	AND IsRecipe=1 
UNION ALL
SELECT 
	RcpCode as [Code], 
	CONCAT(RcpCode,' : ', RcpDesc1, ' | ', RcpDesc2) as [Text],
    RcpUnitCode as BaseUnit,
	'Recipe' as [Type]
FROM 
	PT.Rcp 
WHERE 
	IsActived=1 
    AND RcpCode <> @Code

ORDER BY
	[Text]";

            return sql.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@Code", rcpCode) });

        }

        private DataTable GetDataUnit(string type, string code)
        {
            var dtUnit = new DataTable();
            var sql = new Helpers.SQL(hf_ConnStr.Value);

            if (type.ToUpper().StartsWith("P"))
            {
                dtUnit = sql.ExecuteQuery("SELECT OrderUnit as [Code], Rate FROM [IN].ProdUnit WHERE UnitType='R' AND ProductCode=@Code", new SqlParameter[] { new SqlParameter("@Code", code) });
            }
            else
            {
                dtUnit = sql.ExecuteQuery("SELECT RcpUnitCode AS [Code], CAST(1 as DECIMAL(18,3)) AS [Rate]  FROM PT.Rcp WHERE RcpCode=@Code", new SqlParameter[] { new SqlParameter("@Code", code) });
            }

            return dtUnit;
        }

        private bool ValidateCode(string code, int length = 20)
        {
            return true;
        }

        private decimal GetTotalCost(decimal qty, decimal unitRate, decimal baseCost)
        {
            return RoundAmt(RoundAmt(qty / unitRate) * baseCost);
        }

        private decimal GetSpoilCost(decimal totalCost, decimal spoilRate)
        {
            return RoundAmt(totalCost * RoundAmt(spoilRate / 100));
        }

        private decimal GetValueAdded(decimal value)
        {
            var serviceAmt = RoundAmt(value * RoundAmt(_serviceRate / 100));
            var taxAmt = RoundAmt((value + serviceAmt) * RoundAmt(_taxRate / 100));

            return value + serviceAmt + taxAmt;
        }

        private decimal GetValueIncluded(decimal value)
        {
            // Included
            // extract VAT
            var net_service_Amt = RoundAmt(RoundAmt(value / (100 + _taxRate)) * 100);
            var taxAmt = value - net_service_Amt;

            // extract Service
            var netAmt = RoundAmt(RoundAmt(net_service_Amt / (100 + _serviceRate)) * 100);

            var serviceAmt = net_service_Amt - netAmt;

            return value - serviceAmt - taxAmt;
        }

        private decimal GetValueByCostTotalMix(decimal value)
        {
            var costTotalMix = GetValue(se_CostTotalMix);

            return value == 0 ? 0m : RoundAmt(RoundAmt(costTotalMix / value) * 100);
        }

        private void CalculateSummary()
        {
            var totalCost = GetValue(se_TotalCost);
            var totalMixRate = GetValue(se_TotalMixRate);
            var totalMix = RoundAmt(totalCost * RoundAmt(totalMixRate / 100));
            var costTotalMix = totalCost + totalMix;

            se_TotalMix.Number = totalMix;
            se_CostTotalMix.Number = costTotalMix;

            var netPrice = se_NetPrice.Number;
            var grossPrice = se_GrossPrice.Number;

            var netCost = GetValueByCostTotalMix(netPrice);
            var grossCost = GetValueByCostTotalMix(grossPrice);

            se_NetCost.Number = netCost;
            se_GrossCost.Number = grossCost;

            var portionSize = GetValue(se_PortionSize);

            se_PortionCost.Value = portionSize == 0 ? 0 : RoundAmt(costTotalMix / portionSize);
        }


        // Header
        private string Save()
        {
            var isNew = string.IsNullOrEmpty(_ID);
            var rcpCode = isNew ? txt_RcpCode.Text.Trim() : _ID;
            var categoryCode = GetValue(ddl_Category);
            var locationCode = GetValue(ddl_Location);
            var rcpUnit = GetValue(ddl_RcpUnit);

            // Check required fields
            // Header

            if (string.IsNullOrEmpty(rcpCode))
            {
                ShowAlert("Recipe code is required.");

                return null;
            }

            if (string.IsNullOrEmpty(txt_RcpDesc1.Text.Trim()))
            {
                ShowAlert("Recipe description is required.");

                return null;
            }

            if (string.IsNullOrEmpty(categoryCode))
            {
                ShowAlert("Category is required.");

                return null;
            }

            if (string.IsNullOrEmpty(locationCode))
            {
                ShowAlert("Location is required.");

                return null;
            }

            if (string.IsNullOrEmpty(rcpUnit))
            {
                ShowAlert("Recipe unit is required.");

                return null;
            }

            // Details



            // Save
            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var queries = new StringBuilder();
            var parameters = new List<SqlParameter>();


            var header_query = "";
            #region --Header--

            if (string.IsNullOrEmpty(_ID)) // Create 
            {
                // check dupliacte code

                var dtCheck = sql.ExecuteQuery("SELECT TOP(1) RcpCode, RcpDesc1 FROM PT.Rcp WHERE RcpCode=@Code", new SqlParameter[] { new SqlParameter("@Code", rcpCode) });

                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    ShowAlert(string.Format("<b>{0}</b>, this code is alrady used.", rcpCode));

                    return null;
                }


                #region -- header_query (Insert)--
                header_query =
@"INSERT INTO [PT].[Rcp] (
	[RcpCode],
	[RcpDesc1],
	[RcpDesc2],
	[RcpCateCode],
	[LocationCode],
	[RcpUnitCode],
	[Preparation],
	[PrepTime],
	[TotalTime],
	[PortionSize],
	[PortionCost],
	[Remark],
	[IsActived],
	[RcpImage],

    [RcpCost],
	[MixRatio],
	[MixCost],
	[NetPrice],
	[GrossPrice],
	[NetCost],
	[GrossCost],


    [CreatedDate],
	[CreatedBy],
	[UpdatedDate],
	[UpdatedBy],

	[IsVoid],
    [Attachment],
	[VoidComment],
	[CostUpdated]
)
VALUES	(
	@RcpCode,
	@RcpDesc1,
	@RcpDesc2,
	@RcpCateCode,
	@LocationCode,
	@RcpUnitCode,
	@Preparation,
	@PrepTime,
	@TotalTime,
	@PortionSize,
	@PortionCost,
	@Remark,
	@IsActived,
	@RcpImage,

    @TotalCost,
	@TotalMix,
	@TotalMix,
	@NetPrice,
	@GrossPrice,
	@NetCost,
	@GrossCost,

	@UpdatedDate,
	@UpdatedBy,
	@UpdatedDate,
	@UpdatedBy,

	0,
    NULL,
	NULL,
	NULL
) ";
                #endregion
            }
            else // Update
            {
                #region --header_query (Update)--
                header_query =
@"UPDATE [PT].Rcp 
SET 
    [RcpDesc1]=@RcpDesc1, 
    [RcpDesc2]=@RcpDesc2,
    [RcpCateCode]=@RcpCateCode, 
    [LocationCode]=@LocationCode, 
    [RcpUnitCode]=@RcpUnitCode,
    [Preparation]=@Preparation,
    [PrepTime]=@PrepTime,
    [TotalTime]=@TotalTime,
    [PortionSize]=@PortionSize,
    [PortionCost]=@PortionCost,
    [IsActived]=@IsActived,
    [Remark]=@Remark,
    [RcpImage]=@RcpImage,

    [RcpCost]=@TotalCost,
    [MixRatio]=@TotalMixRate,
    [MixCost]=@TotalMix,
    [NetPrice]=@NetPrice,
    [GrossPrice]=@GrossPrice,
    [NetCost]=@NetCost,
    [GrossCost]=@GrossCost,

    UpdatedDate = @UpdatedDate, 
    UpdatedBy = @UpdatedBy 
WHERE 
    RcpCode=@RcpCode";
                #endregion
            }


            parameters.Add(new SqlParameter("@RcpCode", rcpCode));
            parameters.Add(new SqlParameter("@RcpDesc1", txt_RcpDesc1.Text.Trim()));
            parameters.Add(new SqlParameter("@RcpDesc2", txt_RcpDesc2.Text.Trim()));
            parameters.Add(new SqlParameter("@RcpCateCode", categoryCode));
            parameters.Add(new SqlParameter("@LocationCode", locationCode));
            parameters.Add(new SqlParameter("@RcpUnitCode", rcpUnit));
            parameters.Add(new SqlParameter("@Preparation", txt_Preparation.Text.Trim()));
            parameters.Add(new SqlParameter("@PrepTime", GetValue(se_PrepTime)));
            parameters.Add(new SqlParameter("@TotalTime", GetValue(se_TotalTime)));
            parameters.Add(new SqlParameter("@PortionSize", GetValue(se_PortionSize)));
            parameters.Add(new SqlParameter("@PortionCost", GetValue(se_PortionCost)));
            parameters.Add(new SqlParameter("@Remark", txt_Remark.Text.Trim()));
            parameters.Add(new SqlParameter("@IsActived", ddl_Status.SelectedItem.Value.ToString()));
            parameters.Add(new SqlParameter("@RcpImage", img_RcpImage.ImageUrl.Trim()));

            parameters.Add(new SqlParameter("@TotalCost", GetValue(se_TotalCost)));
            parameters.Add(new SqlParameter("@TotalMixRate", GetValue(se_TotalMixRate)));
            parameters.Add(new SqlParameter("@TotalMix", GetValue(se_TotalMix)));
            parameters.Add(new SqlParameter("@NetPrice", GetValue(se_NetPrice)));
            parameters.Add(new SqlParameter("@GrossPrice", GetValue(se_GrossPrice)));
            parameters.Add(new SqlParameter("@NetCost", GetValue(se_NetCost)));
            parameters.Add(new SqlParameter("@GrossCost", GetValue(se_GrossCost)));

            parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            parameters.Add(new SqlParameter("@UpdatedBy", LoginInfo.LoginName));

            #endregion

            var detail_queries = new StringBuilder();
            #region -- Detail--

            if (!string.IsNullOrEmpty(_ID))
            {
                detail_queries.AppendLine("DELETE FROM PT.RcpDt WHERE RcpCode=@RcpCode");
            }

            if (_dtRcpDt.Rows.Count > 0)
            {


                detail_queries.AppendLine("INSERT INTO PT.RcpDt (RcpCode, RcpDtId, IngredientCode, IngredientType, BaseUnit, Qty, Unit, UnitRate, BaseCost, SpoilRate, SpoilCost, NetCost, TotalCost) VALUES ");

                for (int i = 0; i < _dtRcpDt.Rows.Count; i++)
                {
                    var dr = _dtRcpDt.Rows[i];

                    var rcpDtId = i + 1;
                    var code = dr["IngredientCode"].ToString();
                    var type = dr["IngredientType"].ToString();
                    var qty = GetDecimal(dr["Qty"].ToString());
                    var unit = dr["Unit"].ToString();
                    var unitRate = GetDecimal(dr["UnitRate"].ToString());
                    var baseUnit = dr["BaseUnit"].ToString();
                    var baseCost = GetDecimal(dr["BaseCost"].ToString());
                    var spoilRate = GetDecimal(dr["SpoilRate"].ToString());
                    var spoilCost = GetDecimal(dr["SpoilCost"].ToString());
                    var netCost = GetDecimal(dr["NetCost"].ToString());
                    var totalCost = GetDecimal(dr["TotalCost"].ToString());

                    detail_queries.AppendFormat("(@RcpCode, {0}, N'{1}', N'{2}', N'{3}', {4}, N'{5}', {6}, {7}, {8}, {9}, {10}, {11}),", rcpDtId, code, type, baseUnit, qty, unit, unitRate, baseCost, spoilRate, spoilCost, netCost, totalCost);
                }
            }

            #endregion

            queries.AppendLine("BEGIN TRAN");
            queries.AppendLine(header_query);
            queries.AppendLine(detail_queries.ToString().Trim().TrimEnd(','));
            queries.AppendLine(" COMMIT TRAN");

            try
            {
                sql.ExecuteQuery(queries.ToString(), parameters.ToArray());

                return rcpCode;
            }
            catch (Exception ex)
            {
                ShowError("!Not saved.", ex.Message);

                return null;
            }

        }

        // Details

        private void BindDetails()
        {
            gv_Detail.DataSource = _dtRcpDt;
            gv_Detail.DataBind();
        }

        private void BindUnit(Control control, string type, string code)
        {
            var ddl_Unit = control.NamingContainer.FindControl("ddl_Unit") as ASPxComboBox;

            _dtUnit = GetDataUnit(type, code);
            ddl_Unit.DataSource = _dtUnit;
            ddl_Unit.ValueField = "Code";
            ddl_Unit.TextField = "Code";
            ddl_Unit.TextFormatString = "{0}";
            ddl_Unit.DataBind();
        }

        private void CreateDetail()
        {
            if (HasPendingItem())
            {
                return;
            }

            var rowCount = _dtRcpDt.Rows.Count;

            var dr = _dtRcpDt.NewRow();

            dr["RowId"] = Guid.NewGuid();
            dr["RcpCode"] = txt_RcpCode.Text.Trim();
            dr["RcpDtId"] = -1;
            dr["Qty"] = 0;
            dr["UnitRate"] = 0;
            dr["BaseCost"] = 0;
            dr["SpoilRate"] = 0;
            dr["NetCost"] = 0;
            dr["TotalCost"] = 0;

            _dtRcpDt.Rows.Add(dr);
            //_dtStockInDt.AcceptChanges();

            gv_Detail.EditIndex = rowCount;
            BindDetails();
        }

        private bool HasPendingItem()
        {
            foreach (DataRow dr in _dtRcpDt.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    ShowAlert("Please save/cancel the pending item.");

                    return true;
                }
            }

            return false;

        }
        private void DeleteDetails(IEnumerable<string> rowIds)
        {
            var items = rowIds.ToArray();

            foreach (DataRow dr in _dtRcpDt.Rows)
            {
                var rowId = dr["RowId"].ToString();

                if (items.Contains(rowId))
                {
                    dr.Delete();
                }
            }

            _dtRcpDt.AcceptChanges();

            BindDetails();
        }

        private string SaveDetail(GridViewRow row)
        {
            var hf_RowId = row.FindControl("hf_RowId") as HiddenField;
            var lbl_ItemType = row.FindControl("lbl_ItemType") as Label;
            var ddl_Item = row.FindControl("ddl_Item") as ASPxComboBox;
            var ddl_Unit = row.FindControl("ddl_Unit") as ASPxComboBox;
            var num_Qty = row.FindControl("num_Qty") as ASPxSpinEdit;
            var num_BaseCost = row.FindControl("num_BaseCost") as ASPxSpinEdit;
            var num_SpoilRate = row.FindControl("num_SpoilRate") as ASPxSpinEdit;


            var lbl_NetCost = row.FindControl("lbl_NetCost") as Label;
            var lbl_SpoilCost = row.FindControl("lbl_SpoilCost") as Label;
            var lbl_TotalCost = row.FindControl("lbl_TotalCost") as Label;


            #region -- Check required values --

            if (ddl_Item.Value == null)
            {
                return "Product is required.";
            }

            if (ddl_Unit.Value == null)
            {
                return "Unit is required.";
            }

            if (string.IsNullOrEmpty(num_Qty.Text) || num_Qty.Number == 0m)
            {
                return "Quantity is required.";
            }


            #endregion

            var rowId = hf_RowId.Value;
            var type = lbl_ItemType.Text.ToUpper()[0].ToString();
            var code = ddl_Item.Value.ToString();
            var unit = ddl_Unit.Value.ToString();

            var drItem = _dtItem.AsEnumerable().FirstOrDefault(x => x.Field<string>("Code") == code);
            var baseUnit = drItem == null ? "" : drItem.Field<string>("BaseUnit");

            var drUnit = _dtUnit.AsEnumerable().FirstOrDefault(x => x.Field<string>("Code") == unit);
            var unitRate = drUnit == null ? 0 : drUnit.Field<decimal>("Rate");

            if (unitRate == 0)
            {
                return "Invalid unit or unit rate.";
            }

            var dr = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

            if (dr != null)
            {
                dr["IngredientType"] = type;
                dr["IngredientCode"] = code;
                dr["BaseUnit"] = baseUnit;
                dr["Unit"] = unit;
                dr["UnitRate"] = unitRate;
                dr["Qty"] = GetValue(num_Qty);
                dr["BaseCost"] = GetValue(num_BaseCost);
                dr["SpoilRate"] = GetValue(num_SpoilRate);
                dr["SpoilCost"] = lbl_SpoilCost.Text;
                dr["NetCost"] = lbl_NetCost.Text;
                dr["TotalCost"] = lbl_TotalCost.Text;
            }

            _dtRcpDt.AcceptChanges();

            return "";
        }

        private decimal GetCostOfPortion()
        {
            var size = GetValue(se_PortionSize);
            var totalCost = GetValue(se_TotalCost) + GetValue(se_TotalMix);

            var unitCost = size == 0 ? 0 : RoundAmt(totalCost / size);

            return unitCost;
        }


        private void UpdateTotalCost(DateTime toDate)
        {
            foreach (DataRow dr in _dtRcpDt.Rows)
            {
                var code = dr["IngredientCode"].ToString();
                var type = dr["IngredientType"].ToString().ToUpper();

                var qty = GetDecimal(dr["Qty"].ToString());
                var unitRate = GetDecimal(dr["UnitRate"].ToString());
                var spoilRate = GetDecimal(dr["SpoilRate"].ToString());

                if (type == "P")  // Product
                {

                    var productCost = GetProductCost(code, toDate);
                    var cost  = productCost.Cost;
                    var total = GetTotalCost(qty, unitRate, cost);
                    var spoil = GetSpoilCost(total, spoilRate);
                    var net = total - spoil;

                    dr["BaseCost"] = cost;
                    dr["TotalCost"] = total;
                    dr["SpoilCost"] = spoil;
                    dr["NetCost"] = net;

                    if (productCost.CommittedDate == null)
                        dr["UpdatedDate"] = DBNull.Value;
                    else
                        dr["UpdatedDate"] = productCost.CommittedDate;
                }
            }

            BindDetails();

            // Calculate Header
            var totalCost = _dtRcpDt.AsEnumerable().Sum(x => x.Field<decimal>("TotalCost"));
            var totalMixRate = se_TotalMixRate.Number;
            var totalMix = RoundAmt(totalCost * RoundAmt(totalMixRate / 100));

            var netPrice = se_NetPrice.Number;
            var grossPrice = se_GrossPrice.Number;

            var netCost = GetValueByCostTotalMix(netPrice);
            var grossCost = GetValueByCostTotalMix(grossPrice);


            se_TotalCost.Number = totalCost;
            se_TotalMix.Number = totalMix;
            se_CostTotalMix.Number = totalCost + totalMix;

            se_NetPrice.Number = netPrice;
            se_GrossPrice.Number = grossPrice;

            se_NetCost.Number = netCost;
            se_GrossCost.Number = grossCost;


            var portionSize = GetValue(se_PortionSize);

            se_PortionCost.Value = portionSize == 0 ? 0 : RoundAmt(totalCost / portionSize);
        }


        private void UpdateCost(DateTime toDate)
        {
            foreach (DataRow dr in _dtRcpDt.Rows)
            {
                var code = dr["IngredientCode"].ToString();
                var type = dr["IngredientType"].ToString().ToUpper();

                var qty = GetDecimal(dr["Qty"].ToString());
                var unitRate = GetDecimal(dr["UnitRate"].ToString());
                var spoilRate = GetDecimal(dr["SpoilRate"].ToString());

                if (type == "P")  // Product
                {
                    var productCost = GetProductCost(code, toDate);

                    var cost = productCost.Cost;
                    var total = GetTotalCost(qty, unitRate, cost);
                    var spoil = GetSpoilCost(total, spoilRate);
                    var net = total - spoil;

                    dr["BaseCost"] = cost;
                    dr["TotalCost"] = total;
                    dr["SpoilCost"] = spoil;
                    dr["NetCost"] = net;

                    if (productCost.CommittedDate == null)
                        dr["UpdatedDate"] = DBNull.Value;
                    else
                        dr["UpdatedDate"] = productCost.CommittedDate;
                }
            }
            BindDetails();

            // Calculate Header
            se_TotalCost.Value = _dtRcpDt.AsEnumerable().Sum(x => x.Field<decimal>("TotalCost"));
            CalculateSummary();

            //var totalCost = _dtRcpDt.AsEnumerable().Sum(x => x.Field<decimal>("TotalCost"));
            //var totalMixRate = se_TotalMixRate.Number;
            //var totalMix = RoundAmt(totalCost * RoundAmt(totalMixRate / 100));

            //var netPrice = se_NetPrice.Number;
            //var grossPrice = se_GrossPrice.Number;

            //var netCost = GetValueByCostTotalMix(netPrice);
            //var grossCost = GetValueByCostTotalMix(grossPrice);


            //se_TotalCost.Number = totalCost;
            //se_TotalMix.Number = totalMix;
            //se_CostTotalMix.Number = totalCost + totalMix;

            //se_NetPrice.Number = netPrice;
            //se_GrossPrice.Number = grossPrice;

            //se_NetCost.Number = netCost;
            //se_GrossCost.Number = grossCost;


            //var portionSize = GetValue(se_PortionSize);

            //se_PortionCost.Value = portionSize == 0 ? 0 : RoundAmt(totalCost / portionSize);
        }

        private ProductCost GetProductCost(string productCode, DateTime toDate)
        {
            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var parameters = new SqlParameter[] { new SqlParameter("@Code", productCode), new SqlParameter("@ToDate", toDate) };
            var dt = sql.ExecuteQuery("SELECT TOP(1) ROUND(PriceOnLots/ [IN], APP.DigitAmt()) as Cost, CommittedDate FROM [IN].Inventory WHERE [Type]='RC' AND ProductCode=@code AND CAST(CommittedDate AS DATE) <= CAST(@toDate AS DATE) ORDER BY CommittedDate DESC", parameters);

            var cost = 0m;
            Nullable<DateTime> commitDate = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                cost = GetDecimal(dt.Rows[0]["Cost"].ToString());
                commitDate = Convert.ToDateTime(dt.Rows[0]["CommittedDate"]);

            }


            return new ProductCost
            {
                ProductCode = productCode,
                Cost = cost,
                CommittedDate = commitDate

            };
        }


        public class ProductCost
        {
            public string ProductCode { get; set; }
            public decimal Cost { get; set; }
            public Nullable<DateTime> CommittedDate { get; set; }
        }

    }
        #endregion
}