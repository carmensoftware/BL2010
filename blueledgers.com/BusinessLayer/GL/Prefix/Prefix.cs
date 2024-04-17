using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.GL
{
    public class Prefix : DbHandler
    {
        #region "Attributies"

        public string PrefixCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        #endregion

        #region "Operations"

        public Prefix()
        {
            SelectCommand = "SELECT * FROM GL.Prefix";
            TableName = "Prefix";
        }

        /// <summary>
        ///     Return datatable from prefix.
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrefixList(string connStr)
        {
            return DbRead("GL.GetPrefixList", null, connStr);
        }

        /// <summary>
        ///     Get all prefix data
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <returns></returns>
        public bool GetPrefixList(DataSet dsPrefix, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("GL.GetPrefixList", dsPrefix, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data by code.
        /// </summary>
        /// <param name="prefixCode"></param>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetByCode(string prefixCode, DataSet dsPrefix, string connStr)
        {
            var result = false;
            var dbParams = new DbParameter[1];

            // Create parameter
            dbParams[0] = new DbParameter("@PrefixCode", prefixCode);

            // Get data
            result = DbRetrieve("GL.GetPrefixByCode", dsPrefix, dbParams, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Get name.
        /// </summary>
        /// <param name="prefixCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string prefixCode, string connStr)
        {
            var dsPrefix = new DataSet();
            var result = string.Empty;

            // Get data
            GetByCode(prefixCode, dsPrefix, connStr);

            // Return result
            if (dsPrefix.Tables[TableName].Rows.Count > 0)
            {
                result = dsPrefix.Tables[TableName].Rows[0]["Name"].ToString();
            }

            return result;
        }

        /// <summary>
        ///     Save data to database
        /// </summary>
        /// <param name="dsPrefix"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsPrefix, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsPrefix, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}