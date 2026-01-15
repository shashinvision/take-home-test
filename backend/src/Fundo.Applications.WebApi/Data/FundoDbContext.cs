using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Data
{
    public partial class FundoDbContext : DbContext
    {
        public FundoDbContext()
        {
        }

        public FundoDbContext(DbContextOptions<FundoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.HasIndex(e => e.Email, "UK_EMAIL")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD_HASH");
            });

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.ToTable("APPLICANT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Dni)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("DNI");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("LOAN");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.CurrentBalance)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("CURRENT_BALANCE");

                entity.Property(e => e.IdApplicant).HasColumnName("ID_APPLICANT");

                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_AT");

                entity.HasOne(d => d.IdApplicantNavigation)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.IdApplicant)
                    .HasConstraintName("LOAN_APPLICANT");
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
