using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinanceWebApp.Models
{
    public partial class FinanceApplicationContext : DbContext
    {
        public FinanceApplicationContext()
        {
        }

        public FinanceApplicationContext(DbContextOptions<FinanceApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuyCar> BuyCar { get; set; }
        public virtual DbSet<Cost> Cost { get; set; }
        public virtual DbSet<GeneralExpenses> GeneralExpenses { get; set; }
        public virtual DbSet<RentBuyProperty> RentBuyProperty { get; set; }
        public virtual DbSet<Savings> Savings { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyCar>(entity =>
            {
                entity.HasKey(e => e.IdCar)
                    .HasName("PK__BUY_CAR__2BF8FA1E63182EAF");

                entity.ToTable("BUY_CAR");

                entity.Property(e => e.IdCar).HasColumnName("ID_CAR");

                entity.Property(e => e.CarDeposit)
                    .HasColumnName("CAR_DEPOSIT")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CarInsurance)
                    .HasColumnName("CAR_INSURANCE")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CarInterest).HasColumnName("CAR_INTEREST");

                entity.Property(e => e.CarMake)
                    .HasColumnName("CAR_MAKE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarPurchase)
                    .HasColumnName("CAR_PURCHASE")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MonthlyCarRepayment)
                    .HasColumnName("MONTHLY_CAR_REPAYMENT")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TotalCarRepayment)
                    .HasColumnName("TOTAL_CAR_REPAYMENT")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.BuyCar)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BUY_CAR__USERS_I__1332DBDC");
            });

            modelBuilder.Entity<Cost>(entity =>
            {
                entity.HasKey(e => e.CostsId)
                    .HasName("PK__COST__1F8529625A615460");

                entity.ToTable("COST");

                entity.Property(e => e.CostsId).HasColumnName("COSTS_ID");

                entity.Property(e => e.FinalIncome)
                    .IsRequired()
                    .HasColumnName("FINAL_INCOME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormalExpenses)
                    .IsRequired()
                    .HasColumnName("NORMAL_EXPENSES")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PostDeductions)
                    .IsRequired()
                    .HasColumnName("POST_DEDUCTIONS")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SpendableIncome)
                    .IsRequired()
                    .HasColumnName("SPENDABLE_INCOME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Cost)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COST__USERS_ID__10566F31");
            });

            modelBuilder.Entity<GeneralExpenses>(entity =>
            {
                entity.HasKey(e => e.IdExpenses)
                    .HasName("PK__GENERAL___91E35883C9E83194");

                entity.ToTable("GENERAL_EXPENSES");

                entity.Property(e => e.IdExpenses).HasColumnName("ID_EXPENSES");

                entity.Property(e => e.Groceries)
                    .HasColumnName("GROCERIES")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GrossIncome)
                    .HasColumnName("GROSS_INCOME")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Other)
                    .HasColumnName("OTHER")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TaxDeducted)
                    .HasColumnName("TAX_DEDUCTED")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Travel)
                    .HasColumnName("TRAVEL")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.Property(e => e.WaterLights)
                    .HasColumnName("WATER_LIGHTS")
                    .HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.GeneralExpenses)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GENERAL_E__USERS__7C4F7684");
            });

            modelBuilder.Entity<RentBuyProperty>(entity =>
            {
                entity.HasKey(e => e.IdRentBuy)
                    .HasName("PK__RENT_BUY__6BC779B2939C38DE");

                entity.ToTable("RENT_BUY_PROPERTY");

                entity.Property(e => e.IdRentBuy).HasColumnName("ID_RENT_BUY");

                entity.Property(e => e.MonthlyHomeRepayment)
                    .HasColumnName("MONTHLY_HOME_REPAYMENT")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PropertyDeposit)
                    .HasColumnName("PROPERTY_DEPOSIT")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PropertyInterest).HasColumnName("PROPERTY_INTEREST");

                entity.Property(e => e.PropertyMonthsrepay).HasColumnName("PROPERTY_MONTHSREPAY");

                entity.Property(e => e.PropertyPurchase)
                    .HasColumnName("PROPERTY_PURCHASE")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RentMonthly)
                    .HasColumnName("RENT_MONTHLY")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalHomeRepayment)
                    .HasColumnName("TOTAL_HOME_REPAYMENT")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.RentBuyProperty)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RENT_BUY___USERS__0D7A0286");
            });

            modelBuilder.Entity<Savings>(entity =>
            {
                entity.ToTable("SAVINGS");

                entity.Property(e => e.SavingsId).HasColumnName("SAVINGS_ID");

                entity.Property(e => e.MonthlyAmountTosave)
                    .IsRequired()
                    .HasColumnName("MONTHLY_AMOUNT_TOSAVE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SavingAmount)
                    .HasColumnName("SAVING_AMOUNT")
                    .HasColumnType("money");

                entity.Property(e => e.SavingInterestrate).HasColumnName("SAVING_INTERESTRATE");

                entity.Property(e => e.SavingReason)
                    .IsRequired()
                    .HasColumnName("SAVING_REASON")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SavingYears).HasColumnName("SAVING_YEARS");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Savings)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SAVINGS__USERS_I__0A9D95DB");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserFullname)
                    .IsRequired()
                    .HasColumnName("USER_FULLNAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("USER_PASSWORD")
                    .IsUnicode(false);

                entity.Property(e => e.UserPhone)
                    .IsRequired()
                    .HasColumnName("USER_PHONE")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
