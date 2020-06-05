using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace InferenceEngine
{
    class TruthTableChecking
    {
        static public Result TT(KnowledgeBase knowledgeBase, string query)
        {
            Result result = TTCheckAll(knowledgeBase, query, knowledgeBase.Symbols, new Dictionary<string, bool> { { "", true } });
            if (result.Count == 0)
                result.Success = false;
            return result;
        }

        static private Result TTCheckAll(KnowledgeBase knowledgeBase, string query, List<string> symbols, Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PLTrue(knowledgeBase, model))
                {
                    return new Result((model.Keys.Contains(query))? model[query]: false, count: 1);
                }
                else
                {
                    return new Result(true);
                }
            }
            else
            {
                List<string> symbolsCopy = symbols.ConvertAll(symbol => String.Copy(symbol));
                string first = symbolsCopy[0];
                symbolsCopy.RemoveAt(0);

                Dictionary<string, bool> t = new Dictionary<string, bool>(model);
                Dictionary<string, bool> f = new Dictionary<string, bool>(model);

                t.Add(first, true);
                f.Add(first, false);

                Result r1 = TTCheckAll(knowledgeBase, query, symbolsCopy, t);
                Result r2 = TTCheckAll(knowledgeBase, query, symbolsCopy, f);
                return new Result(r1.Success && r2.Success, count: r1.Count + r2.Count);
            }
        }

        static private bool PLTrue(KnowledgeBase knowledgeBase, Dictionary<string, bool> model)
        {
            
            foreach(Sentence Sentence in knowledgeBase.Sentences)
            {
                List<bool> bSymbols = Sentence.Symbols.ConvertAll(s => model[s]);
                List<string> logicalSymbols = new List<string>(Sentence.LogicalConnectives);
                    
                // loops through Symbols in the order of operation
                foreach (string s in LogicalConnectives.SymbolsOperationOrder)
                {
                    // set the firts occurance of the symbol to i
                    int i;
                    // will check from back to front for not statements to handel double negetives
                    while ((i = (s == "~")?logicalSymbols.LastIndexOf(s): logicalSymbols.IndexOf(s)) != -1) 
                    {
                        bSymbols[i] = LogicalConnectives.Evaluate(s, bSymbols[i], bSymbols[i + 1]);

                        // remove evaluated symbols
                        bSymbols.RemoveAt(i + 1);
                        logicalSymbols.RemoveAt(i);
                    }
                }

                Debug.Assert(bSymbols.Count == 1);

                if (!bSymbols[0])
                    return false;
            }
            return true;
        }
    }
}
