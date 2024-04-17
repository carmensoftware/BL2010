using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.APP
{
    public class LookupItem : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public LookupItem()
        {
            SelectCommand = "SELECT * FROM APP.LookupItem";
            TableName = "LookupItem";
        }


        /// <summary>
        ///     Get lookup item that related to specification.
        /// </summary>
        /// <param name="lookupID"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DataTable GetList(string lookupID, string connectionString)
        {
            var dsLookupItem = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LookupID", lookupID);

            var retrieved = GetList(dsLookupItem, lookupID, connectionString);

            if (retrieved)
            {
                return dsLookupItem.Tables[TableName];
            }

            return null;
        }


        /// <summary>
        ///     Get all actived lookup item related to specified lookup id.
        /// </summary>
        /// <param name="dsLookupItem"></param>
        /// <param name="strLookupID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsLookupItem, string strLookupID, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LookupID", strLookupID);

            return DbRetrieve("APP.GetLookupItemActivedList_LookupID", dsLookupItem, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get lookup item data related to specified lookup id and value.
        /// </summary>
        /// <param name="dsLookupItem"></param>
        /// <param name="strLookupID"></param>
        /// <param name="strValue"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsLookupItem, string strLookupID, string strValue, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LookupID", strLookupID);
            dbParams[1] = new DbParameter("@Value", strValue);

            return DbRetrieve("APP.GetLookupItem_LookupID_Value", dsLookupItem, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get text of lookup item related to specified lookup id and value
        /// </summary>
        /// <param name="strLookupID"></param>
        /// <param name="strValue"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetText(string strLookupID, string strValue, string strConn)
        {
            var dsLookupItem = new DataSet();

            var result = Get(dsLookupItem, strLookupID, strValue, strConn);

            if (result)
            {
                if (dsLookupItem.Tables[TableName].Rows.Count > 0)
                {
                    return dsLookupItem.Tables[TableName].Rows[0]["Text"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        #endregion
    }
}