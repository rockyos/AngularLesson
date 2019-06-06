using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI
{
    public class MyCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext ctx)
        {
            if (ctx.Request.Cookies.TryGetValue("access_token", out var accessToken))
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    ctx.Request.Headers.Add("Authorization", accessToken);
                }
            }
            return this._next(ctx);
        }
    }

    public static class MyCookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCookie(this IApplicationBuilder build)
        {
            return build.UseMiddleware<MyCookieMiddleware>();
        }
    }
}
