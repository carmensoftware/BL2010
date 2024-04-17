using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation.Setup.Application
{
    public class AccountType : DbHandler
    {
        /// <summary>
        ///     Empty contructor
        /// </summary>
        public AccountType()
        {
            SelectCommand = "SELECT * FROM Application.AccountType";
            TableName = "AccountType";
        }

        /// <summary>
        ///     Get AccountType data by using ID.
        /// </summary>
        /// <param name="dsAccountType"></param>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsAccountType, int id, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ID", Convert.ToString(id));

            // Get data and return result
            return DbRetrieve("Application.GetAccountType", dsAccountType, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get AccountType data by using ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable Get(int id, string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            var result = Get(dsAccountType, id, connStr);

            if (result)
            {
                return dsAccountType.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all AccountType data.
        /// </summary>
        /// <param name="dsAccountType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountType, string connStr)
        {
            // Get data and return result
            return DbRetrieve("Application.GetAccountTypeList", dsAccountType, null, TableName, connStr);
        }

        /// <summary>
        ///     Get all AccountType data.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            var result = GetList(dsAccountType, connStr);

            if (result)
            {
                return dsAccountType.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get all actived or inactived AccountType data.
        /// </summary>
        /// <param name="dsAccountType"></param>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsAccountType, bool isActive, string connStr)
        {
            // Paramter value assign to dbparameter array.
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@IsActive", Convert.ToString(isActive));

            // Get data and return result
            return DbRetrieve("Application.GetAccountTypeListActive", dsAccountType, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get all actived or inactived AccountType data.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(bool isActive, string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            var result = GetList(dsAccountType, isActive, connStr);

            if (result)
            {
                return dsAccountType.Tables[TableName];
            }

            return null;
        }

        /// <summary>
        ///     Get AccountType's Name of specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(int id, string connStr)
        {
            var dsAccountType = new DataSet();

            // Get data
            var result = Get(dsAccountType, id, connStr);

            // Return result
            if (result)
            {
                if (dsAccountType.Tables[TableName] == null)
                {
                    return string.Empty;
                }

                if (dsAccountType.Tables[TableName].Rows.Count <= 0)
                {
                    return string.Empty;
                }

                return (dsAccountType.Tables[TableName].Rows[0]["Name"] == DBNull.Value
                    ? string.Empty
                    : dsAccountType.Tables[TableName].Rows[0]["Name"].ToString());
            }

            return string.Empty;
        }

        /// <summary>
        ///     Get AccountType's new ID.
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int GetNewID(string connStr)
        {
            var maxAccountTypeID = 0;

            maxAccountTypeID = DbReadScalar("Application.GetAccountTypeNewID", null, connStr);

            // If value is zero assign 1.
            if (maxAccountTypeID == 0)
            {
                maxAccountTypeID = 1;
            }

            // Return result
            return maxAccountTypeID;
        }

        /// <summary>
        ///     Commit changed AccountType data to DataBase.
        /// </summary>
        /// <param name="dsAccountType"></param>
        /// <param name="dbSaveSource"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsAccountType, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsAccountType, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }
    }
}