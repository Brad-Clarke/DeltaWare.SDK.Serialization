namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time
{
    internal sealed class TimeSpanTransformer : TransformerBase<TimeSpan>
    {
        protected override TimeSpan TransformToObjectType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return TimeSpan.MinValue;
            }

            return TimeSpan.Parse(value);
        }
    }
}