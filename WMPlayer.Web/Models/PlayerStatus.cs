using WMPLib;

namespace WMPlayer.Web.Models
{
    public class PlayerStatus
    {
        public PlayerStatus(WindowsMediaPlayer player)
        {
            if (player!=null && player.currentMedia != null)
            {
                CurrentPositionString = "";
                CurrentSong = new Song(player.currentMedia);
                DurationString = "";
                WmpPlayState = player.playState;
                VolumeLevel = player.settings.volume;

                if (player.currentMedia != null)
                {
                    CurrentPositionString = player.controls.currentPositionString;
                    DurationString = player.currentMedia.durationString;
                }
            }
        }

        public string CurrentPositionString { get; set; }
        public string DurationString { get; set; }
        public Song CurrentSong { get; set; }
        public WMPPlayState WmpPlayState { get; set; }
        public int VolumeLevel { get; set; }
    }
}