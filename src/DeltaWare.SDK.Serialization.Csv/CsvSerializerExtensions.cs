using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Writing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv
{
    public static class CsvSerializerExtensions
    {
        #region Record Deserialization

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, string value) where TContainer : class
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.DeserializeRecordsAsync(csvReader, typeof(TContainer)).CastAsync<TContainer>();
        }

        public static TContainer DeserializeRecords<TContainer>(this ICsvSerializer serializer, string value) where TContainer : class
        {
            using CsvReader csvReader = new CsvReader(value);

            return (TContainer)serializer.DeserializeRecords(csvReader, typeof(TContainer));
        }

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, string value) where TContainer : class
        {
            return DeserializeRecordsAsync<TContainer>((ICsvSerializer)serializer, value);
        }

        public static TContainer DeserializeRecords<TContainer>(this CsvSerializer serializer, string value) where TContainer : class
        {
            return DeserializeRecords<TContainer>((ICsvSerializer)serializer, value);
        }

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, Stream stream) where TContainer : class
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.DeserializeRecordsAsync(csvReader, typeof(TContainer)).CastAsync<TContainer>();
        }

        public static TContainer DeserializeRecords<TContainer>(this ICsvSerializer serializer, Stream stream) where TContainer : class
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return (TContainer)serializer.DeserializeRecords(csvReader, typeof(TContainer));
        }

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, Stream stream) where TContainer : class
        {
            return DeserializeRecordsAsync<TContainer>((ICsvSerializer)serializer, stream);
        }

        public static TContainer DeserializeRecords<TContainer>(this CsvSerializer serializer, Stream stream) where TContainer : class
        {
            return DeserializeRecords<TContainer>((ICsvSerializer)serializer, stream);
        }

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, ICsvReader reader) where TContainer : class
        {
            return serializer.DeserializeRecordsAsync(reader, typeof(TContainer)).CastAsync<TContainer>();
        }

        public static TContainer DeserializeRecords<TContainer>(this ICsvSerializer serializer, ICsvReader reader) where TContainer : class
        {
            return (TContainer)serializer.DeserializeRecords(reader, typeof(TContainer));
        }

        public static Task<TContainer> DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, ICsvReader reader) where TContainer : class
        {
            return DeserializeRecordsAsync<TContainer>((ICsvSerializer)serializer, reader);
        }

        public static TContainer DeserializeRecords<TContainer>(this CsvSerializer serializer, ICsvReader reader) where TContainer : class
        {
            return DeserializeRecords<TContainer>((ICsvSerializer)serializer, reader);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, string value, TContainer container) where TContainer : class
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.DeserializeRecordsAsync(csvReader, container);
        }

        public static void DeserializeRecords<TContainer>(this ICsvSerializer serializer, string value, TContainer container) where TContainer : class
        {
            using CsvReader csvReader = new CsvReader(value);

            serializer.DeserializeRecords(csvReader, container);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, string value, TContainer container) where TContainer : class
        {
            return DeserializeRecordsAsync<TContainer>((ICsvSerializer)serializer, value);
        }

        public static void DeserializeRecords<TContainer>(this CsvSerializer serializer, string value, TContainer container) where TContainer : class
        {
            DeserializeRecords<TContainer>((ICsvSerializer)serializer, value);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, Stream stream, TContainer container) where TContainer : class
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.DeserializeRecordsAsync(csvReader, container);
        }

        public static void DeserializeRecords<TContainer>(this ICsvSerializer serializer, Stream stream, TContainer container) where TContainer : class
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            serializer.DeserializeRecords(csvReader, container);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, Stream stream, TContainer container) where TContainer : class
        {
            return DeserializeRecordsAsync((ICsvSerializer)serializer, stream, container);
        }

        public static void DeserializeRecords<TContainer>(this CsvSerializer serializer, Stream stream, TContainer container) where TContainer : class
        {
            DeserializeRecords((ICsvSerializer)serializer, stream, container);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this ICsvSerializer serializer, ICsvReader reader, TContainer container) where TContainer : class
        {
            return serializer.DeserializeRecordsAsync(reader, container);
        }

        public static void DeserializeRecords<TContainer>(this ICsvSerializer serializer, ICsvReader reader, TContainer container) where TContainer : class
        {
            serializer.DeserializeRecords(reader, container);
        }

        public static Task DeserializeRecordsAsync<TContainer>(this CsvSerializer serializer, ICsvReader reader, TContainer container) where TContainer : class
        {
            return DeserializeRecordsAsync((ICsvSerializer)serializer, reader, container);
        }

        public static void DeserializeRecords<TContainer>(this CsvSerializer serializer, ICsvReader reader, TContainer container) where TContainer : class
        {
            DeserializeRecords((ICsvSerializer)serializer, reader, container);
        }

        public static Task<IEnumerable<object>> DeserializeRecordsAsync(this ICsvSerializer serializer, string value, Type[] recordTypes)
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.DeserializeRecordsAsync(csvReader, recordTypes);
        }

        public static IEnumerable<object> DeserializeRecords(this ICsvSerializer serializer, string value, Type[] recordTypes)
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.DeserializeRecords(csvReader, recordTypes);
        }

        public static Task<IEnumerable<object>> DeserializeRecordsAsync(this CsvSerializer serializer, string value, Type[] recordTypes)
        {
            return DeserializeRecordsAsync((ICsvSerializer)serializer, value, recordTypes);
        }

        public static IEnumerable<object> DeserializeRecords(this CsvSerializer serializer, string value, Type[] recordTypes)
        {
            return DeserializeRecords((ICsvSerializer)serializer, value, recordTypes);
        }

        public static Task<IEnumerable<object>> DeserializeRecordsAsync(this ICsvSerializer serializer, Stream stream, Type[] recordTypes)
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.DeserializeRecordsAsync(csvReader, recordTypes);
        }

        public static IEnumerable<object> DeserializeRecords(this ICsvSerializer serializer, Stream stream, Type[] recordTypes)
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.DeserializeRecords(csvReader, recordTypes);
        }

        public static Task<IEnumerable<object>> DeserializeRecordsAsync(this CsvSerializer serializer, Stream stream, Type[] recordTypes)
        {
            return DeserializeRecordsAsync((ICsvSerializer)serializer, stream, recordTypes);
        }

        public static IEnumerable<object> DeserializeRecords(this CsvSerializer serializer, Stream stream, Type[] recordTypes)
        {
            return DeserializeRecords((ICsvSerializer)serializer, stream, recordTypes);
        }

        #endregion

        #region Record Serialziation

        public static async Task<string> SerializeRecordAsync(this ICsvSerializer serializer, IEnumerable<object> lines)
        {
            await using Stream stream = new MemoryStream();

            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            await serializer.SerializeRecordAsync(lines, csvWriter);

            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new(stream);

            return await reader.ReadToEndAsync();
        }

        public static Task<string> SerializeRecordAsync(this CsvSerializer serializer, IEnumerable<object> lines)
        {
            return SerializeRecordAsync((ICsvSerializer)serializer, lines);
        }

        public static Task SerializeRecordAsync(this ICsvSerializer serializer, IEnumerable<object> lines, Stream stream)
        {
            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            return serializer.SerializeRecordAsync(lines, csvWriter);
        }

        public static Task SerializeRecordAsync(this CsvSerializer serializer, IEnumerable<object> lines, Stream stream)
        {
            return SerializeRecordAsync((ICsvSerializer)serializer, lines, stream);
        }

        public static async Task<string> SerializeRecordAsync(this ICsvSerializer serializer, object container)
        {
            await using Stream stream = new MemoryStream();

            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            await serializer.SerializeRecordAsync(container, csvWriter);

            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new(stream);

            return await reader.ReadToEndAsync();
        }

        public static Task<string> SerializeRecordAsync(this CsvSerializer serializer, object container)
        {
            return SerializeRecordAsync((ICsvSerializer)serializer, container);
        }

        public static Task SerializeRecordAsync(this ICsvSerializer serializer, object container, Stream stream)
        {
            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            return serializer.SerializeRecordAsync(container, csvWriter);
        }

        public static Task SerializeRecordAsync(this CsvSerializer serializer, object container, Stream stream)
        {
            return SerializeRecordAsync((ICsvSerializer)serializer, container, stream);
        }

        public static string SerializeRecord(this ICsvSerializer serializer, IEnumerable<object> lines)
        {
            using Stream stream = new MemoryStream();

            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            serializer.SerializeRecord(lines, csvWriter);

            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new(stream);

            return reader.ReadToEnd();
        }

        public static string SerializeRecord(this CsvSerializer serializer, IEnumerable<object> lines)
        {
            return SerializeRecord((ICsvSerializer)serializer, lines);
        }

        public static void SerializeRecord(this ICsvSerializer serializer, IEnumerable<object> lines, Stream stream)
        {
            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer, WriteMode.Record);

            serializer.SerializeRecord(lines, csvWriter);
        }

        public static void SerializeRecord(this CsvSerializer serializer, IEnumerable<object> lines, Stream stream)
        {
            SerializeRecord((ICsvSerializer)serializer, lines, stream);
        }

        #endregion

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="value">The data to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this ICsvSerializer serializer, string value, bool hasHeaders = true) where TSchema : class
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.Deserialize(csvReader, typeof(TSchema), hasHeaders).Cast<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="value">The data to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this CsvSerializer serializer, string value, bool hasHeaders = true) where TSchema : class
        {
            return Deserialize<TSchema>((ICsvSerializer)serializer, value, hasHeaders);
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="value">The data to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this ICsvSerializer serializer, string value, bool hasHeaders = true) where TSchema : class
        {
            using CsvReader csvReader = new CsvReader(value);

            return serializer.DeserializeAsync(csvReader, typeof(TSchema), hasHeaders).CastAsync<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="value">The data to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this CsvSerializer serializer, string value, bool hasHeaders = true) where TSchema : class
        {
            return DeserializeAsync<TSchema>((ICsvSerializer)serializer, value, hasHeaders);
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this ICsvSerializer serializer, Stream stream, bool hasHeaders = true) where TSchema : class
        {
            StreamReader reader = new StreamReader(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.Deserialize(csvReader, typeof(TSchema), hasHeaders).Cast<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this CsvSerializer serializer, Stream stream, bool hasHeaders = true) where TSchema : class
        {
            return Deserialize<TSchema>((ICsvSerializer)serializer, stream, hasHeaders);
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this ICsvSerializer serializer, Stream stream, bool hasHeaders = true) where TSchema : class
        {
            StreamReader reader = new(stream);

            ICsvReader csvReader = new CsvReader(reader);

            return serializer.DeserializeAsync(csvReader, typeof(TSchema), hasHeaders).CastAsync<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this CsvSerializer serializer, Stream stream, bool hasHeaders = true) where TSchema : class
        {
            return DeserializeAsync<TSchema>((ICsvSerializer)serializer, stream, hasHeaders);
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="reader">The reader to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this ICsvSerializer serializer, ICsvReader reader, bool hasHeaders = true) where TSchema : class
        {
            return serializer.Deserialize(reader, typeof(TSchema), hasHeaders).Cast<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="reader">The reader to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static IEnumerable<TSchema> Deserialize<TSchema>(this CsvSerializer serializer, ICsvReader reader, bool hasHeaders = true) where TSchema : class
        {
            return Deserialize<TSchema>((ICsvSerializer)serializer, reader, hasHeaders);
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="reader">The reader to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this ICsvSerializer serializer, ICsvReader reader, bool hasHeaders = true) where TSchema : class
        {
            return serializer.DeserializeAsync(reader, typeof(TSchema), hasHeaders).CastAsync<TSchema>();
        }

        /// <summary>
        /// Deserializes the CSV data into the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TSchema">The <see cref="Type"/> for the data to be deserialized to.</typeparam>
        /// <param name="reader">The reader to be deserialized.</param>
        /// <param name="hasHeaders">Specifies if the CSV contains headers.</param>
        /// <returns>Returns the deserialized data or an empty <see cref="IEnumerable{T}"/> if no data was present.</returns>
        /// <exception cref="CsvSchemaException">Thrown when the Csv Schema is invalid.</exception>
        /// <exception cref="InvalidCsvDataException">Thrown when the Csv Data is invalid.</exception>
        public static Task<IEnumerable<TSchema>> DeserializeAsync<TSchema>(this CsvSerializer serializer, ICsvReader reader, bool hasHeaders = true) where TSchema : class
        {
            return DeserializeAsync<TSchema>((ICsvSerializer)serializer, reader, hasHeaders);
        }

        public static string Serialize<TSchema>(this ICsvSerializer serializer, IEnumerable<TSchema> lines, bool hasHeader = true) where TSchema : class
        {
            using Stream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);

            ICsvWriter csvWriter = new CsvWriter(writer);

            serializer.Serialize(lines, csvWriter, hasHeader);

            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public static string Serialize<TSchema>(this CsvSerializer serializer, IEnumerable<TSchema> lines, bool hasHeader = true) where TSchema : class
        {
            return Serialize((ICsvSerializer)serializer, lines, hasHeader);
        }

        public static void Serialize<TSchema>(this ICsvSerializer serializer, IEnumerable<TSchema> lines, Stream stream, bool hasHeader = true) where TSchema : class
        {
            StreamWriter writer = new StreamWriter(stream);

            ICsvWriter csvWriter = new CsvWriter(writer);

            serializer.Serialize(lines, csvWriter, hasHeader);
        }

        public static void Serialize<TSchema>(this CsvSerializer serializer, IEnumerable<TSchema> lines, Stream stream, bool hasHeader = true) where TSchema : class
        {
            Serialize((ICsvSerializer)serializer, lines, stream, hasHeader);
        }

        public static async Task<string> SerializeAsync<TSchema>(this ICsvSerializer serializer, IEnumerable<TSchema> lines, bool hasHeader = true) where TSchema : class
        {
            await using Stream stream = new MemoryStream();

            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer);

            await serializer.SerializeAsync(lines, csvWriter, hasHeader);

            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new(stream);

            return await reader.ReadToEndAsync();
        }

        public static Task<string> SerializeAsync<TSchema>(this CsvSerializer serializer, IEnumerable<TSchema> lines, bool hasHeader = true) where TSchema : class
        {
            return SerializeAsync((ICsvSerializer)serializer, lines, hasHeader);
        }

        public static Task SerializeAsync<TSchema>(this ICsvSerializer serializer, IEnumerable<TSchema> lines, Stream stream, bool hasHeader = true) where TSchema : class
        {
            StreamWriter writer = new(stream);

            ICsvWriter csvWriter = new CsvWriter(writer);

            return serializer.SerializeAsync(lines, csvWriter, hasHeader);
        }

        public static Task SerializeAsync<TSchema>(this CsvSerializer serializer, IEnumerable<TSchema> lines, Stream stream, bool hasHeader = true) where TSchema : class
        {
            return SerializeAsync((ICsvSerializer)serializer, lines, stream, hasHeader);
        }
    }
}
