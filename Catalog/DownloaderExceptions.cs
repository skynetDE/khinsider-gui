using System;

namespace khi.Catalog
{
    public class DownloaderExceptions
    {
        public class AlbumException : Exception
        {
            public string Album { get; }

            public AlbumException(string message, Exception innerException, Album album) : base(message, innerException)
            {
                Album = album.Name;
            }

            public AlbumException(string message, Exception innerException, AlbumDownload albumDownload) : base(message,
                innerException)
            {
                Album = albumDownload.Title;
            }
        }

        public class AlbumDownloadSongException : Exception
        {
            public AlbumDownload.Song Song { get; }

            public AlbumDownloadSongException(string message, Exception innerException, AlbumDownload.Song song) : base(
                message, innerException)
            {
                Song = song;
            }
        }
    }
}