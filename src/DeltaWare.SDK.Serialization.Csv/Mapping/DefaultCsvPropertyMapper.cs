﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Extensions;
using DeltaWare.SDK.Serialization.Csv.Serialization;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    internal sealed class DefaultCsvPropertyMapper(IPropertySerializer propertySerializer) : ICsvPropertyMapper
    {
        public IReadOnlyCollection<PropertyMapping> CreatePropertyMappings(Type type, bool requiresSetter, IReadOnlyList<string?>? csvHeaders = null)
        {
            IEnumerable<PropertyMapping> mappingEnumeration;

            if (csvHeaders == null)
            {
                mappingEnumeration = GeneratePropertyMapByIndex(type, requiresSetter);
            }
            else
            {
                mappingEnumeration = GeneratePropertyMapByHeader(type, requiresSetter, csvHeaders);
            }

            var mappings = mappingEnumeration.ToList();

            ValidateMappings(mappings);

            return mappings;
        }

        private void ValidateMappings(IEnumerable<PropertyMapping> mappings)
        {
            var invalidMappings = mappings
                .GroupBy(m => m.Index)
                .Where(m => m.Count() > 1)
                .ToDictionary(m => m.Key, m => m.ToList());

            if (invalidMappings.Count == 0)
            {
                return;
            }

            List<CsvSchemaException> innerExceptions = new List<CsvSchemaException>();

            foreach (var (index, duplicateMappings) in invalidMappings)
            {
                innerExceptions.Add(CsvSchemaException.MultiplePropertiesMappedToSameIndex(index, duplicateMappings[0].Property, duplicateMappings[1].Property));
            }

            throw CsvSchemaException.MultiplePropertiesMappedToSameIndex(innerExceptions);
        }

        private IEnumerable<PropertyMapping> GeneratePropertyMapByHeader(Type type, bool requiresSetter, IReadOnlyList<string?> csvHeaders)
        {
            var properties = GetMappableProperties(type, requiresSetter).Select(ToHeaderNameWithProperty).ToDictionary();

            if (properties.Count > csvHeaders.Count)
            {
                throw CsvSchemaException.HeaderPropertyCountMismatch(csvHeaders.Count, properties.Count);
            }

            foreach (var (targetHeader, property) in properties)
            {
                if (!TryGetHeaderIndex(csvHeaders, targetHeader, out int index))
                {
                    throw CsvSchemaException.PropertyCouldNotBeMappedToHeader(property, targetHeader);
                }

                yield return new PropertyMapping(property, index);
            }
        }
        
        private static bool TryGetHeaderIndex(IEnumerable<string?> headers, string targetHeader, out int index)
        {
            index = 0;

            foreach (var header in headers)
            {
                if (header == targetHeader)
                {
                    return true;
                }

                index++;
            }

            index = -1;

            return false;
        }
        
        private IEnumerable<PropertyMapping> GeneratePropertyMapByIndex(Type type, bool requireSetters)
        {
            var properties = GetMappableProperties(type, requireSetters).ToArray();

            MappingStrategy mappingStrategy = MappingStrategy.Uninitialized;

            for (int i = 0; i < properties.Length; i++)
            {
                var indexAttribute = properties[i].GetCustomAttribute<CsvIndexAttribute>();

                if (indexAttribute == null)
                {
                    if (mappingStrategy == MappingStrategy.AttributeControlled)
                    {
                        throw CsvSchemaException.InvalidPropertyMappingStrategy(properties[i], "Declaration Order", "Attribute Controller");
                    }

                    mappingStrategy = MappingStrategy.DeclarationOrder;
                    
                    yield return new PropertyMapping(properties[i], i);
                }
                else
                {
                    if (mappingStrategy == MappingStrategy.DeclarationOrder)
                    {
                        throw CsvSchemaException.InvalidPropertyMappingStrategy(properties[i], "Attribute Controller", "Declaration Order");
                    }

                    mappingStrategy = MappingStrategy.AttributeControlled;
                    
                    yield return new PropertyMapping(properties[i], indexAttribute.Index);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetMappableProperties(Type type, bool requiresSetter)
            => type
                .GetPublicProperties()
                .Where(p => SupportedPropertiesFilter(p, requiresSetter));

        private bool SupportedPropertiesFilter(PropertyInfo property, bool requiresSetter)
        {
            if (requiresSetter && property.GetSetMethod() == null)
            {
                return false;
            }

            if (property.GetCustomAttribute<CsvIgnoreAttribute>() != null)
            {
                return false;
            }

            if (!propertySerializer.CanSerializer(property))
            {
                throw CsvSchemaException.UnsupportedPropertyType(property);
            }

            return true;
        }

        private static KeyValuePair<string, PropertyInfo> ToHeaderNameWithProperty(PropertyInfo property)
        {
            var header = property.GetCustomAttribute<CsvHeaderAttribute>();

            if (header != null)
            {
                return new KeyValuePair<string, PropertyInfo>(header.Name, property);
            }

            return new KeyValuePair<string, PropertyInfo>(property.Name, property);
        }

        private enum MappingStrategy
        {
            Uninitialized,
            DeclarationOrder,
            AttributeControlled
        }
    }
}
