using System;
using System.Data;
using System.Globalization;
using System.Text;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.APP
{
    public class ViewHandlerCols : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public ViewHandlerCols()
        {
            SelectCommand = "SELECT * FROM APP.ViewHandlerCols";
            TableName = "ViewHandlerCols";
        }

        /// <summary>
        ///     Get view handler column data related to view no.
        /// </summary>
        /// <param name="dsViewHandlerCols"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsViewHandlerCols, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            //return DbRetrieve("[dbo].[APP_ViewHandlerCols_GetList_ViewNo]", dsViewHandlerCols, dbParams, this.TableName, ConnStr);
            return DbRetrieve("APP.GetViewHandlerColsByViewNo", dsViewHandlerCols, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get ViewHandlerCols Table Structure
        /// </summary>
        /// <param name="dsViewHandlerCols"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsViewHandlerCols, string connStr)
        {
            //return DbRetrieve("[dbo].APP_ViewHandlerCols_GetSchema", dsViewHandlerCols, null, this.TableName, ConnStr);
            return DbRetrieveSchema("APP.GetViewHandlerColsSchema", dsViewHandlerCols, null, TableName, connStr);
        }

        /// <summary>
        ///     Get column list related to ViewNo.
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        //public string GetColumnList(int ViewNo, bool IsDefaultLang, string ConnStr)
        public string GetColumnList(int viewNo, string connStr)
        {
            var dsViewHandlerCols = new DataSet();

            var result = GetList(dsViewHandlerCols, viewNo, connStr);

            if (result)
            {
                var sbCols = new StringBuilder();

                foreach (DataRow drCols in dsViewHandlerCols.Tables[TableName].Rows)
                {
                    sbCols.Append((sbCols.Length > 0 ? " , " : string.Empty));

                    // Find only field name.
                    var fieldName = drCols["FieldName"].ToString().Split('.');
                    int lastIndex = fieldName.Length - 1;
                    //sbCols.AppendFormat(" [{0}] ", fieldName[2]);
                    sbCols.AppendFormat(" [{0}] ", fieldName[lastIndex]);
                    sbCols.Append(" AS ");
                    sbCols.AppendFormat(" [{0}] ", drCols["FieldName"]);
                }

                return sbCols.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public double GetColumnWidth(int viewNo, string fieldName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));
            dbParams[1] = new DbParameter("@FieldName", fieldName);

            var dtGet = DbRead("[APP].[GetColumnWidth]", dbParams, connStr);

            //if (dtGet != null)
            //{
            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                //if (dtGet.Rows[0][0].ToString() != "")
                //{
                return double.Parse(dtGet.Rows[0][0].ToString());
                //}
            }
            //}

            return 0;
        }

        #endregion
    }
}