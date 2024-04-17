using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL.ImpGL
{
    public class ImpGLDetail : DbHandler
    {
        #region "Attributies"

        #endregion

        #region "Operations"

        public ImpGLDetail()
        {
            SelectCommand = "SELECT * FROM GL.ImpGLDetail";
            TableName = "ImpGLDetail";
        }


        /// <summary>
        /// </summary>
        /// <param name="dsImpGLDetail"></param>
        /// <param name="impGLCode"></param>
        /// <param name="connStr"></param>
        public void GetImpGLDetailList(DataSet dsImpGLDetail, string connStr)
        {
            var impGLDetail = new ImpGLDetail();
            impGLDetail.DbRetrieve("GL.GetImpGLDetailList", dsImpGLDetail, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGLDetail"></param>
        /// <param name="impGLCode"></param>
        /// <param name="connStr"></param>
        public void GetImpGLDetailListByImpGLCode(DataSet dsImpGLDetail, string impGLCode, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@impGLCode", impGLCode);

            var impGLDetail = new ImpGLDetail();
            impGLDetail.DbRetrieve("GL.GetImpGLDetailListByImpGLCode", dsImpGLDetail, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGLDetail"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLDetailStructure(DataSet dsImpGLDetail, string connStr)
        {
            return DbRetrieveSchema("GL.GetImpGLDetailList", dsImpGLDetail, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // สร้าง SaveSource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // เรียก dbCommit โดยส่ง SaveSource object เป็น parameter
            DbCommit(dbSaveSource, connStr);

            return true;
        }

        #endregion
    }
}