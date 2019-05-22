using LandProject.Common;
using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Service
{
    public interface IVisitorService
    {
        IEnumerable<VisitorStatisticViewModel> VisitorStatisticAccessPage(string filter, string sort, int page, int pageSize);
        VisitorStatisticTotalViewModel CountVisitor();
        IEnumerable<VisitorStatistic> GetAll();
        bool CheckIpAddressExistInDay(string ip);
        VisitorStatistic Add(VisitorStatistic visitor);
        void Save();
    }
    public class VisitorService : IVisitorService
    {
        IVisitorStatisticRepository _visitorStatisticRepository;
        IUnitOfWork _unitOfWork;
        public VisitorService(IVisitorStatisticRepository visitorStatisticRepository, IUnitOfWork unitOfWork)
        {
            _visitorStatisticRepository = visitorStatisticRepository;
            _unitOfWork = unitOfWork;
        }
        public VisitorStatistic Add(VisitorStatistic visitor)
        {
            return _visitorStatisticRepository.Add(visitor);
        }

        public IEnumerable<VisitorStatistic> GetAll()
        {
            return _visitorStatisticRepository.GetAll();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<VisitorStatisticViewModel> VisitorStatisticAccessPage(string filter, string sort, int page, int pageSize)
        {
            return _visitorStatisticRepository.VisitorStatisticAccessPage(filter,sort,page,pageSize);
        }

        public VisitorStatisticTotalViewModel CountVisitor()
        {
            int totalDay = 0;
            int totalAll = 0;
            var lstVisitor = _visitorStatisticRepository.GetAll().ToList();
            if(lstVisitor.Count() != 0)
            {
                totalAll = lstVisitor.Count();
                totalDay = lstVisitor.Where(x => x.VisitedDate.Year == DateTime.UtcNow.Year
                    && x.VisitedDate.Month == DateTime.UtcNow.Month
                    && x.VisitedDate.Day == DateTime.UtcNow.Day).Count();
            }
            VisitorStatisticTotalViewModel vstorTotal = new VisitorStatisticTotalViewModel();
            vstorTotal.TotalInDay = totalDay;
            vstorTotal.TotalAllDay = totalAll;
            return vstorTotal;
        }

        public bool CheckIpAddressExistInDay(string ip)
        {
            return _visitorStatisticRepository.CheckContains(x => x.IPAddress == ip && x.VisitedDate.Year == DateTime.UtcNow.Year
                    && x.VisitedDate.Month == DateTime.UtcNow.Month
                    && x.VisitedDate.Day == DateTime.UtcNow.Day);
        }
    }
}
