using System;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public sealed class CsvSerializationException : Exception
    {
        public CsvSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
