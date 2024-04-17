using System;
using System.ComponentModel;
using System.Data;

namespace BlueLedger.PL.Master.In
{
    public partial class MasterMenu : BaseClass.BaseUserControl
    {
        #region "Attributes"

        [Category("Misc"), Description("Gets or set a starter id for display all menu under"), Browsable(true)]
        public string ParentID { get; set; }

        private readonly Blue.BL.APP.Module _module = new Blue.BL.APP.Module();


        #endregion

        #region "Attributes"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ASPxMenu.Items.Count > 0)
            {
                ASPxMenu.Items.Clear();
            }
            var dtRoot = _module.GetRoot2(LoginInfo.LoginName, LoginInfo.ConnStr);

            if (dtRoot != null)
            {
                foreach (DataRow drRoot in dtRoot.Rows)
                {
                    var item = new DevExpress.Web.ASPxMenu.MenuItem
                    {
                        Text = drRoot["Desc"].ToString(),
                        NavigateUrl = drRoot["NavigateURL"].ToString()
                    };

                    // If node has child, adding child nodes before adding the current node.
                    if (_module.HasChild2(drRoot["ID"].ToString(), LoginInfo.ConnStr))
                    {
                        DisplayMenu(drRoot["ID"].ToString(), item);
                    }

                    ASPxMenu.Items.Add(item);
                }
            }

            //}
        }

        private void DisplayMenu(string parentID, DevExpress.Web.ASPxMenu.MenuItem parentItem)
        {
            // Display child menu.
            var dtChile = _module.GetList2(parentID, LoginInfo.LoginName, LoginInfo.ConnStr);

            foreach (DataRow drChild in dtChile.Rows)
            {
                var item = new DevExpress.Web.ASPxMenu.MenuItem
                {
                    Text = drChild["Desc"].ToString(),
                    NavigateUrl = drChild["NavigateURL"].ToString()
                };

                if (_module.HasChild2(drChild["ID"].ToString(), LoginInfo.ConnStr))
                {
                    DisplayMenu(drChild["ID"].ToString(), item);
                }

                // If the current child node is leaf node and has transtype data attached, add transtype as menu item to the node.


                if (parentItem != null)
                {
                    parentItem.Items.Add(item);
                }
                else
                {
                    ASPxMenu.Items.Add(item);
                }
            }
        }

        #endregion
    }
}