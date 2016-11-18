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
        public IModelReader Manufacture(string filePath)
        {
            Stream fileStream = File.OpenRead(filePath);
            var ext = Path.GetExtension(filePath);
            if ( ext == ".gz")
            {
                fileStream = new GZipStream(fileStream, CompressionMode.Decompress);
                ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath));
            }

            switch (ext)
            {
                case ".txt":
                    return new TextModelReader(fileStream);
                case ".bin":
                    return new BinaryModelReader(fileStream);
                default:
                    var error = new InvalidOperationException("Unrecognized file type");
                    error.Data.Add("extension", System.IO.Path.GetExtension(filePath));
                    throw error;
            }
        }
    }
}
