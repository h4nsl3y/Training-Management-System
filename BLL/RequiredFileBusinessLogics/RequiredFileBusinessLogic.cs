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
        private readonly IGenericRepository<Account> _genericRepository;
        private readonly IRequiredFilesRepository _requiredFileRepository;
        private readonly ILogger _logger;
        private Result<Account> _resultError;
        private Result<bool> _resultBoolError;
        public RequiredFileBusinessLogic(IGenericRepository<Account> genericRepository, IRequiredFilesRepository requiredFileRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _requiredFileRepository = requiredFileRepository;
            _logger = logger;
            _resultError = new Result<Account> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Result<bool> { Success = false, Message = "an Error has been encounter" };
        }

        public byte[] GetFileData(string path) 
        {
            byte[] binaryData = File.ReadAllBytes(path);
            return binaryData;
        }
        public Result<bool> UpdateFile(int prerequisiteId, int accountId, Dictionary<string, object> values)
        {
            try
            {
                Result<bool> result = _requiredFileRepository.UpdateFile(prerequisiteId, accountId, values);    
                return result;
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
    }
}

