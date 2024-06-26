﻿using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        public ActionResult Index()
        {
            IList<Tag> tags = tagRepository.GetAll();
            return View(tags);
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            Tag tag = tagRepository.GetById(id);
            return View(tag);
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                tagRepository.Add(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            Tag tag = tagRepository.GetById(id);
            tagRepository.Delete(tag);
            return RedirectToAction(nameof(Index));
        }
    }
}
