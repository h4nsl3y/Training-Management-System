using DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class AccountTraining : ISystemEntity
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string ManagerName { get; set; }
        public string Title { get; set; }
        public int StateId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
