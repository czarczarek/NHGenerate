using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NHGenerate.Core
{
    /// <summary>
    /// Generate model for datatables.
    /// </summary>
    public class ClassGenerator
    {
        private readonly DbConnection _connection;
        public ClassGenerator(DbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Return NHibernate table schema mapping.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public string NHibernateTableSchema(Setting setting)
        {
            var fileContent = new StringBuilder();

            try
            {
                using (var conn = new SqlConnection(_connection.ConnectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format("SELECT TOP 1 * FROM [{0}].[{1}]", setting.Schema, setting.Table)
                    };

                    DataTable tableSchema;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        tableSchema = reader.GetSchemaTable();
                    }

                    if (tableSchema != null)
                    {
                        fileContent.AppendLine("using System;");
                        fileContent.AppendLine("using NHibernate;");
                        fileContent.AppendLine("using System.Linq;");
                        fileContent.AppendLine("using System.Xml.Serialization;");
                        fileContent.AppendLine("using System.Collections.Generic;");
                        fileContent.AppendLine("using NHibernate.Mapping.Attributes;");
                        fileContent.AppendLine();
                        fileContent.Append("namespace ");
                        fileContent.AppendLine(setting.Namespace);
                        fileContent.AppendLine("{");

                        fileContent.AppendLine(Tab() + "[Serializable]");
                        fileContent.AppendLine(Tab() + string.Format("[Class(0, Name = \"{0}\", Table = \"{0}\", Schema = \"{1}\")]", setting.Table, setting.Schema));

                        DataRow identity = tableSchema.Rows.Cast<DataRow>().FirstOrDefault(field => (bool)field["IsIdentity"]);

                        if (identity != null)
                        {
                            fileContent.AppendLine(Tab() + string.Format("public class {0} : ModelBase<{1}>", setting.Table, setting.UseShortTypeName ? ((Type)identity["DataType"]).ToShort() : ((Type)identity["DataType"]).Name));
                        }
                        else
                        {
                            fileContent.AppendLine(Tab() + string.Format("public class {0}", setting.Table));
                        }

                        fileContent.AppendLine(Tab() + "{");

                        for (int i = 0; i < tableSchema.Rows.Count; i++)
                        {
                            DataRow field = tableSchema.Rows[i];

                            var columnName = (string)field["ColumnName"];
                            var fieldType = (Type)field["DataType"];

                            
                            var fieldTypeName = setting.UseShortTypeName ? fieldType.ToShort() : fieldType.Name;


                            if ((bool)field["IsIdentity"])
                            {
                                fileContent.AppendLine(Tab(2) + string.Format("[Id(0, Name = \"Id\", Column = \"{0}\")]", columnName));
                                if ((bool)field["IsAutoIncrement"])
                                {
                                    fileContent.AppendLine(Tab(2) + string.Format("[Generator(1, Class = \"identity\")]"));
                                }

                                fileContent.AppendLine(Tab(2) + string.Format("public virtual {0}? Id", fieldTypeName));
                                fileContent.AppendLine(Tab(2) + "{");
                                fileContent.AppendLine(Tab(3) + "set { id = value; }");
                                fileContent.AppendLine(Tab(3) + "get { return id; }");
                                fileContent.AppendLine(Tab(2) + "}");
                            }
                            else
                            {
                                var pLine = new StringBuilder();
                                bool addComma = false;

                                if ((bool)field["AllowDBNull"])
                                {
                                    if (fieldType == typeof(string))
                                    {
                                        if (setting.IncludeColumnName)
                                        {
                                            pLine.AppendFormat("Column = \"{0}\"", columnName);
                                            addComma = true;
                                        }

                                        if (setting.IncludeColumnSize)
                                        {
                                            if (addComma) pLine.Append(", ");
                                            pLine.AppendFormat("Length = {0}", field["ColumnSize"]);
                                        }

                                        if (pLine.Length > 0)
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.AppendFormat("[Property({0})]", pLine);
                                            fileContent.AppendLine();
                                        }
                                        else
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.Append("[Property]");
                                            fileContent.AppendLine();
                                        }

                                        fileContent.AppendLine(Tab(2) + "public virtual string " + columnName + " { set; get; }");
                                    }
                                    else
                                    {
                                        if (setting.IncludeColumnName)
                                        {
                                            pLine.AppendFormat("Column = \"{0}\"", columnName);
                                            addComma = true;
                                        }

                                        if (addComma) pLine.Append(", ");
                                        pLine.AppendFormat("TypeType = typeof({0})", fieldTypeName);

                                        if (pLine.Length > 0)
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.AppendFormat("[Property({0})]", pLine);
                                            fileContent.AppendLine();
                                        }
                                        else
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.Append("[Property]");
                                            fileContent.AppendLine();
                                        }

                                        fileContent.AppendLine(Tab(2) + "public virtual " + fieldTypeName + "? " + columnName + " { set; get; }");
                                    }
                                }
                                else
                                {
                                    if (fieldType == typeof(string))
                                    {
                                        if (setting.IncludeColumnName)
                                        {
                                            pLine.AppendFormat("Column = \"{0}\"", columnName);
                                            addComma = true;
                                        }

                                        if (setting.IncludeColumnSize)
                                        {
                                            if (addComma) pLine.Append(", ");
                                            pLine.AppendFormat("Length = {0}", field["ColumnSize"]);
                                            addComma = true;
                                        }

                                        if (setting.IncludeNotNull)
                                        {
                                            if (addComma) pLine.Append(", ");
                                            pLine.Append("NotNull = true");
                                        }

                                        if (pLine.Length > 0)
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.AppendFormat("[Property({0})]", pLine);
                                            fileContent.AppendLine();
                                        }
                                        else
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.Append("[Property]");
                                            fileContent.AppendLine();
                                        }

                                        fileContent.AppendLine(Tab(2) + "public virtual string " + columnName + " { set; get; }");
                                    }
                                    else
                                    {
                                        if (setting.IncludeColumnName)
                                        {
                                            pLine.AppendFormat("Column = \"{0}\"", columnName);
                                            addComma = true;
                                        }

                                        if (addComma) pLine.Append(", ");
                                        pLine.AppendFormat("TypeType = typeof({0})", fieldTypeName);

                                        if (setting.IncludeNotNull)
                                        {
                                            pLine.Append(", ");
                                            pLine.Append("NotNull = true");
                                        }

                                        if (pLine.Length > 0)
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.AppendFormat("[Property({0})]", pLine);
                                            fileContent.AppendLine();
                                        }
                                        else
                                        {
                                            fileContent.Append(Tab(2));
                                            fileContent.Append("[Property]");
                                            fileContent.AppendLine();
                                        }

                                        fileContent.AppendLine(Tab(2) + "public virtual " + fieldTypeName + " " + columnName + " { set; get; }");
                                    }
                                }
                            }

                            if (i + 1 < tableSchema.Rows.Count)
                            {
                                fileContent.AppendLine();
                            }
                        }

                        fileContent.AppendLine(Tab() + "}");

                        fileContent.AppendLine("}");
                    }

                }


            }
            catch (Exception ex)
            {
                fileContent = new StringBuilder();
                fileContent.AppendLine("// An error occurred!");
                fileContent.AppendLine(string.Format("// {0}", ex.Message));
            }

            return fileContent.ToString();
        }

        public string Tab(int count = 1)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++) sb.Append("    ");
            return sb.ToString();
        }
    }

    public static class TypeEx
    {
        public static string ToShort(this Type type)
        {
            string typeName = type.Name.Replace("System.", "");

            switch (typeName)
            {
                case "String":
                    typeName = "string";
                    break;
                case "Int32":
                    typeName = "int";
                    break;
                case "Int64":
                    typeName = "long";
                    break;
                case "Float":
                    typeName = "float";
                    break;
                case "Byte":
                    typeName = "byte";
                    break;
                case "Double":
                    typeName = "double";
                    break;
                case "Boolean":
                    typeName = "bool";
                    break;
                case "Decimal":
                    typeName = "decimal";
                    break;
                case "Char":
                    typeName = "char";
                    break;
            }

            return typeName;
        }
    }
}
