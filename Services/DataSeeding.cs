using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentProject.Models;

namespace PaymentProject.Services;

public static class DataSeeding
{
    public static async Task SeedDatabase(DataContext context) {   // As CardInfo could be added dynamically, need to seed db by Operators
        context.Database.Migrate();
        if(context.PhoneOperators.Count()==0){
            Operator alterl = new Operator(){
                OperatorName="Altel",
                OperatorPrefixes = new(){"700", "708"}
            };
            Operator kcell = new Operator(){
                OperatorName="Kcell",
                OperatorPrefixes = new(){"701", "702", "775", "778"}
            };
            Operator tele2 = new Operator(){
                OperatorName="Tele2",
                OperatorPrefixes = new(){"707", "747"}
            };
            context.PhoneOperators.AddRange(alterl, kcell, tele2);
            await context.SaveChangesAsync();
        } // Sample data for refernce was taken from here "https://www.chaspik.by/telefonnye-kody-mobilnyh-operatorov-kazahstana/"

    }
}
