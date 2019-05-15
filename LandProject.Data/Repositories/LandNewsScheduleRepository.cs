using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandNewsScheduleRepository : IRepository<LandNewsSchedule>
    {
    }

    public class LandNewsScheduleRepository : RepositoryBase<LandNewsSchedule>, ILandNewsScheduleRepository
    {
        public LandNewsScheduleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
