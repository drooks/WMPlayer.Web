using System.Collections.Generic;
using WMPLib;

namespace WMPlayer.Web.Models
{
    public class Playlist
    {
        public static IList<Song> GetPlaylist(WindowsMediaPlayer player)
        {
            IList<Song> result = new List<Song>();
            for (int i = 0; i < player.currentPlaylist.count; i++)
            {
                result.Add(new Song(player.currentPlaylist.Item[i]) { PlaylistIndex = i });
            }
            return result;
        }
    }
}