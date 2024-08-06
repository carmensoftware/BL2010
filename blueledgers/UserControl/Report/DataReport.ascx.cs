using System;
using System.Data;

// ReSharper disable once CheckNamespace
namespace BlueLedger.PL.UserControls.Report
{
    public partial class DataReport : BaseClass.BaseUserControl
    {
        private readonly Blue.BL.Report.Report _report = new Blue.BL.Report.Report();
        private DataSet _dsReport = new DataSet();

        public int ReportID { get; set; }

        /// <summary>
        ///     Get or Set ReportCategory of ChartReport UserControl.
        /// </summary>
        public Blue.BL.GnxLib.ReportCategory Category { get; set; }

        /// <summary>
        ///     Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _dsReport = (DataSet) Session["dsReport"];

                if (_dsReport == null)
                {
                    return;
                }

                if (_dsReport.Tables[_report.TableName] == null)
                {
                    return;
                }

                if (_dsReport.Tables[_report.TableName].Rows.Count > 0)
                {
                    // Report TypeID = 5 is type of XtraReport
                    if ((int) _dsReport.Tables[_report.TableName].Rows[0]["Type"] == 5)
                    {
                        Page_Retrieve();
                    }
                }
            }
            else
            {
                _dsReport = (DataSet) Session["dsReport"];
            }
        }

        /// <summary>
        ///     Retrieve data from database
        /// </summary>
        private void Page_Retrieve()
        {
            var action = Request.Params["action"];

            switch (action.ToUpper())
            {
                case "NEW":
                    break;
            }
        }

        /*
            private enum ReportDesignMode
            {
                New,
                Modify,
                Default
            };
        */
        
    }
}