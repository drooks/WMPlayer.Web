using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WMPlayer.Web.Extensions;

namespace WMPlayer.Web.Models
{
    public class Library
    {
        public static IList<Song> RefreshLibrary()
        {
            // get indexed files
            //var indexedSongs = GetLibraryFromFile();

            // get songs from library path
            var songs = GetFilePaths(Settings.MediaPath).Select(x => new Song(x)).Where(x=>x.FilePath.EndsWith(".mp3")).ToList();

            // resolve library
            //foreach (var song in songs.Where(song => indexedSongs.Any(x=>x.FilePath == song.FilePath && x.SongId!= new Guid())))
            //{
            //    song.SongId = indexedSongs.First(x => x.FilePath == song.FilePath).SongId;
            //}
            
            // write json to file
            File.WriteAllText(Settings.LibraryFilePath, Serializer.ToJson(songs));

            return songs;
        }

        public static IList<Song> GetLibrary()
        {
            // retrieve json from file
            return GetLibraryFromFile();
        }

        public static Song GetSong(Guid songId)
        {
            return GetLibrary().FirstOrDefault(x => x.SongId == songId);
        }

        static IEnumerable<string> GetFilePaths(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    foreach (string t in files)
                    {
                        yield return t;
                    }
                }
            }
        }

        private static IList<Song> GetLibraryFromFile()
        {
            try
            {
                return Serializer.FromJson<IList<Song>>(File.ReadAllText(Settings.LibraryFilePath)).OrderBy(x=>x.Artist).ToList();
            }
            catch
            {
                return new List<Song>();
            }

        }
    }
}
