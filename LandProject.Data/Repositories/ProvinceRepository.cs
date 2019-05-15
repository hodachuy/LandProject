using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface IProvinceRepository : IRepository<Province>
    {
    }

    public class ProvinceRepository : RepositoryBase<Province>, IProvinceRepository
    {
        public ProvinceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
