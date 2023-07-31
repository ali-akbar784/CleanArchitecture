using CleanArch.Api.Filter;
using CleanArch.Api.Helper;
using CleanArch.Api.Models;
using CleanArch.Application.Interfaces;
using CleanArch.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace CleanArch.Api.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase //BaseApiController ctestsa
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<AppSettings> _appSettings;

        /// <summary>
        /// Initialize UserController by injecting an object type of IUnitOfWork
        /// </summary>
        public UserController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            this._unitOfWork = unitOfWork;
            _appSettings = appSettings;
        }


        // Post:
        /// <summary>
        /// Signup api it is geting the data from client side and send it to server side. test commit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User model)
        {
            try
            {
                model.Password = HashHelper.GetMD5Hash(model.Password);
                var result = await this._unitOfWork.Users.AddAsync(model);
                if (result > 0)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                // logg details to logger etc 
            }
            return BadRequest();
        }
        /// <summary>
        /// AUthencitate api is passing username and password to the api controller, and then it will authenicate it in the DB 
        /// once user is authenticated it is generating the JWT token and send it to client side along with Firstname, Last Name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User model)
        {
            if (model is null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest();
            }
            var user = await _unitOfWork.Users.GetUserLoginAsync(model.UserName, HashHelper.GetMD5Hash(model.Password));
            // return null if user not found
            if (user == null) return NotFound();
            model.UserId = user.UserId;
            await firstLoginCheckAndAddBalance(model);
            // authentication successful so generate jwt token
            var token = generateToken(user);
            // Returns User details and Jwt token in HttpResponse which will be user to authenticate the user.
            return Ok(new AuthenticateResponse(user.FirstName, user.LastName, token));
        }
        [AuthorizationFilterAttribute]
        [HttpGet("UserBalance")]
        public async Task<IActionResult> UserBalance()
        {
            var user = (User)HttpContext.Items["User"];
            var balance = await _unitOfWork.Users.GetUserBalance(user.UserId);
            // Returns User details and Jwt token in HttpResponse which will be user to authenticate the user.
            return Ok(new { balance = balance });
        }
        /// <summary>
        /// Here is multiple ways of achieveing this thing
        /// 1. by setting trigger to on the table
        /// 2. first get the previous audit logs and then decide on the basis of audit logs is there any login occurs etc.
        /// I go witht he second approach because we need to push some additional data to the db like device and IP etc.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task firstLoginCheckAndAddBalance(User user)
        {
            var isAlreadyLoggedIn = await _unitOfWork.Users.GetLoginAuditTrail(user.UserId);
            if (isAlreadyLoggedIn == 0)
            {
                await _unitOfWork.Users.AddUserBalance(user.UserId, 5);

            }
            LoginAuditTrail loginAuditTrail = new LoginAuditTrail
            {
                UserId = user.UserId,
                IpAddress = user.IpAddress,
                DeviceId = user.Device,
                LoginTime = DateTime.UtcNow
            };
            await _unitOfWork.Users.AddLoginAuditTrail(loginAuditTrail);
        }
        private string generateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Here you  can fill claim information from database for the users as well
            var claims = new[] {
                new Claim("id", user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, "aliakbar"),
                    new Claim(JwtRegisteredClaimNames.Email, ""),

            };
            var token = new JwtSecurityToken(_appSettings.Value.Issuer, _appSettings.Value.Issuer, claims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
