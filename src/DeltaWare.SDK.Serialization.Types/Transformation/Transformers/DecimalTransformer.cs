using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class DecimalTransformer : NullableTransformer<decimal>
    {
        protected override decimal TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string TransformToString(decimal value, CultureInfo culture = null)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}