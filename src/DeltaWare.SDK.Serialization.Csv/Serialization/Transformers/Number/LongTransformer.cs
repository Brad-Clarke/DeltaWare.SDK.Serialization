using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class LongTransformer : TransformerBase<long>
    {
        protected override long TransformToObjectType(string value, IFormatProvider formatProvider)
            => long.Parse(value, formatProvider);

        protected override string TransformFromObjectType(long value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
