namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number
{
    internal sealed class DecimalTransformer : TransformerBase<decimal>
    {
        protected override decimal TransformToObjectType(string value)
            => decimal.Parse(value);
    }
}
