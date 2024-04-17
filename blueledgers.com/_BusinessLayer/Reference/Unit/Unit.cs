using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class Unit : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Unit()
        {
            SelectCommand = "SELECT * FROM Reference.Unit";
            TableName = "Unit";
        }

        /// <summary>
        ///     Get data by code.
        /// </summary>
        /// <param name="unitCode"></param>
        /// <param name="dsInvoice"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetUnitList(string unitCode, DataSet dsInvoice, string connStr)
        {
            var result = false;
            var dtUnit = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@UnitCode", unitCode);

            // Get data
            result = DbRetrieve("Reference.GetUnitListByUnitCode", dsInvoice, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get invoice detail using invoice id
        /// </summary>
        /// <param name="standardVoucherID"></param>
        /// <returns></returns>
        public DataTable GetUnitListByUnitCode(string unitCode, string connStr)
        {
            var dtUnit = new DataTable();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@UnitCode", unitCode);

            // Get data
            dtUnit = DbRead("Reference.GetUnitListByUnitCode", dbParams, connStr);

            // Return result
            return dtUnit;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetUnitListLookup(string connStr)
        {
            var dtUnit = new DataTable();

            // Get data
            dtUnit = DbRead("Reference.GetUnitList", null, connStr);

            // Return result
            return dtUnit;
        }

        /// <summary>
        /// </summary>
        /// <param name="unitCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetUnitName(string unitCode, string connStr)
        {
            string unitName;
            var dsTmp = new DataSet();
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@UnitCode", unitCode);

            DbRetrieve("Reference.GetUnitListByUnitCode", dsTmp, dbParams, TableName, connStr);

            if (dsTmp.Tables[TableName].Rows.Count > 0)
            {
                unitName = dsTmp.Tables[TableName].Rows[0]["Name"].ToString();
            }
            else
            {
                unitName = null;
            }
            return unitName;
        }

        /// <summary>
        ///     Get Unit
        /// </summary>
        /// <param name="dsUnit"></param>
        /// <returns></returns>
        public bool GetUnitStructure(DataSet dsUnit, string connStr)
        {
            var result = false;

            // Get structure
            result = DbRetrieveSchema("Reference.GetUnitList", dsUnit, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}