using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class CA : DbHandler
    {
        public CA()
        {
            SelectCommand = "SELECT * FROM CA";
            TableName = "CA";
        }

        public bool GetSchema(DataSet dsCA, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@RefNo", string.Empty);

            return DbRetrieveSchema("[IN].GetCA_RefNo", dsCA, dbParams, TableName, ConnStr);
        }

        public string GetNewID(string ConnStr)
        {
            var dtCA = DbRead("[IN].GetCANewID", null, ConnStr);

            if (dtCA != null)
            {
                if (dtCA.Rows.Count > 0)
                {
                    return dtCA.Rows[0][0].ToString();
                }
            }

            return string.Empty;
        }
    }
}