using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Controllers;
using Fundo.Applications.WebApi.DTOs;
using Fundo.Applications.WebApi.Services;

namespace Fundo.Services.Tests.Integration
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthService>();
            _controller = new AuthController(_authService.Object);
        }

        #region Login Tests - Happy Path

        [Test]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = "SecurePassword123!"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                User = new UserDto
                {
                    Id = "1",
                    Email = "user@example.com",
                    Name = "John Doe"
                }
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));

            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        [Test]
        public async Task Login_VerifyResponseStructure()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = "password123"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "some-jwt-token",
                User = new UserDto
                {
                    Id = "123",
                    Email = "user@example.com",
                    Name = "Test User"
                }
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value as LoginResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Token, Is.Not.Null.And.Not.Empty);
            Assert.That(response.User, Is.Not.Null);
            Assert.That(response.User.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(response.User.Email, Is.EqualTo(loginRequest.Email));
            Assert.That(response.User.Name, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Login_VerifyUserDtoIsPopulated()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "test@example.com",
                Password = "password"
            };

            var expectedUser = new UserDto
            {
                Id = "user-456",
                Email = "test@example.com",
                Name = "Jane Smith"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "valid-token",
                User = expectedUser
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value as LoginResponseDto;

            Assert.That(response?.User.Id, Is.EqualTo(expectedUser.Id));
            Assert.That(response?.User.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(response?.User.Name, Is.EqualTo(expectedUser.Name));
        }

        #endregion

        #region Login Tests - Error Cases

        [Test]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = "WrongPassword"
            };

            var errorMessage = "Invalid email or password";
            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new UnauthorizedAccessException(errorMessage));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = result as UnauthorizedObjectResult;

            var response = unauthorizedResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo(errorMessage));

            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        [Test]
        public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "SomePassword123"
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new UnauthorizedAccessException("User not found"));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        [Test]
        public async Task Login_WhenServiceThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = "password123"
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));

            var response = objectResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo("An error occurred during login"));

            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        [Test]
        public async Task Login_WithEmptyEmail_CallsService()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "",
                Password = "password"
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        [Test]
        public async Task Login_WithEmptyPassword_CallsService()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = ""
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        #endregion

        #region Register Tests - Happy Path

        [Test]
        public async Task Register_WithValidData_ReturnsOkWithUserInfo()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "newuser@example.com",
                Password = "SecurePassword123!",
                Name = "John Doe"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJuZXd1c2VyQGV4YW1wbGUuY29tIn0.xyz",
                User = new UserDto
                {
                    Id = "user-123-abc",
                    Email = "newuser@example.com",
                    Name = "John Doe"
                }
            };


            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));

            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Register_VerifyResponseStructure()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                Name = "Test User"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJuZXd1c2VyQGV4YW1wbGUuY29tIn0.xyz",
                User = new UserDto
                {
                    Id = "user-123-abc",
                    Email = "test@example.com",
                    Name = "Test User"
                }
            };


            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value as LoginResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.User.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(response.User.Email, Is.EqualTo(registerRequest.Email));
            Assert.That(response.User.Name, Is.EqualTo(registerRequest.Name));
        }

        [Test]
        public async Task Register_VerifyAllUserFieldsAreReturned()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "complete@example.com",
                Password = "Pass123!",
                Name = "Complete User"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJuZXd1c2VyQGV4YW1wbGUuY29tIn0.xyz",
                User = new UserDto
                {
                    Id = "user-123-abc",
                    Email = "complete@example.com",
                    Name = "Complete User"
                }
            };


            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult?.Value as LoginResponseDto;

            Assert.That(response?.User.Id, Is.EqualTo(expectedResponse.User.Id));
            Assert.That(response?.User.Email, Is.EqualTo(expectedResponse.User.Email));
            Assert.That(response?.User.Name, Is.EqualTo(expectedResponse.User.Name));
        }

        #endregion

        #region Register Tests - Error Cases

        [Test]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "existing@example.com",
                Password = "Password123!",
                Name = "John Doe"
            };

            var errorMessage = "Email already exists";
            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;

            var response = badRequestResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo(errorMessage));

            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Register_WithInvalidEmailFormat_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "invalid-email-format",
                Password = "Password123!",
                Name = "John Doe"
            };

            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new InvalidOperationException("Invalid email format"));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Register_WithWeakPassword_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "user@example.com",
                Password = "123",
                Name = "John Doe"
            };

            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new InvalidOperationException("Password does not meet security requirements"));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Register_WhenServiceThrowsGenericException_ReturnsInternalServerError()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "user@example.com",
                Password = "Password123!",
                Name = "John Doe"
            };

            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));

            var response = objectResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            Assert.That(messageProperty?.GetValue(response), Is.EqualTo("An error occurred during registration"));

            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Register_WithEmptyName_CallsService()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "user@example.com",
                Password = "Password123!",
                Name = ""
            };

            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new InvalidOperationException("Name is required"));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        #endregion

        #region Edge Cases


        [Test]
        public async Task Register_WithMinimalValidData_ReturnsOk()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "a@b.c",
                Password = "Pass1!",
                Name = "A"
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJuZXd1c2VyQGV4YW1wbGUuY29tIn0.xyz",
                User = new UserDto
                {
                    Id = "user-123-abc",
                    Email = "newuser@example.com",
                    Name = "John Doe"
                }
            };


            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            _authService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
        }

        [Test]
        public async Task Login_WithLongEmail_CallsService()
        {
            // Arrange
            var longEmail = new string('a', 250) + "@example.com";
            var loginRequest = new LoginRequestDto
            {
                Email = longEmail,
                Password = "password"
            };

            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            _authService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
        }

        #endregion

        #region Security Tests

        [Test]
        public async Task Login_DoesNotExposeInternalErrorDetails()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Email = "user@example.com",
                Password = "password"
            };

            var sensitiveError = "Connection string: Server=prod-db;User=admin;Password=secret123";
            _authService
                .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ThrowsAsync(new Exception(sensitiveError));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var objectResult = result as ObjectResult;
            var response = objectResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            var message = messageProperty?.GetValue(response)?.ToString();

            Assert.That(message, Is.Not.Null);
            Assert.That(message, Does.Not.Contain("Connection string"));
            Assert.That(message, Does.Not.Contain("Password"));
            Assert.That(message, Does.Not.Contain("secret"));
            Assert.That(message, Is.EqualTo("An error occurred during login"));
        }

        [Test]
        public async Task Register_DoesNotExposeInternalErrorDetails()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Email = "user@example.com",
                Password = "Password123!",
                Name = "User"
            };

            var sensitiveError = "SQL Error: INSERT failed on table users_secret_table with key abc123";
            _authService
                .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ThrowsAsync(new Exception(sensitiveError));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            var objectResult = result as ObjectResult;
            var response = objectResult?.Value;
            var messageProperty = response?.GetType().GetProperty("message");
            var message = messageProperty?.GetValue(response)?.ToString();

            Assert.That(message, Is.Not.Null);
            Assert.That(message, Does.Not.Contain("SQL"));
            Assert.That(message, Does.Not.Contain("users_secret_table"));
            Assert.That(message, Does.Not.Contain("abc123"));
            Assert.That(message, Is.EqualTo("An error occurred during registration"));
        }

        #endregion
    }
}
