using HtmlAgilityPack;
using System;
using System.Linq;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElementIdAttribute : Attribute
    {
        private readonly string _key;

        public ElementIdAttribute(string key)
        {
            _key = key;
        }

        public bool DoesNodeMatch(HtmlNode node)
        {
            HtmlAttribute attribute = node.Attributes.FirstOrDefault(a => a.Name == "id");

            if (attribute == null)
            {
                return false;
            }

            return attribute.Value == _key;
        }
    }
}