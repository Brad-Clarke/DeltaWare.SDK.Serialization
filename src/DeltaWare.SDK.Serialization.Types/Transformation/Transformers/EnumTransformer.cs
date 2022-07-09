using DeltaWare.SDK.Serialization.Types.Transformation.Exceptions;
using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Transformers
{
    public class EnumTransformer : ITransformer
    {
        public Type Type { get; } = typeof(Enum);

        public bool CanSerialize(Type type)
        {
            if (type == Type)
            {
                return true;
            }

            if (type.IsEnum)
            {
                return type.BaseType == Type;
            }

            return false;
        }

        public object TransformToObject(string value, Type toType, CultureInfo culture = null)
        {
            try
            {
                if (toType == Type || toType.BaseType == Type)
                {
                    return (Enum)Enum.Parse(toType, value);
                }

                throw new InvalidTransformationTypeException(toType, Type);
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

        public string TransformToString(object value, Type type, CultureInfo culture = null)
        {
            try
            {
                if (type == Type || type.BaseType == Type)
                {
                    return value.ToString();
                }

                throw new InvalidTransformationTypeException(type, Type);
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