using LandProject.Common;
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
    public interface IAddressCommonService
    {
        IEnumerable<Province> GetAllProvince();
        IEnumerable<District> GetDistrictByProvinceID(int provinceId);
        IEnumerable<Ward> GetWardByDistrictID(int districtId);
        IEnumerable<CountLandNewsDistrictViewModel> GetTotalLandNewsOfWards(int districtId);

        Province GetDetailProvinceByID(int id);
        District GetDetailDistrictByID(int id);
        Ward GetDetailWardByID(int id);

        void Save();
    }
    public class AddressCommonService : IAddressCommonService
    {
        IProvinceRepository _provinceRepository;
        IDistrictRepository _districtRepository;
        IWardRepository _wardRepository;
        IUnitOfWork _unitOfWork;
        public AddressCommonService(IProvinceRepository provinceRepository,
            IUnitOfWork unitOfWork,
            IDistrictRepository districtRepository,
            IWardRepository wardRepository)
        {
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Province> GetAllProvince()
        {
            return _provinceRepository.GetAll().OrderBy(x=>x.SortOrder);
        }

        public IEnumerable<District> GetDistrictByProvinceID(int provinceId)
        {
            return _districtRepository.GetMulti(x => x.ProvinceID == provinceId).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<Ward> GetWardByDistrictID(int districtId)
        {
            return _wardRepository.GetMulti(x => x.DistrictID == districtId).OrderBy(x => x.SortOrder);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<CountLandNewsDistrictViewModel> GetTotalLandNewsOfWards(int districtId)
        {
            return _wardRepository.GetTotalLandNewsOfWards(districtId);
        }

        public Province GetDetailProvinceByID(int id)
        {
            return _provinceRepository.GetSingleById(id);
        }

        public District GetDetailDistrictByID(int id)
        {
            return _districtRepository.GetSingleById(id);
        }

        public Ward GetDetailWardByID(int id)
        {
            return _wardRepository.GetSingleById(id);
        }
    }
}
