using System;
using System.Reflection.Emit;
using PaymentProject.Models;
using PaymentProject.Models.DTOs;

namespace PaymentProject.Services;

public static class MappingService
{
    public static Payment DtoToPayment(this PaymentRequestDto paymentRequestDto, HttpContext context, DataContext dataContext){
        Operator? phoneOperator = null;
        try{
            phoneOperator = IdRetrivingService.OperatorRetriving(dataContext, paymentRequestDto);
            if(phoneOperator is null){
                Console.WriteLine("Phone opertor NULL *****************************************************************************************************************************************************************8");
            }
        }
        catch(KeyNotFoundException ex){
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        }
        catch(Exception ex){
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        if (phoneOperator == null)
        {
            throw new InvalidOperationException("Failed to retrieve operator.");
        }

        CardInfo cardInfo = IdRetrivingService.CardInfoRetriving(dataContext, paymentRequestDto, context.RequestServices.GetRequiredService<DataEncryptionService>());

        return new Payment(){
            PhoneNumber=paymentRequestDto.PhoneNumber,
            Amount = paymentRequestDto.Amount,
            TimeStamp = paymentRequestDto.TimeStamp,
            OperatorId = phoneOperator.OperatorId,
            CardInfoId = cardInfo.CardId
        };
    }
}
