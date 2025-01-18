namespace DeltaWare.SDK.Serialization.Csv
{
    /// <summary>
    /// Indicates the current state of the CSV.
    /// </summary>
    [Flags]
    internal enum CsvState
    {
        SuppressOutput = 0,
        Output = 1,
        FieldStart = 2,
        FieldEnd = 4,
        FieldEncapsulated = 8,
        FieldTerminated = 16,
        EndOfLine = 32,
        QuotationMarkHit = 64,
        EndOfFile = 128,
        WithinComment = 256
    }
}