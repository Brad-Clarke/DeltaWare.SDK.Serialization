using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class GuidTransformer : NullableTransformer<Guid>
    {
        protected override Guid TransformToObject(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Guid.Empty;
            }

            return Guid.Parse(value);
        }

        protected override string TransformToString(Guid value, CultureInfo culture = null)
        {
            return value.ToString();
        }
    }
}