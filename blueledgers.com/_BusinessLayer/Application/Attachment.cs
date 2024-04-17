using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class Attachment : DbHandler
    {
        public Attachment()
        {
            SelectCommand = "SELECT * FROM [Application].[Attachment]";
            TableName = "Attachment";
        }

        public bool Get(DataSet dsAttach, string Module, string RefNo, string connStr)
        {
            // Create parameter
            var dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Module", Module);
            dbParam[1] = new DbParameter("@RefNo", RefNo);

            // Get data and return value
            return DbRetrieve("[Application].GetByModuleRefNo", dsAttach, dbParam, TableName, connStr);
        }

        /// <summary>
        ///     Save attachment data to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }
    }
}