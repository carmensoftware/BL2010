using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.User
{
    public partial class HomePage : BaseUserControl
    {
        #region Attributes

        private readonly Blue.BL.dbo.User user = new Blue.BL.dbo.User();
        private DataSet dsUser = new DataSet();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsUser = (DataSet) Session["dsUser"];
            }
        }

        private void Page_Retrieve()
        {
            dsUser.Clear();

            // Get User data.
            var getUser = user.Get(dsUser, LoginInfo.LoginName);

            if (!getUser)
            {
                // Display Error Message
            }
            Session["dsUser"] = dsUser;
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            var drInsert = dsUser.Tables[user.TableName].Rows[0];

            drInsert["HomePage"] = txt_HomePage.Text;

            var save = user.Save(dsUser);

            if (save)
            {
                pop_HomePage.ShowOnPageLoad = true;
                txt_HomePage.Text = string.Empty;
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            txt_HomePage.Text = string.Empty;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_HomePage.ShowOnPageLoad = false;
        }
    }
}