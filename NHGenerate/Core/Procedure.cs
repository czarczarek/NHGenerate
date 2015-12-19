namespace NHGenerate.Core
{
    public class Procedure
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string FullName { get { return string.Format("{0}.{1}", Schema, Name); } }
    }
}
