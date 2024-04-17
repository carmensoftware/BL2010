using System.Data;
using Blue.DAL;

namespace Blue.BL.PC.TP
{
    public class Template : DbHandler
    {
        public Template()
        {
            SelectCommand = "SELECT * FROM [In].[Template]";
            TableName = "Template";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsTemplate"></param>
        /// <param name="TmpNo"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsTemplate, int TmpNo, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", TmpNo.ToString());

            return DbRetrieve("dbo.TP_GetListByTmpNo", dsTemplate, dbParams, TableName, ConnStr);
        }

        /// <summary>
        ///     Gets all active Template data.
        /// </summary>
        /// <param name="dsTemplate"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsTemplate, string TmpTypeCode, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TmpTypeCode", TmpTypeCode);
            dbParams[1] = new DbParameter("@LocationCode", LocationCode);

            var result = DbRetrieve("dbo.IN_Template_GetList_TmpTypeCode_LocationCode", dsTemplate, dbParams, TableName,
                ConnStr);

            if (result)
            {
                //Create Primary Kye
                dsTemplate.Tables[TableName].PrimaryKey = GetPK(dsTemplate);
            }

            return true;
        }

        private DataColumn[] GetPK(DataSet dsTemplate)
        {
            var primarykeys = new DataColumn[1];
            primarykeys[0] = dsTemplate.Tables[TableName].Columns["TmpNo"];

            return primarykeys;
        }

        /// <summary>
        ///     Get active template by TypeCode
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string TypeCode, string LoginName, string ConnStr)
        {
            var dsTemplate = new DataSet();
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@TmpTypeCode", TypeCode);
            dbParams[1] = new DbParameter("@LoginName", LoginName);

            var result = DbRetrieve("[IN].[GetTemplateActiveListByTmpTypeCode_LoginName]", dsTemplate, dbParams,
                TableName, ConnStr);

            if (result)
            {
                return dsTemplate.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Gets Template data by TmpNo.
        /// </summary>
        /// <param name="dsTemplate"></param>
        /// <param name="LoginName"></param>
        /// <param name="MsgError"></param>
        /// <returns></returns>
        public bool GetListByTmpNo(DataSet dsTemplate, ref string MsgError, int tmpNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNo", tmpNo.ToString());

            var result = DbRetrieve("dbo.TP_GetListByTmpNo", dsTemplate, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        //return DbRetrieve("dbo.TP_GetListByTmpNo", dsTemplate, dbParams, this.TableName, ConnStr);


        /// <summary>
        ///     Get Max Template No.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewTmpNo(string connStr)
        {
            var NewID = DbReadScalar("dbo.TP_GetTmpNewNo", null, connStr);

            // Return result
            return NewID;
        }

        /// <summary>
        ///     Get Lookup TemplateCode.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            // Create parameters
            return DbRead("dbo.TP_GetActiveList", null, connStr);
        }

        public int GetNewTemplateNo(string connStr)
        {
            // Create parameters
            return DbReadScalar("dbo.TP_GetNewTemplateNo", null, connStr);
        }

        public bool GetSchema(DataSet dsTemplate, string ConnStr)
        {
            return DbRetrieve("dbo.IN_Template_GetSchema", dsTemplate, null, TableName, ConnStr);
        }

        /// <summary>
        ///     Commit user changed to database.
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsTemplate, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsTemplate, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsTemplate, new TemplateDt().SelectCommand, new TemplateDt().TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public bool Delete(DataSet dsTemplate, string connStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsTemplate, new TemplateDt().SelectCommand, new TemplateDt().TableName);
            dbSaveSource[1] = new DbSaveSource(dsTemplate, SelectCommand, TableName);
            return DbCommit(dbSaveSource, connStr);
        }

        public int CountByLocationCode(string Location, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", Location);

            return DbReadScalar("dbo.IN_Template_CountByLocationCode", dbParams, ConnStr);
        }

        public int CountByTmpNo(string TmpNp, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@TmpNp", TmpNp);

            return DbReadScalar("dbo.TP_CountByTmpNo", dbParams, ConnStr);
        }

        public DataTable GetCateByLocationCode(string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);

            var dsTmp = new DataSet();

            var result = DbRetrieve("IN.Template_GetCateByLocationCode", dsTmp, dbParams, TableName, ConnStr);

            if (result)
            {
                dsTmp.Tables[TableName].PrimaryKey = GetPK(dsTmp);
            }

            return dsTmp.Tables[TableName];
        }

        public DataTable GetCateByLocationCodeCategoryType(string LocationCode, string CategoryType, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@CategoryType", CategoryType);

            var dsTmp = new DataSet();

            var result = DbRetrieve("IN.Template_GetCateByLocationCodeCategoryType", dsTmp, dbParams, TableName, ConnStr);

            if (result)
            {
                dsTmp.Tables[TableName].PrimaryKey = GetPK(dsTmp);
            }

            return dsTmp.Tables[TableName];
        }
    }
}