using DeltaWare.SDK.Serialization.Csv.Serialization.Attributes;
using DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions;
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
    internal interface ITransformerProvider
    {
        ITransformer GetTransformer(PropertyInfo property);
        bool CanSerializer(PropertyInfo property);
    }

    internal sealed class TransformerProvider : ITransformerProvider
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

        public TransformerProvider(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
            _transformers = _defaultTransformers
                .ToDictionary(t => t.Type, t => t);
        }

        public ITransformer GetTransformer(PropertyInfo property)
        {
            var transformer = InternalGetTransformer(property);

            if (transformer == null)
            {
                throw new InvalidTransformationTypeException(property.PropertyType);
            }

            return transformer;
        }

        public bool CanSerializer(PropertyInfo property)
            => InternalGetTransformer(property) != null;

        public ITransformer? InternalGetTransformer(PropertyInfo property)
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