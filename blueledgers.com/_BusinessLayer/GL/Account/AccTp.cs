using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Account
{
    public class AccTp : DbHandler
    {
        #region "Operations"

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public AccTp()
        {
            SelectCommand = "SELECT * FROM GL.AccTp";
            TableName = "AccTp";
        }

        /// <summary>
        ///     Get all data.
        /// </summary>
        /// <param name="dsAccTp"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccTp, string strConn)
        {
            return DbRetrieve("GL.GetAccTp", dsAccTp, null, TableName, strConn);
        }

        #endregion
    }
}