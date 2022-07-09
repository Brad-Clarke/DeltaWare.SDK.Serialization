namespace DeltaWare.SDK.Serialization.Csv.Enums
{
    /// <summary>
    /// Indicates what the <see cref="CsvSerializer"/> will use to build the PropertyIndex.
    /// </summary>
    internal enum IndexBuilderMode
    {
        /// <summary>
        /// Indicates that no mode has been selected.
        /// </summary>
        None,

        /// <summary>
        /// Indicates the the properties attributes be will used.
        /// </summary>
        Attribute,

        /// <summary>
        /// Indicates that the properties name or index will be used.
        /// </summary>
        Property
    }
}