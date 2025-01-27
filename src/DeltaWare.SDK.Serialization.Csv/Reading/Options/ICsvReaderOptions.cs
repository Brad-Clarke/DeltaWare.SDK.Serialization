namespace DeltaWare.SDK.Serialization.Csv.Reading.Options
{
    /// <summary>
    /// Defines configuration options for the <see cref="CsvStreamReader"/> to customize its parsing behavior.
    /// </summary>
    public interface ICsvReaderOptions
    {
        /// <summary>
        /// The size of the buffer used for reading from the stream.
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        /// The character used to delimit fields within a CSV record.
        /// </summary>
        char DelimiterCharacter { get; }

        /// <summary>
        /// The character used to quote fields, allowing for delimiters within quoted text.
        /// </summary>
        char QuoteCharacter { get; }

        /// <summary>
        /// A value indicating whether to ignore comments in the CSV data.
        /// </summary>
        bool IgnoreComments { get; }

        /// <summary>
        /// The character used to start a comment line within the CSV data.
        /// </summary>
        char CommentCharacter { get; }

        /// <summary>
        /// A value indicating whether to skip empty lines while reading CSV data.
        /// </summary>
        bool SkipEmptyLines { get; }

        /// <summary>
        /// A value indicating whether to trim the whitespace from the beginning an end of a field.
        /// </summary>
        bool TrimFields { get; }
    }
}