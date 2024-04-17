using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Admin.Interface
{
    public class AccountMapp : DbHandler
    {
        public AccountMapp()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[AccountMapp]";
            TableName = "AccountMapp";
        }

        /// <summary>
        ///     Gets all active AccountMapp data related to specified login name.
        /// </summary>
        /// <param name="dsAccountMapp"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountMapp, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.AccountMapp_GetList", dsAccountMapp, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsAccountMapp"></param>
        /// <param name="BusinessUnitCode"></param>
        /// <param name="LocationCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAccountMappingListByBusinessUnitAndStore(DataSet dsAccountMapp, string BusinessUnitCode,
            string LocationCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@businessunitcode", BusinessUnitCode);
            dbParams[1] = new DbParameter("@storecode", LocationCode);

            var result = DbRetrieve("[Admin].[GetAccountMappingListByBusinessUnitAndStore]", dsAccountMapp, dbParams,
                TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsAccountMapp"></param>
        /// <param name="BusinessUnitCode"></param>
        /// <param name="LocationCode"></param>
        /// <param name="ItemGroupCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAccountMappingListByBusinessUnitAndStoreAndItemGroup(DataSet dsAccountMapp,
            string BusinessUnitCode, string LocationCode, string ItemGroupCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@businessunitcode", BusinessUnitCode);
            dbParams[1] = new DbParameter("@storecode", LocationCode);
            dbParams[2] = new DbParameter("@ItemGroupCode", ItemGroupCode);

            var result = DbRetrieve("[Admin].[GetAccountMappingListByBusinessUnitAndStoreAndItemgroup]", dsAccountMapp,
                dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetList(DataSet dsAccountMapp, string LocationCode, string CategoryType, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@CategoryType", CategoryType);

            var result = DbRetrieve("[Admin].[GetAccountMappingListByStoreAndCategoryType]", dsAccountMapp, dbParams,
                TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     GetList
        ///     - Add by mai 20/08/2013 for after add new filed on table accountmapping [postType- for 'AP', 'GL' ]
        /// </summary>
        /// <param name="dsAccountMapp"></param>
        /// <param name="LocationCode"></param>
        /// <param name="CategoryType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountMapp, string LocationCode, string CategoryType, string connStr,
            PostType postType)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@CategoryType", CategoryType);
            dbParams[2] = new DbParameter("@postType", postType.ToString());

            var result = DbRetrieve("[Admin].[GetAccountMappingListByStoreAndCategoryType]", dsAccountMapp, dbParams,
                TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get Max AccCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var NewID = DbReadScalar("dbo.AccountMapp_GetNewID", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Lookup AccCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.AccountMapp_GetList", null, connStr);
        }


        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsProduct"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsAccountMapp, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("dbo.AccountMapp_GetSchema", dsAccountMapp, null, TableName, connStr);

            // return result
            return result;
        }


        /// <summary>
        /// </summary>
        /// <param name="BusinessUnitCode"></param>
        /// <param name="StoreCode"></param>
        /// <param name="ItemGroupCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetA3Code(string BusinessUnitCode, string StoreCode, string ItemGroupCode, string connStr)
        {
            var strA3Code = string.Empty;
            var dtAccountMapp = new DataTable();

            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@BusinessUnitCode", BusinessUnitCode);
            dbParams[1] = new DbParameter("@StoreCode", StoreCode);
            dbParams[2] = new DbParameter("@ItemGroupCode", ItemGroupCode);

            try
            {
                dtAccountMapp = DbRead("[Admin].[GetA3Code]", dbParams, connStr);

                if (dtAccountMapp != null)
                {
                    if (dtAccountMapp.Rows.Count > 0)
                    {
                        strA3Code = dtAccountMapp.Rows[0]["A3"].ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return strA3Code;
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountMapp, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsAccountMapp, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }

    public enum PostType
    {
        AP,
        GL
    }
}