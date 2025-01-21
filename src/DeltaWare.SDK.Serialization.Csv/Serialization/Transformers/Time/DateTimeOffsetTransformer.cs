using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class DateTimeOffsetTransformer : TransformerBase<DateTimeOffset>
    {
        protected override DateTimeOffset TransformToObjectType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTimeOffset.MinValue;
            }

            return DateTimeOffset.Parse(value);
        }

        protected override string TransformFromObjectType(DateTimeOffset value)
        {
            if (value.TimeOfDay == TimeSpan.Zero)
            {
                return value.ToString("d", CultureInfo.InvariantCulture);
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
