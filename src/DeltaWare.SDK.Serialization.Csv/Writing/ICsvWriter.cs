using System.IO;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Writing
{
    public interface ICsvWriter
    {
        /// <inheritdoc cref="StreamWriter.BaseStream"/>>
        StreamWriter BaseStream { get; }

        WriteMode Mode { get; }

        void Dispose();

        /// <inheritdoc cref="StreamWriter.Flush"/>
        void Flush();

        /// <inheritdoc cref="StreamWriter.FlushAsync"/>
        Task FlushAsync();

        Task WriteAllAsync(string[][] lines);

        Task WriteAsync(string field, WriteOperation type = WriteOperation.Field);

        Task WriteLineAsync(string[] line);

        void Write(string field, WriteOperation type = WriteOperation.Field);

        void WriteAll(string[][] lines);

        void WriteLine(string[] line);
    }
}
