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

namespace BlueLedger.PL.PT.RCP
{

    public partial class RecipeEdit : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        #region "Attributes"

        private string _BuCode { get { return Request.QueryString["BuCode"] == null ? "" : Request.QueryString["BuCode"].ToString(); } }
        private string _VID { get { return Request.QueryString["VID"] == null ? "" : Request.QueryString["VID"].ToString(); } }
        private string _MODE { get { return Request.QueryString["MODE"] == null ? "" : Request.QueryString["MODE"].ToString(); } }

        private string _ID { get { return Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString(); } }


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

        #endregion

        // -----------------------------------------------------------------------------------------------------------

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;

            hf_DefaultSvcRate.Value = config.GetValue("APP", "Default", "SvcRate", LoginInfo.ConnStr);
            hf_DefaultTaxRate.Value = config.GetValue("APP", "Default", "TaxRate", LoginInfo.ConnStr);

            hf_DefaultSvcRate.Value = string.IsNullOrEmpty(hf_DefaultSvcRate.Value) ? "0" : hf_DefaultSvcRate.Value;
            hf_DefaultTaxRate.Value = string.IsNullOrEmpty(hf_DefaultTaxRate.Value) ? "0" : hf_DefaultTaxRate.Value;

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

            Page_Setting();

        }

        private void Page_Setting()
        {
            // Header

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

                se_PrepTime.Text = string.IsNullOrEmpty(dr["PrepTime"].ToString()) ? "0" : dr["PrepTile"].ToString();
                se_TotalTime.Text = string.IsNullOrEmpty(dr["TotalTime"].ToString()) ? "0" : dr["TotalTime"].ToString();

                txt_Remark.Text = dr["Remark"].ToString();
                ddl_Status.SelectedValue = dr["IsActived"].ToString();

                var image = dr["RcpImage"].ToString();
                img_RcpImage.ImageUrl = image;
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
                    //var dbParams = new Blue.DAL.DbParameter[2];
                    //dbParams[0] = new Blue.DAL.DbParameter("@RcpCode", txt_RcpCode.Text);
                    //dbParams[1] = new Blue.DAL.DbParameter("@updatedBy", LoginInfo.LoginName);

                    //bool result = rcpDt.UpdateCostOfRecipe(dbParams, LoginInfo.ConnStr);

                    //if (result)
                    //{
                    //    Response.Redirect("RecipeEdit.aspx?BuCode=" + Request.Params["BuCode"] + "&MODE=EDIT&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
                    //}

                    break;

                case "SAVE":
                    //string message = checkHeaderRequired();
                    //if (message == string.Empty)
                    //{
                    //    pop_ConfirmSave.ShowOnPageLoad = true;
                    //}
                    //else
                    //{
                    //    lbl_Warning.Text = message;
                    //    pop_Warning.ShowOnPageLoad = true;
                    //}
                    break;

                case "BACK":
                    var url = string.IsNullOrEmpty(_ID)
                        ? "RecipeList.aspx"
                        : "RecipeDt.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"];

                    Response.Redirect(url);

                    break;
            }
        }

        // Header
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

        protected void se_PortionSize_ValueChanged(object sender, EventArgs e)
        {
            var se = sender as ASPxSpinEdit;
        }

        // ---------------------------------------------------------------------------------------

        protected void menu_CmdItem_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateItem();
                    break;

                case "DELETE":
                    DeleteItem();
                    break;
            }
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
        }

        // GrdiView
        protected void gv_Detail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var rowId = e.CommandArgument.ToString();
            var commandName = e.CommandName.ToUpper();

            switch (commandName)
            {
                case "CREATE":
                    CreateItem();

                    break;

                case "SAVE":
                case "SAVENEW":
                    var gv = sender as GridView;
                    var row = gv.Rows[gv.EditIndex];
                    var error = SaveItem(row);

                    if (!string.IsNullOrEmpty(error))
                    {
                        ShowAlert(error);
                        return;
                    }

                    gv_Detail.EditIndex = -1;
                    BindItems();

                    if (commandName == "SAVENEW")
                        CreateItem();

                    break;

                case "DEL":
                    var items = new string[] { rowId };

                    var dr = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

                    var locationCode = dr == null ? "" : dr["StoreId"].ToString();
                    var productCode = dr == null ? "" : dr["SKU"].ToString();


                    hf_DeleteItems.Value = JsonConvert.SerializeObject(items);

                    ShowConfirmDelete(string.Format("Location: {0}<br />Product: {1}<br />Do you want to delete this item?", locationCode, productCode));
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

                if (e.Row.FindControl("hf_RowId") != null)
                {
                    var item = e.Row.FindControl("hf_RowId") as HiddenField;

                    item.Value = DataBinder.Eval(e.Row.DataItem, "RowId").ToString();
                }

                if (e.Row.FindControl("lbl_ItemType") != null)
                {
                    var lbl = e.Row.FindControl("lbl_ItemType") as Label;
                    var value = DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString();

                    lbl.Text = value == "P" ? "Product" : "Recipe";
                    lbl.ToolTip = lbl.Text;
                }


                var itemType = DataBinder.Eval(e.Row.DataItem, "IngredientType").ToString();
                var itemCode = DataBinder.Eval(e.Row.DataItem, "IngredientCode").ToString();

                var names = GetitemName(itemType, itemCode);
                var name1 = names != null ? names["Name1"].ToString() : "";
                var name2 = names != null ? names["Name2"].ToString() : "";

                if (e.Row.FindControl("lbl_Item") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Item") as Label;

                    lbl.Text = string.Format("{0} : {1}", itemCode, name1);
                    lbl.ToolTip = lbl.Text;
                }

                if (e.Row.FindControl("lbl_ItemDesc2") != null)
                {
                    var lbl = e.Row.FindControl("lbl_ItemDesc2") as Label;

                    lbl.Text = name2;
                    lbl.ToolTip = lbl.Text;
                }

                //    if (e.Row.FindControl("lbl_Product") != null)
                //    {
                //        var lbl = e.Row.FindControl("lbl_Product") as Label;
                //        var code = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

                //        var drProduct = GetProduct(code);
                //        var name1 = drProduct == null ? "" : drProduct["ProductDesc1"].ToString();
                //        var name2 = drProduct == null ? "" : drProduct["ProductDesc2"].ToString();

                //        lbl.Text = string.Format("{0} : {1} | {2}", code, name1, name2);
                //        lbl.ToolTip = lbl.Text;
                //    }


                //    if (e.Row.FindControl("ddl_Product") != null)
                //    {
                //        var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;
                //        var ddl_Location = e.Row.FindControl("ddl_Location") as ASPxComboBox;

                //        var locationCode = GetDropdownValue(ddl_Location);
                //        var productCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

                //        SetProducts(ddl_Product, locationCode, productCode);
                //    }

                //    if (e.Row.FindControl("lbl_Unit") != null)
                //    {
                //        var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;

                //        lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                //        lbl_Unit.ToolTip = lbl_Unit.Text;
                //    }

                //    if (e.Row.FindControl("num_Qty") != null)
                //    {
                //        var num = e.Row.FindControl("num_Qty") as ASPxSpinEdit;
                //        num.DecimalPlaces = DefaultQtyDigit;

                //        num.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                //    }


                //    if (e.Row.FindControl("num_Cost") != null)
                //    {
                //        var num = e.Row.FindControl("num_Cost") as ASPxSpinEdit;

                //        num.DecimalPlaces = DefaultAmtDigit;
                //        num.Text = DataBinder.Eval(e.Row.DataItem, "UnitCost").ToString();
                //    }

                //    if (e.Row.FindControl("txt_Comment") != null)
                //    {
                //        var text = e.Row.FindControl("txt_Comment") as TextBox;

                //        text.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                //        text.ToolTip = text.Text;
                //    }


                // Expand Information --
                if (e.Row.FindControl("lbl_BaseUnit") != null)
                {
                    var lbl = e.Row.FindControl("lbl_BaseUnit") as Label;

                    var unit = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    var baseUnit = DataBinder.Eval(e.Row.DataItem, "BaseUnit").ToString();
                    var unitRate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "UnitRate"));

                    lbl.Text = string.Format("Base unit : {0} = {1} {2}", baseUnit, unitRate.ToString(string.Format("N{0}", 0)), unit);
                    lbl.ToolTip = lbl.Text;
                }
            }
        }

        protected void gv_Detail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;

            gv.EditIndex = e.NewEditIndex;
            BindItems();
        }

        protected void gv_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;
            var row = gv.Rows[gv.EditIndex];

            var ddl_Product = row.FindControl("ddl_Product") as ASPxComboBox;
            var hf_RowId = row.FindControl("hf_RowId") as HiddenField;

            if (ddl_Product.Value == null)
            {
                var rowId = hf_RowId.Value;
                var dr = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

                dr.Delete();

                _dtRcpDt.AcceptChanges();

            }




            gv.EditIndex = -1;
            BindItems();
        }


        protected void ddl_ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var ddl_Item = ddl.NamingContainer.FindControl("ddl_Item") as ASPxComboBox;

            var sql = new Helpers.SQL(hf_ConnStr.Value);
            var query = "";


            if (ddl.Value == "P") // Product
            {
                query = "SELECT ProductCode as [Value], CONCAT(ProductCode, ' : ', ProductDesc1, ' | ' , ProductDesc2) as [Text] FROM [IN].Product WHERE IsActive=1 ORDER BY ProductCode";
            }
            else
            {
                query = "SELECT RcpCode as [Value], CONCAT(RcpCode,' : ', RcpDesc1, ' | ' , RcpDesc2) as [Text] FROM PT.Rcp WHERE IsActived=1 ORDER BY RcpCode";
            }

            var dt = sql.ExecuteQuery(query);

            ddl_Item.Items.Clear();
            ddl_Item.Items.AddRange(dt.AsEnumerable().Select(x => new ListEditItem { Value = x.Field<string>("Value"), Text = x.Field<string>("Text") }).ToArray());

        }


        protected void ddl_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var ddl_ItemType = ddl.NamingContainer.FindControl("ddl_ItemType") as ASPxComboBox;
            var lbl_Unit = ddl.NamingContainer.FindControl("lbl_Unit") as Label;

            //var locationCode = GetDropdownValue(ddl);
            //var productCode = GetDropdownValue(ddl_Product);

            //SetProducts(ddl_Product, locationCode, productCode);

            //SetExpandInformation(ddl, locationCode, productCode, de_DocDate.Date);
        }


        #endregion

        #region -- Method(s) --
        private void ShowAlert(string text)
        {
            lbl_Warning.Text = text.Trim();
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


        private void CreateItem()
        {
        }

        private void DeleteItem()
        {
        }

        private void BindItems()
        {
            gv_Detail.DataSource = _dtRcpDt;
            gv_Detail.DataBind();
        }

        private string SaveItem(GridViewRow row)
        {
            var hf_RowId = row.FindControl("hf_RowId") as HiddenField;
            var ddl_Location = row.FindControl("ddl_Location") as ASPxComboBox;
            var ddl_Product = row.FindControl("ddl_Product") as ASPxComboBox;
            var lbl_Unit = row.FindControl("lbl_Unit") as Label;
            var num_Qty = row.FindControl("num_Qty") as ASPxSpinEdit;
            var num_Cost = row.FindControl("num_Cost") as ASPxSpinEdit;
            var txt_Comment = row.FindControl("txt_Comment") as TextBox;

            #region -- Check required values --

            if (ddl_Location.Value == null)
            {
                return "Location is required.";
            }

            if (ddl_Product.Value == null)
            {
                return "Product is required.";
            }

            if (string.IsNullOrEmpty(num_Qty.Text) || num_Qty.Number == 0m)
            {
                return "Quantity is required.";
            }

            //if (string.IsNullOrEmpty(num_Cost.Text) || num_Cost.Number == 0m)
            //{
            //    ShowAlert("Quantity is required.");
            //    return;
            //}
            #endregion

            var rowId = hf_RowId.Value;
            var dr = _dtRcpDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

            if (dr != null)
            {
                dr["StoreId"] = ddl_Location.Value.ToString();
                dr["SKU"] = ddl_Product.Value.ToString();
                dr["Unit"] = lbl_Unit.Text;
                dr["Qty"] = num_Qty.Number;
                dr["UnitCost"] = num_Cost.Number;
                dr["Comment"] = txt_Comment.Text.Trim();
            }

            _dtRcpDt.AcceptChanges();

            return "";
        }
        #endregion
    }
}