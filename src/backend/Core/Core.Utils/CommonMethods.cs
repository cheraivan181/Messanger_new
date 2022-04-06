using StackExchange.Redis;

namespace Core.Utils;

public static class CommonMethods
{
    public static RedisKey ToRedisKey(this string str) =>
        new RedisKey(str);
}