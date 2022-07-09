using DeltaWare.SDK.Serialization.Types.Transformation.Exceptions;
using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation
{
    public abstract class NullableTransformer<T> : Transformer<T>, INullableTransformer
    {
        public override bool CanSerialize(Type type)
        {
            if (type == Type)
            {
                return true;
            }

            Type nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType == null)
            {
                return false;
            }

            return type == nullableType;
        }

        public override object TransformToObject(string value, Type toType, CultureInfo culture = null)
        {
            if (toType != Type)
            {
                Type nullableType = Nullable.GetUnderlyingType(toType);

                if (nullableType == Type)
                {
                    toType = nullableType;

                    // Only a nullable type will return null. If the type is not nullable it will be
                    // processed again.
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                }
                else
                {
                    throw new InvalidTransformationTypeException(toType, Type);
                }
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

        public override string TransformToString(object value, Type type, CultureInfo culture = null)
        {
            if (type != Type)
            {
                Type nullableType = Nullable.GetUnderlyingType(type);

                if (nullableType == Type)
                {
                    type = nullableType;

                    // Only a nullable type will return null. If the type is not nullable it will be
                    // processed again.
                    if (value == null)
                    {
                        return null;
                    }
                }
                else
                {
                    throw new InvalidTransformationTypeException(type, Type);
                }
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
    }
}