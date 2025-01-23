using CsvHelper.Configuration.Attributes;
using DeltaWare.SDK.Serialization.Csv;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Reading.Options;
using DeltaWare.SDK.Serialization.Csv.Writing;
using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.ReadKey();

            await ProfileCsv();
            await ProfileCsvHelper();
            await ProfileCsv();
            await ProfileCsvHelper();
            await ProfileCsv();
            await ProfileCsvHelper();
            await ProfileCsv();
            await ProfileCsvHelper();

            Console.ReadKey();
        }

        private static async Task ProfileCsv()
        {
            string filePath = @"C:\Users\BradleyClarke\Downloads\test_big.csv";

            var stream = File.Open(filePath, FileMode.Open);

            CsvStreamReader reader = new CsvStreamReader(new StreamReader(stream), new CsvReaderOptions
            {
                BufferSize = 4096,
                TrimFields = true
            });

            CsvSerializer serializer = new CsvSerializer();

            Stopwatch stopwatch = Stopwatch.StartNew();

            var items = await serializer
                .DeserializeAsync<MobileCustomerModel>(reader, false)
                .ToListAsync();

            stopwatch.Stop();

            Console.WriteLine($"CSV              | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

            await stream.DisposeAsync();
        }

        private static async Task ProfileCsvHelper()
        {
            string filePath = @"C:\Users\BradleyClarke\Downloads\test_big.csv";

            var stream = File.Open(filePath, FileMode.Open);

            var config = CsvConfiguration.FromAttributes<MobileCustomerModel>();

            CsvReader reader = new CsvReader(new StreamReader(stream), config);

            Stopwatch stopwatch = Stopwatch.StartNew();

            var items = await reader.GetRecordsAsync<MobileCustomerModel>().ToListAsync();

            stopwatch.Stop();

            Console.WriteLine($"CSV Helper       | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

            await stream.DisposeAsync();
        }
    }

    public static class Temp
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            List<T> items = new List<T>();

            await foreach (var row in enumerable)
            {
                items.Add(row);
            }

            return items;
        }
    }

    public class MobileCustomerModel
    {
        [Ignore]
        [CsvIgnore]
        public Ulid Id { get; set; }

        [Ignore]
        [CsvIgnore]
        public string? PinpointUserId { get; set; }

        [Ignore]
        [CsvIgnore]
        public Ulid? GuardianUserId { get; set; }

        [Ignore]
        [CsvIgnore]
        public int LineNo { get; set; }

        [Index(0)]
        [CsvIndex(0)]
        public int CustomerNumber { get; set; }

        [Ignore]
        [CsvIgnore]
        public string Company { get; set; }

        [Index(29)]
        [CsvIndex(29)]
        public DateTimeOffset DateAdded { get; set; }

        [Index(18)]
        [CsvIndex(18)]
        public string Address { get; set; }

        [Index(19)]
        [CsvIndex(19)]
        public string Address2 { get; set; }

        [Index(20)]
        [CsvIndex(20)]
        public string Suburb { get; set; }

        [Index(22)]
        [CsvIndex(22)]
        public string State { get; set; }

        [Index(21)]
        [CsvIndex(21)]
        public string PostCode { get; set; }

        [Index(7)]
        [CsvIndex(7)]
        public string ContactPhoneNumber { get; set; }

        [Index(9)]
        [CsvIndex(9)]
        public string ContactMobile { get; set; }

        [Index(10)]
        [CsvIndex(10)]
        public string ContactFax { get; set; }

        [Index(11)]
        [CsvIndex(11)]
        public string ContactEmail { get; set; }

        [Index(5)]
        [CsvIndex(5)]
        public string ContactFirstname { get; set; }

        [Index(6)]
        [CsvIndex(6)]
        public string ContactSurname { get; set; }

        [Index(36)]
        [CsvIndex(36)]
        public string CreditStatus { get; set; }

        [Index(37)]
        [CsvIndex(37)]
        public DateTimeOffset DateUpdated { get; set; }

        [Index(40)]
        [CsvIndex(40)]
        public string MarketingOptOut { get; set; }
    }
}
