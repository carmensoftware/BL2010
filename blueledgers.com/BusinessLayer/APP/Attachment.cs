using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class Attachment : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public Attachment()
        {
            SelectCommand = "SELECT * FROM APP.Attachment";
            TableName = "Attachment";
        }

        /// <summary>
        ///     Get application config data related to specified module and module.
        /// </summary>
        /// <param name="dsAttachment"></param>
        /// <param name="refNo"></param>
        /// <param name="strConn"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAttachment, string module, string refNo, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Module", module);
            dbParams[1] = new DbParameter("@RefNo", refNo);

            return DbRetrieve("[APP].[GetAttachmentList_Module_RefNo]", dsAttachment, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Save change to database.
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

        #endregion
    }
}