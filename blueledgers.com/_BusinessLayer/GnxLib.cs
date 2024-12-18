using System;
using System.Management;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using Blue.DAL;

namespace Blue.BL
{
    public class GnxLib
    {
        #region "Attributies & Enumerations"

        /// <summary>
        ///     Use for check group code in account reconcile
        /// </summary>
        public enum AccountGroup
        {
            Bank = 3
        }

        public const string KEY_LOGIN_PASSWORD = "blueledgers.com";

        public enum EnDeCryptor
        {
            EnCrypt = 0,
            DeCrypt = 1
        }


        /// <summary>
        ///     Use for indentify report category
        /// </summary>
        public enum ReportCategory
        {
            //AccountPayable = 1,
            //AccountReceivable = 2,
            GeneralLedger = 3,
            Consolidate = 4,
            Forecast = 5,
            Income = 6,
            Daily = 000,
            PurchaseOrder = 001,
            PriceList = 002,
            Recipe = 003,
            RevenueandCostManagement = 004,
            Statistic = 005,
            GeneralLedgers = 006,
            AccountPayable = 007,
            AccountReceivable = 008,
            Maintenance = 009,
            Receiving = 010
        }

        /// <summary>
        ///     Use for status of reconcile
        /// </summary>
        public enum StatusReconcile
        {
            UnReconcile = 0,
            Reconciled = 1,
            Partial = 2
        }

        private BL.ProjectAdmin.SysParameter sysParameter = new BL.ProjectAdmin.SysParameter();

        #endregion

        #region "Operations"

