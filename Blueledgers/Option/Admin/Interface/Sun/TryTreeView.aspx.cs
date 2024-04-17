using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class AccountMapp : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private string MsgError = string.Empty;
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {

            }

            base.Page_Load(sender, e);
        }
        private void Page_Retrieve()
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
            string connetionString = LoginInfo.ConnStr;
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();

                string strsqlLevel01 = "select * from [in].productcategory where LevelNo = '1' ";
                string strsqlLevel02 = "select * from [in].productcategory where LevelNo = '2' ";
                string treeView = strsqlLevel01 + ";" + strsqlLevel02;

                SqlCommand mycommand2 = new SqlCommand(treeView, cnn);
                SqlDataAdapter da = new SqlDataAdapter(mycommand2);

                DataSet dsTree = new DataSet();
                da.Fill(dsTree);
                dsTree.Tables[0].TableName = "Level01";
                dsTree.Tables[1].TableName = "Level02";


                //Syntax == DataRelation DR = new DataRelation("Name", parentColumn ,childColumn, bool createConstraints);
                //DataRelation DR = new DataRelation("Level", dsTree.Tables["Level01"].Columns["CategoryCode"], dsTree.Tables["Level02"].Columns["ParentNo"]); //error
                //DataRelation DR = new DataRelation("Level", dsTree.Tables["Level01"].Columns["CategoryCode"], dsTree.Tables["Level02"].Columns["CategoryCode"]); // still error
                
                DataRelation DR = new DataRelation("Level", dsTree.Tables["Level01"].Columns["ParentNo"], dsTree.Tables["Level02"].Columns["CategoryCode"]); // still error
                //DataRelation DR = new DataRelation("Level",
                //    new DataColumn[] { dsTree.Tables["Level01"].Columns["ParentNo"], dsTree.Tables["Level01"].Columns["CategoryCode"] },
                //    new DataColumn[] { dsTree.Tables["Level02"].Columns["ParentNo"], dsTree.Tables["Level02"].Columns["CategoryCode"] });

                dsTree.Relations.Add(DR);   
                // ^------------------------------ Always jump out on this line.

                foreach (DataRow rowLevel01 in dsTree.Tables["Level01"].Rows)
                {
                    TreeNode tnLevel01 = new TreeNode();
                    tnLevel01.Text = rowLevel01["CategoryName"].ToString();
                    tnLevel01.Value = rowLevel01["CategoryCode"].ToString();
                    //tview.Nodes.Add(tnLevel01);

                    //foreach (DataRow rowLevel02 in rowLevel01.GetChildRows("Level"))
                    //{
                    //    TreeNode tnLevel02 = new TreeNode();
                    //    tnLevel02.Text = rowLevel02["CategoryName"].ToString();
                    //    tnLevel02.Value = rowLevel02["CategoryCode"].ToString();

                    //    tnLevel01.ChildNodes.Add(tnLevel02);
                    //    //tnLevel01.Value = Convert.ToString(rowLevel02["CategoryCode"]);
                    //}

                    tview.Nodes.Add(tnLevel01);
                    tview.CollapseAll(); //<-------- ?
                }
            }
            catch (Exception ex)
            {
                cnn.Close();
                Response.Write(ex);
            }

        }
    }
}