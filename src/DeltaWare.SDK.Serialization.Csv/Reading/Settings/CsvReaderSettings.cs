using DeltaWare.SDK.Serialization.Csv.Enums;

namespace DeltaWare.SDK.Serialization.Csv.Reading.Settings
{
    /// <inheritdoc cref="ICsvReaderSettings"/>
    public class CsvReaderSettings : ICsvReaderSettings
    {
        /// <inheritdoc cref="ICsvReaderSettings.MissingColumnHandling"/>
        public MissingColumnHandling MissingColumnHandling { get; set; }

        /// <inheritdoc cref="ICsvReaderSettings.Utf32CarriageReturn"/>
        public int Utf32CarriageReturn { get; set; }

        /// <inheritdoc cref="ICsvReaderSettings.Utf32Comma"/>
        public int Utf32Comma { get; set; }

        /// <inheritdoc cref="ICsvReaderSettings.Utf32EndOfFile"/>
        public int Utf32EndOfFile { get; set; }

        /// <inheritdoc cref="ICsvReaderSettings.Utf32LineFeed"/>
        public int Utf32LineFeed { get; set; }

        /// <inheritdoc cref="ICsvReaderSettings.Utf32QuotationMark"/>
        public int Utf32QuotationMark { get; set; }

        /// <summary>
        /// Gets the default settings for the <see cref="CsvReaderSettings"/>.
        /// </summary>
        public static CsvReaderSettings Default => new()
        {
            MissingColumnHandling = MissingColumnHandling.NotAllow,
            Utf32CarriageReturn = 13,
            Utf32Comma = 44,
            Utf32EndOfFile = -1,
            Utf32LineFeed = 10,
            Utf32QuotationMark = 34
        };
    }
}