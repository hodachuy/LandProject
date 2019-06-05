using System;
using LandProject.Common;
using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System.Collections.Generic;

namespace LandProject.Service
{
    public interface ICommonService
    {
        IEnumerable<Slide> GetSlide();
        IEnumerable<Slide> GetAll(string keyword);
        Slide Add(Slide slide);
        void Update(Slide slide);
        Slide Delete(int id);
        Slide GetByIdSlide(int id);
        SystemConfig GetSystemConfig(string code);
        void Save();
    }
    public class CommonService : ICommonService
    {
        ISlideRepository _slideRepository;
        ISystemConfigRepository _systemConfigRepository;
        IUnitOfWork _unitOfWork;
        public CommonService( ISlideRepository slideRepository, ISystemConfigRepository _systemConfigRepository,IUnitOfWork unitOfWork)
        {
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Slide> GetSlide()
        {
            return _slideRepository.GetMulti(x => x.Status);
        }

        public Slide Add(Slide slide)
        {
            return _slideRepository.Add(slide);
        }

        public void Update(Slide slide)
        {
            _slideRepository.Update(slide);
        }

        public Slide Delete(int id)
        {
            return _slideRepository.Delete(id);
        }

        public Slide GetByIdSlide(int id)
        {
            return _slideRepository.GetSingleById(id);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Slide> GetAll(string keyword)
        {
            if (!String.IsNullOrEmpty(keyword))
                return _slideRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _slideRepository.GetAll();
        }

        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }
    }
}
