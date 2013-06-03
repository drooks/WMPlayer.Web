using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WMPLib;
using WMPlayer.Web.Hubs;

namespace WMPlayer.Web.Models
{
    public class PlayerModel : IDisposable
    {
        public static PlayerModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly static Lazy<PlayerModel> _instance = new Lazy<PlayerModel>(() => new PlayerModel(GlobalHost.ConnectionManager.GetHubContext<PlayerHub>().Clients, GlobalHost.ConnectionManager.GetHubContext<PlayerHub>().Clients.All));

        private readonly Timer _timer;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(500);

        private PlayerModel(IHubConnectionContext clients, dynamic caller)
        {
            Clients = clients;

            _player = new WindowsMediaPlayer();
            _player.PlayStateChange += PlayerPlayStateChange;
            _player.PlaylistChange += PlayerPlaylistChange;
            _player.MediaError += PlayerMediaError;
            _player.settings.volume = 100;

            _timer = new Timer(GetPlayerStatus, null, _updateInterval, _updateInterval);
        }

        public void Init()
        {
            Clients.All.broadcastLibrary(Library.GetLibrary());

            Clients.All.broadcastPlaylist(Playlist.GetPlaylist(_player));

            BroadcastPlayerStatus();
        }

        public void QueueSong(Song song)
        {
            //only queue unique
            for (int i = 0; i < _player.currentPlaylist.count; i++)
            {
                if (song.FilePath == _player.currentPlaylist.Item[i].sourceURL)
                {
                    return;
                }
            }
            _player.currentPlaylist.appendItem(_player.newMedia(song.FilePath));
        }

        public void Skip()
        {
            if (_player.currentMedia != null)
            {
                bool playing = _player.playState == WMPPlayState.wmppsPlaying;

                _player.currentPlaylist.removeItem(_player.currentMedia);

                if (playing)
                {
                    _player.controls.play();
                }
            }
        }

        public void Play()
        {
            if (_player.playState == WMPPlayState.wmppsPlaying)
            {
                _player.controls.pause();
            }
            else
            {
                _player.controls.play();
            }
        }

        public void FastForward()
        {
            _player.controls.fastForward();
        }

        public void Stop()
        {
            _player.controls.stop();
        }

        public void UnqueueSong(int playlistIndex)
        {
            _player.currentPlaylist.removeItem(_player.currentPlaylist.Item[playlistIndex]);
        }

        public void ClearPlaylist()
        {
            _player.currentPlaylist.clear();
            //IWMPPlaylist playlist = _player.currentPlaylist;
            //for (int i = 0; i < playlist.count; i++)
            //{
            //    if (i > 0)
            //    {
            //        _player.currentPlaylist.removeItem(playlist.Item[i]);
            //    }
            //}
        }

        public void SetVolume(int level)
        {
            if (level >= 0 && level <= 100)
            {
                _player.settings.volume = level;
                
            }
        }

        public void SetCurrentPosition(double position)
        {
            _player.controls.currentPosition = position;
        }

        public void MoveUp(int playlistIndex)
        {
            _player.currentPlaylist.moveItem(playlistIndex, playlistIndex-1);
        }

        public void MoveDown(int playlistIndex)
        {
            _player.currentPlaylist.moveItem(playlistIndex, playlistIndex + 1);
        }

        public void ShufflePlaylist()
        {
            IList<Song> playlist = Playlist.GetPlaylist(_player);
            int n = playlist.Count;
            if (n > 1)
            {
                Random rng = new Random();
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n - 1) + 2;
                    _player.currentPlaylist.moveItem(n, k);
                }
            }
        }

        private bool _songEnded = false;

        private void PlayerPlayStateChange(int newState)
        {
            if ((WMPPlayState)newState == WMPPlayState.wmppsStopped)
            {
                _player.close();
            }
            if ((WMPPlayState)newState == WMPPlayState.wmppsMediaEnded)
            {
                _songEnded = true;
            }
            if ((WMPPlayState)newState == WMPPlayState.wmppsPlaying)
            {
                if (_songEnded && _player.currentPlaylist.Item[0] != _player.currentMedia)
                {
                    _player.currentPlaylist.removeItem(_player.currentPlaylist.Item[0]);
                }
                _songEnded = false;
            }
        }

        private void PlayerPlaylistChange(object playlist, WMPPlaylistChangeEventType change)
        {
            Clients.All.broadcastPlaylist(Playlist.GetPlaylist(_player));
        }

        private void PlayerMediaError(object pMediaObject)
        {
            _player.close();
        }

        private IHubConnectionContext Clients { get; set; }

        private WindowsMediaPlayer _player;

        private void GetPlayerStatus(object state)
        {
            try
            {
                BroadcastPlayerStatus();
            }
            catch
            {
                
            }
        }

        private void BroadcastPlayerStatus()
        {
            Clients.All.broadcastPlayerStatus(new PlayerStatus(_player));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            if (_player != null)
            {
                Marshal.ReleaseComObject(_player);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}