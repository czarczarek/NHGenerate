namespace NHGenerate.Core
{
    public class ConnectionResult
    {
        public string DefaultNamespace { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string FileName { get; set; }
        public DbConnection Connection { get; set; }
    }
}
