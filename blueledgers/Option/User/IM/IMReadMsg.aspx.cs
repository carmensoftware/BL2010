using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IM
{
    public partial class IMReadMsg : BasePage
    {
        #region "Attributies"

        private readonly Blue.BL.IM.IMDelete delete = new Blue.BL.IM.IMDelete();
        private readonly DataSet dsReceiptList = new DataSet();
        private readonly Blue.BL.IM.IMInbox inbox = new Blue.BL.IM.IMInbox();
        private readonly Blue.BL.IM.IMSent sent = new Blue.BL.IM.IMSent();
        private Blue.BL.GnxLib gnxlib = new Blue.BL.GnxLib();

        #endregion

        #region "Operations"

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
            lbl_Title.Text = "Internal Message";
            switch (Request.Params["MODE"].ToUpper())
            {
                case "INBOX":
                    inbox.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drInboxList = dsReceiptList.Tables[inbox.TableName].Rows[0];

                    lbl_From.Text = drInboxList["Sender"].ToString();
                    lbl_To.Text = drInboxList["Reciever"].ToString();
                    lbl_Subject.Text = drInboxList["Subject"].ToString();
                    lbl_CC.Text = drInboxList["CC"].ToString();
                    if (drInboxList["Importance"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Importance.Checked = true;
                    }
                    if (drInboxList["Request"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Request.Checked = true;
                    }

                    //html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drInboxList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);
                    html_Msg.Visible = false;
                    lbl_Message.Text = Blue.BL.GnxLib.EnDecryptString(drInboxList["Message"].ToString(),
                        Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                    //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drInboxList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);
                    //lbl_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drInboxList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);

                    break;

                case "SENT":

                    sent.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drSentList = dsReceiptList.Tables[sent.TableName].Rows[0];

                    lbl_From.Text = drSentList["Sender"].ToString();
                    lbl_To.Text = drSentList["Reciever"].ToString();
                    lbl_Subject.Text = drSentList["Subject"].ToString();
                    lbl_CC.Text = drSentList["CC"].ToString();
                    if (drSentList["Importance"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Importance.Checked = true;
                    }
                    if (drSentList["Request"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Request.Checked = true;
                    }

                    html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drSentList["Message"].ToString(),
                        Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                    html_Msg.Visible = true;
                    //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drSentList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);
                    //lbl_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drSentList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);

                    break;

                case "DELETE":

                    delete.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);

                    var drDeleteList = dsReceiptList.Tables[delete.TableName].Rows[0];

                    lbl_From.Text = drDeleteList["Sender"].ToString();
                    lbl_To.Text = drDeleteList["Reciever"].ToString();
                    lbl_Subject.Text = drDeleteList["Subject"].ToString();
                    lbl_CC.Text = drDeleteList["CC"].ToString();
                    if (drDeleteList["Importance"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Importance.Checked = true;
                    }
                    if (drDeleteList["Request"].ToString().ToUpper() == "TRUE")
                    {
                        chk_Request.Checked = true;
                    }

                    html_Msg.Html = Blue.BL.GnxLib.EnDecryptString(drDeleteList["Message"].ToString(),
                        Blue.BL.GnxLib.EnDeCryptor.DeCrypt);
                    html_Msg.Visible = true;
                    //txt_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drDeleteList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);
                    //lbl_Msg.Text = Blue.BL.GnxLib.EnDecryptString(drDeleteList["Message"].ToString(),
                    //GnxLib.EnDeCryptor.DeCrypt);

                    break;
            }
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
                if (Request.Params["MODE"].ToUpper() == "INBOX")
                {
                    inbox.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drUpdate = dsReceiptList.Tables[inbox.TableName].Rows[0];
                    drUpdate["IsRead"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);
                }
                else if (Request.Params["MODE"].ToUpper() == "DELETE")
                {
                    delete.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drUpdate = dsReceiptList.Tables[delete.TableName].Rows[0];
                    drUpdate["IsRead"] = true;
                    delete.Save(dsReceiptList, LoginInfo.ConnStr);
                }
                Page_Retrieve();
                Page_Setting();
            }
        }

        private void Forward()
        {
            switch (Request.Params["MODE"].ToUpper())
            {
                case "INBOX":

                    inbox.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drInboxUpdate = dsReceiptList.Tables[inbox.TableName].Rows[0];
                    drInboxUpdate["IsForward"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "SENT":

                    sent.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drSentUpdate = dsReceiptList.Tables[sent.TableName].Rows[0];
                    drSentUpdate["IsForward"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "DELETE":

                    delete.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drDelUpdate = dsReceiptList.Tables[delete.TableName].Rows[0];
                    drDelUpdate["IsForward"] = true;
                    delete.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;
            }

            Response.Redirect("IMSendMsg.aspx?MODE=" + Request.Params["MODE"] + "&ID=" + Request.Params["ID"] +
                              "&Type=F");
        }

        private void ReplyAll()
        {
            switch (Request.Params["MODE"].ToUpper())
            {
                case "INBOX":

                    inbox.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drInboxUpdate = dsReceiptList.Tables[inbox.TableName].Rows[0];
                    drInboxUpdate["IsReply"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "SENT":

                    sent.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drSentUpdate = dsReceiptList.Tables[sent.TableName].Rows[0];
                    drSentUpdate["IsReply"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "DELETE":

                    delete.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drDelUpdate = dsReceiptList.Tables[delete.TableName].Rows[0];
                    drDelUpdate["IsReply"] = true;
                    delete.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;
            }

            Response.Redirect("IMSendMsg.aspx?MODE=" + Request.Params["MODE"] + "&ID=" + Request.Params["ID"] +
                              "&Type=ReA");
        }

        private void Reply()
        {
            switch (Request.Params["MODE"].ToUpper())
            {
                case "INBOX":

                    inbox.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drInboxUpdate = dsReceiptList.Tables[inbox.TableName].Rows[0];
                    drInboxUpdate["IsReply"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "SENT":

                    sent.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drSentUpdate = dsReceiptList.Tables[sent.TableName].Rows[0];
                    drSentUpdate["IsReply"] = true;
                    inbox.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;

                case "DELETE":

                    delete.GetMessage(dsReceiptList, Request.Params["ID"], LoginInfo.ConnStr);
                    var drDelUpdate = dsReceiptList.Tables[delete.TableName].Rows[0];
                    drDelUpdate["IsReply"] = true;
                    delete.Save(dsReceiptList, LoginInfo.ConnStr);

                    break;
            }

            Response.Redirect("IMSendMsg.aspx?MODE=" + Request.Params["MODE"] + "&ID=" + Request.Params["ID"] +
                              "&Type=Re");
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "REPLY":
                    Reply();
                    break;
                case "REPLY ALL":
                    ReplyAll();
                    break;
                case "FORWARD":
                    Forward();
                    break;
                case "BACK":
                    Response.Redirect("~/Option/User/Default.aspx");
                    break;
            }

            switch (e.Item.Name.ToUpper())
            {
                case "REPLY":
                    Reply();
                    break;
                case "REPLY ALL":
                    ReplyAll();
                    break;
                case "FORWARD":
                    Forward();
                    break;
                case "BACK":
                    Response.Redirect("~/Option/User/Default.aspx");
                    break;
            }
        }

        #endregion
    }
}