using DeltaWare.SDK.Serialization.Csv.Writing.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Writing
{
    public sealed class CsvStreamWriter : IDisposable, IAsyncDisposable
    {
        private readonly StreamWriter _baseStream;
        private readonly ICsvWriterOptions _options;

        private readonly StringBuilder _internalBuffer = new StringBuilder();

        public CsvStreamWriter(StreamWriter baseStream, ICsvWriterOptions? options = null)
        {
            _baseStream = baseStream;
            _options = options ?? new CsvWriterOptions();
        }

        public CsvStreamWriter(Stream baseStream, ICsvWriterOptions? options = null)
        {
            _baseStream = new StreamWriter(baseStream);
            _options = options ?? new CsvWriterOptions();
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
            => await _baseStream.FlushAsync(cancellationToken);

        public async Task WriteLineAsync(IEnumerable<string?> fields, CancellationToken cancellationToken = default)
        {
            foreach (var field in fields)
            {
                if (field != null)
                {
                    if (RequiresEncapsulation(field))
                    {
                        WriteEncapsulatedField(field);
                    }
                    else
                    {
                        _internalBuffer.Append(field);
                    }
                }

                _internalBuffer.Append(_options.DelimiterCharacter);
            }

            _internalBuffer.Append("\r\n");

            await _baseStream.WriteAsync(_internalBuffer, cancellationToken);

            _internalBuffer.Clear();
        }

        private void WriteEncapsulatedField(string field)
        {
            _internalBuffer.Append(_options.QuoteCharacter);

            foreach (char character in field)
            {
                if (character == _options.QuoteCharacter)
                {
                    _internalBuffer.Append(character);
                }

                _internalBuffer.Append(character);
            }

            _internalBuffer.Append(_options.QuoteCharacter);
        }

        private bool RequiresEncapsulation(string field)
        {
            foreach (char character in field)
            {
                if (character == _options.DelimiterCharacter)
                {
                    return true;
                }

                if (character == _options.QuoteCharacter)
                {
                    return true;
                }

                if (character == '\r' || character == '\n')
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            _baseStream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _baseStream.DisposeAsync();
        }
    }
}
