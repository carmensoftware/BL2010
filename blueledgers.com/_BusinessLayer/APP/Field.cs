using System;
using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace

namespace Blue.BL.APP
{
    public class Field : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty contructor.
        /// </summary>
        public Field()
        {
            SelectCommand = "SELECT * FROM APP.[Field]";
            TableName = "Field";
        }

        /// <summary>
        ///     Get field data ralated to specified schema name, table name and field name.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsField, string strSchemaName, string strTableName, string strFieldName, string strConn)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@SchemaName", strSchemaName);
            dbParams[1] = new DbParameter("@TableName", strTableName);
            dbParams[2] = new DbParameter("@FieldName", strFieldName);

            return DbRetrieve("APP.GetField_SchemaName_TableName_FieldName", dsField, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Gets field data by specified FieldName.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsField, string fieldName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@FieldName", fieldName);

            //return DbRetrieve("[dbo].[APP_Field_Get_FieldName]", dsField, dbParams, this.TableName, ConnStr);
            return DbRetrieve("APP.GetFieldByFieldName", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Gets field data by specified FieldName and return datatable.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string fieldName, string connStr)
        {
            var dsTmp = new DataSet();
            var result = Get(dsTmp, fieldName, connStr);

            if (result)
            {
                return dsTmp.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get field data ralated to specified schema and table name.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsField, string strSchemaName, string strTableName, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@SchemaName", strSchemaName);
            dbParams[1] = new DbParameter("@TableName", strTableName);

            //return DbRetrieve("GL.GetFieldList_SchemaName_TableName", dsField, dbParams, this.TableName, strConn);
            return DbRetrieve("APP.GetFieldListBySchemaNameTableName", dsField, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get Field Data by TableName.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsField, String tableName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TableName", tableName);

            //return DbRetrieve("[dbo].[APP_Field_GetList_TableName]", dsField, dbParams, this.TableName, ConnStr);
            return DbRetrieve("[APP].[GetFieldListByTableName]", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get only not selected field from APP.Field compare with APP.ViewHandlerCols by ViewNo.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="pageCode"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListViewAvaCols(DataSet dsField, String pageCode, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TableName", (pageCode.Replace("[", "")).Replace("]", ""));
            dbParams[1] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            //return DbRetrieve("[dbo].[APP_Field_GetList_ViewAvaCols]", dsField, dbParams, this.TableName, ConnStr);
            return DbRetrieve("[APP].[GetFieldList_ViewAvaCols]", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get only not selected field from APP.Field compare with APP.ViewHandlerCols by ViewNo.
        /// </summary>
        /// <param name="pageCode"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListViewAvaCols(String pageCode, int viewNo, string connStr)
        {
            var dsField = new DataSet();

            var result = GetListViewAvaCols(dsField, pageCode, viewNo, connStr);

            if (result)
            {
                return dsField.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get only selected field from APP.Field compare with APP.ViewHandlerCols by ViewNo.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="pageCode"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListViewSelCols(DataSet dsField, String pageCode, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TableName", (pageCode.Replace("[", "")).Replace("]", ""));
            dbParams[1] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            //return DbRetrieve("[dbo].[APP_Field_GetList_ViewSelCols]", dsField, dbParams, this.TableName, ConnStr);
            return DbRetrieve("[APP].[GetFieldList_ViewSelCols]", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get only selected field from APP.Field compare with APP.ViewHandlerCols by ViewNo.
        /// </summary>
        /// <param name="pageCode"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListViewSelCols(String pageCode, int viewNo, string connStr)
        {
            var dsField = new DataSet();

            var result = GetListViewSelCols(dsField, pageCode, viewNo, connStr);

            if (result)
            {
                return dsField.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get Field Data by TableName.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string tableName, string connStr)
        {
            var dsField = new DataSet();

            var result = GetList(dsField, tableName, connStr);

            if (result)
            {
                return dsField.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get field data ralated to specified schema and table name.
        /// </summary>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetList(string strSchemaName, string strTableName, string strConn)
        {
            var dsField = new DataSet();

            var result = GetList(dsField, strSchemaName, strTableName, strConn);

            if (result)
            {
                return dsField.Tables[TableName];
            }
            return null;
        }


        /// <summary>
        ///     Get field's description ralated to specified schema name, table name and field name.
        /// </summary>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetDesc(string strSchemaName, string strTableName, string strFieldName, string strConn)
        {
            var dsField = new DataSet();
            var result = Get(dsField, strSchemaName, strTableName, strFieldName, strConn);

            if (result)
            {
                if (dsField.Tables[TableName].Rows.Count > 0)
                {
                    return dsField.Tables[TableName].Rows[0]["Desc"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Gets field's description by specified FieldName.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDesc(string fieldName, string connStr)
        {
            var dsField = new DataSet();

            var result = Get(dsField, fieldName, connStr);

            if (result)
            {
                if (dsField.Tables[TableName].Rows.Count > 0)
                {
                    return dsField.Tables[TableName].Rows[0]["Desc"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Gets field's other description by specified FieldName.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetOthDesc(string fieldName, string connStr)
        {
            var dsField = new DataSet();

            var result = Get(dsField, fieldName, connStr);

            if (result)
            {
                if (dsField.Tables[TableName].Rows.Count > 0)
                {
                    return dsField.Tables[TableName].Rows[0]["OthDesc"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get field's other description ralated to specified schema name, table name and field name.
        /// </summary>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetOthDesc(string strSchemaName, string strTableName, string strFieldName, string strConn)
        {
            var dsField = new DataSet();
            var result = Get(dsField, strSchemaName, strTableName, strFieldName, strConn);

            if (result)
            {
                return dsField.Tables[TableName].Rows[0]["OthDesc"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get field's field type code ralated to specified schema name, table name and field name.
        /// </summary>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetFieldType(string strSchemaName, string strTableName, string strFieldName, string strConn)
        {
            var dsField = new DataSet();
            var result = Get(dsField, strSchemaName, strTableName, strFieldName, strConn);

            if (result)
            {
                if (dsField.Tables[TableName].Rows.Count > 0)
                {
                    return dsField.Tables[TableName].Rows[0]["FieldTypeCode"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get field's lookup id ralated to specified schema name, table name and field name.
        /// </summary>
        /// <param name="strSchemaName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetLookupID(string strSchemaName, string strTableName, string strFieldName, string strConn)
        {
            var dsField = new DataSet();

            var result = Get(dsField, strSchemaName, strTableName, strFieldName, strConn);

            if (result)
            {
                return dsField.Tables[TableName].Rows[0]["LookupID"].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Get field list
        /// </summary>
        /// <param name="tableList"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetWFField(string tableList, string connStr)
        {
            var dbPrams = new DbParameter[1];
            dbPrams[0] = new DbParameter("@TableList", tableList);

            return DbRead("dbo.APP_Field_GetWFField", dbPrams, connStr);
        }

        #endregion
    }
}