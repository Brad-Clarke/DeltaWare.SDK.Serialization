using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Csv.Options
{
    public sealed class CsvSerializerOptions : ICsvSerializerOptions
    {
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.CurrentCulture;
        public bool CaseInsensitiveHeaders { get; set; } = true;
    }
}
