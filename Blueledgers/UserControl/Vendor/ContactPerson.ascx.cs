using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BlueLedger.PL.BaseClass;
//using DevExpress.XtraBars;
using System.IO;
using BlueLedger;


namespace BlueLedger.PL.UserControls.AP
{
    public partial class ContactPerson : BaseUserControl
    {
        #region "Attributies"

        private Blue.BL.Profile.Contact contact = new Blue.BL.Profile.Contact();
        private Blue.BL.Profile.ContactCategory contactCategory = new Blue.BL.Profile.ContactCategory();
        private Blue.BL.Profile.ContactDetail contactDetail = new Blue.BL.Profile.ContactDetail();
        private Blue.BL.Profile.ContactType contactType = new Blue.BL.Profile.ContactType();
        private Blue.BL.AP.Vendor                vendor          = new Blue.BL.AP.Vendor();

        DataSet dsContactList = new DataSet();
        DataSet dsContactCount = new DataSet();
        DataSet dsContactPerson = new DataSet();
        DataSet dsSave          = new DataSet();
        private bool _readonly  = false ;

        private int _contactID = 0;
        /// <summary>
        /// Gets or Sets current row index of selected role.
        /// </summary>
        private int ContactID
        {
            get
            {
                this._contactID = (ViewState["ContactID"] == null ? 0 : (int)ViewState["ContactID"]);
                return this._contactID;
            }
            set
            {
                this._contactID = value;
                ViewState.Add("ContactID", this._contactID);
            }
        }



        private DataSet _dataSource = new DataSet();
        /// <summary>
        /// Get or Set Control DataSource.
        /// </summary>
        public DataSet DataSource
        {
            get
            {
                this._dataSource = (ViewState["DataSource"] == null ? new DataSet() : (DataSet)ViewState["DataSource"]);
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;
                ViewState.Add("DataSource", this._dataSource);
            }

        }

