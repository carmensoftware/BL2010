using System;
using BlueLedger.PL.BaseClass;
using Newtonsoft.Json;
using System.IO;

namespace BlueLedger.PL.PT.Sale
{
    public partial class Data : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();

        public string _ID { get { return Request.QueryString["id"] == null ? "" : Request.QueryString["id"].ToString(); } }


        protected override void Page_Load(object sender, EventArgs e)
        {
            var json = GetJsonData(_ID);
            lbl_Text.Text = JsonPrettify(json);
        }

        private string GetJsonData(string id)
        {
            var sql = "SELECT * FROM [INTF].[Data] WHERE Id=@Id";
            var p = new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@Id", id) };
            var dt = bu.DbExecuteQuery(sql, p, LoginInfo.ConnStr);

            var json = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["Data"].ToString() : null;

            return json;
        }

        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }
}