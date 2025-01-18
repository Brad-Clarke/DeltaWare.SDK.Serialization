namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class CharTransformer : TransformerBase<char>
    {
        protected override char TransformToObjectType(string value)
            => char.Parse(value);
    }
}
