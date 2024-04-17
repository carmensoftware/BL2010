using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.User
{
    public partial class Inbox : BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.IM.IMDelete delete = new Blue.BL.IM.IMDelete();
        private readonly DataSet dsDelete = new DataSet();
        private readonly DataSet dsInbox = new DataSet();
        private readonly Blue.BL.IM.IMInbox inbox = new Blue.BL.IM.IMInbox();
        private DataSet dsTemp = new DataSet();

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_ConnStr.Value = LoginInfo.ConnStr;
            grd_Inbox.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grd_Inbox_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Inbox.PageIndex = e.NewPageIndex;
            grd_Inbox.DataBind();
        }

        protected void grd_Inbox_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add event click to redirect to detail page.
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (var i = 1; i <= grd_Inbox.Columns.Count - 1; i++)
                    {
                        // Add event click to redirect to detail page.
                        e.Row.Cells[i].Attributes.Add("OnClick",
                            "document.location.href='IM/IMReadMsg.aspx?MODE=Inbox&ID=" +
                            DataBinder.Eval(e.Row.DataItem, "InboxNo") + "'");
                        e.Row.Cells[i].Style.Add("cursor", "hand");
                    }
                }
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "NEW":
                    Response.Redirect("~/Option/User/IM/IMSendMsg.aspx");
                    break;

                case "DELETE":
                    pop_ConfrimDelete.ShowOnPageLoad = true;
                    break;
            }
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var GetInboxList = inbox.GetList(dsInbox, LoginInfo.ConnStr);

            if (GetInboxList)
            {
                for (var i = grd_Inbox.Rows.Count - 1; i >= 0; i--)
                {
                    var Chk_Item = (CheckBox) grd_Inbox.Rows[i].Cells[0].FindControl("Chk_Item");

                    if (Chk_Item.Checked)
                    {
                        // Retreive the Inbox No.
                        var inboxno = Convert.ToInt32(grd_Inbox.DataKeys[grd_Inbox.Rows[i].RowIndex].Value);

                        foreach (DataRow drDeleting in dsInbox.Tables[inbox.TableName].Rows)
                        {
                            if (drDeleting.RowState != DataRowState.Deleted)
                            {
                                if (drDeleting["InboxNo"].ToString().ToUpper() == inboxno.ToString().ToUpper())
                                {
                                    var GetDeleteList = delete.GetList(dsDelete, LoginInfo.ConnStr);

                                    if (GetDeleteList)
                                    {
                                        var drInserting = dsDelete.Tables[delete.TableName].NewRow();
                                        drInserting["DeleteNo"] = delete.GetNewNo(LoginInfo.ConnStr);
                                        drInserting["Sender"] = drDeleting["Sender"];
                                        drInserting["Reciever"] = drDeleting["Reciever"];
                                        drInserting["Subject"] = drDeleting["Subject"];
                                        drInserting["Date"] = ServerDateTime;
                                        drInserting["CC"] = drDeleting["CC"];
                                        drInserting["Importance"] = drDeleting["Importance"];
                                        drInserting["Request"] = drDeleting["Request"];
                                        drInserting["Message"] = drDeleting["Message"];
                                        drInserting["IsRead"] = drDeleting["IsRead"];
                                        drInserting["IsForward"] = drDeleting["IsForward"];
                                        drInserting["IsReply"] = drDeleting["IsReply"];
                                        drInserting["DeletedBy"] = LoginInfo.LoginName;
                                        dsDelete.Tables[delete.TableName].Rows.Add(drInserting);

                                        var delsave = delete.Save(dsDelete, LoginInfo.ConnStr);

                                        if (delsave)
                                        {
                                            drDeleting.Delete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var inboxsave = inbox.Save(dsInbox, LoginInfo.ConnStr);

            if (inboxsave)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;
                grd_Inbox.DataBind();

                //Session["dsInbox"] = dsInbox;
            }
        }

        #region "Delete Gridview Devexpress"

        //protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        //{ 
        //    bool GetInboxList = inbox.GetList(dsInbox,LoginInfo.ConnStr);

        //    if (GetInboxList)
        //    {
        //        foreach (int delInboxNo in grd_Inbox.GetSelectedFieldValues("InboxNo"))
        //        {
        //            foreach (DataRow drDeleting in dsInbox.Tables[inbox.TableName].Rows)
        //            {
        //                if (drDeleting.RowState != DataRowState.Deleted)
        //                {
        //                    if (drDeleting["InboxNo"].ToString().ToUpper() == delInboxNo.ToString().ToUpper())
        //                    {
        //                        bool GetDeleteList = delete.GetList(dsDelete, LoginInfo.ConnStr);

        //                        if (GetDeleteList)
        //                        {
        //                            DataRow drInserting = dsDelete.Tables[delete.TableName].NewRow();
        //                            drInserting["DeleteNo"] = delete.GetNewNo(LoginInfo.ConnStr);
        //                            drInserting["Sender"] = drDeleting["Sender"];
        //                            drInserting["Reciever"] = drDeleting["Reciever"];
        //                            drInserting["Subject"] = drDeleting["Subject"];
        //                            drInserting["Date"] = ServerDateTime;
        //                            drInserting["CC"] = drDeleting["CC"];
        //                            drInserting["Importance"] = drDeleting["Importance"];
        //                            drInserting["Request"] = drDeleting["Request"];
        //                            drInserting["Message"] = drDeleting["Message"];
        //                            drInserting["IsRead"] = drDeleting["IsRead"];
        //                            drInserting["IsForward"] = drDeleting["IsForward"];
        //                            drInserting["IsReply"] = drDeleting["IsReply"];
        //                            drInserting["DeletedBy"] = LoginInfo.LoginName;
        //                            dsDelete.Tables[delete.TableName].Rows.Add(drInserting);

        //                            bool delsave = delete.Save(dsDelete, LoginInfo.ConnStr);
        //                            if (delsave)
        //                            {
        //                                drDeleting.Delete();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    bool inboxsave = inbox.Save(dsInbox, LoginInfo.ConnStr);
        //    if (inboxsave)
        //    {
        //        grd_Inbox.Selection.UnselectAll();
        //        pop_ConfrimDelete.ShowOnPageLoad = false;
        //    }
        //    grd_Inbox.DataBind();
        //}

        //protected void btn_CancelDelete_Click(object sender, EventArgs e)
        //{
        //    grd_Inbox.Selection.UnselectAll();
        //    pop_ConfrimDelete.ShowOnPageLoad = false;
        //}

        #endregion
    }
}