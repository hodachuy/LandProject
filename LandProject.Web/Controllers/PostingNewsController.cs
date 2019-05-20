﻿using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Extensions;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LandProject.Web.Controllers
{
    public class PostingNewsController : Controller
    {
        private ILandNewsService _landNewsService;
        private IAgentService _agentService;
        private ILandFileService _landFileService;
        public PostingNewsController(ILandNewsService landNewsService,
                                     IAgentService agentService,
                                     ILandFileService landFileService)
        {
            _landNewsService = landNewsService;
            _agentService = agentService;
            _landFileService = landFileService;
        }
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NeedSaleAndForRent()
        {
            return View();
        }
        public ActionResult NeedBuyAndNeedRent()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(string captchaCode)
        {
            if (String.IsNullOrEmpty(captchaCode))
            {
                return Json(new
                {
                    message = "Vui lòng nhập mã captcha.",
                    status = false
                });
            }
            if (Session["Captcha"].ToString() != captchaCode)
            {
                return Json(new
                {
                    message = "Mã captcha không đúng.",
                    status = false
                });
            }
            else
            {
                // thông tin đất
                var landNewsJson = System.Web.HttpContext.Current.Request.Unvalidated.Form["landnews"];
                if (landNewsJson == null)
                {
                    return Json(new
                    {
                        message = "Vui lòng cung cấp dữ liệu.",
                        status = false
                    });
                }
                var landNewsVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<LandNewsViewModel>(landNewsJson);
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        message = ModelState,
                        status = false
                    });
                }

                // thông tin liên hệ
                var agentJson = System.Web.HttpContext.Current.Request.Unvalidated.Form["agent"];
                if (agentJson == null)
                {
                    return Json(new
                    {
                        message = "Vui lòng cung cấp dữ liệu.",
                        status = false
                    });
                }
                var agentVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<AgentViewModel>(agentJson);

                // save agent
                Agent agentDb = new Agent();
                agentDb.UpdateAgent(agentVm);
                _agentService.Add(agentDb);
                _agentService.Save();

                // save landnews
                LandNews landNewsDb = new LandNews();
                landNewsDb.AgentID = agentDb.ID;
                landNewsDb.Code = DateTime.Now.ToString() + agentDb.ID;
                landNewsDb.UpdateLandNews(landNewsVm);

                // image avatar
                var files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count != 0)
                {
                    var file = files[0];
                    Guid id = Guid.NewGuid();
                    string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                    string fileName = id + "-" + new FileInfo(file.FileName).Name;
                    file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                    + CommonConstants.FolderLandNews + "/" + fileName));
                    landNewsDb.Image = fileName;
                }

                _landNewsService.Add(landNewsDb);
                _landNewsService.Save();

                // save landfile
                if (files.Count != 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        Guid id = Guid.NewGuid();
                        string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                        string fileName = id + "-" + new FileInfo(file.FileName).Name;
                        file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                        + CommonConstants.FolderLandNews + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            LandNewsID = landNewsDb.ID
                        };
                        _landFileService.Add(fileImage);
                    }
                    _landFileService.Save();
                }
                return Json(new
                {
                    code = landNewsDb.Code,// return mã tin nhắn của bạn
                    status = true
                });
            }
        }


        /// <summary>
        /// RenderCaptcha
        /// </summary>
        /// <returns></returns>
        public string CaptchaIndex()
        {
            string[] strArray = new string[36];
            strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            Random autoRand = new Random();
            string strCaptcha = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                int j = Convert.ToInt32(autoRand.Next(0, 62));
                strCaptcha += strArray[j].ToString();
            }
            Session["Captcha"] = strCaptcha;
            ImageConverter converter = new ImageConverter();
            return Convert.ToBase64String((byte[])converter.ConvertTo(CaptchaGeneration(strCaptcha), typeof(byte[])));
        }
        public Bitmap CaptchaGeneration(string captchatxt)
        {
            Bitmap bmp = new Bitmap(133, 48);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                Font font = new Font("Tahoma", 14);
                graphics.FillRectangle(new SolidBrush(Color.Gray), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(captchatxt, font, new SolidBrush(Color.Gold), 25, 10);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();
            }
            return bmp;
        }
    }
}