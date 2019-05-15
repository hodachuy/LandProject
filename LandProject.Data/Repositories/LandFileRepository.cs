using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandFileRepository : IRepository<LandFile>
    {
    }

    public class LandFileRepository : RepositoryBase<LandFile>, ILandFileRepository
    {
        public LandFileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
