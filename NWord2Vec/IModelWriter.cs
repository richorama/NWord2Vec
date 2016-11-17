using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NWord2Vec
{
    public interface IModelWriter : IDisposable
    {
        void Write(Model m);
    }
}
