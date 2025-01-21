using System;
using System.Collections.Generic;
using System.Reflection;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Serialization.Attributes;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public class CsvSchemaException : Exception
    {
        public CsvSchemaException(string message) : base(message)
        {
        }

        public CsvSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal static CsvSchemaException UnsupportedPropertyType(PropertyInfo property)
            => new($"The property '{property.Name}' of type '{property.PropertyType.Name}' is not supported for CSV serialization. To resolve this issue, you can either exclude this property from serialization by applying the [{nameof(CsvIgnoreAttribute)}] or provide a custom serializer for this type using the [{nameof(UseTransformerAttribute)}]");

        internal static CsvSchemaException MultiplePropertiesMappedToSameIndex(int columnIndex, PropertyInfo propertyA, PropertyInfo propertyB) 
            => new($"Property mapping conflict: Both '{propertyA}' and '{propertyB}' are mapped to the same CSV column index {columnIndex}. Each property should be mapped to a unique column index.");
        
        internal static CsvSchemaException MultiplePropertiesMappedToSameIndex(IEnumerable<CsvSchemaException> innerExceptions) 
            => new("Property mapping conflict: Multiple properties are mapped to the same CSV column index. Check the InnerException(s) for details on each conflict.", new AggregateException(innerExceptions));

        internal static CsvSchemaException InvalidPropertyMappingStrategy(PropertyInfo property, string currentStrategy, string invalidStrategy)
            => new($"Property mapping conflict: The property '{property.Name}' of type '{property.PropertyType.Name}' is being mapped using the '{invalidStrategy}' strategy, which is invalid in the current context where '{currentStrategy}' strategy is expected.");

        internal static CsvSchemaException PropertyCouldNotBeMappedToHeader(PropertyInfo property, string targetHeader)
            => new($"Property mapping conflict: The target header '{targetHeader}' could not be found among the CSV headers. As a result, it could not be mapped to the property '{property.Name}'. Ensure that the CSV file includes a header that matches '{targetHeader}' and corresponds to the expected property.");

        internal static CsvSchemaException HeaderPropertyCountMismatch(int headerCount, int propertyCount)
            => new($"CSV configuration error: The number of headers in the CSV file ({headerCount}) does not match the number of mappable properties ({propertyCount}). Ensure that your CSV file includes a header for each property and that all properties intended for mapping are correctly annotated or configured.");
    }
}