﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using System.Web;
using System.Web.Caching;
using System;
using System.Runtime.CompilerServices;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        // GET: /Store/
        public async Task<ActionResult> Index()
        {
            return View(await _storeContext.Genres.ToListAsync());
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<ActionResult> Browse(string genre)
        {
            return View(await _storeContext.Genres.Include("Albums").SingleAsync(g => g.Name == genre));
        }

        public ActionResult Details(int id)
        {
            var album = _storeContext.Albums.SingleOrDefault(i => i.AlbumId == id);

            if (album != null) {
                if (album.Price <=0)
                {
                    throw new Exception("Invalid album price found.");
                }
                else
                {
                    return View(album);
                }
            }
            else
            {
                return (ActionResult)HttpNotFound();
            }

            // return album != null ? View(album) : (ActionResult)HttpNotFound();
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            return PartialView(
                _storeContext.Genres.OrderByDescending(
                    g => g.Albums.Sum(a => a.OrderDetails.Sum(od => od.Quantity))).Take(9).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}