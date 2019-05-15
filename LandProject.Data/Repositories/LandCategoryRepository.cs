using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandCategoryRepository : IRepository<LandCategory>
    {
    }

    public class LandCategoryRepository : RepositoryBase<LandCategory>, ILandCategoryRepository
    {
        public LandCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
