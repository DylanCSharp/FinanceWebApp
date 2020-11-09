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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=19005775.database.windows.net;Initial Catalog=FinanceApplication;Persist Security Info=True;User ID=admin19005775;Password=Dylancox1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyCar>(entity =>
            {
                entity.HasKey(e => e.IdCar)
                    .HasName("PK__BUY_CAR__2BF8FA1E75A3B15D");

                entity.ToTable("BUY_CAR");

                entity.Property(e => e.IdCar).HasColumnName("ID_CAR");

                entity.Property(e => e.CarDeposit)
                    .HasColumnName("CAR_DEPOSIT")
                    .HasColumnType("money");

                entity.Property(e => e.CarInsurance)
                    .HasColumnName("CAR_INSURANCE")
                    .HasColumnType("money");

                entity.Property(e => e.CarInterest).HasColumnName("CAR_INTEREST");

                entity.Property(e => e.CarMake)
                    .HasColumnName("CAR_MAKE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CarPurchase)
                    .HasColumnName("CAR_PURCHASE")
                    .HasColumnType("money");

                entity.Property(e => e.MonthlyCarRepayment)
                    .HasColumnName("MONTHLY_CAR_REPAYMENT")
                    .HasColumnType("money");

                entity.Property(e => e.TotalCarRepayment)
                    .HasColumnName("TOTAL_CAR_REPAYMENT")
                    .HasColumnType("money");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.BuyCar)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BUY_CAR__USERS_I__60A75C0F");
            });

            modelBuilder.Entity<Cost>(entity =>
            {
                entity.HasKey(e => e.CostsId)
                    .HasName("PK__COST__1F8529622706F07C");

                entity.ToTable("COST");

                entity.Property(e => e.CostsId).HasColumnName("COSTS_ID");

                entity.Property(e => e.FinalIncome)
                    .HasColumnName("FINAL_INCOME")
                    .HasColumnType("money");

                entity.Property(e => e.NormalExpenses)
                    .HasColumnName("NORMAL_EXPENSES")
                    .HasColumnType("money");

                entity.Property(e => e.PostDeductions)
                    .HasColumnName("POST_DEDUCTIONS")
                    .HasColumnType("money");

                entity.Property(e => e.SpendableIncome)
                    .HasColumnName("SPENDABLE_INCOME")
                    .HasColumnType("money");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Cost)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__COST__USERS_ID__693CA210");
            });

            modelBuilder.Entity<GeneralExpenses>(entity =>
            {
                entity.HasKey(e => e.IdExpenses)
                    .HasName("PK__GENERAL___91E35883B26F793A");

                entity.ToTable("GENERAL_EXPENSES");

                entity.Property(e => e.IdExpenses).HasColumnName("ID_EXPENSES");

                entity.Property(e => e.Groceries)
                    .HasColumnName("GROCERIES")
                    .HasColumnType("money");

                entity.Property(e => e.GrossIncome)
                    .HasColumnName("GROSS_INCOME")
                    .HasColumnType("money");

                entity.Property(e => e.Other)
                    .HasColumnName("OTHER")
                    .HasColumnType("money");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("money");

                entity.Property(e => e.TaxDeducted)
                    .HasColumnName("TAX_DEDUCTED")
                    .HasColumnType("money");

                entity.Property(e => e.Travel)
                    .HasColumnName("TRAVEL")
                    .HasColumnType("money");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.Property(e => e.WaterLights)
                    .HasColumnName("WATER_LIGHTS")
                    .HasColumnType("money");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.GeneralExpenses)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GENERAL_E__USERS__6383C8BA");
            });

            modelBuilder.Entity<RentBuyProperty>(entity =>
            {
                entity.HasKey(e => e.IdRentBuy)
                    .HasName("PK__RENT_BUY__6BC779B2485D2E3A");

                entity.ToTable("RENT_BUY_PROPERTY");

                entity.Property(e => e.IdRentBuy).HasColumnName("ID_RENT_BUY");

                entity.Property(e => e.MonthlyHomeRepayment)
                    .HasColumnName("MONTHLY_HOME_REPAYMENT")
                    .HasColumnType("money");

                entity.Property(e => e.PropertyDeposit)
                    .HasColumnName("PROPERTY_DEPOSIT")
                    .HasColumnType("money");

                entity.Property(e => e.PropertyInterest).HasColumnName("PROPERTY_INTEREST");

                entity.Property(e => e.PropertyMonthsrepay).HasColumnName("PROPERTY_MONTHSREPAY");

                entity.Property(e => e.PropertyPurchase)
                    .HasColumnName("PROPERTY_PURCHASE")
                    .HasColumnType("money");

                entity.Property(e => e.RentMonthly)
                    .HasColumnName("RENT_MONTHLY")
                    .HasColumnType("money");

                entity.Property(e => e.TotalHomeRepayment)
                    .HasColumnName("TOTAL_HOME_REPAYMENT")
                    .HasColumnType("money");

                entity.Property(e => e.UsersId).HasColumnName("USERS_ID");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.RentBuyProperty)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RENT_BUY___USERS__66603565");
            });

            modelBuilder.Entity<Savings>(entity =>
            {
                entity.ToTable("SAVINGS");

                entity.Property(e => e.SavingsId).HasColumnName("SAVINGS_ID");

                entity.Property(e => e.MonthlyAmountTosave)
                    .HasColumnName("MONTHLY_AMOUNT_TOSAVE")
                    .HasColumnType("money");

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
                    .HasConstraintName("FK__SAVINGS__USERS_I__6C190EBB");
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
