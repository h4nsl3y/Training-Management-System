using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Notification : ISystemEntity
    {
        public int NotificationId {  get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int AccountId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool HasRead {  get; set; } = false;
    }
}
