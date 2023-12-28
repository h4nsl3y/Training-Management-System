using DAL.Entity;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.GenericRepositories;
using DAL.Repository.RequiredFileRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.RequiredFileBusinessLogics
{
    public class RequiredFileBusinessLogic : IRequiredFileBusinessLogic
    {
        private readonly IGenericRepository<RequiredFiles> _genericRepository;
        private readonly IRequiredFilesRepository _requiredFileRepository;
        private readonly ILogger _logger;
        private Response<RequiredFiles> _resultError;
        private Response<bool> _resultBoolError;
        public RequiredFileBusinessLogic(IGenericRepository<RequiredFiles> genericRepository, IRequiredFilesRepository requiredFileRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _requiredFileRepository = requiredFileRepository;
            _logger = logger;
            _resultError = new Response<RequiredFiles> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }

        public byte[] GetFileData(string path) => File.ReadAllBytes(path);

        public async Task<Response<bool>> UpdateFileAsync(int prerequisiteId, int accountId, Dictionary<string, object> values)
        {
            try
            {
                return await _requiredFileRepository.UpdateFileAsync(prerequisiteId, accountId, values);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Response<bool>> UploadFileAsync( HttpPostedFileBase file, string path,int accountId, int prerequisiteId)
        {
            try
            {
                file.SaveAs(path);
                byte[] binaryData = GetFileData(path);
                RequiredFiles requiredFile = new RequiredFiles()
                {
                    FileName = Path.GetFileName(file.FileName),
                    FileType = file.ContentType,
                    FileData = binaryData,
                    AccountId = accountId,
                    PrerequisiteId = prerequisiteId
                };
                return await _genericRepository.AddAsync(requiredFile);
            }
            catch(Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
    }
}

