using DeltaWare.SDK.Serialization.Html.Enums;
using DeltaWare.SDK.Serialization.Html.Types;
using HtmlAgilityPack;
using System;
using System.Diagnostics;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    [DebuggerDisplay("{Index} | {StringMatcher.Key}")]
    public class ElementIndexDynamicAttribute : Attribute
    {
        public int? Index { get; internal set; }

        public StringMatcher StringMatcher { get; }

        public ElementIndexDynamicAttribute(string key, MatchCondition matchCondition = MatchCondition.Complete)
        {
            StringMatcher = new StringMatcher(key, matchCondition);
        }

        public bool DoesMatch(HtmlNode node)
        {
            return StringMatcher.DoesMatch(node.InnerText);
        }
    }
}