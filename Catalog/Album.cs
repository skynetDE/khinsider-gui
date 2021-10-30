using System.Collections.Generic;

namespace khi.Catalog
{
    public class Album
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string GetFullUrl()
        {
            return KhiUrlBuilder.BuildAlbumUrl(this);
        }

        private sealed class NameUrlEqualityComparer : IEqualityComparer<Album>
        {
            public bool Equals(Album x, Album y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Url == y.Url;
            }

            public int GetHashCode(Album obj)
            {
                return obj.Url != null ? obj.Url.GetHashCode() : 0;
            }
        }

        public static IEqualityComparer<Album> EqualityComparer { get; } = new NameUrlEqualityComparer();
    }
}