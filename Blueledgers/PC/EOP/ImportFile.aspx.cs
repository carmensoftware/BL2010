using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Text;



using BlueLedger.PL.BaseClass;



namespace BlueLedger.PL.PC.EOP
{
    public partial class UploadFile : BasePage
    {
        private DataTable csvTable = new DataTable();


        private void BackToPreviousPage()
        {
            Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);

        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string storeId = Request.Params["StoreId"].ToString();
                string storeName = Request.Params["StoreName"].ToString();
                string eopDate = Request.Params["AtDate"].ToString();

                label_Title.Text = string.Format("Store/Location: ({0}) {1} @{2}", storeId, storeName, eopDate);
                pop_Alert.ShowOnPageLoad = false;
            }
            //base.Page_Load(sender, e);
        }

        protected void btn_Import_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;

            if (FileUpload1.HasFile)
            {
                try
                {
                    string uploadFolder = "~/Tmp/";
                    FileUpload1.SaveAs(Server.MapPath(uploadFolder) + FileUpload1.FileName);
                    //Label1.Text = "File name: " + FileUpload1.PostedFile.FileName + "  " + "<b>Uploaded Successfully";

                    FileInfo uploadFile = new FileInfo(FileUpload1.PostedFile.FileName);
                    if (uploadFile.Extension.ToLower() == ".csv")
                    {
                        string csvFilename = Server.MapPath(uploadFolder) + "/" + uploadFile.Name;
                        FileUpload1.SaveAs(csvFilename);

                        //Fetch the location of CSV file
                        //string filePath = Server.MapPath(uploadFolder);
                        //string fileFullPath = filePath + uploadFile.Name;

                        //LoadCSVToDataSet(fileFullPath);
                        LoadCSVToDataSet(csvFilename);

                        string message = IsValidFile(csvTable);

                        // Check negative qty
                        if (message == string.Empty)
                        {
                            int NoImportedCount = UpdateToEOP(csvTable);
                            int eopId = Convert.ToInt32(Request.Params["ID"].ToString());
                            UpdateEopStatus(eopId, "In Process");

                            lbl_Status.Text = "Importing file ...";


                            lbl_Alert.Text = "Import is finished.";
                            pop_Alert.ShowOnPageLoad = true;

                        }
                        else
                        {
                            lbl_Status.Text = string.Empty;

                            lbl_Alert.Text = message;
                            pop_Alert.ShowOnPageLoad = true;
                        }
                        //BackToPreviousPage();
                    }
                    else
                    {
                        //Label_Error.Text = "Unable to recognize file.";
                        errorMessage = "File only support *.csv, please check and import again.";
                    }

                }
                catch (Exception ex)
                {
                    //Label_Error.Text = "ERROR: " + ex.Message.ToString();
                    errorMessage = string.Format("Please contact support and inform error code below. <br />Error: {0}", ex.Message.ToString());
                }
            }
            else
            {
                //Label_Error.Text = "You have not specified a file.";
                errorMessage = "No file is selected.";
            }

            if (errorMessage != string.Empty)
            {
                lbl_Alert.Text = errorMessage;
                pop_Alert.ShowOnPageLoad = true;
            }

        }

        private string IsValidFile(DataTable dt)
        {
            string message = string.Empty;
            string locationCode = Request.Params["StoreId"].ToString();

            foreach (DataRow dr in dt.Rows)
            {
                string storeId = dr["StoreId"].ToString();
                string productCode = dr["ProductCode"].ToString();
                var qty = dr["Qty"].ToString();

                if (storeId != locationCode)
                {
                    message = string.Format("Some transaction is not belong to store/location '{0}'.<br />Please check and import again.", locationCode);
                    break;
                }


                if (qty == string.Empty)
                {
                    message = string.Format("Some product has empty qty such as '{0}'.<br />Please check and import again.", productCode);
                    break;
                }

                if (Convert.ToDecimal(qty) < 0)
                {
                    message = string.Format("Some product has negative qty such as '{0}'.<br />Please check and import again.", productCode);
                    break;
                }

            }

            return message;
        }


        private int getLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }
        protected void LoadCSVToDataSet(string fileName)
        {
            var csvFile = new StreamReader(fileName, Encoding.GetEncoding("tis-620"));
            var headerLine = string.Empty;
            var dataLine = string.Empty;

            // Create Table
            headerLine = csvFile.ReadLine();

            csvTable = new DataTable("CSVTable");
            DataColumn myDataColumn;

            // StoreId Column
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "StoreId";
            csvTable.Columns.Add(myDataColumn);

            // ProductCode Column
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ProductCode";
            csvTable.Columns.Add(myDataColumn);

            // Qty Column
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Qty";
            csvTable.Columns.Add(myDataColumn);


            //lbl_Status.Text = "Importing file ...";
            int lines = getLines(fileName);

            while ((dataLine = csvFile.ReadLine()) != null)
            {
                string[] row = dataLine.Split(',');
                DataRow dr = csvTable.NewRow();

                if (row[0] != string.Empty)
                {
                    dr["StoreId"] = row[0].Trim().ToString();
                    dr["ProductCode"] = row[1].Trim().ToString();

                    int lastColumn = row.Length - 1;
                    if (row[lastColumn] == string.Empty)
                        dr["Qty"] = string.Empty;
                    else
                        dr["Qty"] = row[lastColumn].Trim().ToString();
                }
                csvTable.Rows.Add(dr);
            }
            if (csvFile != null)
                csvFile.Close();
        }


        /// <returns>Returns the number of record that no updated.</returns> 
        private int UpdateToEOP(DataTable dt)
        {

            string sqlSelect = "SELECT COUNT(*) AS RecCount FROM [IN].EopDt WHERE EopId = @EopId AND ProductCode = @ProductCode";
            string sqlUpdate = "UPDATE [IN].EopDt SET Qty = @Qty WHERE EopId = @EopId AND ProductCode = @ProductCode";

            int noUpdatedCount = 0;
            int eopId = 0;
            string productCode = string.Empty;
            string qty = string.Empty;

            using (SqlConnection sqlConn = new SqlConnection(LoginInfo.ConnStr))
            {
                sqlConn.Open();
                // Select command
                SqlCommand cmdSelect = new SqlCommand(sqlSelect, sqlConn);
                cmdSelect.Parameters.Add("@EopId", SqlDbType.Int);
                cmdSelect.Parameters.Add("@ProductCode", SqlDbType.NVarChar);

                // Update command
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConn);
                cmdUpdate.Parameters.Add("@Qty", SqlDbType.Float);
                cmdUpdate.Parameters.Add("@EopId", SqlDbType.Int);
                cmdUpdate.Parameters.Add("@ProductCode", SqlDbType.NVarChar);

                foreach (DataRow dr in dt.Rows)
                {
                    eopId = Convert.ToInt32(Request.Params["ID"].ToString());
                    productCode = dr["ProductCode"].ToString();

                    // Check if exists
                    cmdSelect.Parameters["@EopId"].Value = eopId;
                    cmdSelect.Parameters["@ProductCode"].Value = productCode;
                    if ((int)cmdSelect.ExecuteScalar() > 0)
                    {
                        qty = dr["Qty"].ToString();

                        if (qty == string.Empty)
                            qty = "0.00";

                        cmdUpdate.Parameters["@Qty"].Value = Convert.ToDecimal(qty);
                        cmdUpdate.Parameters["@EopId"].Value = eopId;
                        cmdUpdate.Parameters["@ProductCode"].Value = productCode;

                        cmdUpdate.ExecuteNonQuery();
                    }
                    else
                        noUpdatedCount++;
                }

                sqlConn.Close();

                return noUpdatedCount;
            } //using
        }

        private void UpdateEopStatus(int eopId, string textStatus)
        {
            string sqlUpdate = string.Format("UPDATE [IN].Eop SET [Status] = '{0}' WHERE EopId = {1}", textStatus, eopId.ToString());


            using (SqlConnection sqlConn = new SqlConnection(LoginInfo.ConnStr))
            {
                sqlConn.Open();

                // Update command
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, sqlConn);

                cmdUpdate.ExecuteNonQuery();
                sqlConn.Close();
            }
        }

        //private void LoadDataToDatabase(string tableName, string fileFullPath, string delimeter)
        //{
        //    string sqlQuery = string.Empty;
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendFormat("IF OBJECT_ID('tempdb..#EOP') IS NOT NULL DROP TABLE #EOP ");
        //    sb.AppendFormat("\n");

        //    //ProductCode,ProductDesc1,ProductDesc2,InventoryUnit,Qty
        //    sb.AppendFormat("CREATE TABLE #EOP (");
        //    sb.AppendFormat(" ProductCode NVARCHAR(20) PRIMARY KEY,");
        //    sb.AppendFormat(" ProductDesc1 NVARCHAR(Max),");
        //    sb.AppendFormat(" ProductDesc2 NVARCHAR(Max),");
        //    sb.AppendFormat(" InventoryUnit NVARCHAR(20),");
        //    sb.AppendFormat(" Qty NUMERIC(18,6)");
        //    sb.AppendFormat(")");
        //    sb.AppendFormat("\n");

        //    sb.AppendFormat(string.Format("BULK INSERT #EOP"));
        //    sb.AppendFormat(string.Format(" FROM '{0}'", fileFullPath));
        //    sb.AppendFormat(string.Format(" WITH ( FIELDTERMINATOR = '{0}' , ROWTERMINATOR = '\n' )", delimeter));

        //    sqlQuery = sb.ToString();


        //    using (SqlConnection sqlConn = new SqlConnection(LoginInfo.ConnStr))
        //    {
        //        sqlConn.Open();
        //        SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn);
        //        sqlCmd.ExecuteNonQuery();
        //        sqlConn.Close();

        //    }

        //}
        protected void btn_Back_Click(object sender, EventArgs e)
        {
            BackToPreviousPage();
            // Response.Redirect("EOP.aspx?BuCode=" + Request.Params["BuCode"] + "&ID=" + Request.Params["ID"] + "&VID=" + Request.Params["VID"]);
        }


        protected void btn_Alert_Ok_Click(object sender, EventArgs e)
        {
            lbl_Status.Text = string.Empty;

            pop_Alert.ShowOnPageLoad = false;
        }
    }
}

