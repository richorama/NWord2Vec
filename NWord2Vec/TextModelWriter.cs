using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public class TextModelWriter : IModelWriter
    {
        public TextModelWriter(Stream stream, bool leaveOpen = false, int bufferSize = 4096)
        {
            Writer = new StreamWriter(stream, Encoding.UTF8, bufferSize, leaveOpen);
        }

        private StreamWriter Writer { get; set; }

        public void Write(Model m)
        {
            //Write the header
            WriteHeader(m);

            //Write the vectors
            foreach (var wv in m.Vectors)
            {
                WriteWordVector(wv);
            }
        }

        private void WriteWordVector(WordVector wv)
        {
            Writer.Write(wv.Word);
            Writer.Write(' ');
            Writer.Write(string.Join(" ", wv.Vector));
            Writer.Write('\n');
        }

        private void WriteHeader(Model m)
        {
            Writer.Write(m.Words);
            Writer.Write(' ');
            Writer.Write(m.Size);
            Writer.Write('\n');
        }

        public void Dispose()
        {
            Writer.Dispose();
        }
    }
}
