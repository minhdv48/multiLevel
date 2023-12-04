using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Multi_LevelModels.Models;

public partial class MultiLevelContext : DbContext
{
    public MultiLevelContext()
    {
    }

    public MultiLevelContext(DbContextOptions<MultiLevelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<TreeLevel> TreeLevels { get; set; }
    public virtual DbSet<HistoryPay> HistoryPays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        #region Get connectstring
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(path, false);
        IConfigurationRoot root = configurationBuilder.Build();
        string _connectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        #endregion
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Password)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Path).HasMaxLength(500);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profile");

            entity.Property(e => e.Address).HasMaxLength(2500);
            entity.Property(e => e.CardVerifyBy).HasMaxLength(500);
            entity.Property(e => e.CardVerifyDate).HasColumnType("datetime");
            entity.Property(e => e.CodeRefer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateJoin).HasColumnType("datetime");
            entity.Property(e => e.DateVerify).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(500);
            entity.Property(e => e.Idcard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDCard");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PathInvoice).HasMaxLength(500);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.ToTable("Setting");

            entity.Property(e => e.Profit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RootValue).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TreeLevel>(entity =>
        {
            entity.ToTable("TreeLevel");

            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
        });
        modelBuilder.Entity<HistoryPay>(entity =>
        {
            entity.ToTable("HistoryPay");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
