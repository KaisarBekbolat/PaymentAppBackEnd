using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentProject.Models.DTOs;

public class PaymentRequestDto      // This class used for BINDING data from Front-end JSON
{
    //Operator related info 
    public string PhoneNumber{get;set;}
    public string Prefix { get; set; }
    public string OperatorName { get; set; }
    // Payments related info
    public decimal Amount{get;set;}
    public DateTime TimeStamp{get;set;}

    // Card related info
    public string CardHolder{get;set;}
    public string CardNumber{get;set;}
    public string CVC{get;set;}
    public string ExpirationDate{get;set;}
}
