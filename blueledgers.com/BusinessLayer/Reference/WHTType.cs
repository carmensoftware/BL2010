using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class WhtType : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public WhtType()
        {
            SelectCommand = "SELECT * FROM Reference.WhtType";
            TableName = "WhtType";
        }

        /// <summary>
        /// </summary>
        /// <param name="WHTTypeCode"></param>
        /// <param name="dsWhtType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetWhtTypeList(string WHTTypeCode, DataSet dsWhtType, string connStr)
        {
            var result = false;
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@WhtTypeCode", WHTTypeCode);

            // Get data
            result = DbRetrieve("Reference.GetWhtTypeListByWhtTypeCode", dsWhtType, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetWhtTypeList(string connStr)
        {
            var dtAP = new DataTable();

            // Get data
            dtAP = DbRead("Reference.GetWhtTypeList", null, connStr);

            // Return result
            return dtAP;
        }


        /// <summary>
        /// </summary>
        /// <param name="WHTTypeCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetWhtTypeListByWhtTypeCode(string WHTTypeCode, string connStr)
        {
            var dtAP = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@WhtTypeCode", WHTTypeCode);

            // Get data
            dtAP = DbRead("Reference.GetWhtTypeListByWhtTypeCode", dbParams, connStr);

            // Return result
            return dtAP;
        }

        /// <summary>
        /// </summary>
        /// <param name="WhtTypeCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetWhtTypeName(string WhtTypeCode, string connStr)
        {
            string WHTTypeCodeName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@WHTTypeCode", WhtTypeCode);

            DbRetrieve("Reference.GetWhtTypeListByWhtTypeCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                WHTTypeCodeName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                WHTTypeCodeName = null;
            }
            return WHTTypeCodeName;
        }


        /// <summary>
        /// </summary>
        /// <param name="dsWhtType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetWhtTypeStructure(DataSet dsWhtType, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Reference.GetWhtTypeList", dsWhtType, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}