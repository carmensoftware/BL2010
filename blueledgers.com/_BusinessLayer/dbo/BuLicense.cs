using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class BuLicense : DbHandler
    {
        public BuLicense()
        {
            SelectCommand = "SELECT * FROM [dbo].[BuLicense]";
            TableName = "BuLicense";
        }

        /// <summary>
        ///     Get BuLicense data by BuCode.
        /// </summary>
        /// <param name="dsBuLicense"></param>
        /// <param name="BuCode"></param>
        /// <returns></returns>
        public bool Get(DataSet dsBuLicense, string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("dbo.BuLicense_Get_BuCode", dsBuLicense, dbParams, TableName);
        }

        /// <summary>
        ///     Get BuLicense MaxUser data by BuCode.
        /// </summary>
        /// <param name="BuCode"></param>
        /// <returns></returns>
        public int GetMaxUser(string BuCode)
        {
            var dsBuLicense = new DataSet();

            var result = Get(dsBuLicense, BuCode);

            if (result)
            {
                if (dsBuLicense.Tables[TableName].Rows.Count > 0)
                {
                    return int.Parse(dsBuLicense.Tables[TableName].Rows[0]["MaxUser"].ToString());
                }
            }

            return -1;
        }
    }
}