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
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

        [Fact]
        public async Task Register_ReturnsStatus500_WhenUserCreationFails()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testUser",
                EmailAddress = "test@example.com",
                Password = "TestPassword123"
            };

            var identityErrors = new IdentityError
            {
                Code = null!,
                Description = "User creation failed."
            };
            var failedResult = IdentityResult.Failed(identityErrors);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(failedResult);

            // Act
            var result = await _accountController.Register(registerDto);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(500);

            // Extract the actual IdentityError from the response and compare the properties
            //objectResult.Value.Should().Be(identityErrors);   ---> because this line dose not work 
            var actualErrors = objectResult.Value as IEnumerable<IdentityError>;
            actualErrors.Should().ContainSingle();
            actualErrors!.First().Description.Should().Be(identityErrors.Description);
            actualErrors!.First().Code.Should().Be(identityErrors.Code);

        }

        [Fact]
        public async Task Login_ReturnsOk_WhenUserIsSuccessfullyLoggedIn()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "testUser",
                Password = "testPassword"
            };

            var user = new User { UserName = loginDto.UserName, Email = "testUser@Email.com" };

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(user);

            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
               .Returns("mockedToken");

            //Act 
            var result = await _accountController.Login(loginDto);

            //Assert
            var okResult = result as ObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var responseValue = okResult.Value as UserRegisteredResultDto;
            responseValue.Should().NotBeNull();
            responseValue!.UserName.Should().Be(user.UserName);
            responseValue.Email.Should().Be(user.Email);
            responseValue.Token.Should().Be("mockedToken");
        }

        [Fact]
        public async Task Login_ReturnsUnAuthorize_WhenUserLoginFails()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "testUser",
                Password = "testPassword"
            };

            var user = new User { UserName = loginDto.UserName, Email = "testUser@Email.com" };

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
               .ReturnsAsync(user);

            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            //Act 
            var result = await _accountController.Login(loginDto);

            //Assert
            var objectResult = result as UnauthorizedObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(401);

            var responseValue = objectResult.Value as string;
            responseValue.Should().NotBeNull();
            responseValue.Should().Be("Invalid UserName or Password!");
        }
    }
}
