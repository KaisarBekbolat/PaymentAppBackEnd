using System;
using Microsoft.EntityFrameworkCore;

namespace PaymentProject.Models;

public class DataContext:DbContext
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<CardInfo> CardInfos { get; set; }
    public DbSet<Operator> PhoneOperators { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.Property(e => e.OperatorPrefixesJson).HasColumnName("OperatorPrefixesJson");
        });
        
        modelBuilder.Entity<Operator>()
        .HasIndex(o => o.OperatorName)
        .IsUnique();

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(o => o.OperatorId); // Primary key
            entity.Property(o => o.OperatorId)
                .ValueGeneratedOnAdd(); // Auto-increment
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId); // Primary key
            entity.Property(p => p.PaymentId)
                .ValueGeneratedOnAdd(); // Auto-increment
        });

        modelBuilder.Entity<CardInfo>(entity =>
        {
            entity.HasKey(c => c.CardId); // Primary key
            entity.Property(c => c.CardId)
                .ValueGeneratedOnAdd(); // Auto-increment
        });

    }
}
