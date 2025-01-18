namespace DeltaWare.SDK.Serialization.Csv.Writing.Options
{
    /// <inheritdoc cref="ICsvWriterOptions"/>
    public class CsvWriterOptions : ICsvWriterOptions
    {
        /// <inheritdoc cref="ICsvWriterOptions.BufferSize"/>
        public int BufferSize { get; set; } = 4096;

        /// <inheritdoc cref="ICsvWriterOptions.DelimiterCharacter"/>
        public char DelimiterCharacter { get; set; } = ',';

        /// <inheritdoc cref="ICsvWriterOptions.QuoteCharacter"/>
        public char QuoteCharacter { get; set; } = '"';

        /// <inheritdoc cref="ICsvWriterOptions.SuppressWhitespace"/>
        public bool SuppressWhitespace { get; set; } = false;
    }
}