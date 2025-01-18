namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class ShortTransformer : TransformerBase<short>
    {
        protected override short TransformToObjectType(string value)
            => short.Parse(value);
    }
}
