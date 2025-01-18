using DeltaWare.SDK.Serialization.Csv.Exceptions;
using DeltaWare.SDK.Serialization.Csv.Writing.Options;

namespace DeltaWare.SDK.Serialization.Csv.Writing
{
    internal class CsvStreamWriter
    {
        private readonly StreamWriter _baseStream;

        private readonly ICsvWriterOptions _options;

        private int _expectedRowCount = -1;

        public int LineNumber { get; private set; }

        public int LinePosition { get; private set; }

        public int Length { get; private set; }

        public CsvStreamWriter(StreamWriter baseStream, ICsvWriterOptions options)
        {
            _baseStream = baseStream;
            _options = options;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
            => await _baseStream.FlushAsync(cancellationToken);

        public async Task WriteLineAsync(IReadOnlyCollection<string> fields)
        {
            if (_expectedRowCount == -1)
            {
                _expectedRowCount = fields.Count;
            }
            else if (_expectedRowCount != fields.Count)
            {
                throw InvalidCsvDataException.InvalidColumnCount(LineNumber, _expectedRowCount, fields.Count);
            }

            foreach (var field in fields)
            {
            }
        }

        private async Task WriteFieldAsync(string field, CsvState state)
        {
            field.ToCharArray();
        }
    }
}
