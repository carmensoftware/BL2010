using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.ProdCat
{
    public partial class ProdCatLst : BasePage
    {
        #region "Attributes"

        private string MsgError = string.Empty;
        private Blue.BL.GL.Account.Account account = new Blue.BL.GL.Account.Account();

        private DataSet dsProdCat = new DataSet();
        private DataSet dsProdCatDisplay = new DataSet();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();
        private Blue.BL.Option.Inventory.ProdCateType prodCateType = new Blue.BL.Option.Inventory.ProdCateType();
        private Blue.BL.Option.Inventory.Product prod = new Blue.BL.Option.Inventory.Product();

        DataSet dsTree = new DataSet();
        DataTable dtImportFile = new DataTable();
        private int rejects = 0;

        #endregion

        #region "Operations"
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["rejects"] = 0;

                // Note: 0 = Create, 1 = Edit.
                Session["IsEditProductcategory"] = null;

                pageSetting();
            }
            else
            {
                dsTree = (DataSet)Session["dsTree"];
                dtImportFile = (DataTable)Session["dtImportFile"];
                rejects = (int)Session["rejects"];
            }

            base.Page_Load(sender, e);
        }

        private void pageSetting()
        {
            GetData();
            SetTreeNode();
        }

        private void GetData()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

                string strsqlLevel01 = "select * from [in].productcategory where LevelNo = '1' ";
                string strsqlLevel02 = "select * from [in].productcategory where LevelNo = '2' ";
                string strsqlLevel03 = "select * from [in].productcategory where LevelNo = '3' ";
                string treeView = strsqlLevel01 + ";" + strsqlLevel02 + ";" + strsqlLevel03;

                SqlCommand mycommand2 = new SqlCommand(treeView, cnn);
                SqlDataAdapter da = new SqlDataAdapter(mycommand2);

                dsTree = new DataSet();
                da.Fill(dsTree);
                dsTree.Tables[0].TableName = "Level01";
                dsTree.Tables[1].TableName = "Level02";
                dsTree.Tables[2].TableName = "Level03";
                Session["dsTree"] = (DataSet)dsTree;
            }
            catch (Exception ex)
            {
                cnn.Close();
                Response.Write(ex);
            }
        }

        protected void SetTreeNode()
        {
            tview.Nodes.Clear(); //<--- Because U call this function many times.
            foreach (DataRow rowLevel01 in dsTree.Tables["Level01"].Rows)
            {
                TreeNode tnLevel01 = new TreeNode();
                tnLevel01.Text = rowLevel01["CategoryName"].ToString();
                tnLevel01.Value = rowLevel01["CategoryCode"].ToString();

                string parentID01 = rowLevel01["CategoryCode"].ToString();
                DataRow[] dr02 = dsTree.Tables[1].Select("ParentNo = '" + parentID01 + "'");
                foreach (DataRow rowLevel02 in dr02)
                {
                    TreeNode tnLevel02 = new TreeNode();
                    tnLevel02.Text = rowLevel02["CategoryName"].ToString();
                    tnLevel02.Value = rowLevel02["CategoryCode"].ToString();

                    string parentID02 = rowLevel02["CategoryCode"].ToString();
                    DataRow[] dr03 = dsTree.Tables[2].Select("ParentNo = '" + parentID02 + "'");
                    foreach (DataRow rowLevel03 in dr03)
                    {
                        TreeNode tnLevel03 = new TreeNode();
                        tnLevel03.Text = rowLevel03["CategoryName"].ToString();
                        tnLevel03.Value = rowLevel03["CategoryCode"].ToString();

                        tnLevel02.ChildNodes.Add(tnLevel03);
                    }
                    tnLevel01.ChildNodes.Add(tnLevel02);
                }
                tview.Nodes.Add(tnLevel01);
            }
            tview.CollapseAll();
        }

        protected void tview_SelectedNodeChanged(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            BindDetail();

            if (Session["IsEditProductcategory"] != null)
            {
                menu_CmdBar.Items.FindByName("Import").Visible = false;
                menu_CmdBar.Items.FindByName("AddLevel01").Visible = false;
                menu_CmdBar.Items.FindByName("AddLevel02").Visible = false;
                menu_CmdBar.Items.FindByName("AddLevel03").Visible = false;
                menu_CmdBar.Items.FindByName("Edit").Visible = false;
                //menu_CmdBar.Items.FindByName("Delete").Visible = false;
            }
            else if (Session["IsEditProductcategory"] == null)
            {
                menu_CmdBar.Items.FindByName("Import").Visible = true;
                menu_CmdBar.Items.FindByName("Edit").Visible = true;
                //menu_CmdBar.Items.FindByName("Delete").Visible = false;

                SetButtonAdd();
            }
        }

        protected void btnS_Click(object sender, EventArgs e)
        {
            tview.CollapseAll();
            if (txtSearch.Text != null)
            {
                TreeNode nodeByCode = FindNodeInTreeView(txtSearch.Text);
                if (nodeByCode != null)
                {
                    nodeByCode.Selected = true;
                    BindDetail();
                }
                else
                {
                    lbl_Warning.Text = "Doesn't exist.";
                    pop_Warning.ShowOnPageLoad = true;
                    txtSearch.Text = null;
                }
            }
        }
        private TreeNode FindNodeInTreeView(string search)
        {
            for (int tn01 = 0; tn01 < tview.Nodes.Count; tn01++)
            {
                TreeNode tnLevel01 = tview.Nodes[tn01];
                //if (tnLevel01.Value.ToString().ToUpper() != search.ToUpper())
                if (tnLevel01.Text.ToUpper().Contains(search.ToUpper()) == false)
                {
                    for (int tn02 = 0; tn02 < tnLevel01.ChildNodes.Count; tn02++)
                    {
                        TreeNode tnLevel02 = tnLevel01.ChildNodes[tn02];
                        if (tnLevel02.Text.ToUpper().Contains(txtSearch.Text.ToUpper()) == false)
                        {
                            for (int tn03 = 0; tn03 < tnLevel02.ChildNodes.Count; tn03++)
                            {
                                TreeNode tnLevel03 = tnLevel02.ChildNodes[tn03];
                                if (tnLevel03.Text.ToUpper().Contains(txtSearch.Text.ToUpper()) == true)
                                {
                                    tnLevel01.Expanded = true;
                                    tnLevel02.Expanded = true;
                                    return tnLevel03;
                                }
                            }
                        }
                        else
                        {
                            tnLevel01.Expanded = true;
                            return tnLevel02;
                        }
                    }
                }
                else
                { return tnLevel01; }
            }
            return null;
        }

        protected void Set_AboutLevel(int level)
        {
            if (level == 0)
            {
                lblLevelDesc.Text = "Category";
                lblLevelNo.Text = "0";
            }
            else if (level == 1)
            {
                lblLevelDesc.Text = "Sub Category";
                lblLevelNo.Text = "1";
            }
            else if (level == 2)
            {
                lblLevelDesc.Text = "Item Group";
                lblLevelNo.Text = "2";
            }
        }

        protected void BindDetail()
        {
            string tviewValue = tview.SelectedValue.ToString();
            int tableIndex = getTableIndexByValue(tviewValue);
            int rowIndex = getRowIndex(tableIndex, tviewValue);

            Set_AboutLevel(tableIndex);

            txtParent.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["ParentNo"].ToString();

            if (Session["IsEditProductcategory"] != "0")
            {
                txtCategoryCode.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryCode"].ToString();
                txtCategoryName.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryName"].ToString();

                // Modified on: 09/01/2018 //CategoryType	
                if (dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"] != DBNull.Value)
                {
                    ddlCategoryType.SelectedValue = string.Format("{0}", dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"]);
                }
                else
                {
                    ddlCategoryType.SelectedValue = "-1";
                }
                // End Modifeid.

                if (dsTree.Tables[tableIndex].Rows[rowIndex]["IsActive"] != DBNull.Value)
                    cbTActive.Checked = Convert.ToBoolean(dsTree.Tables[tableIndex].Rows[rowIndex]["IsActive"]);

                txtAccCode.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["TaxAccCode"].ToString();

                if (dsTree.Tables[tableIndex].Rows[rowIndex]["AuthRules"] != DBNull.Value)
                    cbAuthRules.Checked = Convert.ToBoolean(dsTree.Tables[tableIndex].Rows[rowIndex]["AuthRules"]);

                txtApp.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["ApprovalLevel"].ToString();

                se_PriceDeviation.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["PriceDeviation"].ToString();
                se_QtyDeviation.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["QuantityDeviation"].ToString();
            }
            else //if (Session["IsEditProductcategory"].ToString() == "0")
            {
                if (prodCat.GetCategoryType(txtParent.Text, LoginInfo.ConnStr) > 0
                && dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"] != DBNull.Value)
                {
                    ddlCategoryType.SelectedValue = (txtParent.Text != "0")
                      ? string.Format("{0}", prodCat.GetCategoryType(txtParent.Text, LoginInfo.ConnStr))
                      : string.Format("{0}", dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"]);
                }
            }

            // Added on: 23/01/2018, By Fon
            ddlCategoryType.Enabled = (Session["IsEditProductcategory"] != null) ? true : false;
            se_PriceDeviation.Enabled = ddlCategoryType.Enabled;
            se_QtyDeviation.Enabled = ddlCategoryType.Enabled;
            // End Added.
        }

        private int getTableIndexByValue(string value)
        {
            int tableIndex = -1;
            for (int i = 0; i < dsTree.Tables[0].Rows.Count; i++)
            {
                if (dsTree.Tables[0].Rows[i]["CategoryCode"].ToString().ToUpper() == value.ToUpper())
                { return tableIndex = 0; }
            }
            for (int i = 0; i < dsTree.Tables[1].Rows.Count; i++)
            {
                if (dsTree.Tables[1].Rows[i]["CategoryCode"].ToString().ToUpper() == value.ToUpper())
                { return tableIndex = 1; }
            }
            for (int i = 0; i < dsTree.Tables[2].Rows.Count; i++)
            {
                if (dsTree.Tables[2].Rows[i]["CategoryCode"].ToString().ToUpper() == value.ToUpper())
                { return tableIndex = 2; }
            }
            return tableIndex;
        }

        private int getRowIndex(int tableIndex, string value)
        {
            int rowIndex = -1;
            for (int i = 0; i < dsTree.Tables[tableIndex].Rows.Count; i++)
            {
                if (dsTree.Tables[tableIndex].Rows[i]["CategoryCode"].ToString() == value)
                { return rowIndex = i; }
            }
            return rowIndex;
        }

        protected void SetButtonAdd()
        {
            string tviewValue = tview.SelectedValue.ToString();
            int tableNo = getTableIndexByValue(tviewValue);

            menu_CmdBar.Items.FindByName("AddLevel01").Visible = false;
            menu_CmdBar.Items.FindByName("AddLevel02").Visible = false;
            menu_CmdBar.Items.FindByName("AddLevel03").Visible = false;

            if (tableNo == 0)
            {
                menu_CmdBar.Items.FindByName("AddLevel01").Visible = true;
                menu_CmdBar.Items.FindByName("AddLevel02").Visible = true;
            }
            else if (tableNo == 1)
            {
                menu_CmdBar.Items.FindByName("AddLevel02").Visible = true;
                menu_CmdBar.Items.FindByName("AddLevel03").Visible = true;
            }
            else if (tableNo == 2)
            {
                menu_CmdBar.Items.FindByName("AddLevel03").Visible = true;
            }
        }

        protected void ClearDatail()
        {
            txtParent.Text = "";
            txtCategoryCode.Text = "";
            txtCategoryName.Text = "";

            txtAccCode.Text = "";
            txtApp.Text = "";
        }

        protected void SetVisibleOfDetail(int status)
        {
            bool boolStatus = true;
            if (status == 0)
            { boolStatus = false; }
            else if (status == 1)
            { boolStatus = true; }
            lbl00.Visible = boolStatus;
            txtParent.Visible = boolStatus;
        }


        protected bool isDeleted(string categoryCode, int level)
        {
            // Check at table ...


            // Check at table ...

            return true;
        }

        protected void deleteInDataBase(string categoryCode)
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                string strsql = "DELETE from [in].productcategory ";
                strsql += "Where CategoryCode = @CategoryCode";
                SqlCommand deletecmd = new SqlCommand(strsql, cnn);
                deletecmd.Parameters.Clear();
                deletecmd.Parameters.AddWithValue("@CategoryCode", categoryCode);
                deletecmd.ExecuteNonQuery();

                cnn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string tviewValue = tview.SelectedValue.ToString();
            //int tableIndex = getTableIndexByValue(tviewValue);
            int tableIndex = Convert.ToInt32(lblLevelNo.Text);
            if (SaveProcess(tableIndex))
            {
                SettingAfterSave();
                SetVisibleOfDetail(0);
            }
        }
        protected void SettingAfterSave()
        {
            Session["IsEditProductcategory"] = null;

            GetData();
            BindDetail();
            SetTreeNode();

            txtParent.Enabled = false;
            txtCategoryCode.Enabled = false;
            txtCategoryName.Enabled = false;

            //cbTActive.Disabled = true;
            txtAccCode.Enabled = false;
            cbAuthRules.Enabled = false;
            txtApp.Enabled = false;
            se_PriceDeviation.Enabled = false;
            se_QtyDeviation.Enabled = false;

            btnSave.Visible = false;
            btnSaveEdit.Visible = false;
            btnCancel.Visible = false;
            btnDelete.Visible = false;

            txtSearch.Enabled = true;
            btnS.Enabled = true;
            cbTActive.Enabled = btnSaveEdit.Visible;
        }

        protected bool SaveProcess(int tableIndex)
        {
            string errMsg = CheckBeforeSave();
            if (errMsg == string.Empty)
            {
                return
                saveToDataBase(tableIndex, ref errMsg);
            }
            else
            {
                lbl_Warning.Text = errMsg;
                pop_Warning.ShowOnPageLoad = true;
                return false;
            }
        }

        protected string CheckBeforeSave()
        {
            if (txtCategoryName.Text == string.Empty)
                return "Field is required." + "<br/>" + "You have to fill Category Name.";

            int status = getTableIndexByValue(txtCategoryCode.Text);
            if (status != -1 && Session["IsEditProductcategory"] == "0")
                return "Existed." + "<br/>" + "Already had Category Code!";

            if (txtParent.Text == "0" && Convert.ToInt32(ddlCategoryType.SelectedItem.Value) < 0)
                return "Field is required." + "<br/>" + "You have to fill Category Type.";

            return string.Empty;
        }

        protected string sqlInsertString(int stringCode)
        {
            // Note: 0 = Insert, 1 = Edit.

            string strsql = string.Empty;
            switch (stringCode)
            {
                case 0: strsql = string.Format(@"Insert into [in].productcategory 
                    (LevelNo, ParentNo, CategoryCode, CategoryName, CategoryType, 
                    IsActive, TaxAccCode, AuthRules, ApprovalLevel, PriceDeviation, 
                    CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) 
                    Values( @LevelNo, @ParentNo, @CategoryCode, @CategoryName, @CategoryType, 
                    @IsActive, @TaxAccCode, @AuthRules, @ApprovalLevel, @PriceDeviation, 
                    @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy) ");
                    break;

                //                case 1: strsql = string.Format(@"UPDATE [IN].[ProductCategory]
                //                    SET [CategoryName]=@CategoryName, [CategoryType]=@CategoryType, 
                //                    [TaxAccCode]=@TaxAccCode, [UpdatedDate]=@UpdatedDate, [UpdatedBy]=@UpdatedBy
                //                    WHERE [CategoryCode] = @CategoryCode");
                //                    break;
                case 1: strsql = @"UPDATE [IN].[ProductCategory] SET 
                                    [CategoryName]=@CategoryName, 
                                    [CategoryType]=@CategoryType,
                                    [IsActive]=@IsActive,
                                    [TaxAccCode]=@TaxAccCode, 
                                    [AuthRules]=@AuthRules,
                                    [ApprovalLevel]=@ApprovalLevel,
                                    [PriceDeviation]=@PriceDeviation,
                                    [QuantityDeviation]=@QuantityDeviation,
                                    [UpdatedDate]=@UpdatedDate,
                                    [UpdatedBy]=@UpdatedBy
                                  WHERE [CategoryCode] = @CategoryCode";
                    break;

            }
            return strsql;
        }

        protected bool saveToDataBase(int tableIndex, ref string errMsg)
        {
            string connetionString = LoginInfo.ConnStr;
            string categoryType = (Convert.ToInt32(ddlCategoryType.SelectedValue) > -1)
                ? ddlCategoryType.SelectedValue
                : prodCat.GetCategoryType(txtParent.Text, LoginInfo.ConnStr).ToString();

            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                string strsql = string.Empty;
                if ((string)Session["IsEditProductcategory"] == "0")
                {
                    strsql = sqlInsertString(0);
                }
                else if ((string)Session["IsEditProductcategory"] == "1")
                {
                    strsql = sqlInsertString(1);
                }



                SqlCommand cmd = new SqlCommand(strsql, cnn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CategoryCode", txtCategoryCode.Text);
                cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                cmd.Parameters.AddWithValue("@CategoryType", (Convert.ToInt32(categoryType) < 0) ? null : categoryType);
                cmd.Parameters.AddWithValue("@IsActive", cbTActive.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@TaxAccCode", txtAccCode.Text);
                cmd.Parameters.AddWithValue("@AuthRules", cbAuthRules.Checked);
                cmd.Parameters.AddWithValue("@ApprovalLevel", DBNull.Value); //<-- have 2 & 3 ?
                if (string.IsNullOrEmpty(se_PriceDeviation.Text))
                    cmd.Parameters.AddWithValue("@PriceDeviation", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@PriceDeviation", se_PriceDeviation.Value);
                if (string.IsNullOrEmpty(se_QtyDeviation.Text))
                    cmd.Parameters.AddWithValue("@QuantityDeviation", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@QuantityDeviation", se_QtyDeviation.Value);

                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedBy", LoginInfo.LoginName);

                if ((string)Session["IsEditProductcategory"] == "0")
                {
                    cmd.Parameters.AddWithValue("@LevelNo", tableIndex + 1);
                    cmd.Parameters.AddWithValue("@ParentNo", txtParent.Text);

                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreatedBy", LoginInfo.LoginName);
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    cnn.Close();
                    errMsg = ex.Message;
                    return false;
                }

                // Update all if it's parent.
                List<string> cateCodeList = GetChildFromCategory(tview);
                if (txtParent.Text == "0" || cateCodeList.Count > 0)
                {
                    string whereCateCode = string.Format("IN ('{0}')", string.Join("', '", cateCodeList.ToArray()));
                    strsql = string.Format(@"UPDATE [IN].[ProductCategory] SET [CategoryType] = @CategoryType
                        WHERE [CategoryCode] {0}", whereCateCode);

                    cmd = new SqlCommand(strsql, cnn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@CategoryType", (Convert.ToInt32(categoryType) < 0) ? null : categoryType);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                cnn.Close();
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        protected List<string> GetChildFromCategory(TreeView tview)
        {
            TreeNode nodeRoot = tview.FindNode(tview.SelectedNode.ValuePath);
            List<string> cateCodeList = new List<string>();
            foreach (TreeNode subCate in nodeRoot.ChildNodes)
            {
                cateCodeList.Add(subCate.Value);
                if (subCate.ChildNodes.Count > 0)
                {
                    foreach (TreeNode itemGrp in subCate.ChildNodes)
                    {
                        cateCodeList.Add(itemGrp.Value);
                    }
                }
            }

            return cateCodeList;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["IsEditProductcategory"] = null;
            menu_CmdBar.Items.FindByName("Import").Visible = true;
            menu_CmdBar.Items.FindByName("Edit").Visible = true;
            //menu_CmdBar.Items.FindByName("Delete").Visible = true;

            BindDetail();
            SetButtonAdd();
            txtCategoryCode.Enabled = false;
            txtCategoryName.Enabled = false;

            // Modified on: 09/01/2018
            //txtCategoryType.Enabled = false;
            ddlCategoryType.Enabled = false;
            // End Modified.

            se_PriceDeviation.Enabled = false;
            se_QtyDeviation.Enabled = false;

            txtAccCode.Enabled = false;
            //cbTActive.Disabled = true;
            cbAuthRules.Enabled = false;
            txtApp.Enabled = false;

            btnSave.Visible = false;
            btnSaveEdit.Visible = false;
            btnCancel.Visible = false;
            btnDelete.Visible = false;

            txtSearch.Enabled = true;
            btnS.Enabled = true;
            SetVisibleOfDetail(0);
            cbTActive.Enabled = btnSaveEdit.Visible;
        }


        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }
        #endregion

        #region "Added on 2016-11-09"
        protected bool readToDataTable(string fileName)
        {
            DataTable dtImportFile = new DataTable();
            //using (System.IO.TextReader tr = File.OpenText(Server.MapPath("~/Option/LocalConfig/sample.txt")))
            try
            {
                if (File.Exists(Server.MapPath("~/CSV/" + fileName)))
                {
                    using (System.IO.TextReader tr = File.OpenText(Server.MapPath("~/CSV/" + fileName)))
                    {
                        string line;
                        int lineIndex = 0;
                        int reject = 0;

                        dtImportFile.Columns.Add("Category", typeof(String));
                        dtImportFile.Columns.Add("Sub-category", typeof(String));
                        dtImportFile.Columns.Add("Itemgroup", typeof(String));
                        dtImportFile.Columns.Add("Description", typeof(String));
                        dtImportFile.Columns.Add("TaxAccount", typeof(String));

                        while ((line = tr.ReadLine()) != null)
                        {
                            string[] items = line.Split(',');
                            // string code = items[1];
                            string code = string.Empty;
                            for (int i = 0; i < items.Length; i++)
                            {
                                items[i] = items[i].Trim();
                                if (i < 3 && code == string.Empty)
                                { code = items[i]; }
                            }

                            if (lineIndex > 0 && code != string.Empty)
                            {
                                if (items.Length != 5)// Fix 5 columns.
                                {
                                    lbl_Warning.Text = "You’ve requested the '" + fileName;
                                    lbl_Warning.Text += "' file.<br/> That file is wrong format.";
                                    pop_Warning.HeaderText = "Warning!";
                                    pop_Warning.ShowOnPageLoad = true;
                                    return false;
                                }
                                if (getTableIndexByValue(code) < 0)
                                { dtImportFile.Rows.Add(items); }
                                else
                                {
                                    reject++;
                                    Session["rejects"] = reject;
                                }
                            }
                            lineIndex++;
                        }
                        Session["dtImportFile"] = (DataTable)dtImportFile;
                        return true;
                    }
                }
                else
                {
                    lbl_Warning.Text = "You’ve requested the ' " + fileName + " ' file.<br/>";
                    lbl_Warning.Text += "That file doesn’t exists.";
                    pop_Warning.HeaderText = "Warning!";
                    pop_Warning.ShowOnPageLoad = true;
                    return false;
                }
            }
            catch
            { return false; }
        }

        protected bool saveImportFileToDatabase()
        {
            DataTable dtImportFile = (DataTable)Session["dtImportFile"];
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            //Code, Description, Account
            try
            {
                cnn.Open();
                int rowIndex = 0;
                foreach (DataRow row in dtImportFile.Rows)
                {
                    string strsql = sqlInsertString(0);
                    SqlCommand cmd = new SqlCommand(strsql, cnn);

                    string code = string.Empty;
                    int level = 0;
                    for (int i = 0; i < (dtImportFile.Columns.Count) - 2; i++)
                    {
                        if (code == string.Empty)
                        {
                            code = row[i].ToString();
                            level++;
                        }
                    }
                    string parent = findBackToParent(rowIndex, level);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@LevelNo", level);
                    cmd.Parameters.AddWithValue("@ParentNo", parent.ToUpper());
                    cmd.Parameters.AddWithValue("@CategoryCode", code.ToUpper());
                    cmd.Parameters.AddWithValue("@CategoryName", row[3]);
                    cmd.Parameters.AddWithValue("@CategoryType", "2"); // ?
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@TaxAccCode", row[4]);
                    cmd.Parameters.AddWithValue("@AuthRules", false);
                    cmd.Parameters.AddWithValue("@ApprovalLevel", string.Empty); //<-- have 2 & 3
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", LoginInfo.LoginName);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreatedBy", LoginInfo.LoginName);
                    try
                    { cmd.ExecuteNonQuery(); }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        return false;
                    }
                    rowIndex++;
                }
                return true;
            }
            catch
            { return false; }
        }

        protected string findBackToParent(int rowIndex, int level)
        {
            string parent = string.Empty;
            DataTable dtImportFile = (DataTable)Session["dtImportFile"];
            //string str = dtInputFile.Rows[rowIndex][3].ToString();
            for (int i = 0; i < dtImportFile.Rows.Count; i++)
            {
                if (i == rowIndex)
                {
                    for (int back = i; back >= 0; back--)
                    {
                        string category = dtImportFile.Rows[back]["Category"].ToString();
                        string sub_cate = dtImportFile.Rows[back]["Sub-category"].ToString();
                        string itemGroup = dtImportFile.Rows[back]["Itemgroup"].ToString();
                        if (level == 1) { return parent = "0"; }
                        if (level == 2 && sub_cate == string.Empty && itemGroup == string.Empty)
                        { return parent = category; }
                        if (level == 3 && category == string.Empty && itemGroup == string.Empty)
                        { return parent = sub_cate; }
                    }
                }
            }
            return parent;
        }

        protected void btnUploadHide_Click(object sender, EventArgs e)
        {
            string fileName = Session["fileName"].ToString();
            if (!readToDataTable(fileName)) { dtImportFile = null; }
            else
            {
                dtImportFile = (DataTable)Session["dtImportFile"];
                ASPxPopupFile.ShowOnPageLoad = false;

                if (!saveImportFileToDatabase())
                { dtImportFile = null; }
                else { dtImportFile = (DataTable)Session["dtImportFile"]; }
            }

            int rejectline = (int)Session["rejects"];
            if (rejectline > 0)
            { lbl_Warning.Text = "reject: " + rejectline.ToString() + " lines."; }
            else { lbl_Warning.Text = "Successful"; }
            pop_Warning.HeaderText = "File";

            GetData();
            SetTreeNode();
            pop_Warning.ShowOnPageLoad = true;
        }
        #endregion

        #region "Menu Bar"
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "ADDLEVEL01":
                    menuBar_onCreateClick(0);
                    break;
                case "ADDLEVEL02":
                    menuBar_onCreateClick(1);
                    break;
                case "ADDLEVEL03":
                    menuBar_onCreateClick(2);
                    break;

                case "EDIT":
                    menuBar_onEditClick();
                    break;

                case "DELETE":
                    menuBar_onDeleteClick();
                    break;

                case "IMPORT":
                    ASPxPopupFile.ShowOnPageLoad = true;
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        protected void menuBar_onCreateClick(int createLevel)
        {
            // Modified on: 07/02/2018, For: Add sub-level case
            // createLavel = 0: Category, 1: Sub-Category, 2: Itemgroup

            string tviewValue = tview.SelectedValue.ToString();
            int tableIndex = getTableIndexByValue(tviewValue);
            int rowIndex = getRowIndex(tableIndex, tviewValue);

            ClearDatail();
            Open_TofillData();
            SetVisibleOfDetail(1);
            Set_AboutLevel(createLevel);
            Session["IsEditProductcategory"] = "0";

            //txtParent.Text = dsTree.Tables[tableIndex].Rows[rowIndex]["ParentNo"].ToString();
            txtParent.Text = (createLevel > tableIndex)
                ? dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryCode"].ToString()
                : dsTree.Tables[tableIndex].Rows[rowIndex]["ParentNo"].ToString();

            // Added on: 09/01/2018
            btnSave.Visible = true;
            lblLevelNo.Text = string.Format("{0}", createLevel);

            //// Modified on: 21/02/2018
            //ddlCategoryType.Enabled = (txtParent.Text == "0") ? true : false;
            ddlCategoryType.Enabled = true;
            se_PriceDeviation.Enabled = true;
            se_QtyDeviation.Enabled = true;
            if (txtParent.Text != "0")
            {
                if (dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"] != DBNull.Value)
                    ddlCategoryType.SelectedValue = (createLevel > tableIndex)
                        ? string.Format("{0}", prodCat.GetCategoryType(txtParent.Text, LoginInfo.ConnStr))
                        : string.Format("{0}", dsTree.Tables[tableIndex].Rows[rowIndex]["CategoryType"]);
            }
            // End Added.
        }

        protected void menuBar_onEditClick()
        {
            Session["IsEditProductcategory"] = "1";
            string tviewValue = tview.SelectedValue.ToString();
            int tableIndex = getTableIndexByValue(tviewValue);
            int rowIndex = getRowIndex(tableIndex, tviewValue);

            Open_TofillData();
            btnSaveEdit.Visible = true;
            btnDelete.Visible = true;
            txtCategoryCode.Enabled = false;
            //cbTActive.Disabled = true;
            cbTActive.Enabled = btnSaveEdit.Visible;
        }

        protected void Open_TofillData()
        {
            menu_CmdBar.Items.FindByName("AddLevel01").Visible = false;
            menu_CmdBar.Items.FindByName("AddLevel02").Visible = false;
            menu_CmdBar.Items.FindByName("AddLevel03").Visible = false;
            menu_CmdBar.Items.FindByName("Edit").Visible = false;
            //menu_CmdBar.Items.FindByName("Delete").Visible = false;

            btnCancel.Visible = true;
            //cbTActive.Checked = true;

            txtSearch.Enabled = false;
            btnS.Enabled = false;

            txtCategoryCode.Enabled = true;
            txtCategoryName.Enabled = true;
            //cbTActive.Disabled = false;
            //cbTActive.Enabled = btnSaveEdit.Visible;

            txtAccCode.Enabled = true;
            //cbAuthRules.Checked = false;
            cbAuthRules.Enabled = true;
            txtApp.Enabled = true;

            // Modified on: 21/02/2018, By: Fon 
            //ddlCategoryType.Enabled = (txtParent.Text == "0") ? true : false;
            ddlCategoryType.Enabled = true;
            se_PriceDeviation.Enabled = true;
            se_QtyDeviation.Enabled = true;
        }

        protected void menuBar_onDeleteClick()
        {
            string tviewValue = tview.SelectedValue.ToString();
            int tableIndex = getTableIndexByValue(tviewValue);
            if (isDeleted(tviewValue, tableIndex + 1))
            {
                deleteInDataBase(txtCategoryCode.Text);
                GetData();
                SetTreeNode();
                ClearDatail();

                menu_CmdBar.Items.FindByName("AddLevel01").Visible = false;
                menu_CmdBar.Items.FindByName("AddLevel02").Visible = false;
                menu_CmdBar.Items.FindByName("AddLevel03").Visible = false;
                //menu_CmdBar.Items.FindByName("Delete").Visible = false;
            }
            else
            {

            }
        }
        #endregion

        #region About Category Type
        // Added on: 09/01/2018
        protected void ddlCategoryType_Init(object sender, EventArgs e)
        {
            DropDownList ddlCategoryType = (DropDownList)sender;
            ddlCategoryType.DataSource = prodCateType.GetList(LoginInfo.ConnStr);
            ddlCategoryType.DataTextField = "Description";
            ddlCategoryType.DataValueField = "Code";
            ddlCategoryType.DataBind();
        }
        // End Added.
        #endregion

        #region About Edit
        // Added on: 10/01/2018
        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string tviewValue = tview.SelectedValue.ToString();
            int tableIndex = getTableIndexByValue(tviewValue);

            if (SaveProcess(tableIndex))
            {
                SettingAfterSave();
                SetVisibleOfDetail(0);
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var id = tview.SelectedValue.ToString();
            
            
            var query = "SELECT TOP(1) CategoryCode FROM [IN].ProductCategory WHERE ParentNo=@id";

            var dt = prodCat.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", id) }, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                ShowAlert("No deleted, this code is not empty"); 

                return;
            }

            query = "SELECT ProductCode FROM [IN].[Product] WHERE ProductCate = @id";


            dt = prodCat.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", id) }, LoginInfo.ConnStr);

            if (dt != null && dt.Rows.Count > 0)
            {
                var codes = dt.AsEnumerable()
                    .Select(x => x.Field<string>("ProductCode"))
                    .ToArray();

                ShowAlert(string.Format("Some products are using this code.<br/> {0}", string.Join(", ", codes)));

                return;
            }
            lbl_ConfirmDelete.Text = string.Format("Do you want to delete this code '{0}'?", id);
            pop_ConfirmDelete.ShowOnPageLoad = true;

        }

        protected void btn_ConfirmDelete_Click(object sender, EventArgs e)
        {
            var id = tview.SelectedValue.ToString();
            var query = "DELETE FROM [IN].ProductCategory WHERE CategoryCode=@id";
            var dt = prodCat.DbExecuteQuery(query, new Blue.DAL.DbParameter[] { new Blue.DAL.DbParameter("@id", id) }, LoginInfo.ConnStr);

            Response.Redirect("ProdCatLst.aspx");
        }

        // End Added.
        #endregion

        protected void ShowAlert(string text)
        {
            lbl_Warning.Text = text;
            pop_Warning.ShowOnPageLoad = true;
        }
    }
}