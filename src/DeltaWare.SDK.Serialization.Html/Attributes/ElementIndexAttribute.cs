using System;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElementIndexAttribute : Attribute
    {
        public int? EndIndex { get; set; }
        public int StartIndex { get; set; }

        public ElementIndexAttribute(int startIndex)
        {
            StartIndex = startIndex;
        }

        public ElementIndexAttribute(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public bool DoesIndexMatch(int? index)
        {
            if (EndIndex == null)
            {
                return StartIndex == index;
            }

            return index >= StartIndex && index <= EndIndex;
        }
    }
}