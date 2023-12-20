using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Department : ISystemEntity
    {
        public int DepartmentId {  get; set; }
        public string DepartmentName { get; set; }
    }
}