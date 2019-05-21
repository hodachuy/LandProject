using LandProject.Common;
using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface IWardRepository : IRepository<Ward>
    {
        IEnumerable<CountLandNewsDistrictViewModel> GetTotalLandNewsOfWards(int districtId);
    }

    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<CountLandNewsDistrictViewModel> GetTotalLandNewsOfWards(int districtId)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@DistrictID",districtId),
            };
            return DbContext.Database.SqlQuery<CountLandNewsDistrictViewModel>("sp_GetTotalLandNewsByWards @DistrictID", parameters);

        }
    }
}
