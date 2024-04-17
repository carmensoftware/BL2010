using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Option.Admin.Interface.Sun
{
    public partial class ExportRestore : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.ADMIN.Bu bu = new Blue.BL.ADMIN.Bu();
        //private readonly Blue.BL.ADMIN.AccountMapp MyaccountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly Blue.BL.ADMIN.AccountMapp accountMapp = new Blue.BL.ADMIN.AccountMapp();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();
        private readonly Blue.BL.PC.CN.Cn cn = new Blue.BL.PC.CN.Cn();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();

        private SqlConnection con;
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        private DataSet ds = new DataSet();
        private DataSet dsExport = new DataSet();
        private DataSet dsRestore = new DataSet();
        private DataTable dtInterfaceExp = new DataTable();
        private DataTable dtInterfaceExpFalse = new DataTable();
        private string rptquery = string.Empty;


        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                txt_FromDate.Date = ServerDateTime.Date;
                txt_ToDate.Date = ServerDateTime.Date;
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                switch (e.Item.Name.ToUpper())
                {
                    case "PRINT":
                        var objArrList = new ArrayList();
                        objArrList.Add(txt_FromDate.Value);
                        objArrList.Add(txt_ToDate.Value);
                        Session["s_arrNo"] = objArrList;

                        var reportLink = "../../../../RPT/ReportCriteria.aspx?category=001&reportid=170" + "&BuCode=" +
                                         LoginInfo.BuInfo.BuCode;
                        Response.Redirect("javascript:window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("<script>");
                        //Response.Write("window.open('" + reportLink + "','_blank'  )");
                        //Response.Write("</script>");
                        break;
                }
            }
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            GrdPreView();
        }

        private void GrdPreView()
        {
            //con = new SqlConnection(LoginInfo.ConnStr);
            //rptquery = "Select * from vPre_Restore where DocDate >= @FFDate and DocDate <= @TTDate";

            //cmd.CommandType = System.Data.CommandType.Text;
            //cmd.CommandText = rptquery;
            //cmd.Connection = con;
            ////cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Value.ToString();
            ////cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Value.ToString();
            //cmd.Parameters.Add("FFDate", System.Data.SqlDbType.DateTime).Value = txt_FromDate.Date.ToString("yyyy-MM-dd");
            //cmd.Parameters.Add("TTDate", System.Data.SqlDbType.DateTime).Value = txt_ToDate.Date.ToString("yyyy-MM-dd");
            //da.SelectCommand = cmd;
            //da.Fill(ds, "vPre_Restore");

            //grd_Preview2.DataSource = ds;
            string sql = string.Format("SELECT * FROM [ADMIN].vExportToAP WHERE DocDate BETWEEN '{0}' AND '{1}'", txt_FromDate.Date.ToString("yyyy-MM-dd"), txt_ToDate.Date.ToString("yyyy-MM-dd"));
            grd_Preview2.DataSource = bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
            grd_Preview2.DataBind();
        }

        protected void btn_Restore_Click(object sender, EventArgs e)
        {
            lbl_TitleConfirm.Text = string.Format("Confirm to restore export from {0} to {1}?", txt_FromDate.Text, txt_ToDate.Text);
            pop_Confrim.ShowOnPageLoad = true;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            pop_Confrim.ShowOnPageLoad = false;
        }

        protected void btn_Confrim_Click(object sender, EventArgs e)
        {
            //isExp("REST");
            RestoreExport(txt_FromDate.Date, txt_ToDate.Date);
            GrdPreView();

            pop_Confrim.ShowOnPageLoad = false;
        }

        private void RestoreExport(DateTime dateFrom, DateTime dateTo)
        {
            string sql = string.Format("UPDATE PC.Rec SET ExportStatus = 0 WHERE CAST(RecDate AS DATE) BETWEEN '{0}' AND '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);

            sql = string.Format("UPDATE PC.Cn SET ExportStatus = 0 WHERE CAST(CnDate AS DATE) BETWEEN '{0}' AND '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
            bu.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }

        private void isExp(string _exp)
        {
            var dsUpdate = new DataSet();

            //Get data for update
            cn.GetCnbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 1, LoginInfo.ConnStr);
            rec.GetRecbyDate(txt_FromDate.Date, txt_ToDate.Date, dsUpdate, 1, LoginInfo.ConnStr);

            if (dsUpdate != null)
            {
                if (dsUpdate.Tables[cn.TableName].Rows.Count > 0)
                {
                    for (var a = 0; a < dsUpdate.Tables[cn.TableName].Rows.Count; a++)
                    {
                        dsUpdate.Tables[cn.TableName].Rows[a]["ExportStatus"] = 0;
                    }
                }

                if (dsUpdate.Tables[rec.TableName].Rows.Count > 0)
                {
                    for (var b = 0; b < dsUpdate.Tables[rec.TableName].Rows.Count; b++)
                    {
                        dsUpdate.Tables[rec.TableName].Rows[b]["ExportStatus"] = 0;
                    }
                }

                //Commit to database.
                var cnResult = cn.UpdateExportStatus(dsUpdate, LoginInfo.ConnStr);
                var recResult = rec.UpdateExportStatus(dsUpdate, LoginInfo.ConnStr);

                if (cnResult & recResult)
                {

                    grd_Preview2.DataSource = null;
                    grd_Preview2.DataBind();
                }
            }
        }

    }
}