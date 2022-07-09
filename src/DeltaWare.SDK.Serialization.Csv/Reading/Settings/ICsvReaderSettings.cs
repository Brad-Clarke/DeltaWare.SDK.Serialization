using DeltaWare.SDK.Serialization.Csv.Enums;

namespace DeltaWare.SDK.Serialization.Csv.Reading.Settings
{
    /// <summary>
    /// The settings used by the <see cref="CsvReader"/>.
    /// </summary>
    public interface ICsvReaderSettings
    {
        /// <summary>
        /// Specifies how a missing column will be handled by the reader.
        /// </summary>
        MissingColumnHandling MissingColumnHandling { get; }

        /// <summary>
        /// Denotes the Carriage Return character in Utf32 format.
        /// </summary>
        int Utf32CarriageReturn { get; }

        /// <summary>
        /// Denotes the Comma character in Utf32 format.
        /// </summary>
        int Utf32Comma { get; }

        /// <summary>
        /// Denotes the End of File character in Utf32 format.
        /// </summary>
        int Utf32EndOfFile { get; }

        /// <summary>
        /// Denotes the Line Feed character in Utf32 format.
        /// </summary>
        int Utf32LineFeed { get; }

        /// <summary>
        /// Denotes the Quotation Mark character in Utf32 format.
        /// </summary>
        int Utf32QuotationMark { get; }
    }
}