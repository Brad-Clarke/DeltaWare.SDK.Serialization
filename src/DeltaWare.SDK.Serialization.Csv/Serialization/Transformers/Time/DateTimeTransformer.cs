using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class DateTimeTransformer : TransformerBase<DateTime>
    {
        protected override DateTime TransformToObjectType(string value, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(value, formatProvider);
        }

        protected override string TransformFromObjectType(DateTime value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
