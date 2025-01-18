namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    internal interface ICsvPropertyMapper
    {
        IReadOnlyCollection<PropertyMapping> CreatePropertyMappings(Type type, IReadOnlyList<string?>? csvHeaders = null);
    }
}
