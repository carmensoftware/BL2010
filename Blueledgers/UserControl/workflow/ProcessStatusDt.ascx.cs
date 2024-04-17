using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.UserControls.workflow
{
    public partial class ProcessStatusDt : BaseUserControl
    {
        private Blue.BL.APP.WFHis wfHis = new Blue.BL.APP.WFHis();
        private DataSet dsWFHis = new DataSet();
        private string _apprStatus;
        private string _refNo;
        private int _refDtNo;
        private string _module;
        private string _subModule;
        private string _connString;

        public string ApprStatus
        {
            get
            {
                if (ViewState["ApprStatus"] != null)
                {
                    this._apprStatus = ViewState["ApprStatus"].ToString();
                    return this._apprStatus;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this._apprStatus = value;
                ViewState["ApprStatus"] = this._apprStatus;
            }
        }

        public string RefNo
        {
            get 
            {
                if (ViewState["RefNo"] != null)
                {
                    this._refNo = ViewState["RefNo"].ToString();
                    return this._refNo;
                }
                else
                {
                    return string.Empty;
                }
            }
            set 
            { 
                this._refNo = value;
                ViewState["RefNo"] = this._refNo;
            }
        }

        public int RefDtNo
        {
            get
            {
                if (ViewState["RefNo"] != null)
                {
                    this._refDtNo = (int)ViewState["RefDtNo"];;
                    return this._refDtNo;
                }
                else
                {
                    return 0;
                }
            }
            set 
            {
                this._refDtNo = value;
                ViewState["RefDtNo"] = this._refDtNo;             
            }
        }

        public string Module
        {
            get
            {
                if (ViewState["Module"] != null)
                {
                    this._module = ViewState["Module"].ToString();
                    return this._module;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this._module = value;
                ViewState["Module"] = this._module;
            }
        }

        public string SubModule
        {
            get
            {
                if (ViewState["SubModule"] != null)
                {
                    this._subModule = ViewState["SubModule"].ToString();
                    return this._subModule;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this._subModule = value;
                ViewState["SubModule"] = this._subModule;
            }
        }

        public string ConnString
        {
            get
            {
                if (ViewState["ConnString"] != null)
                {
                    this._connString = ViewState["ConnString"].ToString();
                    return this._connString;
                }
                else
                {
                    return string.Empty;
                }
            }
            set 
            {
                this._connString = value;
                ViewState["ConnString"] = this._connString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // No code implement here.    
        }

        public override void DataBind()
        {
            base.DataBind();

            // Display Process Status
            string newStatus = string.Empty;

            for (int i = 0; i <= this.ApprStatus.Length - 10; i += 10)
            {
                if (this.ApprStatus.Substring(i, 10).Contains('R'))
                {
                    newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/REJ.gif\" style=\"width: 8px; height: 16px; border: none;\" />";
                }
                else if (this.ApprStatus.Substring(i, 10).Contains('_') && this.ApprStatus.Substring(i, 10).Contains('P'))
                {
                    newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/PAR.gif\" style=\"width: 8px; height: 16px; border: none;\" />";
                }
                else if (this.ApprStatus.Substring(i, 10).Contains('_'))
                {
                    newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/NA.gif\" style=\"width: 8px; height: 16px; border: none;\" />";
                }
                else
                {
                    newStatus = newStatus + "<img alt=\"\" src=\"../../App_Themes/Default/Images/WF/APP.gif\" style=\"width: 8px; height: 16px; border: none;\" />";
                }
            }

            lbl_ProcessStatusDt.Text = newStatus;
        }

        protected void lnkb_ProcessStatusDt_Click(object sender, EventArgs e)
        {
            // Display Process Histoty
            if (this.RefNo != string.Empty && this.RefDtNo > 0)
            {
                bool getWFHis = wfHis.GetList(dsWFHis, this.RefNo, this.RefDtNo, this.Module, this.SubModule, this.ConnString);

                if (getWFHis)
                {
                    grd_WFHis.DataSource = dsWFHis.Tables[wfHis.TableName];
                    grd_WFHis.DataBind();

                    pop_ProcessStatusDt.ShowOnPageLoad = true;
                }
                else
                {
                    // Display Error Message
                }
            }
            else
            { 
                // Display Error Message
            }
        }
    }
}