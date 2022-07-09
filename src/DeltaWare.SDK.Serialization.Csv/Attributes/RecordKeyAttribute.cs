using DeltaWare.SDK.Core.Validators;
using System;

namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RecordKeyAttribute : Attribute
    {
        public string Type { get; }

        public RecordKeyAttribute(string type)
        {
            StringValidator.ThrowOnNullOrWhitespace(type, nameof(type));

            Type = type;
        }
    }
}
