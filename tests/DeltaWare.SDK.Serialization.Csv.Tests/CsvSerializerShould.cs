using DeltaWare.SDK.Serialization.Csv.Tests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using HeaderRecord = DeltaWare.SDK.Serialization.Csv.Tests.Models.HeaderRecord;

namespace DeltaWare.SDK.Serialization.Csv.Tests
{
    public class CsvSerializerShould
    {
        private static SemaphoreSlim _textRecordFileSemaphore = new SemaphoreSlim(1, 1);

        [Fact]
        public async Task DeserializeRecordContainerAsync()
        {
            CsvSerializer serializer = new();

            RecordContainer recordContainer;

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    recordContainer = await serializer.DeserializeRecordsAsync<RecordContainer>(stream);
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            recordContainer.Users.Count.ShouldBe(3);
            recordContainer.Orders.Count.ShouldBe(2);
            recordContainer.Transactions.Count.ShouldBe(2);
            recordContainer.Products.Count.ShouldBe(0);

            recordContainer.Header.Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            recordContainer.Header.GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            recordContainer.Users[0].Id.ShouldBe(5);
            recordContainer.Users[0].FirstName.ShouldBe("John");
            recordContainer.Users[0].LastName.ShouldBe("Doe");
            recordContainer.Users[0].BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            recordContainer.Users[1].Id.ShouldBe(172);
            recordContainer.Users[1].FirstName.ShouldBe("Debra");
            recordContainer.Users[1].LastName.ShouldBe("Thompson");
            recordContainer.Users[1].BirthDate.ShouldBeNull();

            recordContainer.Users[2].Id.ShouldBe(59);
            recordContainer.Users[2].FirstName.ShouldBe("Grant");
            recordContainer.Users[2].LastName.ShouldBe("Burk");
            recordContainer.Users[2].BirthDate.ShouldBe(new DateTime(1985, 11, 15));

            recordContainer.Orders[0].OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            recordContainer.Orders[0].Date.ShouldBe(new DateTime(2020, 07, 24));
            recordContainer.Orders[0].Amount.ShouldBe(516);

            recordContainer.Orders[1].OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            recordContainer.Orders[1].Date.ShouldBe(new DateTime(2021, 01, 15));
            recordContainer.Orders[1].Amount.ShouldBe(12572.15m);

            recordContainer.Transactions[0].TransactionId.ShouldBe("553245234");
            recordContainer.Transactions[0].Paid.ShouldBe(true);

            recordContainer.Transactions[1].TransactionId.ShouldBe("3482739487");
            recordContainer.Transactions[1].Paid.ShouldBe(false);
        }

