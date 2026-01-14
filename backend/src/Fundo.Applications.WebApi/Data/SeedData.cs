using System;
using System.Linq;
using Fundo.Applications.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.WebApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Fundo.Applications.WebApi.Data.SeedData");

            try
            {
                using var context = new LoanDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<LoanDbContext>>());

                if (context.Loans.Any())
                {
                    logger.LogInformation("Database already contains loan data. Skipping seed.");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // PRÉSTAMOS CON HISTORIA COHERENTE
                var loans = new[]
                {
                    // 1. Maria Silva - Préstamo activo con 2 pagos realizados
                    new Loan
                    {
                        ApplicantName = "Maria Silva",
                        Amount = 1500.00m,
                        CurrentBalance = 500.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        UpdateAt = DateTime.UtcNow.AddDays(-15)
                    },

                    // 2. Juan Perez - Préstamo PAGADO completamente
                    new Loan
                    {
                        ApplicantName = "Juan Perez",
                        Amount = 3000.00m,
                        CurrentBalance = 0.00m,
                        IsActive = 0,
                        CreatedAt = DateTime.UtcNow.AddMonths(-8),
                        UpdateAt = DateTime.UtcNow.AddMonths(-2)
                    },

                    // 3. Carlos Rodriguez - Préstamo activo reciente, 1 pago
                    new Loan
                    {
                        ApplicantName = "Carlos Rodriguez",
                        Amount = 5000.00m,
                        CurrentBalance = 3500.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        UpdateAt = DateTime.UtcNow.AddDays(-5)
                    },

                    // 4. Ana Martinez - Préstamo activo con 3 pagos
                    new Loan
                    {
                        ApplicantName = "Ana Martinez",
                        Amount = 2500.00m,
                        CurrentBalance = 1000.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-5),
                        UpdateAt = DateTime.UtcNow.AddDays(-10)
                    },

                    // 5. Pedro Gonzalez - Préstamo PAGADO hace tiempo
                    new Loan
                    {
                        ApplicantName = "Pedro Gonzalez",
                        Amount = 4000.00m,
                        CurrentBalance = 0.00m,
                        IsActive = 0,
                        CreatedAt = DateTime.UtcNow.AddMonths(-10),
                        UpdateAt = DateTime.UtcNow.AddMonths(-4)
                    },

                    // 6. Laura Torres - Préstamo activo sin pagos aún
                    new Loan
                    {
                        ApplicantName = "Laura Torres",
                        Amount = 2000.00m,
                        CurrentBalance = 2000.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-15),
                        UpdateAt = null
                    }
                };

                context.Loans.AddRange(loans);
                context.SaveChanges();

                logger.LogInformation("Successfully seeded {LoanCount} loans", loans.Length);

                // PAGOS COHERENTES CON LOS PRÉSTAMOS
                var mariaSilva = context.Loans.First(l => l.ApplicantName == "Maria Silva");
                var juanPerez = context.Loans.First(l => l.ApplicantName == "Juan Perez");
                var carlosRodriguez = context.Loans.First(l => l.ApplicantName == "Carlos Rodriguez");
                var anaMartinez = context.Loans.First(l => l.ApplicantName == "Ana Martinez");
                var pedroGonzalez = context.Loans.First(l => l.ApplicantName == "Pedro Gonzalez");

                var payments = new[]
                {
                    // Maria Silva - 2 pagos de $500 (total $1000 pagado)
                    new Payment
                    {
                        IdLoan = mariaSilva.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2).AddDays(-10)
                    },
                    new Payment
                    {
                        IdLoan = mariaSilva.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-15)
                    },

                    // Juan Perez - 3 pagos que completaron los $3000
                    new Payment
                    {
                        IdLoan = juanPerez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-6)
                    },
                    new Payment
                    {
                        IdLoan = juanPerez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    },
                    new Payment
                    {
                        IdLoan = juanPerez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2)
                    },

                    // Carlos Rodriguez - 1 pago de $1500
                    new Payment
                    {
                        IdLoan = carlosRodriguez.Id,
                        Amount = 1500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },

                    // Ana Martinez - 3 pagos de $500 (total $1500)
                    new Payment
                    {
                        IdLoan = anaMartinez.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    },
                    new Payment
                    {
                        IdLoan = anaMartinez.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2)
                    },
                    new Payment
                    {
                        IdLoan = anaMartinez.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    },

                    // Pedro Gonzalez - 4 pagos que completaron los $4000
                    new Payment
                    {
                        IdLoan = pedroGonzalez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-9)
                    },
                    new Payment
                    {
                        IdLoan = pedroGonzalez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-7)
                    },
                    new Payment
                    {
                        IdLoan = pedroGonzalez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-5)
                    },
                    new Payment
                    {
                        IdLoan = pedroGonzalez.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    }
                };

                context.Payments.AddRange(payments);
                context.SaveChanges();

                logger.LogInformation("Successfully seeded {PaymentCount} payments", payments.Length);
                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
    }
}
