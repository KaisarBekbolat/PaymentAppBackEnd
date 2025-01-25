using Microsoft.EntityFrameworkCore;
using PaymentProject.Models;
using PaymentProject.Services;
using PaymentProject.Services.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DataEncryptionService>();

builder.Services.AddDbContext<DataContext>(opts=>{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddCors(options=>{
    options.AddPolicy("ReactFront", policyBuilder=>{
        policyBuilder.WithOrigins("http://localhost:5173");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

builder.Services.AddStackExchangeRedisCache(redisOptions=>{
    string connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
});

builder.Services.AddDataProtection();
builder.Services.AddControllers();


var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider
    .GetRequiredService<DataContext>();

DataSeeding.SeedDatabase(context);

app.MapControllers();

app.UseCors("ReactFront");


// Configure the HTTP request pipeline.



app.Run();
