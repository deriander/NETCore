using Dapper;
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

        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            _configuration = configuration;
        }

        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployee()
        {
            using(var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var procedureName = "SP_GetAllEmployee";
                var employees = await connection.QueryAsync<EmployeeViewModel>(procedureName, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

    }
}
