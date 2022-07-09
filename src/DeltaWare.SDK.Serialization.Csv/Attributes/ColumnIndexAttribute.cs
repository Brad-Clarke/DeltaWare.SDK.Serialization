using System;

namespace DeltaWare.SDK.Serialization.Csv.Attributes
{
    /// <summary>
    /// Specifies the column associated with this property by the index.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnIndexAttribute : Attribute
    {
        /// <summary>
        /// The column index associated with the property.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Specifies the column associated with this property by the index.
        /// </summary>
        /// <param name="index">The column index associated with the property.</param>
        /// <exception cref="ArgumentException">Thrown when an index is a negative number.</exception>
        public ColumnIndexAttribute(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException("Index must be a positive number.", nameof(index));
            }

            Index = index;
        }
    }
}