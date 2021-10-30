using System;
using System.Collections.Generic;

namespace khi.Catalog
{
    public class AlbumDownload
    {
        public class Song
        {
            public string Cd { get; internal set; }
            public string No { get; internal set; }
            public string Name { get; internal set; }
            public string Length { get; internal set; }
            public string DetailUrl { get; internal set; }
        }

        public class Cover
        {
            public string Thumb { get; internal set; }
            public string Link { get; internal set; }
        }

        public string Title { get; internal set; }

        public List<Cover> Covers = new List<Cover>();

        public List<Song> Songs = new List<Song>();
    }
}