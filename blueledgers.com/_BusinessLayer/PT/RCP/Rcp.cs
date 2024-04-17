using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.PT.RCP
{
    public class Rcp : DbHandler
    {
        public Rcp()
        {
            SelectCommand = "SELECT * FROM [PT].[Rcp]";
            TableName = "Rcp";
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet ds, string connStr)
        {
            return DbRetrieveSchema("[PT].[GetSchemaRcp]", ds, null, TableName, connStr);
        }

        /// <summary>
        ///     Get data by RcpCode.
        /// </summary>
        /// <param name="dsRcp"></param>
        /// <param name="MsgError"></param>
        /// <param name="Code"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet ds, string rcpNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RcpNo", rcpNo);

            var result = DbRetrieve("[PT].[GetListRcp]", ds, dbParams, TableName, connStr);
            return result;

        }

        /// <summary>
        ///     Save data to database.
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            var RcpDt = new BL.PT.RCP.RcpDt();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaving, RcpDt.SelectCommand, RcpDt.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        public DataTable GetListCategory(string connStr)
        {
            string sqlSelect = "SELECT * FROM [PT].RcpCategory ORDER BY RcpCateCode";
            return DbExecuteQuery(sqlSelect, null, connStr);
        }

        public DataTable GetListUnit(string connStr)
        {
            //string sqlSelect = "SELECT * FROM [PT].RcpUnit ORDER BY RcpUnitCode";
            string sqlSelect = "SELECT UnitCode as RcpUnitCode, Name as RcpUnitDesc FROM [IN].Unit WHERE IsActived = 1 ORDER BY UnitCode";
            return DbExecuteQuery(sqlSelect, null, connStr);
        }


        //public bool IsValidCode(string code, string connStr)
        //{
        //    string sqlSelect = "SELECT * FROM [PT].Rcp WHERE RcpCode = '" + code + "'";

        //    return DbExecuteQuery(sqlSelect, null, connStr).Rows.Count == 0;
        //}


        public bool IsExistCode(string code, string connStr)
        {
            string sqlSelect = "SELECT COUNT(*) as RecCount FROM [PT].Rcp WHERE RcpCode = '" + code + "'";

            return DbExecuteQuery(sqlSelect, null, connStr).Rows[0][0].ToString() != "0";
        }


    }
}