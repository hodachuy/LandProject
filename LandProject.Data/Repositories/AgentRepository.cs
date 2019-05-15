using LandProject.Data.Infrastructure;
using LandProject.Model.Models;

namespace LandProject.Data.Repositories
{
    public interface IAgentRepository : IRepository<Agent>
    {
    }

    public class AgentRepository : RepositoryBase<Agent>, IAgentRepository
    {
        public AgentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}