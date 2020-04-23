using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Model;
using NETCore.Repository.Data;

namespace NETCore.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class EmployeeController : BasesController<EmployeeModel, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        // insert data
        [HttpPost]
        public async Task<ActionResult<EmployeeViewModel>> Post(EmployeeViewModel entity)
        {
            await _repository.InsertEmployee(entity);
            return CreatedAtAction("Get", new { email = entity.Email }, entity);

        }

       // get all
       [HttpGet]
        public async Task<ActionResult<EmployeeViewModel>> Get()
        {
            var get = await _repository.GetAllEmployee();
            return Ok(new { data = get });
        }

        //get by email
        [HttpGet("{email}")]
        public async Task<ActionResult<EmployeeModel>> Get(string email)
        {
            var get = await _repository.Get(email);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }

        // update
        [HttpPut("{email}")]
        public async Task<ActionResult<EmployeeModel>> Put(string email, EmployeeModel entity)
        {

            var put = await _repository.Get(email);
            if (put == null)
            {
                return NotFound();
            }
            if (put.FirstName != null)
            {
                put.FirstName = entity.FirstName;
            }
            if (put.LastName != null)
            {
                put.LastName = entity.LastName;
            }
            if (put.Email != null)
            {
                put.Email = entity.Email;
            }
            if (put.BirthDate != default(DateTime))
            {
                put.BirthDate = entity.BirthDate;
            }
            if (put.PhoneNumber != null)
            {
                put.PhoneNumber = entity.PhoneNumber;
            }
            if (put.Address != null)
            {
                put.Address = entity.Address;
            }
            put.Department_Id = entity.Department_Id;
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Successfully updated data");
        }

        // delete
        [HttpDelete("{email}")]
        public async Task<ActionResult<EmployeeModel>> Delete(string email)
        {
            var delete = await _repository.Delete(email);
            if (delete == null)
            {
                return NotFound();
            }
            return delete;
        }

        // Get Donutchart Data
        [HttpGet("Donutchart")]
        public async Task<IEnumerable<ChartViewModel>> GetDonutchartData()
        {
            return await _repository.GetDonutchartData();
        }
    }
}