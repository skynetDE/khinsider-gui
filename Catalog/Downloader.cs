using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;

namespace khi.Catalog
{
    public class Downloader
    {
        private readonly string _targetFolder;
        private readonly IEnumerable<Album> _selected;

        public FileType PreferredFileType { get; set; }
        public decimal MaxRetries { get; set; }
        public OnFileExistsDelegate OnFileExists { get; set; }
        public OnExceptionDelegate OnException { get; set; }

        public delegate bool OnFileExistsDelegate(string path);

        public delegate void OnExceptionDelegate(Exception e);

        private readonly DownloadProgress _progress;
        private readonly Scraper _scraper;
        private readonly SongScraper _songScraper;
        private readonly HttpClient _httpClient;
        private readonly Progress<Utils.DownloadProgress> _progressHandler;

        public Downloader(string targetFolder, IEnumerable<Album> selected, IBrowsingContext browsingContext,
            HttpClient httpClient)
        {
            _songScraper = new SongScraper(browsingContext);
            _scraper = new Scraper(browsingContext);
            _targetFolder = targetFolder;
            _selected = selected;
            _progress = new DownloadProgress();
            _httpClient = httpClient;
            _progressHandler = new Progress<Utils.DownloadProgress>(progress =>
            {
                _progress.File.BytesReceived = progress.BytesReceived;
                _progress.File.TotalBytesToReceive = progress.TotalBytesToReceive;
                _progress.File.Current = (decimal) progress.Pct;
            });
        }

        public DownloadProgress GetProgress()
        {
            return _progress;
        }

        public async Task StartDownload(CancellationToken cancellationToken)
        {
            _progress.Total.Max = _selected.Count();
            if (cancellationToken.IsCancellationRequested) return;

            await ProcessAlbums(cancellationToken).ConfigureAwait(false);
        }

        private async Task ProcessAlbums(CancellationToken cancellationToken)
        {
            var albumDownloads = new List<AlbumDownload>();

            var current = 0;

            foreach (var album in _selected)
            {
                _progress.Album.Label = $"Current Album: {album.Name} - Scraping...";

                try
                {
                    var albumDownload =
                        await _scraper.Scrape(album.GetFullUrl(), cancellationToken).ConfigureAwait(false);
                    _progress.Total.Max += albumDownload.Songs.Count;
                    _progress.Total.Current = ++current;
                    _progress.AlbumDownload = albumDownload;
                    albumDownloads.Add(albumDownload);
                }
                catch (Exception e)
                {
                    OnException?.Invoke(new DownloaderExceptions.AlbumException("Unable to process album", e, album));
                }

                if (cancellationToken.IsCancellationRequested) return;
            }

            foreach (var albumDownload in albumDownloads)
            {
                try
                {
                    await ProcessAlbum(albumDownload, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    OnException?.Invoke(
                        new DownloaderExceptions.AlbumException("Unable to process album", e, albumDownload));
                }

                if (cancellationToken.IsCancellationRequested) return;
            }
        }

        private async Task ProcessAlbum(AlbumDownload albumDownload, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            _progress.AlbumDownload = albumDownload;
            _progress.Album.Max = albumDownload.Songs.Count;
            _progress.Album.Label = $"Current Album: {albumDownload.Title} - Downloading...";

            var targetPath = Path.Combine(_targetFolder, albumDownload.Title);
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            await ProcessSongs(albumDownload, targetPath, cancellationToken).ConfigureAwait(false);
        }

        private async Task ProcessSongs(AlbumDownload albumDownload, string targetPath,
            CancellationToken cancellationToken)
        {
            if (albumDownload.Covers.Count > 0)
            {
                foreach (var cover in albumDownload.Covers)
                {
                    await DoDownload(targetPath, cancellationToken, cover.Link);
                }
            }

            foreach (var song in albumDownload.Songs)
            {
                _progress.File.Current = 0;
                _progress.File.Label = $"Current File: {song.Name}";

                var retries = 0;
                while (retries <= MaxRetries)
                {
                    try
                    {
                        await ProcessSong(song, PrepareTargetPath(targetPath, song), cancellationToken)
                            .ConfigureAwait(false);
                        break;
                    }
                    catch (Exception e)
                    {
                        OnException?.Invoke(
                            new DownloaderExceptions.AlbumDownloadSongException("Unable to download song", e, song));
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    retries++;
                }

                _progress.Album.Current++;
                _progress.Total.Current++;
            }
        }

        private static string PrepareTargetPath(string targetPath, AlbumDownload.Song song)
        {
            if (song.Cd == null) return targetPath;

            var newTargetPath = Path.Combine(targetPath, $"CD{song.Cd}");

            if (!Directory.Exists(newTargetPath)) Directory.CreateDirectory(newTargetPath);

            return newTargetPath;
        }

        private async Task ProcessSong(AlbumDownload.Song song, string targetPath, CancellationToken cancellationToken)
        {
            var downloadLinks = await _songScraper
                .Scrape(KhiUrlBuilder.BuildDetailUrl(song), cancellationToken).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested) return;

            var usedDownloadLink = GetUsedDownloadLink(downloadLinks, PreferredFileType);

            if (usedDownloadLink != null)
            {
                await DoDownload(targetPath, cancellationToken, usedDownloadLink);
            }
        }

        private async Task DoDownload(string targetPath, CancellationToken cancellationToken, string url, string filenameOverride = null)
        {
            var uri = new Uri(url);
            var fileName = filenameOverride ?? Path.GetFileName(uri.LocalPath);
            var fullTargetPath = Path.Combine(targetPath, fileName);

            if (File.Exists(fullTargetPath))
            {
                if (OnFileExists == null || !OnFileExists(fullTargetPath)) return;

                File.Delete(fullTargetPath);
            }

            await Utils.DownloadFileAsync(_httpClient, uri, _progressHandler, cancellationToken, fullTargetPath)
                .ConfigureAwait(false);
        }

        private static string GetUsedDownloadLink(IReadOnlyDictionary<FileType, string> fileTypes,
            FileType preferredType)
        {
            return fileTypes.ContainsKey(preferredType)
                ? fileTypes[preferredType]
                : (from value in (FileType[]) Enum.GetValues(typeof(FileType))
                    where fileTypes.ContainsKey(value)
                    select fileTypes[value]).FirstOrDefault();
        }
    }
}