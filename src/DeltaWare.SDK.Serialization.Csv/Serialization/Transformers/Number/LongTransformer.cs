namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class LongTransformer : TransformerBase<long>
    {
        protected override long TransformToObjectType(string value)
            => long.Parse(value);
    }
}
