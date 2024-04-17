using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;


namespace BlueLedger.PL.IN.STOREREQ
{
    public partial class StoreReqLst : BasePage
    {
        #region "Attributes"

        private readonly DataSet dsStoreReq = new DataSet();
        private readonly Blue.BL.IN.StandardRequistion stdReq = new Blue.BL.IN.StandardRequistion();
        private readonly Blue.BL.IN.StandardRequisitionDetail stdReqDt = new Blue.BL.IN.StandardRequisitionDetail();
        /*
                private Blue.BL.IN.storeRequisition storeReq = new Blue.BL.IN.storeRequisition();
                private Blue.BL.IN.StoreRequisitionDetail storeReqDt = new Blue.BL.IN.StoreRequisitionDetail();
                private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();
        */

        private bool isChangedLocation = false;

        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.1";
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            //Label1.Visible = false;
            //DropDownList1.Visible = false;

            ListPage2.setViewMultipleBU(false);
            if (!Page.IsPostBack) // Initial Page
            {
                // Modified by Ake (2014-03-05)
                // --------------------------------------------------------------------------------------------------

                BindDropDownListData();

                // set page
                if (Request.Params["Page"] != null)
                {
                    int pageNo = 0;
                    if (Request.Params["Page"] != string.Empty)
                        pageNo = Convert.ToInt32(Request.Params["Page"]);

                    ListPage2.setPage(pageNo);
                }

                // set filter location
                // ----------------------------------------------------------------------------------------------------
                if (Request.Params["Filter"] != null)
                {
                    string filterParam = Request.Params["Filter"];
                    if (filterParam != string.Empty)
                    {
                        // set store item to dropdownlist
                        DropDownList1.Items.FindByValue(filterParam.Split('~')[1].ToString()).Selected = true;

                        // ----------------------------------------------------------------------------------------------------
                        string locationCode = DropDownList1.SelectedItem.Value.ToString().Split(':')[0].ToString();
                        string filterText = string.Empty;

                        if (locationCode != string.Empty)
                            filterText = "[IN].vStoreRequisition.LocationCode = '" + locationCode + "'";
                        ListPage2.setFilter(filterText);
                    }
                }

                // ----------------------------------------------------------------------------------------------------

                Page_Retrieve();

            }
            else  // Postback
            {
                if (isChangedLocation)
                {
                    ListPage2.setPage(0);
                    isChangedLocation = false;
                }
                else
                    if (Request.Params["Page"] != null)
                    {
                        int pageNo = 0;
                        if (Request.Params["Page"] != string.Empty)
                            pageNo = Convert.ToInt32(Request.Params["Page"]);

                        ListPage2.setPage(pageNo);
                    }

                // set filter location
                // ----------------------------------------------------------------------------------------------------
                string locationCode = DropDownList1.SelectedItem.Value.ToString().Split(':')[0].ToString();
                string filterText = string.Empty;

                if (locationCode != string.Empty)
                    filterText = "[IN].vStoreRequisition.LocationCode = '" + locationCode + "'";
                ListPage2.setFilter(filterText);

            }


            base.Page_Load(sender, e);

            ListPage2.CreateItems.Menu.ItemClick += Menu_ItemClick;
        }

        private void Page_Retrieve()
        {
            Page_Setting();
        }

        private void Page_Setting()
        {
            //ListPage2.PageCode = Request.Params["pagecode"].ToString();
            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create", "CNEW"));
            ListPage2.CreateItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Create from Standard Requisition", "SR"));
            ListPage2.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Store Requisition List", "SL"));

            ListPage2.DataBind();
            Control_HeaderMenuBar();
        }

