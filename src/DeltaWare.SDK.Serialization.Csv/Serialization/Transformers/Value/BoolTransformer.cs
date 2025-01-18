namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class BoolTransformer : TransformerBase<bool>
    {
        protected override bool TransformToObjectType(string value)
            => bool.Parse(value);
    }
}
