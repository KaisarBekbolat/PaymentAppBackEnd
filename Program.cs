using Microsoft.EntityFrameworkCore;
using PaymentProject.Models;
using PaymentProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DataEncryptionService>();

builder.Services.AddDbContext<DataContext>(opts=>{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]);
    opts.EnableSensitiveDataLogging(true);
});


builder.Services.AddDataProtection();
builder.Services.AddControllers();


var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider
    .GetRequiredService<DataContext>();

DataSeeding.SeedDatabase(context);

app.MapControllers();


// Configure the HTTP request pipeline.



app.Run();
