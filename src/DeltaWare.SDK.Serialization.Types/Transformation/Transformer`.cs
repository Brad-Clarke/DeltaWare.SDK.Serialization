using DeltaWare.SDK.Serialization.Types.Transformation.Exceptions;
using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation
{
    public abstract class Transformer<T> : ITransformer
    {
        public Type Type { get; }

        protected Transformer()
        {
            Type = typeof(T);
        }

        public virtual object TransformToObject(string value, Type toType, CultureInfo culture = null)
        {
            if (toType != Type)
            {
                throw new InvalidTransformationTypeException(toType, Type);
            }

            try
            {
                return TransformToObject(value, culture);
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
                throw new InvalidTransformationException(value, toType, e);
            }
        }

        public virtual string TransformToString(object value, Type type, CultureInfo culture = null)
        {
            if (type != Type)
            {
                throw new InvalidTransformationTypeException(type, Type);
            }

            try
            {
                return TransformToString((T)value, culture);
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
                throw new InvalidTransformationException(value, type, e);
            }
        }

        public virtual bool CanSerialize(Type type)
        {
            return type == Type;
        }

        protected abstract T TransformToObject(string value, CultureInfo culture = null);

        protected abstract string TransformToString(T value, CultureInfo culture = null);
    }
}