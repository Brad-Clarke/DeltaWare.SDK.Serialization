using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class DecimalTransformer : TransformerBase<decimal>
    {
        protected override decimal TransformToObjectType(string value, IFormatProvider formatProvider)
            => decimal.Parse(value, formatProvider);

        protected override string TransformFromObjectType(decimal value, IFormatProvider formatProvider)
            => value.ToString(formatProvider);
    }
}
