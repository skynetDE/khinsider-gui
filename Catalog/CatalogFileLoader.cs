using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;

namespace khi.Catalog
{
    public class CatalogFileLoader
    {
        private readonly string _albumPath;
        private readonly string _delimiter;

        public CatalogFileLoader(string albumPath, string delimiter)
        {
            _albumPath = albumPath;
            _delimiter = delimiter;
        }

        public IEnumerable<Album> Load()
        {
            if (!File.Exists(_albumPath))
            {
                return new List<Album>();
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _delimiter
            };

            using var sw = new CsvHelper.CsvReader(new StreamReader(_albumPath, Encoding.UTF8), config);

            return sw.GetRecords<Album>()
                .ToList();
        }
    }
}
