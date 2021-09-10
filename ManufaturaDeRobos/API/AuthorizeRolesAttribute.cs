using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace ManufaturaDeRobos.API
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params RoleType[] r)
        {
            var roles = r.Select(x => Enum.GetName(typeof(RoleType), x));
            Roles = string.Join(",", roles);
        }
    }
}
