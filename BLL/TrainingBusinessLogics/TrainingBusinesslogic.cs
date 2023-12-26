using DAL.Entity;
using DAL.Logger;
using DAL.Repository.TrainingRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.TrainingBusinessLogics
{
    public class TrainingBusinesslogic : ITrainingBusinesslogic
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly ILogger _logger;
        private Result<Training> _resultError;
        private Result<bool> _resultboolError;
        public TrainingBusinesslogic(ITrainingRepository trainingRepository, ILogger logger)
        {
            _trainingRepository = trainingRepository;
            _logger = logger;
            _resultError = new Result<Training> { Success = false, Message = "an Error has been encounter" };
            _resultboolError = new Result<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public Result<Training> GetEnrolled(int accountId)
        {
            try
            {
                return _trainingRepository.GetenrolledAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<Training> GetUnenrolled(int accountId)
        {
            try
            {
                return _trainingRepository.GetUnenrolled(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<bool> SetPrerequisite(int prerequisiteId, string title)
        {
            try
            {
                return _trainingRepository.SetPrerequisiteAsync(prerequisiteId, title);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultboolError;
            }
        }
    }
}