﻿using System;
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
    public class DepartmentController : BasesController<DepartmentModel, DepartmentRepository>
    {
        private readonly DepartmentRepository _repository;
        public DepartmentController(DepartmentRepository departmentRepository) : base(departmentRepository)
        {
            this._repository = departmentRepository;
        }

        
        [HttpPost]
        public async Task<ActionResult<DepartmentModel>> Post(DepartmentModel entity)
        {
            await _repository.Post(entity);
            return CreatedAtAction("Get", new { id = entity.Id }, entity);

        }
        

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentModel>> Put(int id, DepartmentModel entity)
        {
            var put = await _repository.Get(id);
            if (put == null)
            {
                return NotFound();
            }
            put.Name = entity.Name;
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Successfully updated data");
        }

        [HttpGet]
        public async Task<ActionResult<DepartmentViewModel>> Get()
        {
            var get = await _repository.Get();
            return Ok(new { data = get });
        }

    }
}