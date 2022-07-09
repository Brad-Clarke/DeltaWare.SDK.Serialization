using DeltaWare.SDK.Core.Caching;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Header;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Records;
using DeltaWare.SDK.Serialization.Csv.Settings;
using DeltaWare.SDK.Serialization.Csv.Validation;
using DeltaWare.SDK.Serialization.Csv.Writing;
using DeltaWare.SDK.Serialization.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv
{
    public class CsvSerializer : ICsvSerializer
    {
        private readonly IAttributeCache _attributeCache;
        private readonly IHeaderHandler _headerHandler;
        private readonly IObjectSerializer _objectSerializer;
        private readonly ICsvValidator _csvValidator;
        private readonly ICsvSerializerSettings _settings;
        private readonly RecordHelper _recordHelper;

        public CsvSerializer(ICsvSerializerSettings settings = null)
        {
            _settings = settings ?? CsvSerializerSettings.Default;

            _attributeCache = settings?.AttributeCache ?? new AttributeCache();
            _objectSerializer = settings?.Serializer ?? new ObjectSerializer(_attributeCache);
            _csvValidator = settings?.Validator ?? new DefaultCsvValidator(_attributeCache);
            _headerHandler = settings?.HeaderHandler ?? new DefaultHeaderHandler(_attributeCache, _objectSerializer);

            _recordHelper = new RecordHelper(_attributeCache, _settings);
        }

        public CsvSerializer(Action<CsvSerializerSettings> settingsBuilder)
        {
            CsvSerializerSettings settings = CsvSerializerSettings.Default;

            settingsBuilder.Invoke(settings);

            _settings = settings;

            _attributeCache = settings?.AttributeCache ?? new AttributeCache();
            _objectSerializer = settings?.Serializer ?? new ObjectSerializer(_attributeCache);
            _csvValidator = settings?.Validator ?? new DefaultCsvValidator(_attributeCache);
            _headerHandler = settings?.HeaderHandler ?? new DefaultHeaderHandler(_attributeCache, _objectSerializer);

            _recordHelper = new RecordHelper(_attributeCache, _settings);
        }

        #region Deserialize Records

        public async Task<object> DeserializeRecordsAsync(ICsvReader reader, Type containerType)
        {
            object container = Activator.CreateInstance(containerType);

            await DeserializeRecordsAsync(reader, container);

            return container;
        }

        public object DeserializeRecords(ICsvReader reader, Type containerType)
        {
            object container = Activator.CreateInstance(containerType);

            DeserializeRecords(reader, container);

            return container;
        }

        public async Task DeserializeRecordsAsync(ICsvReader reader, object container)
        {
            Dictionary<Type, PropertyInfo> recordSchemas = _recordHelper.GetContainerSchemas(container.GetType());

            IEnumerable<object> records = await DeserializeRecordsAsync(reader, recordSchemas.Keys.ToArray());

            _recordHelper.SetContainerValues(container, records, recordSchemas);
        }

        public void DeserializeRecords(ICsvReader reader, object container)
        {
            Dictionary<Type, PropertyInfo> recordSchemas = _recordHelper.GetContainerSchemas(container.GetType());

            IEnumerable<object> records = DeserializeRecords(reader, recordSchemas.Keys.ToArray());

            _recordHelper.SetContainerValues(container, records, recordSchemas);
        }

        public async Task<IEnumerable<object>> DeserializeRecordsAsync(ICsvReader reader, Type[] recordTypes)
        {
            List<object> deserializedRecords = new List<object>();

            Dictionary<string, Type> recordSchemas = _recordHelper.GetRecordSchemas(recordTypes);

            Dictionary<string, PropertyInfo[]> indexedRecordPropertiesCache = new Dictionary<string, PropertyInfo[]>();

            while (!reader.EndOfStream)
            {
                string[] currentLine = await reader.ReadLineAsync();

                if (TryDeserializeRecord(currentLine, recordSchemas, indexedRecordPropertiesCache, out object deserializeRecord))
                {
                    deserializedRecords.Add(deserializeRecord);
                }
            }

            return deserializedRecords;
        }

        public IEnumerable<object> DeserializeRecords(ICsvReader reader, Type[] recordTypes)
        {
            List<object> deserializedRecords = new List<object>();

            Dictionary<string, Type> recordSchemas = _recordHelper.GetRecordSchemas(recordTypes);

            Dictionary<string, PropertyInfo[]> indexedRecordPropertiesCache = new Dictionary<string, PropertyInfo[]>();

            while (!reader.EndOfStream)
            {
                string[] currentLine = reader.ReadLine();

                if (TryDeserializeRecord(currentLine, recordSchemas, indexedRecordPropertiesCache, out object deserializeRecord))
                {
                    deserializedRecords.Add(deserializeRecord);
                }
            }

            return deserializedRecords;
        }

        #endregion

        #region Serialize Records

        public async Task SerializeRecordAsync(IEnumerable<object> lines, ICsvWriter writer)
        {
            if (writer.Mode != WriteMode.Record)
            {
                throw new InvalidOperationException("Invalid CSV Writer Mode, the mode must be set as Record");
            }

            Dictionary<Type, PropertyInfo[]> indexedRecordProperties = new Dictionary<Type, PropertyInfo[]>();

            foreach (object line in lines)
            {
                Type schemaType = line.GetType();

                if (!indexedRecordProperties.TryGetValue(schemaType, out PropertyInfo[] indexedProperties))
                {
                    indexedProperties = _headerHandler.GetIndexedProperties(schemaType);

                    indexedRecordProperties.Add(schemaType, indexedProperties);
                }

                if (!_attributeCache.TryGetAttribute(schemaType, out RecordKeyAttribute recordType))
                {
                    if (_settings.IgnoreUnknownRecords)
                    {
                        continue;
                    }

                    throw CsvSchemaException.InvalidRecordTypeDeclaration(schemaType);
                }

                await writer.WriteAsync(recordType.Type);

                string[] fields = SerializeLine(line, indexedProperties);

                await writer.WriteLineAsync(fields);
            }

            await writer.FlushAsync();
        }

        public void SerializeRecord(IEnumerable<object> lines, ICsvWriter writer)
        {
            if (writer.Mode != WriteMode.Record)
            {
                throw new InvalidOperationException("Invalid CSV Writer Mode, the mode must be set as Record");
            }

            Dictionary<Type, PropertyInfo[]> indexedRecordProperties = new Dictionary<Type, PropertyInfo[]>();

            foreach (object line in lines)
            {
                Type schemaType = line.GetType();

                if (!indexedRecordProperties.TryGetValue(schemaType, out PropertyInfo[] indexedProperties))
                {
                    indexedProperties = _headerHandler.GetIndexedProperties(schemaType);

                    indexedRecordProperties.Add(schemaType, indexedProperties);
                }

                if (!_attributeCache.TryGetAttribute(schemaType, out RecordKeyAttribute recordType))
                {
                    if (_settings.IgnoreUnknownRecords)
                    {
                        continue;
                    }

                    throw CsvSchemaException.InvalidRecordTypeDeclaration(schemaType);
                }

                writer.Write(recordType.Type);

                string[] fields = SerializeLine(line, indexedProperties);

                writer.WriteLine(fields);
            }

            writer.Flush();
        }

        public Task SerializeRecordAsync(object container, ICsvWriter writer)
        {
            Dictionary<Type, PropertyInfo> recordSchemas = _recordHelper.GetContainerSchemas(container.GetType());

            IEnumerable<object> lines = _recordHelper.GetContainerValues(container, recordSchemas);

            return SerializeRecordAsync(lines, writer);
        }

        public void SerializeRecord(object container, ICsvWriter writer)
        {
            Dictionary<Type, PropertyInfo> recordSchemas = _recordHelper.GetContainerSchemas(container.GetType());

            IEnumerable<object> lines = _recordHelper.GetContainerValues(container, recordSchemas);

            SerializeRecord(lines, writer);
        }

        #endregion

        #region Async Methods



        /// <inheritdoc cref="ICsvSerializer.DeserializeAsync"/>
        public async Task<IEnumerable<object>> DeserializeAsync(ICsvReader reader, Type type, bool hasHeaders = true)
        {
            if (!hasHeaders && _attributeCache.TryGetAttribute(type, out HeaderRequiredAttribute _))
            {
                hasHeaders = true;
            }

            bool firstLine = true;

            PropertyInfo[] indexedProperties = null;

            List<object> deserializedValues = new List<object>();

            while (!reader.EndOfStream)
            {
                string[] currentLine = await reader.ReadLineAsync();

                if (firstLine)
                {
                    firstLine = false;

                    if (hasHeaders)
                    {
                        indexedProperties = _headerHandler.BuildHeaderIndex(type, currentLine.Length, currentLine);

                        continue;
                    }

                    indexedProperties = _headerHandler.BuildHeaderIndex(type, currentLine.Length);
                }

                object deserializedValue = DeserializeLine(currentLine, type, indexedProperties);

                deserializedValues.Add(deserializedValue);
            }

            return deserializedValues;
        }

        /// <inheritdoc cref="ICsvSerializer.SerializeAsync{T}"/>
        public async Task SerializeAsync<T>(IEnumerable<T> lines, ICsvWriter writer, bool hasHeaders = true) where T : class
        {
            PropertyInfo[] indexedProperties = _headerHandler.GetIndexedProperties(typeof(T));

            string[] header = _headerHandler.GetHeaderNames(indexedProperties);

            await writer.WriteLineAsync(header);

            foreach (object line in lines)
            {
                string[] fields = SerializeLine(line, indexedProperties);

                await writer.WriteLineAsync(fields);
            }

            await writer.FlushAsync();
        }

        #endregion

        #region Sync Methods

        /// <inheritdoc cref="ICsvSerializer.Deserialize"/>
        public IEnumerable<object> Deserialize(ICsvReader reader, Type type, bool hasHeaders = true)
        {
            if (!hasHeaders && _attributeCache.TryGetAttribute(type, out HeaderRequiredAttribute _))
            {
                hasHeaders = true;
            }

            bool firstLine = true;

            PropertyInfo[] indexedProperties = null;

            while (!reader.EndOfStream)
            {
                string[] currentLine = reader.ReadLine();

                if (firstLine)
                {
                    firstLine = false;

                    if (hasHeaders)
                    {
                        indexedProperties = _headerHandler.BuildHeaderIndex(type, currentLine.Length, currentLine);

                        continue;
                    }

                    indexedProperties = _headerHandler.BuildHeaderIndex(type, currentLine.Length);
                }

                yield return DeserializeLine(currentLine, type, indexedProperties);
            }
        }

        /// <inheritdoc cref="ICsvSerializer.Serialize{T}"/>
        public void Serialize<T>(IEnumerable<T> lines, ICsvWriter writer, bool hasHeaders = true) where T : class
        {
            PropertyInfo[] propertyIndex = _headerHandler.GetIndexedProperties(typeof(T));

            string[] header = _headerHandler.GetHeaderNames(propertyIndex);

            writer.WriteLine(header);

            foreach (object line in lines)
            {
                string[] fields = SerializeLine(line, propertyIndex);

                writer.WriteLine(fields);
            }

            writer.Flush();
        }

        #endregion

        private object DeserializeLine(string[] line, Type type, PropertyInfo[] indexedProperties)
        {
            object parentObject = Activator.CreateInstance(type);

            for (int j = 0; j < line.Length; j++)
            {
                PropertyInfo property = indexedProperties[j];

                string value = line[j];

                object childObject = _objectSerializer.Deserialize(value, property);

                _csvValidator.Validate(childObject, property);

                property.SetValue(parentObject, childObject);
            }

            return parentObject;
        }

        private string[] SerializeLine(object line, PropertyInfo[] indexedProperties)
        {
            string[] fields = new string[indexedProperties.Length];

            for (int i = 0; i < indexedProperties.Length; i++)
            {
                object childValue = indexedProperties[i].GetValue(line);

                _csvValidator.Validate(childValue, indexedProperties[i]);

                fields[i] = _objectSerializer.Serialize(childValue, indexedProperties[i]);
            }

            return fields;
        }

        private bool TryDeserializeRecord(string[] currentLine, Dictionary<string, Type> recordSchemas, Dictionary<string, PropertyInfo[]> indexedRecordPropertiesCache, out object deserializeRecord)
        {
            deserializeRecord = null;

            string recordType = currentLine[0];

            if (!recordSchemas.TryGetValue(recordType, out Type recordSchema))
            {
                if (_settings.IgnoreUnknownRecords)
                {
                    return false;
                }

                throw CsvSchemaException.UndeclaredRecordTypeEncountered(recordType);
            }

            if (!indexedRecordPropertiesCache.TryGetValue(recordType, out PropertyInfo[] indexedProperties))
            {
                indexedProperties = _headerHandler.BuildHeaderIndex(recordSchema, currentLine.Length - 1);

                indexedRecordPropertiesCache.Add(recordType, indexedProperties);
            }

            deserializeRecord = DeserializeLine(currentLine.Skip(1).ToArray(), recordSchema, indexedProperties);

            return true;
        }
    }
}