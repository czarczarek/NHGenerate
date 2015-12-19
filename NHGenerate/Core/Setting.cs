namespace NHGenerate.Core
{
    /// <summary>
    /// Settings used to generate class model.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Schema of table.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Class namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Table name.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Whether to include column name.
        /// </summary>
        public bool IncludeColumnName { get; set; }

        /// <summary>
        /// Whether to include column size.
        /// </summary>
        public bool IncludeColumnSize { get; set; }

        /// <summary>
        /// Whether to include NotNull clause.
        /// </summary>
        public bool IncludeNotNull { get; set; }

        /// <summary>
        /// Whether to use short type name.
        /// </summary>
        public bool UseShortTypeName { get; set; }
    }
}
