using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.Data.SqlClient;

namespace BlueLedger.PL.Option
{
    public partial class CurrencyExchange : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.Ref.Currency currency = new Blue.BL.Ref.Currency();
        private DataSet dsCurrency = new DataSet();

        private string CurrencyMode
        {
            get { return ViewState["CurrencyMode"].ToString(); }
            set { ViewState["CurrencyMode"] = value; }
        }

        #endregion

        #region "Operations"

        protected void Page_Init(object sender, EventArgs e)
        {
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
            dsCurrency.Clear();

            var getCurrency = currency.GetList(dsCurrency, LoginInfo.ConnStr);
            if (getCurrency)
            {
                // Assign Primarykey                
                dsCurrency.Tables[currency.TableName].PrimaryKey = GetPK();

                Session["dsCurrency"] = dsCurrency;
            }
            else
            {
                // Display Error Message
                return;
            }

            Page_Setting();
        }

        /// <summary>
        ///     Display Unit Data.
        /// </summary>
        private void Page_Setting()
        {
            gv_CurrView.DataSource = currency.GetActiveList(LoginInfo.ConnStr);
            gv_CurrView.DataBind();
        }

        #endregion

        #region "View"

        protected void btn_ViewGo_Click(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        #endregion

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsCurrency.Tables[currency.TableName].Columns["CurrencyCode"];

            return primaryKeys;
        }

        #endregion

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "EDIT":
                    Response.Redirect("EditCurrencyExchange.aspx");
                    break;

                case "PRINT":
                    //Print();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "print", "window.print();", true);
                    break;
            }
        }

        #region About data view

        protected void gv_CurrView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl_CurrCode = new Label();
                if (e.Row.FindControl("lbl_CurrCode") != null)
                {
                    lbl_CurrCode = (Label)e.Row.FindControl("lbl_CurrCode");
                    lbl_CurrCode.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyCode"));
                }

                if (e.Row.FindControl("lbl_CurrDesc") != null)
                {
                    Label lbl_CurrDesc = (Label)e.Row.FindControl("lbl_CurrDesc");
                    lbl_CurrDesc.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Desc"));
                }

                if (e.Row.FindControl("gv_Bottom_5") != null)
                {
                    GridView gv_Bottom_5 = (GridView)e.Row.FindControl("gv_Bottom_5");
                    object[] resultE = Get_EachCurrentHistory(lbl_CurrCode.Text.Trim(), 5);

                    if (Convert.ToBoolean(resultE[0]))
                    {
                        gv_Bottom_5.DataSource = (DataTable)resultE[1];
                        gv_Bottom_5.DataBind();
                    }
                }

                if (e.Row.FindControl("lnk_MoreD") != null)
                {
                    LinkButton lnk_MoreD = (LinkButton)e.Row.FindControl("lnk_MoreD");
                    lnk_MoreD.Visible = IsEachCurrencyMoreThanNumber(lbl_CurrCode.Text, 5);
                }
            }
        }

        protected void gv_Bottom_5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lbl_Time") != null)
                {
                    Label lbl_Time = (Label)e.Row.FindControl("lbl_Time");
                    DateTime date = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "InputDate"));
                    lbl_Time.Text = date.ToString("dd/MM/yyyy");
                }

                if (e.Row.FindControl("lbl_Rate") != null)
                {
                    Label lbl_Rate = (Label)e.Row.FindControl("lbl_Rate");
                    lbl_Rate.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CurrencyRate"));
                }

                if (e.Row.FindControl("lbl_By") != null)
                {
                    Label lbl_By = (Label)e.Row.FindControl("lbl_By");
                    lbl_By.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UpdatedBy"));
                }
            }
        }

        protected object[] Get_EachCurrentHistory(string currencyCode, int take)
        {
            object[] result = new object[2];
            DataTable dtEach = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);

            string sqlStr = string.Format("SELECT");
            if (take > 0)
                sqlStr += string.Format(" TOP({0})", take);

            sqlStr += string.Format(" * FROM [Ref].[CurrencyExchange]");
            sqlStr += string.Format(" WHERE [CurrencyCode] = '{0}'", currencyCode);
            sqlStr += string.Format(" ORDER BY [InputDate] DESC");

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

        protected bool IsEachCurrencyMoreThanNumber(string currency, int number)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(LoginInfo.ConnStr);
            string sqlStr = string.Format("SELECT * FROM [Ref].[CurrencyExchange]");
            sqlStr += string.Format(" WHERE [CurrencyCode] = '{0}'", currency);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(sqlStr, conn);
                da.Fill(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);

                return false;
            }

            return (dt.Rows.Count > number) ? true : false;

        }

        protected void lnk_MoreD_OnClick(object sender, EventArgs e)
        {
            LinkButton lnk_MoreD = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnk_MoreD.NamingContainer;
            Label lbl_CurrCode = (Label)gvr.FindControl("lbl_CurrCode");
            Label lbl_CurrDesc = (Label)gvr.FindControl("lbl_CurrDesc");

            pop_lbl_Code.Text = string.Format("{0}", lbl_CurrCode.Text);
            pop_lbl_Desc.Text = string.Format("{0}", lbl_CurrDesc.Text);
            object[] resultEachD = Get_EachCurrentHistory(lbl_CurrCode.Text, -1);
            if (Convert.ToBoolean(resultEachD[0]))
            {
                pop_gv_EachDetails.DataSource = (DataTable)resultEachD[1];
                pop_gv_EachDetails.DataBind();

                pop_Details.ShowOnPageLoad = true;
            }
        }
        #endregion
    }
}