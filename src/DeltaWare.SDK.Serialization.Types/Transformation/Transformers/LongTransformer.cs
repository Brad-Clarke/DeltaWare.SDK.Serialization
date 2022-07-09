using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class LongTransformer : NullableTransformer<long>
    {
        protected override long TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            return long.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string TransformToString(long value, CultureInfo culture = null)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}