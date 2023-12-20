using DAL.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.RequiredFileBusinessLogics
{
    public interface IRequiredFileBusinessLogic
    {
        byte[] GetFileData(string path);
    }
}
