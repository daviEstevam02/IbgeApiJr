using IbgeDesafio.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace IbgeDesafio.Api.Data;

public class IbgeDesafioContext : DbContext
{
    public IbgeDesafioContext(DbContextOptions<IbgeDesafioContext> options) : base(options)
    {
    }
    
    //Tipando Set com os Models
    public DbSet<User> Users { get; set; }
    public DbSet<Locale> Locales { get; set; }

    //Criando colunas que deve ser migradas ao DB
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Email)
            .HasColumnName("Email")
            .HasColumnType("varchar(150)");
        modelBuilder.Entity<User>().HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>().Property(u => u.Username)
            .HasColumnName("Username")
            .HasMaxLength(150);
        modelBuilder.Entity<User>().Property(u => u.Password)
            .HasColumnName("Password")
            .HasMaxLength(16);

        modelBuilder.Entity<Locale>().HasKey(l => l.Id);
        modelBuilder.Entity<Locale>().Property(l => l.Id)
            .HasColumnName("Id")
            .HasColumnType("char(7)");
        modelBuilder.Entity<Locale>().Property(l => l.City)
            .HasColumnName("City")
            .HasColumnType("nvarchar(80)")
            .IsRequired(false);
        modelBuilder.Entity<Locale>().Property(u => u.State)
            .HasColumnName("State")
            .HasColumnType("char(2)")
            .IsRequired(false);
        base.OnModelCreating(modelBuilder);
    }
}