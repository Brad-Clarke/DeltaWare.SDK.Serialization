using System.IO;

namespace DeltaWare.SDK.Serialization.Html
{
    public interface IHtmlSerializer
    {
        T Deserialize<T>(Stream stream) where T : class;
    }
}