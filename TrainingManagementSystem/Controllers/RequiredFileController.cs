using BLL.Email;
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

                    Response<bool> boolResult = await _requiredFileSBusinessLogic.UpdateFileAsync(prerequisiteId, accountId , values);
                    return (boolResult.Success) ?
                        Json(new { Success = true }) :
                        Json(new { Success = false, Message = "Some error encountered while uploading file" });
                }
                else { return Json(new { Success = false, Message = "Some error encountered while uploading file" }); }
            }
            else { return Json(new { Success = false, Message = "No file has been recieved" }); }
        }

        public async Task<ActionResult> UploadFile(int prerequisiteId, HttpPostedFileBase[] files)
        {
            HttpPostedFileBase file = Request.Files[0];
            string path = Path.Combine(Server.MapPath("~/data"), "fileData");
            return  Json(await _requiredFileSBusinessLogic.UploadFileAsync(file, path, (int)Session["AccountId"], prerequisiteId)) ;
        }

        public async Task<ActionResult> GetFile(int prerequisiteId, int accountId = 0)
        {
            if (accountId == 0) { accountId = (int)Session["AccountId"]; }
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "PREREQUISITEID", prerequisiteId }, { "ACCOUNTID", accountId } } ;
            Response<RequiredFiles> requiredFileResult = await _genericBusinessLogic.GetAsync(conditions);
            RequiredFiles myFile = requiredFileResult.Data.FirstOrDefault();
            return File(myFile.FileData, myFile.FileType);
        }
        public async Task<JsonResult> IsFilePresent(int prerequisiteId)
        {
            int accountId = (int)Session["AccountId"]; 
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "PREREQUISITEID", prerequisiteId }, { "ACCOUNTID", accountId } };
            Response<RequiredFiles> requiredFileResult = await _genericBusinessLogic.GetAsync(conditions);
            return (requiredFileResult.Success) ?
                        Json(new { Success = true, Data = (requiredFileResult.Data.FirstOrDefault() != null) }, JsonRequestBehavior.AllowGet) :
                        Json(new { Success = false, Message = "Some error encountered while uploading file" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> CoutPresentFile(int trainingId) 
         => Json(await _requiredFileSBusinessLogic.CountFilePresentAsync(trainingId, (int) Session["AccountId"]), JsonRequestBehavior.AllowGet);
    }
}