using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using NETCore.Context;
using NETCore.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<EmployeeModel, MyContext>
    {
        private readonly MyContext _myContext;

        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
            _myContext = myContext;
        }

        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        
        // insert
        public async Task<IEnumerable<EmployeeViewModel>> InsertEmployee(EmployeeViewModel data)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var procedureName = "SP_InsertEmployee";
                parameters.Add("@Email", data.Email);
                parameters.Add("@FirstName", data.FirstName);
                parameters.Add("@LastName", data.LastName);
                parameters.Add("@BirthDate", data.BirthDate);
                parameters.Add("@PhoneNumber", data.PhoneNumber);
                parameters.Add("@Address", data.Address);
                parameters.Add("@Department_Id", data.Department_Id);
                var employees = await connection.QueryAsync<EmployeeViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        //get by email
        public async Task<EmployeeModel> Get(string email)
        {
            return await _myContext.Set<EmployeeModel>().FindAsync(email);
        }

        // get for data table
        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployee()
        {
            using(var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var procedureName = "SP_GetAllEmployee";
                var employees = await connection.QueryAsync<EmployeeViewModel>(procedureName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

        public async Task<EmployeeModel> Delete(string email)
        {
            var entity = await Get(email);
            if (entity == null)
            {
                return entity;
            }
            entity.DeleteDate = DateTimeOffset.Now;
            entity.IsDelete = true;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

    }
}
