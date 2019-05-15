using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILProjectCategoryRepository : IRepository<LProjectCategory>
    {
    }

    public class LProjectCategoryRepository : RepositoryBase<LProjectCategory>, ILProjectCategoryRepository
    {
        public LProjectCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
