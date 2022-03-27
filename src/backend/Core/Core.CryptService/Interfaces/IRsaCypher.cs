namespace Core.CryptService.Interfaces;

public interface IRsaCypher
{
    string Crypt(string rsaPublicKey, string dataToCrypt);
    string Decrypt(string rsaPrivateKey, string dataToDecrypt);
    (string privateKey, string publicKey) GenerateKeys();
    string SignData(string rsaPrivateKey, byte[] buffer);
    bool VerifySignature(string rsaPublicKey, byte[] buffer, byte[] siggnedBuffer);
}