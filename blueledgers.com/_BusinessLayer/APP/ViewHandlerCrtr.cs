using System;
using System.Data;
using System.Globalization;
using System.Text;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class ViewHandlerCrtr : DbHandler
    {
        #region "Attributes"

        private readonly Field _field = new Field();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public ViewHandlerCrtr()
        {
            SelectCommand = "SELECT * FROM APP.ViewHandlerCrtr";
            TableName = "ViewHandlerCrtr";
        }

        /// <summary>
        ///     Get ViewHandlerCrtr data by ViewNo.
        /// </summary>
        /// <param name="dsViewHandlerCrtr"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsViewHandlerCrtr, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            //bool result = DbRetrieve("[dbo].[APP_ViewHandlerCrtr_GetList_ViewNo]", dsViewHandlerCrtr, dbParams, this.TableName, ConnStr);
            var result = DbRetrieve("APP.GetViewHandlerCrtrByViewNo", dsViewHandlerCrtr, dbParams, TableName, connStr);

            if (result)
            {
                // Create primarykey                
                dsViewHandlerCrtr.Tables[TableName].PrimaryKey = GetPK(dsViewHandlerCrtr);
            }

            return result;
        }

        /// <summary>
        ///     Get ViewHandlerCrtr Table Structure
        /// </summary>
        /// <param name="dsViewHandlerCrtr"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsViewHandlerCrtr, string connStr)
        {
            //return DbRetrieve("[dbo].APP_ViewHandlerCrtr_GetSchema", dsViewHandlerCrtr, null, this.TableName, ConnStr);
            return DbRetrieveSchema("APP.GetViewHandlerCrtrSchema", dsViewHandlerCrtr, null, TableName, connStr);
        }

        /// <summary>
        ///     Get criteria list by view no.
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="dbParams"></param>
        /// <param name="connStr"></param>
        /// <param name="advanceOpt"></param>
        /// <returns></returns>
        public string GetCriteriaList(int viewNo, string advanceOpt, ref DbParameter[] dbParams, string connStr)
        {
            var dsViewHandlerCrtr = new DataSet();

            var result = GetList(dsViewHandlerCrtr, viewNo, connStr);

            if (result)
            {
                var sbCrtr = new StringBuilder();
                var paramsCount = 0;

                foreach (DataRow drCrtr in dsViewHandlerCrtr.Tables[TableName].Rows)
                {
                    string criteria;
                    var filedName = drCrtr["FieldName"].ToString().Split('.');

                    var strFieldType = _field.GetFieldType(filedName[0], filedName[0] + "." + filedName[1],
                        drCrtr["FieldName"].ToString(), connStr);

                    switch (strFieldType.ToUpper())
                    {
                        case "D":
                            criteria = string.Format("convert(varchar, [{0}], 103) {1} ", filedName[2], drCrtr["Operator"]);
                            break;
                        default:
                            criteria = string.Format("[{0}] {1} ", filedName[2], drCrtr["Operator"]);
                            break;
                    }

                    // Add Parameter to command if Operator is not "IS NULL" and "IS NOT NULL".
                    if (drCrtr["Operator"].ToString().ToUpper() != "IS NULL" &&
                        drCrtr["Operator"].ToString().ToUpper() != "IS NOT NULL")
                    {
                        Array.Resize(ref dbParams, dbParams.Length + 1);
                        criteria += string.Format("@{0}{1} ", filedName[2], drCrtr["ViewCrtrNo"]);
                        dbParams[paramsCount] = new DbParameter(string.Format("@{0}{1}",
                            filedName[2], drCrtr["ViewCrtrNo"]),drCrtr["Value"].ToString());
                        paramsCount++;
                    }

                    
                    // Add logical operation (connect to next criteria)
                    criteria += string.Format("{0} ", drCrtr["LogicalOp"]);

                    // Append criteria to criteria list for return.
                    if (advanceOpt == string.Empty)
                    {
                        // Advance Option is not used
                        sbCrtr.Append(criteria);
                    }
                    else
                    {
                        // Used advance option
                        advanceOpt = advanceOpt.Replace(drCrtr["ViewCrtrNo"].ToString(), criteria);

                        // Clear criteria list and replace by updated criteria
                        if (sbCrtr.Length > 0)
                        {
                            sbCrtr.Replace(sbCrtr.ToString(), advanceOpt);
                        }
                        else
                        {
                            sbCrtr.Append(advanceOpt);
                        }
                    }
                }


                return sbCrtr.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsViewHandleCrtr"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByIsAsking(DataSet dsViewHandleCrtr, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            return DbRetrieve("[dbo].[APP_ViewHandlerCrtr_GetList_IsAsking]", dsViewHandleCrtr, dbParams, TableName,
                connStr);
        }

        /// <summary>
        ///     Create primary keys for ViewHandlerCriteria DataTable from specified DataSet.
        /// </summary>
        /// <param name="dsViewHandlerCrtr"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsViewHandlerCrtr)
        {
            var primaryKeys = new DataColumn[2];
            primaryKeys[0] = dsViewHandlerCrtr.Tables[TableName].Columns["ViewNo"];
            primaryKeys[1] = dsViewHandlerCrtr.Tables[TableName].Columns["ViewCrtrNo"];

            return primaryKeys;
        }

        #endregion
    }
}