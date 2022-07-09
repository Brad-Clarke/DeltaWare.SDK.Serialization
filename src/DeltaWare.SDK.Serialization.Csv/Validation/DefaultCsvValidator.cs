using DeltaWare.SDK.Core.Caching;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Validation
{
    internal class DefaultCsvValidator : ICsvValidator
    {
        private readonly IAttributeCache _attributeCache;

        public DefaultCsvValidator(IAttributeCache attributeCache)
        {
            _attributeCache = attributeCache;
        }

        public void Validate(object value, PropertyInfo property)
        {
            if (_attributeCache.TryGetAttribute(property, out RequiredAttribute required))
            {
                required.Validate(value, property.Name);
            }

            if (_attributeCache.TryGetAttribute(property, out MaxLengthAttribute maxLength))
            {
                maxLength.Validate(value, property.Name);
            }

            if (_attributeCache.TryGetAttribute(property, out MinLengthAttribute minLength))
            {
                minLength.Validate(value, property.Name);
            }
        }
    }
}
