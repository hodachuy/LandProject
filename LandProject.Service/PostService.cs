using System;
using System.Collections.Generic;
using LandProject.Data.Infrastructure;
using LandProject.Data.Repositories;
using LandProject.Model.Models;
using System.Linq;

namespace LandProject.Service
{
    public interface IPostService
    {
        void Add(Post post);

        void Update(Post post);

        void Delete(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetAllByCondition(string condition);

        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

		IEnumerable<Post> GetAllByCategory(int categoryId);

		Post GetById(int id);

        IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);

        void SaveChanges();
    }

    public class PostService : IPostService
    {
        IPostRepository _postRepository;
        IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Post post)
        {
            _postRepository.Add(post);
        }

        public void Delete(int id)
        {
            _postRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll(new string[] { "PostCategory" });
        }

		public IEnumerable<Post> GetAllByCategory(int categoryId)
		{
			return _postRepository.GetMulti(x=>x.PostCategoryID == categoryId, new string[] { "PostCategory" });
		}

		public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x =>x.PostCategoryID == categoryId, out totalRow, page, pageSize, new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAllByCondition(string condition)
        {
            return _postRepository.GetMulti(x => x.Name.Contains(condition), new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {
            //TODO: Select all post by tag
            return _postRepository.GetAllByTag(tag, page, pageSize, out totalRow);

        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Post GetById(int id)
        {
            return _postRepository.GetSingleByCondition(x=>x.ID == id, new string[] { "PostCategory" });
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Post post)
        {
            _postRepository.Update(post);
        }
    }
}