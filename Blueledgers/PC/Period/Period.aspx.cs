using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using DevExpress.Web.ASPxEditors;

namespace BlueLedger.PL.PC
{
    public partial class Period : BasePage
    {
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();
        private DataSet dsPeriod = new DataSet();

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPeriod = (DataSet) Session["dsPeriod"];
            }
        }

        private void Page_Retrieve()
        {
            dsPeriod.Clear();

            ddl_Year.DataSource = GetYearList();
            ddl_Year.DataBind();
            ddl_Year.Value = (ddl_Year.Value != null ? int.Parse(ddl_Year.Value.ToString()) : ServerDateTime.Year);

            var getPeriod = period.GetList(dsPeriod, int.Parse(ddl_Year.Value.ToString()), LoginInfo.ConnStr);

            if (getPeriod)
            {
                // If there is no period data, insert new rows for default
                if (dsPeriod.Tables[period.TableName].Rows.Count == 0)
                {
                    for (var i = 1; i <= 12; i++)
                    {
                        var drPeriod = dsPeriod.Tables[period.TableName].NewRow();

                        drPeriod["Year"] = int.Parse(ddl_Year.Value.ToString());
                        drPeriod["PeriodNumber"] = i;
                        drPeriod["StartDate"] = new DateTime(int.Parse(ddl_Year.Value.ToString()), i, 1);
                        drPeriod["EndDate"] = DateTime.Parse(drPeriod["StartDate"].ToString()).AddMonths(1).AddDays(-1);
                        drPeriod["IsClose"] = false;
                        drPeriod["CreatedDate"] = ServerDateTime;
                        drPeriod["CreatedBy"] = LoginInfo.LoginName;
                        drPeriod["UpdatedDate"] = ServerDateTime;
                        drPeriod["UpdatedBy"] = LoginInfo.LoginName;
                        dsPeriod.Tables[period.TableName].Rows.Add(drPeriod);
                    }
                }
            }
            else
            {
                // Display Error Message
                return;
            }

            Session["dsPeriod"] = dsPeriod;

            Page_Setting();
        }

        private void Page_Setting()
        {
            grd_Period.DataSource = dsPeriod.Tables[period.TableName];
            grd_Period.DataBind();
        }

        protected void grd_Period_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("txt_StartDate") != null)
                {
                    var txt_StartDate = (ASPxDateEdit) e.Row.FindControl("txt_StartDate");
                    txt_StartDate.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "StartDate").ToString()).Date;
                }

                if (e.Row.FindControl("txt_EndDate") != null)
                {
                    var txt_EndDate = (ASPxDateEdit) e.Row.FindControl("txt_EndDate");
                    txt_EndDate.Date = DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "EndDate").ToString()).Date;
                }

                if (e.Row.FindControl("lbl_Status") != null)
                {
                    var lbl_Status = (Label) e.Row.FindControl("lbl_Status");
                    lbl_Status.Text = (bool.Parse(DataBinder.Eval(e.Row.DataItem, "IsClose").ToString())
                        ? "Closed"
                        : "Open");
                }
            }
        }

        protected void ddl_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            Page_Retrieve();
        }

        private DataTable GetYearList()
        {
            var dtYear = new DataTable();
            dtYear.Columns.Add("Year");

            for (var i = ServerDateTime.Year - 5; i <= ServerDateTime.Year + 5; i++)
            {
                var drYear = dtYear.NewRow();
                drYear["Year"] = i;
                dtYear.Rows.Add(drYear);
            }

            return dtYear;
        }

        protected void bnt_Apply_Click(object sender, EventArgs e)
        {
            for (var i = 0; i <= 11; i++)
            {
                var gvrPeriod = grd_Period.Rows[i];
                var txt_StartDate = (ASPxDateEdit) grd_Period.Rows[i].FindControl("txt_StartDate");
                var txt_EndDate = (ASPxDateEdit) grd_Period.Rows[i].FindControl("txt_EndDate");

                var drPeriod = dsPeriod.Tables[period.TableName].Rows[i];

                if (!bool.Parse(dsPeriod.Tables[period.TableName].Rows[i]["IsClose"].ToString()))
                {
                    drPeriod["StartDate"] = txt_StartDate.Date.Date;
                    drPeriod["EndDate"] = txt_EndDate.Date.Date;
                    drPeriod["UpdatedDate"] = ServerDateTime;
                    drPeriod["UpdatedBy"] = LoginInfo.LoginName;
                }
            }

            var savedPeriod = period.Save(dsPeriod, LoginInfo.ConnStr);

            if (savedPeriod)
            {
                pop_CreatedSuccess.ShowOnPageLoad = true;
            }
        }

        protected void btn_OKNotAllow_Click(object sender, EventArgs e)
        {
            pop_CreatedSuccess.ShowOnPageLoad = false;
        }
    }
}