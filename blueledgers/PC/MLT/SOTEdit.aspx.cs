using System;
using System.Data;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxTreeList;

namespace BlueLedger.PL.IN.MLT
{
    public partial class SOTEdit : BasePage
    {
        #region "Attributes"

        private string MsgError = string.Empty;
        private DataSet dsTemplateEdit = new DataSet();
        private Blue.BL.IN.ProdUnit prodUnit = new Blue.BL.IN.ProdUnit();
        private Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private Blue.BL.Option.Inventory.ProdCat prodcat = new Blue.BL.Option.Inventory.ProdCat();
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

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        this.Page_Retrieve();
        //        dsTemplateEdit = (DataSet)Session["dsTemplateEdit"];
        //    }
        //    else
        //    {
        //        dsTemplateEdit = (DataSet)Session["dsTemplateEdit"];
        //    }
        //}        
        ///// <summary>
        ///// Page load event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>

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

            // Modified on: 02/01/2018, By: Fon
            //TemplateDetailMode = "2";
            TemplateDetailMode = conf.GetValue("PC", "SO", "ProdCateType", LoginInfo.ConnStr);
            // End Modified.

            Page_Setting();

            //if (Request.Params["MODE"].ToString().ToUpper() == "NEW")
            //{
            //    // Get Template Structure
            //    bool getTemplateSchema = template.GetSchema(dsTemplateEdit, LoginInfo.ConnStr);

            //    if (getTemplateSchema)
            //    {
            //        // Default new row
            //        DataRow drInserting         = dsTemplateEdit.Tables[template.TableName].NewRow();

            //        drInserting["TmpNo"]        = template.GetNewTemplateNo(LoginInfo.ConnStr);
            //        drInserting["TmpTypeCode"]  = "O";
            //        drInserting["IsActived"]    = true;
            //        drInserting["CreatedBy"]    = LoginInfo.LoginName;
            //        drInserting["CreatedDate"]  = ServerDateTime;
            //        drInserting["UpdatedBy"]    = LoginInfo.LoginName;
            //        drInserting["UpdatedDate"]  = ServerDateTime;
            //        dsTemplateEdit.Tables[template.TableName].Rows.Add(drInserting);
            //    }
            //    else
            //    {
            //        return;
            //    }

            //    // Get Template Detail Structure
            //    bool getTempalteDetailSchema = templateDt.GetSchema(dsTemplateEdit, LoginInfo.ConnStr);

            //    if (!getTempalteDetailSchema)
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    int tmpNo = int.Parse(Request.Params["ID"].ToString());

            //    // Get Template Data
            //    bool getTemplate = template.Get(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

            //    if (!getTemplate)
            //    {
            //        return;
            //    }

            //    // Get template Detail Data
            //    bool getTemplateDt = templateDt.GetListByTmpNo(dsTemplateEdit, tmpNo, LoginInfo.ConnStr);

            //    if (!getTemplateDt)
            //    {
            //        return;
            //    }                
            //}



            //Session["dsTemplateEdit"] = dsTemplateEdit;
            //this.Page_Setting();
        }

