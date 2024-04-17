using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace ReportViewer
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            Main.isLogin = false;
            SqlConnection connection = new SqlConnection(Main.sysDB_ConnStr);

            using (connection)
            {

                string sql = string.Format("SELECT * FROM [dbo].[User] WHERE IsActived = 1 AND LoginName = '{0}'", txt_Username.Text);

                SqlDataAdapter adapterUser = new SqlDataAdapter(sql, connection);
                connection.Open();
                DataSet dsUser = new DataSet();
                adapterUser.Fill(dsUser);
                DataTable dt = dsUser.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    string username = dt.Rows[0]["LoginName"].ToString();
                    string password = dt.Rows[0]["Password"].ToString();
                    password = GnxLib.EnDecryptString(password, GnxLib.EnDeCryptor.DeCrypt, "blueledgers.com");

                    if (password == txt_Password.Text)
                    {
                        Main.loginName = txt_Username.Text;
                        Main.isLogin = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Invalid user/password.", "Login fail!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("User not found or inactive", "Login fail!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                connection.Close();
            }


        }

    }
}
