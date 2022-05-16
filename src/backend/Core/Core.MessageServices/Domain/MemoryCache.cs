using System.Collections.Concurrent;

namespace Core.MessageServices.Services.Implementations;

internal class KeysCache
{
    public static ConcurrentDictionary<Guid, string> Cache = new ConcurrentDictionary<Guid, string>();
}