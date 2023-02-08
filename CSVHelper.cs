using System.Globalization;
using System.IO.Compression;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

public class CSVHelper
{
    static CsvConfiguration CsvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
        Delimiter = "\t"
    };
    public static IEnumerable<T> ReadCompressedCSV<T>(string path)
    {

        using (FileStream originalFileStream = new FileInfo(path).OpenRead())
        {
            using (MemoryStream decompressedFileStream = new MemoryStream())
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(decompressionStream))
                    using (var csv = new CsvReader(reader, CsvConfiguration))
                    {
                        var records = csv.GetRecords<T>();
                        return records.ToList();
                    }
                }
            }
        }
    }
    public static IEnumerable<T> ReadCSV<T>(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CsvConfiguration))
        {
            var records = csv.GetRecords<T>();
            return records.ToList();
        }
    }

    public static void SaveAsCSV<T>(string path, List<T> contributions)
    {
        using (var writer = new StreamWriter(path))
        using (var csvWriter = new CsvWriter(writer, CsvConfiguration))
        {
            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
            csvWriter.WriteRecords(contributions);

            writer.Flush();
        }
    }
}