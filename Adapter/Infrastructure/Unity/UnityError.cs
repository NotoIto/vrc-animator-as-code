using System;

namespace notoito.vrcanimatorascode.Adapter.Infrastructure.Unity
{
    public class SerializerError : DomainDefinedError
    {
        public SerializerError(string message, Exception e = null) : base($"SerializerError({message})", e) { }
    }
}
