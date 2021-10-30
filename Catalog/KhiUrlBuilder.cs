namespace khi.Catalog
{
    public class KhiUrlBuilder
    {
        private const string BaseUrl = "https://downloads.khinsider.com";
        public static string UpdatesUrl => BaseUrl;
        public const string GameSoundtrackUrlPart = "game-soundtracks/album";

        public static string BuildAlbumUrl(Album album)
        {
            return $"{BaseUrl}/{GameSoundtrackUrlPart}/{album.Url}";
        }

        public static string BuildAlbumBrowseUrl(char letter)
        {
            return $"{BaseUrl}/game-soundtracks/browse/{letter}";
        }

        public static string BuildDetailUrl(AlbumDownload.Song song)
        {
            return $"{BaseUrl}/{song.DetailUrl}";
        }
    }
}