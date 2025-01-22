using DeltaWare.SDK.Serialization.Csv.Serialization.Attributes;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Number;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Time;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Serialization
{
    internal sealed class PropertySerializer : IPropertySerializer
    {
        private readonly ITransformer[] _defaultTransformers =
        [
            new DecimalTransformer(),
            new FloatTransformer(),
            new IntTransformer(),
            new LongTransformer(),
            new ShortTransformer(),
            new DateTimeOffsetTransformer(),
            new DateTimeTransformer(),
            new TimeSpanTransformer(),
            new BoolTransformer(),
            new CharTransformer(),
            new GuidTransformer(),
            new StringTransformer()
        ];

        private readonly IFormatProvider _formatProvider;

        private readonly IReadOnlyDictionary<Type, ITransformer> _transformers;

        public PropertySerializer(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
            _transformers = _defaultTransformers
                .ToDictionary(t => t.Type, t => t);
        }

        public string? Serialize(PropertyInfo property, object? value)
        {
            var transformer = GetTransformer(property);

            if (transformer == null)
            {
                throw new Exception("We can't serialize this type because it has no associated transformer.");
            }

            var formatOverride = property.GetCustomAttribute<UseFormatProviderAttribute>()?.FormatProvider;

            return transformer.TransformToString(value, formatOverride ?? _formatProvider);
        }

        public object? Deserialize(PropertyInfo property, string? value)
        {
            var transformer = GetTransformer(property);

            if (transformer == null)
            {
                throw new Exception("We can't serialize this type because it has no associated transformer.");
            }

            var formatOverride = property.GetCustomAttribute<UseFormatProviderAttribute>()?.FormatProvider;

            return transformer.TransformToObject(value, formatOverride ?? _formatProvider);
        }

        public bool CanSerializer(PropertyInfo property)
            => GetTransformer(property) != null;

        private ITransformer? GetTransformer(PropertyInfo property)
        {
            var useTransformer = property.GetCustomAttribute<UseTransformerAttribute>();

            if (useTransformer != null)
            {
                return (ITransformer?)Activator.CreateInstance(useTransformer.TransformerType);
            }

            if (_transformers.TryGetValue(property.PropertyType, out var transformer))
            {
                return transformer;
            }

            if (property.PropertyType.IsEnum)
            {
                return new EnumTransformer(property.PropertyType);
            }

            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);

            if (underlyingType == null)
            {
                return null;
            }

            if (_transformers.TryGetValue(underlyingType, out transformer))
            {
                return transformer;
            }

            return null;

        }
    }
}