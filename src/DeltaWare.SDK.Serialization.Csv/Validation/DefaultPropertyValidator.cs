using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Validation
{
    internal sealed class DefaultPropertyValidator : IPropertyValidator
    {
        private readonly ValidationAttribute[] _validators;

        private readonly PropertyInfo _propertyInfo;

        public DefaultPropertyValidator(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            _validators = _propertyInfo.GetCustomAttributes<ValidationAttribute>(true).ToArray();
        }


        public void Validate(object? value)
        {
            foreach (var validator in _validators)
            {
                validator.Validate(value, _propertyInfo.Name);
            }
        }
    }
}
