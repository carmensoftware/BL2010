using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PT.RCP
{
    public class RcpCategory : DbHandler
    {
        public RcpCategory()
        {
            SelectCommand = "SELECT * FROM [PT].[RcpCategory]";
            TableName = "RcpCategory";
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet ds, string connStr)
        {
            return DbRetrieveSchema("[PT].[GetSchemaRcpCategory]", ds, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data by RcpCateCode.
        /// </summary>
        /// <param name="dsRcp"></param>
        /// <param name="MsgError"></param>
        /// <param name="Code"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet ds, string code, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RcpCateCode", code);

            var result = DbRetrieve("[PT].[GetListRcpCategory]", ds, dbParams, TableName, connStr);
            return result;
        }


        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        public DataTable GetListCategory(string connStr)
        {
            string sqlSelect = "SELECT * FROM [PT].RcpCategory ORDER BY RcpCateCode";
            return DbExecuteQuery(sqlSelect, null, connStr);
        }


        public bool IsValidCode(string code, string connStr)
        {
            string sqlSelect = "SELECT TOP(1) * FROM [PT].RcpCategory WHERE RcpCateCode = '" + code + "'";

            return DbExecuteQuery(sqlSelect, null, connStr).Rows.Count == 0;
        }

        public bool IsDeleted(string code, string connStr)
        {
            string sqlSelect = "SELECT TOP(1) * FROM [PT].Rcp WHERE RcpCateCode = '" + code + "'";

            return DbExecuteQuery(sqlSelect, null, connStr).Rows.Count == 0;
        }

    }
}