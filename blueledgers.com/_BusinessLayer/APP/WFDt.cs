using System;
using System.Data;
using System.Globalization;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class WFDt : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public WFDt()
        {
            SelectCommand = "SELECT * FROM [APP].[WFDt]";
            TableName = "WFDt";
        }

        /// <summary>
        ///     Get WFDt data by WFId.
        /// </summary>
        /// <param name="dsWFDt"></param>
        /// <param name="wfId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsWFDt, int wfId, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@WFId", wfId.ToString(CultureInfo.InvariantCulture));

            return DbRetrieve("[dbo].[APP_WFDt_GetList_WFId]", dsWFDt, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get WF's IsActive Data by Module and SubModule
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(int wfId, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = GetList(dsWFDt, wfId, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName] != null)
                {
                    return dsWFDt.Tables[TableName];
                }
            }

            return null;
        }

        /// <summary>
        ///     Get work-flow configurations detail data related to WFId and over specified Step.
        /// </summary>
        /// <param name="dsWFDt"></param>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsWFDt, int wfId, int step, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@WFId", wfId.ToString(CultureInfo.InvariantCulture));
            dbParams[1] = new DbParameter("@Step", step.ToString(CultureInfo.InvariantCulture));

            return DbRetrieve("APP_WFDt_GetList_WFId_Step", dsWFDt, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get work-flow configurations detail data related to WFId and over specified Step.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(int wfId, int step, string connStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@WFId", wfId.ToString(CultureInfo.InvariantCulture));
            dbParams[1] = new DbParameter("@Step", step.ToString(CultureInfo.InvariantCulture));

            return DbRead("APP_WFDt_GetList_WFId_Step", dbParams, connStr);
        }

        /// <summary>
        ///     Get Criteria by WFId and Step
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetCriteria(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["Criteria"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["Criteria"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Gets AllowCreate
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetAllowCreate(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["AllowCreate"] != DBNull.Value)
                    {
                        return bool.Parse(dsWFDt.Tables[TableName].Rows[0]["AllowCreate"].ToString());
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Get UserChkField by WFId and Step
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetUserChkField(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["UserChkField"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["UserChkField"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get UserChkField by WFId and Step
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetPermissionGrp(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["PermissionGrp"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["PermissionGrp"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get specified field is allowed edit.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsEnableEdit(int wfId, int step, string fieldName, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["EnableField"] != DBNull.Value)
                    {
                        var enableField = dsWFDt.Tables[TableName].Rows[0]["EnableField"].ToString();

                        return enableField.Contains(fieldName);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Get specified field is visibled.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="fieldName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool IsHide(int wfId, int step, string fieldName, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["HideField"] != DBNull.Value)
                    {
                        var hideField = dsWFDt.Tables[TableName].Rows[0]["HideField"].ToString();

                        return hideField.Contains(fieldName);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Get Appr by WFId and Step.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetAppr(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["Appr"] != DBNull.Value)
                    {
                        return int.Parse(dsWFDt.Tables[TableName].Rows[0]["Appr"].ToString());
                    }
                }
            }

            return 0;
        }

        /// <summary>
        ///     Get Position of Approval (1-10) by LoginName
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="loginName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetApprPosition(int wfId, int step, string loginName, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    for (var i = 1; i <= 10; i++)
                    {
                        if (dsWFDt.Tables[TableName].Rows[0]["Appr" + i].ToString().ToUpper() == loginName.ToUpper())
                        {
                            return i;
                        }
                    }
                }
            }

            return 0;
        }

        /// <summary>
        ///     Excecute the Approve Procedure.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dbParams"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public void ExcecuteApprRule(string procedureName, DbParameter[] dbParams, string connStr)
        {
            var cmd = string.Format("exec {0} ", procedureName);

            for (var i = 0; i <= dbParams.Length - 1; i++)
            {
                cmd += dbParams[i].ParameterName + (i == dbParams.Length - 1 ? string.Empty : " , ");
            }

            DbExecuteQuery(cmd, dbParams, connStr);
        }

        /// <summary>
        ///     Get all Step Description for SendBack.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="currentStep"></param>
        /// <param name="lastStep"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetSandBackStepList(int wfId, int currentStep, int lastStep, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@WFId", wfId.ToString(CultureInfo.InvariantCulture));
            dbParams[1] = new DbParameter("@CurrentStep", currentStep.ToString(CultureInfo.InvariantCulture));
            dbParams[2] = new DbParameter("@LastStep", lastStep.ToString(CultureInfo.InvariantCulture));

            return DbRead("APP.WFDt_GetList_SendBack", dbParams, connStr);
        }

        /// <summary>
        ///  Get Step Back of Current Step
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="currenStep"></param>
        /// <param name="prNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        // Added on: 07/09/2017, By: Fon
        public DataTable GetSendBackStep(int wfId, int currenStep, string prNo, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@wfId", wfId.ToString());
            dbParams[1] = new DbParameter("@currentStep", currenStep.ToString());
            dbParams[2] = new DbParameter("@prNo", prNo);

            return DbRead("APP.WFDt_Get_SendBackStep", dbParams, connStr);
        }


        /// <summary>
        ///     Get GetApprRule By WFId And Step.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetApprRule(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["ApprRule"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["ApprRule"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get GetApprRule By WFId And Step.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetRejRule(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["RejRule"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["RejRule"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get GetApprRule By WFId And Step.
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="step"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetSendBkRule(int wfId, int step, string connStr)
        {
            var dsWFDt = new DataSet();

            var result = Get(dsWFDt, wfId, step, connStr);

            if (result)
            {
                if (dsWFDt.Tables[TableName].Rows.Count > 0)
                {
                    if (dsWFDt.Tables[TableName].Rows[0]["SendBkRule"] != DBNull.Value)
                    {
                        return dsWFDt.Tables[TableName].Rows[0]["SendBkRule"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Execute Rule.
        /// </summary>
        /// <param name="ruleTxt"></param>
        /// <param name="dbParams"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// not complete now.
        public void ExecuteRule(string ruleTxt, DbParameter[] dbParams, string connStr)
        {
            ruleTxt = ruleTxt.Replace("\"", "'").Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
            
            DbExecuteQuery(ruleTxt, dbParams, connStr);
        }
        #endregion
    }
}