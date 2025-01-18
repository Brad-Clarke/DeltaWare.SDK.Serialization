using ConsoleApp1.Excel;
using System.Diagnostics;
using DeltaWare.SDK.Serialization.Csv;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Reading.Options;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ProfileCsv();
        }

        private static async Task ProfileCsv()
        {
            string filePath = @"C:\Users\BradleyClarke\Downloads\test.csv";

            var stream = File.Open(filePath, FileMode.Open);

            CsvStreamReader reader = new CsvStreamReader(new StreamReader(stream), new CsvReaderOptions
            {
                BufferSize = 4096
            });

            var temp = await reader.ReadLineAsync().ToListAsync();
            var temp2 = await reader.ReadLineAsync().ToListAsync();
            var temp3 = await reader.ReadLineAsync().ToListAsync();
            var temp4 = await reader.ReadLineAsync().ToListAsync();

            CsvSerializer serializer = new CsvSerializer();

            Stopwatch stopwatch = Stopwatch.StartNew();

            var items = await serializer
                .DeserializeAsync<DetailedMobileServices>(reader, false)
                .ToListAsync();

            stopwatch.Stop();

            Console.WriteLine($"CSV       | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

            await stream.DisposeAsync();
        }

        private static async Task ProfileExcel()
        {
            var filePath = @"C:\Users\BradleyClarke\Downloads\test.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var stream = File.Open(filePath, FileMode.Open);

            var stopwatch = Stopwatch.StartNew();

            var items = ExcelDeserializer.Deserialize<DetailedMobileServices>(stream).ToList();

            stopwatch.Stop();

            Console.WriteLine($"Excel     | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

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

    public class DetailedMobileServices
    {
        [ExcelColumnIndex(2)]
        [CsvIndex(2)]
        public int CustomerNumber { get; set; }

        [ExcelColumnIndex(3)]
        [CsvIndex(3)]
        public string ServiceNumber { get; set; }

        [ExcelColumnIndex(4)]
        [CsvIndex(4)]
        public DateTimeOffset? DateReleased { get; set; }

        [ExcelColumnIndex(7)]
        [CsvIndex(7)]
        public DateTimeOffset DateAdded { get; set; }

        [ExcelColumnIndex(8)]
        [CsvIndex(8)]
        public DateTimeOffset ProRataDate { get; set; }

        [ExcelColumnIndex(9)]
        [CsvIndex(9)]
        public DateTimeOffset DateProvisioned { get; set; }

        [ExcelColumnIndex(10)]
        [CsvIndex(10)]
        public DateTimeOffset? DateDisconnected { get; set; }

        [ExcelColumnIndex(12)]
        [CsvIndex(12)]
        public string RetailPlanOffer { get; set; }

        [ExcelColumnIndex(13)]
        [CsvIndex(13)]
        public DateTimeOffset LastRechargeDate { get; set; }

        [ExcelColumnIndex(14)]
        [CsvIndex(14)]
        public string WholesalePlanOffer { get; set; }

        [ExcelColumnIndex(16)]
        [CsvIndex(16)]
        public string? IMSI { get; set; }
    }
}
