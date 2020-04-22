using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Model;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class UserController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44342/api/")
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "User")
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "User");
        }

        public JsonResult LoadDepartment()
        {
            DepartmentJson departmentViewModel = null;
            var responseTask = client.GetAsync("User");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentViewModel = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return Json(departmentViewModel);
        }

        public JsonResult GetById()
        {   
            var email = HttpContext.Session.GetString("Email");
            object data = null;
            var responseTask = client.GetAsync("Employee/" + email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.SerializeObject(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return Json(data);

        }

        public JsonResult Insert(EmployeeViewModel employeeViewModel)
        {
            var myContent = JsonConvert.SerializeObject(employeeViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("User/Register", byteContent).Result;
            return Json(result);
            
        }

        public JsonResult Edit(EmployeeModel employee)
        {
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("Employee/" + employee.Email, byteContent).Result;
            return Json(result);
        }

        [HttpPost]
        public IActionResult Login(UserViewModel user)
        {
            var myContent = JsonConvert.SerializeObject(user);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("User/Login", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result; // token
                var handler = new JwtSecurityTokenHandler();
                var datajson = handler.ReadJwtToken(data); 

                // get token, role, emai from jwt
                string token = "Bearer " + data;
                string role = datajson.Claims.First(claim => claim.Type == "Role").Value;
                string email = datajson.Claims.First(claim => claim.Type == "Email").Value;

                // set token
                HttpContext.Session.SetString("JWTToken", token);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", email);

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                return View();
            } 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Login", "User");
        }
    }
}