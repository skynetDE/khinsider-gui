using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper.Configuration;

namespace khi.Catalog
{
    public class CatalogFileWriter
    {
        private readonly string _albumPath;
        public static readonly string Delimiter = "|";

        public CatalogFileWriter(string albumPath)
        {
            _albumPath = albumPath;
        }

        public void Write(IEnumerable<Album> albums)
        {
            if (File.Exists(_albumPath))
            {
                File.Delete(_albumPath);
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = Delimiter
            };

            using var sw = new CsvHelper.CsvWriter(new StreamWriter(_albumPath, false, Encoding.UTF8), config);
            sw.WriteRecords(albums);
        }
    }
}
