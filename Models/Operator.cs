using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PaymentProject.Models;

public class Operator
{
    public long OperatorId{get;set;}
    public string OperatorName{get;set;}
    [NotMapped]
    public List<string> OperatorPrefixes{get;set;}

    public string OperatorPrefixesJson{
        get=> JsonSerializer.Serialize(OperatorPrefixes ?? new List<string>());

        set=> OperatorPrefixes = string.IsNullOrEmpty(value) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(value);
    }
}
