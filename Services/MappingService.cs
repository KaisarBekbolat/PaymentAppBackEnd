using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
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

    public static IEnumerable<UserOperationsDto> OperationsToDto(this IQueryable<Payment> paymentQuery){
        var payment = paymentQuery.Include(x=>x.Card).Include(x=>x.Operator).ToList();

        return payment.Select(p=>{
            return new UserOperationsDto(){
            PaymentId = p.PaymentId,
            CardOwner =p.Card.Owner,
            CardNumberLast8 = p.Card.CardNumber.Substring(p.Card.CardNumber.Length - 8, 8),
            PhoneNumber = p.PhoneNumber,
            OperatorName = p.Operator.OperatorName,
            TimeSpan = p.TimeStamp
        };
        });
    }
}
