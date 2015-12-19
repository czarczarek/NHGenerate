using System.Data;
using System.Data.SqlClient;

namespace NHGenerate.Core
{
    /// <summary>
    /// Generate new update script file name.
    /// </summary>
    public class ScriptGenerator
    {
        private readonly ConfigScriptProperties _properties;
        private readonly string _schemaName = "dbo";
        private readonly string _tableName = "UpdateScriptFileNumbers";
        private const int SeedValue = 100000000;
        private readonly DbConnection _connection;

        /// <summary>
        /// Create instance of <see cref="ScriptGenerator"/> with connection to database.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        public ScriptGenerator(DbConnection connection, ConfigScriptProperties properties)
        {
            _connection = connection;
            _properties = properties;

            _schemaName = _properties.UpdateScriptSchemaName;
            _tableName = _properties.UpdateScriptTableName;
        }

        public string FileNumber { get; set; }

        /// <summary>
        /// Get new file name.
        /// </summary>
        /// <returns>Script file name.</returns>
        public string GetFileName()
        {
            string fileName;

            using (var conn = new SqlConnection(_connection.ConnectionString))
            {
                conn.Open();


                bool tableExists = TableExists(conn);
                if (!tableExists)
                {
                    CreateTable(conn);
                }

                int currentId = GetCurrentIdentity(conn);

                bool isUnique = !ExistIdentity(conn, currentId);

                var newIdentity = isUnique ? currentId : currentId + 1;

                FileNumber = newIdentity.ToString("000-0000-00").Replace("-", ".");

                fileName = string.Format(_properties.UpdateScriptTemplate, FileNumber);

                InsertFileName(conn, fileName);
            }

            return fileName;
        }

        private void InsertFileName(SqlConnection conn, string fileName)
        {
            var newRowInsert = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = string.Format("INSERT INTO [{0}].[{1}] ([FileName],[UserName]) VALUES ('{2}','{3}')", _schemaName, _tableName, fileName, _properties.UpdateScriptUserName)
            };

            newRowInsert.ExecuteNonQuery();
        }

        private int GetCurrentIdentity(SqlConnection conn)
        {
            int currentId;
            var currentIdentity = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = string.Format("SELECT CAST(IDENT_CURRENT('[{0}].[{1}]') AS INT)", _schemaName, _tableName)
            };

            using (var reader = currentIdentity.ExecuteReader())
            {
                reader.Read();
                currentId = (int) reader[0];
            }

            return currentId;
        }

        private bool ExistIdentity(SqlConnection conn, int id)
        {
            var cmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = string.Format("IF EXISTS (SELECT 1 FROM [{0}].[{1}] WHERE Id = {2}) SELECT 1 ELSE SELECT 0", _schemaName, _tableName, id)
            };

            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                return (int)reader[0] > 0;
            }
        }

        private bool TableExists(SqlConnection conn)
        {
            var cmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = string.Format("IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U')) SELECT 1 ELSE SELECT 0", _schemaName, _tableName)
            };

            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                return (int) reader[0] > 0;
            }
        }

        private void CreateTable(SqlConnection conn)
        {
            var script = string.Format(@"
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [{0}].[{1}](
	[Id] [int] IDENTITY({2},1) NOT NULL,
	[FileName] [varchar](100) NOT NULL,
    [UserName] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_{1}] UNIQUE NONCLUSTERED 
(
	[FileName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


SET ANSI_PADDING OFF

ALTER TABLE [{0}].[{1}] ADD  CONSTRAINT [DF_{1}_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]", _schemaName, _tableName, SeedValue);

            var createTableCmd = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = script
            };

            createTableCmd.ExecuteNonQuery();
        }
    }
}
