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

        // Properties

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

        // Public method(s)

        public override void DataBind()
        {
            base.DataBind();

            BindLog();
        }


        // Event(s)

        protected void gvLog_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            (sender as GridView).PageIndex = e.NewPageIndex;
            BindLog();  
        }


        // Private method(s)

        private void BindLog()
        {
            var dtLog = new DataTable();

            if (Module != string.Empty && SubModule != string.Empty && RefNo != string.Empty)
            {
                dtLog = transLog.GetTransLogByModule_SubModule_RefNo(Module, SubModule, RefNo, LoginInfo.ConnStr);

                if (dtLog != null)
                {
                    dtLog.TableName = TableName;
                    gvLog.DataSource = dtLog;
                    gvLog.DataBind();
                }
            }
        }
    }
}