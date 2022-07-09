using DeltaWare.SDK.Serialization.Csv.Writing;
using System;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public class InvalidCsvDataException : Exception
    {
        private InvalidCsvDataException(int lineNumber, int linePosition, string message) : base($"Line[{lineNumber}]:Position[{linePosition}] - {message}")
        {
        }

        public static InvalidCsvDataException EncapsulationFieldTerminationExpected(int lineNumber, int linePosition)
        {
            return new InvalidCsvDataException(lineNumber, linePosition, "Encapsulated field was not terminated.");
        }

        public static InvalidCsvDataException EncapsulationFieldTerminationExpectedEndOfFile(int lineNumber, int linePosition)
        {
            return new InvalidCsvDataException(lineNumber, linePosition, "Encapsulated field was not terminated before reaching the end of the file.");
        }

        public static InvalidCsvDataException ExpectedLineFeed(int lineNumber, int linePosition)
        {
            return new InvalidCsvDataException(lineNumber, linePosition, "A line feed was expected.");
        }

        public static InvalidCsvDataException IllegalCharacterInNonEncapsulatedField(int lineNumber, int linePosition)
        {
            return new InvalidCsvDataException(lineNumber, linePosition, "An Illegal character was found in a non encapsulated field.");
        }

        public static InvalidCsvDataException InvalidColumnCount(int lineNumber, int expected, int actual)
        {
            return new InvalidCsvDataException(lineNumber, 0, $"The current line has an invalid row count. Expected: {expected}, found: {actual} - If you're attempting to write a Record Type CSV ensure to set the {nameof(CsvWriter)} to the mode to {nameof(WriteMode.Record)} during instantiation.");
        }
    }
}