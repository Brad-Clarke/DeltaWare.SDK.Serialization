namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class FloatTransformer : TransformerBase<float>
    {
        protected override float TransformToObjectType(string value)
            => float.Parse(value);
    }
}