        private bool _readOnly;
        /// <summary>
        /// Get or Set readonly property.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                this._readOnly = (ViewState["ReadOnly"] == null ? false : (bool)ViewState["ReadOnly"]);
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
                ViewState.Add("ReadOnly", this._readOnly);
            }
        }


        private Guid _profileCode;
        /// <summary>
        /// Get or Set readonly property.
        /// </summary>
        public Guid ProfileCode
        {
            get
            {
                this._profileCode = (ViewState["ProfileCode"] == null ? new Guid() : (Guid)ViewState["ProfileCode"]);
                return this._profileCode;
            }
            set
            {
                this._profileCode = value;
                ViewState.Add("ProfileCode", this._profileCode);
            }
        }

        #endregion

        #region "Operations"

        /// <summary>
        /// ContactPerson new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void btn_ContactPersonInfo_Click(object sender, EventArgs e)
        {
            //DataRow drNew = this.DataSource.Tables[contact.TableName].NewRow();

            DataRow drNew                = dsContactList.Tables[contact.TableName].NewRow();
            drNew["ProfileCode"]         = this.ProfileCode;
            drNew["ContactID"]           = dsContactCount.Tables[contact.TableName].Rows.Count + 1;  //contact.GetContactMaxID(LoginInfo.ConnStr);
            drNew["ContactPerson"]       = string.Empty;
            drNew["Position"]            = string.Empty;
            drNew["ContactCategoryCode"] = string.Empty;

            // Add new row
            dsContactList.Tables[contact.TableName].Rows.Add(drNew);

            DataRow drContactCount                  = dsContactCount.Tables[contact.TableName].NewRow();
            drContactCount["ProfileCode"]           = this.ProfileCode;
            drContactCount["ContactID"]             = dsContactCount.Tables[contact.TableName].Rows.Count + 1;  //contact.GetContactMaxID(LoginInfo.ConnStr);
            drContactCount["ContactPerson"]         = string.Empty;
            drContactCount["Position"]              = string.Empty;
            drContactCount["ContactCategoryCode"]   = string.Empty;

            dsContactCount.Tables[contact.TableName].Rows.Add(drContactCount);
            
            // Editing on new row
            grd_Contact.DataSource = dsContactList.Tables[contact.TableName];//this.DataSource.Tables[contact.TableName];
            grd_Contact.EditIndex  = grd_Contact.Rows.Count;
            grd_Contact.DataBind();

            Session["dsContactList"]  = dsContactList;
            Session["dsContactCount"] = dsContactCount;
            this.DataSource           = dsContactList;
            
            //DataRow drNew = this.DataSource.Tables[contact.TableName].NewRow();

            //drNew["ProfileCode"]         = this.ProfileCode;
            //drNew["ContactID"]           = dsContactList.Tables[contact.TableName].Rows.Count + 1; //contact.GetContactMaxID(LoginInfo.ConnStr);
            //drNew["ContactPerson"]       = string.Empty;
            //drNew["Position"]            = string.Empty;
            //drNew["ContactCategoryCode"] = string.Empty;

            //// Add new row
            //this.DataSource.Tables[contact.TableName].Rows.Add(drNew);

            //// Editing on new row
            //grd_Contact.DataSource = this.DataSource.Tables[contact.TableName];
            //grd_Contact.EditIndex = grd_Contact.Rows.Count;
            //grd_Contact.DataBind();

            //DataRow drContact = dsContactList.Tables[contact.TableName].NewRow();

            //drContact["ProfileCode"]   = this.ProfileCode;
            //drContact["ContactID"]     = dsContactList.Tables[contact.TableName].Rows.Count + 1; //contact.GetContactMaxID(LoginInfo.ConnStr);
            //drContact["ContactPerson"] = string.Empty;
            //drContact["Position"]      = string.Empty;
            //drContact["ContactCategoryCode"] = string.Empty;

            //dsContactList.Tables[contact.TableName].Rows.Add(drContact);
        }

        /// <summary>
        /// Contactdetail new event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ContactDetailInfo_Click(object sender, EventArgs e)
        {
            // Find button control.
            Button btn_ContactDetailInfo = (Button)((Button)sender).Parent.Parent.FindControl("btn_ContactDetailInfo");
            
            // Determine GridRow
             GridViewRow grdGridRow      = (GridViewRow)btn_ContactDetailInfo.NamingContainer;

             DataRow drNew = dsContactList.Tables[contactDetail.TableName].NewRow(); //this.DataSource.Tables[contactDetail.TableName].NewRow();

             drNew["ProfileCode"]        = this.ProfileCode;
            drNew["ContactID"]           = this.ContactID;
            drNew["ContactDetailID"]     = dsContactCount.Tables[contactDetail.TableName].Rows.Count + 1; //contactDetail.GetContactDetailMaxID(LoginInfo.ConnStr);
            drNew["ContactTypeCode"]     = 1;
            drNew["Contact"]             = "phone/email";
            drNew["Remark"]              =  string.Empty;

            // Add new row
            //this.DataSource.Tables[contactDetail.TableName].Rows.Add(drNew);
            dsContactList.Tables[contactDetail.TableName].Rows.Add(drNew);

            DataRow drContactDetailCount            = dsContactCount.Tables[contactDetail.TableName].NewRow();
            drContactDetailCount["ProfileCode"]     = this.ProfileCode;
            drContactDetailCount["ContactID"]       = this.ContactID;
            drContactDetailCount["ContactDetailID"] = dsContactCount.Tables[contactDetail.TableName].Rows.Count + 1;
            drContactDetailCount["ContactTypeCode"] = 1;
            drContactDetailCount["Contact"]         = "phone/email";
            drContactDetailCount["Remark"]          = string.Empty;

            dsContactCount.Tables[contactDetail.TableName].Rows.Add(drContactDetailCount);

            //DataView dv = new DataView(this.DataSource.Tables[contactDetail.TableName]);
            DataView dv = new DataView(dsContactList.Tables[contactDetail.TableName]);
            dv.Sort     = "ContactID ASC";           
            
            // Editing process after created the new event.
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataSource = dv;
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).EditIndex  = ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).Rows.Count;
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataBind();

            Session["dsContactList"]  = dsContactList;
            Session["dsContactCount"] = dsContactCount;
            this.DataSource           = dsContactList;           
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_ContactType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //// Find button control.
            //DropDownList ddl_ContactType = (DropDownList)((DropDownList)sender).Parent.Parent.FindControl("ddl_ContactType");

            //// Determine GridRow
            //GridViewRow grdGridRow = (GridViewRow)ddl_ContactType.NamingContainer;

            //((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).EditIndex = ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).Rows.Count;           
        }

        /// <summary>
         /// Contact row databound event.
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        protected void grd_Contact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnkb_Edit") != null)
                {
                    LinkButton lnkb_Edit = (LinkButton)e.Row.FindControl("lnkb_Edit");
                    
                    // Check edit button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Edit.Visible = false;
                    }
                }

                // Separator
                if (e.Row.FindControl("lbl_Separator") != null)
                {
                    Label lbl_Separator = (Label)e.Row.FindControl("lbl_Separator");

                    // Check Separator for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lbl_Separator.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Update") != null)
                {
                    LinkButton lnkb_Update = (LinkButton)e.Row.FindControl("lnkb_Update");

                    // Check update button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Update.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Cancel") != null)
                {
                    LinkButton lnkb_Cancel = (LinkButton)e.Row.FindControl("lnkb_Cancel");

                    // Check cancel button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Cancel.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Delete") != null)
                {
                    LinkButton lnkb_Delete = (LinkButton)e.Row.FindControl("lnkb_Delete");

                    // Check  delete button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Delete.Visible = false;
                    }
                }

                // ContactPerson
                if (e.Row.FindControl("lbl_ContactPerson") != null)
                {
                    Label lbl_ContactPerson = (Label)e.Row.FindControl("lbl_ContactPerson");
                    lbl_ContactPerson.Text = DataBinder.Eval(e.Row.DataItem, "ContactPerson").ToString();
                }

                // ContactPerson
                if (e.Row.FindControl("txt_ContactPerson") != null)
                {
                    TextBox txt_ContactPerson = (TextBox)e.Row.FindControl("txt_ContactPerson");
                    txt_ContactPerson.Text    = DataBinder.Eval(e.Row.DataItem, "ContactPerson").ToString();
                }

                // Position
                if (e.Row.FindControl("lbl_Position") != null)
                {
                    Label lbl_Position = (Label)e.Row.FindControl("lbl_Position");
                    lbl_Position.Text = DataBinder.Eval(e.Row.DataItem, "Position").ToString();
                }

                // Position
                if (e.Row.FindControl("txt_Position") != null)
                {
                    TextBox txt_Position = (TextBox)e.Row.FindControl("txt_Position");
                    txt_Position.Text = DataBinder.Eval(e.Row.DataItem, "Position").ToString();
                }

                // ContactCategoryCode
                if (e.Row.FindControl("lbl_ContactCategory") != null)
                {
                    Label lbl_ContactCategory = (Label)e.Row.FindControl("lbl_ContactCategory");
                    lbl_ContactCategory.Text = contactCategory.GetContactCategoryName(DataBinder.Eval(e.Row.DataItem, "ContactCategoryCode").ToString(), LoginInfo.ConnStr);
                }

                // ContactCategoryCode
                if (e.Row.FindControl("ddl_ContactCategory") != null)
                {
                    DropDownList ddl_ContactCategory   = (DropDownList)e.Row.FindControl("ddl_ContactCategory");
                    ddl_ContactCategory.DataSource     = contactCategory.GetContactCategory(LoginInfo.ConnStr);
                    ddl_ContactCategory.DataTextField  = "Name";
                    ddl_ContactCategory.DataValueField = "ContactCategoryCode";
                    ddl_ContactCategory.DataBind();
                    ddl_ContactCategory.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ContactCategoryCode").ToString();                    
                }
            }
        }

        /// <summary>
        /// Contact row cancelingedit event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Contact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Setup primary key(s)
            string profileCode = grd_Contact.DataKeys[e.RowIndex].Values[0].ToString();
            int contactID      = Convert.ToInt32(grd_Contact.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
            //for (int i = this.DataSource.Tables[contact.TableName].Rows.Count - 1; i >= 0; i--)
            for (int i = dsContactList.Tables[contact.TableName].Rows.Count - 1; i >= 0; i--)
            {
                //DataRow dr = this.DataSource.Tables[contact.TableName].Rows[i];
                DataRow dr = dsContactList.Tables[contact.TableName].Rows[i];
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(dr["contactID"]) == contactID &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Refresh data in gridview
            grd_Contact.DataSource = dsContactList.Tables[contact.TableName]; //this.DataSource.Tables[contact.TableName];
            grd_Contact.EditIndex = -1;
            grd_Contact.DataBind();

            // Save changed to session
            Session["dsContactList"] = dsContactList;
            this.DataSource = dsContactList;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Contact_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Setup primary key(s)
            string profileCode  = grd_Contact.DataKeys[e.RowIndex].Values[0].ToString();
            int contactID       = Convert.ToInt32(grd_Contact.DataKeys[e.RowIndex].Values[1]);

            //foreach (DataRow drUpdating in this.DataSource.Tables[contact.TableName].Rows)
            foreach (DataRow drUpdating in dsContactList.Tables[contact.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {
                    if (drUpdating["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["contactID"]) == contactID)
                    {
                        // Find controls inside of gridview.
                        TextBox txt_ContactPerson           = (TextBox)grd_Contact.Rows[e.RowIndex].FindControl("txt_ContactPerson");
                        TextBox txt_Position                = (TextBox)grd_Contact.Rows[e.RowIndex].FindControl("txt_Position");
                        DropDownList ddl_ContactCategory    = (DropDownList)grd_Contact.Rows[e.RowIndex].FindControl("ddl_ContactCategory");                        

                        // Updating for assign data.
                        // ContactPerson
                        drUpdating["ContactPerson"]       = txt_ContactPerson.Text.Trim();

                        // Position
                        drUpdating["Position"]            = txt_Position.Text.Trim();

                        // ContactCategoryCode
                        drUpdating["ContactCategoryCode"] = ddl_ContactCategory.SelectedItem.Value.ToString();

                        // Refresh data in GridView
                        //grd_Contact.EditIndex = -1;
                        //grd_Contact.DataSource = dsContactList.Tables[contact.TableName]; //this.DataSource.Tables[contact.TableName];
                        //grd_Contact.DataBind();                       

                        //continue;
                    }
                }
            }

            // Refresh data in GridView
            grd_Contact.DataSource = dsContactList.Tables[contact.TableName];

            grd_Contact.EditIndex = -1;
            grd_Contact.DataBind();

            Session["dsContactList"] = dsContactList;
            this.DataSource = dsContactList;
        }

        /// <summary>
        /// Contact row editing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Contact_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grd_Contact.EditIndex  = e.NewEditIndex;
            grd_Contact.DataSource = dsContactList.Tables[contact.TableName]; //this.DataSource.Tables[contact.TableName];
            grd_Contact.DataBind();
        }

        /// <summary>
        /// Contact row deleting event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_Contact_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Setup primary key(s)
            string profileCode = grd_Contact.DataKeys[e.RowIndex].Values[0].ToString();
            int contactID = Convert.ToInt32(grd_Contact.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            //for (int i = this.DataSource.Tables[contact.TableName].Rows.Count - 1; i >= 0; i--)
            for (int i = dsContactList.Tables[contact.TableName].Rows.Count - 1; i >= 0; i--)
            {
                //DataRow drDelete = this.DataSource.Tables[contact.TableName].Rows[i];

                DataRow drDelete = dsContactList.Tables[contact.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(drDelete["contactID"]) == contactID)
                    {
                        drDelete.Delete();
                        continue;
                    }
                }
            }

            bool result = contact.Save(dsContactList, LoginInfo.ConnStr);

            if (result)
            {
                // Binding grid
                grd_Contact.DataSource = dsContactList.Tables[contact.TableName]; //this.DataSource.Tables[contact.TableName];
                grd_Contact.DataBind();

                // Save changed to session
                Session["dsContactList"] = dsContactList;

                this.DataSource = dsContactList;
            }
            else
            {

            }
        }

        /// <summary>
        /// ContactDetait row databoud event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ContactDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataRow
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("lnkb_Edit") != null)
                {
                    LinkButton lnkb_Edit = (LinkButton)e.Row.FindControl("lnkb_Edit");

                    // Check edit button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Edit.Visible = false;
                    }
                }

                // Separator
                if (e.Row.FindControl("lbl_Separator") != null)
                {
                    Label lbl_Separator = (Label)e.Row.FindControl("lbl_Separator");

                    // Check Separator for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lbl_Separator.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Update") != null)
                {
                    LinkButton lnkb_Update = (LinkButton)e.Row.FindControl("lnkb_Update");

                    // Check update button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Update.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Cancel") != null)
                {
                    LinkButton lnkb_Cancel = (LinkButton)e.Row.FindControl("lnkb_Cancel");

                    // Check cancel button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Cancel.Visible = false;
                    }
                }

                if (e.Row.FindControl("lnkb_Delete") != null)
                {
                    LinkButton lnkb_Delete = (LinkButton)e.Row.FindControl("lnkb_Delete");

                    // Check  delete button for read only and assign visible false.
                    if (ReadOnly)
                    {
                        lnkb_Delete.Visible = false;
                    }
                }                
                
                // ContactType
                if (e.Row.FindControl("lbl_ContactType") != null)
                {
                    Label lbl_ContactType = (Label)e.Row.FindControl("lbl_ContactType");
                    lbl_ContactType.Text = contactType.GetContactTypeName(DataBinder.Eval(e.Row.DataItem, "ContactTypeCode").ToString(), LoginInfo.ConnStr);
                }

                // ContactType
                if (e.Row.FindControl("ddl_ContactType") != null)
                {
                    DropDownList ddl_ContactType = (DropDownList)e.Row.FindControl("ddl_ContactType");
                    ddl_ContactType.DataSource = contactType.GetContactTypeList(LoginInfo.ConnStr);
                    ddl_ContactType.DataTextField = "Name";
                    ddl_ContactType.DataValueField = "ContactTypeCode";
                    ddl_ContactType.DataBind();
                    ddl_ContactType.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ContactTypeCode").ToString();
                }
               
                // Contact
                if (e.Row.FindControl("lbl_Contact") != null)
                {
                    Label lbl_Contact = (Label)e.Row.FindControl("lbl_Contact");
                    lbl_Contact.Text = DataBinder.Eval(e.Row.DataItem, "Contact").ToString();
                }

                //// Contact
                //if (e.Row.FindControl("ddl_Contact") != null)
                //{
                //    DropDownList ddl_Contact = (DropDownList)e.Row.FindControl("ddl_Contact");
                //    ddl_Contact.DataSource = contactDetail.GetContactDetailLookup(LoginInfo.ConnStr);
                //    ddl_Contact.DataTextField = "Contact";
                //    ddl_Contact.DataValueField = "ContactID";
                //    ddl_Contact.DataBind();

                //    ddl_Contact.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ContactID").ToString();
                    
                //}

                // Contact
                if (e.Row.FindControl("txt_Contact") != null)
                {
                    TextBox txt_Contact = (TextBox)e.Row.FindControl("txt_Contact");
                    txt_Contact.Text    = DataBinder.Eval(e.Row.DataItem, "Contact").ToString();
                }

                // Remark
                if (e.Row.FindControl("lbl_Remark") != null)
                {
                    Label lbl_Remark = (Label)e.Row.FindControl("lbl_Remark");
                    lbl_Remark.Text = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
                }

                // Remark
                if (e.Row.FindControl("txt_Remark") != null)
                {
                    TextBox txt_Remark = (TextBox)e.Row.FindControl("txt_Remark");
                    txt_Remark.Text = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
                }
            }
        }

        /// <summary>
        /// grd_ContactDetail row cancelingedit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ContactDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Find grdivew 
            GridView grd_ContactDetail = (GridView)((GridView)sender).Parent.Parent.FindControl("grd_ContactDetail");

            // Determine GridRow
            GridViewRow grdGridRow     = (GridViewRow)grd_ContactDetail.NamingContainer;
            
            // Setup primary key(s)
            string profileCode = grd_ContactDetail.DataKeys[grdGridRow.RowIndex].Values[0].ToString();
            int contactDetailID = Convert.ToInt32(grd_ContactDetail.DataKeys[e.RowIndex].Values[1]);

            // Delete new row
