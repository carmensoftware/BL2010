using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;

namespace BlueLedger.PL.Master.Pc
{
    public partial class MasterMenu : BaseClass.BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.APP.Module _module = new Blue.BL.APP.Module();

        [Category("Misc"), Description("Gets or set a starter id for display all menu under"), Browsable(true)]
        public string ParentID { get; set; }

        #endregion

        #region "Attributes"

        protected void Page_Load(object sender, EventArgs e)
        {
            var dtRoot = _module.GetRoot2(LoginInfo.LoginName, LoginInfo.ConnStr);

            foreach (DataRow drRoot in dtRoot.Rows)
            {
                var item = new MenuItem
                {
                    Text = drRoot["Desc"].ToString(),
                    NavigateUrl = drRoot["NavigateURL"].ToString(),
                    Selectable = (drRoot["NavigateURL"].ToString() != string.Empty)
                };

                // If node has child, adding child nodes before adding the current node.
                if (_module.HasChild2(drRoot["ID"].ToString(), LoginInfo.ConnStr))
                {
                    DisplayMenu(drRoot["ID"].ToString(), item);
                }

                menu_Main.Items.Add(item);
            }
        }

        private void DisplayMenu(string parentID, MenuItem parentItem)
        {
            // Display child menu.
            var dtChile = _module.GetList2(parentID, LoginInfo.LoginName, LoginInfo.ConnStr);

            foreach (DataRow drChild in dtChile.Rows)
            {
                var item = new MenuItem
                {
                    Text = drChild["Desc"].ToString(),
                    NavigateUrl = drChild["NavigateURL"].ToString()
                };

                if (_module.HasChild2(drChild["ID"].ToString(), LoginInfo.ConnStr)){
                    DisplayMenu(drChild["ID"].ToString(), item);
                }

                // If the current child node is leaf node and has transtype data attached, add transtype as menu item to the node.
                if (parentItem != null)
                {
                    parentItem.ChildItems.Add(item);
                }
                else
                {
                    menu_Main.Items.Add(item);
                }
            }
        }

        //protected void Page_Load(object sender, EventArgs e)//{            
        //    DataTable dtRoot = modlue.GetRoot2(LoginInfo.LoginName, LoginInfo.ConnStr);

        //    foreach (DataRow drRoot in dtRoot.Rows)
        //    {
        //        DevExpress.Web.ASPxMenu.MenuItem item   = new DevExpress.Web.ASPxMenu.MenuItem();
        //        item.Text                               = drRoot["Desc"].ToString();
        //        item.NavigateUrl                        = drRoot["NavigateURL"].ToString();                    

        //        // If node has child, adding child nodes before adding the current node.
        //        if (modlue.HasChild2(drRoot["ID"].ToString(), LoginInfo.ConnStr))
        //        {
        //            this.DisplayMenu(drRoot["ID"].ToString(), item);
        //        }

        //        ASPxMenu.Items.Add(item);
        //    }
        //}

        //private void DisplayMenu(string ParentID, DevExpress.Web.ASPxMenu.MenuItem ParentItem)
        //{
        //    // Display child menu.
        //    DataTable dtChile = modlue.GetList2(ParentID, LoginInfo.LoginName, LoginInfo.ConnStr);

        //    foreach (DataRow drChild in dtChile.Rows)
        //    {
        //        DevExpress.Web.ASPxMenu.MenuItem item   = new DevExpress.Web.ASPxMenu.MenuItem();
        //        item.Text                               = drChild["Desc"].ToString();
        //        item.NavigateUrl                        = drChild["NavigateURL"].ToString();

        //        if (modlue.HasChild2(drChild["ID"].ToString(), LoginInfo.ConnStr))
        //        {
        //            DisplayMenu(drChild["ID"].ToString(), item);
        //        }

        //        // If the current child node is leaf node and has transtype data attached, add transtype as menu item to the node.


        //        if (ParentItem != null)
        //        {
        //            ParentItem.Items.Add(item);
        //        }
        //        else
        //        {
        //            ASPxMenu.Items.Add(item);
        //        }
        //    }
        //}

        #endregion
    }
}