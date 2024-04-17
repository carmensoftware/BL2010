using System.Data;

// ReSharper disable once CheckNamespace
namespace Blue.DAL
{
    public class DbSaveSource
    {
        #region "Attributies"

        public string SelectCommand { get; set; }
        public string TableName { get; set; }
        public DataSet SavedData { get; set; }

        public DbSaveSource()
        {
        }

        public DbSaveSource(DataSet savedData, string selectCmd, string tableName)
        {
            SavedData = savedData;
            SelectCommand = selectCmd;
            TableName = tableName;
        }

        #endregion
    }
}