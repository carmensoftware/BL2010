using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdSync : BasePage
    {
        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private DataSet dsProduct = new DataSet();
        private DataSet dsProductHQ = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsProductHQ = (DataSet) Session["dsProductHQ"];
                dsProduct = (DataSet) Session["dsProduct"];
            }
        }

        private void Page_Retrieve()
        {
            // Get avaliable product from HQ
            var getProdHQ = product.GetListFromHQ(dsProductHQ, LoginInfo.BuInfo.BuCode, LoginInfo.HQConnStr);

            if (getProdHQ)
            {
                dsProductHQ.Tables[product.TableName].PrimaryKey = product.GetPK(dsProductHQ);
                Session["dsProductHQ"] = dsProductHQ;
            }
            else
            {
                return;
            }

            // Get current product in local properties
            product.GetList(dsProduct, LoginInfo.ConnStr);

            if (dsProduct != null)
            {
                dsProduct.Tables[product.TableName].PrimaryKey = product.GetPK(dsProduct);
                Session["dsProduct"] = dsProduct;
            }
            else
            {
                return;
            }

            Page_Setting();
        }

        private void Page_Setting()
        {
            grd_Product.DataSource = dsProductHQ.Tables[product.TableName];
            grd_Product.DataBind();
        }

        protected void grd_Product_Load(object sender, EventArgs e)
        {
            grd_Product.DataSource = dsProductHQ.Tables[product.TableName];
            grd_Product.DataBind();
        }

        protected void grd_Product_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_ProductCode") != null)
                {
                    var lbl_ProductCode = e.Row.FindControl("lbl_ProductCode") as Label;
                    lbl_ProductCode.Text = DataBinder.Eval(e.Row.DataItem, "ProductCode").ToString();
                }

                if (e.Row.FindControl("lbl_Desc") != null)
                {
                    var lbl_Desc = e.Row.FindControl("lbl_Desc") as Label;
                    lbl_Desc.Text = DataBinder.Eval(e.Row.DataItem, "ProductDesc1").ToString();
                }

                if (e.Row.FindControl("lbl_OthDesc") != null)
                {
                    var lbl_OthDesc = e.Row.FindControl("lbl_OthDesc") as Label;
                    lbl_OthDesc.Text = DataBinder.Eval(e.Row.DataItem, "ProductDesc2").ToString();
                }
            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            //List<object> columnValues = grd_Product.GetSelectedFieldValues("ProductCode");
            var columnValues = new List<object>();
            for (var i = 0; i < grd_Product.Rows.Count; i++)
            {
                var chk_Item = grd_Product.Rows[i].FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    var lbl_ProductCode = grd_Product.Rows[i].FindControl("lbl_ProductCode") as Label;
                    columnValues.Add(lbl_ProductCode.Text);
                }
            }

            foreach (string selProdCode in columnValues)
            {
                var drProdHQ = dsProductHQ.Tables[product.TableName].Rows.Find(selProdCode);
                var drProd = dsProduct.Tables[product.TableName].Rows.Find(selProdCode);

                if (drProd != null)
                {
                    // Selected product already exist, updating
                    drProd["ProductDesc1"] = drProdHQ["ProductDesc1"];
                    drProd["ProductDesc2"] = drProdHQ["ProductDesc2"];
                    drProd["ProductCate"] = drProdHQ["ProductCate"];
                    drProd["ProductSubCate"] = drProdHQ["ProductSubCate"];
                    drProd["BarCode"] = drProdHQ["BarCode"];
                    drProd["RecipeUnit"] = drProdHQ["RecipeUnit"];
                    drProd["RecipeConvInvent"] = drProdHQ["RecipeConvInvent"];
                    drProd["InventoryUnit"] = drProdHQ["InventoryUnit"];
                    drProd["InventoryConvOrder"] = drProdHQ["InventoryConvOrder"];
                    drProd["OrderUnit"] = drProdHQ["OrderUnit"];
                    drProd["TaxType"] = drProdHQ["TaxType"];
                    drProd["TaxRate"] = drProdHQ["TaxRate"];
                    drProd["StandardCost"] = drProdHQ["StandardCost"];
                    drProd["LastCost"] = drProdHQ["LastCost"];
                    drProd["QuantityDeviation"] = drProdHQ["QuantityDeviation"];
                    drProd["PriceDeviation"] = drProdHQ["PriceDeviation"];
                    drProd["ReqHQAppr"] = drProdHQ["ReqHQAppr"];
                    drProd["IsActive"] = drProdHQ["IsActive"];
                    drProd["AccountCode"] = drProdHQ["AccountCode"];
                    drProd["IsRecipe"] = drProdHQ["IsRecipe"];
                    drProd["SaleItem"] = drProdHQ["SaleItem"];
                    drProd["ApprovalLevel"] = drProdHQ["ApprovalLevel"];
                    drProd["TaxAccCode"] = drProdHQ["TaxAccCode"];
                    drProd["CreatedDate"] = drProdHQ["CreatedDate"];
                    drProd["CreatedBy"] = drProdHQ["CreatedBy"];
                    drProd["UpdatedDate"] = ServerDateTime;
                    drProd["UpdatedBy"] = LoginInfo.LoginName;
                }
                else
                {
                    // Selected product does not exist, inserting
                    var drNew = dsProduct.Tables[product.TableName].NewRow();
                    drNew["ProductCode"] = drProdHQ["ProductCode"];
                    drNew["ProductDesc1"] = drProdHQ["ProductDesc1"];
                    drNew["ProductDesc2"] = drProdHQ["ProductDesc2"];
                    drNew["ProductCate"] = drProdHQ["ProductCate"];
                    drNew["ProductSubCate"] = drProdHQ["ProductSubCate"];
                    drNew["BarCode"] = drProdHQ["BarCode"];
                    drNew["RecipeUnit"] = drProdHQ["RecipeUnit"];
                    drNew["RecipeConvInvent"] = drProdHQ["RecipeConvInvent"];
                    drNew["InventoryUnit"] = drProdHQ["InventoryUnit"];
                    drNew["InventoryConvOrder"] = drProdHQ["InventoryConvOrder"];
                    drNew["OrderUnit"] = drProdHQ["OrderUnit"];
                    drNew["TaxType"] = drProdHQ["TaxType"];
                    drNew["TaxRate"] = drProdHQ["TaxRate"];
                    drNew["StandardCost"] = drProdHQ["StandardCost"];
                    drNew["LastCost"] = drProdHQ["LastCost"];
                    drNew["QuantityDeviation"] = drProdHQ["QuantityDeviation"];
                    drNew["PriceDeviation"] = drProdHQ["PriceDeviation"];
                    drNew["ReqHQAppr"] = drProdHQ["ReqHQAppr"];
                    drNew["IsActive"] = drProdHQ["IsActive"];
                    drNew["AccountCode"] = drProdHQ["AccountCode"];
                    drNew["IsRecipe"] = drProdHQ["IsRecipe"];
                    drNew["SaleItem"] = drProdHQ["SaleItem"];
                    drNew["ApprovalLevel"] = drProdHQ["ApprovalLevel"];
                    drNew["TaxAccCode"] = drProdHQ["TaxAccCode"];
                    drNew["CreatedDate"] = ServerDateTime;
                    drNew["CreatedBy"] = LoginInfo.LoginName;
                    drNew["UpdatedDate"] = ServerDateTime;
                    drNew["UpdatedBy"] = LoginInfo.LoginName;
                    dsProduct.Tables[product.TableName].Rows.Add(drNew);
                }
            }

            // Save Change to database
            var saved = product.Save(dsProduct, LoginInfo.ConnStr);

            if (saved)
            {
                lbl_Message.Text = "Selected product updated successfull";
                pop_Message.ShowOnPageLoad = true;
            }
            else
            {
                lbl_Message.Text = "Can not updating selected product";
                pop_Message.ShowOnPageLoad = true;
            }
        }

        protected void btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProdList.aspx");
        }
    }
}