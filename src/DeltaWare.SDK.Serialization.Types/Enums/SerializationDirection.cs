using System;

namespace DeltaWare.SDK.Serialization.Types.Enums
{
    [Flags]
    public enum SerializationDirection
    {
        None = 0,
        Serialization = 1,
        Deserialization = 2
    }
}