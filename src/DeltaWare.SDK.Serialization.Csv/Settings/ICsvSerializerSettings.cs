#nullable enable
using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Serialization.Csv.Header;
using DeltaWare.SDK.Serialization.Csv.Validation;
using DeltaWare.SDK.Serialization.Types;

namespace DeltaWare.SDK.Serialization.Csv.Settings
{
    public interface ICsvSerializerSettings
    {
        IAttributeCache? AttributeCache { get; }

        IObjectSerializer? Serializer { get; }

        ICsvValidator? Validator { get; }

        IHeaderHandler? HeaderHandler { get; }

        bool IgnoreUnknownRecords { get; }


    }
}
