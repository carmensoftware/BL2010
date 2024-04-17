using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class LookupItem : DbHandler
    {
        # region "Attributies"

        #endregion

        #region "Operations"

        public LookupItem()
        {
            SelectCommand = "SELECT * FROM Application.LookupItem";
            TableName = "Application.LookupItem";
        }

        /// <summary>
        ///     Get all LookupItem data.
        /// </summary>
        /// <param name="dsLookupItem"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsLookupItem, string Connection)
        {
            return DbRetrieve("[Application].[GetLookupItemList]", dsLookupItem, null, TableName, Connection);
        }

        /// <summary>
        ///     Get LookupItem table structure.
        /// </summary>
        /// <param name="dsLookupItem"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public bool GetStruct(DataSet dsLookupItem, string Connection)
        {
            return DbRetrieveSchema("[Application].[GetLookupItemList]", dsLookupItem, null, TableName, Connection);
        }

        /// <summary>
        ///     Get lookup item that related to specification.
        /// </summary>
        /// <param name="dsLookupItem"></param>
        /// <param name="LookupID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsLookupItem, string LookupID, string ConnectionString)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LookupID", LookupID);

            return DbRetrieve("Application.GetLookupItemListLookupID", dsLookupItem, dbParams, TableName,
                ConnectionString);
        }

        /// <summary>
        ///     Get lookup item that related to specification.
        /// </summary>
        /// <param name="LookupID"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataTable GetList(string LookupID, string ConnectionString)
        {
            var dsLookupItem = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LookupID", LookupID);

            var retrieved = GetList(dsLookupItem, LookupID, ConnectionString);

            if (retrieved)
            {
                return dsLookupItem.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get display text value.
        /// </summary>
        /// <param name="LookupID"></param>
        /// <param name="Value"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string GetDisplayText(string LookupID, string Value, string ConnectionString)
        {
            var result = string.Empty;
            var dsLookupItem = new DataSet();

            // Get data.
            var retrieved = GetList(dsLookupItem, LookupID, ConnectionString);

            if (retrieved)
            {
                foreach (DataRow drLookupItem in dsLookupItem.Tables[TableName].Rows)
                {
                    if (drLookupItem["Value"].ToString() == Value)
                    {
                        result = drLookupItem["Text"].ToString();
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get new lookup item id.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public int GetNewID(string connStr)
        //{
        //    DataSet dsLookupItem = new DataSet();

        //    bool result = DbRetrieve("[Application].GetLookupItemNewID", dsLookupItem, null, this.TableName, connStr);

        //    if (result)
        //    {
        //        return int.Parse(dsLookupItem.Tables[this.TableName].Rows[0]["ID"].ToString());
        //    }

        //    return 0;
        //}

        #endregion
    }
}