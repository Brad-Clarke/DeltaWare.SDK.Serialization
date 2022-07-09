using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class StringTransformer : Transformer<string>
    {
        protected override string TransformToObject(string value, CultureInfo culture = null)
        {
            return value;
        }

        protected override string TransformToString(string value, CultureInfo culture = null)
        {
            return value;
        }
    }
}