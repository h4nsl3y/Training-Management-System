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
        private readonly IGenericRepository<Prerequisite> _genericRepository;
        private readonly IPrerequisiteRepository _prerequisiteRepository;
        private readonly ILogger _logger;
        private Result<Prerequisite> _resultError;
        Result<int> _errorResult;
        public PrequisiteBusinessLogic(IPrerequisiteRepository prerequisiteRepository, 
                                        IGenericRepository<Prerequisite> genericRepository,
                                        ILogger logger)
        {
            _genericRepository = genericRepository; 
            _prerequisiteRepository = prerequisiteRepository;
            _logger = logger;
            _resultError = new Result<Prerequisite> { Success = false, Message = "an Error has been encounter" };
            _errorResult = new Result<int> { Success = false, Message = "an Error has been encounter" };
        }
        public Result<Prerequisite> GetPrequisite(int trainingid)
        {
            try
            {
                return _prerequisiteRepository.GetPrequisiteAsync(trainingid);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<int> GetPrerequisiteIdByEmployee(int accountId)
        {
            try
            {
                return _prerequisiteRepository.GetPrerequisiteIdByEmployee(accountId); 
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _errorResult;
            }
        }
    }
}
