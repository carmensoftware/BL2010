﻿using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC
{
    public partial class PeriodEnd : BasePage
    {
        private readonly Blue.BL.ADMIN.RolePermission rolePermiss = new Blue.BL.ADMIN.RolePermission();
        private readonly string moduleID = "3.8.2";
        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.PC.Priod period = new Blue.BL.PC.Priod();

        //private DataSet dsPeriod = new DataSet();
        private DateTime startPeriodDate;
        private DateTime endPeriodDate;

        protected override void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    Page_Retrieve();
            //}
            //else
            //{
            //    //dsPeriod = (DataSet)Session["dsPeriod"];
            //    startPeriodDate = (DateTime)ViewState["startPeriodDate"];
            //    endPeriodDate = (DateTime)ViewState["endPeriodDate"];
            //}

            base.Page_Load(sender, e);

            startPeriodDate = period.GetLatestOpenStartDate(LoginInfo.ConnStr);
            endPeriodDate = period.GetLatestOpenEndDate(LoginInfo.ConnStr);

            gvRec.DataSource = GetPendingReceiving();
            gvRec.DataBind();

            gvEop.DataSource = GetPendingEop();
            gvEop.DataBind();

            lbl_EndDate.Text = endPeriodDate.ToShortDateString();

            Control_HeaderMenuBar();
        }

        protected void Control_HeaderMenuBar()
        {
            int pagePermiss = rolePermiss.GetPagePermission(moduleID, LoginInfo.LoginName, LoginInfo.ConnStr);
            btn_ClosePeriod.Enabled = (pagePermiss >= 3) ? btn_ClosePeriod.Enabled : false;
        }


        protected void gvRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var gv = sender as GridView;
            gv.PageIndex = e.NewPageIndex;
            gv.DataSource = GetPendingReceiving();
            gv.DataBind();
        }

        protected void gvEop_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEop.PageIndex = e.NewPageIndex;
            gvEop.DataSource = GetPendingEop();
            gvEop.DataBind();
        }

        protected void btn_ClosePeriod_Click(object sender, EventArgs e)
        {
            
            if (GetPendingReceiving().Rows.Count > 0 || GetPendingEop().Rows.Count > 0)
            {

                lbl_Warning.Text = "Some Receiving or Closing Balance transations are not committed. ";
                pop_Warning.ShowOnPageLoad = true;
            }
            else
            {
                period.PeriodEnd(startPeriodDate, endPeriodDate, LoginInfo.LoginName, LoginInfo.ConnStr);
                Response.Redirect(Request.RawUrl);    
                
            }
            //if (dsPeriod.Tables[period.TableName].Rows.Count > 0)
            //{
            //    var drPeriod = dsPeriod.Tables[period.TableName].Rows[0];
            //    var result = period.CheckEndDate(DateTime.Parse(drPeriod["EndDate"].ToString()), LoginInfo.ConnStr);
            //    if (result)
            //    {
            //        period.PeriodEnd(DateTime.Parse(drPeriod["StartDate"].ToString()),
            //            DateTime.Parse(drPeriod["EndDate"].ToString()),
            //            LoginInfo.LoginName, LoginInfo.ConnStr);

            //        // Added on: 22/09/2017, By: Fon
            //        ClassLogTool pctool = new ClassLogTool();
            //        string returnDef = pctool.DefragmentActionLog(3);
            //        pctool.SaveActionLog("EOP", string.Empty, "Closed Period: " + lbl_EndDate.Text);
            //        // End Added.

            //        // Display Sucessful Message
            //        Page_Retrieve();
            //    }
            //    else
            //    {
            //        pop_Warning.ShowOnPageLoad = true;
            //    }
            //}
        }

        protected void btn_Warning_Click(object sender, EventArgs e)
        {
            pop_Warning.ShowOnPageLoad = false;
        }

        // Private Method(s)
        private DataTable GetPendingReceiving()
        {
            string sql =
                @";WITH rc AS(
	                SELECT
		                rec.RecNo,
		                SUM(recdt.CurrTotalAmt) as CurrTotalAmt,
		                SUM(recdt.TotalAmt) as TotalAmt
	                FROM
		                PC.REC rec
		                JOIN PC.RECDt recdt
			                ON recdt.RecNo = rec.RecNo
	                WHERE
		                rec.DocStatus = 'Received'
		                AND rec.RecDate BETWEEN '{0}' AND '{1}'
	                GROUP BY
		                rec.RecNo
                )
                SELECT
	                rc.RecNo,
	                CAST(rec.RecDate AS DATE) RecDate,
	                rec.VendorCode + ' - ' + v.Name AS Vendor,
	                InvoiceNo,
	                CAST(rec.InvoiceDate AS DATE) InvoiceDate,
	                rec.[Description],
	                rec.DocStatus,
	                CurrencyCode,
	                rc.CurrTotalAmt,
	                rc.TotalAmt
                FROM
	                rc
	                JOIN PC.Rec rec
		                ON rec.RecNo = rc.RecNo
                    JOIN AP.Vendor v
                        ON rec.VendorCode = v.VendorCode";
            //period.
            sql = string.Format(sql, startPeriodDate.ToString("yyyy-MM-dd"), endPeriodDate.ToString("yyyy-MM-dd"));

            return period.DbExecuteQuery(sql, null, LoginInfo.ConnStr);
        }

        private DataTable GetPendingEop()
        {
            string sql =
                @";WITH eop AS(
                    SELECT
	                    ISNULL(EopId, 0) EopId,
	                    l.LocationCode,
	                    l.LocationName,
	                    l.LocationCode + ' - ' +l.LocationName as Location,
	                    [Description],
	                    Remark,
	                    ISNULL([Status], 'Not Created') as DocStatus
                    FROM
	                    [IN].StoreLocation l
	                    LEFT JOIN [IN].Eop eop 
		                    ON eop.StoreId = l.LocationCode AND CAST(eop.EndDate as DATE) = @Date
                    WHERE
	                    l.EOP = 1
                    )
                    SELECT
	                    *
                    FROM
	                    eop
                    WHERE
	                    DocStatus NOT IN( 'Committed')";
            //period.
            sql = string.Format(sql, endPeriodDate.ToString("yyyy-MM-dd"));

            
            return period.DbExecuteQuery(@sql, null, LoginInfo.ConnStr);

        }


        protected string FormatNumber(decimal number)
        {
            var dp = config.GetConfigValue("APP", "Default", "DigitAmt", LoginInfo.ConnStr);
            if (string.IsNullOrEmpty(dp))
                dp = "2";

            return string.Format("{0:N" + dp + "}", number);
        }
    }
}