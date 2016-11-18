using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public interface IModelReader : IDisposable
    {
        /// <summary>
        /// Word value in the header
        /// </summary>
        /// <remarks>NOT checked against actual word count</remarks>
        int Words { get; }
        /// <summary>
        /// Size value in the header
        /// </summary>
        /// <remarks>NOT checked against actual vector size</remarks>
        int Size { get; }
        /// <summary>
        /// Read a single vector, or null if there are no more
        /// </summary>
        /// <returns></returns>
        WordVector ReadVector();
        /// <summary>
        /// Reset to the beginning of the stream of vectors
        /// </summary>
        void Reset();
    }
}
