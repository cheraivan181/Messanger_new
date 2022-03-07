namespace Core.IdentityService.Domain.Options
{
    public class TokenOptions
    {
        public string Key { get; set; } // ключ для подписи access-token
        public string CypherKey { get; set; } //ключ для шифрования refresh-token (longsession)
        public bool IsJwe { get; set; }
    }
}
