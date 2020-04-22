using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Model
{
    public class EmployeeViewModel
    {
        //public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<DateTimeOffset> CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public int Department_Id { get; set; }

    }

    public class EmployeeJson
    {
        [JsonProperty("data")]
        public IList<EmployeeViewModel> data { get; set; }
    }
}
