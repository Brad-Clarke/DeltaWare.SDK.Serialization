using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class TimeSpanTransformer : TransformerBase<TimeSpan>
    {
        protected override TimeSpan TransformToObjectType(string value, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return TimeSpan.MinValue;
            }

            return TimeSpan.Parse(value, formatProvider);
        }

        protected override string TransformFromObjectType(TimeSpan value, IFormatProvider formatProvider)
            => value.ToString(null, formatProvider);
    }
}