using DAL.Entity;
using DAL.Logger;
using DAL.Repository.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Result<bool> Add(T entity)
        {
            try
            {
                return _genericRepository.Add(entity);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public Result<bool> Delete(T entity)
        {
            try
            {
                return _genericRepository.Delete(entity);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public Result<T> Get(Dictionary<string, object> conditions)
        {
            try
            {
                return _genericRepository.Get(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<T> GetAll(Dictionary<string, object> conditions = null)
        {
            try
            {
                return _genericRepository.GetAll(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
           
        }
        public Result<bool> Update(int Id, Dictionary<string, object> conditions)
        {
            try
            {
                return _genericRepository.Update(Id, conditions);
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
    }
}
