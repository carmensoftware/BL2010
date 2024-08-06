using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;

namespace BlueLedger.PL.IN.PM
{
    public partial class PM : BasePage
    {
        // Business Layer Class
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.Option.Inventory.ProdCat category = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.IN.Inventory inventory = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private decimal Balance;

        private decimal CFQty;
        private decimal Cost;
        private decimal InQty;
        private decimal OutQty;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();
            }
        }

        private void Page_Setting()
        {
            // Get all initial data. --------------------------------------------------------------
            Page_Retrieve();

            // Display all initial data. ----------------------------------------------------------
            // Business Unit
            ddl_Bu.DataSource = bu.GetList(LoginInfo.BuInfo.BuGrpCode);
            ddl_Bu.DataTextField = "BuName";
            ddl_Bu.DataValueField = "BuCode";
            ddl_Bu.Enabled = LoginInfo.BuInfo.IsHQ;
            ddl_Bu.DataBind();
            ddl_Bu.SelectedValue = LoginInfo.BuInfo.BuCode;

            if (ddl_Bu.Items.Count == 0)
            {
                // Display Error Message : There is no Business Unit.
                return;
            }

            // Start Date and End Date
            txt_DateFrom.Text =
                DateTime.Parse(period.GetStartDate(ServerDateTime, bu.GetConnectionString(ddl_Bu.SelectedItem.Value))).ToString("dd/MM/yyyy");
            txt_DateTo.Text =
                DateTime.Parse(period.GetEndDate(ServerDateTime, bu.GetConnectionString(ddl_Bu.SelectedItem.Value))).ToString("dd/MM/yyyy");

            // Binding Product Category DropDownList
            Binding_Category();

            // Added on: 15/02/2018, By: Fon
            Binding_Sub_Category(ddl_Category.SelectedItem.Value);
            Binding_ItemGroup(ddl_SubCate.SelectedItem.Value);
            // End Added.

            // Binding Product DropdDownList
            Binding_Product();

        }

        private void Page_Retrieve()
        {
            // No code implement here.
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            // Print
        }

        protected void txt_DateTo_TextChanged(object sender, EventArgs e)
        {
            DateTime dateTo = Convert.ToDateTime((sender as TextBox).Text);
            DateTime dateFrom = new DateTime(dateTo.Year, dateTo.Month, 1);

            txt_DateFrom.Text = dateFrom.ToShortDateString();

            Binding_Product();
        }

        protected void ddl_Bu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Binding Product Category DropDownList
            Binding_Category();

            // Binding Product DropdDownList
            Binding_Product();
        }

        protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Added on: 15/02/2018, By: Fon, For: Following from P'Oat request.
            Binding_Sub_Category(ddl_Category.SelectedItem.Value);
            Binding_ItemGroup(ddl_SubCate.SelectedItem.Value);
            // End Added.

            // Binding Product DropdDownList
            Binding_Product();
        }

        protected void chk_ZeroValue_CheckedChanged(object sender, EventArgs e)
        {
            // Binding Product DropdDownList
            Binding_Product();
        }

        private void Binding_Category()
        {
            ddl_Category.DataSource = category.GetList(0, bu.GetConnectionString(ddl_Bu.SelectedItem.Value));
            ddl_Category.DataTextField = "CategoryName";
            ddl_Category.DataValueField = "CategoryCode";
            ddl_Category.DataBind();
        }

        private void Binding_Product()
        {
            if (txt_DateFrom.Text.Trim() == string.Empty)
            {
                return;
            }

            if (txt_DateTo.Text.Trim() == string.Empty)
            {
                return;
            }

            string dateFrom = txt_DateFrom.Text.Substring(6, 4) + "-" + txt_DateFrom.Text.Substring(3, 2) + "-" + txt_DateFrom.Text.Substring(0, 2);
            string dateTo = txt_DateTo.Text.Substring(6, 4) + "-" + txt_DateTo.Text.Substring(3, 2) + "-" + txt_DateTo.Text.Substring(0, 2);

            DataTable dt = GetProductList(dateFrom, dateTo, ddl_ItemGroup.SelectedItem.Value, chk_ZeroValue.Checked);

            ddl_Product.DataSource = dt;
            ddl_Product.DataTextField = "ProductDesc1";
            ddl_Product.DataValueField = "ProductCode";
            ddl_Product.DataBind();
            // End Modified.
        }

        protected void btn_Go_Click(object sender, EventArgs e)
        {
            var dsInventory = new DataSet();
            if (!String.IsNullOrEmpty(ddl_Product.Text))
            {
                // Report Header ----------------------------------------------------------------------
                lbl_ItemDesc.Text = ddl_Product.SelectedItem.Value + " : " + ddl_Product.SelectedItem.Text + " : " +
                                    product.GetName2(ddl_Product.SelectedItem.Value,
                                        bu.GetConnectionString(ddl_Bu.SelectedItem.Value));
            }
            else
            {
                lbl_ItemDesc.Text = "No Select Item";
            }
            lbl_BuName.Text = ddl_Bu.SelectedItem.Value + " : " + ddl_Bu.SelectedItem.Text;

            // Report Detail ----------------------------------------------------------------------
            // Display Store Location List on Repeater
            var dtStore = store.GetList(bu.GetConnectionString(ddl_Bu.SelectedItem.Value));
            if (dtStore != null)
            {
                rpt_Store.DataSource = dtStore;
                rpt_Store.DataBind();
            }
            else
            {
                // Display Error
            }
        }

        protected void rpt_Store_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dsProdMove = new DataSet();
                var storeCode = DataBinder.Eval(e.Item.DataItem, "LocationCode").ToString();
                var lbl_Store = e.Item.FindControl("lbl_Store") as Label;
                lbl_Store.Text = storeCode + " : " +
                                 store.GetName(storeCode, bu.GetConnectionString(ddl_Bu.SelectedItem.Value));

                // Bindding GridView to display Movement
                // Get Bwf data
                if (!string.IsNullOrEmpty(ddl_Product.Text))
                {
                    var getBwf = inventory.GetList(dsProdMove, DateTime.Parse(txt_DateFrom.Text.Substring(6, 4) + "-" +
                                                                              txt_DateFrom.Text.Substring(3, 2) + "-" +
                                                                              txt_DateFrom.Text.Substring(0, 2) +
                                                                              " 00:00:00",
                        new CultureInfo(LoginInfo.BuFmtInfo.LangCode, true)),
                        storeCode, ddl_Product.SelectedItem.Value, bu.GetConnectionString(ddl_Bu.SelectedItem.Value));

                    if (!getBwf)
                    {
                        // Display Error Message
                        return;
                    }

                    // Get Current data
                    var getCurrent = inventory.GetList(dsProdMove,
                        DateTime.Parse(
                            txt_DateFrom.Text.Substring(6, 4) + "-" + txt_DateFrom.Text.Substring(3, 2) + "-" +
                            txt_DateFrom.Text.Substring(0, 2) + " 00:00:00",
                            new CultureInfo(LoginInfo.BuFmtInfo.LangCode, true)),
                        DateTime.Parse(
                            txt_DateTo.Text.Substring(6, 4) + "-" + txt_DateTo.Text.Substring(3, 2) + "-" +
                            txt_DateTo.Text.Substring(0, 2) + " 23:59:59",
                            new CultureInfo(LoginInfo.BuFmtInfo.LangCode, true)),
                        storeCode, ddl_Product.SelectedItem.Value, bu.GetConnectionString(ddl_Bu.SelectedItem.Value));

                    if (getCurrent)
                    {
                        InQty = 0;
                        OutQty = 0;
                        Cost = 0;
                        CFQty = 0;
                        Balance = 0;
                    }
                    else
                    {
                        // Display Error Message
                        return;
                    }
                }

                // Display Data
                var grd_ProdMove = e.Item.FindControl("grd_ProdMove") as GridView;

                // GridView
                ((BoundField)grd_ProdMove.Columns[3]).DataFormatString = DefaultQtyFmt;
                ((BoundField)grd_ProdMove.Columns[4]).DataFormatString = DefaultQtyFmt;
                ((BoundField)grd_ProdMove.Columns[5]).DataFormatString = DefaultQtyFmt;
                //((BoundField)grd_ProdMove.Columns[6]).DataFormatString = DefaultQtyFmt;

                ((BoundField)grd_ProdMove.Columns[7]).DataFormatString = DefaultAmtFmt;
                ((BoundField)grd_ProdMove.Columns[8]).DataFormatString = DefaultAmtFmt;
                //((BoundField)grd_ProdMove.Columns[9]).DataFormatString = DefaultAmtFmt;

                grd_ProdMove.DataSource = dsProdMove.Tables[inventory.TableName];
                grd_ProdMove.DataBind();


                // Report Footer ----------------------------------------------------------------------
                var lbl_InQty = e.Item.FindControl("lbl_InQty") as Label;
                var lbl_OutQty = e.Item.FindControl("lbl_OutQty") as Label;
                var lbl_CFQty = e.Item.FindControl("lbl_CFQty") as Label;
                var lbl_Cost = e.Item.FindControl("lbl_Cost") as Label;
                var lbl_Total = e.Item.FindControl("lbl_Total") as Label;

                lbl_InQty.Text = string.Format(DefaultQtyFmt, InQty);
                lbl_OutQty.Text = string.Format(DefaultQtyFmt, OutQty);
                lbl_CFQty.Text = string.Format(DefaultQtyFmt, InQty - OutQty);
                lbl_Cost.Text = string.Format(DefaultAmtFmt, Cost);
                lbl_Total.Text = string.Format(DefaultAmtFmt, RoundAmt(CFQty * Cost));
            }
        }

        protected void grd_ProdMove_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InQty += decimal.Parse((DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString() != string.Empty
                    ? DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString()
                    : "0")) +
                         decimal.Parse((DataBinder.Eval(e.Row.DataItem, "In").ToString() != string.Empty
                             ? DataBinder.Eval(e.Row.DataItem, "In").ToString()
                             : "0"));

                OutQty += decimal.Parse((DataBinder.Eval(e.Row.DataItem, "Out").ToString() != string.Empty
                    ? DataBinder.Eval(e.Row.DataItem, "Out").ToString()
                    : "0"));

                Cost = decimal.Parse(decimal.Parse(DataBinder.Eval(e.Row.DataItem, "Cost").ToString()) != 0
                    ? DataBinder.Eval(e.Row.DataItem, "Cost").ToString()
                    : Cost.ToString("0.00"));

                CFQty = CFQty + decimal.Parse((DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString() != string.Empty
                    ? DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString()
                    : "0")) +
                        decimal.Parse((DataBinder.Eval(e.Row.DataItem, "In").ToString() != string.Empty
                            ? DataBinder.Eval(e.Row.DataItem, "In").ToString()
                            : "0")) -
                        decimal.Parse((DataBinder.Eval(e.Row.DataItem, "Out").ToString() != string.Empty
                            ? DataBinder.Eval(e.Row.DataItem, "Out").ToString()
                            : "0"));

                if (e.Row.FindControl("lbl_CFQty") != null)
                {
                    var lbl_CFQty = e.Row.FindControl("lbl_CFQty") as Label;
                    lbl_CFQty.Text = string.Format(DefaultQtyFmt, CFQty.ToString());
                }

                Balance = Balance +
                          ((decimal.Parse((DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString() != string.Empty
                              ? DataBinder.Eval(e.Row.DataItem, "BwfQty").ToString()
                              : "0")) +
                            decimal.Parse((DataBinder.Eval(e.Row.DataItem, "In").ToString() != string.Empty
                                ? DataBinder.Eval(e.Row.DataItem, "In").ToString()
                                : "0")) -
                            decimal.Parse((DataBinder.Eval(e.Row.DataItem, "Out").ToString() != string.Empty
                                ? DataBinder.Eval(e.Row.DataItem, "Out").ToString()
                                : "0"))) *
                           decimal.Parse((DataBinder.Eval(e.Row.DataItem, "Cost").ToString() != string.Empty
                               ? DataBinder.Eval(e.Row.DataItem, "Cost").ToString()
                               : "0")));

                if (e.Row.FindControl("lbl_Balance") != null)
                {
                    var lbl_Balance = e.Row.FindControl("lbl_Balance") as Label;
                    lbl_Balance.Text = string.Format(DefaultAmtFmt, Balance);
                }
            }
        }

        private DataTable GetList(string StartDate, string EndDate, string CategoryCode, bool IsIncludeZeroQty, string ConnStr)
        {
            var dbHandler = new Blue.DAL.DbHandler();
            var dbParams = new Blue.DAL.DbParameter[4];
            dbParams[0] = new Blue.DAL.DbParameter("@StartDate", StartDate);
            dbParams[1] = new Blue.DAL.DbParameter("@EndDate", EndDate);
            dbParams[2] = new Blue.DAL.DbParameter("@CategoryCode", CategoryCode);
            dbParams[3] = new Blue.DAL.DbParameter("@IsIncludeZeroQty", chk_ZeroValue.Checked.ToString());

            return dbHandler.DbRead("[IN].[GetProductList_CommittedDate_CategoryCode]", dbParams, ConnStr);
        }

        protected void ddl_SubCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Binding_ItemGroup(ddl_SubCate.SelectedItem.Value);
            Binding_Product();
        }

        protected void ddl_ItemGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Binding_Product();
        }

        private DataTable GetProductList(string startDate, string endDate, string itemGroup, bool isIncludeZero)
        {
            // Note: Following from old style but modified some filter.
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format(@"BEGIN
	            IF (@IsIncludeZeroQty = 'True')
		            BEGIN
			            SELECT ProductCode, ProductDesc1, ProductDesc2 FROM [IN].Product 
			            Where [ProductCate] = @itemGroup
				            AND IsActive = 'True'
		            END
	            ELSE
		            BEGIN
			            SELECT 
				            DISTINCT(inv.ProductCode), prod.ProductDesc1, prod.ProductDesc2
			            FROM 
				            [IN].Inventory inv 
				            LEFT OUTER JOIN [IN].Product prod ON (prod.ProductCode = inv.ProductCode)
			            WHERE 
				            (CAST(inv.CommittedDate AS DATE) BETWEEN @StartDate AND @EndDate)
				            AND prod.[ProductCate] = @itemGroup
				            AND prod.IsActive = 'True'
			            GROUP BY 
				            inv.ProductCode, prod.ProductDesc1, prod.ProductDesc2
		            END
            END");

            try
            {
                con.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlStr, con);
                sqlCmd.Parameters.AddWithValue("@StartDate", startDate);
                sqlCmd.Parameters.AddWithValue("@EndDate", endDate);
                sqlCmd.Parameters.AddWithValue("@itemGroup", itemGroup);
                sqlCmd.Parameters.AddWithValue("@IsIncludeZeroQty", isIncludeZero);

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }

        private void Binding_Sub_Category(string categoryCode)
        {
            DataSet ds = new DataSet();
            category.GetListByParentNo(ds, categoryCode, LoginInfo.ConnStr);
            ddl_SubCate.DataSource = ds.Tables[category.TableName];
            ddl_SubCate.DataTextField = "CategoryName";
            ddl_SubCate.DataValueField = "CategoryCode";
            ddl_SubCate.DataBind();
        }

        private void Binding_ItemGroup(string subCateCode)
        {
            DataSet ds = new DataSet();
            category.GetListByParentNo(ds, subCateCode, LoginInfo.ConnStr);
            ddl_ItemGroup.DataSource = ds.Tables[category.TableName];
            ddl_ItemGroup.DataTextField = "CategoryName";
            ddl_ItemGroup.DataValueField = "CategoryCode";
            ddl_ItemGroup.DataBind();
        }

    }
}