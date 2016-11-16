using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public class TextModelSource : IModelSource
    {
        public TextModelSource(string filePath)
        {
            FilePath = filePath;
            ReadHeader();
        }

        private void ReadHeader()
        {
            using (var fs = new FileStream(FilePath, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var headerLine = sr.ReadLine();
                    var parts = headerLine.Split(' ').Select(x => int.Parse(x.Trim())).ToArray();
                    Words = parts[0];
                    Size = parts[1];
                }
            }
        }

        public string FilePath { get; private set; }

        public int Size
        {
            get; private set;
        }

        public int Words
        {
            get; private set;
        }


        public IEnumerable<WordVector> GetVectors()
        {
            using (var fs = new FileStream(FilePath, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    sr.ReadLine(); //Discard header
                    string line;
                    while (null != (line = sr.ReadLine()))
                    {
                        var lineParts = line.Split(' ');
                        var word = lineParts[0];
                        var vector = lineParts.Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => float.Parse(x)).ToArray();
                        yield return new WordVector(word, vector);
                    }
                }
            }
        }
    }
}
