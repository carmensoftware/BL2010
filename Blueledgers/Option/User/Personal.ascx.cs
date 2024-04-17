using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxUploadControl;

namespace BlueLedger.PL.Option.User
{
    public partial class Personal : BaseUserControl
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

            Page_Setting();
        }

        private void Page_Retrieve()
        {
            var result = user.Get(dsUser, LoginInfo.LoginName);

            if (result)
            {
                Session["dsUser"] = dsUser;
            }
            else
            {
            }
        }

        private void Page_Setting()
        {
            var drUser = dsUser.Tables[user.TableName].Rows[0];

            if (drUser["Signature"] != null)
            {
                img_Signature.Visible = true;
                img_Signature.Value = drUser["Signature"];
                img_Signature.DataBind();
            }
        }

        protected void upl_Signature_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            e.CallbackData = SavePostFile(e.UploadedFile);
        }

        protected string SavePostFile(UploadedFile uploadedFile)
        {
            if (uploadedFile.IsValid)
            {
                Page_Retrieve();

                var drUser = dsUser.Tables[user.TableName].Rows[0];
                drUser["Signature"] = uploadedFile.FileBytes;

                var save = user.Save(dsUser);

                if (save)
                {
                    Page_Setting();
                }
            }

            return uploadedFile.FileName;
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
        }
    }
}