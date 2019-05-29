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
    public interface INotifyService
    {
        IEnumerable<Notify> GetAll();
        IEnumerable<Notify> GetNewMessage(string table);
        Notify GetById(int id);
        Notify GetByConditionID(int id);
        Notify Add(Notify notify);
        Notify Delete(int id);
        void Update(Notify notify);
        void Save();
    }

    public class NotifyService : INotifyService
    {
        INotifyRepository _notifyRepository;
        IUnitOfWork _unitOfWork;
        public NotifyService(INotifyRepository notifyRepository, IUnitOfWork unitOfWork)
        {
            _notifyRepository = notifyRepository;
            _unitOfWork = unitOfWork;
        }

        public Notify Add(Notify notify)
        {
            return _notifyRepository.Add(notify);
        }

        public Notify Delete(int id)
        {
            return _notifyRepository.Delete(id);
        }

        public IEnumerable<Notify> GetAll()
        {
            return _notifyRepository.GetAll();
        }

        public Notify GetById(int id)
        {
            return _notifyRepository.GetSingleById(id);
        }
        public Notify GetByConditionID(int id)
        {
            return _notifyRepository.GetSingleByCondition(x => x.TableItemID == id);
        }

        public IEnumerable<Notify> GetNewMessage(string table)
        {
            return _notifyRepository.GetMulti(x => x.TableName.Contains(table) && x.IsRead == false);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Notify notify)
        {
            _notifyRepository.Update(notify);
        }
    }
}
