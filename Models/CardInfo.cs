using System;

namespace PaymentProject.Models;

public class CardInfo
{
    public long CardId { get; set; }
    public string CardNumber { get; set; } // Зашифрованные данные 8/16
    public string CVCNumber { get; set; } // Полностью зашифрованное
    public string Owner { get; set; } // В оригинале
    public string ExpirationDate { get; set; } // В оригинале
}