        [Fact]
        public async Task SerializeRecordContainerAsync()
        {
            CsvSerializer serializer = new();

            RecordContainer recordContainer;

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    recordContainer = await serializer.DeserializeRecordsAsync<RecordContainer>(stream);
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            recordContainer.Users.Count.ShouldBe(3);
            recordContainer.Orders.Count.ShouldBe(2);
            recordContainer.Transactions.Count.ShouldBe(2);
            recordContainer.Products.Count.ShouldBe(0);

            recordContainer.Header.Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            recordContainer.Header.GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            recordContainer.Users[0].Id.ShouldBe(5);
            recordContainer.Users[0].FirstName.ShouldBe("John");
            recordContainer.Users[0].LastName.ShouldBe("Doe");
            recordContainer.Users[0].BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            recordContainer.Users[1].Id.ShouldBe(172);
            recordContainer.Users[1].FirstName.ShouldBe("Debra");
            recordContainer.Users[1].LastName.ShouldBe("Thompson");
            recordContainer.Users[1].BirthDate.ShouldBeNull();

            recordContainer.Users[2].Id.ShouldBe(59);
            recordContainer.Users[2].FirstName.ShouldBe("Grant");
            recordContainer.Users[2].LastName.ShouldBe("Burk");
            recordContainer.Users[2].BirthDate.ShouldBe(new DateTime(1985, 11, 15));

            recordContainer.Orders[0].OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            recordContainer.Orders[0].Date.ShouldBe(new DateTime(2020, 07, 24));
            recordContainer.Orders[0].Amount.ShouldBe(516);

            recordContainer.Orders[1].OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            recordContainer.Orders[1].Date.ShouldBe(new DateTime(2021, 01, 15));
            recordContainer.Orders[1].Amount.ShouldBe(12572.15m);

            recordContainer.Transactions[0].TransactionId.ShouldBe("553245234");
            recordContainer.Transactions[0].Paid.ShouldBe(true);

            recordContainer.Transactions[1].TransactionId.ShouldBe("3482739487");
            recordContainer.Transactions[1].Paid.ShouldBe(false);

            Stream memoryStream = new MemoryStream();

            await serializer.SerializeRecordAsync(recordContainer, memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            recordContainer = await serializer.DeserializeRecordsAsync<RecordContainer>(memoryStream);

            recordContainer.Users.Count.ShouldBe(3);
            recordContainer.Orders.Count.ShouldBe(2);
            recordContainer.Transactions.Count.ShouldBe(2);
            recordContainer.Products.Count.ShouldBe(0);

            recordContainer.Header.Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            recordContainer.Header.GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            recordContainer.Users[0].Id.ShouldBe(5);
            recordContainer.Users[0].FirstName.ShouldBe("John");
            recordContainer.Users[0].LastName.ShouldBe("Doe");
            recordContainer.Users[0].BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            recordContainer.Users[1].Id.ShouldBe(172);
            recordContainer.Users[1].FirstName.ShouldBe("Debra");
            recordContainer.Users[1].LastName.ShouldBe("Thompson");
            recordContainer.Users[1].BirthDate.ShouldBeNull();

            recordContainer.Users[2].Id.ShouldBe(59);
            recordContainer.Users[2].FirstName.ShouldBe("Grant");
            recordContainer.Users[2].LastName.ShouldBe("Burk");
            recordContainer.Users[2].BirthDate.ShouldBe(new DateTime(1985, 11, 15));

            recordContainer.Orders[0].OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            recordContainer.Orders[0].Date.ShouldBe(new DateTime(2020, 07, 24));
            recordContainer.Orders[0].Amount.ShouldBe(516);

            recordContainer.Orders[1].OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            recordContainer.Orders[1].Date.ShouldBe(new DateTime(2021, 01, 15));
            recordContainer.Orders[1].Amount.ShouldBe(12572.15m);

            recordContainer.Transactions[0].TransactionId.ShouldBe("553245234");
            recordContainer.Transactions[0].Paid.ShouldBe(true);

            recordContainer.Transactions[1].TransactionId.ShouldBe("3482739487");
            recordContainer.Transactions[1].Paid.ShouldBe(false);
        }

        [Fact]
        public async Task DeserializeRecordContainerInstanceAsync()
        {
            CsvSerializer serializer = new();

            RecordContainer recordContainer = new();

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                await using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    await serializer.DeserializeRecordsAsync(stream, recordContainer);
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            recordContainer.Users.Count.ShouldBe(3);
            recordContainer.Orders.Count.ShouldBe(2);
            recordContainer.Transactions.Count.ShouldBe(2);
            recordContainer.Products.Count.ShouldBe(0);

            recordContainer.Header.Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            recordContainer.Header.GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            recordContainer.Users[0].Id.ShouldBe(5);
            recordContainer.Users[0].FirstName.ShouldBe("John");
            recordContainer.Users[0].LastName.ShouldBe("Doe");
            recordContainer.Users[0].BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            recordContainer.Users[1].Id.ShouldBe(172);
            recordContainer.Users[1].FirstName.ShouldBe("Debra");
            recordContainer.Users[1].LastName.ShouldBe("Thompson");
            recordContainer.Users[1].BirthDate.ShouldBeNull();

            recordContainer.Users[2].Id.ShouldBe(59);
            recordContainer.Users[2].FirstName.ShouldBe("Grant");
            recordContainer.Users[2].LastName.ShouldBe("Burk");
            recordContainer.Users[2].BirthDate.ShouldBe(new DateTime(1985, 11, 15));

            recordContainer.Orders[0].OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            recordContainer.Orders[0].Date.ShouldBe(new DateTime(2020, 07, 24));
            recordContainer.Orders[0].Amount.ShouldBe(516);

            recordContainer.Orders[1].OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            recordContainer.Orders[1].Date.ShouldBe(new DateTime(2021, 01, 15));
            recordContainer.Orders[1].Amount.ShouldBe(12572.15m);

            recordContainer.Transactions[0].TransactionId.ShouldBe("553245234");
            recordContainer.Transactions[0].Paid.ShouldBe(true);

            recordContainer.Transactions[1].TransactionId.ShouldBe("3482739487");
            recordContainer.Transactions[1].Paid.ShouldBe(false);

        }

        [Fact]
        public async Task DeserializeRecordsAsync()
        {
            CsvSerializer serializer = new();

            List<object> records;

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                await using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    records = await serializer.DeserializeRecordsAsync(stream, new[] { typeof(HeaderRecord), typeof(UserRecord), typeof(OrderRecord), typeof(TransactionRecord) }).ToListAsync();
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            records.Count.ShouldBe(8);

            records[0].GetType().ShouldBe(typeof(HeaderRecord));
            ((HeaderRecord)records[0]).Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            ((HeaderRecord)records[0]).GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            records[1].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[1]).Id.ShouldBe(5);
            ((UserRecord)records[1]).FirstName.ShouldBe("John");
            ((UserRecord)records[1]).LastName.ShouldBe("Doe");
            ((UserRecord)records[1]).BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            records[2].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[2]).Id.ShouldBe(172);
            ((UserRecord)records[2]).FirstName.ShouldBe("Debra");
            ((UserRecord)records[2]).LastName.ShouldBe("Thompson");
            ((UserRecord)records[2]).BirthDate.ShouldBeNull();

