namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class IntTransformer : TransformerBase<int>
    {
        protected override int TransformToObjectType(string value)
            => int.Parse(value);
    }
}
