using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Result
    {
        public bool Success { get; set; }

        public List<string> Symbols { get; }

        public int Count { get; }

        public Result(bool success, List<string> symbols = null, int count = 0)
        {
            Success = success;
            Symbols = symbols;
            Count = count;
        }
    }
}
