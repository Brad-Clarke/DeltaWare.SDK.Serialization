using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class DateTimeTransformer : NullableTransformer<DateTime>
    {
        protected override DateTime TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(value, culture);
        }

        protected override string TransformToString(DateTime value, CultureInfo culture = null)
        {
            if (value.TimeOfDay == TimeSpan.Zero)
            {
                return value.ToString("d", culture);
            }

            return value.ToString(culture);
        }
    }
}