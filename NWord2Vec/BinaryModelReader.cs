using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace NWord2Vec
{
    public class BinaryModelReader : IModelReader
    {
        public BinaryModelReader(Stream stream, bool lineBreaks = false, bool leaveOpen = false)
        {
            this.Stream = stream;
        }

        public Model Open()
        {
            using (var reader = new BinaryReader(Stream, Encoding.UTF8, true))
            {
                var header = ReadHeader(reader);
                var words = header[0];
                var size = header[1];

                var vectors = new List<WordVector>();
                for (var i = 0; i < words; i++)
                {
                    vectors.Add(ReadVector(reader, words, size));
                }

                return new Model(words, size, vectors);
            }

        }


        int[] ReadHeader(BinaryReader reader)
        {
            var words = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);
            var size = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);

            return new int[] { words, size };
        }

        public bool LineBreaks { get; private set; }

        Stream Stream { get; set; }

        public WordVector ReadVector(BinaryReader br, int words, int size)
        {
            var word = ReadString(br);

            var vector = new float[size];

            for (int j = 0; j < size; j++)
            {
                vector[j] = br.ReadSingle();
            }

            var result = new WordVector(word, vector);

            if (LineBreaks) br.ReadByte(); // consume line break
       
            return result;
        }

        private static bool IsStringEnd(char[] c)
        {
            return c == null || c[0] == 32 || c[0] == 10;
        }

        private static char[] ReadUTF8Char(Stream s)
        {
            byte[] bytes = new byte[4];
            var enc = new UTF8Encoding(false, true);
            if (1 != s.Read(bytes, 0, 1)) return null;
            if (bytes[0] <= 0x7F) //Single byte character
            {
                return enc.GetChars(bytes, 0, 1);
            }
            else
            {
                var remainingBytes =
                    ((bytes[0] & 240) == 240) ? 3 : (
                    ((bytes[0] & 224) == 224) ? 2 : (
                    ((bytes[0] & 192) == 192) ? 1 : -1
                ));
                if (remainingBytes == -1) return null;
                s.Read(bytes, 1, remainingBytes);
                return enc.GetChars(bytes, 0, remainingBytes + 1);
            }
        }

        string ReadString(BinaryReader br)
        {
            var sb = new StringBuilder();
            char[] c = null;
            while (!IsStringEnd((c = ReadUTF8Char(br.BaseStream))))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
