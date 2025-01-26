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

        private readonly ICsvSerializerOptions _options;

        public CsvSerializer(ICsvSerializerOptions? options = null)
        {
            _options = options ?? new CsvSerializerOptions();

            _propertyMapper = new DefaultCsvPropertyMapper(new TransformerProvider(_options.FormatProvider), _options.FormatProvider, _options.CaseInsensitiveHeaders);
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

                mappedProperty.PropertyValidator.Validate(fieldObject);

                yield return mappedProperty.Transformer.TransformToString(fieldObject, mappedProperty.FormatProvider);
            }
        }

        public async IAsyncEnumerable<T> DeserializeAsync<T>(CsvStreamReader streamReader, bool hasHeader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<int, PropertyMapping> propertyMappings = await GetPropertyMappingsAsync<T>(streamReader, hasHeader, cancellationToken);

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

        private void DeserializeField(string? field, PropertyMapping fieldMapping, object destinationObject, int lineNumber)
        {
            object? fieldValue;

            try
            {
                fieldValue = fieldMapping.Transformer.TransformToObject(field, fieldMapping.FormatProvider);
            }
            catch (TransformationException ex)
            {
                throw CsvSerializationException.FailedToDeserializeField(lineNumber, fieldMapping.Property, ex);
            }

            try
            {
                fieldMapping.PropertyValidator.Validate(fieldValue);
            }
            catch (Exception ex)
            {
                throw CsvSerializationException.FieldValidationFailed(lineNumber, fieldMapping.Property, ex);
            }

            fieldMapping.Property.SetValue(destinationObject, fieldValue);
        }

        private async Task<IReadOnlyDictionary<int, PropertyMapping>> GetPropertyMappingsAsync<T>(CsvStreamReader streamReader, bool hasHeader, CancellationToken cancellationToken)
        {
            var csvType = typeof(T);

            if (!hasHeader && csvType.GetCustomAttribute<CsvHeaderRequiredAttribute>() == null)
            {
                return _propertyMapper
                    .CreatePropertyMappings(csvType, true)
                    .ToDictionary(m => m.Index, m => m);
            }

            var headers = await streamReader.ReadLineAsync(cancellationToken).ToListAsync(cancellationToken);

            return _propertyMapper
                .CreatePropertyMappings(csvType, true, headers)
                .ToDictionary(m => m.Index, m => m);

        }
    }
}
