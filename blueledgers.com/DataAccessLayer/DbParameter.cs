// ReSharper disable once CheckNamespace
namespace Blue.DAL
{
    public class DbParameter
    {
        #region "Attributies"

        public string ParameterName { get; set; }

        public string ParameterValue { get; set; }

        #endregion

        #region "Operations"

        public DbParameter()
        {
        }

        public DbParameter(string parameterName, string parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        #endregion
    }
}