using System;

namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvRecordAttribute : Attribute
    {
        public string Key { get; }

        public CsvRecordAttribute(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
        }
    }
}