        /// <summary>
        ///     Display business MLTEdit data which retrieved from Page_Retrieve procedure.
        /// </summary>
        private void Page_Setting()
        {
            hf_ConnStr.Value = LoginInfo.ConnStr;
            hf_LoginName.Value = LoginInfo.LoginName;

            // Page Title
            lbl_Title.Text = "Standard Order";

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
                tl_STO.Visible = true;

                //Show TreeList's SaveButton
                tb_MenuSave.Visible = true;

                // Display List
                // Modified on: 26/03/2018, By: Fon, For: Following from P'Oat request.
                //tl_STO.DataSource = template.GetCateByLocationCodeCategoryType(drTemplate["LocationCode"].ToString(),
                //    TemplateDetailMode, LoginInfo.ConnStr);
                //tl_STO.DataBind();

                DataTable dtResult = Join_ProductLocationWith_CategoryType(drTemplate["LocationCode"].ToString(), TemplateDetailMode);
                tl_STO.DataSource = dtResult;
                tl_STO.DataBind();
                // End Modified.


                //Assign Selected Items
                foreach
                (DataRow drTmpDt in templateDt.GetListByTmpNo(drTemplate["TmpNo"].ToString(), LoginInfo.ConnStr).Rows)
                {
                    if (tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()) != null)
                    {
                        tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()).Selected = true;
                        //lbl_Test.Text += string.Format("ProductCode: {0}, ", drTmpDt["ProductCode"]);
                    }
                }
            }
            else
            {
                chk_Active.Checked = true;
            }

            //// Display template
            //DataRow drTemplate      = dsTemplateEdit.Tables[template.TableName].Rows[0];           
            //chk_Active.Checked      = bool.Parse(drTemplate["IsActived"].ToString());
            //txt_Description.Text    = drTemplate["Desc"].ToString();

            //if (Request.Params["MODE"].ToUpper() == "EDIT")
            //{
            //    ddl_Store.Value     = drTemplate["LocationCode"];
            //    txt_TemplateNo.Text = drTemplate["TmpNo"].ToString();
            //    //txt_StoreName.Text  = storeLct.GetName(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    tl_STO.Visible = true;
            //    //Show TreeList's SaveButton
            //    tb_MenuSave.Visible = true;
            //    // Display List
            //    tl_STO.DataSource = template.GetCateByLocationCode(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    tl_STO.DataBind();

            //    //Assign Selected Items
            //    foreach (DataRow drTmpDt in templateDt.GetListByTmpNo(drTemplate["TmpNo"].ToString(),LoginInfo.ConnStr).Rows)
            //    {
            //        if (tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()) != null)
            //        {
            //            tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()).Selected = true;
            //        }
            //    }

            //}
            //else if (Request.Params["MODE"].ToUpper() == "NLIST")
            //{
            //    ddl_Store.Value         = drTemplate["LocationCode"];
            //    ddl_Store.Enabled       = false;
            //    txt_TemplateNo.Text     = drTemplate["TmpNo"].ToString();
            //    txt_TemplateNo.Enabled  = false;
            //    txt_Description.Enabled = false;
            //    chk_Active.Enabled      = false;
            //    //txt_StoreName.Text      = storeLct.GetName(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    tl_STO.Visible          = true;
            //    //Show TreeList's SaveButton
            //    tb_MenuSave.Visible = true;
            //    // Display List
            //    tl_STO.DataSource = template.GetCateByLocationCode(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    tl_STO.DataBind();

            //    //Assign Selected Items
            //    foreach (DataRow drTmpDt in templateDt.GetListByTmpNo(drTemplate["TmpNo"].ToString(), LoginInfo.ConnStr).Rows)
            //    {
            //        if (tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()) != null)
            //        {
            //            tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()).Selected = true;
            //        }
            //    }
            //}
            //else if (Request.Params["MODE"].ToUpper() == "COPY")
            //{
            //    menu_CmdBar.Items.FindByName("Back").Visible    = false;
            //    menu_CmdBar.Items.FindByName("Cancel").Visible  = true;
            //    ddl_Store.Value     = drTemplate["LocationCode"];
            //    txt_TemplateNo.Text = "";
            //    //txt_StoreName.Text  = storeLct.GetName(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    menu_Save.Items.FindByName("Save").Visible      = false;
            //    menu_Save.Items.FindByName("Create").Visible    = true;
            //    menu_Save.Items.FindByName("Delete").Visible    = true;
            //    tl_STO.Visible      = true;
            //    // Display List
            //    tl_STO.DataSource   = template.GetCateByLocationCode(drTemplate["LocationCode"].ToString(), LoginInfo.ConnStr);
            //    tl_STO.DataBind();

            //    //Assign Selected Items
            //    foreach (DataRow drTmpDt in templateDt.GetListByTmpNo(drTemplate["TmpNo"].ToString(), LoginInfo.ConnStr).Rows)
            //    {
            //        if (tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()) != null)
            //        {
            //            tl_STO.FindNodeByKeyValue(drTmpDt["ProductCode"].ToString()).Selected = true;
            //        }
            //    }
            //}
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

                        if (ddl_Store.Value == null || txt_Description.Text == "")
                        {
                            pop_Warning.ShowOnPageLoad = true;
                            return;
                        }
                        else
                            Save();

                        break;

                    case "BACK":
                        if (Request.Params["MODE"].ToUpper() == "EDIT" || Request.Params["MODE"].ToUpper() == "NLIST")
                        {
                            Response.Redirect("SOT.aspx?ID=" + Request.Params["ID"].ToString());
                        }
                        else if (Request.Params["MODE"].ToUpper() == "NEW")
                        {
                            Response.Redirect("SOTLst.aspx");
                        }

                        break;
                    case "CANCEL":
                        Response.Redirect("SOT.aspx?ID=" + Request.Params["ID"].ToString());
                        break;
                }
            }
        }

        private void Save()
        {
            DataRow drSave = null;
            var countNode = 0;

            // Find Node in Tree.
            var selectNodes = tl_STO.GetSelectedNodes();

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
                drSave["TmpTypeCode"] = "O";
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
                    drNewStoreCheck["UnitCode"] = prodUnit.GetDefaultOrderUnit(selectNodes[i].Key.ToString(),
                        LoginInfo.ConnStr);

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
                Response.Redirect("SOT.aspx?ID=" + drSave["TmpNo"].ToString());
            }
        }

        protected void ddl_Store_Load(object sender, EventArgs e)
        {
            ddl_Store.DataSource = storeLct.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);
            ddl_Store.DataBind();
        }

        protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string[] StoreValue    = ddl_Store.Value.ToString().Split(':');
            //txt_StoreName.Text      = StoreValue[1].Trim();

            // Modified on: 27/03/2018, By: Fon
            //tl_STO.DataSource = template.GetCateByLocationCodeCategoryType(ddl_Store.Value.ToString(),
            //    TemplateDetailMode, LoginInfo.ConnStr);
            //tl_STO.DataBind();

            DataTable dtResult = Join_ProductLocationWith_CategoryType(ddl_Store.Value.ToString(), TemplateDetailMode);
            tl_STO.DataSource = dtResult;
            tl_STO.DataBind();
            // End Modified.
        }

        protected void tl_STO_OnLoad(object sender, EventArgs e)
        {
            (sender as ASPxTreeList).SettingsSelection.Recursive = true;

            if (ddl_Store.Value != null)
            {
                //string[] StoreValue = ddl_Store.Value.ToString().Split(':');

                // Modified on: 26/03/2018, By: Fon, For: Following from P'Oat request.
                //tl_STO.DataSource = template.GetCateByLocationCodeCategoryType(ddl_Store.Value.ToString(),
                //    TemplateDetailMode, LoginInfo.ConnStr);
                //tl_STO.DataBind();

                DataTable dtResult = Join_ProductLocationWith_CategoryType(ddl_Store.Value.ToString(), TemplateDetailMode);
                tl_STO.DataSource = dtResult;
                tl_STO.DataBind();
                // End Modified.
            }
            

        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        protected void menu_Save_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    //DataSet dsTmpDt = new DataSet();
                    //dsTmpDt = (DataSet)Session["dsTemplateEdit"];

                    ////Delete All TemplateDt Data
                    //foreach (DataRow dsTmpDtCheck in dsTemplateEdit.Tables[templateDt.TableName].Rows)
                    //{
                    //    if (dsTmpDtCheck.RowState != DataRowState.Deleted)
                    //    {
                    //        dsTmpDtCheck.Delete();
                    //    }
                    //}

                    ////Insert New Row
                    //List<TreeListNode> selectNodes = tl_STO.GetSelectedNodes();

                    //for (int i = 0; i < selectNodes.Count; i++)
                    //{
                    //    if (selectNodes[i].Level == 4)
                    //    {
                    //        DataRow drNewTmpDtCheck         = dsTmpDt.Tables[templateDt.TableName].NewRow();
                    //        drNewTmpDtCheck["TmpNo"]        = txt_TemplateNo.Text;
                    //        drNewTmpDtCheck["TmpDtNo"]      = templateDt.GetNewTmpDtNo(LoginInfo.ConnStr) + i;
                    //        drNewTmpDtCheck["ProductCode"]  = selectNodes[i].Key.ToString();
                    //        drNewTmpDtCheck["UnitCode"]     = product.GetOrderUnit(selectNodes[i].Key.ToString(), LoginInfo.ConnStr);
                    //        dsTmpDt.Tables[templateDt.TableName].Rows.Add(drNewTmpDtCheck);
                    //    }
                    //}

                    //bool save = templateDt.Save(dsTmpDt, LoginInfo.ConnStr);
                    //if (save)
                    //{
                    //    // Clear value
                    //    dsTmpDt.Clear();
                    //    Session["dsTmpDt"] = null;

                    //    // Refresh all Role Data.
                    //    this.Page_Retrieve();

                    //    Response.Redirect("SOT.aspx?ID=" + txt_TemplateNo.Text);
                    //}
                    //else
                    //{
                    //    dsTmpDt.Clear();
                    //}
                    break;
            }
        }

        #endregion

        protected void btn_OK_TL_Click(object sender, EventArgs e)
        {
            pop_InsertTL.ShowOnPageLoad = false;
            return;
        }

        // Added on: 27/03/2018
        protected DataTable Join_ProductLocationWith_CategoryType(string locationCode, string filter)
        {
            DataTable dtProductLocationType = new DataTable();
            //DataTable dtProductLocationType = template.GetCateByLocationCode(locateCode, LoginInfo.ConnStr);

            string sql = ";WITH Product AS( ";
            sql += " SELECT";
            sql += "   pl.ProductCode AS CategoryCode, ";
            sql += "   pl.ProductCode + ' : ' + p.ProductDesc1 AS CategoryName, ";
            sql += "   ProductCate AS ParentNo, ";
            sql += "   p.ProductDesc2 AS ProductDesc2,";
            sql += "   '4' AS LevelNo,";
            sql += "   pc.CategoryType";
            sql += " FROM ";
            sql += "   [IN].ProdLoc pl";
            sql += "   JOIN [IN].Product p ON p.ProductCode = pl.ProductCode";
            sql += "   JOIN [IN].ProductCategory pc ON pc.CategoryCode = p.ProductCate AND pc.LevelNo = 3";
            sql += " WHERE";
            sql += "   p.IsActive = 1";
            sql += "   AND pl.LocationCode = @LocationCode";
            sql += "   AND " + filter;
            sql += " ),";
            sql += " ItemGroup AS(";
            sql += " SELECT";
            sql += "   pc.CategoryCode,";
            sql += "   pc.CategoryName,";
            sql += "   pc.ParentNo,";
            sql += "   NULL AS ProductDesc2,";
            sql += "   '3' AS LevelNo,";
            sql += "   pc.CategoryType";
            sql += " FROM";
            sql += "   Product p";
            sql += "   JOIN [IN].ProductCategory pc ON pc.CategoryCode = p.ParentNo";
            sql += " WHERE";
            sql += "   pc.LevelNo = 3";
            sql += " GROUP BY pc.CategoryCode, pc.CategoryName, pc.ParentNo, pc.CategoryType";
            sql += " ),";
            sql += " SubCate AS(";
            sql += " SELECT";
            sql += "   pc.CategoryCode,";
            sql += "   pc.CategoryName,";
            sql += "   pc.ParentNo,";
            sql += "   NULL AS ProductDesc2,";
            sql += "   '2' AS LevelNo,";
            sql += "   pc.CategoryType";
            sql += " FROM";
            sql += "   itemGroup i";
            sql += "   JOIN [IN].ProductCategory pc ON pc.CategoryCode = i.ParentNo";
            sql += " WHERE";
            sql += "   pc.LevelNo = 2";
            sql += " GROUP BY pc.CategoryCode, pc.CategoryName, pc.ParentNo, pc.CategoryType";
            sql += " ),";
            sql += " Category AS(";
            sql += " SELECT";
            sql += "   pc.CategoryCode,";
            sql += "   pc.CategoryName,";
            sql += "   pc.ParentNo,";
            sql += "   NULL AS ProductDesc2,";
            sql += "   '1' AS LevelNo,";
            sql += "   pc.CategoryType";
            sql += " FROM";
            sql += "   SubCate s";
            sql += "   JOIN [IN].ProductCategory pc ON pc.CategoryCode = s.ParentNo";
            sql += " WHERE";
            sql += "   pc.LevelNo = 1";
            sql += " GROUP BY pc.CategoryCode, pc.CategoryName, pc.ParentNo, pc.CategoryType";
            sql += " ),";
            sql += " so AS(";
            sql += "   SELECT * FROM Category";
            sql += "   UNION";
            sql += "   SELECT * FROM SubCate";
            sql += "   UNION";
            sql += "   SELECT * FROM ItemGroup";
            sql += "   UNION";
            sql += "   SELECT * FROM Product";
            sql += " )";
            sql += " SELECT * FROM so";
            sql += " ORDER BY ";
            sql += "   LevelNo,";
            sql += "   CategoryCode";

            var dbParams = new Blue.DAL.DbParameter[1];
            dbParams[0] = new Blue.DAL.DbParameter("@LocationCode", locationCode);

            dtProductLocationType =  conf.DbExecuteQuery(sql, dbParams, hf_ConnStr.Value);

            //DataRow[] dr =  dt.Select(filter);
            //foreach (DataRow row in dr)
            //{
            //    dtProductLocationType.ImportRow(row);
            //}


            //dtProductLocationType.Columns.Add("CategoryType", typeof(int));

            //int categoryType = 0;
            //if (dtProductLocationType.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dtProductLocationType.Rows)
            //    {
            //        int cateType = prodcat.GetCategoryType(dr["CategoryCode"].ToString(), LoginInfo.ConnStr);
            //        if (dr["ParentNo"].ToString() == "0" && cateType == 0)
            //        {
            //            categoryType = 0;
            //        }

            //        if (cateType > 0)
            //        {
            //            categoryType = cateType;
            //        }

            //        dr["CategoryType"] = categoryType;
            //    }

            //    string strFilter = "[Level] in ('1', '2', '3')";
            //    if (strCustomFilter != string.Empty)
            //    {
            //        DataRow[] drResults = dtProductLocationType.Select(strFilter + " OR " + strCustomFilter);
            //        return drResults.CopyToDataTable();
            //    }
            //    else
            //    { return dtProductLocationType; }
            //}
            //else
            return dtProductLocationType;

        }
        // End Added.
    }
}