using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class StdJvType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public StdJvType()
        {
            SelectCommand = "SELECT * FROM GL.StdJvType";
            TableName = "StdJvType";
        }

        /// <summary>
        ///     Get all standard voucher type
        /// </summary>
        /// <returns></returns>
        public DataTable GetStdJvTypeList(string connStr)
        {
            var dtStdJvType = new DataTable();

            // Get data
            dtStdJvType = DbRead("GL.GetStdJvTypeList", null, connStr);

            // Return result
            return dtStdJvType;
        }


        /// <summary>
        ///     Get type of standard voucher
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public string GetStdJvTypeName(int typeID, string connStr)
        {
            var dsStdJvType = new DataSet();
            var result = string.Empty;

            // Get data
            GetStdJvType(typeID, dsStdJvType, connStr);

            // Return result
            if (dsStdJvType.Tables[TableName].Rows.Count > 0)
            {
                result = dsStdJvType.Tables[TableName].Rows[0]["Desc"].ToString();
            }

            return result;
        }


        /// <summary>
        ///     Get standard voucher type
        /// </summary>
        /// <param name="typeID"></param>
        public bool GetStdJvType(int typeID, DataSet dsStdJvType, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@TypeID", typeID.ToString());

            // Get data
            result = DbRetrieve("GL.GetStdJvType_TypeID", dsStdJvType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}