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
            return Load(new ModelReaderFactory().Manufacture(filename));
        }

        public static Model Load(IModelReader source)
        {
            var m = new Model(source.Words, source.Size);
            WordVector wv;
            while (null != (wv = source.ReadVector()))
            {
                m.AddVector(wv);
                if (wv.Word == "ruble_firmed")
                {
                    Console.WriteLine("Woo!");
                }
            }
            return m;
        }
    }
}
