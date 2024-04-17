using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class AccTpDt : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccTpDt()
        {
            SelectCommand = "SELECT * FROM GL.AccTpDt";
            TableName = "AccTpDt";
        }

        /// <summary>
        ///     Get all data.
        /// </summary>
        /// <param name="dsAccTp"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccTpDt, string strCode, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Code", strCode);

            return DbRetrieve("GL.GetAccTpDt_Code", dsAccTpDt, dbParams, TableName, strConn);
        }

        #endregion
    }
}