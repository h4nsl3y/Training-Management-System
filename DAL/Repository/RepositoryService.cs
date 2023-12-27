using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public abstract  class RepositoryService
    {
        protected object GetPropertyValue(object value) => (value == null) ? DBNull.Value : value;
    }
}
