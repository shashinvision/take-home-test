using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Data
{
    public partial class LoanDbContext : DbContext
    {
        public LoanDbContext()
        {
        }

        public LoanDbContext(DbContextOptions<LoanDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("LOAN");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.ApplicantName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("APPLICANT_NAME");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.CurrentBalance)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("CURRENT_BALANCE");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_AT");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("PAYMENT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.IdLoan).HasColumnName("ID_LOAN");

                entity.HasOne(d => d.IdLoanNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdLoan)
                    .HasConstraintName("FK_LOAN_PAYMENT");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
