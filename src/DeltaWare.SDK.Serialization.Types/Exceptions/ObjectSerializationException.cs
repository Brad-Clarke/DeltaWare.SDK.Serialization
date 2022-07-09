using System;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Types.Exceptions
{
    public class ObjectSerializationException : Exception
    {
        public ObjectSerializationException(Type type, Exception innerException = null) : base($"Serialization failed for {type.Name}", innerException)
        {
        }

        public ObjectSerializationException(PropertyInfo property, Exception innerException = null) : base($"Serialization failed for Property {property.Name}", innerException)
        {
        }
    }
}