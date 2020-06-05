using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ForwardChaining
    {
        static public Result FC(KnowledgeBase knowledgeBase, string query)
        {
            if (!knowledgeBase.IsHornClause)
            {
                throw new Exception("KnowledgeBase must be Horn Clause");
            }
            // count <- a table where count[c] is the number of symbols in c's premis
            Dictionary<Sentence, int> count = new Dictionary<Sentence, int>();
            foreach (Sentence c in knowledgeBase.Sentences)
            {
                count.Add(c, c.Premise.Count);
            }

            // inferred <- a table, where inferred[s] is initially false for all symbols
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            foreach (string s in knowledgeBase.Symbols)
            {
                inferred.Add(s, false);
            }

            // agenda <- a queue of symbols, initially symbols known to be true in KB
            Queue<string> agenda = knowledgeBase.InitiallyTrue();

            List<string> symbolsEntailed = new List<string>();
            // while agenda is not empty
            while (agenda.Count != 0)
            {
                // p <- pop(agenda)
                string p = agenda.Dequeue();
                // store the symbols entailed during FC
                if (!symbolsEntailed.Contains(p))
                    symbolsEntailed.Add(p);
                // if p=q then return true
                if (p == query)
                    return new Result(true, symbols: symbolsEntailed);
                // if inferred[p] = false then
                if (!inferred[p])
                {
                    // inferred[p] <- true
                    inferred[p] = true;
                    // for each Sentence c in KB where p is in c.Premise do
                    foreach (Sentence c in knowledgeBase.InPremise(p))
                    {
                        // decrement count[c] by the items frequency in list
                        count[c] -= c.Premise.FindAll(i => i.Equals(p)).Count;
                        // if count[c] = 0 than add c.Conclusion to agenda
                        if (count[c] == 0)
                            agenda.Enqueue(c.Conclusion);     
                    }
                }
            }

            return new Result(false);
        }
    }
}
