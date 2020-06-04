using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Senctence
    {
        public List<string> Symbols { get; private set; }

        public List<string> LogicalConnectives { get; private set; }

        public bool IsHornSenctence { get; }

        public List<string> Premise { get; private set; }

        public string Conclusion { get; private set; }

        public Senctence(List<string> symbols, List<string> logicalConnectives)
        {
            Symbols = symbols;
            LogicalConnectives = logicalConnectives;

            Debug.Assert(symbols.Count == logicalConnectives.Count + 1);

            IsHornSenctence = false;
            Premise = null;
            Conclusion = null;

            // test if horn Senctence
            if (LogicalConnectives.Count == 0)
            {
                IsHornSenctence = true;
                Conclusion = Symbols[0];
                Premise = new List<string>();
            }
            else if (LogicalConnectives.Last() == "=>")
            {
                IsHornSenctence = true;
                for (UInt16 i = 0; i < LogicalConnectives.Count - 1; i++)
                {
                    if (!LogicalConnectives[i].Equals("&"))
                    {
                        IsHornSenctence = false;
                        break;
                    }
                }

                if (IsHornSenctence)
                {
                    Conclusion = Symbols.Last();
                    Premise = symbols.ConvertAll(symbol => String.Copy(symbol));
                    Premise.Remove(Conclusion);
                }
            }
        }
    }
}
