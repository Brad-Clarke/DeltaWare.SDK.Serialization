using ConsoleApp1.Excel;
using DeltaWare.SDK.Serialization.Csv;
using DeltaWare.SDK.Serialization.Csv.Attributes;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Reading.Options;
using DeltaWare.SDK.Serialization.Csv.Writing;
using System.Diagnostics;
using static ConsoleApp1.MobileServiceModel;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.ReadKey();

            var items = await ProfileCsv();

            string filePath = @"C:\Users\BradleyClarke\Downloads\test_output.csv";

            var stream = File.Create(filePath);

            CsvStreamWriter write = new CsvStreamWriter(new StreamWriter(stream));

            CsvSerializer serializer = new CsvSerializer();

            Stopwatch stopwatch = Stopwatch.StartNew();

            await serializer.SerializeAsync(items, write, false);

            stopwatch.Stop();

            Console.WriteLine($"CSV Write       | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

            await write.FlushAsync();
            await stream.DisposeAsync();

            Console.ReadKey();
        }

        private static async Task<IReadOnlyList<MobileCustomerModel>> ProfileCsv()
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

            Console.WriteLine($"CSV  Read       | Rows: {items.Count}     | Time: {stopwatch.Elapsed.TotalMilliseconds}");

            await stream.DisposeAsync();

            return items;
        }

        private static async Task ProfileExcel()
        {
            var filePath = @"C:\Users\BradleyClarke\Downloads\test.xlsx";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var stream = File.Open(filePath, FileMode.Open);

            var stopwatch = Stopwatch.StartNew();

            var items = ExcelDeserializer.Deserialize<MobileServiceModel>(stream).ToList();

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

    public class MobileServiceModel
    {
        [CsvIgnore]
        public Ulid Id { get; set; }

        [CsvIgnore]
        public string? PinpointSubUserId { get; set; }

        [CsvIgnore]
        public Ulid? BurpFamilyMemberId { get; set; }

        [CsvIgnore]
        public Ulid? OctaneCustomerId { get; set; }

        [CsvIgnore]
        public int LineNo { get; set; }

        [CsvIndex(2)]
        public int CustomerNumber { get; set; }

        [CsvIndex(3)]
        public string ServiceNumber { get; set; }

        [CsvIndex(4)]
        public DateTimeOffset? DateReleased { get; set; }

        [CsvIndex(7)]
        public DateTimeOffset DateAdded { get; set; }

        [CsvIndex(8)]
        public DateTimeOffset ProRataDate { get; set; }

        [CsvIndex(9)]
        public DateTimeOffset DateProvisioned { get; set; }

        [CsvIndex(10)]
        public DateTimeOffset? DateDisconnected { get; set; }

        [CsvIndex(11)]
        public ServiceStatuses ServiceStatus { get; set; }

        [CsvIndex(12)]
        public string RetailPlanOffer { get; set; }

        [CsvIndex(13)]
        public DateTimeOffset LastRechargeDate { get; set; }

        [CsvIndex(14)]
        public string WholesalePlanOffer { get; set; }

        [CsvIndex(17)]
        public string? IMSI { get; set; }

        [CsvIgnore]
        public int ExpiryDays => this.RetailPlanOffer.StartsWith("365") ? 365 : this.RetailPlanOffer.StartsWith("180") ? 180 : 30;

        [CsvIgnore]
        public bool Active => ServiceStatus == ServiceStatuses.OK;

        [CsvIgnore]
        public bool Stale => ServiceStatus == ServiceStatuses.OK && DateTimeOffset.UtcNow.Subtract(LastRechargeDate).TotalDays <= ExpiryDays;

        public enum ServiceStatuses
        {
            Disconnected = 0,
            OK = 1,
        }

        public class MobileCustomerModel
        {
            [CsvIgnore]
            public Ulid Id { get; set; }

            [CsvIgnore]
            public string? PinpointUserId { get; set; }

            [CsvIgnore]
            public Ulid? GuardianUserId { get; set; }

            [CsvIgnore]
            public int LineNo { get; set; }

            [CsvIndex(0)]
            public int CustomerNumber { get; set; }

            [CsvIgnore]
            public string Company { get; set; }

            [CsvIndex(29)]
            public DateTimeOffset DateAdded { get; set; }

            [CsvIndex(18)]
            public string Address { get; set; }

            [CsvIndex(19)]
            public string Address2 { get; set; }

            [CsvIndex(20)]
            public string Suburb { get; set; }

            [CsvIndex(22)]
            public string State { get; set; }

            [CsvIndex(21)]
            public string PostCode { get; set; }

            [CsvIndex(7)]
            public string ContactPhoneNumber { get; set; }

            [CsvIndex(9)]
            public string ContactMobile { get; set; }

            [CsvIndex(10)]
            public string ContactFax { get; set; }

            [CsvIndex(11)]
            public string ContactEmail { get; set; }

            [CsvIndex(5)]
            public string ContactFirstname { get; set; }

            [CsvIndex(6)]
            public string ContactSurname { get; set; }

            [CsvIndex(36)]
            public string CreditStatus { get; set; }

            [CsvIndex(37)]
            public DateTimeOffset DateUpdated { get; set; }

            [CsvIgnore]
            public List<MobileServiceModel> Services { get; } = new();

            [CsvIndex(40)]
            public string MarketingOptOut { get; set; }
        }
    }
}
