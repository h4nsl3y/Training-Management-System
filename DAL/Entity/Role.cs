using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Role : ISystemEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}