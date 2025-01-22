using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers
{
    public interface ITransformer
    {
        Type Type { get; }

        bool CanSerialize(Type type);

        object? TransformToObject(string? value, IFormatProvider formatProvider);

        string? TransformToString(object? value, IFormatProvider formatProvider);
    }
}