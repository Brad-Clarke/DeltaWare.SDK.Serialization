using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class ShortTransformer : NullableTransformer<short>
    {
        protected override short TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return short.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string TransformToString(short value, CultureInfo culture = null)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}