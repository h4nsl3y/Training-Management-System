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
        Task<Response<bool>> UpdateFileAsync(int prerequisiteId, int accountId, Dictionary<string, object> values);
        Task<Response<bool>> UploadFileAsync(HttpPostedFileBase file, string path, int accountId, int prerequisiteId);
        Task<Response<int>> CountFilePresentAsync(int trainingId, int accountId);
    }
}
