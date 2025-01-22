using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class BoolTransformer : TransformerBase<bool>
    {
        protected override bool TransformToObjectType(string value, IFormatProvider formatProvider)
            => bool.Parse(value);

        protected override string TransformFromObjectType(bool value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
