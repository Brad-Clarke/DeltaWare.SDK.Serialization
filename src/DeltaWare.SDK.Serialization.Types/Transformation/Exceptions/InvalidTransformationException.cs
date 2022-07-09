using System;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Exceptions
{
    /// <summary>
    /// Thrown when an invalid value was provided.
    /// </summary>
    public class InvalidTransformationException : Exception
    {
        public InvalidTransformationException(string value, Type toType, Exception innerException = null) : base($"Could not transform the provided value:[{value}] as it is not supported by {toType.Name}.")
        {
        }

        public InvalidTransformationException(object value, Type toType, Exception innerException = null) : base($"Could not transform the provided value:[{value}] as it is not supported by {toType.Name}.")
        {
        }
    }
}