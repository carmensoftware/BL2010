using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class WFHis : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public WFHis()
        {
            SelectCommand = "SELECT * FROM [APP].[WFHis]";
            TableName = "WFHis";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsWFHis"></param>
        /// <param name="refNo"></param>
        /// <param name="refNoDt"></param>
        /// <param name="module"></param>
        /// <param name="subModule"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsWFHis, string refNo, int refNoDt, string module, string subModule, string connStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@RefNo", refNo);
            dbParams[1] = new DbParameter("@RefNoDt", refNoDt.ToString(CultureInfo.InvariantCulture));
            dbParams[2] = new DbParameter("@Module", module);
            dbParams[3] = new DbParameter("@SubModule", subModule);

            return DbRetrieve("APP.GetWFHis_RefNo_RefNoDt", dsWFHis, dbParams, TableName, connStr);
        }

        #endregion
    }
}