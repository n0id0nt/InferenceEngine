using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Clause
    {
        public string Conclusion { get; set; }
        public List<string> Premise { get; set; }

        public Clause(string[] premise, string conclusion)
        {
            Premise = new List<string>();

            if (premise is string[])
                foreach (string l in premise)
                    Premise.Add(l);

            Conclusion = conclusion;
        }
    }
}
