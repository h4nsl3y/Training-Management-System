using BLL.GenericBusinessLogics;
using BLL.RequiredFileBusinessLogics;
using DAL.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class RequiredFileController : Controller
    {
        private readonly IGenericBusinessLogic<RequiredFiles> _genericBusinessLogic;
        private readonly IRequiredFileBusinessLogic _requiredFileSBusinessLogic;
        private readonly string primaryKey;
        public RequiredFileController(IGenericBusinessLogic<RequiredFiles> genericBusinessLogic,IRequiredFileBusinessLogic requiredFileBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _requiredFileSBusinessLogic = requiredFileBusinessLogic;
            PropertyInfo[] properties = typeof(RequiredFiles).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }

        public async Task<ActionResult> UpdateFile(int prerequisiteId)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    int accountId = (int)Session["AccountId"];
                    string path = Path.Combine(Server.MapPath("~/data"), "fileData");
                    file.SaveAs(path);
                    byte[] binaryData = _requiredFileSBusinessLogic.GetFileData(path);
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("FILENAME", Path.GetFileName(file.FileName));
                    values.Add("FILETYPE", file.ContentType);
                    values.Add("FILEDATA", binaryData);

                    Result<bool> boolResult = await _requiredFileSBusinessLogic.UpdateFileAsync(prerequisiteId, accountId , values);
                    return (boolResult.Success) ?
                        Json(new { message = "success" }) :
                        Json(new { message = "failed", data = "Some error encountered while uploading file" });
                }
                else { return Json(new { message = "failed", data = "Some error encountered while uploading file" }); }
            }
            else { return Json(new { message = "failed", data = "No file has been recieved" }); }
        }

        public async Task<ActionResult> UploadFile(int prerequisiteId, HttpPostedFileBase[] files)
        {

            HttpPostedFileBase file = Request.Files[0];

            string path = Path.Combine(Server.MapPath("~/data"), "fileData");
            Result<bool> result = await _requiredFileSBusinessLogic.UploadFileAsync(file, path, (int)Session["AccountId"], prerequisiteId);
            return (result.Success) ?
                Json(new { message = "success" }) :
                Json(new { message = "failed", data = "Some error encountered while uploading file" });

        }

        public async Task<ActionResult> GetFile(int prerequisiteId, int accountId = 0)
        {
            if (accountId == 0) { accountId = (int)Session["AccountId"]; }
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            conditions.Add("ACCOUNTID", accountId);
            Result<RequiredFiles> requiredFileResult = await _genericBusinessLogic.GetAsync(conditions);
            RequiredFiles myFile = requiredFileResult.Data.FirstOrDefault();
            byte[] fileByte = myFile.FileData;
            string fileType = myFile.FileType;
            return File(fileByte, fileType);
        }
        public async Task<JsonResult> IsFilePresent(int prerequisiteId)
        {
            int accountId = (int)Session["AccountId"]; 
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            conditions.Add("ACCOUNTID", accountId);
            Result<RequiredFiles> requiredFileResult = await _genericBusinessLogic.GetAsync(conditions);
            return (requiredFileResult.Success) ?
                        Json(new { message = "success", data = (requiredFileResult.Data.FirstOrDefault() != null) }, JsonRequestBehavior.AllowGet) :
                        Json(new { message = "failed", data = "Some error encountered while uploading file" }, JsonRequestBehavior.AllowGet);

        }
    }
}