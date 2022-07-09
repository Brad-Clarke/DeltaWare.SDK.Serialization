using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Serialization.Csv.Reading
{
    public interface ICsvReader
    {
        int LineNumber { get; }

        int LinePosition { get; }

        /// <inheritdoc cref="StreamReader.EndOfStream"/>>
        bool EndOfStream { get; }

        /// <inheritdoc cref="StreamReader.BaseStream"/>>
        StreamReader BaseStream { get; }

        /// <inheritdoc cref="StreamReader.BaseStream"/>>
        Encoding CurrentEncoding { get; }

        Task<string> ReadAsync();

        Task<string[]> ReadLineAsync();

        Task<string[][]> ReadToEndAsync();
        string Read();

        string[] ReadLine();

        string[][] ReadToEnd();
    }
}
