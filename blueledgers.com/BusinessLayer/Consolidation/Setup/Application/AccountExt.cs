using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.Consolidation.Setup.Application
{
    public class AccountExt : DbHandler
    {
        /// <summary>
        ///     Empty constructor
        /// </summary>
        public AccountExt()
        {
            SelectCommand = "SELECT * FROM [Application].AccountExt";
            TableName = "AccountExt";
        }

    }
}