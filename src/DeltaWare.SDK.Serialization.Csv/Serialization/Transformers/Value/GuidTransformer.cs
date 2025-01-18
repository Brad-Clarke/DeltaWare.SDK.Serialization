namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class GuidTransformer : TransformerBase<Guid>
    {
        protected override Guid TransformToObjectType(string value)
            => Guid.Parse(value);
    }
}
