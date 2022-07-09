using System;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Html.Types
{
    public class ElementInfo
    {
        public ElementInfo ChildElement { get; set; }
        public PropertyInfo Property { get; set; }
        public Type Type { get; set; }
    }
}