using System;

namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    /// <summary>
    /// Specifies the column associated with this property by the header name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnHeaderAttribute : Attribute
    {
        /// <summary>
        /// The column header associated with the property.
        /// </summary>
        public string HeaderName { get; }

        /// <summary>
        /// Specifies the column associated with this property by the header name.
        /// </summary>
        /// <param name="headerName">The name of the header.</param>
        /// <exception cref="ArgumentException">Thrown when a header name is provided that is either null or empty.</exception>
        public ColumnHeaderAttribute(string headerName)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentException(nameof(headerName));
            }

            HeaderName = headerName;
        }
    }
}