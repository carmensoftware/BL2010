using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.ADMIN
{
    public class Department : DbHandler
    {
        public Department()
        {
            SelectCommand = "SELECT * FROM [ADMIN].[Department]";
            TableName = "Department";
        }

        public DataTable GetList(string connStr)
        {
            return DbRead("[ADMIN].[GetListDepartment]", null, connStr);
        }

        public bool GetList(DataSet dsDepartment, string connStr)
        {
            return DbRetrieve("[ADMIN].[GetListDepartment]", dsDepartment, null, TableName, connStr);
        }

        public string GetName(string departCode, string connStr)
        {
            var strName = string.Empty;

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepCode", departCode);

            var dtName = DbRead("[ADMIN].[GetName_DepartMent]", dbParams, connStr);
            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["DepName"].ToString();
            }
            return strName;
        }

        public bool Get(DataSet dsDepartment, string depCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DepCode", depCode);

            return DbRetrieve("[ADMIN].[GetByDepCode]", dsDepartment, dbParams, TableName, connStr);
        }

        public string GetHeadOfDep(string depCode, string connStr)
        {
            var dsDepartment = new DataSet();

            var result = Get(dsDepartment, depCode, connStr);

            if (result)
            {
                if (dsDepartment.Tables[TableName].Rows.Count > 0)
                {
                    return dsDepartment.Tables[TableName].Rows[0]["HeadOfDep"].ToString();
                }
            }

            return string.Empty;
        }

        public bool Save(DataSet dsDepartment, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsDepartment, SelectCommand, TableName);

            return DbCommit(dbSaveSource, connStr);
        }
    }
}