using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class CharTransformer : Transformer<char>
    {
        protected override char TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new char();
            }

            return char.Parse(value);
        }

        protected override string TransformToString(char value, CultureInfo culture = null)
        {
            return value.ToString();
        }
    }
}