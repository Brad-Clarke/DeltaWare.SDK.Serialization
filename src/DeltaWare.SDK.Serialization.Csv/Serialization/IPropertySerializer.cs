using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Serialization
{
    public interface IPropertySerializer
    {
        string? Serialize(PropertyInfo property, object? value);

        object? Deserialize(PropertyInfo property, string? value);

        bool CanSerializer(PropertyInfo property);
    }
}
