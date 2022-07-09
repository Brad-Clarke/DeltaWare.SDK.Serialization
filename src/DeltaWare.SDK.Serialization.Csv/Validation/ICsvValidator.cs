using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Validation
{
    public interface ICsvValidator
    {
        void Validate(object value, PropertyInfo property);
    }
}
