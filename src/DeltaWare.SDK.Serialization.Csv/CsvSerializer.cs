using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
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
            IReadOnlyCollection<PropertyMapping> propertyMappings = await GetPropertyMappings<T>(streamReader, hasHeader, cancellationToken);
            
            while (!streamReader.EndOfFile)
            {
                int index = 0;

                bool receivedData = false;

                T csvObject = Activator.CreateInstance<T>();
                
                await foreach (var field in streamReader.ReadLineAsync().WithCancellation(cancellationToken))
                {
                    receivedData = true;

                    var fieldMapping = propertyMappings.SingleOrDefault(p => p.Index == index);

                    index++;

                    if (fieldMapping == null)
                    {
                        continue;
                    }

                    var fieldValue = _propertySerializer.Deserialize(fieldMapping.Property, field);

                    _csvValidator.Validate(fieldMapping.Property, fieldValue);

                    fieldMapping.Property.SetValue(csvObject, fieldValue);
                };

                if (!receivedData)
                {
                    continue;
                }

                yield return csvObject;
            }
        }

        private async Task<IReadOnlyCollection<PropertyMapping>> GetPropertyMappings<T>(CsvStreamReader streamReader, bool hasHeader, CancellationToken cancellationToken)
        {
            var csvType = typeof(T);

            if (hasHeader || csvType.GetCustomAttribute<CsvHeaderRequiredAttribute>() != null)
            {
                var headers = await streamReader.ReadLineAsync(cancellationToken).ToListAsync(cancellationToken);

                return _propertyMapper.CreatePropertyMappings(csvType, true, headers);
            }

            return _propertyMapper.CreatePropertyMappings(csvType, true);
        }
    }
}
