using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IM
{
    public partial class IMSendMsg : BasePage
    {
        #region "Attributies"

        private readonly DataSet dsSendItem = new DataSet();
        private readonly Blue.BL.IM.IMDelete imDelete = new Blue.BL.IM.IMDelete();
        private readonly Blue.BL.IM.IMInbox imInbox = new Blue.BL.IM.IMInbox();
        private readonly Blue.BL.IM.IMSent imSent = new Blue.BL.IM.IMSent();
        private Blue.BL.Application.Field field = new Blue.BL.Application.Field();
        private Blue.BL.GnxLib gnxLib = new Blue.BL.GnxLib();
        private Blue.BL.ProjectAdmin.Menu menu = new Blue.BL.ProjectAdmin.Menu();

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_BuGrpCode.Value = LoginInfo.BuInfo.BuGrpCode;
        }

        /// <summary>
        ///     Get data from database
        /// </summary>
        private void Page_Retrieve()
        {
        }

        /// <summary>
        ///     Binding controls
        /// </summary>
        private void Page_Setting()
        {
            if (Request.Params["MODE"] != null)
            {
                switch (Request.Params["MODE"].ToUpper())
                {
                    case "INBOX":
                        imInbox.GetMessage(dsSendItem, Request.Params["ID"], LoginInfo.ConnStr);
                        var drInbox = dsSendItem.Tables[imInbox.TableName].Rows[0];
                        txt_Subject.Text = drInbox["Subject"].ToString();
                        //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drInbox["Message"].ToString(), Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drInbox["Message"].ToString(),
                            Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        if (drInbox["Importance"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Importance.Checked = true;
                        }
                        if (drInbox["Request"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Request.Checked = true;
                        }
                        if (Request.Params["Type"].ToUpper() == "RE")
                        {
                            ddl_ChooseEmail.Text = drInbox["Sender"].ToString();
                        }
                        else if (Request.Params["Type"].ToUpper() == "REA")
                        {
                            ddl_ChooseEmail.Text = drInbox["Sender"].ToString();
                            ddl_ChooseEmail2.Text = drInbox["CC"].ToString();
                        }
                        break;
                    case "DELETE":
                        imDelete.GetMessage(dsSendItem, Request.Params["ID"], LoginInfo.ConnStr);
                        var drDelete = dsSendItem.Tables[imDelete.TableName].Rows[0];
                        txt_Subject.Text = drDelete["Subject"].ToString();
                        //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drDelete["Message"].ToString(), Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drDelete["Message"].ToString(),
                            Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        if (drDelete["Importance"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Importance.Checked = true;
                        }
                        if (drDelete["Request"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Request.Checked = true;
                        }
                        if (Request.Params["Type"].ToUpper() == "RE")
                        {
                            ddl_ChooseEmail.Text = drDelete["Sender"].ToString();
                        }
                        else if (Request.Params["Type"].ToUpper() == "REA")
                        {
                            ddl_ChooseEmail.Text = drDelete["Sender"].ToString();
                            ddl_ChooseEmail2.Text = drDelete["CC"].ToString();
                        }
                        break;
                    case "SENT":
                        imSent.GetMessage(dsSendItem, Request.Params["ID"], LoginInfo.ConnStr);
                        var drSent = dsSendItem.Tables[imSent.TableName].Rows[0];
                        txt_Subject.Text = drSent["Subject"].ToString();
                        //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drSent["Message"].ToString(), Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drSent["Message"].ToString(),
                            Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                        if (drSent["Importance"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Importance.Checked = true;
                        }
                        if (drSent["Request"].ToString().ToUpper() == "TRUE")
                        {
                            chk_Request.Checked = true;
                        }
                        if (Request.Params["Type"].ToUpper() == "RE")
                        {
                            ddl_ChooseEmail.Value = drSent["Sender"].ToString();
                        }
                        else if (Request.Params["Type"].ToUpper() == "REA")
                        {
                            ddl_ChooseEmail.Text = drSent["Sender"].ToString();
                            ddl_ChooseEmail2.Text = drSent["CC"].ToString();
                        }
                        break;
                }
            }

            lbl_From.Text = LoginInfo.LoginName;
        }

        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Checking login session 
            base.Page_Load(sender, e);

            // Retrieve data and binding with controls
            if (!IsPostBack)
            {
                Page_Retrieve();
                Page_Setting();
            }
        }

        //protected void lnkb_Send_Click(object sender, EventArgs e)
        //{
        //    bool Save = imInbox.SendMessage(LoginInfo.LoginName, ddl_ChooseEmail.Text, ddl_ChooseEmail2.Text, txt_Subject.Text, ServerDateTime, chk_Importance.Checked, chk_Request.Checked, txt_Msg.Text, LoginInfo.ConnStr);
        //    if(Save)
        //    {
        //            Response.Redirect("~/Option/User/Default.aspx");
        //    }
        //}

        //protected void lnkb_Back_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Option/User/Default.aspx");
        //}

        //protected void lnkb_Cancel_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Option/User/Default.aspx");
        //}

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SEND":
                    if (ddl_ChooseEmail.Value != null)
                    {
                        var Save = imInbox.SendMessage(LoginInfo.LoginName, ddl_ChooseEmail.Text, ddl_ChooseEmail2.Text,
                            txt_Subject.Text, ServerDateTime, chk_Importance.Checked, chk_Request.Checked, html_Msg.Html,
                            LoginInfo.ConnStr);
                        if (Save)
                        {
                            //txt_Msg.Text
                            Response.Redirect("~/Option/User/Default.aspx");
                        }
                    }
                    break;

                default:
                    if (Request.Params["Type"] != null)
                    {
                        Response.Redirect("~/Option/User/IM/IMReadMsg.aspx?MODE=" + Request.Params["MODE"] + "&ID=" +
                                          Request.Params["ID"]);
                    }
                    else
                    {
                        Response.Redirect("~/Option/User/Default.aspx");
                    }
                    break;
            }
            //if (e.Item.Text.ToUpper() == "SEND")
            //{
            //    if (ddl_ChooseEmail.Value != null)
            //    {
            //        bool Save = imInbox.SendMessage(LoginInfo.LoginName, ddl_ChooseEmail.Text, ddl_ChooseEmail2.Text, txt_Subject.Text, ServerDateTime, chk_Importance.Checked, chk_Request.Checked, txt_Msg.Text, LoginInfo.ConnStr);
            //        if (Save)
            //        {
            //            Response.Redirect("~/Option/User/Default.aspx");
            //        }
            //    }
            //}
            //else
            //{
            //    Response.Redirect("~/Option/User/Default.aspx");
            //}
        }

        #endregion

        protected void btn_ClearAll_Click(object sender, EventArgs e)
        {
            ddl_ChooseEmail.Value = null;
            ddl_ChooseEmail2.Value = null;
            txt_Subject.Text = "";
            html_Msg.Html = "";
            chk_Importance.Checked = false;
            chk_Request.Checked = false;
        }
    }
}