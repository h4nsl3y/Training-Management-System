using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Response<T>
    {
        public bool Success {  get; set; }
        public string Message {  get; set; }
        public List<T> Data { get; set; }
        public Response()
        {
            Data = new List<T>();
        }
    }
}
