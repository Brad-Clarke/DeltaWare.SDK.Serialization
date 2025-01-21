using System;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UseTransformerAttribute<T>() : UseTransformerAttribute(typeof(T)) where T : ITransformer;

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class UseTransformerAttribute(Type transformerType) : Attribute
    {
        public Type TransformerType { get; } = transformerType;
    }
}