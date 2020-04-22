using System;
using System.Collections.Generic;
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
    public class DepartmentController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44342/api/")
        };

        // GET: Data
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                return View(LoadDepartment());
            }
            return RedirectToAction("AccessDenied", "User");
        }

        public JsonResult LoadDepartment()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            DepartmentJson departmentViewModel = null;
            var responseTask = client.GetAsync("Department");
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

        public JsonResult GetById(int Id)
        {
            DepartmentViewModel data = null;
            var responseTask = client.GetAsync("Department/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.DeserializeObject<DepartmentViewModel>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return Json(data);
        }

        public JsonResult InsertOrEdit(DepartmentModel department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0)
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return Json(result);
        }
    }
}