using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using System.Data;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.RPT
{
    public partial class TrialReport : BasePage
    {
        #region Variable
        private DataSet dsRptID = new DataSet();
        private int rptId = 0;
        private string connStr = "SERVER=192.168.10.5;DATABASE=TEST;USER id=sa;PASSWORD=rattatue";
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            rptId = 3;
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsRptID = (DataSet)Session["dsRptID"];
            }
        }

        private void Page_Retrieve()
        {
            Get_ReportCriteria2(rptId);
            Page_Setting();
        }

        private void Page_Setting()
        {
            //gv_Test.DataSource = dsRptID.Tables[0];
            //gv_Test.DataBind();
        }

        protected void btn_OK_Click(object sender, EventArgs e)
        {
            //string txt = Convert.ToString(cbb_v.SelectedItem.GetValue("ProductCode"));
            string txt = Convert.ToString(cbb_v.Value);
            lbl_Test.Text = txt;

            //gv_Test.DataSource = dsRptID.Tables[0];
            //gv_Test.DataBind();
        }

        #region Prepare Data
        protected DataTable Get_ReportCriteria2(int id)
        {
            //string cmdStr = string.Format("SELECT * FROM [rpt].[vReportCriteria]");
            //cmdStr += string.Format(" WHERE [RptID] = {0}", id);

            string cmdStr = string.Format("USE [IMPACT_BL]");
            cmdStr += string.Format(" SELECT ProductCode, (ProductCode + ' : ' + ProductDesc1)as Item");
            cmdStr += " ,ProductDesc2 FROM [IN].Product ORDER BY ProductCode";

            SqlConnection con = new SqlConnection(connStr);

            con.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("dt_vReportCriteria");
            da.Fill(dt);
            con.Close();

            dsRptID.Tables.Add(dt);
            Session["dsRptID"] = dsRptID;

            return dt;
        }
        #endregion

        protected void cbb_v_ItemsRequestedByFilterCondition(object source, DevExpress.Web.ASPxEditors.ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSource01.SelectCommand = @"SELECT ProductCode, (ProductCode + ' : ' + ProductDesc1)as Item
                ,ProductDesc2 FROM [IN].Product WHERE 
                ORDER BY ProductCode";
        }
        protected void cbb_v_Init(object sender, EventArgs e)
        {
            //cbb_v.DataSource = dsRptID.Tables["dt_vReportCriteria"];
            string cmdStr = @"USE [IMPACT_BL] SELECT ProductCode
                , (ProductCode + ' : ' + ProductDesc1)as Item , ProductDesc2 
                FROM [IN].Product ORDER BY ProductCode";

            ASPxComboBox cbb_v = (ASPxComboBox)sender;
            cbb_v.DataSource = Get_DataTable_SqlCommand(cmdStr);
            cbb_v.TextField = "Item";
            cbb_v.ValueField = "ProductCode";
            cbb_v.DataBind();
        }

        protected object Get_DataTable_SqlCommand(string cmdStr)
        {
            if (cmdStr != string.Empty)
            {
                SqlConnection con = new SqlConnection(connStr);
                //SqlConnection con = new SqlConnection(connStr_IMPACT_S);

                con.Open();
                SqlCommand cmd = new SqlCommand(cmdStr, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                return dt;
            }
            return null;
        }
        //protected void cbb_v_Load(object sender, EventArgs e)
        //{

        //}
    }

}