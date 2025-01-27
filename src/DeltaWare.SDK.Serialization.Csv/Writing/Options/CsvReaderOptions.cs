using DeltaWare.SDK.Serialization.Csv.Reading.Options;

namespace DeltaWare.SDK.Serialization.Csv.Writing.Options
{
    /// <inheritdoc cref="ICsvWriterOptions"/>
    public class CsvWriterOptions : ICsvWriterOptions
    {
        /// <inheritdoc cref="ICsvReaderOptions.BufferSize"/>
        public int BufferSize { get; set; } = 4096;

        /// <inheritdoc cref="ICsvReaderOptions.DelimiterCharacter"/>
        public char DelimiterCharacter { get; set; } = ',';

        /// <inheritdoc cref="ICsvReaderOptions.QuoteCharacter"/>
        public char QuoteCharacter { get; set; } = '"';
    }
}