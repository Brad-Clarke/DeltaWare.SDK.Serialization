using System;

namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    /// <summary>
    /// Specifies that when the current class is processed by the <see cref="CsvSerializer"/> headers must be present.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HeaderRequiredAttribute : Attribute
    {
    }
}