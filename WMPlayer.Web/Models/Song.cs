using System;
using WMPLib;

namespace WMPlayer.Web.Models
{
    [Serializable]
    public class Song
    {
        public Song(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                WindowsMediaPlayer player = new WindowsMediaPlayer();
                Init(player.newMedia(filePath));
            }
        }

        public Song(IWMPMedia wmpMedia)
        {
            Init(wmpMedia);
        }

        private void Init(IWMPMedia wmpMedia)
        {
            if (wmpMedia != null)
            {
                SongId = Guid.NewGuid();
                TagId = wmpMedia.getItemInfo("TrackingID");
                Title = wmpMedia.getItemInfo("Title");
                Artist = wmpMedia.getItemInfo("Author");
                Album = wmpMedia.getItemInfo("AlbumID");
                Year = wmpMedia.getItemInfo("ReleaseDateYear");
                Genre = wmpMedia.getItemInfo("WM/Genre");
                FilePath = wmpMedia.getItemInfo("SourceURL");
                Duration = wmpMedia.durationString;
            }

        }

        public Guid SongId { get; set; }
        public int PlaylistIndex { get; set; }
        public string TagId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
        public string FilePath { get; set; }
        public string Duration { get; set; }
    }

}
