using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace khi.Catalog
{
    internal class SongScraper
    {
        private readonly IBrowsingContext _browsingContext;

        public SongScraper(IBrowsingContext browsingContext)
        {
            _browsingContext = browsingContext;
        }

        public async Task<Dictionary<FileType, string>> Scrape(string url, CancellationToken cancellationToken)
        {
            var document = await _browsingContext.OpenAsync(url, cancellationToken).ConfigureAwait(false);
            var result = new Dictionary<FileType, string>();

            var links = document.QuerySelectorAll("span.songDownloadLink");
            foreach (var link in links)
            {
                var downloadLink = link.ParentElement?.Attributes.GetNamedItem("href")?.Value;
                var text = link.Text();

                if (text.EndsWith("MP3"))
                    result.Add(FileType.MP3, downloadLink);
                else if (text.EndsWith("FLAC"))
                    result.Add(FileType.FLAC, downloadLink);
                else if (text.EndsWith("M4A"))
                    result.Add(FileType.M4A, downloadLink);
            }

            return result;
        }
    }
}