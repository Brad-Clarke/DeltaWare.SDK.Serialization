using System;

namespace DeltaWare.SDK.Serialization.Csv.Enums
{
    /// <summary>
    /// Indicates the current state of the CSV.
    /// </summary>
    [Flags]
    internal enum CsvState
    {
        /// <summary>
        /// Indicates that the CSV field should not be written to the buffer.
        /// </summary>
        NotWriting = 0,

        /// <summary>
        /// Indicates that the CSV field should be written to the buffer.
        /// </summary>
        Writing = 1,

        /// <summary>
        /// Indicates that the CSV field is encapsulated.
        /// </summary>
        EncapsulateField = 2,

        /// <summary>
        /// Indicates that the end of the field has been reached.
        /// </summary>
        EndOfField = 4,

        /// <summary>
        /// Indicates that the end of the line has been reached.
        /// </summary>
        EndOfLine = 8,

        /// <summary>
        /// Indicates that the end of the file has been reached.
        /// </summary>
        EndOfFile = 16,

        /// <summary>
        /// Indicates that the next character should terminate the field.
        /// </summary>
        FieldTerminated = 32
    }
}