using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Validation
{
    internal sealed class DefaultCsvValidator : ICsvValidator
    {
        public void Validate(PropertyInfo property, object? value)
        {
            property.GetCustomAttribute<RequiredAttribute>()?.Validate(value, property.Name);
            property.GetCustomAttribute<MaxLengthAttribute>()?.Validate(value, property.Name);
            property.GetCustomAttribute<MinLengthAttribute>()?.Validate(value, property.Name);
        }
    }
}
