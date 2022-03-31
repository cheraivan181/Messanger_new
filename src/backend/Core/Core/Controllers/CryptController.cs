using System;
using Core.ApiRequests.Crypt;
using Core.ApiResponses.Base;
using Core.ApiResponses.Crypt;
using Core.CryptService.Interfaces;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

/// <summary>
/// Only crypto provider. Will be removed after migrate in client side 
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class CryptControlle : BaseController
{
    private readonly IRsaCypher _rsaCypher;
    private readonly IAesCypher _aesCypher;
    
    public CryptControlle(IServiceProvider serviceProvider,
        IUserRepository userRepository,
        IRsaCypher rsaCypher,
        IAesCypher aesCypher) 
            : base(serviceProvider, userRepository)
    {
        _rsaCypher = rsaCypher;
        _aesCypher = aesCypher;
    }
    
    [HttpGet("getRsaKeys")]
    public IActionResult GetRsaKeys()
    {
        var keys = _rsaCypher.GenerateKeys();
        var getRsaKeyResponse = new GetRsaKeysResponse()
        {
            PublicKey = keys.publicKey,
            PrivateKey = keys.privateKey
        };

        var response = SucessResponseBuilder.Build(getRsaKeyResponse);
        return Ok(response);
    }
    
    [HttpPost("rsaCrypt")]
    public IActionResult RsaCrypt([FromBody]RsaCryptRequest rsaCryptRequest)
    {
        var result = _rsaCypher.Crypt(rsaCryptRequest.PublicKey, rsaCryptRequest.Text);
        var cryptedResponse = new CryptResponse()
        {
            CryptedText = result
        };

        var response = SucessResponseBuilder.Build(cryptedResponse);
        return Ok(response);
    }
    
    [HttpPost("rsaDecrypt")]
    public IActionResult RsaDecrypt([FromBody]RsaDecryptRequest rsaDecryptRequest)
    {
        var result = _rsaCypher.Decrypt(rsaDecryptRequest.PrivateKey, rsaDecryptRequest.CryptedText);
        var decryptedResponse = new DecryptResponse()
        {
            DecrtyptedText = result
        };

        var response = SucessResponseBuilder.Build(decryptedResponse);
        return Ok(response);
    }

    
    [HttpPost("aesCrypt")]
    public IActionResult AesCrypt([FromBody]AesCryptRequest request)
    {
        var result = _aesCypher.Crypt(request.Key, request.IV, request.Text);
        var cryptedResponse = new CryptResponse()
        {
            CryptedText = result
        };

        var response = SucessResponseBuilder.Build(cryptedResponse);
        return Ok(response);
    }
    
    
    [HttpPost("aesDecrypt")]
    public IActionResult AesDecrypt([FromBody]AesDecryptRequest decryptRequest)
    {
        var result = _aesCypher.Decrypt(decryptRequest.CryptedText, decryptRequest.CypherKey, decryptRequest.IV);
        var decryptedResponse = new DecryptResponse()
        {
            DecrtyptedText = result
        };

        var response = SucessResponseBuilder.Build(decryptedResponse);
        return Ok(response);
    }
    
    
    [HttpGet("getaeskey")]
    public IActionResult GetAesKeyAndIv()
    {
        var result = _aesCypher.GetAesKeyAndIv();
        var getAesKeysResponse = new GetAesKeysResponse()
        {
            Key = result.key,
            Iv = result.iv
        };

        var response = SucessResponseBuilder.Build(getAesKeysResponse);
        return Ok(response);
    }
}