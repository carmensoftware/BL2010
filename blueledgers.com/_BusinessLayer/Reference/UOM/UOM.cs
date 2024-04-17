using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Reference
{
    public class UOM : DbHandler
    {
        #region "Attributies"

        public string UOMCode{get;set;}

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public UOM()
        {
            SelectCommand = "SELECT * FROM Reference.UOM";
            TableName = "UOM";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCategory"></param>
        /// <returns></returns>
        public bool GetUOMList(DataSet dsUOM, string connStr)
        {
            var result = false;

            // Get data
            result = DbRetrieve("Reference.GetUOMList", dsUOM, null, TableName, connStr);

            // Return result
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsCurrency"></param>
        /// <returns></returns>
        public bool Save(DataSet dsUOM, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[1];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsUOM, SelectCommand, TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}