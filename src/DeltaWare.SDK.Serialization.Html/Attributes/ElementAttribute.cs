using DeltaWare.SDK.Serialization.Html.Enums;
using HtmlAgilityPack;
using System;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ElementAttribute : Attribute
    {
        public string Name { get; }

        public ElementAttribute(string name)
        {
            Name = name;
        }

        public ElementAttribute(ElementType type)
        {
            Name = type.ToString().ToLower();
        }

        public bool IsNode(HtmlNode node)
        {
            return node.Name == Name;
        }
    }
}