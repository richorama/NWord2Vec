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
            IModelSource source = new ModelSourceFactory().Manufacture(filename);
            var m = new Model(source.Words, source.Size);
            foreach (var wv in source.GetVectors())
            {
                m.AddVector(wv);
            }
            return m;
        }
    }
}
