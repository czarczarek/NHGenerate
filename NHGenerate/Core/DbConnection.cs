using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NHGenerate.Properties;

namespace NHGenerate.Core
{
    public class DbConnection
    {
        const string Pattern = @"(?<var>\w+)\=(?<val>[\w\.\\]+)";

        public DbConnection()
        {

        }

        public DbConnection(string encoded)
        {
            string value = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

            var regex = new Regex(Pattern);
            if (regex.IsMatch(value))
            {
                var matches = regex.Matches(value);
                foreach (Match m in matches)
                {
                    switch (m.Groups["var"].Value)
                    {
                        case "d": DefaultName = m.Groups["val"].Value; break;
                        case "s": Server = m.Groups["val"].Value; break;
                        case "a": WindowsAuthentication = (m.Groups["val"].Value == "1"); break;
                        case "u": UserName = m.Groups["val"].Value; break;
                        case "p": Password = m.Groups["val"].Value; break;
                        case "db": Database = m.Groups["val"].Value; break;
                    }
                }
            }
        }

        public string DefaultName { get; set; }

        public string Server { get; set; }

        public bool WindowsAuthentication { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public int ConnectionTimeOut = 5;

        public string Encoded
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("d={0};", DefaultName);
                sb.AppendFormat("s={0};", Server);
                sb.AppendFormat("a={0};", WindowsAuthentication ? 1 : 0);
                if (!string.IsNullOrEmpty(UserName)) sb.AppendFormat("u={0};", UserName);
                if (!string.IsNullOrEmpty(Password)) sb.AppendFormat("p={0};", Password);
                sb.AppendFormat("db={0};", Database);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString())); ;
            }
        }

        public string ConnectionString
        {
            get
            {
                string connectionString = WindowsAuthentication ?
                        string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connection Timeout={2}", Server, Database, ConnectionTimeOut) :
                    string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Connection Timeout={4}", Server, Database, UserName, Password, ConnectionTimeOut);
                return connectionString;
            }
        }

        public static bool TestConnection(string defaultName, out DbConnection connection)
        {
            bool connected = false;
            StringCollection list = Settings.Default.Connections;
            connection = null;

            if (list != null)
            {
                foreach (string val in list.Cast<string>().Where(val => !string.IsNullOrEmpty(val)))
                {
                    connection = new DbConnection(val);
                    if (connection.DefaultName == defaultName) break;
                    connection = null;
                }

                if (connection != null)
                {
                    try
                    {
                        string connectionString = connection.WindowsAuthentication ?
                            string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connection Timeout=3", connection.Server, connection.Database) :
                            string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Connection Timeout=3", connection.Server, connection.Database, connection.UserName, connection.Password);

                        using (var conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            conn.Close();
                            connected = true;
                        }

                    }
                    catch
                    {
                        connected = false;
                        connection = null;
                    }
                }
            }

            return connected;
        }
    }
}
