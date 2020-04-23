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
    public class EmployeeController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44342/api/")
        };

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                return View(LoadEmployee());
            }
            return RedirectToAction("AccessDenied", "User");
        }

        public JsonResult LoadEmployee()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            EmployeeJson data = null;
            var responseTask = client.GetAsync("Employee");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.DeserializeObject<EmployeeJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, please try again");
            }

            return Json(data);
        }

        public JsonResult GetById(string Email)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            object data = null;
            var responseTask = client.GetAsync("Employee/" + Email);
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
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            var myContent = JsonConvert.SerializeObject(employeeViewModel);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("User/Register", byteContent).Result;
            return Json(result);

        }

        public JsonResult Edit(EmployeeModel model)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("Employee/" + model.Email, byteContent).Result;
            return Json(result);
        }

        public JsonResult Delete(string Email)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            var result = client.DeleteAsync("Employee/" + Email).Result;
            return Json(result);
        }

        public JsonResult GetDonutchart()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            IEnumerable<ChartViewModel> donutChart = null;
            List<ChartViewModel> dataList = new List<ChartViewModel>();

            var responseTask = client.GetAsync("Employee/Donutchart");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsAsync<IList<ChartViewModel>>();
                data.Wait();
                donutChart = data.Result;

                foreach (var row in donutChart)
                {
                    ChartViewModel details = new ChartViewModel();
                    details.label = row.label.ToString();
                    details.value = row.value;
                    dataList.Add(details);
                }
               
            }
            else
            {
                ModelState.AddModelError(string.Empty, "failed to get data");
            }
            return Json(dataList);
        }

        public JsonResult GetBarchart()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            IEnumerable<ChartViewModel> barChart = null;
            List<ChartViewModel> dataList = new List<ChartViewModel>();

            var responseTask = client.GetAsync("Employee/Donutchart");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsAsync<IList<ChartViewModel>>();
                data.Wait();
                barChart = data.Result;

                foreach (var row in barChart)
                {
                    ChartViewModel details = new ChartViewModel();
                    details.y = row.label.ToString();
                    details.a = row.value;
                    dataList.Add(details);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "failed to get data");
            }
            return Json(dataList);
        }
    }
}