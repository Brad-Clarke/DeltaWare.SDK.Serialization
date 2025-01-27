using System;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public sealed class CsvSerializationException(string message, Exception innerException) : Exception(message, innerException)
    {
        internal static CsvSerializationException FailedToDeserializeField(int lineNumber, PropertyInfo property, Exception innerException)
            => new($"Failed to deserialize the field at line {lineNumber} for property {property.Name}. Please check the data format and ensure it matches the expected schema. Refer to the inner exception for more details.", innerException);

        internal static CsvSerializationException FieldValidationFailed(int lineNumber, PropertyInfo property, Exception innerException)
            => new($"The field at line {lineNumber} failed validation for property {property.Name}. Refer to the inner exception for more details.", innerException);

    }
}
