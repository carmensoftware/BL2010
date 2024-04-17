using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using FastReport;

namespace ReportViewer
{
    public partial class Main : Form
    {
        public static string sysDB_ConnStr = string.Empty;
        public static bool isLogin = false;
        public static string loginName = string.Empty;

        private DataTable dtBuCode = new DataTable();
        private DataTable dtReport = new DataTable();
        private string connStr = string.Empty;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lbl_Username.Text = loginName;
            btn_Login.Visible = true;

            sysDB_ConnStr = ConfigurationManager.ConnectionStrings["SysDB"].ConnectionString;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            Login();

        }


        private void btn_Login_Click(object sender, EventArgs e)
        {
            Login();
        }


        private void Login()
        {
            Login fmLogin = new Login();
            fmLogin.ShowDialog();

            if (isLogin)
            {
                lbl_Username.Text = loginName;

                SqlConnection connection = new SqlConnection(sysDB_ConnStr);
                using (connection)
                {
                    string sql = string.Format(@"
                                SELECT BuUser.BuCode, Bu.BuName, BuUser.BuCode + ' : '+ Bu.BuName as BuItem,  Bu.ServerName, Bu.DatabaseName, Bu.UserName, Bu.[Password], Bu.IsHQ
                                FROM dbo.BuUser 
                                JOIN dbo.Bu ON Bu.BuCode = BuUser.BuCode
                                WHERE 
                                Bu.IsActived = 1
                                AND BuUser.LoginName = '{0}'", loginName);

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dtBuCode = ds.Tables[0];

                    cbb_BuCode.DisplayMember = "BuItem";
                    cbb_BuCode.ValueMember = "BuCode";
                    cbb_BuCode.DataSource = dtBuCode;

                    connection.Close();
                }

            }

            btn_Login.Visible = !isLogin;


        }

        private void cbb_BuCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string buCode = cbb_BuCode.SelectedValue.ToString();
            connStr = GetConnectionString(buCode);


            SqlConnection connection = new SqlConnection(connStr);
            using (connection)
            {

                string sql = "SELECT * FROM [ADMIN].Bu";
                SqlDataAdapter adapterAdmin = new SqlDataAdapter(sql, connection);

                DataSet dsAdmin = new DataSet();
                adapterAdmin.Fill(dsAdmin);
                DataTable dt = dsAdmin.Tables[0];
                if (dt != null)
                {
                    var data = (Byte[])dt.Rows[0]["BuLogo"];
                    var stream = new System.IO.MemoryStream(data);
                    pictureBox1.Image = Image.FromStream(stream);
                }



                sql = "SELECT rpt.RptId, rpt.RptName, rpt.RptFileName, f.Data FROM RPT.Report2 rpt LEFT JOIN RPT.ReportFile f ON f.RptFileName = rpt.RptFileName WHERE rpt.IsActive = 1 ORDER BY RptName";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dtReport = ds.Tables[0];

                grd_Report.DataSource = dtReport;
                grd_Report.Columns["RptId"].Visible = false;
                grd_Report.Columns["RptFileName"].Visible = false;
                grd_Report.Columns["RptName"].HeaderText = "Name";
                grd_Report.Columns["RptName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                grd_Report.Columns["Data"].Visible = false;

                connection.Close();
            }

        }

        private void btn_OpenReport_Click(object sender, EventArgs e)
        {
            OpenReport();
        }


        private void grd_Report_DoubleClick(object sender, EventArgs e)
        {
            OpenReport();
        }

        // --------------------------------------------------------------

        private void OpenReport()
        {
            string data = @grd_Report.SelectedRows[0].Cells["Data"].Value.ToString();

            if (data != string.Empty)
            {
                string fileName = System.IO.Directory.GetCurrentDirectory() + @"\report.tmp";

                using (Report report = new Report())
                {
                    report.LoadFromString(data);
                    setConnectionStringToReport(connStr, report);
                    report.Show();
                }
            }
            else
                MessageBox.Show("No report found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void setConnectionStringToReport(string connStr, Report report)
        {
            for (int i = 0; i < report.Report.Dictionary.Connections.Count; i++)
                report.Report.Dictionary.Connections[i].ConnectionString = connStr;
        }


        private string GetConnectionString(string buCode)
        {
            DataRow[] drArray = dtBuCode.Select(string.Format("BuCode = '{0}'", buCode));

            if (drArray.Length > 0)
            {
                DataRow dr = drArray[0];

                var conn = new SqlConnectionStringBuilder();
                conn.DataSource = dr["ServerName"].ToString();
                conn.InitialCatalog = dr["DatabaseName"].ToString();
                conn.UserID = dr["UserName"].ToString(); ;
                conn.Password = GnxLib.EnDecryptString(dr["Password"].ToString(), GnxLib.EnDeCryptor.DeCrypt);

                //conn.IntegratedSecurity = true;
                return conn.ConnectionString;
            }
            else
                return string.Empty;
        }

        // --------------------------------------------------------------

    }

    public class GnxLib
    {
        public enum EnDeCryptor
        {
            EnCrypt = 0,
            DeCrypt = 1
        }

        /// <summary>
        ///     This function is used to encrypt or decrypt the string by using MD5
        ///     CryptoService and Triple DES CryptoService
        /// </summary>
        /// <param name="text">Original String that you want to encrypt or decrypt</param>
        /// <param name="cryptType">Boolean value specifying whether current request should be encrypt or decrypt</param>
        /// <returns>Encrypted String or Decrypted string return depend on Input Encrypt Boolean Variable</returns>
        public static string EnDecryptString(string text, EnDeCryptor cryptType, string key = null)
        {
            //string key = null;
            if (key == null || key == string.Empty)
                key = "BinEnPassCoder";

            string EncryptString = null;
            string DecryptString = null;
            byte[] HashKey = null;
            byte[] buff = null;
            System.Security.Cryptography.MD5CryptoServiceProvider HashMD5 = null;
            System.Security.Cryptography.TripleDESCryptoServiceProvider Des3 = null;

            if (text != null || text.Length != 0)
            {
                HashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                //Key = "BinEnPassCoder";
                HashKey = HashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                Des3 = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
                Des3.Key = HashKey;
                Des3.Mode = System.Security.Cryptography.CipherMode.ECB;

                if (cryptType == EnDeCryptor.EnCrypt)
                {
                    //CONVERT TO ARRAY BYTE
                    buff = ASCIIEncoding.ASCII.GetBytes(text);

                    //ENCRYPT
                    EncryptString = Convert.ToBase64String(Des3.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
                    var renString = BinToHex(EncryptString);

                    return renString;
                }
                else
                {
                    var sTmp = HexToStr(text);
                    buff = Convert.FromBase64String(sTmp);
                    DecryptString = ASCIIEncoding.ASCII.GetString(Des3.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));

                    return DecryptString;
                }
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        ///     The function is used to convert Binary to HexaDecimal String
        /// </summary>
        /// <param name="strVal">A binary value string input</param>
        /// <returns>Converted HexaDecimal Value as a string</returns>
        public static string BinToHex(string strVal)
        {
            var strRetVal = "";
            var nLength = strVal.Length;

            for (var i = 0; i < nLength; i++)
            {
                var strTemp = Uri.HexEscape(strVal[i]);
                strRetVal += strTemp.Substring(1);
            }

            return strRetVal;
        }

        /// <summary>
        ///     The function is used to convert HexaDecimal value into String
        /// </summary>
        /// <param name="strHex">Input HexaDecimal string</param>
        /// <returns>Converted String Value of current input HexaDecimal Value</returns>
        public static string HexToStr(string strHex)
        {
            var strRet = "";
            var nCount = strHex.Length / 2;
            var iIdx = 0;

            for (var i = 0; i < nCount; i++)
            {
                iIdx = 0;
                var strTemp = "%" + strHex.Substring(i * 2, 2);
                strRet += Uri.HexUnescape(strTemp, ref iIdx);
            }

            return strRet;
        }

    }

}
