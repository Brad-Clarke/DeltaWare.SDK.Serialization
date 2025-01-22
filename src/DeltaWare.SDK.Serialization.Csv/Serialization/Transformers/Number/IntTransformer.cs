using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class IntTransformer : TransformerBase<int>
    {
        protected override int TransformToObjectType(string value, IFormatProvider formatProvider)
            => int.Parse(value, formatProvider);

        protected override string TransformFromObjectType(int value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
