using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BlueLedger.PL.Option
{
    public partial class EditCurrencyExchange : BasePage
    {
        #region "Attributes"
        private DataSet dsCurrency = new DataSet();

        #endregion

        #region "Operations"

        //protected void Page_Init(object sender, EventArgs e)
        //{

        //}

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsCurrency = (DataSet)Session["dsCurrency"];
            }
        }

        /// <summary>
        ///     Get Unit Data.
        /// </summary>
        private void Page_Retrieve()
        {
            object[] resultCF = Get_CurrencyExchange();
            if (Convert.ToBoolean(resultCF[0]))
            {
                if (dsCurrency.Tables["dtCurrencyList"] != null)
                    dsCurrency.Tables.Remove(dsCurrency.Tables["dtCurrencyList"]);

                DataTable dt = (DataTable)resultCF[1];
                dt.Columns.Add("EditRate", typeof(decimal));
                dt.TableName = "dtCurrencyList";
                dsCurrency.Tables.Add(dt);
            }

            Session["dsCurrency"] = dsCurrency;
            Page_Setting();
        }

        /// <summary>
        ///     Display Unit Data.
        /// </summary>
        private void Page_Setting()
        {
            gv_CurrEdit.DataSource = dsCurrency.Tables["dtCurrencyList"];
            gv_CurrEdit.DataBind();

            txt_HeaderDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Set_InputDate_ToGridView(txt_HeaderDate.Text);
        }

        protected void Get_NewestCurrencyExchange()
        {
            if (dsCurrency.Tables["dtCurrencyList"] != null)
                dsCurrency.Tables["dtCurrencyList"].Rows.Clear();

            foreach (GridViewRow gvr in gv_CurrEdit.Rows)
            {
                Label lbl_CurrCode = (Label)gvr.FindControl("lbl_CurrCode");
                Label lbl_CurrDesc = (Label)gvr.FindControl("lbl_CurrDesc");
                TextBox txt_Date = (TextBox)gvr.FindControl("txt_Date");
                TextBox txt_rate = (TextBox)gvr.FindControl("txt_rate");

                DataRow dr = dsCurrency.Tables["dtCurrencyList"].NewRow();
                dr["InputDate"] = Convert.ToDateTime(txt_Date.Text).ToString("dd/MM/yyyy");
                dr["CurrencyCode"] = lbl_CurrCode.Text.Trim();
                dr["Description"] = lbl_CurrDesc.Text.Trim();
                if (txt_rate.Text != string.Empty)
                    dr["CurrencyRate"] = Convert.ToDecimal(txt_rate.Text);

                dsCurrency.Tables["dtCurrencyList"].Rows.Add(dr);
            }
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    Control_SaveCurrency();
                    break;

                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;

                case "BACK":
                    Response.Redirect("CurrencyExchange.aspx");
                    break;
            }
        }

        protected List<string> Get_DataChangeList(DataTable dt, string valueField, string oriField, string editField)
        {
            // U Compare 2 fields in same DataTable;
            List<string> resultLst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[editField].ToString() != string.Empty)
                {
                    if (dr[oriField].ToString() != dr[editField].ToString())
                    {
                        resultLst.Add(dr[valueField].ToString());
                    }
                }

            }

            return resultLst;
        }

        protected void Control_SaveCurrency()
        {
            List<string> diffLst = Get_DataChangeList(dsCurrency.Tables["dtCurrencyList"], "CurrencyCode", "CurrencyRate", "EditRate");
            object[] resultDT = Save_gv_CurrEdit_ToDataTable(diffLst);
            if (Convert.ToBoolean(resultDT[0]))
            {

                object[] resultSave = Save_DT_ToDatabase((DataTable)resultDT[1]);
                if (Convert.ToBoolean(resultSave[0]))
                    lbl_Result.Text = "Successful.";
                else
                    lbl_Result.Text = Convert.ToString(resultSave[1]);

                pop_Warning.HeaderText = "Save";
                pop_Warning.ShowOnPageLoad = true;

            }
        }

        protected void btn_Go_Click(object sender, EventArgs e)
        {
            Button btn_Go = (Button)sender;
            if (Is_DataChange(dsCurrency.Tables["dtCurrencyList"]))
            {
                pop_Warning.HeaderText = "Warning";
                lbl_Result.Text = "The data is changed.";
                lbl_Result.Text += "<br/>Would you like to save changes?";
                poplbl_Hide.Text = "SetDate";
                btn_Result.Text = "YES";
                btn_No.Visible = true;

                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                Set_InputDate_ToGridView(txt_HeaderDate.Text);
            }
        }
        #endregion

        #region About data list

        protected bool Is_DataChange(DataTable dtCurrency)
        {
            foreach (DataRow dr in dtCurrency.Rows)
            {
                if (dr["CurrencyRate"].ToString() != dr["EditRate"].ToString()
                    && dr["EditRate"].ToString() != string.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        protected void Set_InputDate_ToGridView(string date)
        {
            foreach (GridViewRow gvr in gv_CurrEdit.Rows)
            {
                if (gvr.FindControl("txt_Date") != null)
                {
                    TextBox txt_Date = (TextBox)gvr.FindControl("txt_Date");
                    txt_Date.Text = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    txt_Date_Changed(txt_Date);
                }
            }

            Get_NewestCurrencyExchange();
        }

        protected void gv_CurrEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_CurrCode") != null)
                {
                    Label lbl_CurrCode = (Label)e.Row.FindControl("lbl_CurrCode");
                    lbl_CurrCode.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }
                if (e.Row.FindControl("lbl_CurrDesc") != null)
                {
                    Label lbl_CurrDesc = (Label)e.Row.FindControl("lbl_CurrDesc");
                    lbl_CurrDesc.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Description"));
                }

                if (e.Row.FindControl("txt_Date") != null)
                {
                    TextBox txt_Date = (TextBox)e.Row.FindControl("txt_Date");
                    DateTime inputDate;
                    string str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "InputDate"));
                    if (DateTime.TryParse(str, out inputDate))
                    {
                        DateTime inputdate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "InputDate"));
                        txt_Date.Text = inputdate.ToString("dd/MM/yyyy");
                    }
                }

                if (e.Row.FindControl("txt_rate") != null)
                {
                    TextBox txt_rate = (TextBox)e.Row.FindControl("txt_rate");
                    string oriRate = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));
                    string editRate = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EditRate"));

                    txt_rate.Text = (editRate != string.Empty) ? editRate : oriRate;
                }
            }
        }

        protected void txt_Date_OnTextChanged(object sender, EventArgs e)
        {
            txt_Date_Changed(sender);
        }

        protected void txt_rate_OnTextChanged(object sender, EventArgs e)
        {
            txt_rate_Changed(sender);
        }

        protected void txt_Date_Changed(object sender)
        {
            TextBox txt_Date = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txt_Date.NamingContainer;
            TextBox txt_rate = (TextBox)gvr.FindControl("txt_rate");
            Label lbl_CurrCode = (Label)gvr.FindControl("lbl_CurrCode");

            decimal rate = Find_CurrencyRate_ByCodeAndInputDate(lbl_CurrCode.Text, txt_Date.Text);
            txt_rate.Text = (rate != 0) ? Convert.ToString(rate) : string.Empty;
        }

        protected void txt_rate_Changed(object sender)
        {
            TextBox txt_rate = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txt_rate.NamingContainer;
            TextBox txt_Date = (TextBox)gvr.FindControl("txt_Date");
            Label lbl_CurrCode = (Label)gvr.FindControl("lbl_CurrCode");

            //string sqlStr = string.Format("CurrencyCode = '{0}'", lbl_CurrCode.Text.Trim());
            //sqlStr += string.Format(" AND InputDate = '{0}'", Convert.ToDateTime(txt_Date.Text));
            //gv_Test.DataSource = dsCurrency.Tables["dtCurrencyList"].Select("CurrencyCode = 'SGD'");
            /* Why I cannot filter by date or by select() ?*/
            foreach (DataRow dr in dsCurrency.Tables["dtCurrencyList"].Rows)
            {
                if (dr["CurrencyCode"].ToString() == lbl_CurrCode.Text)
                {
                    dr["EditRate"] = Convert.ToDecimal(txt_rate.Text).ToString("N6");
                    break;
                }
            }

            Session["dsCurrency"] = dsCurrency;
        }

        protected object[] Get_CurrencyExchange()
        {
            object[] result = new object[2];
            DataTable dtEach = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = ";WITH group_ce AS";
            sqlStr += " ( SELECT MAX([InputDate]) AS [InputDate], [CurrencyCode]";
            sqlStr += " FROM [Ref].[CurrencyExchange] GROUP BY [CurrencyCode] )";
            sqlStr += " SELECT ce.[InputDate], c.[CurrencyCode]";
            sqlStr += " , c.[Desc] AS [Description], ce.[CurrencyRate]";
            sqlStr += " , ce.UpdatedDate, ce.UpdatedBy";

            sqlStr += " FROM [Ref].[CurrencyExchange] AS ce";
            sqlStr += " INNER JOIN group_ce ON ce.InputDate = group_ce.InputDate";
            sqlStr += " AND ce.CurrencyCode = group_ce.CurrencyCode";
            sqlStr += " RIGHT JOIN [Ref].[Currency] AS c ON group_ce.CurrencyCode = c.CurrencyCode";
            sqlStr += " WHERE c.IsActived = 1";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtEach);
                conn.Close();

                result[0] = true;
                result[1] = dtEach;
            }
            catch (Exception ex)
            {
                conn.Close();
                result[0] = false;
                result[1] = ex.Message;
            }
            return result;
        }

        protected decimal Find_CurrencyRate_ByCodeAndInputDate(string code, string date)
        {
            string formatedDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format("SELECT *");
            sqlStr += string.Format(" FROM [Ref].[CurrencyExchange]");
            sqlStr += string.Format(" WHERE [InputDate] = '{0}'", formatedDate);
            sqlStr += string.Format(" AND [CurrencyCode] = '{0}'", code);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
            }
            catch
            {
                conn.Close();
            }

            if (dt.Rows.Count > 0 && dt.Rows.Count < 2)
                return Convert.ToDecimal(dt.Rows[0]["CurrencyRate"]);

            return 0;

        }
        #endregion

        #region About Save
        protected object[] Save_gv_CurrEdit_ToDataTable(List<string> saveLst)
        {
            object[] result = new object[2];
            DataTable dt = new DataTable();
            dt.Columns.Add("InputDate", typeof(string));
            dt.Columns.Add("CurrencyCode", typeof(string));
            dt.Columns.Add("CurrencyRate", typeof(string));
            dt.Columns.Add("UpdatedDate", typeof(DateTime));
            dt.Columns.Add("UpdatedBy", typeof(string));

            try
            {
                foreach (GridViewRow gvr in gv_CurrEdit.Rows)
                {
                    Label lbl_CurrCode = new Label();
                    TextBox txt_Date = new TextBox();
                    TextBox txt_rate = new TextBox();

                    if (gvr.FindControl("lbl_CurrCode") != null)
                        lbl_CurrCode = (Label)gvr.FindControl("lbl_CurrCode");
                    else
                        continue;

                    if (saveLst.Contains(lbl_CurrCode.Text.Trim()))
                    {
                        if (gvr.FindControl("txt_Date") != null)
                            txt_Date = (TextBox)gvr.FindControl("txt_Date");
                        if (txt_Date.Text == string.Empty) continue;

                        if (gvr.FindControl("txt_rate") != null)
                            txt_rate = (TextBox)gvr.FindControl("txt_rate");
                        if (txt_rate.Text == string.Empty) continue;

                        DataRow dr = dt.NewRow();
                        //dr["InputDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                        dr["InputDate"] = DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                        dr["CurrencyCode"] = lbl_CurrCode.Text.Trim();
                        dr["CurrencyRate"] = Convert.ToDecimal(txt_rate.Text).ToString("N6");
                        dr["UpdatedDate"] = DateTime.Now;
                        dr["UpdatedBy"] = LoginInfo.LoginName;
                        dt.Rows.Add(dr);
                    }
                }

                result[0] = true;
                result[1] = dt;
            }
            catch (Exception ex)
            {
                result[0] = false;
                result[1] = ex.Message;
            }

            return result;
        }

        protected object[] Save_DT_ToDatabase(DataTable dt)
        {
            object[] result = new object[2];
            foreach (DataRow dr in dt.Rows)
            {
                SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
                try
                {
                    conn.Open();
                    string sqlDel = string.Format("DELETE FROM [Ref].[CurrencyExchange]");
                    sqlDel += string.Format(" WHERE [InputDate] = '{0}'", dr["InputDate"]);
                    sqlDel += string.Format(" AND [CurrencyCode] = '{0}'", dr["CurrencyCode"]);
                    SqlCommand cmd = new SqlCommand(sqlDel, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    string sqlInsert = "INSERT INTO [Ref].[CurrencyExchange]";
                    sqlInsert += " ([InputDate], [CurrencyCode], [CurrencyRate], [UpdatedDate], [UpdatedBy])";
                    sqlInsert += " VALUES( @InputDate, @CurrencyCode, @CurrencyRate, @UpdatedDate, @UpdatedBy)";
                    cmd = new SqlCommand(sqlInsert, conn);
                    cmd.Parameters.AddWithValue("@InputDate", dr["InputDate"]);
                    cmd.Parameters.AddWithValue("@CurrencyCode", dr["CurrencyCode"]);
                    cmd.Parameters.AddWithValue("@CurrencyRate", dr["CurrencyRate"]);
                    cmd.Parameters.AddWithValue("@UpdatedDate", dr["UpdatedDate"]);
                    cmd.Parameters.AddWithValue("@UpdatedBy", dr["UpdatedBy"]);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    result[0] = false;
                    result[1] += ex.Message;
                    //break;
                    return result;
                }
            }
            result[0] = true;
            result[1] = string.Empty;
            return result;
        }
        #endregion

        #region Pop
        protected void btn_Result_Click(object sender, EventArgs e)
        {
            btn_Result.Text = "OK";
            btn_No.Visible = false;
            pop_Warning.ShowOnPageLoad = false;
            if (lbl_Result.Text.Contains("changed"))
            {
                Control_SaveCurrency();
            }
            else
            {
                if (lbl_Result.Text.Contains("Success")
                    && poplbl_Hide.Text == string.Empty)
                {
                    Response.Redirect("CurrencyExchange.aspx");
                }
                else if (lbl_Result.Text.Contains("Success")
                    && poplbl_Hide.Text.Contains("SetDate"))
                {
                    Set_InputDate_ToGridView(txt_HeaderDate.Text);
                }
                else
                {
                    Page_Retrieve();
                }
            }

        }

        protected void btn_No_Click(object sender, EventArgs e)
        {
            btn_Result.Text = "OK";
            btn_No.Visible = false;
            pop_Warning.ShowOnPageLoad = false;
            if (lbl_Result.Text.Contains("change"))
            {
                Set_InputDate_ToGridView(txt_HeaderDate.Text);
            }
        }
        #endregion



    }
}