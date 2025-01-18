using DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers
{
    public abstract class TransformerBase<T> : ITransformer
    {
        public Type Type { get; } = typeof(T);

        public bool CanSerialize(Type type)
            => type == Type;

        public object? TransformToObject(string? value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return TransformToObjectType(value)!;
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

        public string? TransformToString(object? value)
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
                return TransformFromObjectType(valueType);
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

        protected abstract T TransformToObjectType(string value);

        protected virtual string TransformFromObjectType(T value) 
            => value?.ToString() ?? string.Empty;
    }
}
