using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using System.Collections.Generic;

namespace BlueLedger.PL.IN.MLT
{
    public partial class MLTEdit : BasePage
    {
        #region "Attributes"

        private string MsgError = string.Empty;
        private DataSet dsTemplateEdit = new DataSet();
        private Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();
        private Blue.BL.Option.Inventory.Unit unit = new Blue.BL.Option.Inventory.Unit();
        private Blue.BL.APP.Config conf = new Blue.BL.APP.Config();

        public string TemplateDetailMode
        {
            get
            {
                if (ViewState["TemplateDetailMode"] != null)
                {
                    return ViewState["TemplateDetailMode"].ToString();
                }

                return string.Empty;
            }
            set { ViewState["TemplateDetailMode"] = value; }
        }

        #endregion

        #region "Operations"

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
                dsTemplateEdit = (DataSet)Session["dsTemplateEdit"];
            }
            else
            {
                dsTemplateEdit = (DataSet)Session["dsTemplateEdit"];
            }
        }

        /// <summary>
        ///     Get business MLTEdit data related to login user.
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            {
                // Get Template Structure
                var getTemplateSchema = template.GetSchema(dsTemplateEdit, LoginInfo.ConnStr);

                if (!getTemplateSchema)
                {
                    return;
                }

                // Get Template Detail Structure
                var getTempalteDetailSchema = templateDt.GetSchema(dsTemplateEdit, LoginInfo.ConnStr);

                if (!getTempalteDetailSchema)
                {
                    return;
                }
            }
            else
            {
                var tmpNo = int.Parse(Request.Params["ID"].ToString());

                // Get Template Data
                var getTemplate = template.Get(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

                if (!getTemplate)
                {
                    return;
                }

                // Get template Detail Data
                var getTemplateDt = templateDt.GetListByTmpNo(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

                if (!getTemplateDt)
                {
                    return;
                }
            }

            Session["dsTemplateEdit"] = dsTemplateEdit;

            // Modified on: 11/02/2018, By: Fon
            //TemplateDetailMode = "1"; // 1 = Market List
            TemplateDetailMode = conf.GetValue("PC", "ML", "ProdCateType", LoginInfo.ConnStr);
            // End Modified.

            Page_Setting();
        }

        /// <summary>
        ///     Display business MLTEdit data which retrieved from Page_Retrieve procedure.
        /// </summary>
        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
            hf_LoginName.Value = LoginInfo.LoginName;

            // Page Title
            lbl_Title.Text = "Market List";

            if (Request.Params["MODE"].ToString().ToUpper() == "EDIT" ||
                Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                var drTemplate = dsTemplateEdit.Tables[template.TableName].Rows[0];

                if (Request.Params["MODE"].ToString().ToUpper() == "EDIT")
                {
                    txt_TemplateNo.Text = drTemplate["TmpNo"].ToString();
                }

                ddl_Store.Value = drTemplate["LocationCode"];
                txt_Description.Text = drTemplate["Desc"].ToString();
                chk_Active.Checked = bool.Parse(drTemplate["IsActived"].ToString());
                tl_Market.Visible = true;

                //Show TreeList's SaveButton
                tb_Assign.Visible = true;

                // Display List
                if (TemplateDetailMode == "1")
                {
                    tl_Market.DataSource = template.GetCateByLocationCodeCategoryType(drTemplate["LocationCode"].ToString(), TemplateDetailMode, LoginInfo.ConnStr);
                    tl_Market.DataBind();
                }

                //Assign Selected Items
                foreach (
                    DataRow drTmpDt in templateDt.GetListByTmpNo(drTemplate["TmpNo"].ToString(), LoginInfo.ConnStr).Rows
                    )
                {
                    if (tl_Market.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()) != null)
                    {
                        tl_Market.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()).Selected = true;
                    }
                }
            }
            else
            {
                chk_Active.Checked = true;
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "SAVE":

                        if (ddl_Store.Value == null)
                        {
                            pop_Store.ShowOnPageLoad = true;
                            return;
                        }

                        if (txt_Description.Text == string.Empty)
                        {
                            pop_Description.ShowOnPageLoad = true;
                            return;
                        }

                        if (ddl_Store.Value != null && txt_Description.Text != "")
                        {
                            Save();
                        }
                        break;

                    case "BACK":
                        if (Request.Params["MODE"].ToUpper() == "EDIT" || Request.Params["MODE"].ToUpper() == "NLIST")
                        {
                            Response.Redirect("MLT.aspx?ID=" + Request.Params["ID"].ToString());
                        }
                        else if (Request.Params["MODE"].ToUpper() == "NEW")
                        {
                            Response.Redirect("MLTLst.aspx");
                        }
                        else if (Request.Params["MODE"].ToUpper() == "COPY")
                        {
                            Response.Redirect("MLT.aspx?ID=" + Request.Params["ID"].ToString());
                        }
                        break;

                    case "CANCEL":
                        Response.Redirect("MLT.aspx?ID=" + Request.Params["ID"].ToString());
                        break;
                }
            }
        }

        private void Save()
        {
            Session["dsTemplateEdit"] = dsTemplateEdit;

            DataRow drSave = null;
            var countNode = 0;

            // Find Node in Tree.
            var selectNodes = tl_Market.GetSelectedNodes();

            // Count Item.
            for (var i = 0; i < selectNodes.Count; i++)
            {
                if (selectNodes[i].Level == 4)
                {
                    // check no unit on product
                    string unitCode = prodUnit.GetDefaultOrderUnit(selectNodes[i].Key.ToString(), LoginInfo.ConnStr);
                    if (unitCode == string.Empty)
                    {
                        lbl_Alert.Text = "There is no order unit of product  '" + selectNodes[i].Key.ToString() + " : " + selectNodes[i].GetValue("CategoryName").ToString() + "'";
                        pop_Alert.ShowOnPageLoad = true;
                        return;
                    }

                    countNode++;
                }
            }

            if (countNode == 0)
            {
                pop_InsertTL.ShowOnPageLoad = true;
                return;
            }
            else if (Request.Params["MODE"].ToString().ToUpper() == "NEW" ||
                     Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                drSave = dsTemplateEdit.Tables[template.TableName].NewRow();
                //drSave = dsTemplateEdit.Tables[template.TableName].Rows[0];

                //Get New ID.
                drSave["TmpNo"] = template.GetNewTemplateNo(LoginInfo.ConnStr);
                drSave["TmpTypeCode"] = "M";
                drSave["CreatedBy"] = LoginInfo.LoginName;
                drSave["CreatedDate"] = ServerDateTime;
            }
            else
            {

                drSave = dsTemplateEdit.Tables[template.TableName].Rows[0];

                // Delete all Prodduct in TreeView. 
                foreach (DataRow drTreeStoreCheck in dsTemplateEdit.Tables[templateDt.TableName].Rows)
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
                    var drNewStoreCheck = dsTemplateEdit.Tables[templateDt.TableName].NewRow();

                    if (Request.Params["MODE"].ToString().ToUpper() == "NEW" ||
                        Request.Params["MODE"].ToString().ToUpper() == "COPY")
                    {
                        drNewStoreCheck["TmpNo"] = template.GetNewTemplateNo(LoginInfo.ConnStr);
                    }
                    else
                    {
                        drSave = dsTemplateEdit.Tables[template.TableName].Rows[0];
                        drNewStoreCheck["TmpNo"] = drSave["TmpNo"].ToString();
                    }

                    drNewStoreCheck["TmpDtNo"] = templateDt.GetNewTmpDtNo(LoginInfo.ConnStr) + i;
                    drNewStoreCheck["ProductCode"] = selectNodes[i].Key.ToString();

                    string unitCode = prodUnit.GetDefaultOrderUnit(selectNodes[i].Key.ToString(), LoginInfo.ConnStr);
                    drNewStoreCheck["UnitCode"] = unitCode;

                    dsTemplateEdit.Tables[templateDt.TableName].Rows.Add(drNewStoreCheck);
                }
            }

            drSave["Desc"] = txt_Description.Text.Trim();
            drSave["LocationCode"] = ddl_Store.Value;
            drSave["IsActived"] = chk_Active.Checked;
            drSave["UpdatedBy"] = LoginInfo.LoginName;
            drSave["UpdatedDate"] = ServerDateTime;

            if (Request.Params["MODE"].ToString().ToUpper() == "NEW"
                || Request.Params["MODE"].ToString().ToUpper() == "COPY")
            {
                dsTemplateEdit.Tables[template.TableName].Rows.Add(drSave);
            }

            var save = template.Save(dsTemplateEdit, hf_ConnStr.Value);

            if (save)
            {
                Response.Redirect("MLT.aspx?ID=" + drSave["TmpNo"].ToString());
            }
        }

        #endregion

        /// <summary>
        ///     Get Name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            var store = ddl_Store.Value.ToString().Split(':');
            //txt_StoreName.Text = store[1].Trim();            
            tl_Market.DataSource = template.GetCateByLocationCodeCategoryType(store[0].ToString().Trim(),
                TemplateDetailMode, LoginInfo.ConnStr);
            tl_Market.DataBind();
        }

        protected void tl_Market_Load(object sender, EventArgs e)
        {
            if (ddl_Store.Value != null)
            {
                var StoreValue = ddl_Store.Value.ToString().Split(':');
                tl_Market.DataSource = template.GetCateByLocationCodeCategoryType(StoreValue[0].ToString().Trim(),
                    TemplateDetailMode, LoginInfo.ConnStr);
                tl_Market.DataBind();
            }
        }

        protected void menu_Module_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    break;
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            ddl_Store.DataSource = storeLct.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_Store.DataBind();
        }

        protected void btn_OK_Msg_Click(object sender, EventArgs e)
        {
            pop_Description.ShowOnPageLoad = false;
            return;
        }

        protected void btn_OK_MsgStore_Click(object sender, EventArgs e)
        {
            pop_Store.ShowOnPageLoad = false;
            return;
        }

        protected void btn_OK_TL_Click(object sender, EventArgs e)
        {
            pop_InsertTL.ShowOnPageLoad = false;
            return;
        }

        protected void btn_Alert_Ok_Click(object sender, EventArgs e)
        {
            pop_Alert.ShowOnPageLoad = false;
        }

        protected void tl_Market_CustomDataCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
        {
            string l = string.Empty;
            l += @"<ul>";

            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in tl_Market.GetSelectedNodes())
            {
                if (node.Level == 4)
                {

                    l += @"<li>" + node.GetValue("CategoryCode").ToString() + " : " + node.GetValue("CategoryName").ToString() + @"</li>";
                }
            }
            l += @"</ul>";

            e.Result = @l;
        }

        protected void tl_Market_DataBound(object sender, EventArgs e)
        {
        }


    }
}