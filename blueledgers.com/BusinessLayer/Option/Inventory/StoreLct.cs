using System;
using System.Data;
using Blue.DAL;

namespace Blue.BL.Option.Inventory
{
    public class StoreLct : DbHandler
    {
        private ProdLoc ProductLocation = new ProdLoc();

        /// <summary>
        /// </summary>
        public StoreLct()
        {
            SelectCommand = "SELECT * FROM [IN].[StoreLocation]";
            TableName = "StoreLocation";
        }

        /// <summary>
        ///     Get All Acitve Location List
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStoreLct, string connStr)
        {
            //Edit on 02/03/2012 By Tong
            //return DbRetrieve("dbo.StoreLct_GetList", dsStoreLct, null, this.TableName, connStr);
            return DbRetrieve("[IN].[GetAllStoreLocationList]", dsStoreLct, null, TableName, connStr);
        }

        // Added on: 06/11/2017, By Fon
        public bool GetList(DataSet dsStoreLct, string loginName, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", loginName);

            return DbRetrieve("[IN].[GetAllStoreLocationListByLoginName]", dsStoreLct, dbParams, TableName, connStr);
        }
        // End Added.

        //public bool GetList2(DataSet dsStoreLct, string connStr)
        //{
        //    return DbRetrieve("[IN].[GetAllStoreLocationList]", dsStoreLct, null, this.TableName, connStr);
        //}
        /// <summary>
        ///     Get All Active Location List
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        ///     Getlist ProdLoc Min Max
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetListMinMax(DataSet dsStoreLct, string ProductCode, string connStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@ProductCode", ProductCode);

            return DbRetrieve("[IN].[StoreLocation_MinMax]", dsStoreLct, dbParams, TableName, connStr);
        }

