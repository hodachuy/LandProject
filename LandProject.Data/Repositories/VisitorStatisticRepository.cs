using LandProject.Common;
using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace LandProject.Data.Repositories
{
    public interface IVisitorStatisticRepository : IRepository<VisitorStatistic>
    {
        IEnumerable<VisitorStatisticViewModel> VisitorStatisticAccessPage(string filter, string sort, int page, int pageSize);
    }

    public class VisitorStatisticRepository : RepositoryBase<VisitorStatistic>, IVisitorStatisticRepository
    {
        public VisitorStatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<VisitorStatisticViewModel> VisitorStatisticAccessPage(string filter, string sort, int page, int pageSize)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@Filter",(String.IsNullOrEmpty(filter) == true ? "": filter)),
                new SqlParameter("@Sort",(String.IsNullOrEmpty(sort) == true ? "": sort)),
                new SqlParameter("@PageNumber",page),
                new SqlParameter("@PageSize",pageSize),
            };
            return DbContext.Database.SqlQuery<VisitorStatisticViewModel>("sp_StatisticVisitorAccessPage @Filter,@Sort,@PageNumber,@PageSize", parameters);

        }
    }
}