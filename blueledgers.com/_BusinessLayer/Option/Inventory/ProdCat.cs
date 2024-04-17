using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class ProdGroup : DbHandler
    {
        /// <summary>
        ///     get ข้อมูล Product Group ที่ Active ทั้งหมด
        /// </summary>
        /// <param name="dsProdGroup"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProdGroup, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.ProdGroup_GetActiveList", dsProdGroup, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        public bool GetListByWF(DataSet dsProdGroup, string WF, ref string MsgError, string connStr)
        {
            //Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@WF", WF);

            var result = DbRetrieve("dbo.ProdGroup_GetActiveList_WF", dsProdGroup, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }
    }

    public class ProdCat : DbHandler
    {
        public ProdCat()
        {
            SelectCommand = "SELECT * FROM [In].[ProductCategory]";
            TableName = "ProductCategory";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProdCat"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsProdCat, string connStr)
        {
            return DbRetrieveSchema("[IN].[GetProductCategorySchema]", dsProdCat, null, TableName, connStr);
        }

        /// <summary>
        ///     Gets all active ProdCat data related to specified login name.
        /// </summary>
        /// <param name="dsProdCat"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProdCat, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.ProdCat_GetActiveList", dsProdCat, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get Max ProdCatCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewCategoryID(string parentNo, string levelNo, string connStr)
        {
            var dtGet = new DataTable();
            var SrNo = 0;
            var CatNo = 0;
            var CateCode = 0;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentNo", parentNo);

            dtGet = DbRead("dbo.ProdCat_GetActiveList_ParentNo", dbParams, connStr);

            if (levelNo == "1")
            {
                foreach (DataRow drGet in dtGet.Rows)
                {
                    SrNo = int.Parse(drGet["SrNo"].ToString());
                    CatNo = int.Parse(drGet["CategoryCode"].ToString());

                    if (SrNo - CatNo != 0)
                    {
                        CateCode = SrNo;
                        break;
                    }
                }

                if (CateCode == 0)
                {
                    CateCode = ProdCat_GeLevelOneNewCategoryCode(connStr);
                }
            }
            else if (levelNo == "2")
            {
                foreach (DataRow drGet in dtGet.Rows)
                {
                    SrNo = int.Parse(drGet["SrNo"].ToString()) - 1;
                    CatNo =
                        int.Parse(drGet["CategoryCode"].ToString()
                            .Substring(drGet["CategoryCode"].ToString().Length - 1, 1));

                    if (SrNo - CatNo != 0)
                    {
                        CateCode = int.Parse(parentNo + SrNo);
                        break;
                    }
                }

                if (CateCode == 0)
                {
                    CateCode = ProdCat_GeLevelTwoNewCategoryCode(int.Parse(parentNo), connStr);
                }
            }
            else if (levelNo == "3")
            {
                foreach (DataRow drGet in dtGet.Rows)
                {
                    SrNo = int.Parse(drGet["SrNo"].ToString()) - 1;
                    CatNo =
                        int.Parse(drGet["CategoryCode"].ToString()
                            .Substring(drGet["CategoryCode"].ToString().Length - 2, 2));

                    if (SrNo - CatNo != 0)
                    {
                        CateCode = int.Parse(parentNo + SrNo.ToString().PadLeft(2, '0'));
                        break;
                    }
                }

                if (CateCode == 0)
                {
                    CateCode = IN_ProductCategory_GetNewItemGroupCode(int.Parse(parentNo), connStr);
                }
            }

            return CateCode;

            //int NewID = DbReadScalar("dbo.ProdCat_GetNewID", null, connStr);

            //// Return result
            //return NewID;
        }

        /// <summary>
        ///     Get Lookup ProdCatCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.ProdCat_GetActiveList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetProdCateList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.ProdCat_GetList_GroupProdCate", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProdCate"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetProdCateList(DataSet dsProdCate, string connStr)
        {
            DbRetrieve("dbo.ProdCat_GetList", dsProdCate, null, TableName, connStr);

            return dsProdCate;
        }

        /// <summary>
        ///     Get Lookup ProdCatCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string LocaCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocaCode);

            // Create parameters
            return DbRead("dbo.ProdCat_GetList_LocatCode", dbParams, connStr);
        }

        /// <summary>
        ///     Get Name ProdCatCode.
        /// </summary>
        /// <param name="ProdCatCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string ProdCatCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", ProdCatCode);

            var drProdCat = DbRead("dbo.ProdCat_GetActiveList_CategoryCode", dbParams, connStr);
            if (drProdCat.Rows.Count > 0)
            {
                strName = drProdCat.Rows[0]["CategoryName"].ToString();
            }

            return strName;
        }

        public int GetCategoryType(string CategoryCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", CategoryCode);

            var drProdCat = DbRead("dbo.ProdCat_GetActiveList_CategoryCode", dbParams, ConnStr);

            if (drProdCat.Rows.Count > 0)
            {
                // Modified on: 27/03/2018, By: Fon
                //return int.Parse(drProdCat.Rows[0]["CategoryType"].ToString());

                if (drProdCat.Rows[0]["CategoryType"] != DBNull.Value)
                    return int.Parse(drProdCat.Rows[0]["CategoryType"].ToString());
                // End Modified.
            }

            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTaxAccCode(string ProductCode, string connStr)
        {
            var strTaxAcc = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            var dtProdCat = DbRead("dbo.ProdCat_GetTaxAccCode_ProductCode", dbParams, connStr);

            if (dtProdCat.Rows.Count > 0)
            {
                strTaxAcc = dtProdCat.Rows[0]["TaxAccCode"].ToString();
            }

            return strTaxAcc;
        }

        /// <summary>
        /// </summary>
        /// <param name="CateogryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByCategoryCode(DataSet dsProdCat, string CategoryCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", CategoryCode);

            var result = DbRetrieve("dbo.ProdCat_GetActiveList_CategoryCode", dsProdCat, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsProdCat"></param>
        /// <param name="ParentNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByParentNo(DataSet dsProdCat, string ParentNo, string connStr)
        {
            //Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentNo", ParentNo);

            var result = DbRetrieve("dbo.ProdCat_GetActiveList_ParentNo", dsProdCat, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get Max CategoryCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int ProdCat_GeLevelOneNewCategoryCode(string connStr)
        {
            var NewCode = DbReadScalar("dbo.ProdCat_GeLevelOneNewCategoryCode", null, connStr);

            // Return result
            return NewCode;
        }

        /// <summary>
        ///     Get Approvallevel.
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int ProdCat_GeApprovalLevelByCategoryCode(int categoryCode, string connStr)
        {
            return ProdCat_GeApprovalLevelByCategoryCode(Convert.ToString(categoryCode), connStr);
        }

        //override by op when product cate not equel integer
        public int ProdCat_GeApprovalLevelByCategoryCode(string categoryCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@categorycode", categoryCode);

            var drProdCat = DbRead("dbo.ProdCat_GeApprovalLevelByCategoryCode", dbParams, connStr);

            var NewCode = Convert.ToInt32(drProdCat.Rows[0]["approvalLevel"].ToString());

            return NewCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string ProdCat_GetTaxAccCodeByCategoryCode(int categoryCode, string connStr)
        {
            return ProdCat_GetTaxAccCodeByCategoryCode(categoryCode.ToString(), connStr);
        }

        public string ProdCat_GetTaxAccCodeByCategoryCode(string categoryCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@categorycode", categoryCode.ToString());

            var drProdCat = DbRead("dbo.ProdCat_GetTaxAccCodeByCategoryCode", dbParams, connStr);

            var TaxAccCode = (drProdCat.Rows[0]["TaxAccCode"].ToString());

            return TaxAccCode;
        }

        /// <summary>
        ///     Get Max CategoryCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int ProdCat_GeLevelTwoNewCategoryCode(int parentNO, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@parentNO", parentNO.ToString());

            var drProdCat = DbRead("[dbo].[ProdCat_GeLevelTwoNewCategoryCode]", dbParams, connStr);

            var NewCode = Convert.ToInt32(drProdCat.Rows[0]["NewCategoryCode"].ToString());

            return NewCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="parentNO"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int IN_ProductCategory_GetNewItemGroupCode(int SubCategoryCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@SubCategoryCode", SubCategoryCode.ToString());

            var drProdCat = DbRead("[dbo].[IN_ProductCategory_GetNewItemGroupCode]", dbParams, connStr);

            var NewCode = Convert.ToInt32(drProdCat.Rows[0]["NewItemGroupCode"].ToString());

            return NewCode;
        }

        public DataTable GetList(int ParentNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentNo", ParentNo.ToString());

            return DbRead("dbo.IN_ProdCat_GetActiveList_ParentNo", dbParams, ConnStr);
        }

        public DataTable GetListByParentNo(string ParentNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ParentNo", ParentNo.ToString());

            return DbRead("dbo.IN_ProdCat_GetActiveList_ParentNo", dbParams, ConnStr);
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsProdCat, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsProdCat, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public int GetCount(string connStr)
        {
            return DbReadScalar("[IN].GetCountProductCategory", null, connStr);
        }

        public string GetChild(string ProdCat, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProdCat", ProdCat);

            //DataTable dtName = DbRead("[dbo].[ProdCat_GetNumberofChild]", dbParams, connStr);
            //string strName = dtName.Rows[0][""].ToString();

            //return strName;

            return DbReadScalar("[dbo].[ProdCat_GetNumberofChild]", dbParams, connStr).ToString();
            //return DbReadScalar("dbo.ProdCat_GetNumberofChild", dbParams, connStr).ToString();

            //return DbRead("[dbo].[ProdCat_GetNumberofChild]",dbParams, connStr).ToString();
        }

        /// <summary>
        ///     Get SubCategory and ItemGroup List
        /// </summary>
        /// <param name="dsProdCat"></param>
        /// <param name="CategoryCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsProdCat, string CategoryCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CateCode", CategoryCode);

            // Create parameters
            return DbRetrieve("[IN].[GetSubCateAndItemGroupListByCateCode]", dsProdCat, dbParams, TableName, connStr);
        }

        public string GetCategoryCode(string itemGroupCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", itemGroupCode);

            var dtProductCategory = DbRead("[IN].GetProductCategoryByItemGroup", dbParams, connStr);

            if (dtProductCategory == null)
            {
                return string.Empty;
            }

            if (dtProductCategory.Rows.Count == 0)
            {
                return string.Empty;
            }

            return dtProductCategory.Rows[0]["ParentNo"].ToString();
        }

        public string GetSubCategoryCode(string itemGroupCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@CategoryCode", itemGroupCode);

            var dtProductCategory = DbRead("[IN].GetProductSubCategoryByItemGroup", dbParams, connStr);

            if (dtProductCategory == null)
            {
                return string.Empty;
            }

            if (dtProductCategory.Rows.Count == 0)
            {
                return string.Empty;
            }

            return dtProductCategory.Rows[0]["ParentNo"].ToString();
        }

        public bool IsExist(string CategoryCode, string ConnStr)
        {
            var dsProdCate = new DataSet();

            var result = GetListByCategoryCode(dsProdCate, CategoryCode, ConnStr);

            if (result)
            {
                if (dsProdCate.Tables[TableName].Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}