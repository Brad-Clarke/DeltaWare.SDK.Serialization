using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Reading.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Reading
{
    /// <summary>
    /// Provides a reader that parses CSV data from a stream, handling different states and options according to CSV format specifications.
    /// </summary>
    /// <remarks>
    /// <para>The CsvStreamReader is designed to read large CSV files efficiently from a stream, offering asynchronous operations to handle potentially large datasets without blocking the calling thread. It supports configuration options to tailor CSV parsing behavior, such as delimiters, quotes, and handling of comments within the CSV data.</para>
    /// <para>This class is not thread-safe; ensure that multiple threads do not modify the same instance concurrently.</para>
    /// <para>Exceptions such as <see cref="InvalidCsvDataException"/> can be thrown if malformed CSV data is encountered.</para>
    /// </remarks>
    public sealed class CsvStreamReader : IDisposable
    {
        private const char CarriageReturnCharacter = '\r';
        private const char LineFeedCharacter = '\n';
        private const char WhitespaceCharacter = ' ';

        /// <summary>
        /// Gets the current position in the stream.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the current line position in the stream.
        /// </summary>
        public int LinePosition { get; private set; }

        /// <summary>
        /// Gets the current line number in the stream.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Indicates whether the end of the file has been reached.
        /// </summary>
        public bool EndOfFile => _state.HasFlag(CsvState.EndOfFile);

        private CsvState _state = CsvState.SuppressOutput | CsvState.FieldStart;
        private int _internalBufferPosition;
        private int _internalBufferLength;

        private readonly char[] _internalBuffer;
        private readonly StreamReader _baseStream;
        private readonly ICsvReaderOptions _options;
        private readonly StringBuilder _fieldBuilder = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the CsvStreamReader class.
        /// </summary>
        /// <param name="baseStream">The input stream to read the CSV data from.</param>
        /// <param name="options">Configuration options that control the behavior of the CSV reader.</param>
        public CsvStreamReader(StreamReader baseStream, ICsvReaderOptions? options = null)
        {
            _baseStream = baseStream;
            _options = options ?? new CsvReaderOptions();
            _internalBuffer = new char[_options.BufferSize];
        }

        /// <summary>
        /// Initializes a new instance of the CsvStreamReader class.
        /// </summary>
        /// <param name="baseStream">The input stream to read the CSV data from.</param>
        /// <param name="options">Configuration options that control the behavior of the CSV reader.</param>
        public CsvStreamReader(Stream baseStream, ICsvReaderOptions? options = null)
        {
            _baseStream = new StreamReader(baseStream);
            _options = options ?? new CsvReaderOptions();
            _internalBuffer = new char[_options.BufferSize];
        }

        /// <summary>
        /// Reads a single line from the CSV file asynchronously each time it is called, returning each field within that line as a separate string.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token to cancel the read operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <see cref="string"/> where each iteration yields a single string representing a field from the current line.</returns>
        /// <exception cref="InvalidCsvDataException">Thrown when an illegal character is found in a non-encapsulated field, 
        /// or when a quotation mark is encountered unexpectedly, indicating malformed CSV data.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the stream is disposed during the operation.</exception>
        /// <remarks>
        /// This method processes characters from the CSV stream to construct and yield fields from a single line based on the specified CSV format settings.
        /// It handles quotes, delimiters, and newlines according to the configuration options provided. Comments are managed based on the reader's settings.
        /// The method concludes reading the current line either at the end of the line or when a cancellation is requested and will continue to read the next line upon subsequent calls until the end of the file is reached.
        /// It is crucial to ensure that the stream remains open and is not disposed or altered externally while reading is in progress.
        /// </remarks>
        public async IAsyncEnumerable<string?> ReadLineAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            bool isEmptyLine = true;

            LinePosition = 0;

            while (_internalBufferPosition < _internalBufferLength || await RefreshInternalBufferAsync())
            {
                if (_state.HasFlag(CsvState.EndOfFile))
                {
                    break;
                }

                char character = _internalBuffer[_internalBufferPosition];

                _state = GetCharacterCsvState(_state, character);

                Position++;
                _internalBufferPosition++;
                LinePosition++;

                if (_state.HasFlag(CsvState.Output))
                {
                    isEmptyLine = false;

                    _fieldBuilder.Append(character);
                }

                if (!_state.HasFlag(CsvState.FieldTerminated))
                {
                    continue;
                }

                if (_options.TrimFields)
                {
                    TrimEnd(_fieldBuilder);
                }

                var field = _fieldBuilder.ToString();

                _fieldBuilder.Clear();

                if (_options.SkipEmptyLines && isEmptyLine && _state.HasFlag(CsvState.EndOfLine))
                {
                    continue;
                }

                yield return field;

                if (!_state.HasFlag(CsvState.EndOfLine))
                {
                    _state = CsvState.SuppressOutput | CsvState.FieldStart;

                    continue;
                }

                _state = CsvState.SuppressOutput | CsvState.FieldStart;

                LineNumber++;

                break;
            }
        }

        private static void TrimEnd(StringBuilder fieldBuilder)
        {
            int end = fieldBuilder.Length - 1;

            while (end >= 0 && fieldBuilder[end] == WhitespaceCharacter)
            {
                end--;
            }

            fieldBuilder.Length = end + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetCharacterCsvState(CsvState previousState, char character)
        {
            if (previousState.HasFlag(CsvState.FieldStart) && character == _options.CommentCharacter)
            {
                return GetCommentCharacterCsvState();
            }

            if (character == WhitespaceCharacter)
            {
                return GetWhitespaceCharacterStateCsvState(previousState);
            }

            if (character == _options.DelimiterCharacter)
            {
                return GetDelimiterCharacterCsvState(previousState);
            }

            if (character == _options.QuoteCharacter)
            {
                return GetQuoteCharacterStateCsvState(previousState, character);
            }

            if (character == CarriageReturnCharacter)
            {
                return GetCarriageReturnCharacterCsvState(previousState);
            }

            if (character == LineFeedCharacter)
            {
                return GetLineFeedCharacterCsvState(previousState);
            }

            if (previousState.HasFlag(CsvState.FieldEnd))
            {
                throw InvalidCsvDataException.IllegalCharacterInNonEncapsulatedField(LineNumber, LinePosition, character);
            }

            if (previousState.HasFlag(CsvState.FieldStart))
            {
                previousState &= ~CsvState.FieldStart;
            }

            return previousState | CsvState.Output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetCommentCharacterCsvState()
        {
            if (_options.IgnoreComments)
            {
                return CsvState.SuppressOutput | CsvState.WithinComment;
            }

            return CsvState.Output | CsvState.WithinComment;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetWhitespaceCharacterStateCsvState(CsvState previousState)
        {
            if (previousState.HasFlag(CsvState.WithinComment))
            {
                return previousState;
            }

            if (previousState.HasFlag(CsvState.FieldStart))
            {
                if (_options.TrimFields)
                {
                    return CsvState.SuppressOutput | CsvState.FieldStart;
                }

                return CsvState.Output | CsvState.FieldStart;
            }

            if (previousState.HasFlag(CsvState.FieldEnd))
            {
                if (_options.TrimFields)
                {
                    return CsvState.SuppressOutput | CsvState.FieldEnd;
                }

                return CsvState.Output | CsvState.FieldEnd;
            }

            return previousState | CsvState.Output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetDelimiterCharacterCsvState(CsvState previousState)
        {
            if (previousState.HasFlag(CsvState.WithinComment))
            {
                return previousState;
            }

            if (previousState.HasFlag(CsvState.FieldEncapsulated) && !previousState.HasFlag(CsvState.QuotationMarkHit))
            {
                return CsvState.Output | CsvState.FieldEncapsulated;
            }

            return CsvState.SuppressOutput | CsvState.FieldTerminated;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetQuoteCharacterStateCsvState(CsvState previousState, char character)
        {
            if (previousState.HasFlag(CsvState.WithinComment))
            {
                return previousState;
            }

            if (previousState.HasFlag(CsvState.FieldStart))
            {
                return CsvState.SuppressOutput | CsvState.FieldEncapsulated;
            }

            if (!previousState.HasFlag(CsvState.FieldEncapsulated))
            {
                throw InvalidCsvDataException.IllegalCharacterInNonEncapsulatedField(LineNumber, LinePosition, character);
            }

            if (previousState.HasFlag(CsvState.QuotationMarkHit))
            {
                return CsvState.Output | CsvState.FieldEncapsulated;
            }

            return CsvState.SuppressOutput | CsvState.FieldEncapsulated | CsvState.QuotationMarkHit;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetCarriageReturnCharacterCsvState(CsvState previousState)
        {
            if (previousState.HasFlag(CsvState.WithinComment))
            {
                return previousState;
            }

            if (previousState.HasFlag(CsvState.FieldEncapsulated) && !previousState.HasFlag(CsvState.QuotationMarkHit))
            {
                return CsvState.Output | CsvState.FieldEncapsulated;
            }

            return CsvState.SuppressOutput | CsvState.FieldEnd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CsvState GetLineFeedCharacterCsvState(CsvState previousState)
        {
            if (previousState.HasFlag(CsvState.FieldEncapsulated) && !previousState.HasFlag(CsvState.QuotationMarkHit))
            {
                return CsvState.Output | CsvState.FieldEncapsulated;
            }

            return CsvState.SuppressOutput | CsvState.FieldTerminated | CsvState.EndOfLine;
        }

        private async Task<bool> RefreshInternalBufferAsync()
        {
            _internalBufferPosition = 0;
            _internalBufferLength = await _baseStream.ReadAsync(_internalBuffer, 0, _internalBuffer.Length);

            var notEndOfStream = _internalBufferLength > 0;

            if (!notEndOfStream)
            {
                _state = CsvState.EndOfFile;
            }

            return notEndOfStream;
        }

        /// <summary>
        /// Disposes the underlying <see cref="StreamReader"/> associated with this instance.
        /// </summary>
        /// <remarks>
        /// Calling this method releases all resources used by the <see cref="StreamReader"/>.
        /// It is recommended to call this method when you are finished using the <see cref="StreamReader"/> to free up system resources promptly.
        /// Once called, further operations on this object may result in an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Dispose()
        {
            _baseStream.Dispose();
        }
    }
}
