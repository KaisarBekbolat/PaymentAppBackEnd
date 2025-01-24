using System;
using Microsoft.AspNetCore.DataProtection;


namespace PaymentProject.Services;

public class DataEncryptionService
{
    private readonly IDataProtector _protector;

    public DataEncryptionService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("CardInfoProtector");
    }

    public string Encrypt(string plainText)
    {
        return _protector.Protect(plainText);
    }
    public string MaskCardNumber(string cardNumber)
    {
        var first8Encrypted = Encrypt(cardNumber.Substring(0, 8));
        var last8 = cardNumber.Substring(8, 8);
        return first8Encrypted + last8;
    }
}
