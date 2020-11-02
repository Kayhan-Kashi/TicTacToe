using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.WebUI.Services
{
    public class CultureProviderResolverService : RequestCultureProvider
    {
        private static readonly char[] _cookieSeparator = new[] { '|' };
        private static readonly string _culturePrefix = "c=";
        private static readonly string _uiCulturePrefix = "uic=";

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string culture;
            if (GetCultureFromQueryString(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);
            else if (GetCultureFromCookies(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);
            else if (GetCultureFromSession(httpContext, out culture))
                return new ProviderCultureResult(culture, culture);

            return await NullProviderCultureResult;
        }

        private bool GetCultureFromQueryString(HttpContext httpContext, out string culture)
        {
            if (httpContext.Request.QueryString.HasValue)
            {
                culture = httpContext.Request.Query["culture"];
                return true;
            }
            culture = null;
            return false;
        }

        private bool GetCultureFromCookies(HttpContext httpContext, out string culture)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var cookie = httpContext.Request.Cookies["culture"];
            if (string.IsNullOrEmpty(cookie))
            {
                culture = null;
                return false;
            }
            else
            {
                culture = ParseCookieValue(cookie);
                return !string.IsNullOrEmpty(culture);
            }
        }

        private bool GetCultureFromSession(HttpContext httpContext, out string culture)
        {
            culture = httpContext.Session.GetString("culture");
            return !string.IsNullOrEmpty(culture);
        }

        public static string ParseCookieValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var parts = value.Split(_cookieSeparator,
             StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return null;
            }

            var potentialCultureName = parts[0];
            var potentialUICultureName = parts[1];

            if (!potentialCultureName.StartsWith(_culturePrefix) || !potentialUICultureName.StartsWith(_uiCulturePrefix))
            {
                return null;
            }

            var cultureName = potentialCultureName.Substring(_culturePrefix.Length);
            var uiCultureName = potentialUICultureName.Substring(_uiCulturePrefix.Length);

            if (cultureName == null && uiCultureName == null)
            {
                return null;
            }

            if (cultureName != null && uiCultureName == null)
            {
                uiCultureName = cultureName;
            }

            if (cultureName == null && uiCultureName != null)
            {
                cultureName = uiCultureName;
            }

            return cultureName;
        }


    }
}
