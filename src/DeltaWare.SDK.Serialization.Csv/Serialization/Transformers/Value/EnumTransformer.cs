using System;
using DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class EnumTransformer : ITransformer
    {
        public Type Type { get; }

        public EnumTransformer(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException();
            }

            Type = enumType;
        }

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

        public object? TransformToObject(string? value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return Enum.Parse(Type, value.Trim());
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
            => value?.ToString();
    }
}