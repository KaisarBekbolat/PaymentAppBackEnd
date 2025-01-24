using System;

namespace PaymentProject.Models.DTOs;

public class UserOperationsDto
{
    public Guid PaymentId { get; set; }

    public string CardOwner{get;set;}
    public string CardNumberLast4 { get; set; }

    public string PhoneNumber{get;set;}
    public string OperatorName { get; set; }
    public DateTime PaymentDate { get; set; }

    public DateTime TimeSpan{get;set;}
}
