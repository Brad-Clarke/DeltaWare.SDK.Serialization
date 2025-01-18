using DeltaWare.SDK.Serialization.Csv.Reading;

namespace DeltaWare.SDK.Serialization.Csv.Writing.Options
{
    /// <summary>
    /// Defines configuration options for the <see cref="CsvStreamReader"/> to customize its parsing behavior.
    /// </summary>
    public interface ICsvWriterOptions
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
        /// A value indicating whether to suppress whitespace at the start and end of each field.
        /// </summary>
        bool SuppressWhitespace { get; }
    }
}