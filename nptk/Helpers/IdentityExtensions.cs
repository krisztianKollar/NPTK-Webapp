using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace nptk.Helpers
{
    public static class IdentityExtensions
    {    
        public static string GetFirstName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("FirstName");
            }
            return null;
        }

        public static string GetFullName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("FullName");
            }
            return null;
        }
    }
}