using System.Security.Cryptography;
using Core.CryptService.Interfaces;
using Core.Utils;

namespace Core.CryptService.Impl;

public class RsaCypher : IRsaCypher
{
    private const string AlgoritmName = "SHA256";
		
		public string Crypt(string rsaPublicKey, string dataToCrypt)
		{
			var rsaPublicParameter = rsaPublicKey.FromJson<RSAParameters>();
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				rsa.ImportParameters(rsaPublicParameter);

				byte[] bytesToCrypt = Convert.FromBase64String(dataToCrypt);
				byte[] buffer = rsa.Encrypt(bytesToCrypt, false);

				return Convert.ToBase64String(buffer);
			}
		}

		public string Decrypt(string rsaPrivateKey, string dataToDecrypt)
		{
			byte[] bufferToDecrypt = Convert.FromBase64String( dataToDecrypt);

			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				var rsaPrivateParameter =rsaPrivateKey.FromJson<RSAParameters>();
				rsa.ImportParameters(rsaPrivateParameter);

				byte[] decryptedData = rsa.Decrypt(bufferToDecrypt, false);

				return Convert.ToBase64String(decryptedData);
			}
		}

		public (string privateKey, string publicKey) GenerateKeys()
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				RSAParameters publicKeyParameter = rsa.ExportParameters(false);
				RSAParameters privateKeyParameter = rsa.ExportParameters(true);

				string privateKey = privateKeyParameter.ToJson();
				string publicKey = publicKeyParameter.ToJson();

				return (privateKey, publicKey);
			}
		}

		public string SignData(string rsaPrivateKey ,byte[] buffer)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				RSAParameters privateKey = rsaPrivateKey.FromJson<RSAParameters>();
				rsa.ImportParameters(privateKey);

				byte[] signedHashValue = rsa.SignData(buffer, SHA1.Create());
				return Convert.ToBase64String(signedHashValue);
			}
		}

		public bool VerifySignature(string rsaPublicKey, byte[] buffer, byte[] siggnedBuffer)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
			{
				RSAParameters publicKey = rsaPublicKey.FromJson<RSAParameters>();
				rsa.ImportParameters(publicKey);

				if (rsa.VerifyData(buffer, SHA1.Create(), siggnedBuffer))
					return true;
				return false;
			}
		}

		private RSACryptoServiceProvider GetRsaCryptoProvider()
		{
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
			rsa.KeySize = 512;

			return rsa;
		}
}