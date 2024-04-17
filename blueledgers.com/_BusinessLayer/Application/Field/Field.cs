using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Application
{
    public class Field : DbHandler
    {
        /// <summary>
        ///     Empty constructure
        /// </summary>
        public Field()
        {
            SelectCommand = "SELECT * FROM [Application].[Field]";
            TableName = "Field";
        }

        /// <summary>
        ///     Get field list by tableID.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetFieldListByTableID(DataSet dsField, string tableID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@tableID", tableID);

            return DbRetrieve("[Application].GetFieldListByTableIDAndRoleID", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get field list by fieldId
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="fieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetFieldListByFieldID(DataSet dsField, string fieldID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@FieldID", fieldID);

            return DbRetrieve("[Application].GetFieldListByFieldID", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get field data is contained table which IsPermissionSetup is 'True'.
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListIsPermissionSetup(DataSet dsField, string connStr)
        {
            return DbRetrieve("[Application].GetFieldListIsPermissionSetup", dsField, null, TableName, connStr);
        }

        /// <summary>
        ///     Get Field data from Field table by FieldID
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsField, string FieldID, string connStr)
        {
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@FieldID", FieldID);

            return DbRetrieve("Application.GetField", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get Field data from Field table by FieldID
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(string FieldID, string connStr)
        {
            var dsField = new DataSet();

            // Get data
            var retrieved = Get(dsField, FieldID, connStr);

            if (retrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    return dsField.Tables[TableName];
                }
            }

            return null;
        }

        /// <summary>
        ///     Get field name from specified field id.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetFieldName(string FieldID, string connStr)
        {
            var dsField = new DataSet();
            var fieldName = string.Empty;

            var retrieved = Get(dsField, FieldID, connStr);

            if (retrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        fieldName = dsField.Tables[TableName].Rows[0]["FieldName"].ToString();
                    }
                }
            }

            return fieldName;
        }

        /// <summary>
        ///     Get field length.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetFieldLength(string FieldID, string connStr)
        {
            var dsField = new DataSet();
            var fieldLength = 0;

            var retrieved = Get(dsField, FieldID, connStr);

            if (retrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        if (dsField.Tables[TableName].Rows[0]["Length"] != DBNull.Value)
                        {
                            fieldLength = (int) dsField.Tables[TableName].Rows[0]["Length"];
                        }
                    }
                }
            }

            return fieldLength;
        }

        /// <summary>
        ///     Get table name from specified field id.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetTableName(string FieldID, string connStr)
        {
            var dsField = new DataSet();
            var TableID = string.Empty;

            // Get field data.
            var fieldRetrieved = Get(dsField, FieldID, connStr);

            if (fieldRetrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        TableID = dsField.Tables[TableName].Rows[0]["TableID"].ToString();
                    }
                }

                // Get table data.
                if (TableID != string.Empty)
                {
                    var dsTable = new DataSet();
                    var table = new Table();

                    var tableRetrieved = table.GetTableListByTableID(dsTable, TableID, connStr);

                    if (tableRetrieved)
                    {
                        if (dsTable.Tables[table.TableName] != null)
                        {
                            if (dsTable.Tables[table.TableName].Rows.Count > 0)
                            {
                                return dsTable.Tables[table.TableName].Rows[0]["TableName"].ToString();
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get schema name from specified field id.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetSchemaName(string FieldID, string connStr)
        {
            var dsField = new DataSet();
            var TableID = string.Empty;

            // Get field data.
            var fieldRetrieved = Get(dsField, FieldID, connStr);

            if (fieldRetrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        TableID = dsField.Tables[TableName].Rows[0]["TableID"].ToString();
                    }
                }

                // Get table data.
                if (TableID != string.Empty)
                {
                    var dsTable = new DataSet();
                    var table = new Table();

                    var tableRetrieved = table.GetTableListByTableID(dsTable, TableID, connStr);

                    if (tableRetrieved)
                    {
                        if (dsTable.Tables[table.TableName] != null)
                        {
                            if (dsTable.Tables[table.TableName].Rows.Count > 0)
                            {
                                return dsTable.Tables[table.TableName].Rows[0]["Schema"].ToString();
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get field type of specified filed id.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetFieldType(string FieldName, string connStr)
        {
            var dsField = new DataSet();

            // Get data
            var retrieved = Get(dsField, FieldName, connStr);

            if (retrieved)
            {
                if (dsField.Tables[TableName] != null)
                {
                    if (dsField.Tables[TableName].Rows.Count > 0)
                    {
                        if (dsField.Tables[TableName].Rows[0]["FieldType"] != DBNull.Value)
                        {
                            return (int) dsField.Tables[TableName].Rows[0]["FieldType"];
                        }
                    }
                }
            }

            return 0;
        }

        /// <summary>
        ///     Get field list by using TableID and IsAdditional
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="TableID"></param>
        /// <param name="IsAdditional"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsField, string TableID, bool IsAdditional, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TableID", TableID);
            dbParams[1] = new DbParameter("@IsAdditional", IsAdditional.ToString());

            // Get data.
            return DbRetrieve("Application.GetFieldListByTableIDIsAdditional", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get field list by using TableID
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="TableID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsField, string TableID, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TableID", TableID);

            // Get data.
            return DbRetrieve("Application.GetFieldListByTableID", dsField, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get list of field data from specfied SchamaName and TableName
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsField, string SchemaName, string TableName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@SchemaName", SchemaName);
            dbParams[1] = new DbParameter("@TableName", TableName);

            // Get data.
            return DbRetrieve("[Application].GetFieldList", dsField, dbParams, this.TableName, connStr);
        }

        /// <summary>
        ///     Get list of field data from specfied SchamaName and TableName
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string SchemaName, string TableName, string connStr)
        {
            var dsField = new DataSet();
            var dtField = new DataTable();

            // Get data.
            var retrieved = GetList(dsField, SchemaName, TableName, connStr);

            if (retrieved)
            {
                if (dsField.Tables[this.TableName] != null)
                {
                    dtField = dsField.Tables[this.TableName];
                }
            }

            return dtField;
        }

        /// <summary>
        ///     Get list of field data from specfied SchamaName and TableName
        /// </summary>
        /// Micky
        /// <param name="SchemaName"></param>
        /// <param name="TableName1"></param>
        /// <param name="TableName2"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListTableIDs(string TableName1, string TableName2, string connStr)
        {
            var retrieved = false;
            var dsField = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TableName1", TableName1);
            dbParams[1] = new DbParameter("@TableName2", TableName2);

            // Get data.            
            retrieved = DbRetrieve("[Application].GetFieldList_TableIDs", dsField, dbParams, TableName, connStr);

            if (retrieved)
            {
                return dsField.Tables[TableName];
            }
            return null;
        }

        /// <summary>
        ///     Get field list by tableID.
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListTableID(string tableID, string connStr)
        {
            var dsField = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@tableID", tableID);

            // Get data           
            var retrieved = DbRetrieve("Application.GetFieldListByTableIDAndRoleID", dsField, dbParams, TableName,
                connStr);

            // Return result
            if (retrieved)
            {
                return dsField.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get field display text by FieldID.
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDisplayText(string FieldID, string Language, string connStr)
        {
            var result = string.Empty;
            var dsField = new DataSet();

            // Get data
            Get(dsField, FieldID, connStr);

            if (dsField.Tables[TableName] != null)
            {
                if (dsField.Tables[TableName].Rows.Count > 0)
                {
                    if (Language.ToUpper() == "EN-US")
                    {
                        result = (dsField.Tables[TableName].Rows[0]["DisplayText1"] == DBNull.Value
                            ? string.Empty
                            : dsField.Tables[TableName].Rows[0]["DisplayText1"].ToString());
                    }
                    else
                    {
                        result = (dsField.Tables[TableName].Rows[0]["DisplayText2"] == DBNull.Value
                            ? string.Empty
                            : dsField.Tables[TableName].Rows[0]["DisplayText2"].ToString());
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Get list of additional field with specified TableName.
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Language"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListIsAdditional(DataSet dsField, string SchemaName, string TableName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@SchemaName", SchemaName);
            dbParams[1] = new DbParameter("@TableName", TableName);

            // Get data.
            return DbRetrieve("[Application].GetFieldListIsAdditional", dsField, dbParams, this.TableName, connStr);
        }

        /// <summary>
        ///     Get list of additional field with specified TableName.
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Language"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListIsAdditional(string SchemaName, string TableName, string connStr)
        {
            var dsField = new DataSet();

            // Get data.
            GetListIsAdditional(dsField, SchemaName, TableName, connStr);

            // Return result
            return dsField.Tables[this.TableName];
        }

        /// <summary>
        ///     Commit changed to database.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, string connStr)
        {
            // Build savesource object
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit change to database (include RolePermission data).
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, DataSet dsRolePermission, string connStr)
        {
            var rolePermission = new Blue.BL.Security.RolePermission();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsRolePermission, rolePermission.SelectCommand, rolePermission.TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit changed to database (include Lookup, LookupItem and RolePermission data).
        ///     This function is used for remove additional field which field type is Lookup.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="dsLookup"></param>
        /// <param name="dsLookupItem"></param>
        /// <param name="dsRolePermission"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, DataSet dsLookup, DataSet dsLookupItem, DataSet dsRolePermission,
            string connStr)
        {
            var lookup = new Lookup();
            var lookupItem = new LookupItem();
            var rolePermission = new Blue.BL.Security.RolePermission();

            // Build savesource object            
            DbSaveSource[] dbSaveSource;

            if (dsLookup.Tables[lookup.TableName].Rows.Count > 0)
            {
                dbSaveSource = new DbSaveSource[4];

                if (dsLookup.Tables[lookup.TableName].Rows[0].RowState != DataRowState.Deleted)
                {
                    dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
                    dbSaveSource[1] = new DbSaveSource(dsLookup, lookup.SelectCommand, lookup.TableName);
                    dbSaveSource[2] = new DbSaveSource(dsLookupItem, lookupItem.SelectCommand, lookupItem.TableName);
                    dbSaveSource[3] = new DbSaveSource(dsRolePermission, rolePermission.SelectCommand,
                        rolePermission.TableName);
                }
                else
                {
                    dbSaveSource[0] = new DbSaveSource(dsRolePermission, rolePermission.SelectCommand,
                        rolePermission.TableName);
                    dbSaveSource[1] = new DbSaveSource(dsLookupItem, lookupItem.SelectCommand, lookupItem.TableName);
                    dbSaveSource[2] = new DbSaveSource(dsLookup, lookup.SelectCommand, lookup.TableName);
                    dbSaveSource[3] = new DbSaveSource(savedData, SelectCommand, TableName);
                }
            }
            else
            {
                dbSaveSource = new DbSaveSource[2];
                dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
                dbSaveSource[1] = new DbSaveSource(dsRolePermission, rolePermission.SelectCommand,
                    rolePermission.TableName);
            }

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Commit changed to database (include Lookup and LookupItem data).
        ///     This function is used for Apply changed additional field data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="dsLookup"></param>
        /// <param name="dsLookupItem"></param>
        /// <param name="dsRolePermission"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet savedData, DataSet dsLookup, DataSet dsLookupItem, string connStr)
        {
            var lookup = new Lookup();
            var lookupItem = new LookupItem();

            // Build savesource object
            var dbSaveSource = new DbSaveSource[3];

            if (dsLookup.Tables[lookup.TableName].Rows[0].RowState != DataRowState.Deleted)
            {
                dbSaveSource[0] = new DbSaveSource(savedData, SelectCommand, TableName);
                dbSaveSource[1] = new DbSaveSource(dsLookup, lookup.SelectCommand, lookup.TableName);
                dbSaveSource[2] = new DbSaveSource(dsLookupItem, lookupItem.SelectCommand, lookupItem.TableName);
            }
            else
            {
                dbSaveSource[0] = new DbSaveSource(dsLookupItem, lookupItem.SelectCommand, lookupItem.TableName);
                dbSaveSource[1] = new DbSaveSource(dsLookup, lookup.SelectCommand, lookup.TableName);
                dbSaveSource[2] = new DbSaveSource(savedData, SelectCommand, TableName);
            }

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, connStr);
        }

        //public bool Save(DataSet savedData, bool includeLookup, string connStr)
        //{
        //    if (includeLookup)
        //    {
        //        Lookup lookup               = new Lookup();
        //        LookupItem lookupItem       = new LookupItem();
        //        DbSaveSource[] dbSaveSource = new DbSaveSource[3];
        //        dbSaveSource[0] = new DbSaveSource(savedData, this.SelectCommand, this.TableName);                
        //        dbSaveSource[1] = new DbSaveSource(savedData, lookup.SelectCommand, lookup.TableName);
        //        dbSaveSource[2] = new DbSaveSource(savedData, lookupItem.SelectCommand, lookupItem.TableName);
        //        // Call function dbCommit for commit data to database
        //        return DbCommit(dbSaveSource, connStr);
        //    }
        //    else
        //    {
        //        return this.Save(savedData, connStr);
        //    }
        //}
        //public bool Delete(DataSet deletedData, bool includeLookup, string connStr)
        //{
        //    if (includeLookup)
        //    {
        //        Lookup lookup               = new Lookup();
        //        LookupItem lookupItem       = new LookupItem();
        //        DbSaveSource[] dbSaveSource = new DbSaveSource[3];
        //        dbSaveSource[0] = new DbSaveSource(deletedData, this.SelectCommand, this.TableName);
        //        dbSaveSource[1] = new DbSaveSource(deletedData, lookupItem.SelectCommand, lookupItem.TableName);
        //        dbSaveSource[2] = new DbSaveSource(deletedData, lookup.SelectCommand, lookup.TableName);
        //        // Call function dbCommit for commit data to database
        //        return DbCommit(dbSaveSource, connStr);
        //    }
        //    else
        //    {
        //        return this.Save(deletedData, connStr);
        //    }
        //}        
        /// <summary>
        ///     Commit changed to database include Lookup data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="includeLookup"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        ///     Commit deleted to database include Lookup data.
        /// </summary>
        /// <param name="savedData"></param>
        /// <param name="includeLookup"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        ///     Get field list by datasourceID.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetFieldListByDataSourceID(int datasourceID, string connStr)
        {
            var dsField = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@DataSourceID", Convert.ToString(datasourceID));

            // Get data           
            var retrieved = DbRetrieve("Application.GetFieldNameByDataSourceID", dsField, dbParams, TableName, connStr);

            // Return result
            if (retrieved)
            {
                return dsField.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get uniqueColumnName by TableName.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetUniqueColumnNameByTableName(string tablName, string connStr)
        {
            var dsField = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@tablName", tablName);

            // Get data           
            var retrieved = DbRetrieve("Application.GetUniqueColumnNameByTableName", dsField, dbParams, TableName,
                connStr);

            // Return result
            if (retrieved)
            {
                return dsField.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get requireColumnName by TableName.
        /// </summary>
        /// <param name="dsSchema"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetRequireColumnNameByTableName(string tablName, string connStr)
        {
            var dsField = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@tablName", tablName);

            // Get data           
            var retrieved = DbRetrieve("Application.GetRequireColumnNameByTableName", dsField, dbParams, TableName,
                connStr);

            // Return result
            if (retrieved)
            {
                return dsField.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get structure
        /// </summary>
        /// <param name="dsField"></param>
        /// <param name="tableID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsField, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieveSchema("[Application].GetFieldListAll", dsField, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     GetFieldList
        /// </summary>
        /// <param name="tableID"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetFieldList(string tableID, string connStr)
        {
            var dsField = new DataSet();
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@tableID", tableID);

            // Get data           
            var retrieved = DbRetrieve("Application.GetFieldListByTableID", dsField, dbParams, TableName, connStr);

            // Return result
            if (retrieved)
            {
                return dsField.Tables[TableName];
            }

            return null;
        }
    }
}