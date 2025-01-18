using DeltaWare.SDK.Serialization.Csv.Writing;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public class InvalidCsvDataException : Exception
    {
        private InvalidCsvDataException(int lineNumber, int linePosition, string message) : base($"Line[{lineNumber}]:Position[{linePosition}] - {message}")
        {
        }

        public static InvalidCsvDataException IllegalCharacterInNonEncapsulatedField(int lineNumber, int linePosition, char character) 
            => new(lineNumber, linePosition, $"An Illegal character \"{character}\" was found in a non encapsulated field.");

        public static InvalidCsvDataException InvalidColumnCount(int lineNumber, int expected, int actual) 
            => new(lineNumber, 0, $"The current line has an invalid row count. Expected: {expected}, found: {actual} - If you're attempting to write a Record Type CSV ensure to set the {nameof(CsvStreamWriter)} to the mode to {nameof(CsvStreamWriter)} during instantiation.");
    }
}