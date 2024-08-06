using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.Mobile
{
    public partial class PrDt : BasePage
    {
        private Blue.BL.APP.ViewHandler viewHandler = new Blue.BL.APP.ViewHandler();

        private DataTable dtPRdt = new DataTable();
        private DataTable dtView = new DataTable();

        private string PRNo;
        private int roleID;

        protected override void Page_Load(object sender, EventArgs e)
        {
            /* http://localhost/blueledgers/Mobile/PrDt.aspx?ID=PR13110505 long list */
            /* http://localhost/blueledgers/Mobile/PrDt.aspx?ID=PR13010053 short list */
            if (!IsPostBack)
            {
                PRNo = Request.QueryString["ID"];
                roleID = 3;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Count", "countCBR()", true);
                Page_Retrieve();
            }
            else
            {

            }
        }

        private void Page_Retrieve()
        {
            Page_Setting();
        }

        protected void Page_Setting()
        {
            GetView();
            DataRow[] drV = dtView.Select("RoleID = '" + roleID + "'");

            GetDetail();
            rptDetail.DataSource = dtPRdt;
            rptDetail.DataBind();
            SetDetailStyle(drV);
            countSelectedItem();
            lbl_countDetail.Text = Convert.ToString(rptDetail.Items.Count);
        }

        protected void GetView()
        {
            dtView.Columns.Add("RoleID", typeof(int));
            dtView.Columns.Add("RoleState", typeof(string));
            dtView.Columns.Add("Block01", typeof(string));
            dtView.Columns.Add("Block02", typeof(string));
            dtView.Columns.Add("Block03", typeof(string));
            dtView.Columns.Add("Block04", typeof(string));
            dtView.Columns.Add("Block05", typeof(string));
            dtView.Columns.Add("Block06", typeof(string));
            dtView.Columns.Add("Block07", typeof(string));
            dtView.Columns.Add("Block08", typeof(string));

            dtView.Rows.Add(1, "ManagerLevel01", "ProductCode", "Descen", "ReqDate", null, null, null, null, null);
            dtView.Rows.Add(2, "ManagerLevel02", "ProductCode", "Descen", "ReqDate", "Price", null, null, null, null);
            dtView.Rows.Add(3, "ManagerLevel03", "ProductCode", "Descen", "ReqQty", "Price", "TotalAmt", null, null, null);
        }

        protected void GetDetail()
        {
            string strconnect = LoginInfo.ConnStr;
            SqlConnection con = new SqlConnection(strconnect);
            try
            { con.Open(); }
            catch
            {
                con.Close();
                lblMsg.Text = "Cannot connecting";
            }
            string strSql = "select * From [PC].PrDt Where PRNo = '" + PRNo + "'";
            SqlCommand cmd = new SqlCommand(strSql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtPRdt);
        }

        protected void SetDetailStyle(DataRow[] dr)
        {
            string block01 = dr[0]["Block01"].ToString();
            string block02 = dr[0]["Block02"].ToString();
            string block03 = dr[0]["Block03"].ToString();
            string block04 = dr[0]["Block04"].ToString();
            string block05 = dr[0]["Block05"].ToString();
            string block06 = dr[0]["Block06"].ToString();
            string block07 = dr[0]["Block07"].ToString();
            string block08 = dr[0]["Block08"].ToString();

            Label lbltxtB01, lblValueB01;
            Label lbltxtB02, lblValueB02;
            Label lbltxtB03, lblValueB03;
            Label lbltxtB04, lblValueB04;
            Label lbltxtB05, lblValueB05;
            Label lbltxtB06, lblValueB06;
            Label lbltxtB07, lblValueB07;
            Label lbltxtB08, lblValueB08;
            for (int i = 0; i < rptDetail.Items.Count; i++)
            {
                lbltxtB01 = (Label)rptDetail.Items[i].FindControl("lbltxtB01");
                lblValueB01 = (Label)rptDetail.Items[i].FindControl("lblValueB01");
                lbltxtB02 = (Label)rptDetail.Items[i].FindControl("lbltxtB02");
                lblValueB02 = (Label)rptDetail.Items[i].FindControl("lblValueB02");
                lbltxtB03 = (Label)rptDetail.Items[i].FindControl("lbltxtB03");
                lblValueB03 = (Label)rptDetail.Items[i].FindControl("lblValueB03");
                lbltxtB04 = (Label)rptDetail.Items[i].FindControl("lbltxtB04");
                lblValueB04 = (Label)rptDetail.Items[i].FindControl("lblValueB04");

                lbltxtB05 = (Label)rptDetail.Items[i].FindControl("lbltxtB05");
                lblValueB05 = (Label)rptDetail.Items[i].FindControl("lblValueB05");
                lbltxtB06 = (Label)rptDetail.Items[i].FindControl("lbltxtB06");
                lblValueB06 = (Label)rptDetail.Items[i].FindControl("lblValueB06");
                lbltxtB07 = (Label)rptDetail.Items[i].FindControl("lbltxtB07");
                lblValueB07 = (Label)rptDetail.Items[i].FindControl("lblValueB07");
                lbltxtB08 = (Label)rptDetail.Items[i].FindControl("lbltxtB08");
                lblValueB08 = (Label)rptDetail.Items[i].FindControl("lblValueB08");

                if (block01 == null || block01 == string.Empty)
                {
                    lbltxtB01.Visible = false;
                    lblValueB01.Visible = false;
                }
                if (block02 == null || block02 == string.Empty)
                {
                    lbltxtB02.Visible = false;
                    lblValueB02.Visible = false;
                }
                if (block03 == null || block03 == string.Empty)
                {
                    lbltxtB03.Visible = false;
                    lblValueB03.Visible = false;
                }
                if (block04 == null || block04 == string.Empty)
                {
                    lbltxtB04.Visible = false;
                    lblValueB04.Visible = false;
                }
                if (block05 == null || block05 == string.Empty)
                {
                    lbltxtB05.Visible = false;
                    lblValueB05.Visible = false;
                }
                if (block06 == null || block06 == string.Empty)
                {
                    lbltxtB06.Visible = false;
                    lblValueB06.Visible = false;
                }
                if (block07 == null || block07 == string.Empty)
                {
                    lbltxtB07.Visible = false;
                    lblValueB07.Visible = false;
                }
                if (block08 == null || block08 == string.Empty)
                {
                    lbltxtB08.Visible = false;
                    lblValueB08.Visible = false;
                }
            }
        }

        protected int countSelectedItem()
        {
            int count = 0;
            int countrptItem = rptDetail.Items.Count;
            foreach (RepeaterItem rptItem in rptDetail.Items)
            {
                HtmlInputCheckBox CB = (HtmlInputCheckBox)rptItem.FindControl("cbPRDt");
                if (CB.Checked)
                    count++;
            }
            lblSelected.Text = "( " + count + " / " + countrptItem + " )";
            return count;
        }

        protected void lbExpandHead_Click(object sender, EventArgs e)
        {
            if (lbExpandHead.Text == "more")
            {
                lbExpandHead.Text = string.Empty;
                lbLessHead.Text = string.Empty;
                pnExpandHeader.Style["display"] = "block";
            }
            if (lbLessHead.Text == "less")
            {
                lbExpandHead.Text = "more";
                pnExpandHeader.Style["display"] = "none";
            }

            lbLessHead.Text = "less";
        }

        protected void lbExpend_OnClick(object sender, EventArgs e)
        {
            LinkButton LB = (LinkButton)sender;
            RepeaterItem rptItem = (RepeaterItem)LB.NamingContainer;

            LinkButton lbExpand = (LinkButton)rptItem.FindControl("lbExpand");
            LinkButton lbLess = (LinkButton)rptItem.FindControl("lbLess");
            Panel pnExpand = (Panel)rptItem.FindControl("pnExpand");
            if (lbExpand.Text == "more")
            {
                lbExpand.Text = string.Empty;
                lbLess.Text = string.Empty;
                pnExpand.Style["display"] = "block";
            }
            if (lbLess.Text == "less")
            {
                lbExpand.Text = "more";
                pnExpand.Style["display"] = "none";
            }

            lbLess.Text = "less";
        }

        protected string GetFieldHeader(int blockNo)
        {
            // Get what cloumn that User want to see from ... 
            string fieldeHeader = string.Empty;
            DataRow[] drV = dtView.Select("RoleID = '" + roleID + "'");

            if (drV[0][blockNo + 1].ToString() != string.Empty ||
                drV[0][blockNo + 1] != DBNull.Value)
            {
                switch (blockNo)
                {
                    // In this case, ProdutCode & Desc is on same priority
                    case 1:
                        //fieldeHeader = drV[0]["Block01"].ToString();
                        return fieldeHeader;
                    case 2:
                        //fieldeHeader = drV[0]["Block02"].ToString();
                        return fieldeHeader;
                    case 3:
                        fieldeHeader = drV[0]["Block03"].ToString();
                        return fieldeHeader;
                    case 4:
                        fieldeHeader = drV[0]["Block04"].ToString();
                        return fieldeHeader;
                    case 5:
                        fieldeHeader = drV[0]["Block05"].ToString();
                        return fieldeHeader;
                    case 6:
                        fieldeHeader = drV[0]["Block06"].ToString();
                        return fieldeHeader;
                    case 7:
                        fieldeHeader = drV[0]["Block07"].ToString();
                        return fieldeHeader;
                    case 8:
                        fieldeHeader = drV[0]["Block08"].ToString();
                        return fieldeHeader;
                }
            }
            return fieldeHeader;
        }

        protected object GetFieldValue(int blockNo)
        {
            // Sent Binder back
            string columnName = string.Empty;
            DataRow[] drV = dtView.Select("RoleID = '" + roleID + "'");

            //Table: ID|State|Block01|Block02|...
            if (drV[0][blockNo + 1].ToString() != string.Empty ||
                drV[0][blockNo + 1] != DBNull.Value)
            {
                switch (blockNo)
                {
                    // Get what cloumn that User want to see from ... 
                    case 1:
                        //columnName = "ProductCode";
                        columnName = drV[0]["Block01"].ToString();
                        return Eval(columnName);
                    case 2:
                        columnName = drV[0]["Block02"].ToString();
                        return Eval(columnName);
                    case 3:
                        columnName = drV[0]["Block03"].ToString();
                        return Eval(columnName);
                    case 4:
                        columnName = drV[0]["Block04"].ToString();
                        return Eval(columnName);
                    case 5:
                        columnName = drV[0]["Block05"].ToString();
                        return Eval(columnName);
                    case 6:
                        columnName = drV[0]["Block06"].ToString();
                        return Eval(columnName);
                    case 7:
                        columnName = drV[0]["Block07"].ToString();
                        return Eval(columnName);
                    case 8:
                        columnName = drV[0]["Block08"].ToString();
                        return Eval(columnName);
                }
            }
            return false;
        }

        protected void visibleCheckBox(bool vs)
        {
            if (vs)
            {
                foreach (RepeaterItem rptItem in rptDetail.Items)
                {
                    //CheckBox CB = (CheckBox)rptItem.FindControl("cbPRDt");
                    HtmlInputCheckBox CB = (HtmlInputCheckBox)rptItem.FindControl("cbPRDt");
                    CB.Style["display"] = "inline";
                }
            }
            else
            {
                foreach (RepeaterItem rptItem in rptDetail.Items)
                {
                    HtmlInputCheckBox CB = (HtmlInputCheckBox)rptItem.FindControl("cbPRDt");
                    CB.Style["display"] = "none";
                }
            }
        }

        protected void ControlCheckList()
        {
            if (cbMenu.Checked)
            {
                foreach (RepeaterItem rptItem in rptDetail.Items)
                {
                    //CheckBox CB = (CheckBox)rptItem.FindControl("cbPRDt");
                    HtmlInputCheckBox CB = (HtmlInputCheckBox)rptItem.FindControl("cbPRDt");
                    CB.Checked = true;
                }
            }
            else
            {
                foreach (RepeaterItem rptItem in rptDetail.Items)
                {
                    HtmlInputCheckBox CB = (HtmlInputCheckBox)rptItem.FindControl("cbPRDt");
                    CB.Checked = false;
                }
            }
        }

        protected void divFloatMenu_Click(object sender, EventArgs e)
        {
            visibleCheckBox(true);
            divLessFullMenu.Style["display"] = "inline";
            divFullMenu.Style["height"] = "230px";
            divFloatMenu.Visible = false;
        }

        protected void divLessFullMenu_Click(object sender, EventArgs e)
        {
            visibleCheckBox(false);
            divLessFullMenu.Style["display"] = "none";
            divFullMenu.Style["height"] = "0px";
            divFloatMenu.Visible = true;
        }

        protected void lbtn_Select_Click(object sender, EventArgs e)
        {
            visibleCheckBox(true);
            divFullMenu.Style["height"] = "230px";
            divLessFullMenu.Style["display"] = "inline";

            if (cbMenu.Checked)
                cbMenu.Checked = false;
            else
                cbMenu.Checked = true;
            ControlCheckList();
            countSelectedItem();
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            visibleCheckBox(true);
            divFloatMenu.Visible = false;
            divLessFullMenu.Style["display"] = "none";
            divFullMenu.Style["height"] = "0%";

            //divEditMode.Visible = true;
            divEditMode.Style["height"] = "120px";
        }

        protected void lbtn_save_Click(object sender, EventArgs e)
        {
            divEditMode.Style["height"] = "0";
            visibleCheckBox(false);
            divFloatMenu.Visible = true;
        }

        protected void lbtn_cancel_Click(object sender, EventArgs e)
        {
            divEditMode.Style["height"] = "0";
            visibleCheckBox(false);
            divFloatMenu.Visible = true;
        }
    }
}