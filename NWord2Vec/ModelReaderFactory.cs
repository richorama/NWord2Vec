using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace NWord2Vec
{
    public class ModelReaderFactory
    {
        Stream OpenStream(string filePath)
        {
            var fileStream = File.OpenRead(filePath);
            if (Path.GetExtension(filePath).ToLower() == ".gz") return new GZipStream(fileStream, CompressionMode.Decompress);
            return fileStream;
        }

        IModelReader GetReader(Stream stream, string fileExtension)
        {
            switch (fileExtension)
            {
                case ".txt":
                    return new TextModelReader(stream);
                case ".bin":
                    return new BinaryModelReader(stream);
                default:
                    var error = new InvalidOperationException("Unrecognized file type");
                    error.Data.Add("extension", fileExtension);
                    throw error;
            }
        }

        public Model Manufacture(string filePath)
        {
            using (var fileStream = OpenStream(filePath))
            {
                var ext = Path.GetExtension(filePath);
                if (ext == ".gz")
                {
                    ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath));
                }

                var reader = GetReader(fileStream, ext.ToLower());

                return reader.Open();
            }
        }
    }
}
