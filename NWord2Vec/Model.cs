using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public class Model
    {
        public int Words { get; private set; }
        public int Size { get; private set; }
        List<WordVector> vectors;

        public IEnumerable<WordVector> Vectors
        {
            get
            {
                return this.vectors;
            }
        }

        protected void AddVector(WordVector vector)
        {
            this.vectors.Add(vector);
        }

        public Model(int words, int size)
        {
            this.Words = words;
            this.Size = size;
            this.vectors = new List<WordVector>();
        }

        public static Model Load(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open))
            {
                return Load(stream);
            }
        }

        public static Model Load(Stream stream)
        {
            var first = true;
            Model model = null;
            foreach (var line in Parse(stream))
            {
                if (first)
                {
                    var parts = line.Split(' ').Select(x => int.Parse(x.Trim())).ToArray();
                    model = new Model(parts[0], parts[1]);
                    first = false;
                    continue;
                }

                var lineParts = line.Split(' ');
                var word = lineParts[0];
                var vector = lineParts.Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => float.Parse(x)).ToArray();
                model.AddVector(new WordVector(word, vector));
            }
            return model;
        }

        static IEnumerable<string> Parse(Stream stream)
        {
            var reader = new StreamReader(stream);
            string line = null;
            while (null != (line = reader.ReadLine()))
            {
                yield return line;
            }
        }


    }
}
