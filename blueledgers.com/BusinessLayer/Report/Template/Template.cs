using System.Data;
using Blue.DAL;

namespace Blue.BL.Report
{
    public class Template : DbHandler
    {
        #region "Operations"

        public Template()
        {
            SelectCommand = "SELECT * FROM Report.Template";
            TableName = "Template";
        }

        /// <summary>
        ///     Get Byte[] of template data
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public byte[] GetTemplateData(string TemplateName, string ConnectionString)
        {
            var dtTemplate = new DataTable();

            var dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@Name", TemplateName);

            dtTemplate = DbRead("Report.GetTemplateReportData", dbParam, ConnectionString);

            if (dtTemplate != null)
            {
                if (dtTemplate.Rows.Count > 0)
                {
                    if (dtTemplate.Rows[0]["ReportData"] != System.DBNull.Value)
                    {
                        return (byte[]) dtTemplate.Rows[0]["ReportData"];
                    }
                }
            }

            return null;
        }

        #endregion
    }
}