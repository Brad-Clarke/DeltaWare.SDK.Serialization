using System;

namespace DeltaWare.SDK.Serialization.Csv.Options
{
    public interface ICsvSerializerOptions
    {
        IFormatProvider FormatProvider { get; }
    }
}
