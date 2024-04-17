using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class AccountMapp : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Instructor.
        /// </summary>
        public AccountMapp()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[AccountMapp]";
            TableName = "AccountMapp";
        }

        public int CountByLocationCode(string location, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", location);

            return DbReadScalar("dbo.ADMIN_AccountMapp_CountByLocationCode", dbParams, connStr);
        }

        public DataTable GetInterfaceExport(string keyWord, string keyWord2, string connStr)
        //public DataTable GetInterfaceExport(DateTime DateFrom, DateTime DateTo, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", keyWord);
            dbParams[1] = new DbParameter("@ToDate", keyWord2);

            return DbRead("ADMIN.GetInterfaceExp", dbParams, connStr);
        }


        public DataTable GetInterfaceExport(string fromDate, string toDate, string storeGrp, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@FromDate", fromDate);
            dbParams[1] = new DbParameter("@ToDate", toDate);
            dbParams[2] = new DbParameter("@StoreGrp", storeGrp);

            return DbRead("[PC].[GetExpToSUN]", dbParams, connStr);
        }

        public DataTable GetExportToCarmen(string fromDate, string toDate, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", fromDate);
            dbParams[1] = new DbParameter("@ToDate", toDate);
            //dbParams[2] = new DbParameter("@StoreGrp", storeGrp);

            return DbRead("[ADMIN].[GetExpToCarmen]", dbParams, connStr);
        }

        public DataTable GetPreviewExportToCarmen(string fromDate, string toDate, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", fromDate);
            dbParams[1] = new DbParameter("@ToDate", toDate);
            //dbParams[2] = new DbParameter("@StoreGrp", storeGrp);

            return DbRead("[ADMIN].[GetPreviewExpToCarmen]", dbParams, connStr);
        }

        public DataTable GetPreviewRestore(string fromDate, string toDate, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", fromDate);
            dbParams[1] = new DbParameter("@ToDate", toDate);
            //dbParams[2] = new DbParameter("@StoreGrp", storeGrp);

            return DbRead("[ADMIN].[GetPreviewRestore]", dbParams, connStr);
        }


        public DataTable GetInterfaceExportFalse(string keyWord, string keyWord2, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", keyWord);
            dbParams[1] = new DbParameter("@ToDate", keyWord2);


            return DbRead("ADMIN.GetInterfaceExpFalse", dbParams, connStr);
        }


        //GetInterfaceExpFalse
        public bool GetInterfaceRestore(DataSet dsRec, string keyWord, string keyWord2, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", keyWord);
            dbParams[1] = new DbParameter("@ToDate", keyWord2);

            var result = DbRetrieve("ADMIN.GetInterfaceExpFalse", dsRec, dbParams, TableName, connStr);

            return result;
        }

        public DataTable GetInterfaceExpNotMap(string keyWord, string keyWord2, string connStr)
            //public DataTable GetInterfaceExport(DateTime DateFrom, DateTime DateTo, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@FromDate", keyWord);
            dbParams[1] = new DbParameter("@ToDate", keyWord2);

            return DbRead("[ADMIN].[GetInterfaceExpAccCodeNotMap]", dbParams, connStr);
        }

        #endregion
    }
}