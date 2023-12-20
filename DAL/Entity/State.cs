using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class State : ISystemEntity
    {
        public int StateId { get; set; }
        public string StateDefinition { get; set; }
    }
}
