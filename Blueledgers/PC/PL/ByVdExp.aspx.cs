using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using BlueLedger.PL.BaseClass;
using System.IO;
using System.Web;
using Microsoft.VisualBasic.FileIO;

namespace BlueLedger.PL.PC.PL
{
    public partial class ByVdExp : BasePage
    {

        private readonly Blue.BL.Option.Inventory.Product product = new Blue.BL.Option.Inventory.Product();
        private readonly Blue.BL.Option.Inventory.ProdCat prodCat = new Blue.BL.Option.Inventory.ProdCat();

        private readonly DataSet dsProduct = new DataSet();
        private DataSet dsProductExp = new DataSet();
        private DataTable dtProduct = new DataTable();

        //private DataSet _dsProdExp
        //{
        //    get { return (DataSet)ViewState["dsProdExp"]; }
        //    set { ViewState["dsProdExp"] = value; }
        //}


        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                Page_Setting();
            }
            else
            {
                dsProductExp = (DataSet)ViewState["dsProductExp"];
            }
        }

        private void Page_Setting()
        {

            ddl_Cat.DataSource = prodCat.GetListByParentNo("0", LoginInfo.ConnStr);
            ddl_Cat.DataTextField = "CategoryName";
            ddl_Cat.DataValueField = "CategoryCode";
            ddl_Cat.DataBind();
            ddl_Cat.Items.Insert(0, new ListItem("", "-1"));


            ddl_SubCat.DataSource = prodCat.GetListByParentNo(ddl_Cat.SelectedItem.Value == string.Empty ? "-1" : ddl_Cat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_SubCat.DataTextField = "CategoryName";
            ddl_SubCat.DataValueField = "CategoryCode";
            ddl_SubCat.DataBind();
            ddl_SubCat.Items.Insert(0, new ListItem("All", ""));

            ddl_ItemGrp.DataSource = prodCat.GetListByParentNo(ddl_SubCat.SelectedItem.Value == string.Empty ? "-1" : ddl_SubCat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_ItemGrp.DataTextField = "CategoryName";
            ddl_ItemGrp.DataValueField = "CategoryCode";
            ddl_ItemGrp.DataBind();
            ddl_ItemGrp.Items.Insert(0, new ListItem("All", ""));

            var getProductExp = product.GetProductExport(dsProductExp, ddl_Cat.SelectedItem.Value,
                ddl_SubCat.SelectedItem.Value,
                ddl_ItemGrp.SelectedItem.Value, LoginInfo.ConnStr);

            if (getProductExp)
            {
                grd_ProductExport.DataSource = dsProductExp.Tables[product.TableName];
                grd_ProductExport.DataBind();
            }


            //_dsProdExp = dsProductExp;

            ViewState["dsProductExp"] = dsProductExp;
        }

        protected void menu_CmdBar_ItemClick(object source, DevExpress.Web.ASPxMenu.MenuItemEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "SAVE":
                    //gridExport.WriteCsvToResponse("PriceList");

                    var CountProd = 0;

                    for (var i = 0; i <= grd_ProductExport.Rows.Count - 1; i++)
                    {
                        var Chk_Item = grd_ProductExport.Rows[i].FindControl("Chk_Item") as CheckBox;

                        //Count Select Product
                        if (Chk_Item.Checked)
                        {
                            CountProd++;
                        }
                    }

                    if (CountProd > 0)
                    {
                        ExportToCSV();
                    }
                    else
                    {
                        pop_SelectProd.ShowOnPageLoad = true;
                    }

                    break;
            }
        }

        protected void ExportToCSV()
        {
            var sb = new StringBuilder();

            // Header Row
            string header = string.Empty;

            //header = "\uFEFF";
            for (var i = 1; i < grd_ProductExport.Columns.Count; i++)
            {
                sb.Append(grd_ProductExport.Columns[i].HeaderText + ',');
            }
            sb.Append("\n");

            // Detail
            foreach (GridViewRow row in grd_ProductExport.Rows)
            {
                //string accessType = row.Cells[3].Text;
                if ((row.FindControl("Chk_Item") as CheckBox).Checked) // if it is selected.
                {
                    string line = string.Empty;
                    for (var i = 1; i < grd_ProductExport.Columns.Count; i++)
                    {
                        var value = row.Cells[i].Text.Replace(',', ' ');
                        //if (i == 1 && value.StartsWith("0"))
						//if (i == 1)	
                            //value = "'" + value;

                        line += value  + ",";
                    }
                    sb.AppendLine(line);
                }
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/text";
            //Response.ContentType = "text/csv";
            //Response.ContentType = "application/csv";
            Response.AddHeader("content-disposition", "attachment;filename=PriceList.csv");
            Response.Charset = "utf-8";

            var decodeText = HttpUtility.HtmlDecode(sb.ToString());
            Response.BinaryWrite(Encoding.UTF8.GetPreamble());

            Response.ContentEncoding = System.Text.Encoding.GetEncoding(Response.Charset);

            Response.Output.Write(decodeText);
            Response.Flush();
            Response.End();

        }

        /// <summary>
        ///     Refresh Subcategory, ItemGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsProductExp.Clear();

            ddl_SubCat.DataSource = prodCat.GetListByParentNo(ddl_Cat.SelectedItem.Value == string.Empty ? "0" : ddl_Cat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_SubCat.DataTextField = "CategoryName";
            ddl_SubCat.DataValueField = "CategoryCode";
            ddl_SubCat.DataBind();
            ddl_SubCat.Items.Insert(0, new ListItem("All", ""));

            //ddl_ItemGrp.DataSource = prodCat.GetListByParentNo(ddl_SubCat.SelectedItem.Value == string.Empty ? "0" : ddl_SubCat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_ItemGrp.DataSource = prodCat.GetListByParentNo(ddl_Cat.SelectedItem.Value == string.Empty ? "0" : ddl_Cat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_ItemGrp.DataTextField = "CategoryName";
            ddl_ItemGrp.DataValueField = "CategoryCode";
            ddl_ItemGrp.DataBind();
            ddl_ItemGrp.Items.Insert(0, new ListItem("All", ""));

            var getProductExp = product.GetProductExport(dsProductExp, ddl_Cat.SelectedItem.Value, ddl_SubCat.SelectedItem.Value, ddl_ItemGrp.SelectedItem.Value, LoginInfo.ConnStr);

            if (getProductExp)
            {
                //_dsProdExp = dsProductExp;
                grd_ProductExport.DataSource = dsProductExp.Tables[product.TableName];
                grd_ProductExport.DataBind();
            }
        }

        /// <summary>
        ///     Refresh ItemGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_SubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsProductExp.Clear();

            ddl_ItemGrp.DataSource = prodCat.GetListByParentNo(ddl_SubCat.SelectedItem.Value == string.Empty ? "0" : ddl_SubCat.SelectedItem.Value, LoginInfo.ConnStr);
            ddl_ItemGrp.DataTextField = "CategoryName";
            ddl_ItemGrp.DataValueField = "CategoryCode";
            ddl_ItemGrp.DataBind();
            ddl_ItemGrp.Items.Insert(0, new ListItem("All", ""));

            var getProductExp = product.GetProductExport(dsProductExp, ddl_Cat.SelectedItem.Value,
                ddl_SubCat.SelectedItem.Value,
                ddl_ItemGrp.SelectedItem.Value, LoginInfo.ConnStr);

            if (getProductExp)
            {
                //_dsProdExp = dsProductExp;
                grd_ProductExport.DataSource = dsProductExp.Tables[product.TableName];
                grd_ProductExport.DataBind();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_ItemGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsProductExp.Clear();

            var getProductExp = product.GetProductExport(dsProductExp, ddl_Cat.SelectedItem.Value, ddl_SubCat.SelectedItem.Value, ddl_ItemGrp.SelectedItem.Value, LoginInfo.ConnStr);

            if (getProductExp)
            {
                //_dsProdExp = dsProductExp;
                grd_ProductExport.DataSource = dsProductExp.Tables[product.TableName];
                grd_ProductExport.DataBind();
            }
        }

        protected void grd_ProductExport_Load(object sender, EventArgs e)
        {
        }

        protected void btn_SelectProduct_Click(object sender, EventArgs e)
        {
            pop_SelectProd.ShowOnPageLoad = false;
        }

        #region "Misc"

        private DataColumn[] GetPK()
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsProduct.Tables[product.TableName].Columns["SKU#"];

            return primaryKeys;
        }

        #endregion
    }
}