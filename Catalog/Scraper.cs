using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace khi.Catalog
{
    internal class Scraper
    {
        private readonly IBrowsingContext _browsingContext;

        public Scraper(IBrowsingContext browsingContext)
        {
            _browsingContext = browsingContext;
        }

        public async Task<AlbumDownload> Scrape(string url, CancellationToken cancellationToken)
        {
            var document = await _browsingContext.OpenAsync(url, cancellationToken).ConfigureAwait(false);

            var content = document.QuerySelector("#EchoTopic") ??
                          throw new PageLoadingException($"Unable to find content on {url}");

            var header = content.QuerySelector("h2") ??
                         throw new PageLoadingException($"Unable to find title on {url}");

            var songList = content.QuerySelector("#songlist") ??
                           throw new PageLoadingException($"Unable to find song list at {url}");

            var coverThumbs = content.QuerySelector("table")?.QuerySelectorAll("a > img");

            var idxCd = -1;
            var idxNo = -1;
            var idxSongName = -1;

            var headers = songList.QuerySelectorAll("#songlist_header > th");

            for (var i = 0; i < headers.Length; i++)
            {
                var th = headers[i];
                switch (th.Text())
                {
                    case "CD":
                        idxCd = i;
                        break;
                    case "#":
                        idxNo = i;
                        break;
                    case "Song Name":
                        idxSongName = i;
                        break;
                }
            }

            var idxLength = idxSongName + 1;
            var idxDetailUrl = headers.Length - 1;

            if (idxSongName == -1) throw new PageLoadingException($"Unable to find Song Name element on page at {url}");

            var result = new AlbumDownload
            {
                Title = WebUtility.HtmlDecode(header.InnerHtml)
            };

            if (coverThumbs?.Length > 0)
            {
                foreach (var coverThumb in coverThumbs)
                {
                    var coverLink = coverThumb.ParentElement?.Attributes.GetNamedItem("href")?.Value;
                    var coverImg = coverThumb.Attributes.GetNamedItem("src")?.Value;

                    if (coverImg != null && coverLink != null)
                    {
                        result.Covers.Add(new AlbumDownload.Cover
                        {
                            Link = coverLink,
                            Thumb = coverImg
                        });
                    }
                }
            }

            foreach (var tr in songList.QuerySelectorAll("tr"))
            {
                if (tr.Id == "songlist_footer" || tr.Id == "songlist_header") continue;

                var tds = tr.QuerySelectorAll("td");

                result.Songs.Add(new AlbumDownload.Song
                {
                    No = idxNo > -1 ? tds[idxNo].Text() : null,
                    Cd = idxCd > -1 ? tds[idxCd].Text() : null,
                    DetailUrl = tds[idxDetailUrl].QuerySelector("a")?.Attributes.GetNamedItem("href")?.Value,
                    Length = tds[idxLength].Text(),
                    Name = tds[idxSongName].Text()
                });
            }


            return result;
        }
    }
}