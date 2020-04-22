using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NETCore.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data.SqlClient;
using NETCore.Repository.Data;

namespace NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly EmployeeRepository _employeeRepository;
        private readonly UserRepository _userRepository;
        private readonly DepartmentRepository _deptRepository;
        public IConfiguration _configuration;

        public UserController(
            IConfiguration config,
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            EmployeeRepository employeeRepository,
             UserRepository userRepository,
             DepartmentRepository deptRepository)
        {
            _configuration = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _deptRepository = deptRepository;
        }


        //
        // POST: /User/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false); 
            if(result.Succeeded)
            {
                UserViewModel role = null;
                IEnumerable<UserViewModel> getRole = await _userRepository.GetRole(model);
                foreach (UserViewModel get in getRole)
                {
                    role = get;
                }

                var claims = new[] {
                    //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    //new Claim("Id", user.UserId.ToString()),
                    //new Claim("FirstName", user.FirstName),
                    //new Claim("LastName", user.LastName),
                    new Claim("UserName", model.Email),
                    new Claim("Email", model.Email),
                    new Claim("Role", role.Role)
                   };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Invalid credentials");
            }

        }


        //
        // POST: /User/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody]EmployeeViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel { UserName = data.Email, Email = data.Email };
                var result = await _userManager.CreateAsync(user, data.Password);

                if (result.Succeeded)
                {
                    // add role user
                    await _userManager.AddToRoleAsync(user, "User");

                    // insert data to employee
                    await _employeeRepository.InsertEmployee(data);

                }
            }
            return Ok("success");
        }

        [HttpGet]
        public async Task<ActionResult<DepartmentViewModel>> Get()
        {
            var get = await _deptRepository.Get();
            return Ok(new { data = get });
        }
    }
}