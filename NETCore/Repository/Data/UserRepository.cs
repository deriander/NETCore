using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class UserRepository
    {
        private readonly MyContext _myContext;

        public UserRepository(MyContext myContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _myContext = myContext;
        }

        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }

        public async Task<IEnumerable<UserViewModel>> GetRole(UserViewModel data)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var procedureName = "SP_GetRole";
                parameters.Add("@Email", data.Email);
                var employees = await connection.QueryAsync<UserViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return employees;
            }
        }

    }
}
