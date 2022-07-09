namespace DeltaWare.SDK.Serialization.Csv.Writing
{
    /// <summary>
    /// Indicates the Field Type.
    /// </summary>
    public enum WriteOperation
    {
        /// <summary>
        /// Normal Field.
        /// </summary>
        /// <remarks>Any field that is not the last field for the current line.</remarks>
        Field,
        /// <summary>
        /// Terminate the current Line.
        /// </summary>
        /// <remarks>The last field for the current line.</remarks>
        TerminateLine,
    }
}