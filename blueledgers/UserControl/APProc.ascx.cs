using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class APProc : BaseUserControl
    {
        #region "Attributies"

        private readonly DataSet dsProc = new DataSet();

        private readonly Blue.BL.AP.VendorTool proc = new Blue.BL.AP.VendorTool();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page_Retrieve();
            Page_Setting();
        }

        /// <summary>
        ///     Retrieve Data
        /// </summary>
        private void Page_Retrieve()
        {
            proc.GetVendorToolList(dsProc, LoginInfo.ConnStr);
            Session["dsProc"] = dsProc;
        }

        /// <summary>
        ///     Set data to grid, lable
        /// </summary>
        private void Page_Setting()
        {
            grd_APProc.DataSource = dsProc.Tables[proc.TableName];
            grd_APProc.DataBind();
        }

        /// <summary>
        ///     Set HyperLink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_APProc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnk_Proc") != null)
                {
                    var lnk_Proc = (HyperLink) e.Row.FindControl("lnk_Proc");
                    lnk_Proc.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                    lnk_Proc.NavigateUrl = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                    lnk_Proc.Target = "_new";
                }
            }
        }

        #endregion
    }
}