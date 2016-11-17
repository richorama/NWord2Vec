using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public class BinaryModelWriter : IModelWriter
    {
        public BinaryModelWriter(Stream s, bool leaveOpen = false)
        {
            Writer = new BinaryWriter(s, Encoding.UTF8, leaveOpen);
        }
        private BinaryWriter Writer { get; set; }

        public void Write(Model m)
        {
            WriteHeader(m);
            foreach (var wv in m.Vectors)
            {
                WriteWordVector(wv);
            }
        }

        private void WriteHeader(Model m)
        {
            WriteString(string.Format("{0} {1}\n", m.Words, m.Size));
        }

        private void WriteWordVector(WordVector wv)
        {
            WriteString(wv.Word);
            WriteString(" ");
            foreach (var f in wv.Vector)
            {
                Writer.Write(f);
            }
        }

        private void WriteString(string s)
        {
            var enc = new UTF8Encoding(false, true);
            Writer.Write(enc.GetBytes(s));
        }

        public void Dispose()
        {
            Writer.Dispose();
        }
    }
}