        /// <summary>
        ///     This function is used to encrypt or decrypt the string by using MD5
        ///     CryptoService and Triple DES CryptoService
        /// </summary>
        /// <param name="text">Original String that you want to encrypt or decrypt</param>
        /// <param name="cryptType">Boolean value specifying whether current request should be encrypt or decrypt</param>
        /// <returns>Encrypted String or Decrypted string return depend on Input Encrypt Boolean Variable</returns>
        public static string EnDecryptString(string text, EnDeCryptor cryptType, string key = null)
        {
            //string key = null;
            if (key == null || key == string.Empty)
                key = "BinEnPassCoder";

            string EncryptString = null;
            string DecryptString = null;
            byte[] HashKey = null;
            byte[] buff = null;
            MD5CryptoServiceProvider HashMD5 = null;
            TripleDESCryptoServiceProvider Des3 = null;

            if (text != null || text.Length != 0)
            {
                HashMD5 = new MD5CryptoServiceProvider();
                //Key = "BinEnPassCoder";
                HashKey = HashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                Des3 = new TripleDESCryptoServiceProvider();
                Des3.Key = HashKey;
                Des3.Mode = CipherMode.ECB;

                if (cryptType == EnDeCryptor.EnCrypt)
                {
                    //CONVERT TO ARRAY BYTE
                    buff = ASCIIEncoding.ASCII.GetBytes(text);

                    //ENCRYPT
                    EncryptString = Convert.ToBase64String(Des3.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
                    var renString = BinToHex(EncryptString);

                    return renString;
                }
                else
                {
                    var sTmp = HexToStr(text);
                    buff = Convert.FromBase64String(sTmp);
                    DecryptString =
                        ASCIIEncoding.ASCII.GetString(Des3.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));

                    return DecryptString;
                }
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        ///     The function is used to convert Binary to HexaDecimal String
        /// </summary>
        /// <param name="strVal">A binary value string input</param>
        /// <returns>Converted HexaDecimal Value as a string</returns>
        public static string BinToHex(string strVal)
        {
            var strRetVal = "";
            var nLength = strVal.Length;

            for (var i = 0; i < nLength; i++)
            {
                var strTemp = Uri.HexEscape(strVal[i]);
                strRetVal += strTemp.Substring(1);
            }

            return strRetVal;
        }

        /// <summary>
        ///     The function is used to convert HexaDecimal value into String
        /// </summary>
        /// <param name="strHex">Input HexaDecimal string</param>
        /// <returns>Converted String Value of current input HexaDecimal Value</returns>
        public static string HexToStr(string strHex)
        {
            var strRet = "";
            var nCount = strHex.Length / 2;
            var iIdx = 0;

            for (var i = 0; i < nCount; i++)
            {
                iIdx = 0;
                var strTemp = "%" + strHex.Substring(i * 2, 2);
                strRet += Uri.HexUnescape(strTemp, ref iIdx);
            }

            return strRet;
        }

        /// <summary>
        ///     The function is used for get the system date from data base server.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSysDate(string ConnectionString = "")
        {
            var sysParameter = new BL.ProjectAdmin.SysParameter();

            var dbType = ConfigurationManager.AppSettings["DBType"].ToString();


            if (dbType.ToUpper() == "SQL")
            {
                var sqlHelper = new DAL.SQL.SqlHelper();
                return sqlHelper.SqlDate(ConnectionString);
            }

            return DateTime.Now;
        }

        /// <summary>
        ///     Rounding the decimal number in 2 decimals format
        /// </summary>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static decimal Round2Digits(decimal rounding, string connStr)
        {
            var sysParameter = new BL.ProjectAdmin.SysParameter();

            var midPointRounding = int.Parse(sysParameter.GetValue("System", "MidPointRounding", connStr));

            if (midPointRounding == 0)
            {
                // AwayFromZero
                return decimal.Round(rounding, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                // ToEven (Bank Rounding)
                return decimal.Round(rounding, 2, MidpointRounding.ToEven);
            }
        }

        /// <summary>
        ///     Convert external date format(dd/MM/yyyy) to internal date format(yyyy-MM-dd)
        /// </summary>
        /// <param name="exDate">dd/MM/yyyy</param>
        /// <returns></returns>
        public static string ToIntDate(string exDate)
        {
            var result = string.Empty;
            var date = exDate.Split('/');

            result = date[2] + "-" + date[1] + "-" + date[0];

            // Return result
            return result;
        }

        /// <summary>
        ///     Get data in specified field.
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ReturnValueField"></param>
        /// <param name="ParameterField"></param>
        /// <param name="ParameterValue"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static string GetFieldValue(string TableName, string ReturnValueField, string ParameterField,
            string ParameterValue, string ConnectionString)
        {
            var dbHandler = new DbHandler();
            var dbParams = new DbParameter[1];
            var result = string.Empty;
            var command = string.Empty;

            // Create Select Command
            command = "SELECT [" + ReturnValueField + "] FROM " + TableName + " WHERE [" + ParameterField + "] = @" +
                      ParameterField;

            // Create Parameter            
            dbParams[0] = new DbParameter("@" + ParameterField, ParameterValue);

            // Get data
            var dtResult = dbHandler.DbExecuteQuery(command, dbParams, ConnectionString);

            if (dtResult != null)
            {
                if (dtResult.Rows.Count > 0)
                {
                    result = (dtResult.Rows[0][ReturnValueField] == DBNull.Value
                        ? string.Empty
                        : dtResult.Rows[0][ReturnValueField].ToString());
                }
            }

            return result;
        }

        /// <summary>
        ///     Get data list in specified table.
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static DataTable GetDataList(string TableName, string ConnectionString)
        {
            var dbHandler = new DbHandler();
            var command = string.Empty;

            // Create Select Command
            command = "SELECT * FROM " + TableName;

            // Get data
            var dtResult = dbHandler.DbExecuteQuery(command, null, ConnectionString);

            if (dtResult != null)
            {
                return dtResult;
            }

            return null;
        }

        /// <summary>
        ///     Check unique value on specified field with a given value.
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <param name="FieldValue"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static bool CheckDBUniqueValue(string SchemaName, string TableName, string FieldName, string FieldValue,
            string ConnectionString)
        {
            var cmdSelect = string.Empty;
            var dbHandler = new DbHandler();
            var dbParameter = new DbParameter[1];
            var dtUnique = new DataTable();
            var uniqued = true;

            // Create command
            cmdSelect = "SELECT COUNT(*) AS ValueOccurs FROM [" + SchemaName + "].[" + TableName + "] WHERE [" +
                        FieldName + "] = @" + FieldName;

            // Create Parameter
            dbParameter[0] = new DbParameter("@" + FieldName, FieldValue);

            // Get data
            dtUnique = dbHandler.DbExecuteQuery(cmdSelect, dbParameter, ConnectionString);

            if (dtUnique != null)
            {
                if (dtUnique.Rows.Count > 0)
                {
                    if (dtUnique.Rows[0]["ValueOccurs"] != DBNull.Value)
                    {
                        if ((int)dtUnique.Rows[0]["ValueOccurs"] > 0)
                        {
                            uniqued = false;
                        }
                    }
                }
            }

            // Return result
            return uniqued;
        }

        /// <summary>
        ///     Check statement syntax is correct or not.
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <param name="FieldValue"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static bool SqlParse(DataTable view, DataTable viewCriteria, DataTable viewColumn, string connStr)
        {
            var viewQuery = string.Empty;
            var columnList = string.Empty;
            var whereClause = string.Empty;

            var field = new Blue.BL.Application.Field();
            //BL.APP.Field field = new BL.APP.Field();

            // Generate Column
            if (viewColumn.Rows.Count > 0)
            {
                foreach (DataRow dr in viewColumn.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        columnList += (columnList != string.Empty ? "," : string.Empty) +
                                      field.GetFieldName(dr["FieldID"].ToString(), connStr);
                    }
                }
            }

            // Generate Criteria
            if (viewCriteria.Rows.Count > 0)
            {
                // Non-Advance option
                if (view.Rows[0]["IsAdvance"].ToString() != "True")
                {
                    foreach (DataRow dr in viewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            whereClause += (whereClause != string.Empty ? " " : string.Empty) +
                                           field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " + dr["Operator"] +
                                           " '" +
                                           dr["Value"].ToString() + "' " + dr["LogicalOp"].ToString();
                        }
                    }
                }
                // Advance option
                else
                {
                    whereClause = view.Rows[0]["AvanceOption"].ToString();

                    foreach (DataRow dr in viewCriteria.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            var eachWhereClause = field.GetFieldName(dr["FieldID"].ToString(), connStr) + " " +
                                                  dr["Operator"] + " '" +
                                                  dr["Value"].ToString() + "' ";
                            whereClause = whereClause.Replace(dr["SeqNo"].ToString(), eachWhereClause);
                        }
                    }
                }
            }

