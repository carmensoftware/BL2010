using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web.ASPxEditors;
using BlueLedger.PL.BaseClass;


namespace BlueLedger.PL.Option.Admin.WF
{
    public partial class Workflow : BasePage
    {
        #region "Attribute"

        private readonly Blue.BL.APP.WF WF = new Blue.BL.APP.WF();
        private readonly Blue.BL.APP.WFDt WFDt = new Blue.BL.APP.WFDt();


        private DataSet dsWF = new DataSet();

        //private DataSet dsWF
        //{
        //    get
        //    {
        //        if (Session["dsWF"] == null)
        //            Session["dsWF"] = new DataSet();

        //        return (DataSet)Session["dsWF"];
        //    }
        //    set { Session["dsWF"] = value; }
        //}

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsWF = (DataSet)Session["dsWF"];

                ddl_Module.DataSource = dsWF.Tables[WF.TableName];
                ddl_Module.DataBind();
            }

        }

        private void Page_Retrieve()
        {
            // get Workflow data to dsWF
            WF.GetList(dsWF, LoginInfo.ConnStr);
            Session.Remove("dsWF");
            Session["dsWF"] = dsWF;

            Page_Setting();
        }

        private void Page_Setting()
        {
            ddl_Module.DataSource = dsWF.Tables[WF.TableName];
            ddl_Module.DataBind();
        }


        // ----------------------------------------------------------------------------------------------------


        protected void ddl_Module_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Module.SelectedIndex > -1)
            {
                int wfID = Convert.ToInt32(ddl_Module.SelectedItem.Value);
                WFDt.GetList(dsWF, wfID, LoginInfo.ConnStr);
                // Display configurations detail            
                grd_WFDt.DataSource = dsWF.Tables[WFDt.TableName];
                grd_WFDt.DataBind();
            }
        }


        protected void grd_WFDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

    }
}