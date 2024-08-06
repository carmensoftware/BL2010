using System;
using System.Data;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.CA
{
    public partial class CAEdit : BasePage
    {
        private readonly Blue.BL.dbo.Bu bu = new Blue.BL.dbo.Bu();
        private readonly Blue.BL.IN.CA ca = new Blue.BL.IN.CA();
        private readonly Blue.BL.IN.CADt cadt = new Blue.BL.IN.CADt();

        private DataSet dsCA = new DataSet();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.Params["BuCode"] != null)
            {
                hf_ConnStr.Value = bu.GetConnectionString(Request.Params["BuCode"]);
            }
            else
            {
                hf_ConnStr.Value = LoginInfo.ConnStr;
            }

            hf_LoginName.Value = LoginInfo.LoginName;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Setting();
            }
            else
            {
                dsCA = (DataSet) Session["dsCA"];
            }
        }

        private void Page_Setting()
        {
            var getData = Page_Retrieve();

            if (getData)
            {
                var drCA = dsCA.Tables[ca.TableName].Rows[0];

                // Display Header Data
                ddl_Store.Value = drCA["FromStore"].ToString();
                lbl_Date.Text = String.Format("{0:dd/MM/yyyy HH:mm}", drCA["Date"]);
                lbl_Status.Text = drCA["Status"].ToString();
                txt_Desc.Text = drCA["Description"].ToString();

                // Display Detail Data
                grd_CADt.DataSource = dsCA.Tables[cadt.TableName];
                grd_CADt.DataBind();
            }
        }

        private bool Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                var getCASchema = ca.GetSchema(dsCA, hf_ConnStr.Value);

                if (getCASchema)
                {
                    // Insert new row
                    var drNew = dsCA.Tables[ca.TableName].NewRow();
                    drNew["RefNo"] = ca.GetNewID(hf_ConnStr.Value);
                    drNew["Date"] = ServerDateTime;
                    drNew["FromStore"] = string.Empty;
                    drNew["Status"] = "New";
                    drNew["CreatedDate"] = ServerDateTime;
                    drNew["CreatedBy"] = LoginInfo.LoginName;
                    drNew["UpdatedDate"] = ServerDateTime;
                    drNew["UpdatedBy"] = LoginInfo.LoginName;
                    dsCA.Tables[ca.TableName].Rows.Add(drNew);
                }
                else
                {
                    // Display Error Message
                    return false;
                }

                var getCADtSchema = cadt.GetSchema(dsCA, hf_ConnStr.Value);

                if (!getCADtSchema)
                {
                    // Display Error Message
                    return false;
                }
            }

            Session["dsCA"] = dsCA;

            return true;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Save();
                    break;

                case "BACK":
                    Back();
                    break;
            }
        }

        private void Save()
        {
        }

        private void Back()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                Response.Redirect("CALst.aspx");
            }
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            // Disable Save Button
            menu_CmdBar.Items[0].Enabled = false;

            // Insert new row
            //DataRow drNew = dsTrfEdit.Tables[trfDt.TableName].NewRow();
            //drNew["DeliveryDate"] = de_ReqDate.Date;

            //if (dsTrfEdit.Tables[trfDt.TableName].Rows.Count > 0)
            //{
            //    drNew["ToLocationCode"] = dsTrfEdit.Tables[trfDt.TableName].Rows[dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1]["ToLocationCode"].ToString();
            //}

            //dsTrfEdit.Tables[trfDt.TableName].Rows.Add(drNew);

            ////Session["dsStoreReqEdit"] = dsStoreReqEdit;

            //grd_TrfEdit.DataSource = dsTrfEdit.Tables[trfDt.TableName];
            //grd_TrfEdit.EditIndex = dsTrfEdit.Tables[trfDt.TableName].Rows.Count - 1;
            //grd_TrfEdit.DataBind();

            //this.TrfEditMode = "New";
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
        }

        protected void grd_CADt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void grd_CADt_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void grd_CADt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
        }

        protected void grd_CADt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }

        protected void grd_CADt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void grd_CADt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}