using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WMPlayer.Web.Models;

namespace WMPlayer.Web.Controllers
{
    [OutputCache(Duration = 0)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSong(Guid id)
        {
            Song song = Library.GetLibrary().FirstOrDefault(x=>x.SongId == id);
            if(song!=null && song.FilePath!=null)
            {
                Response.CacheControl = "private";

                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", Path.GetFileName(song.FilePath).Replace(" ","_")));
                
                return File(song.FilePath, @"audio/mp3");
            }

            return new EmptyResult();
        }
    }
}
