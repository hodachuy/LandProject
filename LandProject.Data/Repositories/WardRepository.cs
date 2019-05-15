using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface IWardRepository : IRepository<Ward>
    {
    }

    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
