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
        private Response<T> _resultError;
        private Response<bool> _resultBoolError;
        public GenericBusinessLogic(IGenericRepository<T> genericRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
            _resultError = new Response<T> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<bool>> AddAsync(T entity)
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
        public async Task<Response<bool>> DeleteAsync(T entity)
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
        public async Task<Response<T>> GetAsync(Dictionary<string, object> conditions)
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
        public async Task<Response<T>> GetAllAsync(Dictionary<string, object> conditions = null)
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
        public async Task<Response<bool>> UpdateAsync(T entity)
        {
            try
            {
                return await _genericRepository.UpdateAsync(entity);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
    }
}
