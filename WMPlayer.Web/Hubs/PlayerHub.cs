using System;
using Microsoft.AspNet.SignalR;
using WMPlayer.Web.Models;

namespace WMPlayer.Web.Hubs
{
    public class PlayerHub : Hub
    {
        private readonly PlayerModel _playerModel;

        public PlayerHub() : this(PlayerModel.Instance) { }

        public PlayerHub(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void Init()
        {
            _playerModel.Init();
        }

        public void Play()
        {
            _playerModel.Play();
        }

        public void FastForward()
        {
            _playerModel.FastForward();
        }

        public void Skip()
        {
            _playerModel.Skip();
        }

        public void Stop()
        {
            _playerModel.Stop();
        }

        public void RefreshLibrary()
        {
            Clients.All.broadcastLibrary(Library.RefreshLibrary());
            Send("System","refreshed library");
        }

        public void QueueSong(Guid songId)
        {
            _playerModel.QueueSong(Library.GetSong(songId));
        }

        public void UnqueueSong(int playlistIndex)
        {
            _playerModel.UnqueueSong(playlistIndex);
        }

        public void ClearPlaylist()
        {
            _playerModel.ClearPlaylist();
        }

        public void SetVolume(int level)
        {
            _playerModel.SetVolume(level);
        }

        public void SetPosition(int position)
        {
            _playerModel.SetCurrentPosition(position);
        }

        public void Send(string name, string message)
        {
            Clients.All.broadcastNewMessage(name, message);
        }

        public void MoveUp(int playlistIndex)
        {
            _playerModel.MoveUp(playlistIndex);
        }

        public void MoveDown(int playlistIndex)
        {
            _playerModel.MoveDown(playlistIndex);
        }

        public void ShufflePlaylist()
        {
            _playerModel.ShufflePlaylist();
        }
    }
}