using System;
using Microsoft.AspNetCore.DataProtection;
using PaymentProject.Models;
using PaymentProject.Models.DTOs;

namespace PaymentProject.Services;

public static class IdRetrivingService
{
    public static Operator? OperatorRetriving(DataContext context, PaymentRequestDto paymentRequestDto){
        Operator? opertator = context.PhoneOperators.FirstOrDefault(x=>x.OperatorName.Equals(paymentRequestDto.OperatorName));
        // TODO catch excpetion at controller
        if(opertator !=  null && opertator.OperatorPrefixes.Contains(paymentRequestDto.Prefix)){
            return opertator;
        }
        throw new KeyNotFoundException($"No operator with given name: {paymentRequestDto.OperatorName} OR prefix {paymentRequestDto.Prefix}");

        //TODO As adding Kcell and Tele2 by user is bad, it is good to make additional service that populates DB by Admin
    }

    public static CardInfo CardInfoRetriving(DataContext context, PaymentRequestDto paymentRequestDto, DataEncryptionService dataProtector){
        var last8 = paymentRequestDto.CardNumber.Substring(8, 8);
        
        CardInfo? cardInfo = context.CardInfos.FirstOrDefault(x=>x.CardNumber.EndsWith($"{last8}").Equals(last8) 
            && x.Owner.Equals(paymentRequestDto.CardHolder) );
            
        if(cardInfo is null){
                // Encrypt the first 8 digits of the card number
            var first8Encrypted = dataProtector.Encrypt(paymentRequestDto.CardNumber.Substring(0, 8));
            
            // Combine encrypted first 8 and unencrypted last 8
            var encryptedCardNumber = first8Encrypted + last8;

            // Create a new CardInfo record
            var newCardInfo = new CardInfo
            {
                CardNumber = encryptedCardNumber, // Store encrypted + unencrypted card number
                CVCNumber = dataProtector.Encrypt(paymentRequestDto.CVC),
                Owner = paymentRequestDto.CardHolder,
                ExpirationDate = paymentRequestDto.ExpirationDate
            };

            // Add to the database and save changes
            cardInfo = context.CardInfos.Add(newCardInfo).Entity;
            context.SaveChanges();




            // var cardCreated = context.CardInfos.Add(new CardInfo(){
            //     CardNumber=dataProtector.MaskCardNumber(paymentRequestDto.CardNumber), //Encryption made
            //     CVCNumber=dataProtector.Encrypt(paymentRequestDto.CVC),
            //     Owner=paymentRequestDto.CardHolder,
            //     ExpirationDate=paymentRequestDto.ExpirationDate
            // }).Entity;
            // context.SaveChanges();
        }
        return cardInfo;
    }
}
