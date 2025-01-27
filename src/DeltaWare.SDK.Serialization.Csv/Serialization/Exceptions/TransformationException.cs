using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Exceptions
{
    public abstract class TransformationException(string message, Exception? innerException = null) : Exception(message, innerException);
}
