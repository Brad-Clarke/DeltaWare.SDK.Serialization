using DeltaWare.SDK.Serialization.Csv.Tests.Models;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.Serialization.Csv.Reading;
using DeltaWare.SDK.Serialization.Csv.Reading.Options;
using DeltaWare.SDK.Serialization.Csv.Writing;
using Xunit;

namespace DeltaWare.SDK.Serialization.Csv.Tests
{
    public class CsvSerializerShould
    {
        private static SemaphoreSlim _textRecordFileSemaphore = new SemaphoreSlim(1, 1);

        [Fact]
        public async Task SerializeCsvAsync()
        {
            CsvSerializer serializer = new();

            Stream stream = new FileStream("./_Data/TEST.Persons.csv", FileMode.Open);

            CsvStreamReader csvStreamReader = new CsvStreamReader(stream, new CsvReaderOptions
            {
                TrimFields = true
            });
            
            PersonWithHeader[] persons = Should.NotThrow(() => serializer.DeserializeAsync<PersonWithHeader>(csvStreamReader, true).ToBlockingEnumerable().ToArray());

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

            CsvStreamWriter csvStreamWriter = new CsvStreamWriter(stream);

            await serializer.SerializeAsync(persons, csvStreamWriter, true);

            await csvStreamWriter.FlushAsync();

            stream.Seek(0, SeekOrigin.Begin);

            csvStreamReader = new CsvStreamReader(stream, new CsvReaderOptions
            {
                TrimFields = true
            });

            persons = Should.NotThrow(() => serializer.DeserializeAsync<PersonWithHeader>(csvStreamReader, true).ToBlockingEnumerable().ToArray());

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