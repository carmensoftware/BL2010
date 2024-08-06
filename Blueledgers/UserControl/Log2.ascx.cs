using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls
{
    public partial class Log2 : BaseUserControl
    {
        private readonly Blue.BL.ADMIN.TransLog transLog = new Blue.BL.ADMIN.TransLog();
        private DataSet _dataSource = new DataSet();
        private string _module = string.Empty;
        private string _refNo = string.Empty;
        private string _submodule = string.Empty;
        private string _tableName = string.Empty;
        private Blue.DAL.DbHandler dbHandler = new Blue.DAL.DbHandler();
        private Blue.BL.GL.JournalVoucherActiveLog journalVoucherActiveLog = new Blue.BL.GL.JournalVoucherActiveLog();
        private Blue.BL.Security.User user = new Blue.BL.Security.User();

        /// <summary>
        ///     Gets or sets module which contain log data.
        /// </summary>
        public string Module
        {
            get
            {
                if (ViewState["Module"] == null)
                {
                    return _module;
                }

                return ViewState["Module"].ToString();
            }
            set
            {
                _module = value;
                ViewState["Module"] = _module;
            }
        }

        /// <summary>
        /// </summary>
        public string SubModule
        {
            get
            {
                if (ViewState["Submodule"] == null)
                {
                    return _submodule;
                }

                return ViewState["Submodule"].ToString();
            }
            set
            {
                _submodule = value;
                ViewState["Submodule"] = _submodule;
            }
        }

        /// <summary>
        ///     Gets or sets data source of log data.
        /// </summary>
        private DataSet DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    return _dataSource;
                }

                return (DataSet)ViewState["DataSource"];
            }
            set
            {
                _dataSource = value;
                ViewState["DataSource"] = _dataSource;
            }
        }

        /// <summary>
        ///     Gets or sets table name which contain log data.
        /// </summary>
        public string TableName
        {
            get
            {
                if (ViewState["TableName"] == null)
                {
                    return _tableName;
                }

                return ViewState["TableName"].ToString();
            }
            set
            {
                _tableName = value;
                ViewState["TableName"] = _tableName;
            }
        }

        /// <summary>
        ///     Gets or sets reference number related to log data.
        /// </summary>
        public string RefNo
        {
            get
            {
                if (ViewState["RefNo"] == null)
                {
                    return _refNo;
                }

                return ViewState["RefNo"].ToString();
            }
            set
            {
                _refNo = value;
                ViewState["RefNo"] = _refNo;
            }
        }

        /// <summary>
        ///     Retrieve and display log data.
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            // Get log data
            var result = Log_Retrieve();

            if (result)
            {
                // Display log data.
                grd_Log.DataSource = DataSource;
                grd_Log.DataBind();

                grd2.DataSource = DataSource;
                grd2.DataBind();
            }
        }

        /// <summary>
        ///     Retrieve log data related to specified table name and refno
        /// </summary>
        /// <returns></returns>
        private bool Log_Retrieve()
        {
            var dsLog = new DataSet();
            var dtLog = new DataTable();
            var command = string.Empty;

            //if (this.TableName != string.Empty && this.RefNo != string.Empty)
            //{
            //    //command = "SELECT * FROM " + @TableName + " WHERE RefNo = '" + @RefNo + "' ORDER BY CreatedDate DESC";
            //    //dtLog   = dbHandler.DbExecuteQuery(command, null, LoginInfo.ConnStr);

            //    dtLog     = journalVoucherActiveLog.GetJournalVoucherActLogByRefNo(this.RefNo.ToString(),LoginInfo.ConnStr);

            //    if (dtLog != null)
            //    {
            //        dtLog.TableName = this.TableName;
            //        dsLog.Tables.Add(dtLog);
            //        this.DataSource = dsLog;
            //    }
            //}
            if (Module != string.Empty && SubModule != string.Empty && RefNo != string.Empty)
            {
                //command = "SELECT * FROM " + @TableName + " WHERE RefNo = '" + @RefNo + "' ORDER BY CreatedDate DESC";
                //dtLog   = dbHandler.DbExecuteQuery(command, null, LoginInfo.ConnStr);

                dtLog = transLog.GetTransLogByModule_SubModule_RefNo(Module, SubModule, RefNo, LoginInfo.ConnStr);

                if (dtLog != null)
                {
                    dtLog.TableName = TableName;
                    dsLog.Tables.Add(dtLog);
                    DataSource = dsLog;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Display log data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Log_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Log
                if (e.Row.FindControl("tv_Log") != null)
                {
                    var tv_Log = (TreeView)e.Row.FindControl("tv_Log");
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(DataBinder.Eval(e.Row.DataItem, "Log").ToString());

                    for (var i = 0; i < xmlDoc.ChildNodes.Count; i++)
                    {
                        // Add root node
                        var tn_root = new TreeNode();
                        tn_root.Text = xmlDoc.ChildNodes[i].Name;

                        tn_root.SelectAction = TreeNodeSelectAction.Expand;
                        tv_Log.Nodes.Add(tn_root);

                        if (xmlDoc.ChildNodes[i].HasChildNodes)
                        {
                            for (var j = 0; j < xmlDoc.ChildNodes[i].ChildNodes.Count; j++)
                            {
                                // Add section changed node
                                var tn_section = new TreeNode();
                                tn_section.Text = xmlDoc.ChildNodes[i].ChildNodes[j].Attributes[i].Value;
                                tn_section.SelectAction = TreeNodeSelectAction.Expand;
                                tv_Log.Nodes[i].ChildNodes.Add(tn_section);

                                if (xmlDoc.ChildNodes[i].ChildNodes[j].HasChildNodes)
                                {
                                    for (var k = 0; k < xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes.Count; k++)
                                    {
                                        // Add field changed node
                                        var tn_field = new TreeNode();
                                        tn_field.Text = xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name;
                                        tn_field.SelectAction = TreeNodeSelectAction.Expand;
                                        tv_Log.Nodes[i].ChildNodes[j].ChildNodes.Add(tn_field);

                                        // change value will be display in this step.
                                        if (xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].HasChildNodes)
                                        {
                                            for (var kk = 0;
                                                kk < xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                                kk++)
                                            {
                                                // Add fieldvalue added node
                                                var tn_fieldValue = new TreeNode();
                                                tn_fieldValue.Text =
                                                    xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[kk]
                                                        .InnerText;
                                                tn_fieldValue.SelectAction = TreeNodeSelectAction.None;
                                                tv_Log.Nodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Add(tn_fieldValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Date
                if (e.Row.FindControl("lbl_DateLog") != null)
                {
                    var lbl_DateLog = (Label)e.Row.FindControl("lbl_DateLog");
                    lbl_DateLog.Text =
                        DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString())
                            .ToString(DateTimeFormat);
                }

                // by
                if (e.Row.FindControl("lbl_ByLog") != null)
                {
                    var lbl_ByLog = (Label)e.Row.FindControl("lbl_ByLog");
                    lbl_ByLog.Text = DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString();
                    //user.GetUserDisplayName(int.Parse((DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString())), LoginInfo.ConnStr);
                }
            }
        }

        protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var logTitle = string.Empty;
            var logValue = string.Empty;
            var log = string.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Action & Log
                if (e.Row.FindControl("lbl_log") != null)
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(DataBinder.Eval(e.Row.DataItem, "Log").ToString());

                    for (var i = 0; i < xmlDoc.ChildNodes.Count; i++)
                    {
                        // Action
                        var lbl_Action = (Label)e.Row.FindControl("lbl_Action");
                        lbl_Action.Text = xmlDoc.ChildNodes[i].Name;

                        if (xmlDoc.ChildNodes[i].HasChildNodes)
                        {
                            for (var j = 0; j < xmlDoc.ChildNodes[i].ChildNodes.Count; j++)
                            {
                                // Add section changed node

                                if (xmlDoc.ChildNodes[i].ChildNodes[j].HasChildNodes)
                                {
                                    for (var k = 0; k < xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes.Count; k++)
                                    {
                                        logTitle = xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name;

                                        if (xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].HasChildNodes)
                                        {
                                            for (var kk = 0; kk < xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count; kk++)
                                            {
                                                logValue = xmlDoc.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[kk].InnerText;

                                                log = string.Format("{0} : {1} <br>", logTitle, logValue);

                                                var lbl_Log = (Label)e.Row.FindControl("lbl_Log");
                                                lbl_Log.Text += log;

                                                //if (lbl_Log.Text.Trim() != string.Empty)
                                                //{
                                                //    lbl_Log.Text += "  ,<br>" + strLog;
                                                //}
                                                //else
                                                //{
                                                //    lbl_Log.Text = strLog;
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Date
                if (e.Row.FindControl("lbl_DateLog") != null)
                {
                    var lbl_DateLog = (Label)e.Row.FindControl("lbl_DateLog");
                    lbl_DateLog.Text = String.Format("{0:d/M/yyyy HH:mm:ss}",
                        DataBinder.Eval(e.Row.DataItem, "CreatedDate"));
                    //DateTime.Parse(DataBinder.Eval(e.Row.DataItem, "CreatedDate").ToString()).ToString(DateTimeFormat);
                }

                // by
                if (e.Row.FindControl("lbl_ByLog") != null)
                {
                    var lbl_ByLog = (Label)e.Row.FindControl("lbl_ByLog");
                    lbl_ByLog.Text = DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString();
                    //user.GetUserDisplayName(int.Parse((DataBinder.Eval(e.Row.DataItem, "CreatedBy").ToString())), LoginInfo.ConnStr);
                }
            }
        }
    }
}