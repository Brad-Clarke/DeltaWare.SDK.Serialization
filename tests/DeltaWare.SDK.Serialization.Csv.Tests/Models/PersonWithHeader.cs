using DeltaWare.SDK.Serialization.Csv.Attributes;
using System;

namespace DeltaWare.SDK.Serialization.Csv.Tests.Models
{
    [HeaderRequired]
    public class PersonWithHeader
    {
        public bool Active { get; set; }

        [ColumnHeader("birth date")]
        public DateTime BirthDate { get; set; }

        [ColumnHeader("first name")]
        public string FirstName { get; set; }

        [ColumnHeader("id")]
        public long Id { get; set; }

        [ColumnHeader("last name")]
        public string LastName { get; set; }

        [ColumnHeader("class id")]
        public int? ClassId { get; set; }
    }
}