using DeltaWare.SDK.Serialization.Types.Transformation.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.SDK.Serialization.Types.Transformation
{
    public class TransformerCollection
    {
        private readonly List<ITransformer> _transformers = new List<ITransformer>();

        private readonly List<INullableTransformer> _nullableTransformers = new List<INullableTransformer>();

        public static TransformerCollection DefaultCollection { get; } = new TransformerCollection
        (
            new StringTransformer(),
            new CharTransformer(),
            new BoolTransformer(),
            new ShortTransformer(),
            new IntTransformer(),
            new LongTransformer(),
            new FloatTransformer(),
            new GuidTransformer(),
            new DecimalTransformer(),
            new DateTimeTransformer(),
            new TimeSpanTransformer(),
            new EnumTransformer()
        );

        public TransformerCollection(params ITransformer[] transformers)
        {
            foreach (ITransformer transformer in transformers)
            {
                Add(transformer);
            }
        }

        public void Add(ITransformer transformer)
        {
            if (transformer is INullableTransformer nullableTransformer)
            {
                _nullableTransformers.Add(nullableTransformer);
            }
            else
            {
                _transformers.Add(transformer);
            }
        }

        public bool ContainsTransformer(Type type)
        {
            Type nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                return _nullableTransformers.Any(t => t.CanSerialize(nullableType));
            }

            if (_transformers.Any(t => t.CanSerialize(type)))
            {
                return true;
            }

            return _nullableTransformers.Any(t => t.CanSerialize(type));
        }

        public bool TryGetTransformer(Type type, out ITransformer transformer)
        {
            Type nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                transformer = _nullableTransformers.SingleOrDefault(t => t.CanSerialize(nullableType));
            }
            else
            {
                transformer = _transformers.SingleOrDefault(t => t.CanSerialize(type));

                if (transformer == null)
                {
                    transformer = _nullableTransformers.SingleOrDefault(t => t.CanSerialize(type));
                }
            }

            return transformer != null;
        }
    }
}