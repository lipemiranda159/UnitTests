using UnitTests.Controllers;
using UnitTests.Facades;
using UnitTests.Facades.Interfaces;
using UnitTests.Models;
using UnitTests.Models.Ui;
using UnitTests.Models.UI;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Shouldly;

using Xunit;

namespace UnitTests.Tests
{
    public class AuthorizationUnitTests
    {
        private readonly IAuthorizationFacade _authorizationFacade;

        public AuthorizationUnitTests()
        {
            var settings = new ApiSettings
            {
                BlipBotSettings = new BlipBotSettings
                {
                    Authorization = "allowed-authorization"
                }
            };
            _authorizationFacade = new AuthorizationFacade(settings);
        }

        [Theory]
        [InlineData("allowed-authorization", true)]
        [InlineData("not-allowed-authorization", false)]
        public void IsValidBotKey_UnitTest(string botKey, bool isValid)
        {
            var controller = new HealthController();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers[Constants.BLIP_BOT_KEY] = botKey;
            var context = controller.ControllerContext.HttpContext;

            var check = _authorizationFacade.IsValidBotKey(context);
            check.ShouldBe(isValid);
        }
    }
}
