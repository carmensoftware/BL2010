using System.Data;
using Blue.DAL;

namespace Blue.BL.dbo
{
    public class Bu : DbHandler
    {
        #region "Attributes"

        #endregion

        #region "Operations"

        public Bu()
        {
            SelectCommand = "SELECT * FROM [dbo].[Bu]";
            TableName = "Bu";
        }

        public string GetHQBuCode(string BuGrpCode)
        {
            var dsBu = new DataSet();

            var result = GetList(dsBu, BuGrpCode);

            if (result)
            {
                foreach (DataRow drBu in dsBu.Tables[TableName].Rows)
                {
                    if ((bool)drBu["IsHQ"])
                    {
                        return drBu["BuCode"].ToString();
                    }
                }
            }

            return string.Empty;
        }

        public bool GetList(DataSet dsBu, string BuCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            var result = DbRetrieve("dbo.Bu_GetList", dsBu, dbParams, TableName, connStr);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public bool GetList(DataSet dsBu, string BuGrpCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);

            return DbRetrieve("[dbo].[GetBuList_BuGrpCode]", dsBu, dbParams, TableName);
        }


        public DataTable GetList(string BuGrpCode)
        {
            var dsBu = new DataSet();

            var getBu = GetList(dsBu, BuGrpCode);

            if (getBu)
            {
                return dsBu.Tables[TableName];
            }

            return null;
        }


        public string GetName(string BuCode, string connStr)
        {
            var strName = string.Empty;
            var dsBu = new DataSet();

            GetList(dsBu, BuCode, connStr);

            if (dsBu.Tables[TableName] != null)
            {
                if (dsBu.Tables[TableName].Rows.Count > 0)
                {
                    strName = dsBu.Tables[TableName].Rows[0]["BuName"].ToString();
                }
            }

            return strName;
        }


        public DataTable GetListNonHQ(string BuGrpCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);

            return DbRead("[dbo].[GetBuListNonHQ_BuGrpCode]", dbParams);
        }

        public bool GetListNonHQ(DataSet dsBu, string BuGrpCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);

            return DbRetrieve("[dbo].[GetBuListNonHQ_BuGrpCode]", dsBu, dbParams, TableName);
        }

        public bool Get(DataSet dsBu, string BuCode)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuCode", BuCode);

            return DbRetrieve("dbo.GetBu_BuCode", dsBu, dbParams, TableName);
        }

        public bool IsHQ(string BuCode)
        {
            var dsBu = new DataSet();

            var result = Get(dsBu, BuCode);

            if (result)
            {
                return bool.Parse(dsBu.Tables[TableName].Rows[0]["IsHQ"].ToString());
            }

            return false;
        }

        public string GetConnectionString(string BuCode)
        {
            var dsBu = new DataSet();

            var result = Get(dsBu, BuCode);

            if (result)
            {
                if (dsBu.Tables[TableName].Rows.Count > 0)
                {
                    var drBu = dsBu.Tables[TableName].Rows[0];

                    return new Common.ConnectionStringConstant().Get(drBu);

                    //return "Data Source=" + drBu["ServerName"].ToString() + "; " +
                    //    "Initial Catalog = " + drBu["DatabaseName"].ToString() + "; " +
                    //    "User ID = " + drBu["UserName"].ToString() + "; " +
                    //    "Password = " + GnxLib.EnDecryptString(drBu["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);
                }
            }

            return string.Empty;
        }

        public DataTable GetHQConnectionString(string BuGrpCode)
        {
            //DataSet dsBu = new DataSet();
            //bool result = this.Get(dsBu, BuCode);

            //if (result)
            //{
            //    string BuGrpCode = dsBu.Tables[this.TableName].Rows[0]["BuGrpCode"].ToString();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@BuGrpCode", BuGrpCode);
            return DbRead("[dbo].[Get_HQConnStr]", dbParams);

            //}

            //return string.Empty;
        }

        public string GetHQConnStr(string BuGrpCode)
        {
            var HQConnStr = string.Empty;

            var dtBuHQ = GetHQConnectionString(BuGrpCode);

            if (dtBuHQ != null)
            {
                HQConnStr = "Data Source=" + dtBuHQ.Rows[0]["ServerName"] + "; " +
                            "Initial Catalog = " + dtBuHQ.Rows[0]["DatabaseName"] + "; " +
                            "User ID = " + dtBuHQ.Rows[0]["UserName"] + "; " +
                            "Password = " +
                            GnxLib.EnDecryptString(dtBuHQ.Rows[0]["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);
            }

            return HQConnStr;
        }


        #endregion
    }
}