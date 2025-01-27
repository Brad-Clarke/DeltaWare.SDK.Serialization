using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions
{
    /// <summary>
    /// Thrown when an invalid type was provided.
    /// </summary>
    public class InvalidTransformationTypeException : TransformationException
    {
        public InvalidTransformationTypeException(Type type) : base($"The Type {type.Name} Could not be serialized because it has not associated transformer. Please use the [UseTransformerAttribute] to provide the transformer to be used for this type.")
        {
        }

        public InvalidTransformationTypeException(Type type, Type expectedType) : base($"{type.Name} cannot be transformed as it is not a {expectedType.Name}.")
        {
        }
    }
}