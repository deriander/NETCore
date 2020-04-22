using Microsoft.AspNetCore.Identity;
using NETCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Model
{
    [Table("TB_M_Employee")]
    public class EmployeeModel : IEntity
    {
        [Key]
        public string Email { get; set; } // PK & FK
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }

        public DepartmentModel Department { get; set; }
        [ForeignKey("Department")]
        public int Department_Id { get; set; }
    }
}
