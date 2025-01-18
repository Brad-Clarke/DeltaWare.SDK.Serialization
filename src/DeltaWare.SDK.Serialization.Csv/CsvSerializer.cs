using System.Reflection;
using System.Runtime.CompilerServices;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Extensions;
using DeltaWare.SDK.Serialization.Csv.Mapping;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Serialization;
using DeltaWare.SDK.Serialization.Csv.Validation;

namespace DeltaWare.SDK.Serialization.Csv
{
    public class CsvSerializer
    {
        private readonly ICsvPropertyMapper _propertyMapper;

        private readonly IPropertySerializer _propertySerializer;

        private readonly ICsvValidator _csvValidator;

        public CsvSerializer(ICsvValidator? csvValidator = null)
        {
            _csvValidator = csvValidator ?? new DefaultCsvValidator();

            _propertySerializer = PropertySerializer.Instance;
            _propertyMapper = new DefaultCsvPropertyMapper(_propertySerializer);
        }

        public async IAsyncEnumerable<T> DeserializeAsync<T>(CsvStreamReader streamReader, bool hasHeader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var csvType = typeof(T);

            IReadOnlyCollection<PropertyMapping> propertyMappings;

            if (hasHeader || csvType.GetCustomAttribute<CsvHeaderRequiredAttribute>() != null)
            {
                var headers = await streamReader.ReadLineAsync(cancellationToken).ToListAsync(cancellationToken);

                propertyMappings = _propertyMapper.CreatePropertyMappings(csvType, headers);
            }
            else
            {
                propertyMappings = _propertyMapper.CreatePropertyMappings(csvType);
            }

            while (!streamReader.EndOfFile)
            {
                yield return await DeserializeLine<T>(propertyMappings, streamReader, cancellationToken);
            }
        }

        private async Task<T> DeserializeLine<T>(IReadOnlyCollection<PropertyMapping> propertyMappings, CsvStreamReader streamReader, CancellationToken cancellationToken = default)
        {
            int index = 0;

            T csvObject = Activator.CreateInstance<T>();

            await foreach (var field in streamReader.ReadLineAsync().WithCancellation(cancellationToken))
            {
                var fieldMapping = propertyMappings.SingleOrDefault(p => p.Index == index);

                index++;

                if (fieldMapping == null || fieldMapping.Ignore)
                {
                    continue;
                }
                
                var fieldValue = _propertySerializer.Deserialize(fieldMapping.Property, field);

                _csvValidator.Validate(fieldMapping.Property, fieldValue);

                fieldMapping.Property.SetValue(csvObject, fieldValue);
            };

            return csvObject;
        }
    }
}
