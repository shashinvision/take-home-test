using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Controllers;
using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Fundo.Services.Tests.Integration
{
    [TestFixture]
    public class PaymentManagementControllerTests
    {
        private Mock<IPaymentManagementService> _paymentService;
        private PaymentManagementController _paymentManagementController;

        [SetUp]
        public void Setup()
        {
            _paymentService = new Mock<IPaymentManagementService>();
            _paymentManagementController = new PaymentManagementController(_paymentService.Object);
        }

        [Test]
        public async Task CreatePayment_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = 1000,
                IdLoan = 1
            };

            _paymentService
                .Setup(s => s.CreatePayment(It.IsAny<PaymentDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.Not.Null);

            _paymentService.Verify(s => s.CreatePayment(paymentDto), Times.Once);
        }

        [Test]
        public async Task CreatePayment_WithNullPaymentDto_ReturnsBadRequest()
        {
            // Arrange
            PaymentDto paymentDto = null;

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Payment data is required"));

            _paymentService.Verify(s => s.CreatePayment(It.IsAny<PaymentDto>()), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(null)]
        public async Task CreatePayment_WithInvalidAmount_ReturnsBadRequest(decimal? amount)
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = amount,
                IdLoan = 1
            };

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Amount must be greater than 0"));

            _paymentService.Verify(s => s.CreatePayment(It.IsAny<PaymentDto>()), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public async Task CreatePayment_WithInvalidIdLoan_ReturnsBadRequest(int idLoan)
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = 1000,
                IdLoan = idLoan
            };

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Valid id loan ID is required"));

            _paymentService.Verify(s => s.CreatePayment(It.IsAny<PaymentDto>()), Times.Never);
        }

        [Test]
        public async Task CreatePayment_WhenServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = 1000,
                IdLoan = 1
            };

            var expectedErrorMessage = "Loan not found";
            _paymentService
                .Setup(s => s.CreatePayment(It.IsAny<PaymentDto>()))
                .ThrowsAsync(new ArgumentException(expectedErrorMessage));

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo(expectedErrorMessage));

            _paymentService.Verify(s => s.CreatePayment(paymentDto), Times.Once);
        }

        [Test]
        public async Task CreatePayment_WhenServiceThrowsGenericException_ReturnsBadRequest()
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = 1000,
                IdLoan = 1
            };

            var expectedErrorMessage = "Unexpected error occurred";
            _paymentService
                .Setup(s => s.CreatePayment(It.IsAny<PaymentDto>()))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo(expectedErrorMessage));

            _paymentService.Verify(s => s.CreatePayment(paymentDto), Times.Once);
        }

        [Test]
        public async Task CreatePayment_VerifySuccessMessageStructure()
        {
            // Arrange
            var paymentDto = new PaymentDto
            {
                Amount = 1000,
                IdLoan = 1
            };

            _paymentService
                .Setup(s => s.CreatePayment(It.IsAny<PaymentDto>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _paymentManagementController.CreatePayment(paymentDto);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value;

            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty, Is.Not.Null);
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo("Payment created successfully"));
        }


    }
}
