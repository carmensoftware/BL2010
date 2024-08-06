using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

/// <summary>
/// Utility functions to help easy work.
/// </summary>
public class Helpers
{
    public class SQL
    {
        private string _connStr;

        public SQL()
        {
        }

        public SQL(string connectionString)
        {
            _connStr = connectionString;
        }

        public string ConnectionString { set { _connStr = value; } }


        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null, string connectionString = null)
        {
            try
            {
                connectionString = string.IsNullOrEmpty(connectionString) ? _connStr : connectionString;

                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (var p in parameters)
                            {
                                da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                            }
                        }

                        var dt = new DataTable();
                        da.Fill(dt);

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet ExecuteQueries(string query, SqlParameter[] parameters = null, string connectionString = null)
        {
            try
            {
                connectionString = string.IsNullOrEmpty(connectionString) ? _connStr : connectionString;

                using (var conn = new SqlConnection(connectionString))
                {
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (var p in parameters)
                            {
                                da.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                            }
                        }

                        var ds = new DataSet();

                        da.Fill(ds);

                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }

    public class TextUtils
    {
        public int GetCompareScore(string source, string target)
        {
            //var sources = source.Split(' ').Select(x => RemoveNonAlphaNumeric(x.Trim().ToLower())).Where(x => x != "").ToArray();
            //var targets = target.Split(' ').Select(x => RemoveNonAlphaNumeric(x.Trim().ToLower())).Where(x => x != "").ToArray();
            var sources = source.Split(' ').Select(x => x.Trim().ToLower()).Where(x => x != "").ToArray();
            var targets = target.Split(' ').Select(x => x.Trim().ToLower()).Where(x => x != "").ToArray();

            var score = 0;

            foreach (var s in sources)
            {
                if (targets.Contains(s))
                    score += 50;

                foreach (var t in targets)
                {
                    var len = t.Length < s.Length ? t.Length : s.Length;

                    var count = 0;

                    for (int i = 0; i < len; i++)
                    {
                        if (s[i] == t[i])
                            count++;
                    }

                    score += (count == len ? count : 0);
                }
            }

            if (Enumerable.SequenceEqual(sources, targets))
                score += 1000;
            else if (string.Join("", sources) == string.Join("", targets))
                score += 500;

            return score;
        }

        private string RemoveNonAlphaNumeric(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }

    public static class Config
    {
        public static bool IsEnableEditCommit(DateTime docDate, string connectionString)
        {
            var sql = new SQL(connectionString);
            var query = string.Format("SELECT TOP 1 StartDate FROM [IN].Period WHERE IsClose=0 AND '{0}' >= StartDate", docDate.ToString("yyyy-MM-dd"));
            var dt = new DataTable();
            
            dt = sql.ExecuteQuery(query);
            var isOpenPeriod = dt != null && dt.Rows.Count > 0;

            dt = sql.ExecuteQuery("SELECT * FROM APP.Config WHERE Module='APP' AND SubModule='SYS' AND [Key]='EnableEditCommit' AND [Value]='1'"); 
            var enableEditCommit = dt != null && dt.Rows.Count > 0;

            dt = sql.ExecuteQuery("SELECT * FROM APP.Config WHERE Module='IN' AND SubModule='SYS' AND [Key]='COST' AND [Value]='AVCO'");
            var isAverage = dt != null && dt.Rows.Count > 0;

            return isOpenPeriod && enableEditCommit && isAverage;
        }


    }

}