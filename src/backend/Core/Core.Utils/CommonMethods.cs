using StackExchange.Redis;

namespace Core.Utils;

public static class CommonMethods
{
    public static RedisKey ToRedisKey(this string str) =>
        new RedisKey(str);

    public static Guid GetDialogId(Guid myDialogId, Guid dialog1Id, Guid dialog2Id) =>
        myDialogId == dialog1Id
            ? dialog2Id
            : dialog1Id;
}