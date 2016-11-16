using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public interface IModelSource
    {
        int Words { get; }
        int Size { get; }

        IEnumerable<WordVector> GetVectors();
    }
}
