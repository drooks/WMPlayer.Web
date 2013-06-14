using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMPlayer.Web.Models;

namespace WMPlayer.Web.Controllers
{
    public class StandAloneController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLibrary() {
            return new JsonResult 
            { 
                Data = Library.GetLibrary(), 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet 
            };
        }

        public JsonResult GetSongInfo(Guid id) {
            return new JsonResult {
                Data = Library.GetLibrary().FirstOrDefault(x => x.SongId == id),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetSong(Guid id)
        {
            Song song = Library.GetLibrary().FirstOrDefault(x => x.SongId == id);
            if (song != null && song.FilePath != null)
            {
                Response.CacheControl = "private";

                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", Path.GetFileName(song.FilePath).Replace(" ", "_")));

                return File(song.FilePath, @"audio/mp3");
            }

            return new EmptyResult();
        }
    }
}
