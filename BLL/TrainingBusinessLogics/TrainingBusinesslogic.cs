using DAL.Entity;
using DAL.Logger;
using DAL.Repository.TrainingRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BLL.TrainingBusinessLogics
{
    public class TrainingBusinesslogic : ITrainingBusinessLogic
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly ILogger _logger;
        private readonly Response<Training> _resultError;
        private readonly Response<bool> _resultboolError;
        public TrainingBusinesslogic(ITrainingRepository trainingRepository, ILogger logger)
        {
            _trainingRepository = trainingRepository;
            _logger = logger;
            _resultError = new Response<Training> { Success = false, Message = "an Error has been encounter" };
            _resultboolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<bool>> DeleteTrainingAsync(int trainingId)
        {
            try
            {
                return await _trainingRepository.DeleteTrainingAsync(trainingId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultboolError;
            }
        }
        public async Task<Response<Training>> GetTrainingAsync(int trainingId)
        {
            try
            {
                return await _trainingRepository.GetTrainingAsync(trainingId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Training>> GetAllTrainingAsync()
        {
            try
            {
                return await _trainingRepository.GetAllTrainingAsync();
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Training>> GetEnrolledTrainingAsync(int accountId)
        {
            try
            {
                return await _trainingRepository.GetEnrolledTrainingAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Training>> GetAvailableTrainingAsync(int accountId)
        {
            try
            {
                return await _trainingRepository.GetAvailableTrainingAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<bool>> IsAnyEnrollmentByTrainingAsync(int trainingId)
        {
            try
            {
                Response<bool> response = await _trainingRepository.IsAnyEnrollmentByTrainingAsync(trainingId);
                return response.Data.FirstOrDefault() ?
                    new Response<bool> { Success = true, Data = { true }, Message = "Some users has already registered for this training" }:
                    new Response<bool>() { Success = true, Data = { false } };
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return new Response<bool> { Success = false, Message = "An error has occured" };
            }
        }
        public async Task<Response<bool>> RegisterTrainingAsync(Training training, List<int> prerequisites)
        {
            try
            {
                return await _trainingRepository.RegisterTrainingAsync(training, prerequisites);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultboolError;
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
        public async Task<Response<bool>> UpdateTrainingAsync(Training training, List<int> prerequisites)
        {
            try
            {
                Response<bool> boolResponse = await _trainingRepository.IsAnyEnrollmentByTrainingAsync(training.TrainingId);
                bool IsAnyEnrollment = boolResponse.Data.FirstOrDefault();
                return IsAnyEnrollment ?
                        new Response<bool> { Success = false, Message = "Some users has already registered for this training" } :
                        await _trainingRepository.UpdateTrainingAsync(training, prerequisites);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultboolError;
            }
        }
    }
}