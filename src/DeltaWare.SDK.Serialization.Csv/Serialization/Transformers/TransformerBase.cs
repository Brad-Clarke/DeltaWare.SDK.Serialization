using DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions;
using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers
{
    public abstract class TransformerBase<T> : ITransformer
    {
        public Type Type { get; } = typeof(T);

        public bool CanSerialize(Type type)
            => type == Type;

        public object? TransformToObject(string? value, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return null;
            }

            if (Type != typeof(string) && string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                return TransformToObjectType(value, formatProvider)!;
            }
            catch (InvalidTransformationException)
            {
                throw;
            }
            catch (InvalidTransformationTypeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidTransformationException(value, Type, e);
            }
        }

        public string? TransformToString(object? value, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return null;
            }

            if (value is not T valueType)
            {
                throw new InvalidTransformationTypeException(value.GetType(), Type);
            }

            try
            {
                return TransformFromObjectType(valueType, formatProvider);
            }
            catch (InvalidTransformationException)
            {
                throw;
            }
            catch (InvalidTransformationTypeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidTransformationException(value, Type, e);
            }
        }

        protected abstract T TransformToObjectType(string value, IFormatProvider formatProvider);

        protected abstract string TransformFromObjectType(T value, IFormatProvider formatProvider);
    }
}
