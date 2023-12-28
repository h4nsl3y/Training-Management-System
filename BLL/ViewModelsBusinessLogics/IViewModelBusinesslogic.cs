﻿using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModelsBusinessLogics
{
    public interface IViewModelBusinesslogic<T>
    {
        Task<Response<T>> GetTrainingEnrollmentView(int accountId);
    }
}
