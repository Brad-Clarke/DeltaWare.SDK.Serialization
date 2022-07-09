using DeltaWare.SDK.Serialization.Csv.Enums;
using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Reading.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Reading
{
    public sealed class CsvReader : ICsvReader, IDisposable
    {
        private readonly StreamReader _baseStream;
        private readonly ICsvReaderSettings _readerSettings;
        private CsvState _state = CsvState.NotWriting;

        public int LineNumber { get; private set; }

        public int LinePosition { get; private set; }

        /// <inheritdoc cref="StreamReader.EndOfStream"/>>
        public bool EndOfStream => _baseStream.EndOfStream;

        /// <inheritdoc cref="StreamReader.BaseStream"/>>
        public StreamReader BaseStream => _baseStream;

        /// <inheritdoc cref="StreamReader.BaseStream"/>>
        public Encoding CurrentEncoding => _baseStream.CurrentEncoding;

        public CsvReader(StreamReader baseStream, ICsvReaderSettings settings = null)
        {
            _baseStream = baseStream;
            _readerSettings = settings ?? CsvReaderSettings.Default;
        }

        public CsvReader(StreamReader baseStream, Action<CsvReaderSettings> settingsBuild)
        {
            _baseStream = baseStream;

            CsvReaderSettings settings = CsvReaderSettings.Default;

            settingsBuild.Invoke(settings);

            _readerSettings = settings;
        }

        public CsvReader(string value, ICsvReaderSettings settings = null)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(value);
            writer.Flush();
            stream.Position = 0;

            _baseStream = new StreamReader(stream);
            _readerSettings = settings ?? CsvReaderSettings.Default;
        }

        public CsvReader(string value, Action<CsvReaderSettings> settingsBuild)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(value);
            writer.Flush();
            stream.Position = 0;

            _baseStream = new StreamReader(stream);

            CsvReaderSettings settings = CsvReaderSettings.Default;

            settingsBuild.Invoke(settings);

            _readerSettings = settings;
        }

        #region Async Methods

        public Task<string> ReadAsync()
        {
            // TODO: Implement this correctly.
            return Task.FromResult(Read());
        }

        public async Task<string[]> ReadLineAsync()
        {
            List<string> fields = new List<string>();

            StringBuilder fieldBuilder = new StringBuilder();

            do
            {
                string field = await ReadAsync();

                if (_state.HasFlag(CsvState.Writing))
                {
                    fields.Add(field);
                }
                else
                {
                    fields.Add(null);
                }

                fieldBuilder.Clear();

                LineNumber++;
            }
            while (!_state.HasFlag(CsvState.EndOfLine));

            return fields.ToArray();
        }

        public async Task<string[][]> ReadToEndAsync()
        {
            List<string[]> csvRows = new List<string[]>();

            int expectedRowCount = -1;

            int lineNumber = 1;

            while (!_baseStream.EndOfStream)
            {
                string[] row = await ReadLineAsync();

                if (expectedRowCount == -1)
                {
                    expectedRowCount = row.Length;
                }
                else if (_readerSettings.MissingColumnHandling == MissingColumnHandling.NotAllow && row.Length != expectedRowCount)
                {
                    throw InvalidCsvDataException.InvalidColumnCount(lineNumber, expectedRowCount, row.Length);
                }

                lineNumber++;

                csvRows.Add(row);
            }

            return csvRows.ToArray();
        }

        #endregion

        #region Sync Methods

        public string Read()
        {
            _state = CsvState.NotWriting;

            StringBuilder fieldBuilder = new();

            do
            {
                int utf32 = _baseStream.Read();

                LinePosition++;

                // End Of File.
                if (utf32 == _readerSettings.Utf32EndOfFile)
                {
                    if (_state.HasFlag(CsvState.EncapsulateField))
                    {
                        throw InvalidCsvDataException.EncapsulationFieldTerminationExpectedEndOfFile(LineNumber, LinePosition);
                    }

                    _state = CsvState.EndOfField | CsvState.EndOfLine | CsvState.EndOfFile;
                }
                else if (utf32 == _readerSettings.Utf32Comma)
                {
                    if (_state.HasFlag(CsvState.EncapsulateField))
                    {
                        // We've hit a comma whilst in an encapsulated field, write that data.
                        _state |= CsvState.Writing;
                    }
                    else
                    {
                        // We've hit a comma so we're at the end of the field.
                        _state = CsvState.EndOfField;
                    }
                }
                else if (utf32 == _readerSettings.Utf32CarriageReturn)
                {
                    if (_state.HasFlag(CsvState.EncapsulateField))
                    {
                        // We've hit a carriage return whilst in an encapsulated field, write that data.
                        _state |= CsvState.Writing;
                    }
                    else if (_baseStream.Peek() == _readerSettings.Utf32LineFeed)
                    {
                        _baseStream.Read();

                        // We've hit a new line
                        _state = CsvState.EndOfField | CsvState.EndOfLine;
                    }
                    else
                    {
                        throw InvalidCsvDataException.ExpectedLineFeed(LineNumber, LinePosition);
                    }
                }
                else if (_state.HasFlag(CsvState.FieldTerminated))
                {
                    // An encapsulated field was terminated and we did not terminate the field,
                    // throw an exception.
                    throw InvalidCsvDataException.EncapsulationFieldTerminationExpected(LineNumber, LinePosition);
                }
                else if (utf32 == _readerSettings.Utf32QuotationMark)
                {
                    if (_state.HasFlag(CsvState.EncapsulateField))
                    {
                        if (_baseStream.Peek() == _readerSettings.Utf32QuotationMark)
                        {
                            utf32 = _baseStream.Read();

                            LinePosition++;

                            _state |= CsvState.Writing;
                        }
                        else
                        {
                            _state = CsvState.FieldTerminated;
                        }
                    }
                    else if (fieldBuilder.Length == 0)
                    {
                        _state |= CsvState.EncapsulateField;
                    }
                    else
                    {
                        throw InvalidCsvDataException.IllegalCharacterInNonEncapsulatedField(LineNumber, LinePosition);
                    }
                }
                else
                {
                    _state |= CsvState.Writing;
                }

                if (_state.HasFlag(CsvState.Writing))
                {
                    fieldBuilder.Append(char.ConvertFromUtf32(utf32));
                }
            }
            while (!_state.HasFlag(CsvState.EndOfField));

            if (fieldBuilder.Length > 0)
            {
                _state |= CsvState.Writing;
            }

            return fieldBuilder.ToString();
        }

        public string[] ReadLine()
        {
            List<string> fields = new();

            StringBuilder fieldBuilder = new();

            do
            {
                string field = Read();

                if (_state.HasFlag(CsvState.Writing))
                {
                    fields.Add(field);
                }
                else
                {
                    fields.Add(null);
                }

                fieldBuilder.Clear();

                LineNumber++;
            }
            while (!_state.HasFlag(CsvState.EndOfLine));

            return fields.ToArray();
        }

        public string[][] ReadToEnd()
        {
            List<string[]> csvRows = new();

            int expectedRowCount = -1;

            int lineNumber = 1;

            while (!_baseStream.EndOfStream)
            {
                string[] row = ReadLine();

                if (expectedRowCount == -1)
                {
                    expectedRowCount = row.Length;
                }
                else if (_readerSettings.MissingColumnHandling == MissingColumnHandling.NotAllow && row.Length != expectedRowCount)
                {
                    throw InvalidCsvDataException.InvalidColumnCount(lineNumber, expectedRowCount, row.Length);
                }

                lineNumber++;

                csvRows.Add(row);
            }

            return csvRows.ToArray();
        }

        #endregion

        public void Dispose()
        {
            _baseStream?.Dispose();
        }
    }
}