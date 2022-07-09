using DeltaWare.SDK.Serialization.Csv.Attributes;
using System;
using System.Collections.Generic;

namespace DeltaWare.SDK.Serialization.Csv.Tests.Models
{
    public class RecordContainer
    {
        public HeaderRecord Header { get; set; }

        public List<UserRecord> Users { get; set; } = new();

        public List<OrderRecord> Orders { get; set; } = new();

        public List<TransactionRecord> Transactions { get; set; } = new();

        public List<ProductRecord> Products { get; set; } = new();
    }

    [RecordKey("header")]
    public class HeaderRecord
    {
        [ColumnIndex(0)]
        public Guid Id { get; set; }

        [ColumnIndex(1)]
        public DateTime GeneratedDate { get; set; }
    }

    [RecordKey("users")]
    public class UserRecord
    {
        [ColumnIndex(0)]
        public int Id { get; set; }

        [ColumnIndex(1)]
        public string FirstName { get; set; }

        [ColumnIndex(2)]
        public string LastName { get; set; }

        [ColumnIndex(3)]
        public DateTime? BirthDate { get; set; }
    }

    [RecordKey("orders")]
    public class OrderRecord
    {
        public Guid OrderId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }

    [RecordKey("transactions")]
    public class TransactionRecord
    {
        public string TransactionId { get; set; }

        public bool Paid { get; set; }
    }

    [RecordKey("products")]
    public class ProductRecord
    {
        public string TransactionId { get; set; }

        public bool Paid { get; set; }
    }
}
