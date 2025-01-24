using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PaymentProject.Models;

public class Payment
{
    [BindNever]
    public long PaymentId{get;set;}   // WHY NOT GUID USED https://stackoverflow.com/a/10617658/20003693
    [Required]
    public string PhoneNumber{get;set;}
    [Required]
    public decimal Amount{get;set;}
    [Required]
    public DateTime TimeStamp{get;set;}
    [Required]
    public long OperatorId { get; set; }
    [Required]
    public long CardInfoId { get; set; }

    public Operator? Operator{get;set;} // Navigation property
    public CardInfo? Card {get;set;} // Navigation property
}
