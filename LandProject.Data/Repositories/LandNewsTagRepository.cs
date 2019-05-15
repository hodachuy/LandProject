using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandNewsTagRepository : IRepository<LandNewsTag>
    {
    }

    public class LandNewsTagRepository : RepositoryBase<LandNewsTag>, ILandNewsTagRepository
    {
        public LandNewsTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
