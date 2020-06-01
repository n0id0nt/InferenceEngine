using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class TruthTableChecking
    {
        static public Result TT(KnowledgeBase knowledgeBase, string query)
        {
            return TTCheckAll(knowledgeBase, query, knowledgeBase.Symbols, new Dictionary<string, bool> { { "", false } });
        }

        static private Result TTCheckAll(KnowledgeBase knowledgeBase, string query, List<string> symbols, Dictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PLTrue(knowledgeBase, model))
                {
                    bool result = PLTrue(new KnowledgeBase(query), model);
                    return new Result(result, count: 1);
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

                Dictionary<string, bool> t = model.ToDictionary(m => m.Key, m => m.Value);
                Dictionary<string, bool> f = model.ToDictionary(m => m.Key, m => m.Value);

                t.Add(first, true);
                f.Add(first, false);

                Result r1 = TTCheckAll(knowledgeBase, query, symbolsCopy, t);
                Result r2 = TTCheckAll(knowledgeBase, query, symbolsCopy, f);
                return new Result(r1.Success && r2.Success, count: r1.Count + r2.Count);
            }
        }

        static private bool PLTrue(KnowledgeBase knowledgeBase, Dictionary<string, bool> model)
        {
            bool result = true;
            foreach(Clause clause in knowledgeBase.Sentences)
            {
                bool innerResult = model[clause.Symbols[0]];

                for (int i = 0; i < clause.Symbols.Count - 1; i++)
                {
                    innerResult = Evaluate(ref i, innerResult, clause, model);
                }

                result &= innerResult;
            }
            return result;
        }

        static private bool Evaluate(ref int index, bool result, Clause clause, Dictionary<string, bool> model)
        {
            if (clause.Symbols[index + 1] == "") // means not symbol
            {
                result = LogicalConnectives.Evaluate(clause.LogicalConnectives[index++], result, Evaluate(ref index, result, clause, model));
            }
            else
            {
                result = LogicalConnectives.Evaluate(clause.LogicalConnectives[index], result, model[clause.Symbols[index + 1]]);
            }
            return result;
        }
    }
}
