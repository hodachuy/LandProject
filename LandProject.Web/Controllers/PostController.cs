using AutoMapper;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
    public class PostController : Controller
    {
		ILandCategoryService _landCategoryService;
		ILandTypeService _landTypeService;
		IPostService _postService;
		IPostCategoryService _postCategoryService;

		public PostController(ILandCategoryService landCategoryService,
			ILandTypeService landTypeService,
			IPostService postService,
			IPostCategoryService postCategoryService)
		{
			_landCategoryService = landCategoryService;
			_landTypeService = landTypeService;
			_postService = postService;
			_postCategoryService = postCategoryService;
		}
		// GET: Post
		public ActionResult Index()
        {
			var landType = _landTypeService.GetAll().ToList();
			ViewBag.LandType = landType;

			var postCommonVm = new PostCommonViewModel();

			var lstPostTop = _postService.GetAll().Where(x => x.Status).OrderByDescending(x => x.UpdatedDate).Take(10);
			var lstPostTopVm = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(lstPostTop);
			postCommonVm.PostTop = lstPostTopVm;

			var lstPostCategory = _postCategoryService.GetAll().OrderBy(x =>x.DisplayOrder);
			var lstPostCategoryVm = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(lstPostCategory);

			if (lstPostCategoryVm.Count() != 0)
			{
				foreach(var item in lstPostCategoryVm)
				{
					var lstPost = _postService.GetAllByCategory(item.ID).Where(x =>x.IsPublished).OrderByDescending(x =>x.UpdatedDate).Take(3);
					item.Posts = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(lstPost);
				}
			}
			postCommonVm.PostListCategory = lstPostCategoryVm;

			return View(postCommonVm);
        }

		public ActionResult PostCategory(int id)
		{
			var landType = _landTypeService.GetAll().ToList();
			ViewBag.LandType = landType;

			PostCategoryViewModel postCategoryVm = new PostCategoryViewModel();

			var postCategoryDb = _postCategoryService.GetById(id);
			postCategoryVm = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategoryDb);

			var lstPost = _postService.GetAllByCategory(id);
			var lstPostVm = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(lstPost);
			postCategoryVm.Posts = lstPostVm;
			return View(postCategoryVm);
		}

		public ActionResult Detail(int id)
		{
			var landType = _landTypeService.GetAll().ToList();
			ViewBag.LandType = landType;

			var post = _postService.GetById(id);
			var postVm = Mapper.Map<Post, PostViewModel>(post);
			return View(postVm);
		}
	}
}