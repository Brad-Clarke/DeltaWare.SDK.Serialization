using HtmlAgilityPack;
using System;
using System.Linq;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnoreElementWithAttributeAttribute : Attribute
    {
        public string Name { get; }

        public string Value { get; }

        public IgnoreElementWithAttributeAttribute(string name, string value = null)
        {
            Name = name;
            Value = value;
        }

        public bool MatchAttribute(HtmlNode node)
        {
            HtmlAttribute attribute = node.Attributes.FirstOrDefault(a => a.Name == Name);

            if (attribute == null)
            {
                return false;
            }

            return attribute.Value.Equals(Value);
        }
    }
}