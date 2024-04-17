using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class PeriodEnd : DbHandler
    {
        #region "Attibuties"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public PeriodEnd()
        {
            SelectCommand = "SELECT * FROM GL.PeriodEnd";
            TableName = "PeriodEnd";
        }


        public bool GetSchema(DataSet dsPeriodEnd, string strConn)
        {
            return DbRetrieveSchema("GL.GetPeriodEnd", dsPeriodEnd, null, TableName, strConn);
        }

        public DataSet GetByYearAndPeriodId(int year, int periodID, DataSet dsPeriodEnd, string connStr)
        {
            var dsPeriod = new DataSet();
            var dbParams = new DbParameter[2];

            // Generate parameter
            dbParams[0] = new DbParameter("@Year", year.ToString());
            dbParams[1] = new DbParameter("@PeriodID", periodID.ToString());

            // Get data
            DbRetrieve("GL.GetPeriod_Year_PeriodId", dsPeriodEnd, dbParams, TableName, connStr);

            // Return result
            return dsPeriod;
        }

        #endregion
    }
}