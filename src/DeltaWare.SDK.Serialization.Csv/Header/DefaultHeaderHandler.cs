using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Types;
using DeltaWare.SDK.Serialization.Types.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Header
{
    internal class DefaultHeaderHandler : IHeaderHandler
    {
        private readonly IAttributeCache _attributeCache;

        private readonly IObjectSerializer _objectSerializer;

        public DefaultHeaderHandler(IAttributeCache attributeCache, IObjectSerializer objectSerializer)
        {
            _attributeCache = attributeCache;
            _objectSerializer = objectSerializer;
        }

        public string[] GetHeaderNames(PropertyInfo[] indexedProperties)
        {
            string[] headers = new string[indexedProperties.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                string header;

                if (_attributeCache.TryGetAttribute(indexedProperties[i], out ColumnHeaderAttribute columnHeader))
                {
                    header = columnHeader.HeaderName;
                }
                else
                {
                    header = indexedProperties[i].Name;
                }

                headers[i] = header;
            }

            return headers;
        }

        public PropertyInfo[] BuildHeaderIndex(Type type, int columnCount, string[] headers = null)
        {
            PropertyInfo[] indexedProperties = new PropertyInfo[columnCount];

            if (headers == null)
            {
                BuildHeaderIndex(type, indexedProperties);
            }
            else
            {
                BuildHeaderIndex(type, indexedProperties, headers);
            }

            return indexedProperties;
        }

        public PropertyInfo[] GetIndexedProperties(Type type)
        {
            List<PropertyInfo> properties = new();

            foreach (PropertyInfo property in type.GetPublicProperties())
            {
                if (_attributeCache.TryGetAttribute(property, out DoNotSerialize _))
                {
                    // Skip properties with the DoNotSerialize attribute.
                    continue;
                }

                if (!_objectSerializer.CanSerialize(property))
                {
                    // Skip properties we cannot serialize.
                    continue;
                }

                properties.Add(property);
            }

            PropertyInfo[] indexedProperties = new PropertyInfo[properties.Count];

            Queue<PropertyInfo> nonIndexedProperties = new();

            foreach (PropertyInfo property in properties)
            {
                if (_attributeCache.TryGetAttribute(property, out ColumnIndexAttribute indexAttribute))
                {
                    if (indexedProperties[indexAttribute.Index] != null)
                    {
                        throw CsvSchemaException.DuplicateProperties(indexAttribute.Index, indexedProperties[indexAttribute.Index].Name, property.Name);
                    }

                    indexedProperties[indexAttribute.Index] = property;
                }
                else
                {
                    nonIndexedProperties.Enqueue(property);
                }
            }

            if (nonIndexedProperties.Count == 0)
            {
                return indexedProperties;
            }

            for (int i = 0; i < indexedProperties.Length; i++)
            {
                if (indexedProperties[i] != null)
                {
                    continue;
                }

                if (nonIndexedProperties.TryDequeue(out PropertyInfo property))
                {
                    indexedProperties[i] = property;
                }
                else
                {
                    throw new Exception();
                }
            }

            return indexedProperties;
        }

        /// <summary>
        /// Finds a matching csv header and returns the index.
        /// </summary>
        /// <param name="header">The column header.</param>
        /// <param name="headers">The CSV headers to be searched.</param>
        /// <returns>
        /// Returns the index of the CSV header if they match, -1 if no matching header is found.
        /// </returns>
        protected virtual int GetIndexByColumnName(string header, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (header.Equals(headers[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        #region Build Header Index

        private void BuildHeaderIndex(Type type, PropertyInfo[] indexedProperties, string[] headers)
        {
            foreach (PropertyInfo property in type.GetPublicProperties())
            {
                if (_attributeCache.TryGetAttribute(property, out DoNotSerialize _))
                {
                    // Skip properties with the DoNotSerialize attribute.
                    continue;
                }

                if (!_objectSerializer.CanSerialize(property))
                {
                    // Skip properties we cannot serialize.
                    continue;
                }

                string headerName;

                if (_attributeCache.TryGetAttribute(property, out ColumnHeaderAttribute columnHeader))
                {
                    headerName = columnHeader.HeaderName;
                }
                else
                {
                    headerName = property.Name;
                }

                int index = GetIndexByColumnName(headerName, headers);

                if (index == -1)
                {
                    throw new Exception($"The column name could not be found {headerName}.");
                }

                indexedProperties[index] = property;
            }
        }

        private void BuildHeaderIndex(Type type, PropertyInfo[] indexedProperties)
        {
            Queue<PropertyInfo> nonIndexedProperties = new();

            foreach (PropertyInfo property in type.GetPublicProperties())
            {
                if (_attributeCache.TryGetAttribute(property, out DoNotSerialize _))
                {
                    // Skip properties with the DoNotSerialize attribute.
                    continue;
                }

                if (!_objectSerializer.CanSerialize(property))
                {
                    // Skip properties we cannot serialize.
                    continue;
                }

                if (_attributeCache.TryGetAttribute(property, out ColumnIndexAttribute indexAttribute))
                {
                    if (indexedProperties[indexAttribute.Index] != null)
                    {
                        throw CsvSchemaException.DuplicateProperties(indexAttribute.Index, indexedProperties[indexAttribute.Index].Name, property.Name);
                    }

                    indexedProperties[indexAttribute.Index] = property;
                }
                else
                {
                    nonIndexedProperties.Enqueue(property);
                }
            }

            if (nonIndexedProperties.Count == 0)
            {
                return;
            }

            for (int i = 0; i < indexedProperties.Length; i++)
            {
                if (indexedProperties[i] != null)
                {
                    continue;
                }

                if (nonIndexedProperties.TryDequeue(out PropertyInfo property))
                {
                    indexedProperties[i] = property;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        #endregion Build Header Index
    }
}