using System;
using BlueLedger.PL.BaseClass;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace BlueLedger.PL.Option.Admin
{
    public partial class Api : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
                Page_Setting();

        }

        private void Page_Setting()
        {
            var query = "SELECT AppName, BuCode, ClientID, ExpiryDate FROM BuAPI ORDER BY AppName, BuCode";
            var dt = bu.DbExecuteQuery(query, null);


            gv.DataSource = dt;
            gv.DataBind();

            var intf = new KeyValues();
            intf.Text = config.GetConfigValue("APP", "INTF", "ACCOUNT", LoginInfo.ConnStr);

            var host = intf.Value("host");

            txt_CarmenHost.Text = host;


        }

        protected void btn_Get_Click(object sender, EventArgs e)
        {
            var host = txt_CarmenHost.Text.Trim();
            var adminToken = txt_CarmenAdminToken.Text.Trim();
            var username = txt_CarmenUsername.Text;
            var password = txt_CarmenPassword.Text;


            try
            {
                txt_Result.Text = "";


                var apiTenant = string.Format("api/tenant/adminToken/{0}", adminToken);

                var json = Get(host, apiTenant);

                var o = JObject.Parse(json);

                var data = JArray.Parse(o["Data"].ToString());

                var tenants = new List<Tenant>();

                foreach (var item in data.Children())
                {
                    var t = JObject.Parse(item.ToString());

                    tenants.Add(new Tenant
                    {
                        Code = t["Tenant"].Value<string>(),
                        Name = t["Description"].Value<string>(),
                    });
                }

                foreach (var tenant in tenants)
                {
                    var accessToken = LoginCarmen(tenant.Code);

                    var endpoint = "api/interfaceBlueLedgers/createSessionToken";
                    var body = "";

                    var res = Post(host, endpoint, body, accessToken);
                    var blue = JObject.Parse(res);

                    var directToken = blue["Authorization"].Value<string>();


                    tenant.Token = directToken;

                }

                var s = new StringBuilder();

                foreach (var tenant in tenants)
                {
                    s.AppendFormat("{0} = {1}", tenant.Name, tenant.Token);
                    s.AppendLine("");
                }


                txt_Result.Text = s.ToString();
            }
            catch (Exception ex)
            {
                txt_Result.Text = ex.Message;
            }

        }

        public string Get(string host, string endpoint, string authorization = "")
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.BaseAddress = host.TrimEnd('/') + "/";

                    if (!string.IsNullOrEmpty(authorization))
                        client.Headers.Add("Authorization", authorization);

                    endpoint = endpoint.Trim().TrimStart('/');


                    var data = client.DownloadData(endpoint);


                    return Encoding.UTF8.GetString(data);
                }

            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public string Post(string host, string endpoint, string data, string authorization = "")
        {

            var response = "";

            using (var client = new WebClient())
            {
                if (!string.IsNullOrEmpty(authorization))
                    client.Headers.Add("Authorization", authorization);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";

                var url = host.TrimEnd('/') + "/" + endpoint.Trim().TrimStart('/');

                response = client.UploadString(url, data);

            }

            return response;
        }

        public string LoginCarmen(string tenant)
        {
            var accessToken = "";

            var host = txt_CarmenHost.Text.Trim().TrimEnd('/');
            var adminToken = txt_CarmenAdminToken.Text.Trim();
            var username = txt_CarmenUsername.Text;
            var password = txt_CarmenPassword.Text;


            var endpoint = string.Format("{0}/api/login?adminToken={1}", host, adminToken);

            var data = new Login()
            {
                Tenant = tenant,
                UserName = username,
                Password = password
            };

            //var data = string.Format("{\"Tenant\":\"{0}\",\"UserName\":\"{1}\",\"Password\":\"{2}\"}", tenant, username, password);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";


                var response = client.UploadString(endpoint, JsonConvert.SerializeObject(data));

                var o = JObject.Parse(response);

                accessToken = o["AccessToken"].Value<string>();

            }



            return accessToken;
        }


       

        public class Login
        {
            public string Tenant { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Language { get; set; }
        }


        public class Tenant
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Token { get; set; }
        }

    }


}