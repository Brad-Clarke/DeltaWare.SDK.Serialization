using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class TimeSpanTransformer : NullableTransformer<TimeSpan>
    {
        protected override TimeSpan TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return TimeSpan.MinValue;
            }

            string[] time = value.Split(':');

            return TimeSpan.Parse($"{time[0]}:{time[1]}:{time[2]}.{time[3]}");
        }

        protected override string TransformToString(TimeSpan value, CultureInfo culture = null)
        {
            return value.ToString();
        }
    }
}