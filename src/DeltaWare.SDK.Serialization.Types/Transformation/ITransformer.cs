using DeltaWare.SDK.Serialization.Types.Transformation.Exceptions;
using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation
{
    /// <summary>
    /// Transforms a string to a object.
    /// </summary>
    public interface ITransformer
    {
        Type Type { get; }

        /// <summary>
        /// Checks if the specified <see cref="Type"/> can be Transformed by the transformer.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to be checked.</param>
        /// <returns>Returns <see landword="true"/> if the <see cref="Type"/> can be Transformed or <see langword="false"/> if it cannot.</returns>
        bool CanSerialize(Type type);

        /// <summary>
        /// Transforms the specified string to an object.
        /// </summary>
        /// <param name="value">The value to be transformed.</param>
        /// <param name="toType">The type being transformed.</param>
        /// <param name="culture">Specifies the culture.</param>
        /// <exception cref="InvalidTransformationTypeException">
        /// Thrown when an invalid type is provided.
        /// </exception>
        /// <exception cref="InvalidTransformationException">
        /// Thrown when an invalid value is provided.
        /// </exception>
        object TransformToObject(string value, Type toType, CultureInfo culture = null);

        string TransformToString(object value, Type type, CultureInfo culture = null);
    }

    public interface INullableTransformer : ITransformer
    {
    }
}