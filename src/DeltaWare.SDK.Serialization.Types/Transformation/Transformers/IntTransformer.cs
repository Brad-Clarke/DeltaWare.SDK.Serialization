using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class IntTransformer : NullableTransformer<int>
    {
        protected override int TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string TransformToString(int value, CultureInfo culture = null)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}