using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public class TextModelSerializer
    {
        public TextModelSerializer(Model model)
        {
            Model = model;
        }
        private Model Model { get; set; }

        public void Write(StreamWriter sw)
        {
            sw.Write(Model.Words);
            sw.Write(' ');
            sw.Write(Model.Size);
            sw.Write('\n');
            foreach ( var wv in Model.Vectors)
            {
                sw.Write(wv.Word);
                sw.Write(' ');
                sw.Write(string.Join(" ", wv.Vector));
                sw.Write('\n');
            }
        }
    }
}
