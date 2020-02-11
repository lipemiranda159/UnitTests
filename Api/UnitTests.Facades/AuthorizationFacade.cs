using UnitTests.Facades.Interfaces;
using UnitTests.Models;
using UnitTests.Models.Ui;

using Microsoft.AspNetCore.Http;

namespace UnitTests.Facades
{
    public class AuthorizationFacade : IAuthorizationFacade
    {
        private readonly ApiSettings _settings;

        public AuthorizationFacade(ApiSettings settings)
        {
            _settings = settings;
        }

        public bool IsValidBotKey(HttpContext context) => 
                            _settings.BlipBotSettings
                            .Authorization
                            .Equals(context.Request.Headers[Constants.BLIP_BOT_KEY]);
    }
}
