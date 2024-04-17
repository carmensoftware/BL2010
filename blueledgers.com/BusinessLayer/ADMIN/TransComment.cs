using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class TransComment : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty Instructor.
        /// </summary>
        public TransComment()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[TransComment]";
            TableName = "TransComment";
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var newID = DbReadScalar("[ADMIN].[GetTransCommentNewID]", null, connStr);

            // Return result
            return newID;
        }

        /// <summary>
        ///     Get schema
        /// </summary>
        /// <param name="dsLog"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsLog, string connStr)
        {
            return DbRetrieve("[ADMIN].[GetTransCommentList]", dsLog, null, TableName, connStr);
        }

        public bool GetTransCommentByModule_SubModule_RefNo(DataSet dsComment, string module, string submodule,
            string refNo, string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@Submodule", submodule);
            dbParams[2] = new DbParameter("@RefNo", refNo);

            return DbRetrieve("[ADMIN].[GetTransCommentByModule_SubMobule_RefNo]", dsComment, dbParams, TableName,
                connStr);
            //return this.DbRead("[ADMIN].[GetTransCommentByModule_SubMobule_RefNo]", dbParams, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="module"></param>
        /// <param name="submodule"></param>
        /// <param name="refNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetTransCommentByModule_SubModule_RefNo(string module, string submodule, string refNo,
            string connStr)
        {
            // parameter value assing to param array.
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@Submodule", submodule);
            dbParams[2] = new DbParameter("@RefNo", refNo);

            return DbRead("[ADMIN].[GetTransCommentByModule_SubMobule_RefNo]", dbParams, connStr);
        }

        public bool Save(DataSet dsSaving, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}