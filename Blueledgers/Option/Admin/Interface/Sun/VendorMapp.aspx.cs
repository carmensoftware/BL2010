using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class VendorMapp : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsVendorMapp = new DataSet();
        private readonly Blue.BL.ADMIN.VendorMapp vendorMapp = new Blue.BL.ADMIN.VendorMapp();
        private DataSet dsVendorMappDisp = new DataSet();

        #endregion

        #region "Operations"

        #region "Page Load"

        protected void Page_Init(object sender, EventArgs e)
        {
            hf_BuCode.Value = LoginInfo.BuInfo.BuCode;
            hf_ConnStr.Value = LoginInfo.ConnStr;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
        }

        private void Page_Retrieve()
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
            grd_VendorMapp.DataSource = vendorMapp.GetList(hf_ConnStr.Value);
            grd_VendorMapp.DataBind();
        }

        #endregion

        #region "Commandbar"

        /// <summary>
        ///     Commandbar
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":

                    var result = Save();

                    if (result)
                    {
                        // Display Successfully Message.
                        pop_Saved.ShowOnPageLoad = true;
                    }
                    else
                    {
                        // Display Error Message.
                        pop_SaveFailed.ShowOnPageLoad = true;
                    }

                    break;
                case "PRINT":
                    //var objArrList = new ArrayList();

                    //var sb = new StringBuilder();

                    //for (var i = 0; i < dsVendorMapp.Tables[vendorMapp.TableName].Rows.Count; i++)
                    //{
                    //    var ListID = dsVendorMapp.Tables[vendorMapp.TableName].Rows[i]["HQCode"].ToString();
                    //    sb.Append("'" + ListID + "',");
                    //}
                    //if (sb.Length > 0)
                    //{
                    //    objArrList.Add(sb.ToString().Substring(0, sb.Length - 1));
                    //}
                    //else
                    //{
                    //    objArrList.Add('*');
                    //}

                    //Session["s_arrNo"] = objArrList;
                    //var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=231" + "&BuCode=" +
                    //                 LoginInfo.BuInfo.BuCode;
                    //ClientScript.RegisterStartupScript(GetType(), "newWindow",
                    //    "<script>window.open('" + reportLink + "','_blank')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        /// <summary>
        ///     Save change to database.
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            // Get all VendorMapping data for delete  
            var getVendorMapp = vendorMapp.GetList(dsVendorMapp, LoginInfo.ConnStr);

            if (getVendorMapp)
            {
                foreach (DataRow drVendorMapp in dsVendorMapp.Tables[vendorMapp.TableName].Rows)
                {
                    drVendorMapp.Delete();
                }
            }
            else
            {
                return false;
            }

            // Insert all current VendorMapping data
            for (var i = 0; i < grd_VendorMapp.Rows.Count; i++)
            {
                if (grd_VendorMapp.Rows[i].FindControl("txt_LocalCode") != null)
                {
                    var txt_LocalCode = grd_VendorMapp.Rows[i].FindControl("txt_LocalCode") as TextBox;

                    if (txt_LocalCode.Text != string.Empty)
                    {
                        var drNew = dsVendorMapp.Tables[vendorMapp.TableName].NewRow();
                        drNew["ID"] = i + 1;
                        drNew["BuCode"] = grd_VendorMapp.Rows[i].Cells[0].Text; //Get value from table at column BuCode
                        drNew["LocalCode"] = txt_LocalCode.Text.Trim();
                        drNew["HQCode"] = grd_VendorMapp.Rows[i].Cells[2].Text; //Get value from table at column HQCode
                        dsVendorMapp.Tables[vendorMapp.TableName].Rows.Add(drNew);
                    }
                }
            }

            // Save Change to Database.
            return vendorMapp.Save(dsVendorMapp, LoginInfo.ConnStr);
        }

        #endregion

        protected void grd_VendorMapp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("txt_LocalCode") != null)
                {
                    var txt_LocalCode = e.Row.FindControl("txt_LocalCode") as TextBox;
                    txt_LocalCode.Text = DataBinder.Eval(e.Row.DataItem, "LocalCode").ToString();
                }
            }
        }

        #endregion
    }
}