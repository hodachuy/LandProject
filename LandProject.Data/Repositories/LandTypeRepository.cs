using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandTypeRepository : IRepository<LandType>
    {
    }

    public class LandTypeRepository : RepositoryBase<LandType>, ILandTypeRepository
    {
        public LandTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
