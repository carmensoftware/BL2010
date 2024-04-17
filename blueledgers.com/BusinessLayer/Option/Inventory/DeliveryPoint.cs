using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class DeliveryPoint : DbHandler
    {
        public DeliveryPoint()
        {
            SelectCommand = "SELECT * FROM [IN].DeliveryPoint";
            TableName = "DeliveryPoint";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDeliPoint"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDeliPoint, ref string MsgError, string connStr)
        {
            var result = DbRetrieve("dbo.DeliPoint_GetActiveList", dsDeliPoint, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get List for lookup
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.DeliPoint_GetActiveList", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsDeliPoint"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsDeliPoint, string connStr)
        {
            return DbRetrieve("dbo.IN_DeliPoint_GetList", dsDeliPoint, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var NewID = DbReadScalar("dbo.DeliPoint_GetNewID", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Name of Delivery Point
        /// </summary>
        /// <param name="dptCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string dptCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DptCode", dptCode);

            var drUnit = DbRead("dbo.DeliPoint_GetName", dbParams, connStr);

            if (drUnit.Rows.Count > 0)
            {
                strName = drUnit.Rows[0]["Name"].ToString();
            }

            return strName;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsDeliPoint = new DataSet();

            // Get Data
            GetList(dsDeliPoint, connStr);

            // Return result
            return dsDeliPoint.Tables[TableName];
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsDeliPoint, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsDeliPoint, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }
    }
}