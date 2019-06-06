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
    public interface ILandNewsScheduleService
    {
        IEnumerable<LandNewsSchedule> GetAllByCondition(string condition);
        IEnumerable<LandNewsSchedule> GetAll();
        LandNewsSchedule GetByID(int scheduleId);
        LandNewsSchedule Add(LandNewsSchedule schedule);
        LandNewsSchedule Delete(int id);

		LandNewsSchedule GetSingleAlias(string condition);

		void Update(LandNewsSchedule schedule);
        void Save();
    }
    public class LandNewsScheduleService : ILandNewsScheduleService
    {
        ILandNewsScheduleRepository _landNewsRepository;
        IUnitOfWork _unitOfWork;
        public LandNewsScheduleService(IUnitOfWork unitOfWork, ILandNewsScheduleRepository landNewsRepository)
        {
            _unitOfWork = unitOfWork;
            _landNewsRepository = landNewsRepository;
        }
        public LandNewsSchedule Add(LandNewsSchedule schedule)
        {
            return _landNewsRepository.Add(schedule);
        }

        public LandNewsSchedule Delete(int id)
        {
            return _landNewsRepository.Delete(id);
        }

        public IEnumerable<LandNewsSchedule> GetAll()
        {
            return _landNewsRepository.GetAll();
        }

        public IEnumerable<LandNewsSchedule> GetAllByCondition(string condition)
        {
            return _landNewsRepository.GetMulti(x => x.Name.Contains(condition));
        }

		public LandNewsSchedule GetSingleAlias(string condition)
		{
			return _landNewsRepository.GetSingleByCondition(x => x.Alias.Contains(condition));
		}

		public LandNewsSchedule GetByID(int scheduleId)
        {
            return _landNewsRepository.GetSingleById(scheduleId);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LandNewsSchedule schedule)
        {
            _landNewsRepository.Update(schedule);
        }
    }
}
