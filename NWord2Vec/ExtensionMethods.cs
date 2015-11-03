using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public static class ExtensionMethods
    {
        public static float[] Add(this float[] value1, float[] value2)
        {
            if (value1 == null) throw new ArgumentNullException("value1");
            if (value2 == null) throw new ArgumentNullException("value2");
            if (value1.Length != value2.Length) throw new ArgumentException("vector lengths do not match");

            var result = new float[value1.Length];
            for (var i = 0; i < value1.Length; i++)
            {
                result[i] = value1[i] + value2[i];
            }
            return result;
        }

        public static float[] Subtract(this float[] value1, float[] value2)
        {
            if (value1 == null) throw new ArgumentNullException("value1");
            if (value2 == null) throw new ArgumentNullException("value2");
            if (value1.Length != value2.Length) throw new ArgumentException("vector lengths do not match");

            var result = new float[value1.Length];
            for (var i = 0; i < value1.Length; i++)
            {
                result[i] = value1[i] - value2[i];
            }
            return result;
        }

        public static double Distance(this float[] value1, float[] value2)
        {
            if (value1 == null) throw new ArgumentNullException("value1");
            if (value2 == null) throw new ArgumentNullException("value2");
            if (value1.Length != value2.Length) throw new ArgumentException("vector lengths do not match");

            return Math.Sqrt(value1.Subtract(value2).Select(x => x * x).Sum());
        }


        public static WordVector GetByWord(this Model model, string word)
        {
            return model.Vectors.FirstOrDefault(x => x.Word == word);
        }

        public static IEnumerable<WordVector> Nearest(this Model model, float[] vector)
        {
            return model.Vectors.OrderBy(x => x.Vector.Distance(vector));
        }

        public static WordVector NearestSingle(this Model model, float[] vector)
        {
            return model.Vectors.OrderBy(x => x.Vector.Distance(vector)).First();
        }

        public static double Distance(this Model model, string word1, string word2)
        {
            var vector1 = model.GetByWord(word1);
            var vector2 = model.GetByWord(word2);
            if (vector1 == null) throw new ArgumentException(string.Format("cannot find word1 '{0}'", word1));
            if (vector2 == null) throw new ArgumentException(string.Format("cannot find word2 '{0}'", word2));
            return vector1.Vector.Distance(vector2.Vector);
        }

        public static IEnumerable<WordDistance> Nearest(this Model model, string word)
        {
            var vector = model.GetByWord(word);
            if (vector == null) throw new ArgumentException(string.Format("cannot find word '{0}'", word));
            return model.Vectors.Select(x => new WordDistance(x.Word, x.Vector.Distance(vector.Vector))).OrderBy(x => x.Distance).Where(x => x.Word != word);
        }

        public static double Distance(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Distance(word2.Vector);
        }

        public static float[] Add(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Add(word2.Vector);
        }

        public static float[] Subtract(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Subtract(word2.Vector);
        }

        public static float[] Add(this float[] word1, WordVector word2)
        {
            return word1.Add(word2.Vector);
        }

        public static float[] Subtract(this float[] word1, WordVector word2)
        {
            return word1.Subtract(word2.Vector);
        }

        public static double Distance(this float[] word1, WordVector word2)
        {
            return word1.Distance(word2.Vector);
        }

    }
}
