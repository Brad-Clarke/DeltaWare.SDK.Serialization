using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Core.Helpers;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Records
{
    internal class RecordHelper
    {
        private readonly IAttributeCache _attributeCache;

        private readonly ICsvSerializerSettings _settings;

        public RecordHelper(IAttributeCache attributeCache, ICsvSerializerSettings settings)
        {
            _attributeCache = attributeCache;
            _settings = settings;
        }

        public Dictionary<string, Type> GetRecordSchemas(Type[] recordTypes)
        {
            Dictionary<string, Type> recordSchemas = new Dictionary<string, Type>();

            foreach (Type recordSchema in recordTypes)
            {
                if (!_attributeCache.TryGetAttribute(recordSchema, out RecordKeyAttribute recordType))
                {
                    throw CsvSchemaException.InvalidRecordTypeDeclaration(recordSchema);
                }

                recordSchemas.Add(recordType.Type, recordSchema);
            }

            return recordSchemas;
        }

        public Dictionary<Type, PropertyInfo> GetContainerSchemas(Type containerType)
        {
            PropertyInfo[] publicProperties = containerType.GetPublicProperties();

            Dictionary<Type, PropertyInfo> recordSchemas = new Dictionary<Type, PropertyInfo>();

            foreach (PropertyInfo property in publicProperties)
            {
                Type propertyType = property.PropertyType;

                if (propertyType.IsGenericType)
                {
                    propertyType = propertyType.GetGenericArguments().First();
                }

                if (!_attributeCache.HasAttribute<RecordKeyAttribute>(propertyType))
                {
                    continue;
                }

                recordSchemas.Add(propertyType, property);
            }

            if (recordSchemas.IsEmpty())
            {
                throw new Exception("No record schemas found for container");
            }

            return recordSchemas;
        }

        public IEnumerable<object> GetContainerValues(object container, Dictionary<Type, PropertyInfo> recordSchemas)
        {
            List<object> lines = new List<object>();

            foreach ((_, PropertyInfo property) in recordSchemas)
            {
                if (property.PropertyType.ImplementsInterface<IList>())
                {
                    IList list = (IList)property.GetValue(container);

                    lines.AddRange(list);
                }
                else
                {
                    object value = property.GetValue(container);

                    lines.Add(value);
                }
            }

            return lines;
        }

        public void SetContainerValues(object container, IEnumerable<object> records, Dictionary<Type, PropertyInfo> recordSchemas)
        {
            IEnumerable<IGrouping<Type, object>> recordTypeGroup = records.GroupBy(r => r.GetType());

            foreach (IGrouping<Type, object> recordType in recordTypeGroup)
            {
                object[] recordValues = recordType.ToArray();

                if (!recordSchemas.TryGetValue(recordType.Key, out PropertyInfo property))
                {
                    if (_settings.IgnoreUnknownRecords)
                    {
                        continue;
                    }

                    throw CsvSchemaException.UndeclaredRecordTypeEncountered(recordType.Key.Name);
                }

                if (property.PropertyType.ImplementsInterface<IList>())
                {
                    IList list = (IList)property.GetValue(container);

                    if (list == null)
                    {
                        list = GenericTypeHelper.CreateGenericList(recordType.Key);

                        property.SetValue(container, list);
                    }

                    list.AddRange(recordValues);
                }
                else if (recordValues.Length != 1)
                {
                    throw CsvSchemaException.DuplicateRecordsForNonCollection(recordType.Key, recordValues.Length);
                }
                else
                {
                    property.SetValue(container, recordValues[0]);
                }
            }
        }
    }
}
