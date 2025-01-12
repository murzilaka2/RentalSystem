using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Moq;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using RentalSystem.Pages.Account;
using System.Security.Claims;
using Xunit;

namespace RentalSystem.Tests.Account
{
    //Install-Package Microsoft.NET.Test.Sdk
    //Install-Package xunit
    //Install-Package xunit.runner.visualstudio
    //Install-Package Moq
    public class ChangePasswordTests
    {
        [Fact]
        public async Task OnPostChangePasswordAsync_ShouldReturnPageWithError_WhenNewPasswordIsSameAsOld()
        {
            // Arrange
            var mockUserService = new Mock<IUser>();
            var mockEnvironment = new Mock<IWebHostEnvironment>();

            var pageModel = new ProfileModel(mockUserService.Object, mockEnvironment.Object);

            // Настройка CurrentUser
            var user = new User
            {
                Id = 1,
                HashPassword = "oldpassword",
                Profile = new UserProfile()
            };
            mockUserService
                .Setup(u => u.GetUserWithUserProfileByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);
            mockUserService
                .Setup(u => u.IsTheSameUserPasswordAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Настройка TempData
            var tempData = new Mock<ITempDataDictionary>();
            pageModel.TempData = tempData.Object;

            // Настройка HttpContext
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "1")
             }))
            };

            httpContext.Features.Set<IHttpRequestFeature>(new HttpRequestFeature
            {
                Method = "POST"
            });

            var formCollection = new FormCollection(new Dictionary<string, StringValues>
            {
                { "ChangePasswordModel.NewPassword", "oldpassword" },
                { "ChangePasswordModel.ConfirmPassword", "oldpassword" }
            });
            httpContext.Request.Form = formCollection;

            var pageContext = new PageContext
            {
                HttpContext = httpContext
            };

            pageModel.PageContext = pageContext;

            // Act
            var result = await pageModel.OnPostChangePasswordAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            tempData.VerifySet(t => t["ErrorPasswordChanged"] = "Provide a new password.", Times.Once);
        }

    }
}
