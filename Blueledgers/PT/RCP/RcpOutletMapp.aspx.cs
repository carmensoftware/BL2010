using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlueLedger.PL.BaseClass;
using AjaxControlToolkit;
using System.Data.SqlClient;

namespace BlueLedger.PL.PT.RCP
{
    public partial class RcpOutletMapp : BasePage
    {
        #region "Attributes"
        private readonly Blue.BL.Option.Inventory.StoreLct stoLocate = new Blue.BL.Option.Inventory.StoreLct();

        DataSet dsRcpMapp = new DataSet();
        private string outletMappName = "PosOutletLocation";
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtPosOutletLocation = Get_PosOutletLocation();
                dsRcpMapp.Tables.Add(dtPosOutletLocation);

                Page_Retrieve();
            }
            else
            {
                dsRcpMapp = (DataSet)Session["dsRcpMapp"];
            }
        }

        private void Page_Retrieve()
        {
            Session["dsRcpMapp"] = dsRcpMapp;
            Page_Setting();
        }

        private void Page_Setting()
        {
            grd_OL_Mapp.DataSource = dsRcpMapp.Tables[outletMappName];
            grd_OL_Mapp.DataBind();
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch(e.Item.Name.ToUpper())
            {
                case "BACK":
                    Response.Redirect("RecipeList.aspx");
                    break;

                case "PRINT":
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        #region About: grd_OL_Mapp
        protected void grd_OL_Mapp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string locateCode = string.Empty;

                if (e.Row.FindControl("lbl_OL_Code") != null)
                {
                    Label lbl_OL_Code = (Label)e.Row.FindControl("lbl_OL_Code");
                    lbl_OL_Code.Text = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "OutletCode"));
                }

                if (e.Row.FindControl("comb_LocateCode") != null)
                {
                    locateCode = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "LocationCode"));
                    ComboBox comb_LocateCode = (ComboBox)e.Row.FindControl("comb_LocateCode");
                    comb_LocateCode.DataSource = stoLocate.GetList(1, LoginInfo.LoginName, LoginInfo.ConnStr);
                    comb_LocateCode.DataTextField = "LocationCode";
                    comb_LocateCode.DataValueField = "LocationCode";
                    comb_LocateCode.DataBind();
                    comb_LocateCode.SelectedValue = locateCode;
                }

                if (e.Row.FindControl("lbl_LocateCode") != null)
                {
                    Label lbl_LocateCode = (Label)e.Row.FindControl("lbl_LocateCode");
                    locateCode = string.Format("{0}", DataBinder.Eval(e.Row.DataItem, "LocationCode"));
                    lbl_LocateCode.Text = locateCode;
                }

                if (e.Row.FindControl("lbl_LocateDesc") != null)
                {
                    Label lbl_LocateDesc = (Label)e.Row.FindControl("lbl_LocateDesc");
                    lbl_LocateDesc.Text = stoLocate.GetName(locateCode, LoginInfo.ConnStr);
                }
            }
        }

        protected void grd_OL_Mapp_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_OL_Mapp.DataSource = dsRcpMapp.Tables[outletMappName];
            grd_OL_Mapp.EditIndex = e.NewEditIndex;
            grd_OL_Mapp.DataBind();
        }

        protected void grd_OL_Mapp_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dsRcpMapp = (DataSet)Session["dsRcpMapp"];
            ComboBox comb_LocateCode = (ComboBox)grd_OL_Mapp.Rows[grd_OL_Mapp.EditIndex].FindControl("comb_LocateCode");
            DataRow drEdit = dsRcpMapp.Tables[outletMappName].Rows[grd_OL_Mapp.EditIndex];
            drEdit["LocationCode"] = comb_LocateCode.SelectedValue;

            grd_OL_Mapp.DataSource = dsRcpMapp.Tables[outletMappName];
            grd_OL_Mapp.EditIndex = -1;
            grd_OL_Mapp.DataBind();

            Save_PosOutletLocation(drEdit);
            Page_Retrieve();
        }

        protected void grd_OL_Mapp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd_OL_Mapp.DataSource = dsRcpMapp.Tables[outletMappName];
            grd_OL_Mapp.EditIndex = -1;
            grd_OL_Mapp.DataBind();
        }

        protected void comb_LocateCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comb_LocateCode = (ComboBox)sender;
            Label lbl_LocateDesc = (Label)grd_OL_Mapp.Rows[grd_OL_Mapp.EditIndex].FindControl("lbl_LocateDesc");
            string locateName = stoLocate.GetName(comb_LocateCode.SelectedValue, LoginInfo.ConnStr);
            lbl_LocateDesc.Text = locateName;
        }
        #endregion

        #region Control Data
        protected DataTable Get_PosOutletLocation()
        {
            string cmdStr = @"SELECT * FROM [PT].[PosOutletLocation]";
            DataTable dtPosOutletLocation = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPosOutletLocation);
                dtPosOutletLocation.TableName = outletMappName;
            }
            finally
            { conn.Close(); }

            return dtPosOutletLocation;
        }

        protected bool Save_PosOutletLocation(DataRow dr)
        {
            string cmdStr = string.Format(@"UPDATE [PT].[PosOutletLocation]
                    SET [LocationCode] = @locate
                    WHERE [OutletCode] = @outlet ");
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.AddWithValue("@locate", dr["LocationCode"]);
                cmd.Parameters.AddWithValue("@outlet", dr["OutletCode"]);
                cmd.ExecuteNonQuery();
            }
            catch { return false; }
            return true;
        }
        #endregion
}
}