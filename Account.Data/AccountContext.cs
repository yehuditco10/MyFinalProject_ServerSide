using Account.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Account.Data
{
    public class AccountContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Entities.Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options)
  : base(options)
        { }
        public AccountContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["AccountConnection"]);
                base.OnConfiguring(optionsBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Entities.Account>().ToTable("Account");
            modelBuilder.Entity<Customer>()
                               .Property(customer => customer.Id);
            modelBuilder.Entity<Customer>()
                              .Property(customer => customer.FirstName);
            modelBuilder.Entity<Customer>()
                .Property(customer => customer.LastName);
            modelBuilder.Entity<Customer>()
                  .HasIndex(customer => customer.Email)
                  .IsUnique();
            modelBuilder.Entity<Customer>()
                .Property(customer => customer.Password)
           .IsRequired();

            //VerificationEmail

            modelBuilder.Entity<Entities.Account>()
                .Property(account => account.Id)
           .IsRequired(); ;
            modelBuilder.Entity<Entities.Account>()
               .Property(account => account.CustomerId)
           .IsRequired(); ;
            modelBuilder.Entity<Entities.Account>()
                  .Property(account => account.Opendate)
                  .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Entities.Account>()
           .Property(account => account.Balance)
           .HasDefaultValue(1000);



        }
    }
}
