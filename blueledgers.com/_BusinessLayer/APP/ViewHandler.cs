using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Blue.DAL;


// ReSharper disable once CheckNamespace

namespace Blue.BL.APP
{
    public class ViewHandler : DbHandler
    {
        #region "Attributes"

        private readonly dbo.Bu _bu = new dbo.Bu();
        private readonly ViewHandlerCols _viewHandlerCols = new ViewHandlerCols();
        private readonly ViewHandlerCrtr _viewHandlerCrtr = new ViewHandlerCrtr();
        private readonly ViewHandlerOrder _viewHandlerOrder = new ViewHandlerOrder();
        private readonly WFDt _wfDt = new WFDt();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public ViewHandler()
        {
            SelectCommand = "SELECT * FROM APP.ViewHandler";
            TableName = "ViewHandler";
        }

        /// <summary>
        ///     Get ViewHandler data by ViewNo.
        /// </summary>
        /// <param name="dsViewHandler"></param>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsViewHandler, int viewNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ViewNo", viewNo.ToString(CultureInfo.InvariantCulture));

            //return DbRetrieve("[dbo].[APP_ViewHandler_Get_ViewNo]", dsViewHandler, dbParams, this.TableName, ConnStr);
            return DbRetrieve("APP.GetViewHandlerByViewNo", dsViewHandler, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get advance option by ViewNo.
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetAdvanceOption(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            if (result)
            {
                if (dsViewHandler.Tables[TableName].Rows.Count > 0)
                {
                    return dsViewHandler.Tables[TableName].Rows[0]["AdvOpt"].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get view handler data related to PageCode and LoginName.
        /// </summary>
        /// <param name="dsViewHandler"></param>
        /// <param name="pageCode"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsViewHandler, string pageCode, string loginName, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@PageCode", pageCode);
            dbParams[1] = new DbParameter("@LoginName", loginName);

            return DbRetrieve("[APP].GetViewHandlerListByPageCode_LoginName", dsViewHandler, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get view handler data related to PageCode and LoginName and return in DataTable format.
        /// </summary>
        /// <param name="pageCode"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string pageCode, string loginName, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = GetList(dsViewHandler, pageCode, loginName, connStr);

            if (result)
            {
                return dsViewHandler.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="buGrpCode"></param>
        /// <param name="dsDataList"></param>
        /// <param name="viewNo"></param>
        /// <param name="pageCode"></param>
        /// <param name="keyFieldName"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool GetDataListHQ(string buGrpCode, DataSet dsDataList, int viewNo, string pageCode, string keyFieldName,
            string loginName)
        {
            // Get All actived Bu in specified BuGrpCode
            var dsBu = new DataSet();

            var result = _bu.GetList(dsBu, buGrpCode);

            if (result)
            {
                // Get all data in group
                foreach (DataRow drBu in dsBu.Tables[_bu.TableName].Rows)
                {
                    // Create Connection String by Business Unit
                    var connStr = new Common.ConnectionStringConstant().Get(drBu);

                    //this.GetDataList(dsDataList, ViewNo, PageCode, KeyFieldName, drBu["BuCode"].ToString(), LoginName, connStr, drBu["BuCode"].ToString());
                    GetDataList(dsDataList, viewNo, pageCode, keyFieldName, "", drBu["BuCode"].ToString(), loginName,
                        connStr);
                }

                return true;
            }

            return false;
        }


        public bool GetDataList(DataSet dsDataList, int viewNo, string pageCode, string keyFieldName, string filterText, string buCode, string loginName, string connStr)
        {
            var sbCommand = new StringBuilder();
            var advOption = GetAdvanceOption(viewNo, connStr);
            var wfId = GetWFId(viewNo, connStr);
            var wfStep = GetWFStep(viewNo, connStr);

            // Modified By:Fon
            var isHOD = IsFilterByHOD(viewNo, connStr);
            var isHOL = IsFilterByHOL(viewNo, connStr);


            sbCommand.Append("SELECT " + keyFieldName + ", ");

            // Always add ApprStatus to the query when work-flow was enable
            if (GetIsWFEnable(viewNo, connStr))
            {
                sbCommand.Append("ApprStatus, ");
            }

            var buName = "";
            var dtBu = DbExecuteQuery("SELECT TOP 1 [Name] FROM [ADMIN].Bu", null, connStr);

            if (dtBu != null && dtBu.Rows.Count > 0)
                buName = dtBu.Rows[0][0].ToString();

            //sbCommand.AppendFormat("'{0}' AS BuCode, (SELECT [Name] FROM [ADMIN].Bu WHERE BuCode='{0}') AS [Buisness Unit Name], ", buCode);
            sbCommand.AppendFormat("'{0}' AS BuCode, '{1}' AS [Buisness Unit Name], ", buCode, buName);

            // Get column list            
            sbCommand.Append(_viewHandlerCols.GetColumnList(viewNo, connStr));
            sbCommand.AppendFormat(" FROM {0} ", pageCode);
            sbCommand.AppendFormat(" WHERE 1=1 ");


            var dbParams = new DbParameter[0];
            // Get criteria
            var criteriaList = _viewHandlerCrtr.GetCriteriaList(viewNo, advOption, ref dbParams, connStr);

            // Assign Special Params to dbParams
            for (int i = 0; i < dbParams.Length; i++)
            {
                if (dbParams[i].ParameterValue == "@UserDepartment")
                {
                    DataTable dt = DbExecuteQuery(string.Format("SELECT TOP(1) DepCode FROM [ADMIN].UserDepartment WHERE LoginName='{0}'", loginName), null, connStr);
                    if (dt.Rows.Count > 0)
                    {
                        dbParams[i].ParameterValue = Regex.Replace(dbParams[i].ParameterValue.ToString(), "@UserDepartment", dt.Rows[0][0].ToString(), RegexOptions.IgnoreCase);
                    }
                }
                else if (dbParams[i].ParameterValue == "@LoginName")
                {
                    dbParams[i].ParameterValue = Regex.Replace(dbParams[i].ParameterValue.ToString(), "@LoginName", loginName, RegexOptions.IgnoreCase);
                }

            }


            // Always add work-flow criteria to query when work-flow was enable
            #region
            if (wfId > 0)
            {

                // Add Normal Criteria
                string criteriaField = Regex.Replace(_wfDt.GetCriteria(wfId, wfStep, connStr), "@LoginName", " '" + loginName + "' ", RegexOptions.IgnoreCase);

                criteriaList += string.Format("{0} {1} ", (criteriaList != string.Empty ? " AND " : " "), criteriaField);

                var userChkField = _wfDt.GetUserChkField(wfId, wfStep, connStr);
                if (userChkField != string.Empty)
                {
                    if (userChkField.Split(',').Length > 0)
                    {
                        string[] fields = userChkField.Split(',');

                        criteriaList += string.Format(" {0} ({1} = '{2}'", (criteriaList.Trim() != string.Empty ? "AND" : " "), fields[0].Trim(), loginName);
                        for (int i = 1; i <= fields.Length - 1; i++)
                        {
                            criteriaList += string.Format(" OR {0} = '{1}'", fields[i].Trim(), loginName);
                        }

                        criteriaList += " ) ";
                    }
                    else
                        criteriaList += string.Format(" {0}  {1} = '{2}'", (criteriaList.Trim() != string.Empty ? "AND" : " "), userChkField, loginName);
                }

                // Add Addition Critera (User Permission Group)
                var permissionGrp = _wfDt.GetPermissionGrp(wfId, wfStep, connStr);
                if (permissionGrp != string.Empty)
                {
                    criteriaList += string.Format("{0}'{1}' IN ({2})", (criteriaList != string.Empty ? " AND " : " "), loginName, permissionGrp);
                }
            }
            #endregion

            // Get sorting
            var sortingList = _viewHandlerOrder.GetSortingList(viewNo, connStr);
            // Criteria Condition
            if (criteriaList.Trim() != string.Empty)
                sbCommand.AppendFormat(" AND {0}", criteriaList);

            // Filter Condition
            if (filterText != string.Empty)
                sbCommand.AppendFormat(" AND {0}", filterText);

            // Head of Location Condition
            if (isHOD)
            {
                sbCommand.AppendFormat(" AND [HOD] IN (SELECT DepCode FROM [Admin].vHeadOfDepartment WHERE LoginName = '{0}')", loginName);
            }

            // Head of Location Condition
            if (isHOL)
            {
            }
            // ---------------------------- //

            if (sortingList.Trim() != string.Empty)
            {
                sbCommand.AppendFormat(" ORDER BY {0}", sortingList);
            }

            
            return DbExecuteQuery(sbCommand.ToString(), dsDataList, dbParams.Length > 0 ? dbParams : null, pageCode, _bu.GetConnectionString(buCode));
        }

        public bool GetDataListK(DataSet dsDataList, int viewNo, string pageCode, string keyFieldName, string buCode, string loginName, string connStr)
        {
            var sbCommand = new StringBuilder();
            var advacnceOption = GetAdvanceOption(viewNo, connStr);
            var wfId = GetWFId(viewNo, connStr);
            var wfStep = GetWFStep(viewNo, connStr);
            var dbParams = new DbParameter[0];

            sbCommand.AppendFormat("SELECT {0}, ", keyFieldName);

            // Always add ApprStatus to the query when work-flow was enable
            if (GetIsWFEnable(viewNo, connStr))
            {
                sbCommand.Append("ApprStatus, ");
            }


            var buName = "";
            var dtBu = DbExecuteQuery("SELECT TOP 1 [Name] FROM [ADMIN].Bu", null, connStr);

            if (dtBu != null && dtBu.Rows.Count > 0)
                buName = dtBu.Rows[0][0].ToString();

            //sbCommand.AppendFormat("'{0}' AS BuCode, (SELECT [Name] FROM [ADMIN].Bu WHERE BuCode='{0}') AS [Buisness Unit Name], ", buCode);
            sbCommand.AppendFormat("'{0}' AS BuCode, '{1}' AS [Buisness Unit Name], ", buCode, buName);
            
            // Get column list            
            sbCommand.Append(_viewHandlerCols.GetColumnList(viewNo, connStr));
            sbCommand.AppendFormat(" FROM {0} ", pageCode);

            // Get criteria
            var criteriaList = "";

            criteriaList += _viewHandlerCrtr.GetCriteriaList(viewNo, advacnceOption, ref dbParams, connStr);


            // Always add work-flow criteria to query when work-flow was enable
            if (wfId > 0)
            {
                // Add Normal Criteria and assign @LoginName = loginName value

                string criteriaField = Regex.Replace(_wfDt.GetCriteria(wfId, wfStep, connStr), "@LoginName", " '" + loginName + "' ", RegexOptions.IgnoreCase);

                criteriaList += string.Format("{0} {1} ", (criteriaList != string.Empty ? " AND " : " "), criteriaField);

                // Add Additional Criteria (Compare LoginName with a Transaction's field)
                var userChkField = _wfDt.GetUserChkField(wfId, wfStep, connStr);

                if (userChkField != string.Empty)
                {
                    if (userChkField.Split(',').Length > 0)
                    {
                        string[] fields = userChkField.Split(',');

                        criteriaList += string.Format(" {0} ({1} = '{2}'", (criteriaList.Trim() != string.Empty ? "AND" : " "), fields[0].Trim(), loginName);
                        for (int i = 1; i <= fields.Length - 1; i++)
                        {

                            criteriaList += string.Format(" OR {0} = '{1}'", fields[i].Trim(), loginName);
                        }

                        criteriaList += " ) ";
                    }
                    else

                        criteriaList += string.Format("{0}{1} = '{2}'", (criteriaList.Trim() != string.Empty ? " AND " : " "), userChkField, loginName);
                }
            }

            // Get sorting
            var sortingList = _viewHandlerOrder.GetSortingList(viewNo, connStr);

            if (criteriaList.Trim() != string.Empty)
            {
                sbCommand.Append(" WHERE " + criteriaList);
            }

            if (sortingList.Trim() != string.Empty)
            {
                sbCommand.Append(" ORDER BY " + sortingList);
            }

            return DbExecuteQuery(sbCommand.ToString(), dsDataList, dbParams.Length > 0 ? dbParams : null, buCode, _bu.GetConnectionString(buCode));
        }

        /// <summary>
        ///     Get new ViewNo
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            return DbReadScalar("[dbo].[APP_ViewHandler_GetNewID]", null, connStr);
        }

        /// <summary>
        ///     Get Desc by ViewNo.
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetDesc(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            return result && dsViewHandler.Tables[TableName].Rows.Count > 0
                ? dsViewHandler.Tables[TableName].Rows[0]["Desc"].ToString()
                : string.Empty;
        }

        /// <summary>
        ///     Get WFId by ViewNo
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetWFId(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            return result && dsViewHandler.Tables[TableName].Rows.Count > 0 && dsViewHandler.Tables[TableName].Rows[0]["WFId"] != DBNull.Value
                ? int.Parse(dsViewHandler.Tables[TableName].Rows[0]["WFId"].ToString())
                : 0;
        }

        /// <summary>
        ///     Get WFStep by ViewNo
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetWFStep(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            return result && dsViewHandler.Tables[TableName].Rows.Count > 0 && dsViewHandler.Tables[TableName].Rows[0]["WFStep"] != DBNull.Value
                ? int.Parse(dsViewHandler.Tables[TableName].Rows[0]["WFStep"].ToString())
                : 0;
        }

        public bool IsFilterByHOD(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();
            var result = Get(dsViewHandler, viewNo, connStr);

            DataColumnCollection columns = dsViewHandler.Tables[TableName].Columns;
            if (columns.Contains("IsHOD"))
            {

                if (result && dsViewHandler.Tables[TableName].Rows.Count > 0)
                {
                    return dsViewHandler.Tables[TableName].Rows[0]["IsHOD"] != DBNull.Value
                        ? Convert.ToBoolean(dsViewHandler.Tables[TableName].Rows[0]["IsHOD"])
                        : false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsFilterByHOL(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();
            var result = Get(dsViewHandler, viewNo, connStr);
            DataColumnCollection columns = dsViewHandler.Tables[TableName].Columns;
            if (columns.Contains("IsHOL"))
            {
                if (result && dsViewHandler.Tables[TableName].Rows.Count > 0)
                {
                    return dsViewHandler.Tables[TableName].Rows[0]["IsHOL"] != DBNull.Value
                        ? Convert.ToBoolean(dsViewHandler.Tables[TableName].Rows[0]["IsHOL"])
                        : false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        ///     Get IsStandard by ViewNo
        /// </summary>
        /// <param name="viewNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetIsStandard(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            return result && dsViewHandler.Tables[TableName].Rows.Count > 0 &&
                   dsViewHandler.Tables[TableName].Rows[0]["IsStandard"].ToString() != string.Empty &&
                   bool.Parse(dsViewHandler.Tables[TableName].Rows[0]["IsStandard"].ToString());
        }

        public bool GetIsWFEnable(int viewNo, string connStr)
        {
            var dsViewHandler = new DataSet();

            var result = Get(dsViewHandler, viewNo, connStr);

            return result && dsViewHandler.Tables[TableName].Rows.Count > 0 &&
                   dsViewHandler.Tables[TableName].Rows[0]["IsWFEnable"].ToString() != string.Empty &&
                   bool.Parse(dsViewHandler.Tables[TableName].Rows[0]["IsWFEnable"].ToString());
        }

        /// <summary>
        ///     Get ViewHandler Table Structure
        /// </summary>
        /// <param name="dsViewHandler"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetSchema(DataSet dsViewHandler, string connStr)
        {
            //return DbRetrieve("[dbo].[APP_ViewHandler_GetSchema]", dsViewHandler, null, this.TableName, ConnStr);
            return DbRetrieveSchema("APP.GetViewHandlerSchema", dsViewHandler, null, TableName, connStr);
        }

        /// <summary>
        ///     Save ViewHandler, ViewHandler Column and ViewHandler Criteria changed to DataBase.
        /// </summary>
        /// <param name="dsViewHandler"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsViewHandler, string connStr)
        {
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(dsViewHandler, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsViewHandler, _viewHandlerCrtr.SelectCommand, _viewHandlerCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(dsViewHandler, _viewHandlerCols.SelectCommand, _viewHandlerCols.TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        /// <summary>
        ///     Delete ViewHandler
        /// </summary>
        /// <param name="dsViewHandler"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsViewHandler, string connStr)
        {
            var dbSaveSource = new DbSaveSource[3];
            dbSaveSource[0] = new DbSaveSource(dsViewHandler, _viewHandlerCols.SelectCommand, _viewHandlerCols.TableName);
            dbSaveSource[1] = new DbSaveSource(dsViewHandler, _viewHandlerCrtr.SelectCommand, _viewHandlerCrtr.TableName);
            dbSaveSource[2] = new DbSaveSource(dsViewHandler, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }

        #endregion
    }
}