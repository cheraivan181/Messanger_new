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

        public const string ReceiveMessages = "receiveMessageTopic";

        #endregion
    }
}
