using BLL.GenericBusinessLogics;
using BLL.RequiredFileBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public ActionResult UpdateFile(int prerequisiteId)
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

                    Result<bool> boolResult = _requiredFileSBusinessLogic.UpdateFile(prerequisiteId, accountId , values);
                    return (boolResult.Success) ?
                        Json(new { message = "success" }) :
                        Json(new { message = "failed", data = "Some error encountered while uploading file" });
                }
                else { return Json(new { message = "failed", data = "Some error encountered while uploading file" }); }
            }
            else { return Json(new { message = "failed", data = "No file has been recieved" }); }
        }

        public ActionResult UploadFile(int prerequisiteId, HttpPostedFileBase[] files)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/data"), "fileData");
                    file.SaveAs(path);
                    byte[] binaryData = _requiredFileSBusinessLogic.GetFileData(path);
                    RequiredFiles requiredFile = new RequiredFiles()
                    {
                        FileName = Path.GetFileName(file.FileName),
                        FileType = file.ContentType,
                        FileData = binaryData,
                        AccountId = (int)Session["AccountId"],
                        PrerequisiteId = prerequisiteId
                    };
                    Result<bool> boolResult = _genericBusinessLogic.Add(requiredFile);
                    return (boolResult.Success) ?
                        Json(new { message = "success" }) :
                        Json(new { message = "failed", data = "Some error encountered while uploading file" });
                }
                else { return Json(new { message = "failed", data = "Some error encountered while uploading file" }); }
            }
            else { return Json(new { message = "failed", data = "No file has been recieved" }); }
        }

        public ActionResult GetFile(int prerequisiteId, int accountId = 0)
        {
            if (accountId == 0) { accountId = (int)Session["AccountId"]; }
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            conditions.Add("ACCOUNTID", accountId);
            Result<RequiredFiles> requiredFileResult = _genericBusinessLogic.Get(conditions);
            RequiredFiles myFile = requiredFileResult.Data.FirstOrDefault();
            byte[] fileByte = myFile.FileData;
            string fileType = myFile.FileType;
            string filename = myFile.FileName;

            return File(fileByte, fileType);
        }
        public JsonResult IsFilePresent(int prerequisiteId)
        {
            int accountId = (int)Session["AccountId"]; 
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            conditions.Add("ACCOUNTID", accountId);
            Result<RequiredFiles> requiredFileResult = _genericBusinessLogic.Get(conditions);
            return (requiredFileResult.Success) ?
                        Json(new { message = "success", data = (requiredFileResult.Data.FirstOrDefault() != null) }) :
                        Json(new { message = "failed", data = "Some error encountered while uploading file" });

        }
    }
}