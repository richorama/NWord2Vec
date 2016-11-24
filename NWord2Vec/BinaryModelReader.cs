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
            Stream = stream;
            LeaveOpen = leaveOpen;
            ReadHeader();
        }

        private void ReadHeader()
        {
            using (var br = GetReader())
            {
                Words = int.Parse(ReadString(br), CultureInfo.InvariantCulture);
                Size = int.Parse(ReadString(br), CultureInfo.InvariantCulture);
            }
        }

        private BinaryReader GetReader()
        {
            return new BinaryReader(Stream, Encoding.UTF8, true);
        }

        public bool LineBreaks { get; private set; }

        public void Reset()
        {
            Stream.Seek(0, SeekOrigin.Begin);
            CurrentIndex = 0;
            ReadHeader();
        }

        public int Size
        {
            get; private set;
        }

        public int Words
        {
            get; private set;
        }

        private Stream Stream { get; set; }
        private bool LeaveOpen { get; set; }
        private int CurrentIndex { get; set; }

        public WordVector ReadVector()
        {
            if (CurrentIndex == Words)
                return null;
            WordVector result = null;
            using (var br = GetReader())
            {
                var word = ReadString(br);

                float[] vector = new float[Size];

                for (int j = 0; j < Size; j++)
                {
                    vector[j] = br.ReadSingle();
                }

                result = new WordVector(word, vector);

                if (LineBreaks)
                {
                    br.ReadByte(); // consume line break
                }
            }
            CurrentIndex++;
            return result;
        }

        public void Dispose()
        {
            if (!LeaveOpen)
            {
                Stream.Dispose();
            }
        }

        private static bool IsStringEnd(char[] c)
        {
            return c == null || c[0] == 32 || c[0] == 10;
        }

        private static char[] ReadUTF8Char(Stream s)
        {
            byte[] bytes = new byte[4];
            var enc = new UTF8Encoding(false, true);
            if (1 != s.Read(bytes, 0, 1))
                return null;
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
                if (remainingBytes == -1)
                    return null;
                s.Read(bytes, 1, remainingBytes);
                return enc.GetChars(bytes, 0, remainingBytes + 1);
            }
        }

        private String ReadString(BinaryReader br)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = null;
            while (!IsStringEnd((c = ReadUTF8Char(br.BaseStream))))
                sb.Append(c);
            return sb.ToString();
        }
    }
}
