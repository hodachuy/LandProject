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
    public interface ILProjectCategoryService
    {
		IEnumerable<LProjectCategory> GetAllByCondition(string condition);
		IEnumerable<LProjectCategory> GetAll();
        LProjectCategory Add(LProjectCategory LProjectCategory);
		LProjectCategory GetByID(int lProjectCategoryID);
		LProjectCategory Delete(int id);
        void Update(LProjectCategory LProjectCategory);
        void Save();
    }
    public class LProjectCategoryService : ILProjectCategoryService
    {
        ILProjectCategoryRepository _lProjectCategoryRepository;
        IUnitOfWork _unitOfWork;
        public LProjectCategoryService(ILProjectCategoryRepository lProjectCategoryRepository, IUnitOfWork unitOfWork)
        {
            _lProjectCategoryRepository = lProjectCategoryRepository;
            _unitOfWork = unitOfWork;
        }

		public IEnumerable<LProjectCategory> GetAllByCondition(string condition)
		{
			if (!String.IsNullOrEmpty(condition))
				return _lProjectCategoryRepository.GetMulti(x => x.Name.Contains(condition));
			return _lProjectCategoryRepository.GetAll();
		}

		public LProjectCategory Add(LProjectCategory LProjectCategory)
        {
            return _lProjectCategoryRepository.Add(LProjectCategory);
        }

        public LProjectCategory Delete(int id)
        {
            return _lProjectCategoryRepository.Delete(id);
        }

        public IEnumerable<LProjectCategory> GetAll()
        {
            return _lProjectCategoryRepository.GetAll();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LProjectCategory LProjectCategory)
        {
            _lProjectCategoryRepository.Update(LProjectCategory);
        }

		public LProjectCategory GetByID(int lProjectCategoryID)
		{
			return _lProjectCategoryRepository.GetSingleById(lProjectCategoryID);
		}
	}
}
