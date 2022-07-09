using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Core.Helpers;
using DeltaWare.SDK.Serialization.Types.Attributes;
using DeltaWare.SDK.Serialization.Types.Exceptions;
using DeltaWare.SDK.Serialization.Types.Transformation;
using DeltaWare.SDK.Serialization.Types.Transformation.Attributes;
using DeltaWare.SDK.Serialization.Types.Transformation.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Types
{
    /// <summary>
    /// Handles serialization of objects.
    /// </summary>
    public class ObjectSerializer : IObjectSerializer
    {
        private readonly IAttributeCache _attributeCache;

        /// <summary>
        /// The Culture to be used during serialization.
        /// </summary>
        /// <remarks>Using the <see cref="UseCultureAttribute"/> will override the culture.</remarks>
        public CultureInfo Culture { get; }

        /// <summary>
        /// A collection of <see cref="ITransformer"/> available to the <see cref="IObjectSerializer"/>.
        /// </summary>
        /// <remarks>Using the <see cref="UseTransformerAttribute"/> will override these transformers.</remarks>
        public TransformerCollection Transformers { get; } = TransformerCollection.DefaultCollection;

        /// <summary>
        /// Creates a new instance of the <see cref="ObjectSerializer"/>.
        /// </summary>
        /// <param name="attributeCache">Overrides the <see cref="IAttributeCache"/>.</param>
        /// <remarks>Uses Australian Culture by Default.</remarks>
        public ObjectSerializer(IAttributeCache attributeCache = null)
        {
            Culture = CultureInfo.GetCultureInfo("en-AU");

            _attributeCache = attributeCache ?? new AttributeCache();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ObjectSerializer"/>.
        /// </summary>
        /// <param name="culture">Overrides the <see cref="CultureInfo"/>.</param>
        /// <param name="attributeCache">Overrides the <see cref="IAttributeCache"/>.</param>
        public ObjectSerializer(CultureInfo culture, IAttributeCache attributeCache = null)
        {
            Culture = culture;

            _attributeCache = attributeCache ?? new AttributeCache();
        }

        public bool CanSerialize(Type type)
        {
            if (_attributeCache.TryGetAttribute(type, out UseTransformerAttribute _))
            {
                return true;
            }

            return Transformers.ContainsTransformer(type);
        }

        public bool CanSerialize(PropertyInfo property)
        {
            if (_attributeCache.TryGetAttribute(property, out UseTransformerAttribute _))
            {
                return true;
            }

            return CanSerialize(property.PropertyType);
        }

        public bool CanSerialize<T>()
        {
            return Transformers.ContainsTransformer(typeof(T));
        }

        public T Deserialize<T>(string value)
        {
            return (T)Deserialize(value, typeof(T));
        }

        public object Deserialize(string value, Type type)
        {
            try
            {
                if (_attributeCache.TryGetAttribute(type, out UseTransformerAttribute transformerOverride))
                {
                    return transformerOverride.TransformToObject(value, type);
                }

                if (!Transformers.TryGetTransformer(type, out ITransformer transformer))
                {
                    throw new InvalidTransformationTypeException(type);
                }

                return transformer.TransformToObject(value, type, Culture);
            }
            catch (Exception e)
            {
                throw new ObjectSerializationException(type, e);
            }
        }

        public object Deserialize(string value, PropertyInfo property)
        {
            try
            {
                if (_attributeCache.TryGetAttribute(property, out UseTransformerAttribute transformerOverride))
                {
                    return transformerOverride.TransformToObject(value, property.PropertyType);
                }

                if (_attributeCache.TryGetAttribute(property.PropertyType, out transformerOverride))
                {
                    return transformerOverride.TransformToObject(value, property.PropertyType);
                }

                if (!Transformers.TryGetTransformer(property.PropertyType, out ITransformer transformer))
                {
                    throw new InvalidTransformationTypeException(property.PropertyType);
                }

                if (_attributeCache.TryGetAttribute(property, out UseCultureAttribute cultureOverride))
                {
                    return transformer.TransformToObject(value, property.PropertyType, cultureOverride.Culture);
                }

                return transformer.TransformToObject(value, property.PropertyType, Culture);
            }
            catch (Exception e)
            {
                throw new ObjectSerializationException(property, e);
            }
        }

        public T Deserialize<T>(Dictionary<string, string> propertyValues) where T : class
        {
            T instance = Activator.CreateInstance<T>();

            PropertyInfo[] properties = typeof(T).GetPublicProperties();

            foreach (PropertyInfo property in properties)
            {
                NameAttribute name = property.GetCustomAttribute<NameAttribute>();

                string propertyName = property.Name;

                if (name != null)
                {
                    propertyName = name.Value;
                }

                object value = null;

                if (propertyValues.TryGetValue(propertyName, out string stringValue))
                {
                    value = Deserialize(stringValue, property);
                }

                RequiredAttribute requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();

                requiredAttribute?.Validate(stringValue, propertyName);

                property.SetValue(instance, value);
            }

            return instance;
        }

        public string Serialize<T>(T value)
        {
            return Serialize(value, typeof(T));
        }

        public string Serialize(object value, Type type)
        {
            try
            {
                if (_attributeCache.TryGetAttribute(type, out UseTransformerAttribute transformerOverride))
                {
                    return transformerOverride.TransformToString(value, type);
                }

                if (!Transformers.TryGetTransformer(type, out ITransformer transformer))
                {
                    throw new InvalidTransformationTypeException(type);
                }

                return transformer.TransformToString(value, type, Culture);
            }
            catch (Exception e)
            {
                throw new ObjectSerializationException(type, e);
            }
        }

        public string Serialize(object value, PropertyInfo property)
        {
            try
            {
                if (_attributeCache.TryGetAttribute(property, out UseTransformerAttribute transformerOverride))
                {
                    return transformerOverride.TransformToString(value, property.PropertyType);
                }

                if (_attributeCache.TryGetAttribute(property.PropertyType, out transformerOverride))
                {
                    return transformerOverride.TransformToString(value, property.PropertyType);
                }

                if (!Transformers.TryGetTransformer(property.PropertyType, out ITransformer transformer))
                {
                    throw new InvalidTransformationTypeException(property.PropertyType);
                }

                if (_attributeCache.TryGetAttribute(property, out UseCultureAttribute cultureOverride))
                {
                    return transformer.TransformToString(value, property.PropertyType, cultureOverride.Culture);
                }

                return transformer.TransformToString(value, property.PropertyType, Culture);
            }
            catch (Exception e)
            {
                throw new ObjectSerializationException(property, e);
            }
        }

        public Dictionary<string, string> SerializeToDictionary<T>(T value) where T : class
        {
            return SerializeToDictionary(value, typeof(T).GetPublicProperties());
        }

        public Dictionary<string, string> SerializeToDictionary<T>(T value, Expression<Func<T, object>> propertySelector) where T : class
        {
            return SerializeToDictionary(value, PropertySelectorHelper.GetSelectedProperties(propertySelector));
        }

        private KeyValuePair<string, string> GetPropertyKeyValuePair<T>(T parentObject, PropertyInfo property)
        {
            RequiredAttribute requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();

            object childObject = property.GetValue(parentObject);

            if (requiredAttribute != null)
            {
                requiredAttribute.Validate(childObject, property.Name);

                if (childObject == null)
                {
                    throw new PropertyRequiredException(property);
                }
            }

            if (childObject == null)
            {
                return new KeyValuePair<string, string>();
            }

            string childString = Serialize(childObject, property);

            property.GetCustomAttribute<MinLengthAttribute>()?.Validate(childString, property.Name);
            property.GetCustomAttribute<MaxLengthAttribute>()?.Validate(childString, property.Name);

            NameAttribute name = property.GetCustomAttribute<NameAttribute>();

            string propertyName;

            if (name == null)
            {
                propertyName = property.Name;
            }
            else
            {
                propertyName = name.Value;
            }

            return new KeyValuePair<string, string>(propertyName, childString);
        }

        private Dictionary<string, string> SerializeToDictionary<T>(T value, IEnumerable<PropertyInfo> properties) where T : class
        {
            Dictionary<string, string> propertyDictionary = new Dictionary<string, string>();

            foreach (PropertyInfo property in properties)
            {
                var propertyValue = GetPropertyKeyValuePair(value, property);

                if (string.IsNullOrWhiteSpace(propertyValue.Value))
                {
                    continue;
                }

                propertyDictionary.Add(propertyValue.Key, propertyValue.Value);
            }

            return propertyDictionary;
        }
    }
};