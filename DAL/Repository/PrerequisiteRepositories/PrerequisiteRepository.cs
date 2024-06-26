﻿using DAL.DataBaseHelpers;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.PrerequisiteRepositories
{
    public class PrerequisiteRepository : RepositoryService, IPrerequisiteRepository
    {
        private readonly IDataBaseHelper<Prerequisite> _dataBaseHelper;

        public PrerequisiteRepository(IDataBaseHelper<Prerequisite> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
        }
        public async Task<Response<Prerequisite>> GetPrequisiteAsync(int trainingId)
        {
            string query = @"SELECT * FROM PREREQUISITE 
                             WHERE PREREQUISITEID IN 
                                (SELECT PREREQUISITEID FROM TrainingPrerequisite 
                                WHERE TRAININGID = @TRAININGID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            return await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<int>> GetPrerequisiteIdByEmployee(int accountId)
        {
            
            string query = @"SELECT * FROM PREREQUISITE 
                             WHERE PREREQUISITEID IN 
                                (SELECT PREREQUISITEID FROM REQUIREDFILES 
                                WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ACCOUNTID", accountId) };
            Response<Prerequisite> prerequisiteResult = await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
            return new Response<int>() { 
                Success = true ,
                Data = prerequisiteResult.Data.Select(prerequisites => prerequisites.PrerequisiteId).ToList()
             };
        }
    }
}
