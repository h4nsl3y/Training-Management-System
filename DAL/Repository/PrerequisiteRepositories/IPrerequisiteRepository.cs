using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.PrerequisiteRepositories
{
    public interface IPrerequisiteRepository
    {
        Result<Prerequisite> GetPrequisite(int trainingId);
    }
}
