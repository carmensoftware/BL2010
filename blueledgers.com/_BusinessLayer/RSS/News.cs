using System.Configuration;
using System.Data;

namespace Blue.BL.RSS
{
    public class News
    {
        #region "Properties"

        private readonly string _blnews = ConfigurationManager.AppSettings["BLNewsRSS"];
        private readonly string _gennews = ConfigurationManager.AppSettings["GnxNewsRSS"];

        #endregion

        #region "Operations"

        public DataTable GetBlNews()
        {
            var dsBlNews = new DataSet();

            // Get xml data from rss feed website.
            dsBlNews.ReadXml(_blnews);

            // return retrieved data in DataTable format.            
            if (dsBlNews != null)
            {
                return dsBlNews.Tables[2];
            }
            return null;
        }

        public DataTable GetGenNews()
        {
            var dsGenNews = new DataSet();

            // Get xml data from rss feed website.
            dsGenNews.ReadXml(_gennews);

            // return retrieved data in DataTable format.            
            if (dsGenNews != null)
            {
                return dsGenNews.Tables[2];
            }
            return null;
        }

        #endregion
    }
}