using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TickdTechnical.Models
{
    public partial class TickdContext : DbContext
    {
        public TickdContext()
        {
        }

        public TickdContext(DbContextOptions<TickdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAccounts> TblAccounts { get; set; }
        public virtual DbSet<TblMeterReadings> TblMeterReadings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAccounts>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tbl_accounts");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountFirstName)
                    .IsRequired()
                    .HasColumnName("account_first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.AccountLastName)
                    .IsRequired()
                    .HasColumnName("account_last_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblMeterReadings>(entity =>
            {
                entity.HasKey(e => e.EntryId);

                entity.ToTable("tbl_meter_readings");

                entity.Property(e => e.EntryId)
                    .HasColumnName("entry_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.MeterReadValue)
                    .IsRequired()
                    .HasColumnName("meter_read_value")
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.MeterReadingDatetime)
                    .HasColumnName("meter_reading_datetime")
                    .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
