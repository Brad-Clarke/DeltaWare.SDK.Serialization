namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    /// <summary>
    /// Specifies the column associated with this property by the header name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvHeaderAttribute : Attribute
    {
        /// <summary>
        /// The column header associated with the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Specifies the column associated with this property by the header name.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <exception cref="ArgumentException">Thrown when a header name is provided that is either null or empty.</exception>
        public CsvHeaderAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;
        }
    }
}