using DAL.Entity;
using DAL.Logger;
using DAL.Repository.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GenericBusinessLogics
{
    public class GenericBusinessLogic<T> : IGenericBusinessLogic<T> where T : ISystemEntity
    {
        private readonly IGenericRepository<T> _genericRepository;
        private readonly ILogger _logger;
        private Result<T> _resultError;
        private Result<bool> _resultBoolError;
        public GenericBusinessLogic(IGenericRepository<T> genericRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
            _resultError = new Result<T> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Result<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Result<bool>> AddAsync(T entity)
        {
            try
            {
                return await _genericRepository.AddAsync(entity);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Result<bool>> DeleteAsync(T entity)
        {
            try
            {
                return await _genericRepository.DeleteAsync(entity);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Result<T>> GetAsync(Dictionary<string, object> conditions)
        {
            try
            {
                return await _genericRepository.GetAsync(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Result<T>> GetAllAsync(Dictionary<string, object> conditions = null)
        {
            try
            {
                return await _genericRepository.GetAllAsync(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
           
        }
        public async Task<Result<bool>> UpdateAsync(int Id, Dictionary<string, object> conditions)
        {
            try
            {
                return await _genericRepository.UpdateAsync(Id, conditions);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
    }
}
