using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class FloatTransformer : TransformerBase<float>
    {
        protected override float TransformToObjectType(string value, IFormatProvider formatProvider)
            => float.Parse(value, formatProvider);

        protected override string TransformFromObjectType(float value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
