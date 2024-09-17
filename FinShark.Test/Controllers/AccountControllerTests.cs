using Api.Controllers;
using Api.Dtos.Account;
using Api.Interfaces;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FinShark.Test.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AccountController _accountController;
        private readonly Mock<SignInManager<User>> _mockSignInManager;

        public AccountControllerTests()
        {
            // Setup user manager mock
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            // Setup token service mock
            _mockTokenService = new Mock<ITokenService>();

            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),   // Using Mock.Of<T>() for simplicity
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<User>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<User>>());


            // Instantiate the controller with the mocked services
            _accountController = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testuser",
                EmailAddress = "test@example.com",
                Password = "TestPassword123"
            };

            var user = new User { UserName = registerDto.UserName, Email = registerDto.EmailAddress };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<String>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
                .Returns("mockedToken");

            //Act 
            var result = await _accountController.Register(registerDto);

            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var responseValue = okResult.Value as UserRegisteredResultDto;
            responseValue.Should().NotBeNull();
            responseValue!.UserName.Should().Be(registerDto.UserName);
            responseValue.Email.Should().Be(registerDto.EmailAddress);
            responseValue.Token.Should().Be("mockedToken");
        }
    }
}