        /// <summary>
        ///     Get All Active Location List
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool GetList(DataSet dsStoreLct, ref string MsgError, string connStr)
        {
            //Edit on 02/03/2012 By Tong
            //bool result = DbRetrieve("dbo.StoreLct_GetList", dsStoreLct, null, this.TableName, connStr);
            var result = DbRetrieve("[IN].[GetAllStoreLocationList]", dsStoreLct, null, TableName, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        //public bool GetList2(DataSet dsStoreLct, ref string MsgError, string connStr)
        //{
        //    bool result = DbRetrieve("[IN].[GetAllStoreLocationList]", dsStoreLct, null, this.TableName, connStr);
        //    if (!result)
        //    {
        //        MsgError = "Msg001";
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        ///     Get All Active Location List
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        ///     All Active Location and Location ที่ค้างอยู่จากการทำ pr,po,rec
        ///     และ rec,cn ที่ complete แล้ว(ใช้ในการทำ export sun)
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetUsingLocation(string connStr)
        {
            return DbRead("[IN].[GetAllLocation]", null, connStr);
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Save(DataSet dsStoreLct, ref string MsgError, string connStr)
        {
            var prodLoc = new ProdLoc();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStoreLct, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsStoreLct, prodLoc.SelectCommand, prodLoc.TableName);

            var result = DbCommit(dbSaveSource, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Delete(DataSet dsStoreLct, ref string MsgError, string connStr)
        {
            var prodLoc = new ProdLoc();

            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsStoreLct, prodLoc.SelectCommand, prodLoc.TableName);
            dbSaveSource[1] = new DbSaveSource(dsStoreLct, SelectCommand, TableName);
            var result = DbCommit(dbSaveSource, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="MsgError"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool SaveHeader(DataSet dsStoreLct, ref string MsgError, string connStr)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsStoreLct, SelectCommand, TableName);

            var result = DbCommit(dbSaveSource, connStr);

            if (!result)
            {
                MsgError = "Msg001";
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Get all visibled store for specified LoginName.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string LoginName, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginName", LoginName);

            return DbRead("[IN].[StoreLocation_GetList_LoginName]", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get Lookup Store location.(Active List)
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetList(string connStr)
        {
            //Edit on 02/03/2012 By Tong
            // Create parameters
            //return DbRead("dbo.StoreLct_GetList", null, connStr);
            return DbRead("[IN].[GetAllStoreLocationList]", null, connStr);
        }

        //public DataTable GetList2(string connStr)
        //{
        //    // Create parameters
        //    return DbRead("[IN].[GetAllStoreLocationList]", null, connStr);
        //}
        /// <summary>
        ///     Get Lookup Store location.(Active List)
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        /// <summary>
        /// </summary>
        /// <param name="LocaCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public string GetName(string LocaCode, string connStr)
        {
            var strName = string.Empty;

            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", LocaCode);

            //Edit on 02/03/2012 By Tong
            //DataTable dtName = DbRead("dbo.StoreLct_GetName_LocateCode", dbParams, connStr);
            var dtName = DbRead("[IN].[StoreLct_GetName_LocateCode]", dbParams, connStr);

            if (dtName.Rows.Count > 0)
            {
                strName = dtName.Rows[0]["LocationName"].ToString();
            }
            return strName;
        }

        //public string GetName2(string LocaCode, string connStr)
        //{
        //    string strName = string.Empty;

        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@LocateCode", LocaCode);

        //    DataTable dtName = DbRead("[IN].[StoreLct_GetName_LocateCode]", dbParams, connStr);
        //    if (dtName.Rows.Count > 0)
        //    {
        //        strName = dtName.Rows[0]["LocationName"].ToString();
        //    }
        //    return strName;
        //}  

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string connStr)
        {
            var dsStoreLct = new DataSet();

            // Get Data
            GetList(dsStoreLct, connStr);

            // Return result
            if (dsStoreLct.Tables[TableName] != null)
            {
                var drBlank = dsStoreLct.Tables[TableName].NewRow();
                dsStoreLct.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            // Return result
            return dsStoreLct.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookupForMapping(string connStr)
        {
            var dsStoreLct = new DataSet();

            // Get Data
            GetList(dsStoreLct, connStr);


            // Return result
            return dsStoreLct.Tables[TableName];
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStoreLct"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataSet GetLookup(DataSet dsStoreLct, string connStr)
        {
            // Get Data
            GetList(dsStoreLct, connStr);

            // Return result
            if (dsStoreLct.Tables[TableName] != null)
            {
                var drBlank = dsStoreLct.Tables[TableName].NewRow();
                dsStoreLct.Tables[TableName].Rows.InsertAt(drBlank, 0);
            }

            return dsStoreLct;
        }

        /// <summary>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public object GetLookUp(string p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="dsStore"></param>
        /// <param name="LocationCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public bool Get(DataSet dsStore, string LocationCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", LocationCode);

            //Edit on 02/03/2012 By Tong
            //return DbRetrieve("dbo.StoreLct_Get_LocateCode", dsStore, dbParams, this.TableName, ConnStr);
            return DbRetrieve("[IN].[StoreLct_Get_LocateCode]", dsStore, dbParams, TableName, ConnStr);
        }

        //public bool Get2(DataSet dsStore, string LocationCode, string ConnStr)
        //{
        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@LocateCode", LocationCode);

        //    return DbRetrieve("[IN].[StoreLct_Get_LocateCode]", dsStore, dbParams, this.TableName, ConnStr);
        //}

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetListByRowFilter(string filter, int startIndex, int endIndex, string connStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@filter", filter);
            dbParams[1] = new DbParameter("@startIndex", startIndex.ToString());
            dbParams[2] = new DbParameter("@endIndex", endIndex.ToString());

            //Edit on 02/03/2012 By Tong
            // Create parameters
            //return DbRead("dbo.StoreLct_GetStoreListByRowFilter", dbParams, connStr);
            return DbRead("[IN].[StoreLct_GetStoreListByRowFilter]", dbParams, connStr);
        }

        //public DataTable GetListByRowFilter2(string filter, int startIndex, int endIndex, string connStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[3];
        //    dbParams[0] = new DbParameter("@Filter", filter.ToString());
        //    dbParams[1] = new DbParameter("@StartIndex", startIndex.ToString());
        //    dbParams[2] = new DbParameter("@EndIndex", endIndex.ToString());

        //    // Create parameters
        //    return DbRead("[IN].[StoreLct_GetStoreListByRowFilter]", dbParams, connStr);
        //}

        /// <summary>
        /// </summary>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetActiveProdCate(string ConnStr)
        {
            var dsStore = new DataSet();

            //Edit on 02/03/2012 By Tong
            //bool result = DbRetrieve("dbo.IN_StoreLct_GetActiveProdCate", dsStore, null, this.TableName, ConnStr);
            var result = DbRetrieve("[IN].[StoreLct_GetActiveProdCate] ", dsStore, null, TableName, ConnStr);

            if (result)
            {
                dsStore.Tables[TableName].PrimaryKey = GetPK(dsStore);
            }

            return dsStore.Tables[TableName];
        }

        //public DataTable GetActiveProdCate2(string ConnStr)
        //{
        //    DataSet dsStore = new DataSet();

        //    bool result = DbRetrieve("[IN].[StoreLct_GetActiveProdCate] ", dsStore, null, this.TableName, ConnStr);

        //    if (result)
        //    {
        //        dsStore.Tables[this.TableName].PrimaryKey = GetPK(dsStore);
        //    }

        //    return dsStore.Tables[this.TableName];
        //}

        /// <summary>
        ///     Set Primary Key.
        /// </summary>
        /// <param name="dsPo"></param>
        /// <returns></returns>
        private DataColumn[] GetPK(DataSet dsPo)
        {
            var primaryKeys = new DataColumn[1];
            primaryKeys[0] = dsPo.Tables[TableName].Columns["LocationCode"];

            return primaryKeys;
        }

        /// <summary>
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int StrLct_GetNewStoreCode(string connStr)
        {
            //Edit on 02/03/2012 By Tong
            //int NewCode = DbReadScalar("dbo.StrLct_GeNewStoreCode", null, connStr);
            var NewCode = DbReadScalar("[IN].[StrLct_GetNewStoreCode]", null, connStr);

            // Return result
            return NewCode;
        }

        //public int StrLct_GeNewStoreCode2(string connStr)
        //{
        //    int NewCode = DbReadScalar("[IN].[StrLct_GeNewStoreCode]", null, connStr);

        //    // Return result
        //    return NewCode;
        //}

        /// <summary>
        /// </summary>
        /// <param name="DeliveryPoint"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int Get_CountByDeliveryPoint(string DeliveryPoint, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@DeliveryPoint", DeliveryPoint);

            //Edit on 02/03/2012 By Tong
            //return DbReadScalar("dbo.StoreLct_Get_CountByDeliveryPoint", dbParams, ConnStr);
            return DbReadScalar("[IN].[StoreLct_Get_CountByDeliveryPoint]", dbParams, ConnStr);
        }

        //public int Get_CountByDeliveryPoint2(string DeliveryPoint, string ConnStr)
        //{
        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@DeliveryPoint", DeliveryPoint);

        //    return DbReadScalar("[IN].[StoreLct_Get_CountByDeliveryPoint]", dbParams, ConnStr);
        //}

        /// <summary>
        /// </summary>
        /// <param name="LocateCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int StoreLctCountByLocationCode(string LocateCode, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocateCode", LocateCode);

            //Edit on 02/03/2012 By Tong
            //return DbReadScalar("dbo.StoreLctCountByLocationCode", dbParams, ConnStr);
            return DbReadScalar("[IN].[StoreLctCountByLocationCode]", dbParams, ConnStr);
        }

        //public int StoreLctCountByLocationCode2(string LocateCode, string ConnStr)
        //{
        //    // Create parameters
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@LocateCode", LocateCode);

        //    return DbReadScalar("[IN].[StoreLctCountByLocationCode]", dbParams, ConnStr);
        //}

        /// <summary>
        ///     Get Delivery Point code by Location code.
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetDeliveryPoint(string LocationCode, string ConnStr)
        {
            var dsStoreLocation = new DataSet();

            var result = Get(dsStoreLocation, LocationCode, ConnStr);

            if (result)
            {
                if (dsStoreLocation.Tables[TableName].Rows.Count > 0)
                {
                    if (dsStoreLocation.Tables[TableName].Rows[0]["DeliveryPoint"] != DBNull.Value)
                    {
                        return int.Parse(dsStoreLocation.Tables[TableName].Rows[0]["DeliveryPoint"].ToString());
                    }
                }
            }

            return 0;
        }

        public bool GetList(DataSet dsStoreLocation, int EOP, string LoginName, DateTime EndDate, string ConnStr)
        {
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@EOP", EOP.ToString());
            dbParams[1] = new DbParameter("@LoginName", LoginName);
            dbParams[2] = new DbParameter("@EndDate", EndDate.ToString("yyyy-MM-dd"));

            return DbRetrieve("[IN].[GetStoreLocationListByEOPLoginNameEndDate]", dsStoreLocation, dbParams, TableName,
                ConnStr);
        }

        public DataTable GetList(int EOP, string LoginName, DateTime EndDate, string ConnStr)
        {
            var dsStoreLocation = new DataSet();

            var result = GetList(dsStoreLocation, EOP, LoginName, EndDate, ConnStr);

            if (result)
            {
                return dsStoreLocation.Tables[TableName];
            }

            return null;
        }

        public bool GetList(DataSet dsStoreLocation, int EOP, string LoginName, string ConnStr)
        {
            //var dbParams = new DbParameter[2];
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@EOP", EOP.ToString());
            dbParams[1] = new DbParameter("@LoginName", LoginName);

            // Added on: 25/09/2017. By: Fon
            // Because they must have 3th param.
            dbParams[2] = new DbParameter("@EndDate", DBNull.Value.ToString());
            // End Added.

            return DbRetrieve("[IN].[GetStoreLocationListByEOPLoginName]", dsStoreLocation, dbParams, TableName, ConnStr);
        }

        public DataTable GetList(int EOP, string LoginName, string ConnStr)
        {
            var dsStoreLocation = new DataSet();

            var result = GetList(dsStoreLocation, EOP, LoginName, ConnStr);

            if (result)
            {
                return dsStoreLocation.Tables[TableName];
            }

            return null;
        }

        public DataTable GetLookUp_ByCategoryType(string CategoryType, string LoginName, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[2];
            dbParams[0] = new DbParameter("@CategoryType", CategoryType);
            dbParams[1] = new DbParameter("@LoginName", LoginName);

            // Create parameters
            var dtLookUp = DbRead("[IN].[Location_GetByCategoryType]", dbParams, connStr);

            return dtLookUp;
        }

        /// <summary>
        ///     Get Location List that not include one location
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public DataTable GetLookup(string LocationCode, string connStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LocationCode", LocationCode);
            return DbRead("[IN].[GetStoreLocationList]", dbParams, connStr);
        }

        //public DataTable GetLookupByMovementType(string locationCode, string adjID, string connStr)
        public DataTable GetLookupByMovementType(string locationCode, string adjID, string loginName, string connStr)
        {
            // Create parameters
            if (adjID == "")
                adjID = null;

            // Modified on: 03/11/2017, By: Fon
            var dbParams = new DbParameter[3];
            dbParams[0] = new DbParameter("@LocationCode", locationCode);
            dbParams[1] = new DbParameter("@AdjID", adjID);

            dbParams[2] = new DbParameter("@LoginName", loginName);

            //return DbRead("[IN].[GetStoreLocationList]", dbParams, connStr);
            return DbRead("[IN].[GetStoreLocationListByLoginName]", dbParams, connStr);
        }

        /// <summary>
        /// Delete data in table [ADMIN].UserStore and [IN].ProdLoc
        /// </summary>
        /// <param name="LocationCode"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        //public bool DeleteRelation(string LocationCode, string ConnStr)
        //{
        //    DbParameter[] dbParams = new DbParameter[1];
        //    dbParams[0] = new DbParameter("@LocationCode", LocationCode);

        //    DbExecuteQuery("EXEC [IN].[DeleteLocationRelation] @LocationCode", dbParams, ConnStr);

        //    return true;
        //}
    }
}