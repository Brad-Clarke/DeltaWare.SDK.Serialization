using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Types
{
    public interface IObjectSerializer
    {
        CultureInfo Culture { get; }

        bool CanSerialize(Type type);

        bool CanSerialize(PropertyInfo property);

        bool CanSerialize<T>();

        public T Deserialize<T>(string value);

        object Deserialize(string value, Type type);

        object Deserialize(string value, PropertyInfo property);

        T Deserialize<T>(Dictionary<string, string> propertyValues) where T : class;

        string Serialize<T>(T value);

        string Serialize(object value, Type type);

        string Serialize(object value, PropertyInfo property);

        Dictionary<string, string> SerializeToDictionary<T>(T value) where T : class;

        Dictionary<string, string> SerializeToDictionary<T>(T value, Expression<Func<T, object>> propertySelector) where T : class;
    }
}