using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public class ModelSourceFactory
    {
        public IModelReader Manufacture(string filePath)
        {
            switch (System.IO.Path.GetExtension(filePath))
            {
                case ".txt":
                    return new TextModelReader(System.IO.File.OpenRead(filePath));
                case ".bin":
                    return new BinaryModelReader(System.IO.File.OpenRead(filePath));
                default:
                    var error = new InvalidOperationException("Unrecognized file type");
                    error.Data.Add("extension", System.IO.Path.GetExtension(filePath));
                    throw error;
            }
        }
    }
}
