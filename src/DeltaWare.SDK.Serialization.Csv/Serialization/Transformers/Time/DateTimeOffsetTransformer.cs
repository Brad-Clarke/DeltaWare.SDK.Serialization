using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class DateTimeOffsetTransformer : TransformerBase<DateTimeOffset>
    {
        protected override DateTimeOffset TransformToObjectType(string value, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTimeOffset.MinValue;
            }

            return DateTimeOffset.Parse(value, formatProvider);
        }

        protected override string TransformFromObjectType(DateTimeOffset value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
