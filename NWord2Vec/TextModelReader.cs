using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NWord2Vec
{
    public class TextModelReader : IModelReader
    {
        Stream Stream;

        public TextModelReader(Stream stream)
        {
            this.Stream = stream;
        }

        public Model Open()
        {
            using (var reader = new StreamReader(this.Stream, Encoding.UTF8, true, 4 * 1024))
            {
                var header = ReadHeader(reader);
                var words = header[0];
                var size = header[1];

                var vectors = new List<WordVector>();
                WordVector vector = null;
                while (null != (vector = ReadVector(reader)))
                {
                    vectors.Add(vector);
                }
                return new Model(words, size, vectors);
            }
        }

        int[] ReadHeader(StreamReader reader)
        {
            var headerLine = reader.ReadLine();
            return headerLine.Split(' ').Select(x => int.Parse(x.Trim(), CultureInfo.InvariantCulture)).ToArray();
        }

        WordVector ReadVector(StreamReader reader)
        {
            var line = reader.ReadLine();
            if (line == null) return null;
            var lineParts = line.Split(' ');
            var word = lineParts[0];
            var vector = lineParts.Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            return new WordVector(word, vector);
        }

    }
}
