namespace DeltaWare.SDK.Serialization.Csv.Reading.Options
{
    /// <inheritdoc cref="ICsvReaderOptions"/>
    public class CsvReaderOptions : ICsvReaderOptions
    {
        /// <inheritdoc cref="ICsvReaderOptions.BufferSize"/>
        public int BufferSize { get; set; } = 4096;

        /// <inheritdoc cref="ICsvReaderOptions.DelimiterCharacter"/>
        public char DelimiterCharacter { get; set; } = ',';

        /// <inheritdoc cref="ICsvReaderOptions.QuoteCharacter"/>
        public char QuoteCharacter { get; set; } = '"';

        /// <inheritdoc cref="ICsvReaderOptions.SuppressWhitespace"/>
        public bool SuppressWhitespace { get; set; } = false;

        /// <inheritdoc cref="ICsvReaderOptions.IgnoreComments"/>
        public bool IgnoreComments { get; set; } = true;

        /// <inheritdoc cref="ICsvReaderOptions.CommentCharacter"/>
        public char CommentCharacter { get; set; } = '#';

        /// <inheritdoc cref="ICsvReaderOptions.SkipEmptyLines"/>
        public bool SkipEmptyLines { get; set; } = true;

        /// <inheritdoc cref="ICsvReaderOptions.TrimFields"/>
        public bool TrimFields { get; set; } = false;
    }
}