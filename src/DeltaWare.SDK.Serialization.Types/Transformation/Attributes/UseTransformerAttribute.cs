using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UseTransformerAttribute : Attribute, ITransformer
    {
        public ITransformer Transformer { get; }

        public Type Type { get; }

        public UseTransformerAttribute(Type transformer)
        {
            if (transformer == null)
            {
                throw new ArgumentNullException(nameof(transformer));
            }

            if (!transformer.IsClass)
            {
                throw new ArgumentException($"{transformer.FullName} must be a class");
            }

            if (!transformer.ImplementsInterface<ITransformer>())
            {
                throw new ArgumentException($"{transformer.FullName} must implement {nameof(ITransformer)}");
            }

            Transformer = (ITransformer)Activator.CreateInstance(transformer);
            Type = Transformer.Type;
        }

        public bool CanSerialize(Type type)
        {
            return Transformer.CanSerialize(type);
        }

        /// <inheritdoc cref="ITransformer.TransformToObject"/>
        public object TransformToObject(string value, Type toType, CultureInfo culture = null)
        {
            return Transformer.TransformToObject(value, toType);
        }

        public string TransformToString(object value, Type type, CultureInfo culture = null)
        {
            return Transformer.TransformToString(value, type);
        }
    }
}