using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace NWord2Vec
{
    public class TextModelReader : IModelReader
    {
        public TextModelReader(Stream stream, bool leaveOpen = false)
        {
            Reader = new StreamReader(stream, Encoding.UTF8, true, 4 * 1024, leaveOpen);
            LeaveOpen = leaveOpen;
            ReadHeader();
        }

        private void ReadHeader()
        {
            var headerLine = Reader.ReadLine();
            var parts = headerLine.Split(' ').Select(x => int.Parse(x.Trim(), CultureInfo.InvariantCulture)).ToArray();
            Words = parts[0];
            Size = parts[1];
        }

        public void Reset()
        {
            Reader.BaseStream.Seek(0, SeekOrigin.Begin);
            Reader.DiscardBufferedData();
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

        private bool LeaveOpen { get; set; }

        private StreamReader Reader { get; set; }

        public WordVector ReadVector()
        {
            var line = Reader.ReadLine();
            if (line == null)
                return null;
            var lineParts = line.Split(' ');
            var word = lineParts[0];
            var vector = lineParts.Skip(1).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            return new WordVector(word, vector);
        }

        public void Dispose()
        {
            Reader.Dispose();
        }
    }
}
