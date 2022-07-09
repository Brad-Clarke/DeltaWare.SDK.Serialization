using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ReplaceInnerTextAttribute : Attribute
    {
        private readonly bool _ignoreCase;
        private readonly string _newValue;
        private readonly string _oldValue;

        public ReplaceInnerTextAttribute(string oldValue, string newValue, bool ignoreCase = false)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _ignoreCase = ignoreCase;
        }

        public string Replace(string value)
        {
            return value.Replace(_oldValue, _newValue, _ignoreCase, CultureInfo.InvariantCulture);
        }
    }
}