using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTabControl;
using Newtonsoft.Json;

namespace BlueLedger.PL.IN.STK
{
    public partial class StkInEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.ADMIN.TransLog _transLog = new Blue.BL.ADMIN.TransLog();
        private readonly Blue.BL.APP.Module module = new Blue.BL.APP.Module();

        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.IN.StockIn stkIn = new Blue.BL.IN.StockIn();


        private string _MODE { get { return Request.QueryString["MODE"] == null ? "" : Request.QueryString["MODE"].ToString().ToUpper(); } }

        private string _ID { get { return Request.QueryString["ID"] == null ? "" : Request.QueryString["ID"].ToString().Trim(); } }

        private DataTable _dtStockIn
        {
            get { return ViewState["_dtStockIn"] as DataTable; }
            set { ViewState["_dtStockIn"] = value; }
        }

        private DataTable _dtStockInDt
        {
            get { return ViewState["_dtStockInDt"] as DataTable; }
            set { ViewState["_dtStockInDt"] = value; }
        }




        #endregion


        #region --Event(s)--
        protected void Page_Init(object sender, EventArgs e)
        {
            //hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
            hf_ConnStr.Value = LoginInfo.ConnStr;

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
            _dtStockIn = bu.DbExecuteQuery("SELECT * FROM [IN].StockIn WHERE RefId=@id", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);
            _dtStockInDt = bu.DbExecuteQuery("SELECT NEWID() as RowId, * FROM [IN].StockInDt WHERE Id=@id", new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", _ID) }, hf_ConnStr.Value);

            Page_Setting();
        }

        private void Page_Setting()
        {

            lbl_RefId.Text = _ID;
            de_DocDate.Date = DateTime.Today;

            if (_dtStockIn.Rows.Count > 0)
            {
                DataRow drStockIn = _dtStockIn.Rows[0];

                //var dateField = string.IsNullOrEmpty(drStockIn["CommitDate"].ToString()) ? "CreateDate" : "CommitDate";
                de_DocDate.Date = Convert.ToDateTime(drStockIn["CreateDate"]);
                ddl_Type.Value = drStockIn["Type"].ToString();
                lbl_Status.Text = drStockIn["Status"].ToString();
                txt_Desc.Text = drStockIn["Description"].ToString();
            }

            BindItems();


            // Edit
            if (_MODE != "NEW")
            {
                de_DocDate.Enabled = false;
            }

        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    SaveAndCommit(false);
                    break;
                case "COMMIT":
                    SaveAndCommit(true);
                    break;
                case "BACK":
                    if (Request.Params["MODE"].ToUpper() == "EDIT")
                    {
                        Response.Redirect("StkInDt.aspx?BuCode=" + Request.Params["BuCode"] +
                                          "&ID=" + Request.Params["ID"] +
                                          "&VID=" + Request.Params["VID"]);
                    }
                    else
                    {
                        Response.Redirect("StkInLst.aspx");
                    }
                    break;
            }
        }

        // Header
        protected void ddl_Type_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var dtType = GetAdjustType();

