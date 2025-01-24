using System;

namespace PaymentProject.Models.DTOs;

public class UserOperationsDto
{
    public long PaymentId { get; set; }

    public string CardOwner{get;set;}
    public string CardNumberLast8 { get; set; }

    public string PhoneNumber{get;set;}
    public string OperatorName { get; set; }
    public DateTime TimeSpan{get;set;}
}
