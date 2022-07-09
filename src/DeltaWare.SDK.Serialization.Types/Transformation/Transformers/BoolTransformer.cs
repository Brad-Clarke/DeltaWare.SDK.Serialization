using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class BoolTransformer : NullableTransformer<bool>
    {
        protected override bool TransformToObject(string value, CultureInfo culture = null)
        {
            return bool.Parse(value);
        }

        protected override string TransformToString(bool value, CultureInfo culture = null)
        {
            return value.ToString();
        }
    }
}