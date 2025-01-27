using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class StringTransformer : TransformerBase<string>
    {
        protected override string TransformToObjectType(string value, IFormatProvider formatProvider)
            => value;

        protected override string TransformFromObjectType(string value, IFormatProvider formatProvider)
            => value;
    }
}
