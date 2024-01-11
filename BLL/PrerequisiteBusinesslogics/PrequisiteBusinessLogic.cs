using DAL.Entity;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.GenericRepositories;
using DAL.Repository.PrerequisiteRepositories;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.PrerequisiteBusinesslogics
{
    public class PrequisiteBusinessLogic : IPrerequisiteBusinessLogic
    {
        private readonly IPrerequisiteRepository _prerequisiteRepository;
        private readonly ILogger _logger;
        private readonly Response<Prerequisite> _resultError;
        private readonly Response<int> _errorResult;
        public PrequisiteBusinessLogic(IPrerequisiteRepository prerequisiteRepository, 
                                        ILogger logger)
        {
            _prerequisiteRepository = prerequisiteRepository;
            _logger = logger;
            _resultError = new Response<Prerequisite> { Success = false, Message = "an Error has been encounter" };
            _errorResult = new Response<int> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<Prerequisite>> GetPrequisiteAsync(int trainingid)
        {
            try
            {
                return await _prerequisiteRepository.GetPrequisiteAsync(trainingid);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<int>> GetPrerequisiteIdByEmployee(int accountId)
        {
            try
            {
                return await _prerequisiteRepository.GetPrerequisiteIdByEmployee(accountId); 
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _errorResult;
            }
        }
    }
}
