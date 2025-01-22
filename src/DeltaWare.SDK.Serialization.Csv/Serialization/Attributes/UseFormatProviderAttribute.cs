using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UseFormatProviderAttribute(IFormatProvider formatProvider) : Attribute
    {
        public IFormatProvider FormatProvider { get; } = formatProvider;
    }
}
