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
    public interface ILandTypeService
    {
        IEnumerable<LandType> GetAllByCondition(string condition);
        IEnumerable<LandType> GetAll();
		LandType GetByID(int landTypeID);
		LandType Add(LandType landType);
        LandType Delete(int id);
        void Update(LandType landType);
        void Save();
    }
    public class LandTypeService : ILandTypeService
    {
        ILandTypeRepository _landTypeRepository;
        IUnitOfWork _unitOfWork;
        public LandTypeService(ILandTypeRepository landTypeRepository, IUnitOfWork unitOfWork)
        {
            _landTypeRepository = landTypeRepository;
            _unitOfWork = unitOfWork;
        }
        public LandType Add(LandType landType)
        {
            return _landTypeRepository.Add(landType);
        }

        public LandType Delete(int id)
        {
            return _landTypeRepository.Delete(id);
        }

        public IEnumerable<LandType> GetAll()
        {
            return _landTypeRepository.GetAll().OrderBy(x => x.SortOrder);
        }

        public IEnumerable<LandType> GetAllByCondition(string condition)
        {
            if (!String.IsNullOrEmpty(condition))
                return _landTypeRepository.GetMulti(x => x.Name.Contains(condition));
            return _landTypeRepository.GetAll();
        }

		public LandType GetByID(int landTypeID)
		{
			return _landTypeRepository.GetSingleById(landTypeID);
		}

		public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LandType landType)
        {
            _landTypeRepository.Update(landType);
        }
    }
}
