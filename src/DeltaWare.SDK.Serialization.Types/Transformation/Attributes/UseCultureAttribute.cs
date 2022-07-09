using System;
using System.Globalization;

namespace DeltaWare.SDK.Serialization.Types.Transformation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class UseCultureAttribute : Attribute
    {
        public CultureInfo Culture { get; }

        public UseCultureAttribute(int culture)
        {
            Culture = CultureInfo.GetCultureInfo(culture);
        }

        public UseCultureAttribute(string name)
        {
            Culture = CultureInfo.GetCultureInfo(name);
        }
    }
}