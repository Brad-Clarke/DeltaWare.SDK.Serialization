using System.Globalization;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class DateTimeTransformer : TransformerBase<DateTime>
    {
        protected override DateTime TransformToObjectType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(value);
        }

        protected override string TransformFromObjectType(DateTime value)
        {
            if (value.TimeOfDay == TimeSpan.Zero)
            {
                return value.ToString("d", CultureInfo.InvariantCulture);
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
