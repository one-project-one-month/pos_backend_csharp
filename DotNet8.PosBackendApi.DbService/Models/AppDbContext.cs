using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNet8.PosBackendApi.DbService.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductCategory> TblProductCategories { get; set; }

    public virtual DbSet<TblSaleInvoice> TblSaleInvoices { get; set; }

    public virtual DbSet<TblSaleInvoiceDetail> TblSaleInvoiceDetails { get; set; }

    public virtual DbSet<TblSequence> TblSequences { get; set; }

    public virtual DbSet<TblShop> TblShops { get; set; }

    public virtual DbSet<TblStaff> TblStaffs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Tbl_Customer");

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TownshipCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Product");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductCategoryCode).HasMaxLength(50);
            entity.Property(e => e.ProductCode).HasMaxLength(50);
            entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PK_ProductCategory");

            entity.ToTable("Tbl_ProductCategory");

            entity.Property(e => e.ProductCategoryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblSaleInvoice>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceId);

            entity.ToTable("Tbl_SaleInvoice");

            entity.Property(e => e.Change).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerAccountNo).HasMaxLength(20);
            entity.Property(e => e.CustomerCode).HasMaxLength(50);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentType).HasMaxLength(10);
            entity.Property(e => e.ReceiveAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SaleInvoiceDateTime).HasColumnType("datetime");
            entity.Property(e => e.StaffCode).HasMaxLength(50);
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoucherNo).HasMaxLength(20);
        });

        modelBuilder.Entity<TblSaleInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceDetailId);

            entity.ToTable("Tbl_SaleInvoiceDetail");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductCode).HasMaxLength(50);
            entity.Property(e => e.VoucherNo).HasMaxLength(20);
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.ToTable("Tbl_Sequence");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Field).HasMaxLength(50);
        });

        modelBuilder.Entity<TblShop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK_Shop");

            entity.ToTable("Tbl_Shop");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShopCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShopName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblStaff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_Staff");

            entity.ToTable("Tbl_Staff");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
