namespace Core.CryptService.Interfaces;

public interface IAesCypher
{
    string Crypt(string key, string iv, string value);
    string Decrypt(string cryptedText, string key, string iv);
    string Decrypt(byte[] cryptedTextBuffer, byte[] keyBuffer, byte[] ivBuffer);
    (string key, string iv) GetAesKeyAndIv();
}