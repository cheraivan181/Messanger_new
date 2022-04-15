using Front.Domain.Message;

namespace Front
{
    public static class GlobalStorage
    {
        public static bool IsAlive { get; set; }
        public static bool IsAuthenticated { get; set; }
        public static bool IsGlobalError { get; set; }

        public static Dictionary<Guid, List<Message>> Messages = new Dictionary<Guid, List<Message>>();
    }
}
