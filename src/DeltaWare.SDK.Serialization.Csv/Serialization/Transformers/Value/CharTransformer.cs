using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class CharTransformer : TransformerBase<char>
    {
        protected override char TransformToObjectType(string value, IFormatProvider formatProvider)
            => char.Parse(value);

        protected override string TransformFromObjectType(char value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
