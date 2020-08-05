using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    /// <summary>
    ///Authentication Controller
    /// </summary>
    [EnableCors("AllowOrigin")]
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthentication _authenticationService;

        public AuthenticationController(IAuthentication authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticates a user on login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<JsonResult> AuthenticateAsync([FromBody]AuthenticationModel creds)
        {
            var user = _authenticationService.Authenticate(creds.Email, creds.Password);
            if (!ValidateLogin(creds.Email, creds.Password) || user == null)
            {
                return Json(new
                {
                    error = "Login failed"
                });
            }

            var principal = GetPrincipal(user, Startup.CookieAuthScheme);
            await HttpContext.SignInAsync(Startup.CookieAuthScheme, principal);

            return Json(new
            {
                name = principal.Identity.Name,
                email = principal.FindFirstValue(ClaimTypes.Email),
                role = principal.FindFirstValue(ClaimTypes.Role)
            });

        }

        /// <summary>
        ///Controller to log out a user
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<StatusCodeResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return StatusCode(200);
        }

        /// <summary>
        /// Controller to create a user
        /// </summary>
        /// <param name="user">the details of the user</param>
        /// <returns>Ok</returns>
        [HttpPost]
        public IActionResult Add([FromBody] UserModel user)
        {
            _authenticationService.Add(user);
            return Ok();
        }

        /// <summary>
        /// Get the context of the logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet("context")]
        public JsonResult Context()
        {
            return Json(new
            {
                name = this.User?.Identity?.Name,
                email = this.User?.FindFirstValue(ClaimTypes.Email),
                role = this.User?.FindFirstValue(ClaimTypes.Role),
            });
        }

        // On a real project, you would use a SignInManager to verify the identity
        // using:
        //  _signInManager.PasswordSignInAsync(user, password, lockoutOnFailure: false);
        // With JWT you would rather avoid that to prevent cookies being set and use: 
        //  _signInManager.UserManager.FindByEmailAsync(email);
        //  _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        private bool ValidateLogin(string email, string password)
        {

            if (email == null || password == null)
            {
                return false;
            }
            return true;
        }

        // On a real project, you would use the SignInManager 
        // to locate the user by its email and build its ClaimsPrincipal:
        //  var user = await _signInManager.UserManager.FindByEmailAsync(email);
        //  var principal = await _signInManager.CreateUserPrincipalAsync(user)
        private ClaimsPrincipal GetPrincipal(UserModel user, string authScheme)
        {
            // Here we are just creating a Principal for any user, 
            // using its email and a hardcoded “User” role
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Forename + " " + user.Surname),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, authScheme));
        }
    }
}

