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
    public class RequiredFileBusinessLogic : IRequiredFileBusinessLogic
    {
        public byte[] GetFileData(string path) 
        {
            byte[] binaryData = File.ReadAllBytes(path);
            return binaryData;
        }
    }
}

