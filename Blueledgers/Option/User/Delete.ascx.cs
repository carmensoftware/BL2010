using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.User
{
    public partial class Delete : BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.IM.IMDelete delete = new Blue.BL.IM.IMDelete();
        private readonly DataSet dsDelete = new DataSet();

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_LoginName.Value = LoginInfo.LoginName;
            hf_ConnStr.Value = LoginInfo.ConnStr;
            grd_Delete.DataBind();
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
            var GetDeleteList = delete.GetList(dsDelete, LoginInfo.ConnStr);

            if (GetDeleteList)
            {
                foreach (GridViewRow grd_Row in grd_Delete.Rows)
                {
                    var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                    if (chk_Item.Checked)
                    {
                        var DelNo = int.Parse(grd_Delete.DataKeys[grd_Row.RowIndex].Value.ToString());

                        for (var i = grd_Delete.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dsDelete.Tables[delete.TableName].Rows[i].RowState != DataRowState.Deleted)
                            {
                                if (int.Parse(dsDelete.Tables[delete.TableName].Rows[i]["DeleteNo"].ToString()) == DelNo)
                                {
                                    dsDelete.Tables[delete.TableName].Rows[i].Delete();
                                }
                            }
                        }
                    }
                }
            }

            var save = delete.Save(dsDelete, LoginInfo.ConnStr);
            if (save)
            {
                //grd_Delete.Selection.UnselectAll();
                //foreach (GridViewRow grd_Row in grd_Delete.Rows)
                //{
                //    CheckBox chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                //    if (chk_Item.Checked)
                //    {
                //        chk_Item.Checked = false;
                //    }
                //}

                pop_ConfrimDelete.ShowOnPageLoad = false;
            }

            grd_Delete.DataBind();
        }

        protected void btn_CancelDelete_Click(object sender, EventArgs e)
        {
            //grd_Delete.Selection.UnselectAll();
            foreach (GridViewRow grd_Row in grd_Delete.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    chk_Item.Checked = false;
                }
            }

            pop_ConfrimDelete.ShowOnPageLoad = false;
        }

        protected void grd_Delete_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (var i = 1; i <= grd_Delete.Columns.Count - 1; i++)
                {
                    // Add event click to redirect to detail page.
                    e.Row.Cells[i].Attributes.Add("OnClick", "document.location.href='IM/IMReadMsg.aspx?MODE=Delete&ID="
                                                             + DataBinder.Eval(e.Row.DataItem, "DeleteNo") + "'");
                    e.Row.Cells[i].Style.Add("cursor", "hand");
                }
            }
        }
    }
}