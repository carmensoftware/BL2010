using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Linq;

namespace BlueLedger.PL.IN.RE
{
    public partial class ReStock : BasePage
    {
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.Option.Inventory.ProdCat category = new Blue.BL.Option.Inventory.ProdCat();
        private readonly Blue.BL.ADMIN.Department dep = new Blue.BL.ADMIN.Department();

        private readonly Blue.BL.PC.PR.PR pr = new Blue.BL.PC.PR.PR();
        private readonly Blue.BL.PC.PR.PRDt prDt = new Blue.BL.PC.PR.PRDt();
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.StoreLct store = new Blue.BL.Option.Inventory.StoreLct();
        private readonly Blue.BL.APP.WF workFlow = new Blue.BL.APP.WF();

        private readonly Blue.BL.APP.WFDt wfDt = new Blue.BL.APP.WFDt();
        private readonly Blue.BL.APP.ViewHandler viewH = new Blue.BL.APP.ViewHandler();
        private DataSet dsProductRestock = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();
            }
            else
            {
                dsProductRestock = (DataSet)Session["dsProductRestock"];
            }
        }

        private void Page_Setting()
        {
            // Store
            ddl_Store.DataSource = store.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_Store.DataTextField = "LocationName";
            ddl_Store.DataValueField = "LocationCode";
            ddl_Store.DataBind();

            if (ddl_Store.Items.Count == 0)
            {
                // Display Error Message : There is no Store Location.
                return;
            }

            // Category
            //ddl_Category.DataSource = category.GetList(ddl_Store.SelectedItem.Value, LoginInfo.ConnStr);

            ddl_Category.DataSource = GetListCategoryByLocation(ddl_Store.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_Category.DataTextField = "CategoryName";
            ddl_Category.DataValueField = "CategoryCode";
            ddl_Category.DataBind();

            Session["dsProductRestock"] = dsProductRestock;
        }

        private DataTable GetListCategoryByLocation(string locationCode, string connStr)
        {
            string query =
               string.Format(
                    @";WITH
                    cat AS(
	                    SELECT
		                    CategoryCode AS Code,
		                    CategoryName AS [Name]
	                    FROM
		                    [IN].ProductCategory
	                    WHERE
		                    LevelNo = 1
                    ),
                    sub AS(
	                    SELECT
		                    CategoryCode AS Code,
		                    ParentNo AS ParentCode
	                    FROM
		                    [IN].ProductCategory
	                    WHERE
		                    LevelNo = 2
                    ),
                    itm AS(
	                    SELECT
		                    CategoryCode AS code,
		                    ParentNo AS ParentCode
	                    FROM
		                    [IN].ProductCategory
	                    WHERE
		                    LevelNo = 3
                    ),
                    category AS(
	                    SELECT
		                    itm.Code AS ItemGroupCode,
		                    cat.Code AS CategoryCode,
		                    cat.[Name] AS CategoryName
	                    FROM
		                    itm
		                    JOIN sub ON sub.Code = itm.ParentCode
		                    JOIN cat ON cat.Code = sub.ParentCode
	                    GROUP BY
		                    itm.Code,
		                    cat.Code,
		                    cat.[Name]
                    ),
                    prodcate AS(

	                    SELECT
		                    c.CategoryCode,
		                    c.CategoryName

	                    FROM
		                    [IN].ProdLoc pl
		                    JOIN [IN].Product p
			                    ON p.ProductCode = pl.ProductCode
		                    JOIN category c
			                    ON c.ItemGroupCode = p.ProductCate
	                    WHERE
		                    pl.LocationCode = '{0}'
                    )
                    SELECT
	                    DISTINCT CategoryCode,
	                    CategoryName

                    FROM
	                    prodcate

                    ORDER BY
	                    CategoryCode	", ddl_Store.SelectedItem.Value);

            return category.DbExecuteQuery(query, null, connStr);
        }

        /// <summary>
        ///     Refresh Category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddl_Category.DataSource = category.GetList(ddl_Store.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_Category.DataSource = GetListCategoryByLocation(ddl_Store.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_Category.DataTextField = "CategoryName";
            ddl_Category.DataValueField = "CategoryCode";
            ddl_Category.DataBind();
        }

        protected void btn_Go_Click(object sender, EventArgs e)
        {
            dsProductRestock.Clear();

            var getProductRestock = product.GetListRestock(dsProductRestock, ddl_Store.SelectedItem.Value,
                ddl_Category.SelectedItem.Value, int.Parse(rbl_Status.SelectedItem.Value), LoginInfo.ConnStr);

            if (getProductRestock)
            {
                grd_ProductRestock.DataSource = dsProductRestock.Tables[product.TableName];
                grd_ProductRestock.DataBind();

                Session["dsProductRestock"] = dsProductRestock;
            }
        }

       

        protected void chk_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var chk_SelectAll = sender as CheckBox;

            foreach (GridViewRow row in grd_ProductRestock.Rows)
            {
                var chk_Select = row.FindControl("chk_Select") as CheckBox;
                chk_Select.Checked = chk_SelectAll.Checked;
            }
        }

        protected void chk_Select_CheckedChanged(object sender, EventArgs e)
        {
            var chk_SelectAll = grd_ProductRestock.HeaderRow.FindControl("chk_SelectAll") as CheckBox;

            foreach (GridViewRow row in grd_ProductRestock.Rows)
            {
                var chk_Select = row.FindControl("chk_Select") as CheckBox;

                if (!chk_Select.Checked)
                {
                    chk_SelectAll.Checked = false;
                    return;
                }
            }

            chk_SelectAll.Checked = true;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "GENERATE PR":
                    try
                    {
                        string depCode = string.Empty;
                        var dt = pr.DbExecuteQuery("SELECT TOP 1 DepCode FROM [ADMIN].UserDepartment WHERE LoginName = '" + LoginInfo.LoginName + "'", null, LoginInfo.ConnStr);
                        if (dt.Rows.Count > 0)
                            depCode = dt.Rows[0][0].ToString();
                        else
                        {
                            alert_Text.Text = "User must be assigned Department before creating PR.";
                            pop_Alert.ShowOnPageLoad = true;
                            return;
                        }


                        bool hasSelected = false;
                        for (var i = 0; i < grd_ProductRestock.Rows.Count; i++)
                        {
                            var chk_Select = (CheckBox)grd_ProductRestock.Rows[i].FindControl("chk_Select");

                            if (chk_Select.Checked)
                            {
                                hasSelected = true;
                                break;
                            }
                        }

                        if (!hasSelected)
                        {
                            alert_Text.Text = "No item is selected";
                            pop_Alert.ShowOnPageLoad = true;
                            return;
                        }


                        if (grd_ProductRestock.Rows.Count > 0)
                        {
                            #region
                            // Get Pr, PrDt Table Schema
                            var dsPr = new DataSet();
                            var getPrSchema = pr.GetStructure(dsPr, LoginInfo.ConnStr);
                            var getPrDtSchema = prDt.GetStructure(dsPr, LoginInfo.ConnStr);

                            // Create PR Header
                            var drPr = dsPr.Tables[pr.TableName].NewRow();
                            drPr["PRNo"] = pr.GetNewID(ServerDateTime, LoginInfo.ConnStr);
                            drPr["PrDate"] = ServerDateTime;
                            drPr["Description"] = "Restock";
                            drPr["PrType"] = category.GetCategoryType(ddl_Category.SelectedItem.Value, LoginInfo.ConnStr);


                            if (workFlow.GetIsActive("PC", "PR", LoginInfo.ConnStr))
                            {
                                drPr["ApprStatus"] = workFlow.GetHdrApprStatus("PC", "PR", LoginInfo.ConnStr);
                            }

                            //drPr["HOD"] = dep.GetHeadOfDep(LoginInfo.DepartmentCode, LoginInfo.ConnStr);
                            //drPr["HOD"] = LoginInfo.DepartmentCode;
                            drPr["HOD"] = depCode;
                            drPr["DocStatus"] = "In Process";
                            drPr["CreatedDate"] = ServerDateTime;
                            drPr["CreatedBy"] = LoginInfo.LoginName;
                            drPr["UpdatedDate"] = ServerDateTime;
                            drPr["UpdatedBy"] = LoginInfo.LoginName;
                            dsPr.Tables[pr.TableName].Rows.Add(drPr);

                            // Create PR Detail
                            var prDtCount = 1;
                            var currencyCode = config.GetConfigValue("APP", "BU", "DefaultCurrency", LoginInfo.ConnStr); 

                            for (var i = 0; i < grd_ProductRestock.Rows.Count; i++)
                            {
                                var chk_Select = (CheckBox)grd_ProductRestock.Rows[i].FindControl("chk_Select");
                                var txt_ReStock = (TextBox)grd_ProductRestock.Rows[i].FindControl("txt_ReStock");

                                if (string.IsNullOrEmpty(txt_ReStock.Text))
                                {
                                    continue;
                                }
                                var restock = decimal.Parse(txt_ReStock.Text);

                                if (chk_Select.Checked && restock > 0)
                                {
                                    var drPrDt = dsPr.Tables[prDt.TableName].NewRow();

                                    var productCode = dsProductRestock.Tables[product.TableName].Rows[grd_ProductRestock.Rows[i].DataItemIndex]["ProductCode"].ToString();

                                    var item = dsProductRestock.Tables[product.TableName].AsEnumerable().FirstOrDefault(x => x.Field<string>("ProductCode") == productCode);

                                    var unitRate = item.Field<decimal>("UnitRate");
                                    var orderUnit = item.Field<string>("OrderUnit");
                                    var reqQty =  Math.Truncate(restock / unitRate);

                                    if (reqQty < reqQty * unitRate)
                                    {
                                        reqQty = reqQty + 1;
                                    }


                                    drPrDt["PRNo"] = drPr["PRNo"];
                                    drPrDt["PRDtNo"] = prDtCount;
                                    drPrDt["BuCode"] = LoginInfo.BuInfo.BuCode;
                                    drPrDt["LocationCode"] = dsProductRestock.Tables[product.TableName].Rows[grd_ProductRestock.Rows[i].DataItemIndex]["LocationCode"].ToString();
                                    drPrDt["ProductCode"] = productCode;
                                    drPrDt["OrderUnit"] = orderUnit;
                                    drPrDt["ReqQty"] = reqQty;
                                    drPrDt["HOD"] = LoginInfo.DepartmentCode;
                                    drPrDt["Reqdate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

                                    drPrDt["OrderQty"] = 0;
                                    drPrDt["ApprQty"] = 0;
                                    drPrDt["FOCQty"] = 0;
                                    drPrDt["RcvQty"] = 0;
                                    drPrDt["DiscPercent"] = 0;
                                    drPrDt["DiscAmt"] = 0;
                                    drPrDt["TaxAdj"] = false;
                                    drPrDt["TaxAmt"] = 0;
                                    drPrDt["NetAmt"] = 0;
                                    drPrDt["TotalAmt"] = 0;

                                    drPrDt["CurrDiscAmt"] = 0;
                                    drPrDt["CurrTaxAmt"] = 0;
                                    drPrDt["CurrNetAmt"] = 0;
                                    drPrDt["CurrTotalAmt"] = 0;

                                    drPrDt["CurrencyCode"] = currencyCode;
                                    drPrDt["CurrencyRate"] = 1;

                                    drPrDt["TaxType"] = product.GetTaxType(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                                    drPrDt["TaxRate"] = product.GetTaxRate(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                                    drPrDt["Price"] = 0;
                                    drPrDt["DeliPoint"] = store.GetDeliveryPoint(drPrDt["LocationCode"].ToString(), LoginInfo.ConnStr);
                                    drPrDt["Descen"] = product.GetName(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);
                                    drPrDt["Descll"] = product.GetName2(drPrDt["ProductCode"].ToString(), LoginInfo.ConnStr);

                                    if (workFlow.GetIsActive("PC", "PR", LoginInfo.ConnStr))
                                    {
                                        drPrDt["ApprStatus"] = workFlow.GetDtApprStatus("PC", "PR", LoginInfo.ConnStr);
                                    }

                                    dsPr.Tables[prDt.TableName].Rows.Add(drPrDt);

                                    prDtCount++;
                                }
                            }


                            Session["dsTemplate"] = dsPr;

                            // Redirec to PrEdit.aspx page
                            // Modified on: 29/01/2018, By: Fon
                            //Response.Cookies["[PC].[vPrList]"].Value = "6";
                            //Response.Redirect("~/PC/PR/PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode +
                            //                  "&MODE=template&VID=6&type=");

                            int stepCreate = -1;
                            DataTable dtWfDt = wfDt.GetList(1, LoginInfo.ConnStr);
                            foreach (DataRow dr in dtWfDt.Rows)
                            {
                                if (wfDt.GetAllowCreate(1, Convert.ToInt32(dr["Step"]), LoginInfo.ConnStr))
                                {
                                    stepCreate = Convert.ToInt32(dr["Step"]);
                                    break;
                                }
                            }

                            int viewValue = -1;
                            //DataTable dtView = viewH.GetList("[PC].[vPrList]", LoginInfo.LoginName, LoginInfo.ConnStr);
                            //foreach (DataRow dr in dtView.Rows)
                            //{
                            //    if (dr["WFId"] != DBNull.Value && dr["WfStep"] != DBNull.Value)
                            //    {
                            //        if (Convert.ToInt32(dr["WFId"]) == 1
                            //            && Convert.ToInt32(dr["WFStep"]) == stepCreate)
                            //        {
                            //            viewValue = Convert.ToInt32(dr["ViewNo"]);
                            //            break;
                            //        }
                            //    }
                            //}
                            var dtViewNo = pr.DbExecuteQuery("SELECT TOP 1 ViewNo FROM [APP].ViewHandler WHERE WfId=1 AND WFStep = 1 ", null, LoginInfo.ConnStr);
                            if (dtViewNo.Rows.Count > 0)
                                viewValue = Convert.ToInt32(dtViewNo.Rows[0][0]);


                            // Have right to create PR
                            if (viewValue > -1)
                            {
                                Response.Cookies["[PC].[vPrList]"].Value = string.Format("{0}", viewValue);
                                Response.Redirect("~/PC/PR/PrEdit.aspx?BuCode=" + LoginInfo.BuInfo.BuCode +
                                   "&MODE=template&VID=" + viewValue + "&type=");
                            }

                            // End Modified.
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        alert_Text.Text = ex.Message;
                        pop_Alert.ShowOnPageLoad = true;
                    }


                    break;
            }
        }
    }
}