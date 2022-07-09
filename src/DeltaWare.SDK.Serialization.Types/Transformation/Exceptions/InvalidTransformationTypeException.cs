using System;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Exceptions
{
    /// <summary>
    /// Thrown when an invalid type was provided.
    /// </summary>
    public class InvalidTransformationTypeException : Exception
    {
        public InvalidTransformationTypeException(Type type) : base($"{type.Name} is not a supported type.")
        {
        }

        public InvalidTransformationTypeException(Type type, Type expectedType) : base($"{type.Name} cannot be transformed as it is not a {expectedType.Name}.")
        {
        }
    }
}