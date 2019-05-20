using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System.Collections.Generic;
using System;

namespace LandProject.Service
{
    public interface ILandFileService
    {
        LandFile Add(LandFile file);
        LandFile Delete(int id);
        void Update(LandFile file);
        void DeleteMutiImageByLProjectID(int lProjectID);
        void DeleteMutiImageByLandNewsID(int lNewsID);
        IEnumerable<LandFile> GetByLandNewsID(int lNewsID);
        IEnumerable<LandFile> GetByLProjectID(int lProjectID);
        void Save();
    }
    public class LandFileService : ILandFileService
    {
        ILandFileRepository _landFileRepository;
        IUnitOfWork _unitOfWork;
        public LandFileService(ILandFileRepository landFileRepository, IUnitOfWork unitOfWork)
        {
            _landFileRepository = landFileRepository;
            _unitOfWork = unitOfWork;

        }
        public LandFile Add(LandFile file)
        {
            return _landFileRepository.Add(file);
        }

        public LandFile Delete(int id)
        {
            return _landFileRepository.Delete(id);
        }

        public void DeleteMutiImageByLProjectID(int lProjectID)
        {
            _landFileRepository.DeleteMulti(x => x.LProjectID == lProjectID);
        }

        public void DeleteMutiImageByLandNewsID(int lNewsID)
        {
            _landFileRepository.DeleteMulti(x => x.LandNewsID == lNewsID);
        }

        public IEnumerable<LandFile> GetByLandNewsID(int lNewsID)
        {
            return _landFileRepository.GetMulti(x => x.LandNewsID == lNewsID);
        }
        public IEnumerable<LandFile> GetByLProjectID(int lProjectID)
        {
            return _landFileRepository.GetMulti(x => x.LProjectID == lProjectID);
        }
        public void Update(LandFile file)
        {
            _landFileRepository.Update(file);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
