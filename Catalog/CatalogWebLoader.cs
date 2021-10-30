using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace khi.Catalog
{
    public class CatalogWebLoader
    {
        private static readonly char[] DefaultLetters = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly char[] _letters;
        private readonly IBrowsingContext _browsingContext;

        public CatalogWebLoader(IBrowsingContext browsingContext) : this(DefaultLetters, browsingContext)
        {
        }

        public CatalogWebLoader(char[] letters, IBrowsingContext browsingContext)
        {
            _letters = letters;
            _browsingContext = browsingContext;
        }

        public async Task<List<Album>> Load(LoadProgress loadProgress, CancellationToken cancellationToken)
        {
            var result = new List<Album>();

            for (var i = 0; i < _letters.Length; i++)
            {
                result.AddRange(
                    await LoadPage(KhiUrlBuilder.BuildAlbumBrowseUrl(_letters[i]), _browsingContext, cancellationToken)
                        .ConfigureAwait(false));
                loadProgress(_letters.Length, i + 1, $"Found {result.Count} albums", _letters[i]);
                if (cancellationToken.IsCancellationRequested) return null;
            }
            
            return result;
        }

        private static async Task<List<Album>> LoadPage(string url, IBrowsingContext browsingContext,
            CancellationToken cancellationToken)
        {
            var document = await browsingContext.OpenAsync(url, cancellationToken).ConfigureAwait(false);

            var albums = new List<Album>();
            if (cancellationToken.IsCancellationRequested) return albums;

            var content = document.QuerySelector($"#EchoTopic") ??
                          throw new PageLoadingException($"Unable to find content on {url}");

            var links = content.QuerySelectorAll("a");

            foreach (var link in links)
            {
                var href = link.Attributes.GetNamedItem("href")?.Value;
                var name = link.Text().Trim();
                if (href != null && href.Contains(KhiUrlBuilder.GameSoundtrackUrlPart) && !string.IsNullOrEmpty(name))
                {
                    albums.Add(new Album
                    {
                        Name = link.Text().Trim(),
                        Url = href.Substring(
                            href.IndexOf(KhiUrlBuilder.GameSoundtrackUrlPart, StringComparison.Ordinal) +
                            KhiUrlBuilder.GameSoundtrackUrlPart.Length + 1)
                    });
                }
            }

            return albums;
        }

        public async Task<List<Album>> LoadUpdates()
        {
            return await LoadPage(KhiUrlBuilder.UpdatesUrl, _browsingContext, CancellationToken.None).ConfigureAwait(false);
        }
    }

    public delegate void LoadProgress(int total, int current, string text, char currentLetter);
}