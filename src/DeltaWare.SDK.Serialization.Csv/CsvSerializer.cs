using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Extensions;
using DeltaWare.SDK.Serialization.Csv.Mapping;
using DeltaWare.SDK.Serialization.Csv.Options;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Serialization;
using DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Validation;
using DeltaWare.SDK.Serialization.Csv.Writing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv
{
    public class CsvSerializer
    {
        private readonly ICsvPropertyMapper _propertyMapper;

        private readonly IPropertySerializer _propertySerializer;

        private readonly ICsvValidator _csvValidator;

        private readonly ICsvSerializerOptions _options;

        public CsvSerializer(ICsvSerializerOptions? options = null, ICsvValidator? csvValidator = null)
        {
            _options = options ?? new CsvSerializerOptions();
            _csvValidator = csvValidator ?? new DefaultCsvValidator();

            _propertySerializer = new PropertySerializer(_options.FormatProvider);
            _propertyMapper = new DefaultCsvPropertyMapper(_propertySerializer, _options.CaseInsensitiveHeaders);
        }



        public async Task SerializeAsync<T>(IEnumerable<T> rows, CsvStreamWriter streamWriter, bool hasHeader, CancellationToken cancellationToken = default)
        {
            var mappedProperties = _propertyMapper
                .CreatePropertyMappings(typeof(T), false)
                .OrderBy(mp => mp.Index)
                .ToList();

            if (hasHeader)
            {
                await streamWriter.WriteLineAsync(mappedProperties.Select(s => s.HeaderName), cancellationToken);
            }

            foreach (var row in rows)
            {
                var serializedLine = SerializeLine(mappedProperties, row!, cancellationToken);

                await streamWriter.WriteLineAsync(serializedLine, cancellationToken);
            }
        }

        private IEnumerable<string?> SerializeLine(IEnumerable<PropertyMapping> propertyMappings, object rowObject, CancellationToken cancellationToken)
        {
            foreach (var mappedProperty in propertyMappings)
            {
                var fieldObject = mappedProperty.Property.GetValue(rowObject);

                _csvValidator.Validate(mappedProperty.Property, fieldObject);

                yield return _propertySerializer.Serialize(mappedProperty.Property, fieldObject);
            }
        }

        public async IAsyncEnumerable<T> DeserializeAsync<T>(CsvStreamReader streamReader, bool hasHeader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<int, PropertyInfo> propertyMappings = await GetPropertyMappingsAsync<T>(streamReader, hasHeader, cancellationToken);

            while (!streamReader.EndOfFile)
            {
                int index = 0;

                bool receivedData = false;

                T csvObject = Activator.CreateInstance<T>()!;

                await foreach (var field in streamReader.ReadLineAsync().WithCancellation(cancellationToken))
                {
                    receivedData = true;

                    if (propertyMappings.TryGetValue(index, out var fieldMapping))
                    {
                        DeserializeField(field, fieldMapping, csvObject, streamReader.LineNumber);
                    }

                    index++;
                };

                if (!receivedData)
                {
                    continue;
                }

                yield return csvObject;
            }
        }

        private void DeserializeField(string? field, PropertyInfo fieldMapping, object destinationObject, int lineNumber)
        {
            object? fieldValue;

            try
            {
                fieldValue = _propertySerializer.Deserialize(fieldMapping, field);
            }
            catch (TransformationException ex)
            {
                throw CsvSerializationException.FailedToDeserializeField(lineNumber, fieldMapping, ex);
            }

            try
            {
                _csvValidator.Validate(fieldMapping, fieldValue);
            }
            catch (Exception ex)
            {
                throw CsvSerializationException.FieldValidationFailed(lineNumber, fieldMapping, ex);
            }

            fieldMapping.SetValue(destinationObject, fieldValue);
        }

        private async Task<IReadOnlyDictionary<int, PropertyInfo>> GetPropertyMappingsAsync<T>(CsvStreamReader streamReader, bool hasHeader, CancellationToken cancellationToken)
        {
            var csvType = typeof(T);

            if (!hasHeader && csvType.GetCustomAttribute<CsvHeaderRequiredAttribute>() == null)
            {
                return _propertyMapper
                    .CreatePropertyMappings(csvType, true)
                    .ToDictionary(m => m.Index, m => m.Property);
            }

            var headers = await streamReader.ReadLineAsync(cancellationToken).ToListAsync(cancellationToken);

            return _propertyMapper
                .CreatePropertyMappings(csvType, true, headers)
                .ToDictionary(m => m.Index, m => m.Property);

        }
    }
}
