using System.Reflection.Metadata;

namespace Core.Utils
{
    public class CommonConstants
    {
        public const string UniqueClaimName = "unique";
        public const string SessionClaimName = "sessionId";
        
        #region Roles

        public const string UserRole = "User";
        public const string ProtocoledUser = "ProtocoledUser";
        public const string AdminRole = "Admin";

        #endregion

        #region KafkaTopics

        public const string MessagesTopic = "messages";
        public const string DisconnectMessagesTopid = "disconnectMessages";
        
        
        #endregion

        #region CacheSettings

        public const int CacheDialogMessages = 20;
        public const int CountMessagesInPage = 100;

        public const int MinutesStoreDialogMessages = 60;
        public const int MinutesConnectionsInCache = 60;
        public const int MinutesSessionInCache = 60;
        public const int DialogListCache = 60;
        
        
        #endregion

        #region Cache_CommonKeys

        public const string MessageCacheInitializerKey = "Initializer_Set";

        #endregion
    }
}
