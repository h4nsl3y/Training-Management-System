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
        public async Task<Result<Training>> GetEnrolledAsync(int accountId)
        {
            try
            {
                return await _trainingRepository.GetUnenrolleTrainingListdAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Result<Training>> GetUnenrolledAsync(int accountId)
        {
            try
            {
                return await _trainingRepository.GetUnenrolleTrainingListdAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Result<bool>> SetPrerequisiteAsync(int prerequisiteId, string title)
        {
            try
            {
                return await _trainingRepository.SetPrerequisiteAsync(prerequisiteId, title);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultboolError;
            }
        }
    }
}