using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class LogicalConnectives
    {
        public static string[] Symbols { get
            {
                return new string[] { "~", "&", "||", "<=>", "=>" }; // order of this list is important so => is not found before <=> when parsing file
            } }

        public static string[] SymbolsOperationOrder
        {
            get
            {
                return new string[] { "~", "&", "||", "=>", "<=>" };
            }
        }

        public static bool Evaluate(string connector, bool left, bool right)
        {
            switch (Array.IndexOf(Symbols, connector))
            {
                case 0: // ~
                    return !right;
                case 1: // &
                    return left && right;
                case 2: // ||
                    return left || right;
                case 3: // <=>
                    return left ? right : !right;
                case 4: // =>            
                    return left ? right : true; 
            }

            return false;
        }
    }
}
