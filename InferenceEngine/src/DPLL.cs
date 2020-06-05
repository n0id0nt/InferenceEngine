using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class DavisPutnamAlgorithm
    {
        static public Result DPLLSatisfiable(KnowledgeBase knowledgeBase, string query)
        {
            return DPLL(knowledgeBase.Sentences, knowledgeBase.Symbols, new Dictionary<string, bool>());
        }
           
        static private Result DPLL(List<Sentence> sentences, List<string> symbols, Dictionary<string, bool> model)
        {
            // if every clause is true return ture

            // if any clause is false return false
            string P = "";
            bool value = false;
            // find a unused symbol in teh model

            // if symbol is not null 
            Dictionary<string, bool> modelCopy = new Dictionary<string, bool>(model);
            modelCopy.Add(P, value);
            return DPLL(sentences, symbols, modelCopy);

            // 
        }
    }
}
