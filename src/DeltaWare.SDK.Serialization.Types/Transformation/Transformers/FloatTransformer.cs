using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class FloatTransformer : NullableTransformer<float>
    {
        protected override float TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string TransformToString(float value, CultureInfo culture = null)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}