            ddl_Type.DataSource = dtType;
            ddl_Type.ValueField = "Value";
            ddl_Type.TextField = "Text";
            ddl_Type.DataBind();

        }

        // Detail
        protected void menu_Detail_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    CreateItem();
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

        protected void Img_Create_Click(object sender, ImageClickEventArgs e)
        {
            //Create();
        }

        protected void ddl_Location_Load(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;

            var sql = @"
;WITH
ul AS(
	SELECT
		LocationCode
	FROM
		[ADMIN].UserStore
	WHERE
		LoginName=@LoginName
)
SELECT
	l.LocationCode,
	l.LocationName
FROM
	[IN].StoreLocation l
	JOIN ul ON ul.LocationCode=l.LocationCode
WHERE
	l.IsActive=1
	AND EOP <> 2
ORDER BY
	l.LocationCode";
            var dt = bu.DbExecuteQuery(sql, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@LoginName", LoginInfo.LoginName) }, hf_ConnStr.Value);

            ddl.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new DevExpress.Web.ASPxEditors.ListEditItem
                {
                    Text = dr["LocationCode"].ToString() + " : " + dr["LocationName"].ToString(),
                    Value = dr["LocationCode"].ToString()
                });
            }

        }

        protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var ddl_Product = ddl.NamingContainer.FindControl("ddl_Product") as ASPxComboBox;
            var lbl_Unit = ddl.NamingContainer.FindControl("lbl_Unit") as Label;

            var locationCode = GetDropdownValue(ddl);
            var productCode = GetDropdownValue(ddl_Product);

            SetProducts(ddl_Product, locationCode, productCode);

            SetExpandInformation(ddl, locationCode, productCode, de_DocDate.Date);
        }

        protected void ddl_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as ASPxComboBox;
            var ddl_Location = ddl.NamingContainer.FindControl("ddl_Location") as ASPxComboBox;
            var lbl_Unit = ddl.NamingContainer.FindControl("lbl_Unit") as Label;

            var locationCode = GetDropdownValue(ddl_Location);
            var productCode = GetDropdownValue(ddl);

            lbl_Unit.Text = GetInventoryUnit(productCode);

            SetExpandInformation(ddl, locationCode, productCode, de_DocDate.Date);
        }

        protected void btn_ComfirmDelete_Click(object sender, EventArgs e)
        {
            var values = string.IsNullOrEmpty(hf_DeleteItems.Value) ? "[]" : hf_DeleteItems.Value;
            var items = JsonConvert.DeserializeObject<IEnumerable<string>>(values);

            if (items.Count() > 0)
            {
                DeleteItems(items);
            }
            pop_ConfirmDelete.ShowOnPageLoad = false;

        }

        // GrdiView

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

                if (e.Row.FindControl("lbl_Location") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Location") as Label;
                    var code = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                    var name = GetLocationName(code);

                    lbl.Text = string.Format("{0} : {1}", code, name);
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("ddl_Location") != null)
                {
                    var ddl = e.Row.FindControl("ddl_Location") as ASPxComboBox;

                    ddl.Value = DataBinder.Eval(e.Row.DataItem, "StoreId").ToString();
                }



                if (e.Row.FindControl("lbl_Product") != null)
                {
                    var lbl = e.Row.FindControl("lbl_Product") as Label;
                    var code = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

                    var drProduct = GetProduct(code);
                    var name1 = drProduct == null ? "" : drProduct["ProductDesc1"].ToString();
                    var name2 = drProduct == null ? "" : drProduct["ProductDesc2"].ToString();

                    lbl.Text = string.Format("{0} : {1} | {2}", code, name1, name2);
                    lbl.ToolTip = lbl.Text;
                }


                if (e.Row.FindControl("ddl_Product") != null)
                {
                    var ddl_Product = e.Row.FindControl("ddl_Product") as ASPxComboBox;
                    var ddl_Location = e.Row.FindControl("ddl_Location") as ASPxComboBox;

                    var locationCode = GetDropdownValue(ddl_Location);
                    var productCode = DataBinder.Eval(e.Row.DataItem, "SKU").ToString();

                    SetProducts(ddl_Product, locationCode, productCode);
                }

                if (e.Row.FindControl("lbl_Unit") != null)
                {
                    var lbl_Unit = e.Row.FindControl("lbl_Unit") as Label;

                    lbl_Unit.Text = DataBinder.Eval(e.Row.DataItem, "Unit").ToString();
                    lbl_Unit.ToolTip = lbl_Unit.Text;
                }

                if (e.Row.FindControl("num_Qty") != null)
                {
                    var num = e.Row.FindControl("num_Qty") as ASPxSpinEdit;
                    num.DecimalPlaces = DefaultQtyDigit;

                    num.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }


                if (e.Row.FindControl("num_Cost") != null)
                {
                    var num = e.Row.FindControl("num_Cost") as ASPxSpinEdit;

                    num.DecimalPlaces = DefaultAmtDigit;
                    num.Text = DataBinder.Eval(e.Row.DataItem, "UnitCost").ToString();
                }

                if (e.Row.FindControl("txt_Comment") != null)
                {
                    var text = e.Row.FindControl("txt_Comment") as TextBox;

                    text.Text = DataBinder.Eval(e.Row.DataItem, "Comment").ToString();
                    text.ToolTip = text.Text;
                }


                // Expand Information --
                SetExpandInformation(
                    e.Row.FindControl("txt_Comment"),
                    DataBinder.Eval(e.Row.DataItem, "StoreId").ToString(),
                    DataBinder.Eval(e.Row.DataItem, "SKU").ToString(),
                    de_DocDate.Date);

            }
        }

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

                    var dr = _dtStockInDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

                    var locationCode = dr == null ? "" : dr["StoreId"].ToString();
                    var productCode = dr == null ? "" : dr["SKU"].ToString();


                    hf_DeleteItems.Value = JsonConvert.SerializeObject(items);

                    ShowConfirmDelete(string.Format("Location: {0}<br />Product: {1}<br />Do you want to delete this item?", locationCode, productCode));
                    break;
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
                var dr = _dtStockInDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

                dr.Delete();

                _dtStockInDt.AcceptChanges();

            }




            gv.EditIndex = -1;
            BindItems();
        }

        #endregion

        // Method(s)
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

        private string GetDropdownValue(ASPxComboBox ddl)
        {
            return ddl.Value == null ? "" : ddl.Value.ToString();
        }

        private DataTable GetAdjustType()
        {
            var sql = @"
SELECT
	AdjId as [Value],
	AdjName as [Text]
FROM
	[IN].AdjType
WHERE
	AdjType = 'Stock In'
	AND IsActived=1
ORDER BY
	[AdjName]";

            return bu.DbExecuteQuery(sql, null, hf_ConnStr.Value);
        }

        private void SetProducts(ASPxComboBox ddl_Product, string locationCode, string value)
        {
            value = string.IsNullOrEmpty(value) ? "" : value;
            var sql = @"
SELECT
	p.ProductCode as [Value],
	CONCAT(p.ProductCode, ' : ',p.ProductDesc1, ' | ', ISNULL(p.ProductDesc2,'')) as [Text]
    
FROM
	[IN].ProdLoc pl
	JOIN [IN].Product p
		ON p.ProductCode=pl.ProductCode
WHERE
	pl.LocationCode=@LocationCode
	AND p.IsActive=1
ORDER BY
	p.ProductCode";
            var dt = bu.DbExecuteQuery(sql, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@LocationCode", locationCode) }, hf_ConnStr.Value);
            var isFound = false;

            ddl_Product.Items.Clear();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Value"].ToString() == value)
                    {
                        isFound = true;
                    }

                    ddl_Product.Items.Add(new DevExpress.Web.ASPxEditors.ListEditItem
                    {
                        Text = dr["Text"].ToString(),
                        Value = dr["Value"].ToString()
                    });
                }


                ddl_Product.Value = isFound ? value : null;
            }

        }

        private void SetExpandInformation(Control item, string locationCode, string productCode, DateTime toDate)
        {
            var row = item.NamingContainer;

            var ds = new DataSet();
            var isExist = prDt.GetStockSummary(ds, productCode, locationCode, toDate.ToString("yyyy-MM-dd"), hf_ConnStr.Value);

            var onhand = 0m;
            var onorder = 0m;
            var reorder = 0m;
            var restock = 0m;
            var lastPrice = 0m;
            var lastVendor = "";

            if (isExist)
            {
                var dr = ds.Tables[prDt.TableName].Rows[0];

                onhand = Convert.ToDecimal(dr["OnHand"]);
                onorder = Convert.ToDecimal(dr["OnOrder"]);
                reorder = Convert.ToDecimal(dr["Reorder"]);
                restock = Convert.ToDecimal(dr["Restock"]);
                lastPrice = Convert.ToDecimal(dr["LastPrice"]);
                lastVendor = dr["LastVendor"].ToString();
            }

            if (row.FindControl("lbl_OnHand") != null)
            {
                var lbl = row.FindControl("lbl_OnHand") as Label;

                lbl.Text = FormatQty(onhand);
                lbl.ToolTip = lbl.Text;
            }
            if (row.FindControl("lbl_OnOrder") != null)
            {
                var lbl = row.FindControl("lbl_OnOrder") as Label;

                lbl.Text = FormatQty(onorder);
                lbl.ToolTip = lbl.Text;
            }
            if (row.FindControl("lbl_ReOrder") != null)
            {
                var lbl = row.FindControl("lbl_ReOrder") as Label;

                lbl.Text = FormatQty(reorder);
                lbl.ToolTip = lbl.Text;
            }
            if (row.FindControl("lbl_ReStock") != null)
            {
                var lbl = row.FindControl("lbl_ReStock") as Label;

                lbl.Text = FormatQty(restock);
                lbl.ToolTip = lbl.Text;
            }
            if (row.FindControl("lbl_LastPrice") != null)
            {
                var lbl = row.FindControl("lbl_LastPrice") as Label;

                lbl.Text = FormatAmt(lastPrice);
                lbl.ToolTip = lbl.Text;
            }
            if (row.FindControl("lbl_LastVendor") != null)
            {
                var lbl = row.FindControl("lbl_LastVendor") as Label;

                lbl.Text = lastVendor;
                lbl.ToolTip = lbl.Text;
            }

        }

        private DataRow GetProduct(string productCode)
        {
            var dt = bu.DbExecuteQuery(
                "SELECT * FROM [IN].Product WHERE ProductCode=@ProductCode",
                new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@ProductCode", productCode) },
                hf_ConnStr.Value);

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private string GetInventoryUnit(string productCode)
        {
            var dt = bu.DbExecuteQuery(
                "SELECT InventoryUnit FROM [IN].Product WHERE ProductCode=@ProductCode",
                new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@ProductCode", productCode) },
                hf_ConnStr.Value);

            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }

        private string GetLocationName(string locationCode)
        {
            var dt = bu.DbExecuteQuery(
                "SELECT LocationName FROM [IN].StoreLocation WHERE LocationCode=@LocationCode",
                new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@LocationCode", locationCode) },
                hf_ConnStr.Value);

            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }

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

        #region --Detail--

        private void BindItems()
        {
            gv_Detail.DataSource = _dtStockInDt;
            gv_Detail.DataBind();
        }

        private void CreateItem()
        {
            if (HasPendingItem())
            {
                return;
            }

            var rowCount = _dtStockInDt.Rows.Count;
            var locationCode = _dtStockInDt.Rows.Count == 0 ? "" : _dtStockInDt.Rows[rowCount - 1]["StoreId"].ToString(); ;

            var dr = _dtStockInDt.NewRow();
            dr["RowId"] = Guid.NewGuid();
            dr["Id"] = lbl_RefId.Text.Trim();
            dr["RefId"] = 0;
            dr["StoreId"] = locationCode;
            dr["Qty"] = 0;
            dr["UnitCost"] = 0;

            _dtStockInDt.Rows.Add(dr);
            //_dtStockInDt.AcceptChanges();

            gv_Detail.EditIndex = rowCount;
            BindItems();
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
            var dr = _dtStockInDt.AsEnumerable().FirstOrDefault(x => x.Field<Guid>("RowId").ToString() == rowId);

            if (dr != null)
            {
                dr["StoreId"] = ddl_Location.Value.ToString();
                dr["SKU"] = ddl_Product.Value.ToString();
                dr["Unit"] = lbl_Unit.Text;
                dr["Qty"] = num_Qty.Number;
                dr["UnitCost"] = num_Cost.Number;
                dr["Comment"] = txt_Comment.Text.Trim();
            }

            _dtStockInDt.AcceptChanges();

            return "";
        }

        private void DeleteItems(IEnumerable<string> listRowId)
        {
            var items = listRowId.ToArray();

            foreach (DataRow dr in _dtStockInDt.Rows)
            {
                var rowId = dr["RowId"].ToString();

                if (items.Contains(rowId))
                {
                    dr.Delete();
                }
            }

            _dtStockInDt.AcceptChanges();

            BindItems();
        }

        private bool HasPendingItem()
        {
            foreach (DataRow dr in _dtStockInDt.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    ShowAlert("Please save/cancel the pending item.");

                    return true;
                }
            }

            return false;

        }

        #endregion


        private void SaveAndCommit(bool isCommit = false)
        {
            var startPeriodDate = period.GetLatestOpenStartDate(hf_ConnStr.Value);

            #region --Data Validation--
            // Pending save item

            if (HasPendingItem())
            {
                return;
            }


            // Header
            if (de_DocDate.Date < startPeriodDate)
            {
                ShowAlert("The date should be during the open period.");

                return;
            }

            if (ddl_Type.Value == null)
            {
                ShowAlert("Please select any type.");

                return;
            }

            if (txt_Desc.Text.Trim().Length > 255)
            {
                ShowAlert("Description is too long (only 255 characters).");

                return;

            }

            // Detail

            if (_dtStockInDt.Rows.Count == 0)
            {
                ShowAlert("No any items created.");

                return;
            }


            if (_dtStockInDt.AsEnumerable().FirstOrDefault(x => string.IsNullOrEmpty(x.Field<string>("StoreId"))) != null)
            {
                ShowAlert("The store/location is required. Please enter the store/location.");

                return;
            }

            if (_dtStockInDt.AsEnumerable().FirstOrDefault(x => string.IsNullOrEmpty(x.Field<string>("SKU"))) != null)
            {
                ShowAlert("The product item is required. Please enter the product item.");

                return;
            }


            if (_dtStockInDt.AsEnumerable().FirstOrDefault(x => x.Field<decimal>("Qty") <= 0m) != null)
            {
                ShowAlert("The quantity is required. Please enter the quantity.");

                return;
            }

            #endregion

            var queries = new StringBuilder();
            var parameters = new List<SqlParameter>();

            #region --Header--
            var refId = _ID;

            var header_query = @"UPDATE [IN].StockIn SET [Type]=@Type, [Description]=@Description, CommitDate = NULL, UpdateDate = @UpdateDate, UpdateBy = @UpdateBy WHERE RefId=@RefId";

            if (string.IsNullOrEmpty(_ID)) // Create 
            {
                refId = stkIn.GetNewID(de_DocDate.Date, hf_ConnStr.Value);
                header_query = @"INSERT INTO [IN].StockIn (RefId, [Type], [Status], [Description], CommitDate, CreateBy, CreateDate, UpdateBy, UpdateDate) 
                                 VALUES (@RefId, @Type, 'Saved', @Description, NULL, @UpdateBy, @DocDate, @UpdateBy, @UpdateDate)";
            }

            parameters.Add(new SqlParameter("@RefId", refId));
            parameters.Add(new SqlParameter("@Type", ddl_Type.Value.ToString()));
            parameters.Add(new SqlParameter("@Description", txt_Desc.Text.Trim()));
            parameters.Add(new SqlParameter("@DocDate", de_DocDate.Date));

            parameters.Add(new SqlParameter("@UpdateBy", LoginInfo.LoginName));
            parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));

            #endregion

            queries.AppendLine("BEGIN TRAN");
            queries.AppendLine(header_query);
            // Delete old items then insert
            queries.AppendLine("DELETE FROM [IN].StockInDt WHERE Id=@Id");
            queries.AppendLine("INSERT INTO [IN].StockInDt (Id, StoreId, SKU, Unit, Qty, UnitCost, Comment) VALUES ");


            #region --Details--

            var item_queries = new StringBuilder();

            for (int i = 0; i < _dtStockInDt.Rows.Count; i++)
            {
                var dr = _dtStockInDt.Rows[i];

                var storeId = dr["StoreId"].ToString();
                var sku = dr["SKU"].ToString();
                var unit = dr["Unit"].ToString();
                var qty = dr["Qty"].ToString();
                var unitCost = dr["UnitCost"].ToString();
                var comment = dr["Comment"];

                item_queries.AppendFormat("(@Id, N'{1}', N'{2}', N'{3}', {4}, {5}, @Comment{0}),", i, storeId, sku, unit, qty, unitCost);

                parameters.Add(new SqlParameter("@Comment" + i.ToString(), comment));

            }

            parameters.Add(new SqlParameter("@Id", refId));

            #endregion

            queries.AppendLine(item_queries.ToString().Trim().TrimEnd(','));
            queries.AppendLine("COMMIT TRAN");


            var sql = new Helpers.SQL(hf_ConnStr.Value);
            try
            {
                sql.ExecuteQuery(queries.ToString(), parameters.ToArray());
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message, queries.ToString());
                ShowAlert("Save is unsuccess. Please try to save again.");
                return;
            }


            var buCode = Request.Params["BuCode"];
            var vid = Request.Params["VID"];
            var _action = string.IsNullOrEmpty(_ID) ? "CREATE" : "MODIFY";


            if (isCommit)
            {

                var dtCommittedDate = bu.DbExecuteQuery(string.Format("SELECT [IN].GetCommittedDate('{0}', NULL)", de_DocDate.Date.ToString("yyyy-MM-dd")), null, hf_ConnStr.Value);

                var committedDate = DateTime.Now;

                if (dtCommittedDate != null && dtCommittedDate.Rows.Count > 0)
                {
                    committedDate = Convert.ToDateTime(dtCommittedDate.Rows[0][0]);
                }

                queries.Clear();
                queries.AppendLine("BEGIN TRAN");
                queries.AppendLine("DELETE FROM [IN].Inventory WHERE [Type]='SI' AND HdrNo=@HdrNo");
                queries.AppendLine("INSERT INTO [IN].Inventory (HdrNo, DtNo, InvNo, ProductCode, Location, [IN], [OUT], Amount, PriceOnLots, [Type], CommittedDate)");
                queries.AppendLine("SELECT Id, RefId, 1, SKU, StoreId, Qty, 0, UnitCost, ROUND(Qty*UnitCost, APP.DigitAmt()) as PriceOnLots, 'SI', @CommittedDate  FROM [IN].StockInDt WHERE Id=@HdrNo");

                parameters.Clear();

                queries.AppendLine("UPDATE [IN].StockIn SET [Status]='Committed', CommitDate=@CommittedDate, UpdateDate=GETDATE(), UpdateBy=@UpdateBy WHERE RefId=@HdrNo");
                queries.AppendLine("EXEC [IN].UpdateAverageCost @HdrNo");

                parameters.Add(new SqlParameter("@HdrNo", refId));
                parameters.Add(new SqlParameter("@CommittedDate", committedDate));
                parameters.Add(new SqlParameter("@UpdateBy", LoginInfo.LoginName));

                queries.AppendLine("COMMIT TRAN");

                try
                {
                    sql.ExecuteQuery(queries.ToString(), parameters.ToArray());
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.Message, "985");
                    ShowAlert("Save is unsuccess. Please try to save again.");
                    return;
                }


                _transLog.Save("IN", "STKIN", refId, "COMMIT", string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
            }
            else
            {
                _transLog.Save("IN", "STKIN", refId, _action, string.Empty, LoginInfo.LoginName, hf_ConnStr.Value);
            }

            Response.Redirect("StkInDt.aspx?BuCode=" + buCode + "&ID=" + refId + "&VID=" + vid);
        }


        private void ErrorLog(string text, string comment = "")
        {
            var tmp = System.IO.Path.GetTempPath();
            //var dir = Server.MapPath("~\\");
            var dir = System.IO.Path.Combine(tmp, Request.Url.Host, "Blueledgers", LoginInfo.BuFmtInfo.BuCode);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            var file = System.IO.Path.Combine(dir, "errors.txt");

            //System.IO.File.WriteAllText(file, "101");
            var error = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + @text + "\t" + comment + Environment.NewLine + Environment.NewLine;
            System.IO.File.AppendAllText(file, error);


        }

    }
}