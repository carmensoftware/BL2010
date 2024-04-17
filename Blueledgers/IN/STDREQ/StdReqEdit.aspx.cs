using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.STDREQ
{
    public partial class StdReqEdit : BasePage
    {
        private Blue.BL.IN.StandardRequistion StdReq = new Blue.BL.IN.StandardRequistion();
        private Blue.BL.IN.StandardRequisitionDetail StdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        private DataSet dsStdReq = new DataSet();
        private Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct strLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                dsStdReq = (DataSet)Session["dsStdReq"];
            }
            else
            {
                dsStdReq = (DataSet)Session["dsStdReq"];
            }
        }

        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            {
                var result = StdReq.GetSchema(dsStdReq, LoginInfo.ConnStr);

                if (!result)
                {
                    return;
                }

                var resultDetail = StdReqDt.GetSchema(dsStdReq, LoginInfo.ConnStr);

                if (!resultDetail)
                {
                    return;
                }
            }
            else
            {
                var result = StdReq.Get(dsStdReq, int.Parse(Request.Params["ID"].ToString()), LoginInfo.ConnStr);

                if (!result)
                {
                    return;
                }

                var resultDt = StdReqDt.Get(dsStdReq, int.Parse(Request.Params["ID"].ToString()), LoginInfo.ConnStr);

                if (!resultDt)
                {
                    return;
                }
            }

            Session["dsStdReq"] = dsStdReq;

            Page_Setting();
        }

        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
            hf_LoginName.Value = LoginInfo.LoginName;

            //DataRow drStdReq = dsStdReq.Tables[StdReq.TableName].Rows[0];

            if (Request.Params["MODE"].ToString().ToUpper() == "EDIT")
            {
                var drStdReq = dsStdReq.Tables[StdReq.TableName].Rows[0];

                lbl_RefID.Text = drStdReq["RefId"].ToString();
                ddl_StoreLocation.Value = drStdReq["LocationCode"].ToString();
                txt_Desc.Text = drStdReq["Description"].ToString();
                chk_Status.Checked = bool.Parse(drStdReq["Status"].ToString());

                //Tree View.
                tl_StdReqEdit.DataSource = template.GetCateByLocationCode(drStdReq["LocationCode"].ToString(),
                    LoginInfo.ConnStr);
                tl_StdReqEdit.DataBind();

                //Assign Selected Items
                foreach (
                    DataRow drStdDt in StdReqDt.GetListByRefId(drStdReq["RefId"].ToString(), LoginInfo.ConnStr).Rows)
                {
                    if (tl_StdReqEdit.FindNodeByKeyValue(drStdDt["ProductCode"].ToString()) != null)
                    {
                        tl_StdReqEdit.FindNodeByKeyValue(drStdDt["ProductCode"].ToString()).Selected = true;
                    }
                }
            }
            else if (Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                var drStdReq = dsStdReq.Tables[StdReq.TableName].Rows[0];

                lbl_RefID.Enabled = false;
                chk_Status.Enabled = true;
                ddl_StoreLocation.Value = drStdReq["LocationCode"].ToString();
                txt_Desc.Text = drStdReq["Description"].ToString();
                chk_Status.Checked = bool.Parse(drStdReq["Status"].ToString());

                //Tree View.
                tl_StdReqEdit.DataSource = template.GetCateByLocationCode(drStdReq["LocationCode"].ToString(),
                    LoginInfo.ConnStr);
                tl_StdReqEdit.DataBind();

                //Assign Selected Items
                foreach (
                    DataRow drStdDt in StdReqDt.GetListByRefId(drStdReq["RefId"].ToString(), LoginInfo.ConnStr).Rows)
                {
                    if (tl_StdReqEdit.FindNodeByKeyValue(drStdDt["ProductCode"].ToString()) != null)
                    {
                        tl_StdReqEdit.FindNodeByKeyValue(drStdDt["ProductCode"].ToString()).Selected = true;
                    }
                }
            }
            else
            {
                //chk_Status.Checked  = bool.Parse(drStdReq["Status"].ToString());

                chk_Status.Checked = true;

                lbl_RefID.Enabled = false;
                tr_Header1.Visible = false;
                //tr_Header2.Visible = false;
            }
        }

        // List Store Location.
        protected void ddl_StoreLocation_Load(object sender, EventArgs e)
        {
            ddl_StoreLocation.DataSource = strLct.GetList(hf_LoginName.Value, hf_ConnStr.Value);
            ddl_StoreLocation.DataBind();
        }

        // Create Tree View.
        protected void btn_Create_Assign_Click(object sender, EventArgs e)
        {
        }

        protected void tl_StdReqEdit_Load(object sender, EventArgs e)
        {
            if (hf_TreeView.Value == "true")
            {
                if (ddl_StoreLocation.Value != null)
                {
                    tl_StdReqEdit.DataSource = template.GetCateByLocationCode(ddl_StoreLocation.Value.ToString(),
                        LoginInfo.ConnStr);
                    tl_StdReqEdit.DataBind();
                }
            }
            else if (Request.Params["MODE"].ToString().ToUpper() == "EDIT" ||
                     Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                if (ddl_StoreLocation.Value != null)
                {
                    tl_StdReqEdit.DataSource = template.GetCateByLocationCode(ddl_StoreLocation.Value.ToString(),
                        LoginInfo.ConnStr);
                    tl_StdReqEdit.DataBind();
                }
            }
        }

        // Pop Up Button.
        protected void btn_ChkDetail_OK_Click(object sender, EventArgs e)
        {
            pop_ChkDetail.ShowOnPageLoad = false;
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
            DataRow drSave = null;
            var countNode = 0;

            // Find Node in Tree.
            var selectNodes = tl_StdReqEdit.GetSelectedNodes();

            // Count Item.
            for (var i = 0; i < selectNodes.Count; i++)
            {
                if (selectNodes[i].Level == 4)
                {
                    countNode++;
                }
            }

            if (countNode == 0)
            {
                pop_ChkDetail.ShowOnPageLoad = true;
                return;
            }
            else if (Request.Params["MODE"].ToString().ToUpper() == "NEW" ||
                     Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                drSave = dsStdReq.Tables[StdReq.TableName].NewRow();
                //Get New ID.
                drSave["RefId"] = StdReq.GetNewID(LoginInfo.ConnStr);
                drSave["CreatedDate"] = ServerDateTime;
                drSave["CreatedBy"] = LoginInfo.LoginName;
            }
            else
            {
                drSave = dsStdReq.Tables[StdReq.TableName].Rows[0];

                // Delete all Prodduct in TreeView. 
                foreach (DataRow drTreeStoreCheck in dsStdReq.Tables[StdReqDt.TableName].Rows)
                {
                    if (drTreeStoreCheck.RowState != DataRowState.Deleted)
                    {
                        drTreeStoreCheck.Delete();
                    }
                }
            }

            // Assign Item.
            for (var i = 0; i < selectNodes.Count; i++)
            {
                if (selectNodes[i].Level == 4)
                {
                    var drNewStdDtCheck = dsStdReq.Tables[StdReqDt.TableName].NewRow();
                    drNewStdDtCheck["RefId"] = StdReqDt.GetNewID(LoginInfo.ConnStr);
                    drNewStdDtCheck["DocumentId"] = drSave["RefId"].ToString();
                    drNewStdDtCheck["ProductCode"] = selectNodes[i].Key.ToString();
                    drNewStdDtCheck["CategoryCode"] =
                        product.GetProductCategory(drNewStdDtCheck["ProductCode"].ToString(), LoginInfo.ConnStr);
                    //drNewStdDtCheck["RequestUnit"] = product.GetOrderUnit(drNewStdDtCheck["ProductCode"].ToString(), LoginInfo.ConnStr);

                    //drNewStdDtCheck["RequestUnit"] = prodUnit.GetInvenUnit(drNewStdDtCheck["ProductCode"].ToString(), LoginInfo.ConnStr);
                    drNewStdDtCheck["RequestUnit"] = product.GetProdList(drNewStdDtCheck["ProductCode"].ToString(), LoginInfo.ConnStr).Rows[0]["InventoryUnit"].ToString();
                    dsStdReq.Tables[StdReqDt.TableName].Rows.Add(drNewStdDtCheck);
                }
            }

            drSave["LocationCode"] = ddl_StoreLocation.Value.ToString();
            drSave["Description"] = txt_Desc.Text;
            drSave["Status"] = chk_Status.Checked;
            drSave["UpdatedDate"] = ServerDateTime;
            drSave["UpdatedBy"] = LoginInfo.LoginName;

            if (Request.Params["MODE"].ToString().ToUpper() == "NEW"
                || Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                dsStdReq.Tables[StdReq.TableName].Rows.Add(drSave);
            }

            var save = StdReq.Save(dsStdReq, hf_ConnStr.Value);

            if (save)
            {
                Session["RefId"] = drSave["RefId"];
                pop_Save.ShowOnPageLoad = true;
                //Response.Redirect("StdReqDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() + "&ID=" + drSave["RefId"].ToString() + "&VID=" + Request.Params["VID"].ToString());
            }
        }

        private void Back()
        {
            if (Request.Params["MODE"] == "EDIT")
            {
                Response.Redirect("StdReqDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() + "&MODE=EDIT&ID=" +
                                  Request.Params["ID"].ToString() +
                                  "&VID=" + Request.Params["VID"].ToString());
            }
            else
            {
                Response.Redirect("StdReqLst.aspx");
            }
        }

        protected void menu_CmdGrd_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CREATE":
                    if (ddl_StoreLocation.Value != null)
                    {
                        tl_StdReqEdit.DataSource = template.GetCateByLocationCode(ddl_StoreLocation.Value.ToString(),
                            LoginInfo.ConnStr);
                        tl_StdReqEdit.DataBind();
                    }
                    else
                    {
                        pop_ChkStore.ShowOnPageLoad = true;
                    }

                    // Show Assign after Click Create Button
                    hf_TreeView.Value = "true";

                    break;
            }
        }

        protected void btn_ChkStore_OK_Click(object sender, EventArgs e)
        {
            pop_ChkStore.ShowOnPageLoad = false;
            return;
        }

        protected void btn_Save_Success_Click(object sender, EventArgs e)
        {
            pop_Save.ShowOnPageLoad = false;
            var RefId = Session["RefId"].ToString();
            Response.Redirect("StdReqDt.aspx?BuCode=" + Request.Params["BuCode"].ToString() + "&ID=" + RefId + "&VID=" +
                              Request.Params["VID"].ToString());
        }
    }
}