            viewQuery = "SELECT " + columnList + " FROM ANYTABLE " +
                        (whereClause == string.Empty ? string.Empty : "where " + whereClause);

            var validateQuery = new StringBuilder();

            validateQuery.Append("BEGIN TRY ");
            validateQuery.Append("SET PARSEONLY ON " + viewQuery + " ");
            validateQuery.Append("END TRY ");
            validateQuery.Append("BEGIN CATCH ");
            validateQuery.Append("SELECT ERROR_MESSAGE() ");
            validateQuery.Append("END CATCH");

            var result = new DbHandler().DbParseQuery(validateQuery.ToString(), null, connStr);

            // Return result
            return result;
        }

        /// <summary>
        ///     Generate xml format for log information.
        /// </summary>
        /// <returns></returns>
        public static string GetXMLFormat(string action, ArrayList arrayCombine)
        {
            var doc = new XmlDocument();

            if (action.ToUpper() == "MODIFY")
            {
                // Create the rootnode for MODIFY.
                XmlNode rootNode = doc.CreateElement("MODIFY");
                doc.AppendChild(rootNode);

                // Creat tablenames arraylist to add all of the tablename
                var tableNames = new ArrayList();

                // separate tablename,fieldname and values.
                foreach (var itemTable in arrayCombine)
                {
                    var ItemArrayTable = ((string[])itemTable);

                    for (var j = 0; j < ItemArrayTable.Length - 2; j++)
                    {
                        var tableNameOnly = ItemArrayTable[j];

                        tableNames.Add(tableNameOnly);
                    }
                }

                // Create arraylist for filter duplicate tablename.
                var filterDups = new ArrayList();

                foreach (string strItem in tableNames)
                {
                    if (!filterDups.Contains(strItem.Trim()))
                    {
                        filterDups.Add(strItem.Trim());
                        filterDups.Sort();
                    }
                }

                // After filter the tablename and creat for xml node.
                for (var i = 0; i < filterDups.Count; i++)
                {
                    // Create table node dynamically.
                    XmlNode tablesNode = doc.CreateElement("TABLE");
                    var tableAttribute = doc.CreateAttribute("id");
                    tableAttribute.Value = filterDups[i].ToString();
                    tablesNode.Attributes.Append(tableAttribute);
                    rootNode.AppendChild(tablesNode);

                    // loop for arrylistcombie that included tablename/filedname/fieldvalue
                    foreach (var item in arrayCombine)
                    {
                        var ItemArray = ((string[])item);

                        // separate tablename,fieldname and values.
                        for (var j = 0; j < ItemArray.Length - 2; j++)
                        {
                            // get tablename from arraylist
                            var tableName = ItemArray[j];

                            // condition for tablename.
                            if (filterDups[i].ToString() == tableName.ToString())
                            {
                                // obj1 assigned for fieldname.
                                var obj1 = ItemArray[j + 1];

                                // obj2 assigned for fieldvalue.
                                var obj2 = ItemArray[j + 2];

                                // In the arrylist value of fieldname and values are not same
                                // Create to Xmlnode for fieldName and values changes.
                                if (obj2.ToString() != obj1.ToString())
                                {
                                    XmlNode nameNode = doc.CreateElement(obj1.ToString());
                                    nameNode.AppendChild(doc.CreateTextNode(obj2.ToString()));
                                    tablesNode.AppendChild(nameNode);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Create the rootnode for CREATE.
                XmlNode rootNode = doc.CreateElement("CREATE");
                doc.AppendChild(rootNode);
            }

            // return xml format.
            return doc.InnerXml;
        }

        /// <summary>
        ///     Get horizental alignment from DataColumn.DataType
        /// </summary>
        /// <param name="grdColumn"></param>
        /// <returns></returns>
        public static HorizontalAlign GetColumnHorizentalAlign(DataColumn grdColumn)
        {
            switch (grdColumn.DataType.Name.ToString().ToUpper())
            {
                case "BYTE":
                case "DECIMAL":
                case "DOUBLE":
                case "INT16":
                case "INT32":
                case "INT64":
                case "SBYTE":
                case "SINGLE":
                case "UINT16":
                case "UINT32":
                case "UINT64":
                    return HorizontalAlign.Right;
                default:
                    return HorizontalAlign.Left;
            }
        }

        public static string GenPassword()
        {
            var rnd = new Random();
            var newPassword = new byte[8];

            for (var i = 0; i <= 7; i++)
            {
                newPassword[i] = (byte)rnd.Next(65, 90);
            }

            return System.Text.Encoding.UTF8.GetString(newPassword);
        }

        public static void SendMail(string receivers, string subjects, string body)
        {
            var PingSender = new Ping();
            var mSMTP = ConfigurationManager.AppSettings["mSMTP"];
            var Reply = PingSender.Send(mSMTP);
            if (Reply.Status == IPStatus.Success)
            {
                var SenderMail = ConfigurationManager.AppSettings["SenderMail"];
                var DisplayName = ConfigurationManager.AppSettings["DisplayName"];
                var SenderPass = ConfigurationManager.AppSettings["SenderPass"];
                //string mSMTP = ConfigurationManager.AppSettings["mSMTP"];
                var Mails = new MailMessage();
                var Receivers = receivers.Split(';');
                foreach (var Receiver in Receivers)
                {
                    Mails.To.Add(Receiver);
                }
                Mails.From = new MailAddress(SenderMail, DisplayName);
                Mails.Subject = subjects;
                Mails.Body = body;
                var mailClient = new SmtpClient(mSMTP);
                var userInfo = new System.Net.NetworkCredential(SenderMail, SenderPass);
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = userInfo;
                mailClient.Send(Mails);
                Mails = null;
            }
            else
            {
                MessageBox.Show("Server is not Connect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // คำนวนหา ผลรวมราคาก่อนหักภาษี
        public static decimal CalAmt(decimal Price, decimal Discount, decimal Quantity)
        {
            var Result = (Price - Discount) * Quantity;
            Result = Math.Round(Result, 2);
            return Result;
        }

        // คำนวนหา ผลรวมของภาษี
        public static decimal TaxAmt(string TaxType, decimal TaxRate, decimal Price, decimal Discount, decimal Quantity)
        {
            TaxType = TaxType.ToUpper();
            decimal Result;
            switch (TaxType)
            {
                case "I":
                    Result = ((GnxLib.CalAmt(Price, Discount, Quantity) * TaxRate) / (100 + TaxRate));
                    Result = Math.Round(Result, 2);
                    break;
                case "A":
                    Result = ((GnxLib.CalAmt(Price, Discount, Quantity) * TaxRate) / 100);
                    Result = Math.Round(Result, 2);
                    break;
                default:
                    Result = 0;
                    break;
            }
            return Result;
        }

        // คำนวนหา ผลรวมราคาหลังหักภาษี
        public static decimal NetAmt(string TaxType, decimal TaxRate, decimal Price, decimal Discount, decimal Quantity)
        {
            TaxType = TaxType.ToUpper();
            decimal Result;

            if (TaxType == "I")
            {
                Result = GnxLib.CalAmt(Price, Discount, Quantity) -
                         GnxLib.TaxAmt(TaxType, TaxRate, Price, Discount, Quantity);
                Result = Math.Round(Result, 2);
            }
            else
            {
                Result = GnxLib.CalAmt(Price, Discount, Quantity);
                Result = Math.Round(Result, 2);
            }
            return Result;
        }

        // คำนวนหา ผลรวมราคา + ภาษี
        public static decimal Amount(string TaxType, decimal TaxRate, decimal Price, decimal Discount, decimal Quantity)
        {
            TaxType = TaxType.ToUpper();
            decimal Result;
            if (TaxType == "I")
            {
                Result = GnxLib.NetAmt(TaxType, TaxRate, Price, Discount, Quantity) +
                         GnxLib.TaxAmt(TaxType, TaxRate, Price, Discount, Quantity);
                Result = Math.Round(Result, 2);
            }
            else
            {
                //Result = NetAmt(TaxType, TaxRate, Price, Discount, Quantity) + TaxAmt(TaxType, TaxRate, Price, Discount, Quantity);
                Result = GnxLib.CalAmt(Price, Discount, Quantity) + TaxAmt(TaxType, TaxRate, Price, Discount, Quantity);
                Result = Math.Round(Result, 2);
            }

            return Result;
        }

        public static string GetTaxTypeName(string taxType)
        {
            var strName = string.Empty;

            switch (taxType.ToUpper())
            {
                case "A":
                    strName = "Add";
                    break;
                case "I":
                    strName = "Included";
                    break;
                case "N":
                    strName = "None";
                    break;
            }

            return strName;
        }

        /// <summary>
        ///     Evaluate function in C# .Net as Eval() function in Javascript.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static double Evaluate(string expression)
        {
            var table = new System.Data.DataTable();

            table.Columns.Add("expression", string.Empty.GetType(), expression);

            var row = table.NewRow();
            table.Rows.Add(row);

            return double.Parse((string)row["expression"]);
        }

        #endregion

        #region "License"
        public static string GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }

        public static string EncryptProductKey(string painText)
        {
            // return the 25 encrypt characters.
            string productKey = string.Empty;


            return productKey;
        }

        public static string DecryptProductKey(string productKey)
        {
            // return the pain text (original) that is encrypted.
            string painText = string.Empty;

            return painText;
        }



        #endregion

    }
}