            records[6].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[6]).Id.ShouldBe(59);
            ((UserRecord)records[6]).FirstName.ShouldBe("Grant");
            ((UserRecord)records[6]).LastName.ShouldBe("Burk");
            ((UserRecord)records[6]).BirthDate.ShouldBe(new DateTime(1985, 11, 15));


            records[3].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[3]).OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            ((OrderRecord)records[3]).Date.ShouldBe(new DateTime(2020, 07, 24));
            ((OrderRecord)records[3]).Amount.ShouldBe(516);

            records[5].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[5]).OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            ((OrderRecord)records[5]).Date.ShouldBe(new DateTime(2021, 01, 15));
            ((OrderRecord)records[5]).Amount.ShouldBe(12572.15m);


            records[4].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[4]).TransactionId.ShouldBe("553245234");
            ((TransactionRecord)records[4]).Paid.ShouldBe(true);

            records[7].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[7]).TransactionId.ShouldBe("3482739487");
            ((TransactionRecord)records[7]).Paid.ShouldBe(false);

        }

        [Fact]
        public async Task SerializeRecordsAsync()
        {
            CsvSerializer serializer = new();

            List<object> records;

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                await using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    records = await serializer.DeserializeRecordsAsync(stream, new[] { typeof(HeaderRecord), typeof(UserRecord), typeof(OrderRecord), typeof(TransactionRecord) }).ToListAsync();
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            records.Count.ShouldBe(8);

            records[0].GetType().ShouldBe(typeof(HeaderRecord));
            ((HeaderRecord)records[0]).Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            ((HeaderRecord)records[0]).GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            records[1].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[1]).Id.ShouldBe(5);
            ((UserRecord)records[1]).FirstName.ShouldBe("John");
            ((UserRecord)records[1]).LastName.ShouldBe("Doe");
            ((UserRecord)records[1]).BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            records[2].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[2]).Id.ShouldBe(172);
            ((UserRecord)records[2]).FirstName.ShouldBe("Debra");
            ((UserRecord)records[2]).LastName.ShouldBe("Thompson");
            ((UserRecord)records[2]).BirthDate.ShouldBeNull();

            records[6].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[6]).Id.ShouldBe(59);
            ((UserRecord)records[6]).FirstName.ShouldBe("Grant");
            ((UserRecord)records[6]).LastName.ShouldBe("Burk");
            ((UserRecord)records[6]).BirthDate.ShouldBe(new DateTime(1985, 11, 15));


            records[3].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[3]).OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            ((OrderRecord)records[3]).Date.ShouldBe(new DateTime(2020, 07, 24));
            ((OrderRecord)records[3]).Amount.ShouldBe(516);

            records[5].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[5]).OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            ((OrderRecord)records[5]).Date.ShouldBe(new DateTime(2021, 01, 15));
            ((OrderRecord)records[5]).Amount.ShouldBe(12572.15m);


            records[4].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[4]).TransactionId.ShouldBe("553245234");
            ((TransactionRecord)records[4]).Paid.ShouldBe(true);

            records[7].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[7]).TransactionId.ShouldBe("3482739487");
            ((TransactionRecord)records[7]).Paid.ShouldBe(false);

            Stream memoryStream = new MemoryStream();

            await serializer.SerializeRecordAsync(records, memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);

            records = await serializer.DeserializeRecordsAsync(memoryStream, new[] { typeof(HeaderRecord), typeof(UserRecord), typeof(OrderRecord), typeof(TransactionRecord) }).ToListAsync();

            records.Count.ShouldBe(8);

            records[0].GetType().ShouldBe(typeof(HeaderRecord));
            ((HeaderRecord)records[0]).Id.ShouldBe(new Guid("7B31E22D-1E83-4F4D-95FC-E0A83F5106F5"));
            ((HeaderRecord)records[0]).GeneratedDate.ShouldBe(new DateTime(2021, 06, 11));

            records[1].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[1]).Id.ShouldBe(5);
            ((UserRecord)records[1]).FirstName.ShouldBe("John");
            ((UserRecord)records[1]).LastName.ShouldBe("Doe");
            ((UserRecord)records[1]).BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            records[2].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[2]).Id.ShouldBe(172);
            ((UserRecord)records[2]).FirstName.ShouldBe("Debra");
            ((UserRecord)records[2]).LastName.ShouldBe("Thompson");
            ((UserRecord)records[2]).BirthDate.ShouldBeNull();

            records[6].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[6]).Id.ShouldBe(59);
            ((UserRecord)records[6]).FirstName.ShouldBe("Grant");
            ((UserRecord)records[6]).LastName.ShouldBe("Burk");
            ((UserRecord)records[6]).BirthDate.ShouldBe(new DateTime(1985, 11, 15));


            records[3].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[3]).OrderId.ShouldBe(new Guid("A95A067E-A77C-43FE-BAFF-211725608AA7"));
            ((OrderRecord)records[3]).Date.ShouldBe(new DateTime(2020, 07, 24));
            ((OrderRecord)records[3]).Amount.ShouldBe(516);

            records[5].GetType().ShouldBe(typeof(OrderRecord));
            ((OrderRecord)records[5]).OrderId.ShouldBe(new Guid("5B24142F-B003-4CF2-81E8-E036D9A0E3A1"));
            ((OrderRecord)records[5]).Date.ShouldBe(new DateTime(2021, 01, 15));
            ((OrderRecord)records[5]).Amount.ShouldBe(12572.15m);


            records[4].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[4]).TransactionId.ShouldBe("553245234");
            ((TransactionRecord)records[4]).Paid.ShouldBe(true);

            records[7].GetType().ShouldBe(typeof(TransactionRecord));
            ((TransactionRecord)records[7]).TransactionId.ShouldBe("3482739487");
            ((TransactionRecord)records[7]).Paid.ShouldBe(false);

        }

        [Fact]
        public async Task DeserializeRecordsAndIgnoreUndeclaredAsync()
        {
            CsvSerializer serializer = new(s =>
            {
                s.IgnoreUnknownRecords = true;
            });

            List<object> records;

            await _textRecordFileSemaphore.WaitAsync();

            try
            {
                await using (Stream stream = new FileStream("./_Data/TEST.Records.csv", FileMode.Open))
                {
                    records = await serializer.DeserializeRecordsAsync(stream, new[] { typeof(UserRecord) }).ToListAsync();
                }
            }
            finally
            {
                _textRecordFileSemaphore.Release();
            }

            records.Count.ShouldBe(3);

            records[0].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[0]).Id.ShouldBe(5);
            ((UserRecord)records[0]).FirstName.ShouldBe("John");
            ((UserRecord)records[0]).LastName.ShouldBe("Doe");
            ((UserRecord)records[0]).BirthDate.ShouldBe(new DateTime(1970, 04, 24));

            records[1].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[1]).Id.ShouldBe(172);
            ((UserRecord)records[1]).FirstName.ShouldBe("Debra");
            ((UserRecord)records[1]).LastName.ShouldBe("Thompson");
            ((UserRecord)records[1]).BirthDate.ShouldBeNull();

            records[2].GetType().ShouldBe(typeof(UserRecord));
            ((UserRecord)records[2]).Id.ShouldBe(59);
            ((UserRecord)records[2]).FirstName.ShouldBe("Grant");
            ((UserRecord)records[2]).LastName.ShouldBe("Burk");
            ((UserRecord)records[2]).BirthDate.ShouldBe(new DateTime(1985, 11, 15));
        }

        [Fact]
        public void SerializeCsv()
        {
            CsvSerializer serializer = new();

            Stream stream = new FileStream("./_Data/TEST.Persons.csv", FileMode.Open);

            PersonWithHeader[] persons = Should.NotThrow(() => serializer.Deserialize<PersonWithHeader>(stream)).ToArray();

            stream.Dispose();

            persons.Length.ShouldBe(3);

            persons[0].Id.ShouldBe(0);
            persons[0].FirstName.ShouldBe("John");
            persons[0].LastName.ShouldBe("Smith");
            persons[0].BirthDate.ShouldBe(new DateTime(1987, 01, 01));
            persons[0].Active.ShouldBe(true);
            persons[0].ClassId.ShouldBeNull();

            persons[1].Id.ShouldBe(1);
            persons[1].FirstName.ShouldBe("Jeb");
            persons[1].LastName.ShouldBe("Kerbal");
            persons[1].BirthDate.ShouldBe(new DateTime(1970, 04, 24));
            persons[1].Active.ShouldBe(false);
            persons[1].ClassId.Value.ShouldBe(15);

            persons[2].Id.ShouldBe(2);
            persons[2].FirstName.ShouldBe("Del \"Fonzie\" Fon");
            persons[2].LastName.ShouldBe("Mathi");
            persons[2].BirthDate.ShouldBe(new DateTime(2000, 06, 27));
            persons[2].Active.ShouldBe(true);
            persons[2].ClassId.ShouldBeNull();

            stream = new MemoryStream();

            serializer.Serialize(persons, stream);

            stream.Seek(0, SeekOrigin.Begin);

            persons = Should.NotThrow(() => serializer.Deserialize<PersonWithHeader>(stream)).ToArray();

            persons.Length.ShouldBe(3);

            persons[0].Id.ShouldBe(0);
            persons[0].FirstName.ShouldBe("John");
            persons[0].LastName.ShouldBe("Smith");
            persons[0].BirthDate.ShouldBe(new DateTime(1987, 01, 01));
            persons[0].Active.ShouldBe(true);
            persons[0].ClassId.ShouldBeNull();

            persons[1].Id.ShouldBe(1);
            persons[1].FirstName.ShouldBe("Jeb");
            persons[1].LastName.ShouldBe("Kerbal");
            persons[1].BirthDate.ShouldBe(new DateTime(1970, 04, 24));
            persons[1].Active.ShouldBe(false);
            persons[1].ClassId.Value.ShouldBe(15);

            persons[2].Id.ShouldBe(2);
            persons[2].FirstName.ShouldBe("Del \"Fonzie\" Fon");
            persons[2].LastName.ShouldBe("Mathi");
            persons[2].BirthDate.ShouldBe(new DateTime(2000, 06, 27));
            persons[2].Active.ShouldBe(true);
            persons[2].ClassId.ShouldBeNull();
        }

        [Fact]
        public async Task SerializeCsvAsync()
        {
            CsvSerializer serializer = new();

            Stream stream = new FileStream("./_Data/TEST.Persons.csv", FileMode.Open);

            PersonWithHeader[] persons = Should.NotThrow(async () => await serializer.DeserializeAsync<PersonWithHeader>(stream)).ToArray();

            await stream.DisposeAsync();

            persons.Length.ShouldBe(3);

            persons[0].Id.ShouldBe(0);
            persons[0].FirstName.ShouldBe("John");
            persons[0].LastName.ShouldBe("Smith");
            persons[0].BirthDate.ShouldBe(new DateTime(1987, 01, 01));
            persons[0].Active.ShouldBe(true);
            persons[0].ClassId.ShouldBeNull();

            persons[1].Id.ShouldBe(1);
            persons[1].FirstName.ShouldBe("Jeb");
            persons[1].LastName.ShouldBe("Kerbal");
            persons[1].BirthDate.ShouldBe(new DateTime(1970, 04, 24));
            persons[1].Active.ShouldBe(false);
            persons[1].ClassId.Value.ShouldBe(15);

            persons[2].Id.ShouldBe(2);
            persons[2].FirstName.ShouldBe("Del \"Fonzie\" Fon");
            persons[2].LastName.ShouldBe("Mathi");
            persons[2].BirthDate.ShouldBe(new DateTime(2000, 06, 27));
            persons[2].Active.ShouldBe(true);
            persons[2].ClassId.ShouldBeNull();

            stream = new MemoryStream();

            await serializer.SerializeAsync(persons, stream);

            stream.Seek(0, SeekOrigin.Begin);

            persons = Should.NotThrow(async () => await serializer.DeserializeAsync<PersonWithHeader>(stream)).ToArray();

            persons.Length.ShouldBe(3);

            persons[0].Id.ShouldBe(0);
            persons[0].FirstName.ShouldBe("John");
            persons[0].LastName.ShouldBe("Smith");
            persons[0].BirthDate.ShouldBe(new DateTime(1987, 01, 01));
            persons[0].Active.ShouldBe(true);
            persons[0].ClassId.ShouldBeNull();

            persons[1].Id.ShouldBe(1);
            persons[1].FirstName.ShouldBe("Jeb");
            persons[1].LastName.ShouldBe("Kerbal");
            persons[1].BirthDate.ShouldBe(new DateTime(1970, 04, 24));
            persons[1].Active.ShouldBe(false);
            persons[1].ClassId.Value.ShouldBe(15);

            persons[2].Id.ShouldBe(2);
            persons[2].FirstName.ShouldBe("Del \"Fonzie\" Fon");
            persons[2].LastName.ShouldBe("Mathi");
            persons[2].BirthDate.ShouldBe(new DateTime(2000, 06, 27));
            persons[2].Active.ShouldBe(true);
            persons[2].ClassId.ShouldBeNull();
        }
    }
}