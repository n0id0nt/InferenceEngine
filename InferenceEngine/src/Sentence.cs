using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Sentence
    {
        public List<string> Symbols { get; private set; }

        public List<string> LogicalConnectives { get; private set; }

        public bool IsHornClause { get; }

        public List<string> Premise { get; private set; }

        public string Conclusion { get; private set; }

        public Sentence(List<string> symbols, List<string> logicalConnectives)
        {
            Symbols = symbols;
            LogicalConnectives = logicalConnectives;

            Debug.Assert(symbols.Count == logicalConnectives.Count + 1);

            IsHornClause = false;
            Premise = null;
            Conclusion = null;

            // test if horn Sentence
            if (LogicalConnectives.Count == 0)
            {
                IsHornClause = true;
                Conclusion = Symbols[0];
                Premise = new List<string>();
            }
            else if (LogicalConnectives.Last() == "=>")
            {
                IsHornClause = true;
                for (int i = 0; i < LogicalConnectives.Count - 1; i++)
                {
                    if (!LogicalConnectives[i].Equals("&"))
                    {
                        IsHornClause = false;
                        break;
                    }
                }

                if (IsHornClause)
                {
                    Conclusion = Symbols.Last();
                    Premise = symbols.ConvertAll(symbol => String.Copy(symbol));
                    Premise.Remove(Conclusion);
                }
            }
        }
    }
}
