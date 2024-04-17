using System.Data;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Blue.BL.Common
{
    public class ConnectionStringConstant
    {
        private const string FmtConn = "Data Source={0};Initial Catalog={1};User ID={2};Password={3};packet size=4096;Min Pool Size=5; Max Pool Size=200;Connection Timeout=30;";

        private const string Serv = "ServerName";
        private const string DB = "DatabaseName";
        private const string Un = "UserName";
        private const string Pwd = "Password";

        public string Get(DataRow drBu)
        {
            return new StringBuilder().AppendFormat(FmtConn,
                drBu[Serv],
                drBu[DB],
                drBu[Un],
                GnxLib.EnDecryptString(drBu[Pwd].ToString(), GnxLib.EnDeCryptor.DeCrypt)).ToString();
        }
    }
}