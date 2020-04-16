using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Model;
using NETCore.Repository.Data;

namespace NETCore.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EmployeeController : BasesController<EmployeeModel, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeViewModel>> Get()
        {
            var get = await _repository.GetAllEmployee();
            return Ok(new { data = get });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeModel>> Put(int id, EmployeeModel entity)
        {
            var put = await _repository.Get(id);
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
    }
}