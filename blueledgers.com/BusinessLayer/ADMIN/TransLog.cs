using System;
using System.Data;
using Blue.DAL;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class TransLog : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Instructor.
        /// </summary>
        public TransLog()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[TransLog]";
            TableName = "TransLog";
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var drTransLog = DbRead("[Admin].[GetTransLogNewID]", null, connStr);

            return Convert.ToInt32(drTransLog.Rows[0]["ID"]);
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsLog, string connStr)
        {
            return DbRetrieveSchema("Admin.GetTransLogList", dsLog, null, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="module"></param>
        /// <param name="submodule"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTransLogByModule_SubModule_RefNo(string module, string submodule, string refNo,
            string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@Submodule", submodule);
            dbParams[2] = new DbParameter("@RefNo", refNo);

            return DbRead("[Admin].[GetTransLogByModule_SubModule_RefNo]", dbParams, connStr);
        }

        /// <summary>
        ///     Save data to datebase
        /// </summary>
        /// <param name="dsLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsLog, string connStr)
        {
            var dbSaveSorce = new DbSaveSource[1];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsLog, SelectCommand, TableName);

            // Save to database
            var result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        public void Save(string module, string subMoudle, string refNo, string action, string remark, string createdBy, string connStr)
        {
            string log = string.Empty;

            if (remark != string.Empty)
                log = string.Format("<{0}><TABLE id=\"{1}\"><Remark>{2}</Remark></TABLE></{0}>", action, subMoudle, remark);
            else
                log = string.Format("<{0} />", action);

            string id = this.GetNewID(connStr).ToString();
            //string createdDate = Blue.BL.GnxLib.GetSysDate(string.Empty).ToString("yyyy-MM-dd hh:mm:ss");

            string sql = "INSERT INTO [ADMIN].TransLog(ID, Module, SubModule, RefNo, Log, CreatedDate, CreatedBy)";
            //sql += string.Format(" VALUES ( {0}, '{1}', '{2}', '{3}', '{4}', GETDATE(), '{5}')", id, module, subMoudle, refNo, @log, createdBy);
            //DbExecuteQuery(@sql, null, connStr);

            sql += string.Format(" VALUES ( {0}, '{1}', '{2}', '{3}', @Log, GETDATE(), @CreatedBy)", id, module, subMoudle, refNo);
            var p = new List<DbParameter>();
            
            p.Add(new DbParameter("Log", log));
            p.Add(new DbParameter("CreatedBy", createdBy));

            DbExecuteQuery(@sql, p.ToArray(), connStr);
        }


        #endregion
    }
}