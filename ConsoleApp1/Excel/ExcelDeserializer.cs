using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace ConsoleApp1.Excel
{
    public static class ExcelDeserializer
    {
        private static readonly TimeZoneInfo AestTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

        public static IEnumerable<T> Deserialize<T>(Stream stream)
        {
            var mappings = GetPropertyMappings<T>();

            using var reader = ExcelReaderFactory.CreateReader(stream);

            var dataTable = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                // Gets or sets a value indicating whether to set the DataColumn.DataType 
                // property in a second pass.
                UseColumnDataType = true,
                // Gets or sets a callback to determine whether to include the current sheet
                // in the DataSet. Called once per sheet before ConfigureDataTable.
                FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0,
                // Gets or sets a callback to obtain configuration options for a DataTable. 
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    // Gets or sets a value indicating the prefix of generated column names.
                    EmptyColumnNamePrefix = "Column",
                    // Gets or sets a value indicating whether to use a row from the 
                    // data as column names.
                    UseHeaderRow = true,
                    ReadHeaderRow = rowReader =>
                    {
                        rowReader.Read();
                    }
                }
            });

            for (var i = 0; i < dataTable.Tables[0].Rows.Count; i++)
            {
                DataRow? data = dataTable.Tables[0].Rows[i];

                if (data is not null)
                {
                    yield return CreateRowObject<T>(i, data, mappings);
                }
            }
        }

        private static Dictionary<PropertyInfo, int> GetPropertyMappings<T>()
            => typeof(T)
                .GetProperties()
                .Select(p => new { PropertyInfo = p, Attribute = p.GetCustomAttribute<ExcelColumnIndexAttribute>() })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.PropertyInfo, x => x.Attribute!.Index);

        private static T CreateRowObject<T>(int row, DataRow dataRow, Dictionary<PropertyInfo, int> mappings)
        {
            var modelType = typeof(T);

            var item = (T)Activator.CreateInstance(modelType)!;

            modelType.GetProperty("LineNo")?.SetValue(item, row);

            foreach (var (property, columnIndex) in mappings)
            {
                var data = dataRow[columnIndex];

                var propertyValue = GetPropertyValueFromData(property, data);

                property.SetValue(item, propertyValue);
            }

            return item;
        }

        private static object? GetPropertyValueFromData(PropertyInfo property, object? data)
        {
            if (data == null)
            {
                return null;
            }

            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (propertyType == typeof(string))
            {
                return data.ToString()!.Trim();
            }

            if (propertyType == typeof(DateTimeOffset))
            {
                return ConvertToDateTimeOffset(data);
            }

            if (propertyType == typeof(int))
            {
                return int.Parse(data.ToString()!);
            }

            if (propertyType.IsEnum)
            {
                return Enum.Parse(property.PropertyType, data.ToString()!.Trim());
            }

            if (propertyType == typeof(bool))
            {
                return data.ToString() is "Yes" or "True" or "1";
            }

            throw new ArgumentException($"The properties type {property.PropertyType} is not supported. Add support by updating the {nameof(GetPropertyValueFromData)} method.");
        }

        private static DateTimeOffset ConvertToDateTimeOffset(object dateValue)
        {
            if (dateValue is DateTime time)
            {
                return new DateTimeOffset(time);
            }

            var dateString = dateValue.ToString();

            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime) ||
                DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) ||
                DateTime.TryParseExact(dateString, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) ||
                DateTime.TryParseExact(dateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return new DateTimeOffset(dateTime, AestTimeZone.GetUtcOffset(dateTime));
            }

            return new DateTimeOffset();
        }
    }
}
