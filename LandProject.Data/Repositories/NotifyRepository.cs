using LandProject.Data.Infrastructure;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Data.Repositories
{
	public interface INotifyRepository : IRepository<Notify>
	{
	}

	public class NotifyRepository : RepositoryBase<Notify>, INotifyRepository
	{
		public NotifyRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}
	}
}
