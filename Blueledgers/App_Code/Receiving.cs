using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlueLedger.PL.BaseClass;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Receiving functions
/// </summary>
namespace BlueLedger.PL.IN.REC
{
    public partial class RecFunc : BasePage
    {

        #region "Attributes"


        private readonly Blue.BL.APP.Config config = new Blue.BL.APP.Config();
        private readonly Blue.BL.IN.Inventory inv = new Blue.BL.IN.Inventory();
        private readonly Blue.BL.PC.REC.REC rec = new Blue.BL.PC.REC.REC();
        private readonly Blue.BL.PC.REC.RECDt recDt = new Blue.BL.PC.REC.RECDt();

        static string _connectionString = string.Empty;

        #endregion // Atttributes

        #region "Methods"

        public RecFunc()
        {
        }

        public RecFunc(string connString)
        {
            _connectionString = connString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void UpdateInventoryForCommit(DataSet dsSave, DataRow drRecDt)
        {

            string costMethod = config.GetValue("IN", "SYS", "COST", _connectionString);


            if (costMethod == "AVCO") // Average Cost
            {
                var drInv = dsSave.Tables[inv.TableName].NewRow();

                string productCode = drRecDt["ProductCode"].ToString();

                decimal currRate = Convert.ToDecimal(drRecDt["Rate"].ToString()) == 0 ? Convert.ToDecimal("1") : Convert.ToDecimal(drRecDt["Rate"].ToString());
                decimal inQty = (Convert.ToDecimal(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString())) * currRate;
                DateTime atDate = dsSave.Tables[rec.TableName].Rows[0].Field<DateTime>("CommitDate");

                drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
                drInv["InvNo"] = 1;
                drInv["ProductCode"] = productCode;
                drInv["Location"] = drRecDt["LocationCode"].ToString();
                drInv["IN"] = inQty;
                drInv["OUT"] = Convert.ToDecimal("0.00");
                drInv["Amount"] = 0;
                drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                drInv["Type"] = "RC";

                decimal extraCost = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
                drInv["PriceOnLots"] = Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCost;  //+ Convert.ToDecimal(drRecDt["ExtraCost"]);  // Keep total net amount for calculating average cost

                //drInv["FIFOAudit"] = System.DBNull.Value;
                //drInv["FIFOMng"] = System.DBNull.Value;
                //drInv["FIFOBank"] = System.DBNull.Value;
                //drInv["MAvgAudit"] = System.DBNull.Value;
                //drInv["MAvgMng"] = System.DBNull.Value;
                //drInv["MAvgBank"] = System.DBNull.Value;
                //drInv["PAvgAudit"] = System.DBNull.Value;
                //drInv["PAvgMng"] = System.DBNull.Value;
                //drInv["PAvgBank"] = System.DBNull.Value;
                //drInv["RptAudit"] = System.DBNull.Value;
                //drInv["RptMng"] = System.DBNull.Value;
                //drInv["RptBank"] = System.DBNull.Value;

                dsSave.Tables[inv.TableName].Rows.Add(drInv);

            }
            else  //FIFO Calculation
            {

                var TotalRecvQty = decimal.Parse(drRecDt["RecQty"].ToString()) + decimal.Parse(drRecDt["FOCQty"].ToString());

                var invQty = TotalRecvQty * Convert.ToDecimal(drRecDt["Rate"].ToString());
                decimal extraCostFifo = string.IsNullOrEmpty(drRecDt["ExtraCost"].ToString()) ? 0 : Convert.ToDecimal(drRecDt["ExtraCost"]);
                var netPrice = Math.Round(Convert.ToDecimal(drRecDt["NetAmt"].ToString()) + extraCostFifo, 2);
                var pricePerQty = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
                var FifoCost = Math.Round((netPrice / (invQty == 0 ? 1 : invQty)), 2);
                var diffCount = Math.Round((netPrice - (pricePerQty * (invQty == 0 ? 1 : invQty))), 2) * 100;

                if (diffCount == 0)
                {
                    var drInv = dsSave.Tables[inv.TableName].NewRow();

                    drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                    drInv["DtNo"] = drRecDt["RecDtNo"].ToString();
                    drInv["InvNo"] = 1;
                    drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
                    drInv["Location"] = drRecDt["LocationCode"].ToString();
                    drInv["IN"] = invQty;
                    drInv["OUT"] = Convert.ToDecimal("0.00");
                    drInv["Amount"] = pricePerQty;
                    drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                    drInv["Type"] = "RC";

                    //drInv["PriceOnLots"] = dsSave.Tables[recDt.TableName].Rows[0]["NetAmt"]; // Keep NetAmt
                    drInv["PriceOnLots"] = netPrice; // Keep NetAmt

                    //drInv["FIFOAudit"] = System.DBNull.Value;
                    //drInv["FIFOMng"] = System.DBNull.Value;
                    //drInv["FIFOBank"] = System.DBNull.Value;
                    //drInv["MAvgAudit"] = System.DBNull.Value;
                    //drInv["MAvgMng"] = System.DBNull.Value;
                    //drInv["MAvgBank"] = System.DBNull.Value;
                    //drInv["PAvgAudit"] = System.DBNull.Value;
                    //drInv["PAvgMng"] = System.DBNull.Value;
                    //drInv["PAvgBank"] = System.DBNull.Value;
                    //drInv["RptAudit"] = System.DBNull.Value;
                    //drInv["RptMng"] = System.DBNull.Value;
                    //drInv["RptBank"] = System.DBNull.Value;

                    dsSave.Tables[inv.TableName].Rows.Add(drInv);


                }
                else
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var drInv = dsSave.Tables[inv.TableName].NewRow();

                        drInv["HdrNo"] = drRecDt["RecNo"].ToString();
                        drInv["DtNo"] = drRecDt["RecDtNo"];
                        drInv["InvNo"] = i + 1;
                        drInv["ProductCode"] = drRecDt["ProductCode"].ToString();
                        drInv["Location"] = drRecDt["LocationCode"].ToString();

                        if (i == 0)
                        {
                            drInv["IN"] = invQty - Math.Abs(diffCount);
                            //drInv["FIFOAudit"] = FifoCost;
                            drInv["Amount"] = pricePerQty;
                        }
                        else
                        {
                            drInv["IN"] = Math.Abs(diffCount);

                            if (diffCount < 0)
                            {
                                //case : QuantityPerUnit * Quantity > Net
                                drInv["Amount"] = pricePerQty - Convert.ToDecimal("0.01");
                                drInv["FIFOAudit"] = FifoCost - Convert.ToDecimal("0.01");
                            }
                            else
                            {
                                //case : QuantityPerUnit * Quantity < Net
                                drInv["Amount"] = pricePerQty + Convert.ToDecimal("0.01");
                                //drInv["FIFOAudit"] = FifoCost + Convert.ToDecimal("0.01");
                            }
                        }

                        drInv["OUT"] = 0;
                        drInv["CommittedDate"] = dsSave.Tables[rec.TableName].Rows[0]["CommitDate"];
                        drInv["Type"] = "RC";
                        drInv["PriceOnLots"] = Math.Round(pricePerQty * Convert.ToDecimal(drInv["IN"].ToString()), 2); // Keep NetAmt

                        //drInv["FIFOMng"] = System.DBNull.Value;
                        //drInv["FIFOBank"] = System.DBNull.Value;
                        //drInv["MAvgAudit"] = System.DBNull.Value;
                        //drInv["MAvgMng"] = System.DBNull.Value;
                        //drInv["MAvgBank"] = System.DBNull.Value;
                        //drInv["PAvgAudit"] = 0;
                        //drInv["PAvgMng"] = System.DBNull.Value;
                        //drInv["PAvgBank"] = System.DBNull.Value;
                        //drInv["RptAudit"] = System.DBNull.Value;
                        //drInv["RptMng"] = System.DBNull.Value;
                        //drInv["RptBank"] = System.DBNull.Value;

                        dsSave.Tables[inv.TableName].Rows.Add(drInv);

                    }
                }
            }  //else (FIFO)
        }


        #endregion // Methods
    }
}