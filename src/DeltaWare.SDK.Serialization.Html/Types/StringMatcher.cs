using DeltaWare.SDK.Serialization.Html.Enums;
using System;

namespace DeltaWare.SDK.Serialization.Html.Types
{
    public class StringMatcher
    {
        public string Key { get; }
        public MatchCondition MatchCondition { get; }

        public StringMatcher(string key, MatchCondition matchCondition)
        {
            Key = key;
            MatchCondition = matchCondition;
        }

        public bool DoesMatch(string value)
        {
            return MatchCondition switch
            {
                MatchCondition.Complete => value.Equals(Key),
                MatchCondition.StartsWith => value.StartsWith(Key),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}