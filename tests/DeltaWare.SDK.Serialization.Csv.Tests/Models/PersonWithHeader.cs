using DeltaWare.SDK.Serialization.Csv.Attributes;
using System;

namespace DeltaWare.SDK.Serialization.Csv.Tests.Models
{
    [CsvHeaderRequired]
    public class PersonWithHeader
    {
        //[CsvHeader("active")]
        public bool Active { get; set; }

        [CsvHeader("birth date")]
        public DateTime BirthDate { get; set; }

        [CsvHeader("first name")]
        public string FirstName { get; set; }

        [CsvHeader("id")]
        public long Id { get; set; }

        [CsvHeader("last name")]
        public string LastName { get; set; }

        [CsvHeader("class id")]
        public int? ClassId { get; set; }
    }
}