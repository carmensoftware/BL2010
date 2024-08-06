using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Inventory
{
    public partial class ProdCat : BasePage
    {
        #region "Attributes"

        private string MsgError = string.Empty;
        private Blue.BL.GL.Account.Account account = new Blue.BL.GL.Account.Account();

        private DataSet dsProdCat = new DataSet();
        private DataSet dsProdCatDisplay = new DataSet();
        private Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        /// <summary>
        ///     Gets or set business unit related to user.
        /// </summary>
        //private DataSet dsProdCat
        //{
        //    get
        //    {
        //        if (ViewState["dsProdCat"] != null)
        //        {
        //            this._dsProdCat = (DataSet)ViewState["dsProdCat"];
        //        }
        //        return this._dsProdCat;
        //    }
        //    set
        //    {
        //        this._dsProdCat = value;
        //        ViewState["dsProdCat"] = this._dsProdCat;
        //    }
        //}

        #endregion

        #region "Operations"
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.Page_Retrieve();
                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Sub Category", "SC"));
                ListPage.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create Item Group", "IG"));
                ListPage.DataBind();
            }
            //else
            //{
            //    dsProdCat = (DataSet)Session["dsProdCat"];
            //}

            ListPage.CreateItems.Menu.ItemClick += menu_ItemClick;
            base.Page_Load(sender, e);
        }

        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SC":
                    break;
                case "IG":
                    break;
            }
        }


        /// <summary>
        /// Get business unit data related to login user.
        /// </summary>
        //private void Page_Retrieve()
        //{

        //   prodCat.GetList(dsProdCat, ref MsgError, LoginInfo.ConnStr);

        //   Page_Setting();

        //   Session["dsProdCat"] = dsProdCat;


        //}

        /// <summary>
        /// Display business unit data which retrieved from Page_Retrieve procedure.
        /// </summary>
        //private void Page_Setting()
        //{
        //    this.BindingTreeView();

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void imgb_AddParent_Click(object sender, ImageClickEventArgs e)
        //{

        //    dsProdCat.Tables[prodCat.TableName].Rows.Clear();

        //    DataRow drAddParent = dsProdCat.Tables[prodCat.TableName].NewRow();

        //    // Assign default value
        //    drAddParent["LevelNo"]      = 1; //Default
        //    drAddParent["ParentNo"]     = 0; //Default
        //    drAddParent["CategoryCode"] = prodCat.ProdCat_GeLevelOneNewCategoryCode(LoginInfo.ConnStr);
        //    drAddParent["CategoryName"] = string.Empty;
        //    drAddParent["CategoryType"] = 1;
        //    drAddParent["IsActive"]     = true;
        //    drAddParent["TaxAccCode"]   = string.Empty;
        //    drAddParent["AuthRules"]    = true;
        //    drAddParent["approvalLevel"] = System.DBNull.Value;
        //    drAddParent["CreatedDate"]  = ServerDateTime;
        //    drAddParent["CreatedBy"]    = LoginInfo.LoginName;
        //    drAddParent["UpdatedDate"]  = ServerDateTime;
        //    drAddParent["UpdatedBy"]    = LoginInfo.LoginName;

        // Add new row
        //dsProdCat.Tables[prodCat.TableName].Rows.Add(drAddParent);

        //Session["dsProdCat"] = dsProdCat;

        // Binding Controls for display data.
        //this.Binding_Controls(drAddParent);
    }


    //private void SetNode(string nodeName, string nodeText)
    //{
    //    TreeNode selectedNode = GetTreeNode(nodeName, nodeText);
    //    if (selectedNode != null)
    //    {
    //        tv_ProdCat.SelectedNode = selectedNode;
    //        selectedNode.EnsureVisible();
    //        selectedNode.TreeView.Focus();
    //        selectedNode.TreeView.Select();
    //     }
    //}

    //private TreeNode GetTreeNode(string nodeName, string nodeText)
    //{
    //    TreeNode selectedNode = null;
    //    TreeNode[] tNodes = ktvClientCaseProjectInfo.Nodes.Find(nodeName, true);
    //    foreach (TreeNode tNode in tNodes)
    //    {
    //        if (tNode.Text == nodeText)
    //        {
    //            selectedNode = tNode;
    //            break;
    //        }
    //    }
    //    return selectedNode;
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void imgb_AddChild_Click(object sender, ImageClickEventArgs e)
    //{
    //    dsProdCat.Tables[prodCat.TableName].Rows.Clear();

    //    DataRow drAddChild = dsProdCat.Tables[prodCat.TableName].NewRow();

    //    if (tv_ProdCat.SelectedValue.ToString() != string.Empty)
    //    {
    //        // Assign default value
    //        if (tv_ProdCat.SelectedNode.Depth.ToString() == "0")
    //        {
    //            drAddChild["LevelNo"]       = 2;
    //            drAddChild["ParentNo"]      = tv_ProdCat.SelectedNode.Value;
    //            drAddChild["CategoryCode"]  = prodCat.ProdCat_GeLevelTwoNewCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()),LoginInfo.ConnStr);
    //            drAddChild["CategoryName"]  = string.Empty;
    //            drAddChild["CategoryType"]  = 1;
    //            drAddChild["IsActive"]      = true;
    //            drAddChild["TaxAccCode"]    = prodCat.ProdCat_GetTaxAccCodeByCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()), LoginInfo.ConnStr);
    //            drAddChild["AuthRules"]     = true;
    //            drAddChild["ApprovalLevel"] = prodCat.ProdCat_GeApprovalLevelByCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()), LoginInfo.ConnStr);
    //            drAddChild["CreatedDate"]   = ServerDateTime;
    //            drAddChild["CreatedBy"]     = LoginInfo.LoginName;
    //            drAddChild["UpdatedDate"]   = ServerDateTime;
    //            drAddChild["UpdatedBy"]     = LoginInfo.LoginName;

    //            // Add new row
    //            dsProdCat.Tables[prodCat.TableName].Rows.Add(drAddChild);

    //            Session["dsProdCat"] = dsProdCat;

    //            // Binding Controls for display data.
    //            this.Binding_Controls(drAddChild);
    //        }
    //        else if (tv_ProdCat.SelectedNode.Depth.ToString() == "1")
    //        {
    //            drAddChild["LevelNo"]       = 3;
    //            drAddChild["ParentNo"]      = tv_ProdCat.SelectedNode.Value;
    //            drAddChild["CategoryCode"]  = prodCat.ProdCat_GeLevelThreeNewCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()), LoginInfo.ConnStr);
    //            drAddChild["CategoryName"]  = string.Empty;
    //            drAddChild["CategoryType"]  = 1;
    //            drAddChild["IsActive"]      = true;
    //            drAddChild["TaxAccCode"]   = prodCat.ProdCat_GetTaxAccCodeByCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()), LoginInfo.ConnStr);
    //            drAddChild["AuthRules"]     = true;
    //            drAddChild["ApprovalLevel"] = prodCat.ProdCat_GeApprovalLevelByCategoryCode(Convert.ToInt32(tv_ProdCat.SelectedNode.Value.ToString()), LoginInfo.ConnStr);
    //            drAddChild["CreatedDate"]   = ServerDateTime;
    //            drAddChild["CreatedBy"]     = LoginInfo.LoginName;
    //            drAddChild["UpdatedDate"]   = ServerDateTime;
    //            drAddChild["UpdatedBy"]     = LoginInfo.LoginName;

    //            // Add new row
    //            dsProdCat.Tables[prodCat.TableName].Rows.Add(drAddChild);

    //            Session["dsProdCat"] = dsProdCat;

    //            // Binding Controls for display data.
    //            this.Binding_Controls(drAddChild);


    //        }
    //        else if (tv_ProdCat.SelectedNode.Depth.ToString() == "2")
    //        {
    //            // show message for not allowed to child node in ths level.
    //            Response.Write("<script>window.alert('Not Allowed To Add Child Node in This Level')</script>");
    //        }


    //    }
    //    else
    //    { 

    //        // show message as select one node
    //        Response.Write("<script>window.alert('Please Select One Node in Tree List')</script>");

    //    }
    //}

    /// <summary>
    /// Binding Controls
    /// </summary>
    /// <param name="drBinding"></param>
    //private void Binding_Controls(DataRow drBinding)
    //{
    //    //txt_CategoryCode.Enabled = true;

    //    txt_LevelNo.Text                = drBinding["LevelNo"].ToString();
    //    txt_ParentNo.Text               = drBinding["ParentNo"].ToString();
    //    txt_CategoryCode.Text           = drBinding["CategoryCode"].ToString();
    //    txt_CategoryName.Text           = drBinding["CategoryName"].ToString();
    //    txt_TaxAccCode.Text             = drBinding["TaxAccCode"].ToString();
    //    chk_AuthRules.Checked           = (bool)drBinding["AuthRules"];

    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void imgb_RemoveNode_Click(object sender, ImageClickEventArgs e)
    //{
    //    // Get categorycode by selected node value.
    //    string categoryCode = (tv_ProdCat.SelectedNode.Value.ToString());


    //    // Delete selected node
    //    for (int i = dsProdCat.Tables[prodCat.TableName].Rows.Count - 1; i >= 0; i--)
    //    {
    //        DataRow drRemove = dsProdCat.Tables[prodCat.TableName].Rows[i];

    //        if (drRemove.RowState != DataRowState.Deleted)
    //        {
    //            if (drRemove["CategoryCode"].ToString() == categoryCode.Trim())
    //            {
    //                drRemove.Delete();
    //                continue;
    //            }
    //        }
    //    }

    //    // Save to database
    //    bool deleted = prodCat.Save(dsProdCat, LoginInfo.ConnStr);

    //    if (deleted)
    //    {
    //        //dsProdCat.Clear();
    //        // Save changed to session
    //        Session["dsProdCat"] = dsProdCat;


    //        if (tv_ProdCat.SelectedNode.Depth.ToString() == "0")
    //        {
    //            tv_ProdCat.Nodes.Remove(tv_ProdCat.SelectedNode);
    //        }
    //        else
    //        {

    //            tv_ProdCat.SelectedNode.Parent.ChildNodes.Remove(tv_ProdCat.SelectedNode);
    //        }


    //        this.ClearControls();

    //        this.MessageBox("Deleted Successful !");

    //    }
    //    else
    //    {
    //        //Display Error
    //    }


    //}

    /// <summary>
    /// Message box
    /// </summary>
    /// <param name="msg"></param>
    //private void MessageBox(string msg)
    //{
    //    Label lbl = new Label();
    //    lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
    //    Page.Controls.Add(lbl);
    //}


    /// <summary>
    /// 
    /// </summary>
    //private void ClearControls()
    //{

    //    txt_ParentNo.Text       = string.Empty;
    //    txt_LevelNo.Text        = string.Empty;
    //    txt_CategoryCode.Text   = string.Empty;
    //    txt_CategoryName.Text   = string.Empty;
    //    txt_TaxAccCode.Text     = string.Empty;
    //    //txt_AccountDesc.Text    = string.Empty;
    //    chk_AuthRules.Checked   = false;
    //    txt_ApprovalLevel.Text  = string.Empty;
    //}


    /// <summary>
    /// Binding Tree list
    /// </summary>
    //private void BindingTreeView()
    //{

    //    // Create the root tree node.
    //    dsProdCat.Tables[prodCat.TableName].DefaultView.RowFilter = "LevelNo = '" + "1" + "'";
    //    DataView dvParent = dsProdCat.Tables[prodCat.TableName].DefaultView;


    //    // Category List            
    //    foreach (DataRowView drParent in dvParent)
    //    {

    //        // Add Root Node
    //        TreeNode root  = new TreeNode();
    //        root.Text      = drParent["CategoryCode"] + "  " + (drParent["CategoryName"] == string.Empty ? Convert.ToString("-") : drParent["CategoryName"].ToString());
    //        root.Value     = (drParent["CategoryCode"]   == DBNull.Value ? Convert.ToString("-") : drParent["CategoryCode"].ToString());

    //        // Add Childes Node
    //        dsProdCat.Tables[prodCat.TableName].DefaultView.RowFilter = "LevelNo = '" + "2" + "' " + "AND ParentNo = '" + drParent["CategoryCode"] + "' "; 
    //        DataView dvChild = dsProdCat.Tables[prodCat.TableName].DefaultView;

    //        foreach (DataRowView drChild in dvChild)
    //        {
    //            TreeNode child = new TreeNode();

    //            child.Text = drChild["CategoryCode"].ToString() + "  " + (drChild["CategoryName"] == null ? string.Empty : drChild["CategoryName"].ToString());
    //            child.Value = drChild["CategoryCode"].ToString();

    //            // Add Childes Node
    //            dsProdCat.Tables[prodCat.TableName].DefaultView.RowFilter = "LevelNo = '" + "3" + "' " + "AND ParentNo = '" + drChild["CategoryCode"] + "' "; 
    //            DataView dvSubChild = dsProdCat.Tables[prodCat.TableName].DefaultView;

    //            foreach (DataRowView drSubChild in dvSubChild)
    //            {
    //                TreeNode subchild = new TreeNode();

    //                subchild.Text  = drSubChild["CategoryCode"].ToString() + "  " + (drSubChild["CategoryName"] == null ? string.Empty : drSubChild["CategoryName"].ToString());
    //                subchild.Value = drSubChild["CategoryCode"].ToString();


    //                child.ChildNodes.Add(subchild);


    //            }
    //            root.ChildNodes.Add(child);

    //        }

    //        // Add Node
    //        tv_ProdCat.Nodes.Add(root);

    //    }

    //}

    /// <summary>
    /// Search node by value all node level.
    /// </summary>
    /// <param name="treeNode"></param>
    /// <param name="treeValue"></param>
    //private void SelectNode(TreeNode treeNode, string treeValue)
    //{
    //    foreach (TreeNode node in treeNode.ChildNodes)
    //    {
    //        if (node.Value == treeValue)
    //        {
    //            node.Select();
    //            this.ExpandNode(node);
    //            break;
    //        }
    //        else
    //        {
    //            if (node.ChildNodes.Count > 0)
    //            {
    //                SelectNode(node, treeValue);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// Expand All Sepecified Node's Parent
    /// </summary>
    /// <param name="treeNode"></param>
    //private void ExpandNode(TreeNode treeNode)
    //{
    //    treeNode.Parent.Expand();

    //    if (treeNode.Parent.Parent != null)
    //    {
    //        this.ExpandNode(treeNode.Parent);
    //    }
    //}

    /// <summary>
    /// Treeview Node Selected change event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void tv_ProdCat_SelectedNodeChanged(object sender, EventArgs e)
    //{

    //    txt_CategoryCode.Enabled = false;

    //    if (dsProdCat.Tables[prodCat.TableName] != null)
    //    {
    //        dsProdCat.Tables[prodCat.TableName].Rows.Clear();
    //    }

    //    // Retrieve data by selected tree node value.
    //    prodCat.GetListByCategoryCode(dsProdCat, tv_ProdCat.SelectedValue.ToString(), LoginInfo.ConnStr);


    //    txt_LevelNo.Text = dsProdCat.Tables[prodCat.TableName].Rows[0]["LevelNo"].ToString();

    //    if (Convert.ToInt32(txt_LevelNo.Text.ToString()) == 1)
    //    {
    //        lbl_LevelDesc.Text = "Category";
    //    }
    //    else if (Convert.ToInt32(txt_LevelNo.Text.ToString()) == 2)
    //    {
    //        lbl_LevelDesc.Text = "Sub Category";
    //    }
    //    else if (Convert.ToInt32(txt_LevelNo.Text.ToString()) == 3)
    //    {
    //        lbl_LevelDesc.Text = "Item Group";
    //    }


    //    txt_ParentNo.Text      = dsProdCat.Tables[prodCat.TableName].Rows[0]["ParentNo"].ToString();
    //    txt_CategoryCode.Text  = dsProdCat.Tables[prodCat.TableName].Rows[0]["CategoryCode"].ToString();
    //    txt_CategoryName.Text  = dsProdCat.Tables[prodCat.TableName].Rows[0]["CategoryName"].ToString();
    //    txt_TaxAccCode.Text    = dsProdCat.Tables[prodCat.TableName].Rows[0]["TaxAccCode"].ToString();
    //    txt_ApprovalLevel.Text = dsProdCat.Tables[prodCat.TableName].Rows[0]["ApprovalLevel"].ToString();
    //    chk_AuthRules.Checked  = (bool)dsProdCat.Tables[prodCat.TableName].Rows[0]["AuthRules"];


    //    Session["dsProdCat"] = dsProdCat;

    //}

    /// <summary>
    /// Save event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btn_Save_Click(object sender, EventArgs e)
    //{
    //    this.Page.Validate();
    //    if (Page.IsValid)
    //    {
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["LevelNo"]       = txt_LevelNo.Text;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["ParentNo"]      = txt_ParentNo.Text;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["CategoryCode"]  = txt_CategoryCode.Text;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["CategoryName"]  = txt_CategoryName.Text;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["IsActive"]      = true;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["TaxAccCode"]    = txt_TaxAccCode.Text.Trim();
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["ApprovalLevel"] = Convert.ToInt32(txt_ApprovalLevel.Text.ToString());
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["AuthRules"]     = (bool)chk_AuthRules.Checked;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["CreatedDate"]   = ServerDateTime;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["CreatedBy"]     = LoginInfo.LoginName;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["UpdatedDate"]   = ServerDateTime;
    //        dsProdCat.Tables[prodCat.TableName].Rows[0]["UpdatedBy"]     = LoginInfo.LoginName;

    //        bool result = prodCat.Save(dsProdCat, LoginInfo.ConnStr);

    //        if (result)
    //        {

    //            // Save changed to session
    //            //Session["dsProdCat"] = dsProdCat;

    //            dsProdCat.Clear();

    //            tv_ProdCat.Nodes.Clear();

    //            prodCat.GetList(dsProdCat, ref MsgError, LoginInfo.ConnStr);

    //            //refresh page.
    //            this.Page_Setting();


    //            if (tv_ProdCat.SelectedNode != null)
    //            {
    //                TreeNode Node = tv_ProdCat.SelectedNode;

    //                if (Node.ChildNodes != null && Node.ChildNodes.Count != 0 && Node.Expanded.HasValue)
    //                {
    //                    Node.ExpandAll();

    //                }

    //            }
    //            else
    //            {
    //                tv_ProdCat.ExpandAll();

    //            }


    //        }
    //        else
    //        {
    //            //Display Error
    //        }
    //    }

    //}

    //private void ExpandParent(TreeNode tn)
    //{
    //    TreeNode parent = tn.Parent;
    //    if (parent != null)
    //    {
    //        ExpandParent(parent);
    //    }
    //    else
    //    {
    //        tn.Expand();
    //    }
    //}
    //private TreeNode FindNode(TreeView tv, string text)
    //{
    //    foreach (TreeNode node in tv.Nodes)
    //    {
    //        if (node.Value == text)
    //        {
    //            return node;
    //        }
    //        else
    //        {
    //            TreeNode child = FindChildNode(node, text);
    //            if (child != null)
    //            {
    //                return child;
    //            }
    //        }
    //    }

    //    return (TreeNode)null;
    //}
    //private TreeNode FindChildNode(TreeNode node, string text)
    //{
    //    foreach (TreeNode child in node.ChildNodes)
    //    {
    //        if (child.Value == text)
    //        {
    //            return child;
    //        }
    //        else
    //        {
    //            TreeNode childNode = FindChildNode(child, text);
    //            if (childNode != null)
    //            {
    //                return childNode;
    //            }
    //        }
    //    }
    //    return (TreeNode)null;
    //}


    /// <summary>
    /// Cancel event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btn_Cancel_Click(object sender, EventArgs e)
    //{
    //    this.Page.Validate();
    //    if (Page.IsValid)
    //    {
    //        // Get categorycode by selected node value.
    //        string categoryCode = (txt_CategoryCode.Text.ToString());


    //        // Delete added row
    //        for (int i = dsProdCat.Tables[prodCat.TableName].Rows.Count - 1; i >= 0; i--)
    //        {
    //            DataRow drCanceling = dsProdCat.Tables[prodCat.TableName].Rows[i];

    //            if (drCanceling.RowState != DataRowState.Deleted)
    //            {
    //                if ((string)drCanceling["CategoryCode"] == categoryCode &&
    //                    drCanceling.RowState == DataRowState.Added)
    //                {
    //                    drCanceling.Delete();
    //                    continue;
    //                }
    //            }
    //        }

    //        this.ClearControls();

    //        //dsProdCat.Clear();
    //        ////tv_ProdCat.Nodes.Clear();

    //        //////refresh page.
    //        //this.Page_Retrieve();
    //    }

    //}

    #endregion
}

//}