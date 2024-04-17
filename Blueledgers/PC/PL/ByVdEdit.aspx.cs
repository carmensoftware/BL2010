using System;
using System.Data;
using BlueLedger.PL.BaseClass;

namespace BlueLedger.PL.PC.PL
{
    public partial class ByVdEdit : BasePage
    {
        #region "Attributes"

        private readonly Blue.BL.IN.PriceList priceList = new Blue.BL.IN.PriceList();
        private readonly Blue.BL.AP.Vendor vendor = new Blue.BL.AP.Vendor();

        private DataSet dsPriceList = new DataSet();

        #endregion

        #region "Operations"

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Retrieve();
            }
            else
            {
                dsPriceList = (DataSet) Session["dsPriceList"];
            }
        }

        /// <summary>
        /// </summary>
        private void Page_Retrieve()
        {
            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                // Get schema.
                priceList.GetSchema(dsPriceList, LoginInfo.ConnStr);

                // Create new row.
                var drNew = dsPriceList.Tables[priceList.TableName].NewRow();
                drNew["PrlNo"] = Guid.NewGuid();

                dsPriceList.Tables[priceList.TableName].Rows.Add(drNew);
            }
            else
            {
                var keys = Request.Params["ID"].Split(',');

                // Get Price List Data
                priceList.GetListByDateFromDateToVendor(dsPriceList, keys[0], keys[1], keys[2], LoginInfo.ConnStr);
            }

            //Get data for lookup
            vendor.GetList(dsPriceList, LoginInfo.ConnStr);

            Session["dsPriceList"] = dsPriceList;

            Page_Setting();
        }

        /// <summary>
        /// </summary>
        private void Page_Setting()
        {
            cmb_Vendor.DataSource = dsPriceList.Tables[vendor.TableName];
            cmb_Vendor.DataBind();

            if (Request.Params["MODE"].ToUpper() == "NEW")
            {
                dte_DateFrom.Date = ServerDateTime;
                dte_DateTo.Date = ServerDateTime;
            }
            else
            {
                if (dsPriceList.Tables[priceList.TableName].Rows.Count > 0)
                {
                    var drPLT = dsPriceList.Tables[priceList.TableName].Rows[0];

                    txt_RefNo.Text = drPLT["RefNo"].ToString();
                    cmb_Vendor.Value = drPLT["VendorCode"].ToString();
                    txt_VendorName.Text = vendor.GetName(drPLT["VendorCode"].ToString(), LoginInfo.ConnStr);
                    dte_DateFrom.Date = DateTime.Parse(drPLT["DateFrom"].ToString());
                    dte_DateTo.Date = DateTime.Parse(drPLT["DateTo"].ToString());
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            lbl_CheckSaveNew.Text = string.Empty;

            switch (e.Item.Text.ToUpper())
            {
                case "SAVE":
                    // Check request field.
                    if (cmb_Vendor.Value == null || dte_DateFrom.Value == null || dte_DateTo.Value == null)
                    {
                        if (cmb_Vendor.Value == null)
                        {
                            lbl_CheckSaveNew.Text = "Vendor Code must be greater than 0.";
                        }
                        else if (dte_DateFrom.Value == null)
                        {
                            if (lbl_CheckSaveNew.Text != string.Empty)
                            {
                                lbl_CheckSaveNew.Text += "<br> Date From can not be empty.";
                            }
                            else
                            {
                                lbl_CheckSaveNew.Text = "Date From can not be empty.";
                            }
                        }
                        else if (dte_DateTo.Value == null)
                        {
                            if (lbl_CheckSaveNew.Text != string.Empty)
                            {
                                lbl_CheckSaveNew.Text += "<br> Date To can not be empty.";
                            }
                            else
                            {
                                lbl_CheckSaveNew.Text = "Date To can not be empty.";
                            }
                        }

                        pop_CheckSaveNew.ShowOnPageLoad = true;
                        return;
                    }

                    // Commit Change to Database.
                    var drSave = dsPriceList.Tables[priceList.TableName].Rows[0];

                    drSave["RefNo"] = txt_RefNo.Text;
                    drSave["VendorCode"] = cmb_Vendor.Value;
                    drSave["DateFrom"] = dte_DateFrom.Date.Date;
                    drSave["DateTo"] = dte_DateTo.Date.Date;
                    drSave["CreatedDate"] = ServerDateTime;
                    drSave["CreatedBy"] = LoginInfo.LoginName;
                    drSave["UpdatedDate"] = ServerDateTime;
                    drSave["UpdatedBy"] = LoginInfo.LoginName;

                    var result = priceList.Save(dsPriceList, LoginInfo.ConnStr);
                    if (result)
                    {
                        //Session["dsPriceList"] = null;
                        Response.Redirect("ByVd.aspx?ID=" + dte_DateFrom.Date.Date + "," + dte_DateTo.Date.Date + "," +
                                          cmb_Vendor.Value);
                    }

                    break;

                case "BACK":

                    if (Request.Params["MODE"].ToUpper() == "NEW")
                    {
                        Response.Redirect("ByVdLst.aspx");
                    }
                    else
                    {
                        Response.Redirect("ByVd.aspx?ID=" + Request.Params["ID"]);
                    }

                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Vendor_Load(object sender, EventArgs e)
        {
            cmb_Vendor.DataSource = dsPriceList.Tables[vendor.TableName];
            cmb_Vendor.DataBind();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmb_Vendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strCode = cmb_Vendor.Value.ToString();

            if (strCode != string.Empty)
            {
                txt_VendorName.Text = vendor.GetName(strCode, LoginInfo.ConnStr);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dte_DateTo_DateChanged(object sender, EventArgs e)
        {
            if (dte_DateTo.Date.Date < dte_DateFrom.Date.Date)
            {
                lbl_CheckSaveNew.Text = "Start date must greater than start date";
                pop_CheckSaveNew.ShowOnPageLoad = true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dte_DateFrom_DateChanged(object sender, EventArgs e)
        {
            if (dte_DateFrom.Date.Date > dte_DateTo.Date.Date)
            {
                lbl_CheckSaveNew.Text = "Start date must less than ended date";
                pop_CheckSaveNew.ShowOnPageLoad = true;
            }
        }

        #endregion
    }
}