namespace DeltaWare.SDK.Serialization.Csv.Writing
{
    /// <summary>
    /// Specifies how the <see cref="CsvWriter"/> will write the CSV data.
    /// </summary>
    public enum WriteMode
    {
        /// <summary>
        /// Default for writing CSV data.
        /// </summary>
        Default,
        /// <summary>
        /// Enables support for writing Record Type CSV data, this disables checks performed to check for CSV data validity.
        /// </summary>
        Record
    }
}
