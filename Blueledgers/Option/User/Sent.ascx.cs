using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.User
{
    public partial class Sent : BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.IM.IMDelete delete = new Blue.BL.IM.IMDelete();
        private readonly DataSet dsDelete = new DataSet();
        private readonly DataSet dsSent = new DataSet();
        private readonly Blue.BL.IM.IMSent sent = new Blue.BL.IM.IMSent();

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_ConnStr.Value = LoginInfo.ConnStr;
            grd_Sent.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void btn_ConfrimDelete_Click(object sender, EventArgs e)
        {
            var GetSentList = sent.GetList(dsSent, LoginInfo.ConnStr);

            if (GetSentList)
            {
                for (var i = grd_Sent.Rows.Count - 1; i >= 0; i--)
                {
                    var Chk_Item = (CheckBox) grd_Sent.Rows[i].Cells[0].FindControl("Chk_Item");

                    if (Chk_Item.Checked)
                    {
                        // Retreive the Sent No.
                        var sentno = Convert.ToInt32(grd_Sent.DataKeys[grd_Sent.Rows[i].RowIndex].Value);

                        foreach (DataRow drDeleting in dsSent.Tables[sent.TableName].Rows)
                        {
                            if (drDeleting.RowState != DataRowState.Deleted)
                            {
                                if (drDeleting["SentNo"].ToString().ToUpper() == sentno.ToString().ToUpper())
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
                                        drInserting["IsRead"] = true;
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

            var inboxsave = sent.Save(dsSent, LoginInfo.ConnStr);

            if (inboxsave)
            {
                pop_ConfrimDelete.ShowOnPageLoad = false;
                grd_Sent.DataBind();
            }
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_Sent.Selection.UnselectAll();
            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void grd_Sent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (var i = 1; i <= grd_Sent.Columns.Count - 1; i++)
                {
                    // Add event click to redirect to detail page.
                    e.Row.Cells[i].Attributes.Add("OnClick",
                        "document.location.href='IM/IMReadMsg.aspx?MODE=Sent&ID=" +
                        DataBinder.Eval(e.Row.DataItem, "SentNo") + "'");
                    e.Row.Cells[i].Style.Add("cursor", "hand");
                }
            }
        }

        protected void grd_Sent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Sent.PageIndex = e.NewPageIndex;
            grd_Sent.DataBind();
        }
    }
}