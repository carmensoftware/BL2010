using System.Data;
//using System.Data.SqlClient;
using Blue.DAL;
using System;

namespace Blue.BL.Option.Inventory
{
    public class Product : DbHandler
    {
        public Product()
        {
            SelectCommand = "SELECT * FROM [In].[Product]";
            TableName = "Product";
        }

        /// <summary>
        /// </summary>
        /// <param name="SubCateCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string SubCateCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SubCateCode", SubCateCode);

            // Create parameters
            return DbRead("dbo.Product_GetList_SubCateCode", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetList(DataSet dsProduct, string connStr)
        {
            // Create parameters
            DbRetrieve("dbo.Product_GetList", dsProduct, null, TableName, connStr);

            return dsProduct;
        }

        public DataTable GetList(string ConnStr)
        {
            // Create parameters
            return DbRead("dbo.Product_GetList", null, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetProdList(string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            // Create parameters
            return DbRead("dbo.Product_GetList_ProductCode", dbParams, connStr);
        }

        public bool Get(DataSet dsProduct, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            return DbRetrieve("dbo.Product_GetList_ProductCode", dsProduct, dbParams, TableName, ConnStr);
        }

        public bool Get2(DataSet dsProduct, ref string MsgError, string ProductCode, string connStr)
        {
            var result = false;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProdCode", ProductCode);

            result = DbRetrieve("[IN].[GetProduct_ProdCode]", dsProduct, dbParams, TableName, connStr);

            return result;
        }


        /// <summary>
        ///     Get data by check active for lookup.
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetLookUp(DataSet dsProduct, string connStr)
        {
            // Create parameters
            DbRetrieve("dbo.IN_Product_GetActiveList", dsProduct, null, TableName, connStr);

            return dsProduct;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookUp_LocationCode(string LocateCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", LocateCode);

            // Create parameters
            var dtLookUp = DbRead("dbo.IN_Product_GetList_LocationCode", dbParams, connStr);

            //if (dtLookUp != null)
            //{                
            //    DataRow drBlank = dtLookUp.NewRow();
            //    dtLookUp.Rows.InsertAt(drBlank, 0);
            //}

            return dtLookUp;
        }

        public DataTable GetListByTwoLocation(string LocationCode1, string LocationCode2, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode1", LocationCode1);
            dbParams[1] = new DbParameter("@LocationCode2", LocationCode2);

            return DbRead("[IN].[GetProductInTwoLocation]", dbParams, connStr);
        }

        //public DataTable GetLookUp_LocationCodeCateCode(string LocateCode, string CategoryType, string connStr)
        //{
        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[2];
        //    dbParams[0] = new DbParameter("@LocateCode", LocateCode);
        //    dbParams[1] = new DbParameter("@CategoryType", CategoryType);

        //    // Create parameters
        //    DataTable dtLookUp = DbRead("[IN].[Product_GetListByProdCateType]", dbParams, connStr);

        //    return dtLookUp;
        //}

        /// <summary>
        ///     Get data list.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookUp(string connStr)
        {
            // Create parameters
            return DbRead("dbo.IN_Product_GetActiveList", null, connStr);
        }

        /// <summary>
        ///     Get data by check filter vendorcode for lookup.
        /// </summary>
        /// <param name="VendorCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookUp_ByVendorCode(string VendorCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@VendorCode", VendorCode);

            // Create parameters
            return DbRead("dbo.ProductGetProduct_ByVendorCode", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProduct, string ProductCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var result = DbRetrieve("dbo.Product_GetList_ProductCode", dsProduct, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public int GetApprovalLevel(string ProductCode, string connStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, connStr);

            if (result && dsProduct.Tables[TableName].Rows.Count > 0)
            {
                return int.Parse(dsProduct.Tables[TableName].Rows[0]["ApprovalLevel"].ToString());
            }
            return 0;
        }

        /// <summary>
        ///     Get description
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string ProductCode, string connStr)
        {
            var strName = string.Empty;
            var dsProd = new DataSet();

            GetList(dsProd, ProductCode, connStr);

            if (dsProd.Tables[TableName] != null)
            {
                if (dsProd.Tables[TableName].Rows.Count > 0)
                {
                    strName = dsProd.Tables[TableName].Rows[0]["ProductDesc1"].ToString();
                }
            }

            return strName;
        }

        /// <summary>
        ///     Get local description
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName2(string ProductCode, string connStr)
        {
            var strName = string.Empty;
            var dsProd = new DataSet();

            GetList(dsProd, ProductCode, connStr);

            if (dsProd.Tables[TableName] != null)
            {
                if (dsProd.Tables[TableName].Rows.Count > 0)
                {
                    strName = dsProd.Tables[TableName].Rows[0]["ProductDesc2"].ToString();
                }
            }

            return strName;
        }

        /// <summary>
        ///     Get Onorder.
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetProductOnOrder(string LocationCode, string ProductCode, string connStr)
        {
            // Create parameters
            var OnOrder = 0;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("dbo.IN_Product_GetOnOrder_ByLocaProd", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                OnOrder = int.Parse(drProduct.Rows[0]["OrdQty"].ToString()) -
                          int.Parse(drProduct.Rows[0]["RcvQty"].ToString());
            }

            return OnOrder;
        }

        /// <summary>
        ///     Get Onhand
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetProductOnHand(string LocationCode, string ProductCode, string connStr)
        {
            // Create parameters
            var OnHand = 0;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("IN_Product_GetOnHand_ByLocaProd", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                OnHand = int.Parse(drProduct.Rows[0]["IN"].ToString()) - int.Parse(drProduct.Rows[0]["OUT"].ToString());
            }

            return OnHand;
        }

        /// <summary>
        ///     Get all onorder
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetProductOnOrderAll(string ProductCode, string connStr)
        {
            // Create parameters
            var OnOrder = 0;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("dbo.IN_Product_GetAllOnOrder_ByProd", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                if (drProduct.Rows[0]["TotalOrdQty"].ToString() != string.Empty)
                {
                    OnOrder = int.Parse(drProduct.Rows[0]["OrdQty"].ToString()) -
                              int.Parse(drProduct.Rows[0]["RcvQty"].ToString());
                }
            }

            return OnOrder;
        }

        //public decimal GetProductOnHandAll(string ProductCode, string connStr)
        //{
        //    // Create parameters
        //    int OnHand = 0;
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@ProductCode", ProductCode);
        //    DataTable drProduct = DbRead("dbo.IN_Product_GetAllOnHand_ByProd", dbParams, connStr);
        //    if (drProduct.Rows.Count > 0)
        //    {
        //        if (drProduct.Rows[0]["TotalOnHand"].ToString() != string.Empty)
        //        {
        //            OnHand = int.Parse(drProduct.Rows[0]["IN"].ToString()) - int.Parse(drProduct.Rows[0]["OUT"].ToString());
        //        }
        //        else
        //        {
        //            OnHand = 0;
        //        }
        //    }
        //    return OnHand;
        //}
        /// <summary>
        ///     Get all onhand
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetProductReOrder(string LocationCode, string ProductCode, string connStr)
        {
            // Create parameters
            var ReOrder = 0;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("dbo.IN_Product_GetReOrderReStk_ByLocaProd", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                ReOrder = int.Parse(drProduct.Rows[0]["OrdReorderPoint"].ToString());
            }

            return ReOrder;
        }

        /// <summary>
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetProductReStock(string LocationCode, string ProductCode, string connStr)
        {
            // Create parameters
            var ReStock = 0;
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("dbo.IN_Product_GetReOrderReStk_ByLocaProd", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                ReStock = int.Parse(drProduct.Rows[0]["OrdRestockLevel"].ToString());
            }

            return ReStock;
        }

        /// <summary>
        ///     Get data of inventory qty converse to order qty
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetInvConvOrder(string ProductCode, string connStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, connStr);

            if (result)
            {
                if (dsProduct.Tables[TableName].Rows.Count > 0)
                {
                    if (dsProduct.Tables[TableName].Rows[0]["InventoryConvOrder"].ToString() != string.Empty)
                    {
                        return decimal.Parse(dsProduct.Tables[TableName].Rows[0]["InventoryConvOrder"].ToString());
                    }
                }
            }

            return 0;
        }

        /// <summary>
        ///     Get data of recipe qty converse to inventory qty
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetReciConvInven(string ProductCode, string connStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, connStr);

            if (result)
            {
                if (dsProduct.Tables[TableName].Rows.Count > 0)
                {
                    return decimal.Parse(dsProduct.Tables[TableName].Rows[0]["RecipeConvInvent"].ToString());
                }
            }

            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetProductName(string ProductCode, string connStr)
        {
            // Create parameters
            var productName = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var drProduct = DbRead("dbo.Product_GetList_ProductCode", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                productName = (drProduct.Rows[0]["ProductDesc1"].ToString() == string.Empty
                    ? drProduct.Rows[0]["ProductDesc2"].ToString()
                    : drProduct.Rows[0]["ProductDesc1"].ToString());
            }

            return productName;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetProductCategory(string ProductCode, string connStr)
        {
            // Create parameters
            var productCategory = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var dtProduct = DbRead("[IN].[GetProductCate]", dbParams, connStr);

            if (dtProduct.Rows.Count > 0)
            {
                productCategory = (dtProduct.Rows[0]["productCate"].ToString() == string.Empty
                    ? dtProduct.Rows[0]["productCate"].ToString()
                    : dtProduct.Rows[0]["productCate"].ToString());
            }

            return productCategory;
        }

        /// <summary>
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetGenerateProductCode(string categoryCode, string connStr)
        {
            // Create parameters
            var GenProductCode = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@categorycode", categoryCode);

            var drProduct = DbRead("[IN].[GetProductGenCode]", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                GenProductCode = (drProduct.Rows[0]["GenProductCode"].ToString());
            }

            return GenProductCode;
        }

        public string GetParentNoByCategoryCode(string categoryCode, string connStr)
        {
            // Create parameters
            var ParentNo = string.Empty;
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@categorycode", categoryCode);

            var drProduct = DbRead("dbo.ProdCat_GeParentNoByCategoryCode", dbParams, connStr);

            if (drProduct.Rows.Count > 0)
            {
                ParentNo = (drProduct.Rows[0]["ParentNo"].ToString());
            }

            return ParentNo;
        }

        /// <summary>
        ///     Get InvenUnit by ProductCode.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetInvenUnit(string ProductCode, string ConnStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, ConnStr);

            if (result)
            {
                if (dsProduct.Tables[TableName].Rows.Count > 0)
                {
                    return dsProduct.Tables[TableName].Rows[0]["InventoryUnit"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public string GetOrderUnit(string ProductCode, string ConnStr)
        {
            var dsProduct = new DataSet();

            var result = GetList(dsProduct, ProductCode, ConnStr);

            if (result)
            {
                if (dsProduct.Tables[TableName].Rows.Count > 0)
                {
                    return dsProduct.Tables[TableName].Rows[0]["OrderUnit"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsProduct, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("dbo.Product_GetSchema", dsProduct, null, TableName, connStr);

            // return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetTaxRate(string ProductCode, string connStr)
        {
            var dsProd = new DataSet();

            GetList(dsProd, ProductCode, connStr);

            if (dsProd.Tables[TableName] != null)
            {
                if (dsProd.Tables[TableName].Rows.Count > 0)
                {
                    return decimal.Parse(dsProd.Tables[TableName].Rows[0]["TaxRate"].ToString());
                }
            }

            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTaxType(string ProductCode, string connStr)
        {
            var dsProd = new DataSet();

            GetList(dsProd, ProductCode, connStr);

            if (dsProd.Tables[TableName] != null)
            {
                if (dsProd.Tables[TableName].Rows.Count > 0)
                {
                    return dsProd.Tables[TableName].Rows[0]["TaxType"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTaxAccCode(string ProductCode, string connStr)
        {
            var dsProd = new DataSet();

            GetList(dsProd, ProductCode, connStr);

            if (dsProd.Tables[TableName] != null)
            {
                if (dsProd.Tables[TableName].Rows.Count > 0)
                {
                    return dsProd.Tables[TableName].Rows[0]["TaxAccCode"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProduct, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProduct, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public bool SaveProductAndProdUnit(DataSet dsProduct, string connStr)
        {
            var prodUnit = new Blue.BL.IN.ProdUnit();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsProduct, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsProduct, prodUnit.SelectCommand, prodUnit.TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        public bool Delete(DataSet dsProduct, string connStr)
        {
            var prodUnit = new Blue.BL.IN.ProdUnit();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsProduct, prodUnit.SelectCommand, prodUnit.TableName);
            dbSaveSource[1] = new DbSaveSource(dsProduct, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        ///// <summary>
        ///// Save data in table prodbu and product.
        ///// </summary>
        ///// <param name="dsProduct"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public bool Save2(DataSet dsProduct, string connStr)
        //{
        //    BL.IN.ProdBu prodBU = new BL.IN.ProdBu();

        //    DbSaveSource[] dbSaveSource = new DbSaveSource[2];

        //    dbSaveSource[0] = new DbSaveSource(dsProduct, this.SelectCommand, this.TableName);
        //    dbSaveSource[1] = new DbSaveSource(dsProduct, prodBU.SelectCommand, prodBU.TableName);

        //    return DbCommit(dbSaveSource, connStr);
        //}      


        public DataTable GetCategoryListLookup(string connStr)
        {
            var dsCategory = new DataSet();

            // Get Data
            DbRetrieve("dbo.Product_GetCategoryListLookup", dsCategory, null, TableName, connStr);


            // Return result
            if (dsCategory.Tables[TableName] != null)
            {
                var drBlank = dsCategory.Tables[TableName].NewRow();
                dsCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsCategory.Tables[TableName];
        }


        public DataTable GetSubCategoryListLookup(string connStr)
        {
            var dsSubCategory = new DataSet();

            // Get Data
            DbRetrieve("dbo.Product_GetSubCategoryListLookup", dsSubCategory, null, TableName, connStr);


            // Return result
            if (dsSubCategory.Tables[TableName] != null)
            {
                var drBlank = dsSubCategory.Tables[TableName].NewRow();
                dsSubCategory.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsSubCategory.Tables[TableName];
        }


        public DataTable GetItemGroupListLookup(string connStr)
        {
            var dsItemGroup = new DataSet();

            // Get Data
            DbRetrieve("dbo.Product_GetItemGroupListLookup", dsItemGroup, null, TableName, connStr);


            // Return result
            if (dsItemGroup.Tables[TableName] != null)
            {
                var drBlank = dsItemGroup.Tables[TableName].NewRow();
                dsItemGroup.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsItemGroup.Tables[TableName];
        }


        public DataTable GetItemGroupListLookupByParentNo(string parentNO, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentNo", parentNO);


            var dsItemGroup = new DataSet();

            // Get Data
            DbRetrieve("dbo.Product_GetItemGroupListLookupByParentNo", dsItemGroup, dbParams, TableName, connStr);


            // Return result
            if (dsItemGroup.Tables[TableName] != null)
            {
                var drBlank = dsItemGroup.Tables[TableName].NewRow();
                dsItemGroup.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsItemGroup.Tables[TableName];
        }

        public DataTable GetProductDescList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.IN_Product_GetProductDescList", null, connStr);
        }

        public bool GetProductExport(DataSet dsProductExp, string Category, string SubCategory, string ItemGroup,
            string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Category", Category);
            dbParams[1] = new DbParameter("@SubCategory", SubCategory);
            dbParams[2] = new DbParameter("@ItemGroup", ItemGroup);

            return DbRetrieve("[IN].[GetProductExport]", dsProductExp, dbParams, TableName, ConnStr);
        }

        //public DataTable GetProductExport(string ConnStr)
        //{
        //    return DbRead("[IN].[GetProductExport]", null, ConnStr);
        //}

        /// <summary>
        ///     Get avalible product in HQ by BuCode.
        /// </summary>
        /// <param name="BuCode"></param>
        /// <param name="HQConnStr"></param>
        /// <returns></returns>
        public bool GetListFromHQ(DataSet dsProduct, string BuCode, string HQConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("[IN].[GetProductListHQ]", dsProduct, dbParams, TableName, HQConnStr);
        }

        public DataColumn[] GetPK(DataSet dsPr)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsPr.Tables[TableName].Columns["ProductCode"];

            return primaryKeys;
        }

        public DataTable GetListByProductFromHQ(DataSet dsProduct, string BuCode, string HQConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            var result = DbRetrieve("[IN].[GetProductListFromHQ]", dsProduct, dbParams, TableName, HQConnStr);

            if (result)
            {
                dsProduct.Tables[TableName].PrimaryKey = GetPK(dsProduct);
            }

            return dsProduct.Tables[TableName];
        }

        public DataTable GetActiveList(string ConnStr)
        {
            return DbRead("[IN].GetProductListActive", null, ConnStr);
        }

        public bool GetActiveList(DataSet dsProduct, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            return DbRetrieve("[IN].GetProductListActiveByLocationCode", dsProduct, dbParams, TableName, ConnStr);
        }

        public DataTable GetActiveListForTreeView(string ConnStr)
        {
            return DbRead("[IN].[GetProductActiveListForTreeView]", null, ConnStr);
        }

        public DataTable GetProductListForTreeView(string LocationCode, string connStr)
        {
            //Create Parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            return DbRead("[IN].[GetProductListForTreeViewByLocationCode]", dbParams, connStr);
        }

        public DataTable GetProductListForTreeView(string LocationCode, int localDesc, string connStr)
        {
            //Create Parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@localDesc", localDesc.ToString());

            return DbRead("[IN].[GetProductListForTreeViewByLocationCode]", dbParams, connStr);
        }

        public DataTable GetLookUp_ByLocationCodeCateType(string CategoryType, string LocationCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@CategoryType", CategoryType);
            dbParams[1] = new DbParameter("@LocationCode", LocationCode);

            // Create parameters
            var dtLookUp = DbRead("[IN].[Product_GetByLocationCodeCategoryType]", dbParams, connStr);

            return dtLookUp;
        }

        public bool GetListRestock(DataSet dsProduct, string LocationCode, string CategoryCode, int IsAll,
            string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCate", CategoryCode);
            dbParams[2] = new DbParameter("@IsAll", IsAll.ToString());

            return DbRetrieve("[IN].GetProduct_Location_Category", dsProduct, dbParams, TableName, ConnStr);
        }

        public bool GetLastPrice(DataSet dsProduct, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].[Product_GetLastPrice]", dsProduct, dbParams, TableName, ConnStr);
        }

        public decimal GetLastPrice(string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var dtRecDt = DbRead("[IN].[Product_GetLastPrice]", dbParams, ConnStr);

            if (dtRecDt == null)
            {
                return 0;
            }

            if (dtRecDt.Rows.Count == 0)
            {
                return 0;
            }

            return decimal.Parse(dtRecDt.Rows[0]["LastPrice"].ToString());
        }

        public decimal GetLastCost(string productCode, DateTime toDate, string connStr)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;

            APP.Config config = new APP.Config();
            string costSystem = config.GetCostSystem(connStr);
            if (costSystem == "AVCO")
            {
                sql = "SELECT TOP(1)	ISNULL(Amount, 0) as Value ";
                sql += " FROM [IN].Inventory ";
                sql += " WHERE ISNULL(Amount, 0) <> 0";
                sql += " AND ProductCode = @productCode";
                sql += " AND CAST(CommittedDate as DATE) <= CAST(@toDate AS DATE)";
                sql += " ORDER BY CommittedDate DESC";
            }
            else // Last Receiving/ StockIn
            {
                sql = "SELECT TOP(1) CAST(PriceOnLots/[IN] AS DECIMAL(18,2)) as Cost";
                sql += " FROM [IN].Inventory";
                sql += " WHERE [Type] IN ('RC','SI') AND ISNULL([IN],0) <> 0";
                sql += " AND ProductCode = @productCode";
                sql += " AND CAST(CommittedDate as DATE) <= CAST(@toDate AS DATE)";
                sql += " ORDER BY CommittedDate DESC";
            }

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@productCode", productCode);
            dbParams[1] = new DbParameter("@toDate", toDate.ToString("yyyy-MM-dd"));

            dt = DbExecuteQuery(sql, dbParams, connStr);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0][0]);
            }
            else
                return 0;
        }



        public bool IsAbleToInactiveProduct(string productCode, string connStr)
        {
            //SqlConnection conn = new SqlConnection(connStr);
            //conn.Open();

            //string sql = "EXEC [IN].IsAbleToInactiveProduct '" + productCode + "'";
            //SqlCommand cmd = new SqlCommand(sql, conn);
            //SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            //if (reader["RecordCount"].ToString() == "0")
            //    return false;
            //else
            //    return true;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", productCode);

            var dtRecDt = DbRead("[IN].IsAbleToInactiveProduct", dbParams, connStr);

            return bool.Parse(dtRecDt.Rows[0][0].ToString());

        }


    }
}