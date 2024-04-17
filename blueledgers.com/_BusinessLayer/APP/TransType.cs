using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class TransType : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public TransType()
        {
            SelectCommand = "SELECT * FROM [APP].[TransType]";
            TableName = "TransType";
        }

        /// <summary>
        ///     Get all TransType data by specified ModuleID.
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string moduleID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ModuleID", moduleID);

            return DbRead("[APP].[GetTransTypeListByModuleID]", dbParams, connStr);
        }

        /// <summary>
        ///     Determine the specified ModuleID has child or not.
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool HasChild(string moduleID, string connStr)
        {
            var dtModule = GetList(moduleID, connStr);

            if (dtModule.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}