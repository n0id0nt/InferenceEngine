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
            if (!knowledgeBase.IsHornClause)
            {
                throw new Exception("KnowledgeBase must be Horn clause");
            }
            return BCRecursive(knowledgeBase, query);
        }

        static private Result BCRecursive(KnowledgeBase knowledgeBase, string target)
        {
            List<Clause> statements = knowledgeBase.InConclusion(target);
            List<string> symbolsEntailed = new List<string>();
            List<bool> results = new List<bool>();
            foreach (Clause c in statements)
            {
                bool result = true;
                foreach (string s in c.Premise)
                {
                    Result r = BCRecursive(knowledgeBase, s);
                    result &= r.Success;
                    if (r.Success)
                        symbolsEntailed = r.Symbols;
                }
                results.Add(result);
                if (!symbolsEntailed.Contains(c.Conclusion))
                    symbolsEntailed.Add(c.Conclusion);
            }

            return new Result(results.All(r => r) && results.Count != 0, symbols: symbolsEntailed);
        }
    }
}
