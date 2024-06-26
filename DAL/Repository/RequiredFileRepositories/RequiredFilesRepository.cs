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

namespace DAL.Repository.RequiredFileRepositories
{
    public class RequiredFilesRepository : IRequiredFilesRepository
    {
        private readonly DataBaseHelper<RequiredFiles> _dataBaseHelper;
        private readonly string primaryKey;
        private readonly string tableName;
        public RequiredFilesRepository(DataBaseHelper<RequiredFiles> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
            PropertyInfo[] properties = typeof(RequiredFiles).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
            tableName = typeof(RequiredFiles).Name;
        }
        public async Task<Response<bool>> UpdateFileAsync(int prerequisiteId, int accountId, Dictionary<string, object> values)
        {
            string query = "UPDATE REQUIREDFILES SET ";
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var value in values)
            {
                query += $"{value.Key} = @{value.Key} ,";
                parameters.Add(new SqlParameter($"@{value.Key}", value.Value));
            }
            query = query.Substring(0, query.Length - 1);

            query += " WHERE PREREQUISITEID  = @PREREQUISITEID AND ACCOUNTID = @ACCOUNTID; ";
            parameters.Add(new SqlParameter("@PREREQUISITEID", prerequisiteId));
            parameters.Add(new SqlParameter("@ACCOUNTID", accountId));

            return await _dataBaseHelper.AffectedRowsAsync(query, parameters);
        }
        public async Task<Response<int>> CountFilePresentAsync(int trainingId, int accountId)
        {
            string query = @"SELECT FILEDATA FROM REQUIREDFILES
                            WHERE REQUIREDFILES.PREREQUISITEID IN 
                                        (SELECT PREREQUISITEID FROM TRAININGPREREQUISITE WHERE TRAININGID = @TRAININGID)
                            AND ACCOUNTID = @ACCOUNTID";


            List<SqlParameter> parameters = new List<SqlParameter>() 
            { 
                new SqlParameter("@ACCOUNTID", accountId),
                new SqlParameter("@TRAININGID", trainingId)
            };
            Response<RequiredFiles> result = await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
            return new Response<int>() { Success = true, Data = { result.Data.Count } };
        }
    }
}
