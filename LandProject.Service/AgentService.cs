using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Service
{
    public interface IAgentService
    {
        Agent Add(Agent Agent);
        Agent Delete(int id);
        void Update(Agent Agent);
        void Save();
    }
    public class AgentService : IAgentService
    {
        IAgentRepository _agentRepository;
        IUnitOfWork _unitOfWork;
        public AgentService(IAgentRepository agentRepository, IUnitOfWork unitOfWork)
        {
            _agentRepository = agentRepository;
            _unitOfWork = unitOfWork;
        }
        public Agent Add(Agent Agent)
        {
            return _agentRepository.Add(Agent);
        }

        public Agent Delete(int id)
        {
            return _agentRepository.Delete(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Agent Agent)
        {
            _agentRepository.Update(Agent);
        }
    }
}
