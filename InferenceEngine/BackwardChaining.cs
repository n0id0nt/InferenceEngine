using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class BackwardChaining
    {
        static public Result BC(KnowledgeBase knowledgeBase, string query)
        {
            return BCRecursive(knowledgeBase, query);
        }

        static private Result BCRecursive(KnowledgeBase knowledgeBase, string target)
        {
            List<Clause> statements = knowledgeBase.InConclusion(target);

            List<bool> results = new List<bool>();
            foreach (Clause c in statements)
            {
                bool result = true;
                foreach (string s in c.Premise)
                    result &= BCRecursive(knowledgeBase, s).Success;
                results.Add(result);
            }

            return new Result(results.All(r => r) && results.Count != 0);
        }
    }
}
