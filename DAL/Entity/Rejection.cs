using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Rejection : ISystemEntity
    {
        public int EnrollmentId { get; set; }
        public string Comment { get; set;}
    }
}
