using System;
using System.ComponentModel;
using System.Data;

namespace BlueLedger.PL.Master
{
    public partial class MenuList : BaseClass.BaseUserControl
    {
        #region "Attributes"

        private readonly Blue.BL.APP.Module _module = new Blue.BL.APP.Module();

        [Category("Misc"), Description("Gets or set a starter id for display all menu under"), Browsable(true)]
        public string ParentID { get; set; }

        #endregion

        #region "Operations"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Display menu list
                if (!string.IsNullOrEmpty(ParentID))
                {
                    // Add Group
                    var dtGroup = _module.GetList2(ParentID, LoginInfo.LoginName, LoginInfo.ConnStr);

                    foreach (DataRow drGroup in dtGroup.Rows)
                    {
                        if (drGroup["SubModule"].ToString().ToUpper() != "RP")
                        {
                            var grp = new DevExpress.Web.ASPxNavBar.NavBarGroup {Text = drGroup["Desc"].ToString()};

                            // Add Item
                            if (_module.HasChild2(drGroup["ID"].ToString(), LoginInfo.ConnStr))
                            {
                                DisplayMenu(drGroup["ID"].ToString(), grp);
                            }

                            nav_Modules.Groups.Add(grp);
                        }
                    }
                }
            }
        }

        private void DisplayMenu(string parentID, DevExpress.Web.ASPxNavBar.NavBarGroup navGrp)
        {
            // Display child menu.
            var dtChile = _module.GetList2(parentID, LoginInfo.LoginName, LoginInfo.ConnStr);

            foreach (DataRow drChild in dtChile.Rows)
            {
                if (_module.HasChild2(drChild["ID"].ToString(), LoginInfo.ConnStr))
                {
                    DisplayMenu(drChild["ID"].ToString(), navGrp);
                }
                else
                {
                    var item = new DevExpress.Web.ASPxNavBar.NavBarItem
                    {
                        Text = drChild["Desc"].ToString(),
                        NavigateUrl = drChild["NavigateURL"].ToString()
                    };
                    navGrp.Items.Add(item);
                }
            }
        }

        #endregion
    }
}