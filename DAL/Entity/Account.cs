using DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Account : ISystemEntity
    {
        [Key]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "National identification number is required.")]
        public string NationalIdentificationNumber { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\+^\d{2,3}\s\d{8}$", ErrorMessage = "Invalid mobile number.")] 
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; } = (int)RoleEnum.Employee;

    }
}