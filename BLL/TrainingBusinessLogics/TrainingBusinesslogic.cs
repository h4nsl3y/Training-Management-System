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
        private Response<Training> _resultError;
        private Response<bool> _resultboolError;
        public TrainingBusinesslogic(ITrainingRepository trainingRepository, ILogger logger)
        {
            _trainingRepository = trainingRepository;
            _logger = logger;
            _resultError = new Response<Training> { Success = false, Message = "an Error has been encounter" };
            _resultboolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<Training>> GetEnrolledAsync(int accountId)
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
        public async Task<Response<Training>> GetUnenrolledAsync(int accountId)
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
        public async Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title)
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