using DeltaWare.SDK.Serialization.Html.Enums;
using DeltaWare.SDK.Serialization.Html.Types;
using HtmlAgilityPack;
using System;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MatchElementWithInnerTextAttribute : Attribute
    {
        private readonly StringMatcher _htmlMatcher;

        public MatchElementWithInnerTextAttribute(string value, MatchCondition matchCondition = MatchCondition.StartsWith)
        {
            _htmlMatcher = new StringMatcher(value, matchCondition);
        }

        public bool DoesNodeMatch(HtmlNode node)
        {
            return _htmlMatcher.DoesMatch(node.InnerHtml);
        }
    }
}