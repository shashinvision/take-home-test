using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Controllers;
using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.Services;

namespace Fundo.Services.Tests.Integration
{
    [TestFixture]
    public class LoanManagementControllerTests
    {
        private Mock<ILoanManagementService> _loanService;
        private LoanManagementController _controller;

        [SetUp]
        public void Setup()
        {
            _loanService = new Mock<ILoanManagementService>();
            _controller = new LoanManagementController(_loanService.Object);
        }

        #region GET Tests

        [Test]
        public async Task Get_ReturnsOkWithAllLoans()
        {
            // Arrange
            var expectedLoans = new List<Loan>
            {
                new Loan { Id = 1, Amount = 1000, IdApplicant = 1 },
                new Loan { Id = 2, Amount = 2000, IdApplicant = 2 }
            };

            _loanService
                .Setup(s => s.GetAllLoans())
                .ReturnsAsync(expectedLoans);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedLoans));

            _loanService.Verify(s => s.GetAllLoans(), Times.Once);
        }

        [Test]
        public async Task Get_WhenNoLoansExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptyLoans = new List<Loan>();

            _loanService
                .Setup(s => s.GetAllLoans())
                .ReturnsAsync(emptyLoans);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var loans = okResult?.Value as List<Loan>;
            Assert.That(loans, Is.Not.Null);
            Assert.That(loans, Is.Empty);

            _loanService.Verify(s => s.GetAllLoans(), Times.Once);
        }

        #endregion

        #region GET by ID Tests

        [Test]
        public async Task GetById_WithValidId_ReturnsOkWithLoan()
        {
            // Arrange
            var loanId = 1;
            var expectedLoan = new Loan
            {
                Id = loanId,
                Amount = 1000,
                IdApplicant = 1
            };

            _loanService
                .Setup(s => s.GetLoanById(loanId))
                .ReturnsAsync(expectedLoan);

            // Act
            var result = await _controller.GetById(loanId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedLoan));

            _loanService.Verify(s => s.GetLoanById(loanId), Times.Once);
        }

        [Test]
        public async Task GetById_WhenLoanNotFound_ReturnsOkWithNull()
        {
            // Arrange
            var loanId = 999;

            _loanService
                .Setup(s => s.GetLoanById(loanId))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _controller.GetById(loanId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Null);

            _loanService.Verify(s => s.GetLoanById(loanId), Times.Once);
        }

        #endregion

        #region POST Tests - Happy Path

        [Test]
        public async Task CreateLoan_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 5000,
                IdApplicant = 1
            };

            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Not.Null);

            _loanService.Verify(s => s.CreateLoan(loanDto), Times.Once);
        }

        [Test]
        public async Task CreateLoan_VerifySuccessMessageStructure()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 5000,
                IdApplicant = 1
            };

            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value;

            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty, Is.Not.Null);
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo("Loan created successfully"));
        }

        #endregion

        #region POST Tests - Validation

        [Test]
        public async Task CreateLoan_WithNullLoanDto_ReturnsBadRequest()
        {
            // Arrange
            LoanDto loanDto = null;

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Loan data is required"));

            _loanService.Verify(s => s.CreateLoan(It.IsAny<LoanDto>()), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(null)]
        public async Task CreateLoan_WithInvalidAmount_ReturnsBadRequest(decimal? amount)
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = amount,
                IdApplicant = 1
            };

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Amount must be greater than 0"));

            _loanService.Verify(s => s.CreateLoan(It.IsAny<LoanDto>()), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(null)]
        public async Task CreateLoan_WithInvalidIdApplicant_ReturnsBadRequest(int? idApplicant)
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 5000,
                IdApplicant = idApplicant
            };

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Valid applicant ID is required"));

            _loanService.Verify(s => s.CreateLoan(It.IsAny<LoanDto>()), Times.Never);
        }

        #endregion

        #region POST Tests - Exception Handling

        [Test]
        public async Task CreateLoan_WhenServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 5000,
                IdApplicant = 1
            };

            var expectedErrorMessage = "Applicant not found";
            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .ThrowsAsync(new ArgumentException(expectedErrorMessage));

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo(expectedErrorMessage));

            _loanService.Verify(s => s.CreateLoan(loanDto), Times.Once);
        }

        [Test]
        public async Task CreateLoan_WhenServiceThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 5000,
                IdApplicant = 1
            };

            var expectedErrorMessage = "Database connection failed";
            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
            Assert.That(objectResult?.Value, Is.EqualTo($"Internal server error: {expectedErrorMessage}"));

            _loanService.Verify(s => s.CreateLoan(loanDto), Times.Once);
        }

        #endregion

        #region Edge Cases

        [Test]
        public async Task CreateLoan_WithMinimalValidAmount_ReturnsOk()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 0.01m, // Mínimo válido
                IdApplicant = 1
            };

            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            _loanService.Verify(s => s.CreateLoan(loanDto), Times.Once);
        }

        [Test]
        public async Task CreateLoan_WithLargeAmount_ReturnsOk()
        {
            // Arrange
            var loanDto = new LoanDto
            {
                Amount = 999999999.99m, // Monto grande
                IdApplicant = 1
            };

            _loanService
                .Setup(s => s.CreateLoan(It.IsAny<LoanDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateLoan(loanDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            _loanService.Verify(s => s.CreateLoan(loanDto), Times.Once);
        }

        #endregion
    }
}
