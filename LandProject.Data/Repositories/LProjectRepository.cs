using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILProjectRepository : IRepository<LProject>
    {
    }

    public class LProjectRepository : RepositoryBase<LProject>, ILProjectRepository
    {
        public LProjectRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
