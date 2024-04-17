using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Consolidation
{
    public class BusinessUnit : DbHandler
    {
        #region "Attibuties"

        private readonly BusinessUnitAddress _businessUnitAddress = new BusinessUnitAddress();

        public string BusinessUnitCode { get; set; }

        public string Name { get; set; }

        public string TaxID { get; set; }

        public string RegID { get; set; }

        public DateTime RegDate { get; set; }

        public string RegName { get; set; }

        public string Logo { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Website { get; set; }

        public string Remark { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }

        #endregion

        #region "Operations"

        public BusinessUnit()
        {
            SelectCommand = "SELECT * FROM Reference.BusinessUnit";
            TableName = "BusinessUnit";
        }

        /// <summary>
        ///     Get all data from table businessunit
        /// </summary>
        /// <param name="dsBusinessUnit"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataSet GetBusinessUnit(DataSet dsBusinessUnit, string connStr)
        {
            DbRetrieve("Reference.GetBusinessUnitList", dsBusinessUnit, null, TableName, connStr);
            return dsBusinessUnit;
        }

        /// <summary>
        ///     Get businessunit base currency code
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetBaseCurrencyCode(string connStr)
        {
            var dsBU = new DataSet();
            var baseCurrCode = string.Empty;

            GetBusinessUnit(dsBU, connStr);

            if (dsBU != null && dsBU.Tables[TableName] != null && dsBU.Tables[TableName].Rows.Count > 0)
            {
                if (dsBU.Tables[TableName].Rows[0]["BaseCurrencyCode"] != DBNull.Value)
                {
                    baseCurrCode = dsBU.Tables[TableName].Rows[0]["BaseCurrencyCode"].ToString();
                }
            }

            return baseCurrCode;
        }

        /// <summary>
        ///     Save data
        /// </summary>
        /// <param name="dsCity"></param>
        /// <returns></returns>
        public bool Save(DataSet dsBusinessUnit, string connStr)
        {
            var result = false;
            var dbSaveSource = new DbSaveSource[2];

            // Create DbSaveSource
            dbSaveSource[0] = new DbSaveSource(dsBusinessUnit, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsBusinessUnit, _businessUnitAddress.SelectCommand,
                _businessUnitAddress.TableName);

            // Commit to database
            result = DbCommit(dbSaveSource, connStr);

            // Return result
            return result;
        }

        #endregion
    }
}