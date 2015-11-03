using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWord2Vec
{
    public struct WordDistance
    {
        public WordDistance(string word, double distance)
        {
            this.Word = word;
            this.Distance = distance;
        }

        public string Word { get; private set; }
        public double Distance { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Word, this.Distance);
        }
    }
}
