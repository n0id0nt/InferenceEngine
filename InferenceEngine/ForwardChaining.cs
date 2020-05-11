using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class ForwardChaining
    {
        static public bool FC(KnowledgeBase knowledgeBase, string query)
        {
            // count <- a table where count[c] is the number of symbols in c's premis
            Dictionary<Clause, int> count = new Dictionary<Clause, int>();
            foreach (Clause c in knowledgeBase.Sentences)
            {
                count.Add(c, c.Premise.Count);
            }

            // inferred <- a table, where inferred[s] is initiall false for all symbols
            Dictionary<string, bool> inferred = new Dictionary<string, bool>();
            foreach (string s in knowledgeBase.Symbols)
            {
                inferred.Add(s, false);
            }

            // agenda <- a queue of sumbols, initially symbols known to be true in KB
            Queue<string> agenda = knowledgeBase.InitiallyTrue();

            // while agenda is not empty
            while (agenda.Count != 0)
            {
                // p <- pop(agenda)
                string p = agenda.Dequeue();
                // if p=q then return true
                if (p == query)
                    return true;
                // if inferred[p] = false then
                if (!inferred[p])
                {
                    // inferred[p] <- true
                    inferred[p] = true;
                    // for each clause c in KB where p is in c.Premise do
                    foreach (Clause c in knowledgeBase.InPremise(p))
                    {
                        // decrement count[c]
                        count[c]--;
                        // if count[c] = 0 than add c.Conclusion to agenda
                        if (count[c] == 0)
                            agenda.Enqueue(c.Conclusion);     
                    }
                }
            }

            return false;
        }
    }
}