//            for (int i = this.DataSource.Tables[contactDetail.TableName].Rows.Count - 1; i >= 0; i--)
            for (int i = dsContactList.Tables[contactDetail.TableName].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dsContactList.Tables[contactDetail.TableName].Rows[i];
                // DataRow dr = this.DataSource.Tables[contactDetail.TableName].Rows[i];

                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(dr["contactDetailID"]) == contactDetailID &&
                        dr.RowState == DataRowState.Added)
                    {
                        dr.Delete();
                        break;
                    }
                }
            }

            // Binding gridview by depend on each of the selected row.
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataSource = dsContactList.Tables[contactDetail.TableName]; //this.DataSource.Tables[contactDetail.TableName];
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).EditIndex = -1;
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataBind();

            // Save changed to session
            Session["dsContactList"] = dsContactList;
            this.DataSource = dsContactList;
        }

        /// <summary>
        /// Contat detail row updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ContactDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find grdivew 
            GridView grd_ContactDetail = (GridView)((GridView)sender).Parent.Parent.FindControl("grd_ContactDetail");             

            // Determine GridRow
             GridViewRow grdGridRow = (GridViewRow)grd_ContactDetail.NamingContainer;

            string profileCode  = grd_ContactDetail.DataKeys[e.RowIndex].Values[0].ToString();
            int contactDetailID = Convert.ToInt32(grd_ContactDetail.DataKeys[e.RowIndex].Values[1]);

            //foreach (DataRow drUpdating in this.DataSource.Tables[contactDetail.TableName].Rows)
             foreach (DataRow drUpdating in dsContactList.Tables[contactDetail.TableName].Rows)
            {
                if (drUpdating.RowState != DataRowState.Deleted)
                {

                    if (drUpdating["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                        Convert.ToInt32(drUpdating["contactDetailID"]) == contactDetailID)
                    {
                        // Find controls inside of gridview.
                        DropDownList ddl_ContactType = (DropDownList)grd_ContactDetail.Rows[e.RowIndex].FindControl("ddl_ContactType");
                        //DropDownList ddl_Contact     = (DropDownList)grd_ContactDetail.Rows[e.RowIndex].FindControl("ddl_Contact");
                        TextBox txt_Contact          = (TextBox)grd_ContactDetail.Rows[e.RowIndex].FindControl("txt_Contact");
                        TextBox txt_Remark           = (TextBox)grd_ContactDetail.Rows[e.RowIndex].FindControl("txt_Remark");

                        // Updating for assign data.
                        // ContactPerson
                        drUpdating["ContactTypeCode"] = ddl_ContactType.SelectedItem.Value.ToString();

                        // Contact
                        drUpdating["Contact"]     = txt_Contact.Text.Trim(); //ddl_Contact.SelectedItem.Text.Trim();

                        // Remark
                        drUpdating["Remark"]          = txt_Remark.Text.Trim();

                        //// Refresh data in GridView
                        //grd_ContactDetail.EditIndex   = -1;
                        //grd_ContactDetail.DataSource = dsContactList.Tables[contactDetail.TableName]; //this.DataSource.Tables[contactDetail.TableName];
                        //grd_ContactDetail.DataBind();

                        continue;
                    }
                }
            }

             // Refresh data in GridView
             grd_ContactDetail.DataSource = dsContactList.Tables[contactDetail.TableName];
             grd_ContactDetail.EditIndex = -1;
             grd_ContactDetail.DataBind();

             Session["dsContactList"] = dsContactList;
             this.DataSource = dsContactList;
        }

        /// <summary>
        /// Contat detail row editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ContactDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Find grdivew 
            GridView grd_ContactDetail = (GridView)((GridView)sender).Parent.Parent.FindControl("grd_ContactDetail");

            // Determine GridRow
            GridViewRow grdGridRow = (GridViewRow)grd_ContactDetail.NamingContainer;

            // Binding gridview by depend on each of the selected row.
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).EditIndex = e.NewEditIndex;
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataSource = dsContactList.Tables[contactDetail.TableName]; //this.DataSource.Tables[contactDetail.TableName];
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataBind();          
        }

        /// <summary>
        /// Contat detail row deleting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ContactDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Find grdivew 
            GridView grd_ContactDetail = (GridView)((GridView)sender).Parent.Parent.FindControl("grd_ContactDetail");

            // Determine GridRow
            GridViewRow grdGridRow     = (GridViewRow)grd_ContactDetail.NamingContainer;

            // Setup primary key(s)
            string profileCode  = grd_ContactDetail.DataKeys[grdGridRow.RowIndex].Values[0].ToString();
            int contactDetailID = Convert.ToInt32(grd_ContactDetail.DataKeys[e.RowIndex].Values[1]);

            // Delete the record 
            //for (int i = this.DataSource.Tables[contactDetail.TableName].Rows.Count - 1; i >= 0; i--)
            for (int i = dsContactList.Tables[contactDetail.TableName].Rows.Count - 1; i >= 0; i--)
            {
                //DataRow drDelete = this.DataSource.Tables[contactDetail.TableName].Rows[i];
                DataRow drDelete = dsContactList.Tables[contactDetail.TableName].Rows[i];

                if (drDelete.RowState != DataRowState.Deleted)
                {
                    if (drDelete["profileCode"].ToString().ToUpper() == profileCode.ToUpper() &&
                       Convert.ToInt32(drDelete["contactDetailID"]) == contactDetailID)
                    {
                        drDelete.Delete();
                        continue;
                    }
                }
            }

            // Binding grid
            // Binding gridview by depend on each of the selected row.
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataSource = dsContactList.Tables[contactDetail.TableName]; //this.DataSource.Tables[contactDetail.TableName];
            ((GridView)grd_Contact.Rows[grdGridRow.RowIndex].FindControl("grd_ContactDetail")).DataBind();

            // Save changed to session
            Session["dsContactList"] = dsContactList;
            this.DataSource = dsContactList;

        }

        /// <summary>
        /// Image button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void img_Detail_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton img_Detail          = (ImageButton)((ImageButton)sender).Parent.Parent.FindControl("img_Detail");
            Panel pnl_ContactDetail         = (Panel)((ImageButton)sender).Parent.Parent.FindControl("pnl_ContactDetail");

            Button btn_ContactDetailInfo    = (Button)((ImageButton)sender).Parent.Parent.FindControl("btn_ContactDetailInfo");
            GridView grd_ContactDetail      = (GridView)((ImageButton)sender).Parent.Parent.FindControl("grd_ContactDetail");
            Label lbl_ContactPerson         = (Label)((ImageButton)sender).Parent.Parent.FindControl("lbl_ContactPerson");
            HiddenField hid_ContactID       = (HiddenField)((ImageButton)sender).Parent.Parent.FindControl("hid_ContactID");

            img_Detail.ImageUrl = string.Empty;

            if (pnl_ContactDetail != null)
            {
                if (pnl_ContactDetail.Visible == true)
                {
                    pnl_ContactDetail.Visible = false;
                    img_Detail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";

                    grd_ContactDetail.DataSource = null;
                    grd_ContactDetail.DataBind();
                }
                else
                {
                    pnl_ContactDetail.Visible = true;
                    img_Detail.ImageUrl       = "~/App_Themes/default/pics/hide_detail_icon.png";

                    // if the readonly is true.not allow to create new process.
                    if (ReadOnly)
                    {
                        // visible for new button.
                        btn_ContactDetailInfo.Visible = false;                    
                    }

                    if (lbl_ContactPerson.Text.ToString() != string.Empty)
                    {
                        GridViewRow grdGridRow;

                        int contactID;

                        // Determine GridRow
                        grdGridRow = (GridViewRow)img_Detail.NamingContainer;

                        // Derive DataKey from GridRow
                        contactID = Convert.ToInt32(grd_Contact.DataKeys[grdGridRow.RowIndex].Values[1]);

                        // assign contact ID for public use.
                        ContactID = contactID;

                        // Clear table value before binding.
                        //this.DataSource.Tables[contactDetail.TableName].Clear();

                        dsContactList.Tables[contactDetail.TableName].Clear();

                        //contactDetail.GetContactDetailList(Convert.ToInt32(contactID), this.DataSource, LoginInfo.ConnStr);

                        contactDetail.GetContactDetailList(Convert.ToInt32(contactID), dsContactList, LoginInfo.ConnStr);
                    }

                    grd_ContactDetail.DataSource = dsContactList.Tables[contactDetail.TableName]; //this.DataSource.Tables[contactDetail.TableName];
                    grd_ContactDetail.DataBind();
                }
            }
            else
            {
                img_Detail.ImageUrl = "~/App_Themes/default/pics/show_detail_icon.png";
                this.MessageBox("Please try for update process complete !");
            }            
        }

        /// <summary>
        /// Get contact person data from session dataset
        /// </summary>
        private void Page_Retrieve()
        {   
            //contact.GetContactList(dsContactList, LoginInfo.ConnStr);
            // Get contact information.
            contact.GetContactList(this.ProfileCode.ToString(), dsContactList, LoginInfo.ConnStr);
            contactDetail.GetContactDetailStructure(dsContactList, LoginInfo.ConnStr);
            
            contact.GetContactList(dsContactCount, LoginInfo.ConnStr);
            contactDetail.GetContactDetailList(dsContactCount, LoginInfo.ConnStr);

            Session["dsContactList"] = dsContactList;
            Session["dsContactCount"] = dsContactCount;

            this.DataSource = dsContactList;
        }

        /// <summary>
        /// Display bankaccount data.
        /// </summary>
        private void Page_Setting()
        {
            // if the readonly is true.edit/delte/update/new process would not be allow.
            if (ReadOnly)
            {                
                btn_ContactPersonInfo.Visible        = false;                
            }

            grd_Contact.DataSource = dsContactList.Tables[contact.TableName]; //this.DataSource.Tables[contact.TableName];
            grd_Contact.DataBind();
        }

        /// <summary>
        /// Main process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page_Retrieve();
                this.Page_Setting();
            }
            else
            {
               dsContactList = (DataSet)Session["dsContactList"] ;
               dsContactCount = (DataSet)Session["dsContactCount"];
            }           
        }


        /// <summary>
        /// Error message
        /// </summary>
        /// <param name="msg"></param>
        private void MessageBox(string msg)
        {
            Label lbl = new Label();
            lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
            Page.Controls.Add(lbl);
        }

        #endregion
    }
}