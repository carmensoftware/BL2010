using System;
using System.Web.UI;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class favorites : BaseUserControl
    {
        #region "Attributes"

        private Blue.BL.Application.UserShortcut favorite = new Blue.BL.Application.UserShortcut();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Jump to favorites page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Favorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Favorites.SelectedItem.Value != string.Empty)
            {
                Response.Redirect(ddl_Favorites.SelectedItem.Value);
            }
        }

        private void Page_Setting()
        {
            //DataTable dtFavorutes = favorite.GetList(LoginInfo.LoginName, LoginInfo.ConnStr);

            //if (dtFavorutes != null)
            //{
            //    ddl_Favorites.DataSource        = dtFavorutes;
            //    ddl_Favorites.DataTextField     = "Name";
            //    ddl_Favorites.DataValueField    = "URL";
            //    ddl_Favorites.DataBind();
            //}

            //ddl_Favorites.Items.Insert(0, new ListItem("----- Favorites -----", ""));
            //ddl_Favorites.Items.Add(new ListItem("--------------------", ""));
            //ddl_Favorites.Items.Add(new ListItem("Organize Favorites..", "~/Favorites/FavoritesEdit.aspx"));
        }

        protected void img_AddFav_Click(object sender, ImageClickEventArgs e)
        {
            //BL.Application.UserShortcut favorite = new BL.Application.UserShortcut();

            //if (!favorite.IsExist(LoginInfo.LoginName, Request.Url.ToString(), LoginInfo.ConnStr))
            //{
            //    bool added = favorite.Add(Page.Title, Request.Url.ToString(), LoginInfo.LoginName, LoginInfo.ConnStr);

            //    if (added)
            //    {
            //        Response.Redirect(Request.Url.ToString());
            //    }
            //}
        }

        /// <summary>
        ///     Retrieve data and binding controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page_Setting();
            }
        }

        #endregion
    }
}