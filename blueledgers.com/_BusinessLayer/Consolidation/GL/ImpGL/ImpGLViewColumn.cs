using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.GL.ImpGL
{
    public class ImpGLViewColumn : DbHandler
    {
        #region "Attributies"

        //private int impGLViewColumnID;
        //private int impGLViewID;
        //private int seqNo;

        #endregion

        #region "Operations"

        public ImpGLViewColumn()
        {
            SelectCommand = "SELECT * FROM GL.ImpGLViewColumn";
            TableName = "ImpGLViewColumn";
        }

        /// <summary>
        ///     Generate column list using accountViewID
        /// </summary>
        /// <param name="accountViewID"></param>
        /// <returns></returns>
        public static string GetImpGLViewColumn(int impGLViewID, string connStr)
        {
            var impGLViewColumn = new DataTable();
            var dbParams = new DbParameter[1];
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Create parameters
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get Data
            impGLViewColumn = new DbHandler().DbRead("GL.GetImpGLViewColumnListByImpGLViewID", dbParams, connStr);

            // Generate Column List
            if (impGLViewColumn != null)
            {
                foreach (DataRow dr in impGLViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsImpGLViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLViewColumnSchema(DataSet dsImpGLViewColumn, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("GL.GetImpGLViewColumnList", dsImpGLViewColumn, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="impGLViewID"></param>
        /// <param name="dsImpGLViewColumn"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetImpGLViewColumnList(int impGLViewID, DataSet dsImpGLViewColumn, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameters
            dbParams[0] = new DbParameter("@impGLViewID", impGLViewID.ToString());

            // Get Data
            result = DbRetrieve("GL.GetImpGLViewColumnListByImpGLViewID", dsImpGLViewColumn, dbParams, TableName,
                connStr);

            // return result
            return result;
        }

        /// <summary>
        ///     Generate column list using datatable
        /// </summary>
        /// <param name="dsPreview"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string GetImpGLViewColumnPreview(DataTable dtPreview, string connStr)
        {
            var impGLViewColumn = new DataTable();
            var columnList = string.Empty;
            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Get Data
            impGLViewColumn = dtPreview;

            // Generate Column List
            if (impGLViewColumn != null)
            {
                foreach (DataRow dr in impGLViewColumn.Rows)
                {
                    columnList += (columnList != string.Empty ? "," : string.Empty) + "[" +
                                  field.GetFieldName(dr["FieldID"].ToString(), connStr) + "]" +
                                  " AS [" + dr["FieldID"] + "]";
                }
            }

            return columnList;
        }

        #endregion
    }
}