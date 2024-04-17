using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class Config : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empy contructor.
        /// </summary>
        public Config()
        {
            SelectCommand = "SELECT * FROM APP.Config";
            TableName = "Config";
        }

        /// <summary>
        ///     Get application config data related to specified module and module.
        /// </summary>
        /// <param name="dsConfig"></param>
        /// <param name="strModule"></param>
        /// <param name="strSubModule"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsConfig, string strModule, string strSubModule, string strConn)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@Module", strModule);
            dbParams[1] = new DbParameter("@SubModule", strSubModule);

            return DbRetrieve("APP.GetConfigList_Module_SubModule", dsConfig, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get application config data related to specified module, sub module and key.
        /// </summary>
        /// <param name="dsConfig"></param>
        /// <param name="strModule"></param>
        /// <param name="strSubModule"></param>
        /// <param name="strKey"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Get(DataSet dsConfig, string strModule, string strSubModule, string strKey, string strConn)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Module", strModule);
            dbParams[1] = new DbParameter("@SubModule", strSubModule);
            dbParams[2] = new DbParameter("@Key", strKey);

            return DbRetrieve("APP.GetConfig_Module_SubModule_Key", dsConfig, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get config value related to specified module, sub module and key.
        /// </summary>
        /// <param name="strModule"></param>
        /// <param name="strSubModule"></param>
        /// <param name="strKey"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetValue(string strModule, string strSubModule, string strKey, string strConn)
        {
            var dsConfig = new DataSet();
            var result = Get(dsConfig, strModule, strSubModule, strKey, strConn);

            if (result)
            {
                if (dsConfig.Tables[TableName].Rows.Count > 0)
                    return dsConfig.Tables[TableName].Rows[0]["Value"].ToString();
                else
                    return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        ///     Save changed data to database.
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


        //------------- CLR Store Procedure -----------------
        /// <summary>
        /// </summary>
        /// <param name="dsConfig"></param>
        /// <param name="strModule"></param>
        /// <param name="strSubModule"></param>
        /// <param name="strKey"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsConfig, string strModule, string strSubModule, string strKey, string strConn)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@Module", strModule);
            dbParams[1] = new DbParameter("@SubModule", strSubModule);
            dbParams[2] = new DbParameter("@Key", strKey);

            return DbRetrieve("dbo.APP_Config_Get_Module_SubModule_Key", dsConfig, dbParams, TableName, strConn);
        }

        /// <summary>
        /// </summary>
        /// <param name="strModule"></param>
        /// <param name="strSubModule"></param>
        /// <param name="strKey"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public string GetConfigValue(string strModule, string strSubModule, string strKey, string strConn)
        {
            var dsConfig = new DataSet();
            var result = GetList(dsConfig, strModule, strSubModule, strKey, strConn);

            if (result)
            {
                if (dsConfig.Tables[TableName].Rows.Count > 0)
                {
                    return dsConfig.Tables[TableName].Rows[0]["Value"].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }


        public void SetConfigValue(string module, string subModule, string key, string value, string connStr)
        {
            DataSet ds = new DataSet();
            this.Get(ds, module, subModule, key, connStr);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["Module"] = module;
                dr["SubModule"] = subModule;
                dr["Key"] = key;
                dr["Value"] = value;
                ds.Tables[0].Rows.Add(dr);
            }
            else
                ds.Tables[0].Rows[0]["Value"] = value;

            this.Save(ds, connStr);

        }

        public string GetCostSystem(string connStr)
        {
            string value = GetValue("IN", "SYS", "COST", connStr);
            return value != string.Empty ? value : "FIFO";
        }


        #endregion
    }
}