using System;

namespace DeltaWare.SDK.Serialization.Csv.Exceptions
{
    public class InvalidCsvDataException : Exception
    {
        private InvalidCsvDataException(int lineNumber, int linePosition, string message) : base($"Line[{lineNumber}]:Position[{linePosition}] - {message}")
        {
        }

        public static InvalidCsvDataException IllegalCharacterInNonEncapsulatedField(int lineNumber, int linePosition, char character) 
            => new(lineNumber, linePosition, $"An Illegal character \"{character}\" was found in a non encapsulated field.");
    }
}