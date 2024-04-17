using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.APP
{
    public class Lookup : DbHandler
    {
        #region "Attributes"

        private readonly LookupItem _lookupItem = new LookupItem();

        #endregion

        #region "Operations"

        /// <summary>
        ///     Empty constructor
        /// </summary>
        public Lookup()
        {
            SelectCommand = "SELECT * FROM APP.[Lookup]";
            TableName = "Lookup";
        }

        /// <summary>
        ///     Get all actived lookup item related to specified lookup id.
        /// </summary>
        /// <param name="strLooupID"></param>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public DataTable GetItemList(string strLooupID, string strConn)
        {
            var dsLookupItem = new DataSet();

            var result = _lookupItem.GetList(dsLookupItem, strLooupID, strConn);

            if (result)
            {
                return dsLookupItem.Tables[_lookupItem.TableName];
            }
            return null;
        }

        #endregion
    }
}