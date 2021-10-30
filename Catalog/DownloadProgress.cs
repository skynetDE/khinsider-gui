namespace khi.Catalog
{
    public class DownloadProgress
    {
        public DownloadProgress()
        {
            Total = new Stats("Total");
            Album = new Stats("Current Album");
            File = new FileStats("Current File");
        }

        public Stats Total { get; }
        public Stats Album { get; }
        public FileStats File { get; }
        public AlbumDownload AlbumDownload { get; set; }

        public class Stats
        {
            private int _max;

            public Stats(string label)
            {
                Label = label;
            }

            public int Max
            {
                get => _max;
                internal set
                {
                    _max = value;
                    Current = 0;
                }
            }

            public decimal Current { get; internal set; }
            public string Label { get; internal set; }
        }

        public class FileStats : Stats
        {
            public FileStats(string label) : base(label)
            {
            }

            public long BytesReceived { get; set; }
            public long TotalBytesToReceive { get; set; }
        }
    }
}