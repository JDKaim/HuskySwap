using LightNap.Core.Api;
using LightNap.Core.Configuration;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Services;
using LightNap.Core.Interfaces;
using LightNap.Core.Tests.Utilities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Web;

namespace LightNap.Core.Tests
{
    [TestClass]
    public class IdentityServiceTests
    {
        // Hardcoded in Core library. Not great since name might change, but good enough for now.
        private const string _refreshTokenCookieName = "refreshToken";

        // These will be initialized during TestInitialize.
#pragma warning disable CS8618
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private IdentityService _identityService;
        private ICookieManager _cookieManager;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
#pragma warning restore CS8618

        [TestInitialize]
        public void TestInitialize()
        {
            var services = new ServiceCollection();
            services.AddLogging()
                .AddLightNapInMemoryDatabase()
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Use EphemeralDataProtectionProvider for testing things like generating a password reset token.
            services.AddSingleton<IDataProtectionProvider, EphemeralDataProtectionProvider>();

            var serviceProvider = services.BuildServiceProvider();
            this._dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<IdentityService>>();

            this._tokenServiceMock = new Mock<ITokenService>();
            this._tokenServiceMock.Setup(ts => ts.GenerateRefreshToken()).Returns("refresh-token");
            this._tokenServiceMock.Setup(ts => ts.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("access-token");

            this._emailServiceMock = new Mock<IEmailService>();

            var applicationSettings = Options.Create(
                new ApplicationSettings
                {
                    AutomaticallyApplyEfMigrations = false,
                    LogOutInactiveDeviceDays = 30,
                    RequireTwoFactorForNewUsers = false,
                    SiteUrlRootForEmails = "https://example.com/",
                    UseSameSiteStrictCookies = true
                });

            this._cookieManager = new TestCookieManager();

            TestUserContext userContext = new()
            {
                IpAddress = "127.0.0.1"
            };

            this._identityService = new IdentityService(logger, this._userManager, signInManager, this._tokenServiceMock.Object, this._emailServiceMock.Object,
                applicationSettings, this._dbContext, this._cookieManager, userContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._dbContext.Database.EnsureDeleted();
            this._dbContext.Dispose();
        }

        [TestMethod]
        public async Task LogInAsync_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var requestDto = new LoginRequestDto
            {
                Email = "test@test.com",
                Password = "ValidPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice",
            };

            var user = await TestHelper.CreateTestUserAsync(this._userManager, "user-id", "UserName", requestDto.Email);
            await this._userManager.AddPasswordAsync(user, requestDto.Password);

            // Act
            var result = await this._identityService.LogInAsync(requestDto);

            // Assert
            Assert.IsFalse(result.TwoFactorRequired);
            Assert.IsNotNull(result.BearerToken);

            var cookie = this._cookieManager.GetCookie(IdentityServiceTests._refreshTokenCookieName);
            Assert.IsNotNull(cookie);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task LogInAsync_BadEmail_ThrowsError()
        {
            // Arrange
            var requestDto = new LoginRequestDto
            {
                Email = "test@test.com",
                Password = "ValidPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice",
            };

            // Act
            await this._identityService.LogInAsync(requestDto);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task LogInAsync_BadPassword_ThrowsError()
        {
            // Arrange
            var requestDto = new LoginRequestDto
            {
                Email = "test@test.com",
                Password = "BadPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice",
            };

            var user = await TestHelper.CreateTestUserAsync(this._userManager, "user-id", "UserName", requestDto.Email);
            await this._userManager.AddPasswordAsync(user, "GoodPassword123!");

            // Act
            await this._identityService.LogInAsync(requestDto);
        }

        [TestMethod]
        public async Task GetAccessTokenAsync_LoggedIn_ReturnsSuccess()
        {
            // Arrange
            var requestDto = new LoginRequestDto
            {
                Email = "test@test.com",
                Password = "ValidPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice",
            };
            var user = await TestHelper.CreateTestUserAsync(this._userManager, "user-id", "UserName", requestDto.Email);
            await this._userManager.AddPasswordAsync(user, requestDto.Password);
            var loginResult = await this._identityService.LogInAsync(requestDto);
            var newToken = "new-token";
            Assert.AreNotEqual(newToken, loginResult.BearerToken);
            this._tokenServiceMock.Setup(ts => ts.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(newToken);

            // Act
            var accessTokenResult = await this._identityService.GetAccessTokenAsync();

            // Assert
            Assert.AreEqual(newToken, accessTokenResult);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task GetAccessTokenAsync_NotLoggedIn_ThrowsError()
        {
            // Arrange

            // Act
            await this._identityService.GetAccessTokenAsync();
        }

        [TestMethod]
        public async Task RegisterAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var requestDto = new RegisterRequestDto
            {
                Email = "newuser@test.com",
                Password = "NewUserPassword123!",
                UserName = "NewUser",
                ConfirmPassword = "NewUserPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice"
            };

            this._emailServiceMock.Setup(ts => ts.SendRegistrationEmailAsync(It.IsAny<ApplicationUser>())).Returns(Task.CompletedTask);

            // Act
            var registeredUser = await this._identityService.RegisterAsync(requestDto);

            // Assert
            Assert.IsFalse(registeredUser.TwoFactorRequired);
            Assert.IsNotNull(registeredUser.BearerToken);

            var user = await this._userManager.FindByEmailAsync(requestDto.Email);
            Assert.IsNotNull(user);

            var cookie = this._cookieManager.GetCookie(IdentityServiceTests._refreshTokenCookieName);
            Assert.IsNotNull(cookie);

            this._emailServiceMock.Verify(ts => ts.SendRegistrationEmailAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [TestMethod]
        public async Task LogOutAsync_UserLoggedIn_ReturnsSuccess()
        {
            // Arrange
            var requestDto = new RegisterRequestDto
            {
                Email = "newuser@test.com",
                Password = "NewUserPassword123!",
                UserName = "NewUser",
                ConfirmPassword = "NewUserPassword123!",
                RememberMe = true,
                DeviceDetails = "TestDevice"
            };
            await this._identityService.RegisterAsync(requestDto);
            var cookie = this._cookieManager.GetCookie(IdentityServiceTests._refreshTokenCookieName);
            Assert.IsNotNull(cookie);

            // Act
            await this._identityService.LogOutAsync();

            // Assert
            cookie = this._cookieManager.GetCookie("refreshCookie");
            Assert.IsNull(cookie);
        }

        [TestMethod]
        public async Task ResetPasswordAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var requestDto = new ResetPasswordRequestDto
            {
                Email = "test@test.com"
            };

            var user = await TestHelper.CreateTestUserAsync(this._userManager, "user-id", "UserName", requestDto.Email);
            await this._userManager.AddPasswordAsync(user, "OldPassword123!");

            string capturedPasswordResetUrl = string.Empty;
            this._emailServiceMock
                .Setup(ts => ts.SendPasswordResetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback<ApplicationUser, string>((user, url) => capturedPasswordResetUrl = url)
                .Returns(Task.CompletedTask);

            // Act
            await this._identityService.ResetPasswordAsync(requestDto);

            // Assert
            this._emailServiceMock.Verify(ts => ts.SendPasswordResetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            Assert.IsFalse(string.IsNullOrEmpty(capturedPasswordResetUrl), "Password reset URL should be captured.");
        }

        [TestMethod]
        public async Task NewPasswordAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var passwordResetRequestDto = new ResetPasswordRequestDto
            {
                Email = "test@test.com"
            };

            var user = await TestHelper.CreateTestUserAsync(this._userManager, "user-id", "UserName", passwordResetRequestDto.Email);
            await this._userManager.AddPasswordAsync(user, "OldPassword123!");

            string capturedPasswordResetUrl = string.Empty;
            this._emailServiceMock
                .Setup(ts => ts.SendPasswordResetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback<ApplicationUser, string>((user, url) => capturedPasswordResetUrl = url)
                .Returns(Task.CompletedTask);

            await this._identityService.ResetPasswordAsync(passwordResetRequestDto);

            // Not ideal since it's hardcoded to a very specific URL format, so expect to discover this comment if that gets changed.
            string passwordResetToken = HttpUtility.UrlDecode(capturedPasswordResetUrl.Substring(capturedPasswordResetUrl.LastIndexOf('/') + 1));

            var newPasswordRequestDto = new NewPasswordRequestDto
            {
                Email = passwordResetRequestDto.Email,
                DeviceDetails = "TestDevice",
                Password = "NewPassword123!",
                RememberMe = true,
                Token = passwordResetToken
            };

            // Act
            await this._identityService.NewPasswordAsync(newPasswordRequestDto);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task NewPasswordAsync_InvalidData_ThrowsError()
        {
            // Arrange
            var newPasswordRequestDto = new NewPasswordRequestDto
            {
                Email = "test@test.com",
                DeviceDetails = "TestDevice",
                Password = "NewPassword123!",
                RememberMe = true,
                Token = "bad-token"
            };

            // Act
            await this._identityService.NewPasswordAsync(newPasswordRequestDto);
        }

    }
}