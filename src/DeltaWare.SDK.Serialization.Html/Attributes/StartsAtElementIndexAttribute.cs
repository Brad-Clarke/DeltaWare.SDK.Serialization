using System;

namespace DeltaWare.SDK.Serialization.Html.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StartsAtElementIndexAttribute : Attribute
    {
        public int Index { get; }

        public StartsAtElementIndexAttribute(int index)
        {
            Index = index;
        }
    }
}