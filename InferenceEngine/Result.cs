using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Result
    {
        public bool Success { get; }

        public string Path { get; }

        public Result(bool success, string path = "")
        {
            Success = success;
            Path = path;
        }
    }
}
