using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.IN.MLT
{
    public partial class MLTLst : BasePage
    {
        #region "Attributes"

        private string MsgError = string.Empty;
        private DataSet dsTemplate = new DataSet();
        private Blue.BL.Option.Inventory.StoreLct storeLct = new Blue.BL.Option.Inventory.StoreLct();
        private Blue.BL.PC.TP.Template template = new Blue.BL.PC.TP.Template();
        private Blue.BL.PC.TP.TemplateDt templateDt = new Blue.BL.PC.TP.TemplateDt();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!Page.IsPostBack)
            {
                ListPage.PrintItems.Items.Add(new DevExpress.Web.ASPxMenu.MenuItem("Market List", "MLD"));
                ListPage.DataBind();
            }
            ListPage.CreateItems.Menu.ItemClick += menu_ItemClick;
        }


        /// <summary>
        ///     Display Market List/Standard Order
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void menu_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "MLD":
                    //ArrayList objArrList = new ArrayList();
                    //objArrList.Add(ListPage.ListKeys.Replace("'",""));
                    //objArrList.Add( "'M'" );
                    //Session["s_arrNo"] = objArrList;

                    //string[,] paramFieldReport = new string[1, 3];
                    //paramFieldReport[0, 0] = "Type";
                    //paramFieldReport[0, 1]= "=" ;
                    //paramFieldReport[0, 2] = "M";
                    //Session["paramFieldReport"] = paramFieldReport;


                    var dtReport = new DataTable();
                    var cl = new DataColumn("BUCode");
                    dtReport.Columns.Add(cl);

                    cl = new DataColumn("No");
                    dtReport.Columns.Add(cl);

                    cl = new DataColumn("No1");
                    dtReport.Columns.Add(cl);

                    for (var jj = 0; jj < ListPage.dtBuKeys.Rows.Count; jj++)
                    {
                        var dr = dtReport.NewRow();
                        dr[0] = ListPage.dtBuKeys.Rows[jj][0];
                        dr[1] = ListPage.dtBuKeys.Rows[jj][1];
                        dr[2] = "M";
                        dtReport.Rows.Add(dr);
                    }

                    Session["dtBuKeys"] = dtReport;
                    var reportLink = "../../RPT/ReportCriteria.aspx?category=001&reportid=150";
                    //Response.Write("<script>");
                    //Response.Write("window.open('" + reportLink + "','_blank'  )");
                    //Response.Write("</script>");
                    ClientScript.RegisterStartupScript(GetType(), "newWindow",
                        "<script>window.open('" + reportLink + "','_blank')</script>");
                    break;
            }
        }


        //private String GetArrMLNo()
        //{

        //    StringBuilder sb = new StringBuilder();

        //    GridView grdd = (GridView)ListPage.FindControl("grd_BU");
        //    GridView grdTran = (GridView)grdd.Rows[0].FindControl("grd_Trans");


        //    for (int i = 0; i < grdTran.Rows.Count; i++)
        //    {
        //        string prId = grdTran.Rows[i].Cells[2].Text;

        //        sb.Append("'" + prId + "',");
        //    }
        //    if (sb.Length > 0)
        //    {
        //        return sb.ToString().Substring(0, sb.Length - 1);
        //    }
        //    else
        //    {
        //        return "'*'";
        //    }
        //}

        ///// <summary>
        ///// Get business MLTLst data related to login user.
        ///// </summary>
        //private void Page_Retrieve()
        //{
        //    // Clear all exist data.
        //    dsTemplate.Clear();

        //    // Get Store Data.
        //    bool getStore = storeLct.GetList(dsTemplate, LoginInfo.ConnStr);

        //    if (!getStore)
        //    {
        //        return;
        //    }

        //    // Get Template Data.
        //    if (dsTemplate.Tables[storeLct.TableName] != null)
        //    {
        //        if (dsTemplate.Tables[storeLct.TableName].Rows.Count > 0)
        //        {
        //            string tmpTypeCode  = Request.Params["TYPE"].ToString().ToUpper();
        //            string locationCode = dsTemplate.Tables[storeLct.TableName].Rows[0]["LocationCode"].ToString();

        //            bool getTmplate = template.GetList(dsTemplate, tmpTypeCode, locationCode, LoginInfo.ConnStr);

        //            if (getTmplate)
        //            {
        //                Session["dsTemplate"] = dsTemplate;
        //                this.Page_Setting();
        //            } 
        //        }
        //    }
        //}

        ///// <summary>
        ///// Display business MLTLst data which retrieved from Page_Retrieve procedure.
        ///// </summary>
        //private void Page_Setting()
        //{
        //    //lbl_Title.Text = (Request.Params["TYPE"].ToString().ToUpper() == "M" ? "Market List" : "Standard Order");

        //    //ddl_Store.DataSource = dsTemplate.Tables[storeLct.TableName];
        //    //ddl_Store.DataValueField = "LocationCode";
        //    //ddl_Store.DataTextField = "LocationName";
        //    //ddl_Store.DataBind();

        //    //grd_TmpList.DataSource = dsTemplate.Tables[template.TableName];
        //    //grd_TmpList.DataBind();
        //}

        ///// <summary>
        ///// Create new template.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lnkb_New_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("MLTEdit.aspx?action=NEW&type=" + Request.Params["TYPE"].ToString());
        //}

        ///// <summary>
        ///// Display market list/standard order data related selected store.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddl_Store_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grd_TmpList_Refresh();
        //}

        ///// <summary>
        ///// Refresh template data.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btn_OK_Click(object sender, EventArgs e)
        //{
        //    grd_TmpList_Refresh();
        //}

        ///// <summary>
        ///// Display template data.
        ///// </summary>
        //private void grd_TmpList_Refresh()
        //{
        //    //string tmpTypeCode  = Request.Params["TYPE"].ToString().ToUpper();
        //    //string locationCode = ddl_Store.SelectedItem.Value;

        //    //// Clear exist template data.
        //    //if (dsTemplate.Tables[template.TableName] != null)
        //    //{
        //    //    dsTemplate.Tables[template.TableName].Clear();
        //    //}

        //    //// Get template data.
        //    //bool getTmplate = template.GetList(dsTemplate, tmpTypeCode, locationCode, LoginInfo.ConnStr);

        //    //if (getTmplate)
        //    //{
        //    //    grd_TmpList.DataSource = dsTemplate.Tables[template.TableName];
        //    //    grd_TmpList.DataBind();

        //    //    Session["dsTemplate"] = dsTemplate;                
        //    //}   
        //}

        ///// <summary>
        ///// Delete template.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_TmpList_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    // Find deleting row.
        //    DataRow drDeleting = dsTemplate.Tables[template.TableName].Rows.Find(e.Keys[0]);

        //    // Get template detail data.
        //    bool getTemplateDt = templateDt.GetListByTmpNo(dsTemplate, ref MsgError, int.Parse(drDeleting["TmpNo"].ToString()), LoginInfo.ConnStr);

        //    if (getTemplateDt)
        //    {
        //        // Delete Template
        //        drDeleting.Delete();

        //        // Delete Template Detail
        //        foreach (DataRow drTemplateDetail in dsTemplate.Tables[templateDt.TableName].Rows)
        //        {
        //            drTemplateDetail.Delete();
        //        }

        //        bool delete = template.Delete(dsTemplate, LoginInfo.ConnStr);

        //        if (delete)
        //        {
        //            e.Cancel = true;
        //            grd_TmpList_Refresh();
        //        }
        //    }
        //}        

        ///// <summary>
        ///// Go to edit page.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void grd_TmpList_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        //{
        //    Response.Redirect("MLTEdit.aspx?action=EDIT&TYPE=" + Request.Params["TYPE"].ToString() + "&TmpNo=" + e.EditingKeyValue.ToString());
        //}

        #endregion
    }
}