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
    public interface ILandNewsRepository : IRepository<LandNews>
    {
        IEnumerable<LandNews> GetAllByTag(string tag, int pageIndex, int pageSize, out int totalRow);

        IEnumerable<LandNewsFilterViewModel> GetLandNewsByFilter(string filter, string sort, int page, int pageSize);

        LandNewsFilterViewModel GetLandNewsByID(int id);

        IEnumerable<CountLandNewsByLandTypeViewModel> GetTotalLandNewsToLandType();

        void PublishLandNews(LandNews lnews);

        void DeleteLandNews(LandNews lnews);
    }

    public class LandNewsRepository : RepositoryBase<LandNews>, ILandNewsRepository
    {
        public LandNewsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<CountLandNewsByLandTypeViewModel> GetTotalLandNewsToLandType()
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@LandNewsID",1),
            };
            return DbContext.Database.SqlQuery<CountLandNewsByLandTypeViewModel>("sp_GetTotalLandNewsToLandType @LandNewsID", parameters);
        }
        public IEnumerable<LandNewsFilterViewModel> GetLandNewsByFilter(string filter, string sort, int page, int pageSize)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@Filter",(String.IsNullOrEmpty(filter) == true ? "": filter)),
                new SqlParameter("@Sort",(String.IsNullOrEmpty(sort) == true ? "": sort)),
                new SqlParameter("@PageNumber",page),
                new SqlParameter("@PageSize",pageSize),
            };
            return DbContext.Database.SqlQuery<LandNewsFilterViewModel>("sp_GetLandNewsByFilter @Filter,@Sort,@PageNumber,@PageSize", parameters);

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

        public LandNewsFilterViewModel GetLandNewsByID(int id)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@LandNewsID",id)
            };
            return DbContext.Database.SqlQuery<LandNewsFilterViewModel>("sp_GetLandNewsByID @LandNewsID", parameters).FirstOrDefault();
        }

        public void PublishLandNews(LandNews lnews)
        {
            DbContext.LandNewss.Attach(lnews);
            DbContext.Entry(lnews).Property(x => x.IsPublished).IsModified = true;
        }

        public void DeleteLandNews(LandNews lnews)
        {
            DbContext.LandNewss.Attach(lnews);
            DbContext.Entry(lnews).Property(x => x.IsDelete).IsModified = true;
        }
    }
}
