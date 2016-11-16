using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public class BinaryModelReader : IModelReader
    {
        private const int MAX_SIZE = 50;

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
                Words = int.Parse(ReadString(br));
                Size = int.Parse(ReadString(br));
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
                    vector[j] = ReadFloat(br);
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

        private float ReadFloat(BinaryReader br)
        {
            byte[] bytes = new byte[4];
            br.Read(bytes, 0, 4);
            return BitConverter.ToSingle(bytes, 0);
        }


        private String ReadString(BinaryReader dis)
        {
            byte[] bytes = new byte[MAX_SIZE];
            byte b = dis.ReadByte();
            int i = -1;
            StringBuilder sb = new StringBuilder();
            while (b != 32 && b != 10)
            {
                i++;
                bytes[i] = b;
                b = dis.ReadByte();
                if (i == 49)
                {
                    sb.Append(Encoding.UTF8.GetString(bytes));
                    i = -1;
                    bytes = new byte[MAX_SIZE];
                }
            }
            sb.Append(Encoding.UTF8.GetString(bytes, 0, i + 1));
            return sb.ToString();
        }
    }
}
