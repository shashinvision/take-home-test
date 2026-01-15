using System;
using System.Linq;
using Fundo.Applications.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fundo.Applications.WebApi.Data;

namespace Fundo.Applications.WebApi.Infraestructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Fundo.Applications.WebApi.Data.SeedData");

            try
            {
                using var context = new FundoDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<FundoDbContext>>());

                // Verificar si ya hay datos
                if (context.Applicants.Any())
                {
                    logger.LogInformation("Database already contains data. Skipping seed.");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // 1. PRIMERO LOS APPLICANTS (solicitantes)
                var applicants = new[]
                {
                    new Applicant
                    {
                        FullName = "Maria Silva",
                        Dni = "12345678-9"
                    },
                    new Applicant
                    {
                        FullName = "Juan Perez",
                        Dni = "23456789-0"
                    },
                    new Applicant
                    {
                        FullName = "Carlos Rodriguez",
                        Dni = "34567890-1"
                    },
                    new Applicant
                    {
                        FullName = "Ana Martinez",
                        Dni = "45678901-2"
                    },
                    new Applicant
                    {
                        FullName = "Pedro Gonzalez",
                        Dni = "56789012-3"
                    },
                    new Applicant
                    {
                        FullName = "Laura Torres",
                        Dni = "67890123-4"
                    }
                };

                context.Applicants.AddRange(applicants);
                context.SaveChanges();
                logger.LogInformation("Successfully seeded {ApplicantCount} applicants", applicants.Length);

                // 2. AHORA LOS PRÉSTAMOS (ya con IDs reales)
                var mariaSilva = context.Applicants.First(a => a.Dni == "12345678-9");
                var juanPerez = context.Applicants.First(a => a.Dni == "23456789-0");
                var carlosRodriguez = context.Applicants.First(a => a.Dni == "34567890-1");
                var anaMartinez = context.Applicants.First(a => a.Dni == "45678901-2");
                var pedroGonzalez = context.Applicants.First(a => a.Dni == "56789012-3");
                var lauraTorres = context.Applicants.First(a => a.Dni == "67890123-4");

                var loans = new[]
                {
                    // 1. Maria Silva - Préstamo activo con 2 pagos realizados
                    new Loan
                    {
                        IdApplicant = mariaSilva.Id,
                        Amount = 1500.00m,
                        CurrentBalance = 500.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        UpdateAt = DateTime.UtcNow.AddDays(-15)
                    },

                    // 2. Juan Perez - Préstamo PAGADO completamente
                    new Loan
                    {
                        IdApplicant = juanPerez.Id,
                        Amount = 3000.00m,
                        CurrentBalance = 0.00m,
                        IsActive = 0,
                        CreatedAt = DateTime.UtcNow.AddMonths(-8),
                        UpdateAt = DateTime.UtcNow.AddMonths(-2)
                    },

                    // 3. Carlos Rodriguez - Préstamo activo reciente, 1 pago
                    new Loan
                    {
                        IdApplicant = carlosRodriguez.Id,
                        Amount = 5000.00m,
                        CurrentBalance = 3500.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        UpdateAt = DateTime.UtcNow.AddDays(-5)
                    },

                    // 4. Ana Martinez - Préstamo activo con 3 pagos
                    new Loan
                    {
                        IdApplicant = anaMartinez.Id,
                        Amount = 2500.00m,
                        CurrentBalance = 1000.00m,
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.AddMonths(-5),
                        UpdateAt = DateTime.UtcNow.AddDays(-10)
                    },

                    // 5. Pedro Gonzalez - Préstamo PAGADO hace tiempo
                    new Loan
                    {
                        IdApplicant = pedroGonzalez.Id,
                        Amount = 4000.00m,
                        CurrentBalance = 0.00m,
                        IsActive = 0,
                        CreatedAt = DateTime.UtcNow.AddMonths(-10),
                        UpdateAt = DateTime.UtcNow.AddMonths(-4)
                    },

                    // 6. Laura Torres - Préstamo activo sin pagos aún
                    new Loan
                    {
                        IdApplicant = lauraTorres.Id,
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

                // 3. FINALMENTE LOS PAGOS (con los IDs reales de los loans)
                var loanMaria = context.Loans.First(l => l.IdApplicant == mariaSilva.Id);
                var loanJuan = context.Loans.First(l => l.IdApplicant == juanPerez.Id);
                var loanCarlos = context.Loans.First(l => l.IdApplicant == carlosRodriguez.Id);
                var loanAna = context.Loans.First(l => l.IdApplicant == anaMartinez.Id);
                var loanPedro = context.Loans.First(l => l.IdApplicant == pedroGonzalez.Id);

                var payments = new[]
                {
                    // Maria Silva - 2 pagos de $500 (total $1000 pagado, queda $500)
                    new Payment
                    {
                        IdLoan = loanMaria.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2).AddDays(-10)
                    },
                    new Payment
                    {
                        IdLoan = loanMaria.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-15)
                    },

                    // Juan Perez - 3 pagos que completaron los $3000
                    new Payment
                    {
                        IdLoan = loanJuan.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-6)
                    },
                    new Payment
                    {
                        IdLoan = loanJuan.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    },
                    new Payment
                    {
                        IdLoan = loanJuan.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2)
                    },

                    // Carlos Rodriguez - 1 pago de $1500 (queda $3500)
                    new Payment
                    {
                        IdLoan = loanCarlos.Id,
                        Amount = 1500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },

                    // Ana Martinez - 3 pagos de $500 (total $1500, queda $1000)
                    new Payment
                    {
                        IdLoan = loanAna.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    },
                    new Payment
                    {
                        IdLoan = loanAna.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2)
                    },
                    new Payment
                    {
                        IdLoan = loanAna.Id,
                        Amount = 500.00m,
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    },

                    // Pedro Gonzalez - 4 pagos que completaron los $4000
                    new Payment
                    {
                        IdLoan = loanPedro.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-9)
                    },
                    new Payment
                    {
                        IdLoan = loanPedro.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-7)
                    },
                    new Payment
                    {
                        IdLoan = loanPedro.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-5)
                    },
                    new Payment
                    {
                        IdLoan = loanPedro.Id,
                        Amount = 1000.00m,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4)
                    }

                    // Laura Torres no tiene pagos todavía
                };

                context.Payments.AddRange(payments);
                context.SaveChanges();
                logger.LogInformation("Successfully seeded {PaymentCount} payments", payments.Length);

                logger.LogInformation("Database seeding completed successfully");
                logger.LogInformation("Summary: {ApplicantCount} applicants, {LoanCount} loans, {PaymentCount} payments",
                    applicants.Length, loans.Length, payments.Length);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }
    }
}