        // Added on: 15/11/2017, By: Fon
        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            ListPage2.CreateItems.Visible = (pagePermiss >= 3) ? ListPage2.CreateItems.Visible : false;
        }
        // End Added.

        private void BindDropDownListData()
        {
            // Location List depend on User right
            string sqlSelect = "SELECT us.LocationCode, (us.LocationCode + ': ' + sl.LocationName) as item";
            sqlSelect = sqlSelect + " FROM [ADMIN].[UserStore] us";
            sqlSelect = sqlSelect + " LEFT JOIN [IN].StoreLocation sl ON us.LocationCode = sl.LocationCode";
            sqlSelect = sqlSelect + " WHERE EOP = 1";
            sqlSelect = sqlSelect + "   AND us.LoginName = @UserName";
            sqlSelect = sqlSelect + " ORDER BY us.LocationCode";

            using (SqlConnection sqlConn = new SqlConnection(LoginInfo.ConnStr))
            {
                sqlConn.Open();

                SqlCommand cmd = new SqlCommand(sqlSelect, sqlConn);

                cmd.Parameters.AddWithValue("@UserName", LoginInfo.LoginName);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DropDownList1.DataSource = ds;
                DropDownList1.DataTextField = "item";
                DropDownList1.DataValueField = "LocationCode";
                DropDownList1.DataBind();


                sqlConn.Close();
            }

            DropDownList1.Items.Insert(0, new ListItem("All", string.Empty));

        }

        private void Menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CNEW":

                    var httpCookie = Request.Cookies["[IN].[vStoreRequisition]"];
                    if (httpCookie != null)
                        Response.Redirect(string.Format("StoreReqEdit.aspx?MODE=New&VID={0}", httpCookie.Value));
                    break;

                case "SR":

                    DisplayTemplate();
                    break;
                case "SL":

                    for (var i = 0; i < ListPage2.dtBuKeys.Rows.Count; i++)
                    {
                        ListPage2.dtBuKeys.Rows[i][1] =
                            ListPage2.dtBuKeys.Rows[i][1].ToString().Replace("'", "").Replace("*", "0");
                    }
                    Session["dtBuKeys"] = ListPage2.dtBuKeys;

                    var reportLink = "../../RPT/ReportCriteria.aspx?category=012&reportid=321";
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }

        private void DisplayTemplate()
        {
            var Get = stdReq.Get(dsStoreReq, LoginInfo.LoginName, LoginInfo.ConnStr);

            if (Get)
            {
                grd_Template.DataSource = dsStoreReq.Tables[stdReq.TableName];
                grd_Template.DataBind();

                //Session["dsStoreReq"] = dsStoreReq;
                pop_Template.ShowOnPageLoad = true;
            }
        }

        protected void btn_TemplateOk_Click(object sender, EventArgs e)
        {
            var columnValues = new List<object>();

            var grd_Grid = grd_Template;

            foreach (GridViewRow grd_Row in grd_Grid.Rows)
            {
                var chk_Item = grd_Row.FindControl("chk_Item") as CheckBox;

                if (chk_Item.Checked)
                {
                    columnValues.Add(grd_Grid.DataKeys[grd_Row.RowIndex].Value);
                }
            }

            if (columnValues.Count > 0)
            {

                for (var i = 0; i < columnValues.Count; i++)
                {
                    var get = stdReq.Get(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);

                    if (!get)
                    {
                        return;
                    }

                    var GetDt = stdReqDt.Get(dsStoreReq, int.Parse(columnValues[i].ToString()), LoginInfo.ConnStr);

                    if (!GetDt)
                    {
                        return;
                    }
                }

                Session["dsStoreReqDt"] = dsStoreReq;

                Response.Redirect(string.Format("StoreReqFromStdReq.aspx?MODE=Create&VID={0}", Request.Cookies["[IN].[vStoreRequisition]"].Value));
            }
            else
            {
                pop_Template.ShowOnPageLoad = false;
            }
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Session["LocationCode"] = (sender as DropDownList).SelectedValue.ToString();
            isChangedLocation = true;
            //ListPage2.setPageIndex(0);

            Page_Load(sender, e);
        }
    }
}