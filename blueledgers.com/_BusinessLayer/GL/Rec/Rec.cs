using System.Data;
using Blue.DAL;

namespace Blue.BL.GL.Rec
{
    public class Rec : DbHandler
    {
        #region "Attributes"

        private readonly RecView recView = new RecView();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructor
        /// </summary>
        public Rec()
        {
            SelectCommand = "SELECT * FROM GL.Rec";
            TableName = "Rec";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsRec, string strConn)
        {
            return DbRetrieveSchema("GL.GetRecList", dsRec, null, TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetMaxNo(string connStr)
        {
            var dsRec = new DataSet();
            string maxNo;
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetRecMaxRecNo", dsRec, null, TableName, connStr);

            if (result)
            {
                maxNo = dsRec.Tables["Rec"].Rows[0]["RecNo"].ToString();
            }
            else
            {
                maxNo = string.Empty;
            }

            // Return result
            return maxNo;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="intViewID"></param>
        /// <param name="strUserName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsRec, int intViewID, string strUserName, string strConn)
        {
            // Get query string and parameter
            var dbParams = new DbParameter[0];
            var strQuery = recView.GetQuery(intViewID, ref dbParams, strUserName, strConn);

            // Get account list.
            return DbExecuteQuery(strQuery, dsRec, (dbParams.Length > 0 ? dbParams : null), TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        public void GetByRecNo(DataSet dsRec, string recNo, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RecNo", recNo);

            var rec = new Rec();
            rec.DbRetrieve("GL.GetRec_RecNo", dsRec, dbParams, TableName, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsRec, string connStr)
        {
            var recDt = new BL.GL.Rec.RecDt();
            var ledgers = new GL.JV.Ledgers();
            var jv = new Blue.BL.GL.JV.JV();
            var jvDetail = new GL.JV.JVDt();
            var result = false;

            var dbSaveSorce = new DbSaveSource[5];
            //DbSaveSource[] dbSaveSorce = new DbSaveSource[2];

            // Create dbSaveSource
            dbSaveSorce[0] = new DbSaveSource(dsRec, jv.SelectCommand, jv.TableName);
            dbSaveSorce[1] = new DbSaveSource(dsRec, jvDetail.SelectCommand, jvDetail.TableName);
            dbSaveSorce[2] = new DbSaveSource(dsRec, ledgers.SelectCommand, ledgers.TableName);
            dbSaveSorce[3] = new DbSaveSource(dsRec, SelectCommand, TableName);
            dbSaveSorce[4] = new DbSaveSource(dsRec, recDt.SelectCommand, recDt.TableName);

            // Save to database
            result = DbCommit(dbSaveSorce, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}