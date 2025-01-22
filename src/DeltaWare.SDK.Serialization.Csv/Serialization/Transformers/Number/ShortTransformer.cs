using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class ShortTransformer : TransformerBase<short>
    {
        protected override short TransformToObjectType(string value, IFormatProvider formatProvider)
            => short.Parse(value, formatProvider);

        protected override string TransformFromObjectType(short value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
