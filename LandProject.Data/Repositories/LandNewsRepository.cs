using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
    public interface ILandNewsRepository : IRepository<LandNews>
    {
        IEnumerable<LandNews> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);
    }

    public class LandNewsRepository : RepositoryBase<LandNews>, ILandNewsRepository
    {
        public LandNewsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<LandNews> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow)
        {
            var query = from l in DbContext.LandNewss
                        join lt in DbContext.LandNewsTags
                        on l.ID equals lt.LandNewsID
                        where lt.TagID == tag && l.IsPublished
                        orderby l.CreatedDate descending
                        select l;

            totalRow = query.Count();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return query;
        }
    }
}
