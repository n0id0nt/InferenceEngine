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
                throw new Exception("KnowledgeBase must be Horn Clause");
            }
            List<string> symbolsEnailed = new List<string>();

            short result = BCRecursive(knowledgeBase, query, new List<Sentence>(), ref symbolsEnailed);

            if (result == 1)
                return new Result(true, symbols: symbolsEnailed);
            else
                return new Result(false);
        }

        static private short BCRecursive(KnowledgeBase knowledgeBase, string target, List<Sentence> sentencesChecked, ref List<string> symbolsEntailed)
        {
            List<Sentence> statements = knowledgeBase.InConclusion(target);
            short results = 0;
            foreach (Sentence c in statements)
            {
                if (!sentencesChecked.Contains(c))
                {
                    sentencesChecked.Add(c);
                    short result = 1;
                    foreach (string s in c.Premise)
                    {
                        List<Sentence> sentencesCheckedCopy = new List<Sentence>(sentencesChecked);
                        short r = BCRecursive(knowledgeBase, s, sentencesCheckedCopy, ref symbolsEntailed);

                        result = r;

                        if (r != 1)
                        {
                            break; // can break out of loop cause all statements must be true
                        }
                    }

                    if (!symbolsEntailed.Contains(c.Conclusion))
                        symbolsEntailed.Add(c.Conclusion);

                    if (result != 0)
                        results = result;
                    else
                        break;
                }
                else if (results == 0) results = -1; // to indicate overflow on branch so no inference can be drawn from it
            }

            return results;
        }
    }
}
