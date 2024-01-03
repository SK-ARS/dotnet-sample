using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Filters
{
    public sealed class ContentSecurityPolicy : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext.GetOwinContext();
            var rng = new RNGCryptoServiceProvider();
            var nonceBytes = new byte[32];
            rng.GetBytes(nonceBytes);
            var nonce = Convert.ToBase64String(nonceBytes);
            context.Set("ScriptNonce", nonce);
            context.Response.Headers.Remove("Content-Security-Policy");
            context.Response.Headers.Add("Content-Security-Policy",
                new[] { string.Format("font-src * 'self' data:; " +
                                      "img-src * data:; " +
                                      "script-src 'self' 'nonce-{0}' 'unsafe-eval'; " +
                                      "style-src * 'unsafe-inline'; " +
                                      "object-src 'none'; " +
                                      "manifest-src 'self'; " +
                                      "base-uri 'self'; " +
                                      "connect-src 'self' http://10.10.1.12:8080 http://10.10.1.12:8082", 
                                      nonce)
                });
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}