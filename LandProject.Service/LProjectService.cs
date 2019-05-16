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
    public interface ILProjectService
    {
        IEnumerable<LProject> GetAll();
		IEnumerable<LProject> GetAllByCondition(string lProjectName, int lProjectCategoryID);
		IEnumerable<LProject> GetByLProjectCategoryID(int lProjectCategoryID);
		LProject Add(LProject LProject);
        LProject Delete(int id);
        void Update(LProject LProject);
        void Save();
    }
    public class LProjectService : ILProjectService
    {
        ILProjectRepository _lProjectRepository;
        IUnitOfWork _unitOfWork;
        public LProjectService(ILProjectRepository lProjectRepository, IUnitOfWork unitOfWork)
        {
            _lProjectRepository = lProjectRepository;
            _unitOfWork = unitOfWork;
        }
        public LProject Add(LProject LProject)
        {
            return _lProjectRepository.Add(LProject);
        }

        public LProject Delete(int id)
        {
            return _lProjectRepository.Delete(id);
        }

        public IEnumerable<LProject> GetAll()
        {
            return _lProjectRepository.GetAll(new string[] { "LProjectCategory" });
        }

		public IEnumerable<LProject> GetAllByCondition(string lProjectName, int lProjectCategoryID)
		{
			var lstLProject = _lProjectRepository.GetAll(new string[] { "LProjectCategory" });
			if (!String.IsNullOrEmpty(lProjectName))
				lstLProject = lstLProject.Where(x => x.Name.Contains(lProjectName));
			if (lProjectCategoryID != 0)
				lstLProject = lstLProject.Where(x => x.LProjectCaregoryID == lProjectCategoryID);
			return lstLProject;
		}

		public IEnumerable<LProject> GetByLProjectCategoryID(int lProjectCategoryID)
		{
			return _lProjectRepository.GetMulti(x => x.LProjectCaregoryID == lProjectCategoryID, new string[] {"LProjectCategory"});
		}

		public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(LProject LProject)
        {
            _lProjectRepository.Update(LProject);
        }
    }
}
