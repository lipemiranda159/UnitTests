﻿using System;
using System.Net;
using System.Threading.Tasks;

using UnitTests.Facades.Interfaces;
using UnitTests.Models;
using UnitTests.Models.Ui;

using Microsoft.AspNetCore.Http;
using Serilog;

namespace UnitTests.Middleware
{
    /// <summary>
    /// Wraps all controller actions with a header verification to avoid code repetition
    /// </summary>
    public class AuthorizationMiddleware
    {
        private const string HEALTH_CHECK_PATH = "/api/health";

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IAuthorizationFacade _authorizationLogic;
        private readonly ApiSettings _settings;

        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AuthorizationMiddleware(
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            RequestDelegate next,
            ILogger logger,
            IAuthorizationFacade auth,
            ApiSettings settings)
        {
            _next = next;
            _logger = logger;
            _authorizationLogic = auth;
            _settings = settings;
        }

        /// <summary>
        /// Invoke Method, to validate requester authorization
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!IsHealthCheck(context) && !IsAuthorized(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(string.Empty);
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[{@user}] Error In Authorization: {@exception}", context.Request.Headers[Constants.BLIP_USER_HEADER], ex.Message);
            }
        }

        private bool IsHealthCheck(HttpContext context) => context.Request.Path.Value.StartsWith(HEALTH_CHECK_PATH, StringComparison.OrdinalIgnoreCase);
        private bool IsAuthorized(HttpContext context)
        {
            if(_settings.CheckAuthorizationKey)
            {
                return _authorizationLogic.IsValidBotKey(context);
            }
            else
            {
                return true;
            }
        }
    }
}
