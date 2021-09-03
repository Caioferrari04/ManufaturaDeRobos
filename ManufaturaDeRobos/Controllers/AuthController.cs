using ManufaturaDeRobos.Controllers;
using ManufaturaDeRobos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueFashionRetailer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ApiBaseController
    {
        IAuthService service;

        public AuthController (IAuthService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("NewUser")]
        [AllowAnonymous]
        public IActionResult NewUser(IdentityUser identityUser)
        {
            try
            {
                IdentityResult result = service.Create(identityUser).Result;
                if (!result.Succeeded) throw new Exception();
                identityUser.PasswordHash = "";
                return ApiOk(identityUser);
            }
            catch
            {
                return ApiBadRequest("Erro ao criar usuário!");
            }
        }

        [HttpPost]
        [Route("Token")]
        [AllowAnonymous]
        public IActionResult Token([FromBody] IdentityUser identityUser)
        {
            try
            {
                return ApiOk(service.GenerateToken(identityUser));
            }
            catch (Exception exception)
            {
                return ApiBadRequest(exception, exception.Message);
            }
        }
    }
}
