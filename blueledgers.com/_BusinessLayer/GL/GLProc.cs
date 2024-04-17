using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class GLProc : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructure
        /// </summary>
        public GLProc()
        {
            SelectCommand = "SELECT * FROM GL.GLProc";
            TableName = "GLProc";
        }

        /// <summary>
        ///     This function is used for get all GL.GLProc data by using stored procedure "GL.GetGLProcList".
        /// </summary>
        /// <param name="dsTrialBalLst"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTrialBalLst, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetGLProcList", dsTrialBalLst, null, TableName, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}