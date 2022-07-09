#nullable enable
using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Serialization.Csv.Header;
using DeltaWare.SDK.Serialization.Csv.Validation;
using DeltaWare.SDK.Serialization.Types;

namespace DeltaWare.SDK.Serialization.Csv.Settings
{
    public class CsvSerializerSettings : ICsvSerializerSettings
    {
        public IAttributeCache? AttributeCache { get; set; }
        public IObjectSerializer? Serializer { get; set; }
        public ICsvValidator? Validator { get; set; }
        public IHeaderHandler? HeaderHandler { get; set; }
        public bool IgnoreUnknownRecords { get; set; }

        public static CsvSerializerSettings Default => new();
    }
}
