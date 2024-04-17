using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.IN
{
    public class Inventory : DbHandler
    {
        public Inventory()
        {
            SelectCommand = "SELECT * FROM [IN].[Inventory]";
            TableName = "Inventory";
        }

        /// <summary>
        /// </summary>
        /// <param name="dsInv"></param>
        /// <param name="MsgError"></param>
        /// <param name="hdrNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListByHdrNo(DataSet dsInv, ref string MsgError, string hdrNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@HdrNo", hdrNo);

            var result = DbRetrieve("dbo.IN_Inventory_Get_HdrNo", dsInv, dbParams, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get Data output to dataset
        /// </summary>
        /// <param name="dsRec"></param>
        /// <param name="recNo"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetListByHdrNo(DataSet dsRec, string recNo, string connStr)
        {
            var MsgError = string.Empty;

            var result = GetListByHdrNo(dsRec, ref MsgError, recNo, connStr);

            if (result)
            {
                return dsRec;
            }
            return null;
        }


        public DataTable GetListByHdrNo_DtNo(string hdrNo, string dtNo, string connStr)
        {
            string sql = string.Format("SELECT * FROM [IN].Inventory WHERE HdrNo = '{0}' AND DtNo={1} ORDER BY InvNo", hdrNo, dtNo );
            return DbExecuteQuery(sql, null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsInv"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetStructure(DataSet dsInv, string ConnStr)
        {
            return DbRetrieve("dbo.IN_Inventory_GetSchema", dsInv, null, TableName, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetMaxInvNo(string ConnStr)
        {
            return DbReadScalar("dbo.IN_Inventory_GetMaxNo", null, ConnStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="strProductCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public decimal GetMAvgAudit(string strProductCode, string connStr)
        {
            decimal decPrice = 0;
            var dtGet = new DataTable();

            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", strProductCode);

            dtGet = DbRead("dbo.IN_Inventory_GetMAvgAudit_ProductCode", dbParams, connStr);

            if (dtGet.Rows.Count > 0)
            {
                decPrice = decimal.Parse(dtGet.Rows[0]["MAvgAudit"].ToString());
            }

            return decPrice;
        }

        /// <summary>
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="RecQty"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public decimal ChangeOrderUnitToInventoryUnit(string ProductCode, string Qty, string ConnStr)
        {
            decimal Unit = 0;
            var dtGet = new DataTable();

            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@Qty", Qty);

            dtGet = DbRead("[IN].[ChangeOrderUnitToInventoryUnit]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0)
            {
                Unit = decimal.Parse(dtGet.Rows[0]["InvQty"].ToString());
            }

            return Unit;
        }

        /// <summary>
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="LocationCode"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool SetPAvgAudit(string StartDate, string EndDate, string LocationCode, string ProductCode,
            string ConnStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@strStartDate", StartDate); //StartDate.ToString("yyyy-MM-dd")
            dbParams[1] = new DbParameter("@strEndDate", EndDate); //EndDate.ToString("yyyy-MM-dd")
            dbParams[2] = new DbParameter("@LocationCode", LocationCode);
            dbParams[3] = new DbParameter("@ProductCode", ProductCode);

            DbExecuteQuery("EXEC [IN].[SetInventoryPAvgAudit] @strStartDate,@strEndDate,@LocationCode,@ProductCode",
                dbParams, ConnStr);

            return true;
        }

        public void UpdateAverageCostByDocument(string docNo, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DocNo", docNo);

            DbExecuteQuery("EXEC [IN].[UpdateAverageCost] @DocNo", dbParams, connStr);
        }

        public bool GetStockMovement(DataSet dsInventory, string HdrNo, string DtNo, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@HdrNo", HdrNo);
            dbParams[1] = new DbParameter("@DtNo", DtNo);

            return DbRetrieve("[IN].[GetStockMovementByHdrNoAndDtNo]", dsInventory, dbParams, TableName, ConnStr);
        }

        public decimal GetPAvgAudit(string StartDate, string EndDate, string LocationCode, string ProductCode,
            string ConnStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@strStartDate", StartDate);
            dbParams[1] = new DbParameter("@strEndDate", EndDate);
            dbParams[2] = new DbParameter("@LocationCode", LocationCode);
            dbParams[3] = new DbParameter("@ProductCode", ProductCode);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetInvpAvgAudit]", dbParams, ConnStr);
            //dtGet = DbRead("[IN].[GetInvPAvgAudit]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        public decimal SetPAvgAuditByCommitDate(string CommitDate, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@CommitDate", DateTime.Parse(CommitDate).ToString("yyyy-MM-dd"));
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetPAvgAuditByCommitDate]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        public decimal GetLastAvg(string LocationCode, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            dbParams[1] = new DbParameter("@ProductCode", ProductCode);

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetLastAvg]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        public decimal GetAverageCost(string ProductCode, DateTime toDate, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@ToDate", toDate.ToString("yyyy-MM-dd"));

            var dtGet = new DataTable();
            dtGet = DbRead("[IN].[GetAverageCost]", dbParams, ConnStr);

            if (dtGet.Rows.Count > 0 && dtGet.Rows[0][0] != DBNull.Value)
            {
                return decimal.Parse(dtGet.Rows[0][0].ToString());
            }

            return 0;
        }

        public bool InsertConsumption(int EOPId, int EOPDtId, string LocationCode, string ProductCode,
            DateTime StartDate, DateTime EndDate,
            Decimal ClosingQty, DateTime CommittedDate, string ConnStr)
        {
            var dbParams = new DbParameter[8];
            dbParams[0] = new DbParameter("@EOPId", EOPId.ToString());
            dbParams[1] = new DbParameter("@EOPDtId", EOPDtId.ToString());
            dbParams[2] = new DbParameter("@LocationCode", LocationCode);
            dbParams[3] = new DbParameter("@ProductCode", ProductCode);
            dbParams[4] = new DbParameter("@StartDate", StartDate.ToString("yyyy-MM-dd"));
            dbParams[5] = new DbParameter("@EndDate", EndDate.ToString("yyyy-MM-dd"));
            dbParams[6] = new DbParameter("@ClosingQty", ClosingQty.ToString());
            dbParams[7] = new DbParameter("@CommittedDate", CommittedDate.ToString("yyyy-MM-dd"));

            DbExecuteQuery(
                "EXEC [IN].[InsertConsumption] @EOPId, @EOPDtId, @LocationCode, @ProductCode, @StartDate, @EndDate, @ClosingQty, @CommittedDate",
                dbParams, ConnStr);

            return true;
        }

        /// <summary>
        ///     Get Product List for Product Movement
        /// </summary>
        /// <param name="dsInventory"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="Location"></param>
        /// <param name="Category"></param>
        /// <param name="ProductDesc"></param>
        /// <param name="IsAllProduct"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInventory, DateTime DateFrom, DateTime DateTo, string Location, string Category,
            string ProductDesc, bool IsAllProduct, string ConnStr)
        {
            var dbParams = new DbParameter[6];
            dbParams[0] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[1] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[2] = new DbParameter("@Location", Location);
            dbParams[3] = new DbParameter("@ProductCate", Category);
            dbParams[4] = new DbParameter("@ProductDesc", ProductDesc);
            dbParams[5] = new DbParameter("@AllProduct", IsAllProduct.ToString());

            return DbRetrieve("[IN].GetInventoty_CommittedDate_Location_ProductCate_ProductDesc", dsInventory, dbParams,
                TableName, ConnStr);
        }

        /// <summary>
        ///     Get Bwf data for Product Movement
        /// </summary>
        /// <param name="dsInventory"></param>
        /// <param name="DateFrom"></param>
        /// <param name="Location"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInventory, DateTime DateFrom, string Location, string ProductCode, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@CommittedDate", DateFrom.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[1] = new DbParameter("@Location", Location);
            dbParams[2] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].GetInventoryMovementBwf_CommittedDate_Location_ProductCode", dsInventory, dbParams,
                TableName, ConnStr);
        }

        /// <summary>
        ///     Get Current data for Product Movement
        /// </summary>
        /// <param name="dsInventory"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="Location"></param>
        /// <param name="ProductCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsInventory, DateTime DateFrom, DateTime DateTo, string Location, string ProductCode,
            string ConnStr)
        {
            var dbParams = new DbParameter[4];
            dbParams[0] = new DbParameter("@DateFrom", DateFrom.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[1] = new DbParameter("@DateTo", DateTo.ToString("yyyy-MM-dd HH:mm:ss"));
            dbParams[2] = new DbParameter("@Location", Location);
            dbParams[3] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].GetInventoryMovement_CommittedDate_Location_ProductCode", dsInventory, dbParams,
                TableName, ConnStr);
        }

        /// <summary>
        ///     Save data to database
        /// </summary>
        /// <param name="dsSaving"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public bool Save(DataSet dsSaving, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];

            dbSaveSource[0] = new DbSaveSource(dsSaving, SelectCommand, TableName);

            // Call function dbCommit for commit data to database
            return DbCommit(dbSaveSource, strConn);
        }

        public void Delete(string hdrNo, string strConn)
        {

            // Call function dbCommit for commit data to database
            string cmd = "DELETE FROM [IN].Inventory WHERE HdrNo = '" + hdrNo + "'";
            DbExecuteQuery(cmd, null, strConn);
        }


        /// <summary>
        ///     Get Product Onhand quantity by location.
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="LocationCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public decimal GetOnHand(string ProductCode, string LocationCode, string ConnStr)
        {
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);
            dbParams[1] = new DbParameter("@Location", LocationCode);

            var dtInv = DbRead("[IN].GetInventoryOnHand_ProductCode_Location", dbParams, ConnStr);

            if (dtInv != null)
            {
                if (dtInv.Rows.Count > 0)
                {
                    if (dtInv.Rows[0]["OnHand"] != DBNull.Value)
                    {
                        return Convert.ToDecimal(dtInv.Rows[0]["OnHand"]);
                    }
                }
            }

            return 0;
        }

        /// <summary>
        ///     Sub Class for Fifo
        /// </summary>
        public class Fifo : DbHandler
        {
            /// <summary>
            ///     FIFO initial class
            /// </summary>
            public Fifo()
            {
                //this.SelectCommand = "SELECT * FROM [IN].[FIFO]";
                TableName = "[FIFO]";
                TableNameCN = "[CN]";
            }

            public string TableNameCN { get; set; }

            public bool SaveSO(string DocNo, string DocDtNo, string LocationCode, string ProductCode, Decimal Qty,
                string ConnStr)
            {
                var dbParams = new DbParameter[5];
                dbParams[0] = new DbParameter("@DocNo", DocNo);
                dbParams[1] = new DbParameter("@DocDtNo", DocDtNo);
                dbParams[2] = new DbParameter("@LocationCode", LocationCode);
                dbParams[3] = new DbParameter("@ProductCode", ProductCode);
                dbParams[4] = new DbParameter("@QtyOut", Qty.ToString());

                DbExecuteQuery("EXEC [IN].[InsertInventorySO] @DocNo,@DocDtNo,@LocationCode,@ProductCode,@QtyOut",
                    dbParams, ConnStr);

                return true;
            }


            /// <summary>
            ///     Get structure
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetStructure(DataSet dsFifo, string connStr)
            {
                var dbParams = new DbParameter[2];
                dbParams[0] = new DbParameter("@Location", string.Empty);
                dbParams[1] = new DbParameter("@ProductCode", string.Empty);

                return DbRetrieve("[in].[GetListFIFO]", dsFifo, dbParams, TableName, connStr);
            }

            /// <summary>
            ///     List of aviable que order by createdate
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetList(DataSet dsFifo, string connStr)
            {
                return GetList(dsFifo, null, null, connStr);
            }

            /// <summary>
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="locationCode"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetList(DataSet dsFifo, string locationCode, string connStr)
            {
                return GetList(dsFifo, locationCode, null, connStr);
            }

            /// <summary>
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="locationCode"></param>
            /// <param name="productCode"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetList(DataSet dsFifo, string locationCode, string productCode, string connStr)
            {
                var dbParams = new DbParameter[2];
                dbParams[0] = new DbParameter("@Location", locationCode);
                dbParams[1] = new DbParameter("@ProductCode", productCode);
                try
                {
                    var dt = DbRead("[in].[GetListFIFO]", dbParams, connStr);
                    dt.DefaultView.RowFilter = "Remain > 0";
                    var dtt = dt.DefaultView.ToTable(TableName);

                    var t = dsFifo.Tables.IndexOf(TableName);
                    if (t > 0)
                    {
                        dsFifo.Tables.Remove(TableName);
                    }
                    dsFifo.Tables.Add(dtt);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            ///     Get list cn by createdate
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="hdrNo"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetCN(DataSet dsFifo, string hdrNo, string connStr)
            {
                var dbParams = new DbParameter[2];
                dbParams[0] = new DbParameter("@Location", null);
                dbParams[1] = new DbParameter("@ProductCode", null);
                try
                {
                    var dt = DbRead("[in].[GetListFIFO]", dbParams, connStr);
                    dt.DefaultView.RowFilter = "HdrNo = '" + hdrNo + "' AND ISNULL(CnNo,'') <> '' ";
                    var dtt = dt.DefaultView.ToTable(TableNameCN);

                    var t = dsFifo.Tables.IndexOf(TableNameCN);
                    if (t > 0)
                    {
                        dsFifo.Tables.Remove(TableNameCN);
                    }
                    dsFifo.Tables.Add(dtt);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetListCN(DataSet dsFifo, string connStr)
            {
                return GetListCN(dsFifo, null, null, connStr);
            }

            /// <summary>
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="location"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetListCN(DataSet dsFifo, string location, string connStr)
            {
                return GetListCN(dsFifo, location, null, connStr);
            }

            /// <summary>
            /// </summary>
            /// <param name="dsFifo"></param>
            /// <param name="location"></param>
            /// <param name="productCode"></param>
            /// <param name="connStr"></param>
            /// <returns></returns>
            public bool GetListCN(DataSet dsFifo, string location, string productCode, string connStr)
            {
                var dbParams = new DbParameter[2];
                dbParams[0] = new DbParameter("@Location", location);
                dbParams[1] = new DbParameter("@ProductCode", productCode);
                try
                {
                    var dt = DbRead("[in].[GetListFIFO]", dbParams, connStr);
                    dt.DefaultView.RowFilter = "Remain > 0 AND ISNULL(CnNo,'') <> '' ";
                    var dtt = dt.DefaultView.ToTable("CN");

                    var t = dsFifo.Tables.IndexOf(TableNameCN);
                    if (t > 0)
                    {
                        dsFifo.Tables.Remove(TableNameCN);
                    }
                    dsFifo.Tables.Add(dtt);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}