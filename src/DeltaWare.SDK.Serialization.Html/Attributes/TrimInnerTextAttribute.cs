using System;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TrimInnerTextAttribute : Attribute
    {
        private readonly string _value;

        public TrimInnerTextAttribute(string value)
        {
            _value = value;
        }

        public string TrimStart(string value)
        {
            if (!value.StartsWith(_value))
            {
                throw new Exception();
            }

            int length = _value.Length;

            return value.Substring(length, value.Length - length);
        }
    }
}