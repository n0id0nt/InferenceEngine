using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        static bool ParseFile(string filename, ref KnowledgeBase knowledgeBase, ref string query)
        {
            knowledgeBase.AddStatement(new Clause(new string[] { "p2" }, "p3"));
            knowledgeBase.AddStatement(new Clause(new string[] { "p3" }, "p1"));
            knowledgeBase.AddStatement(new Clause(new string[] { "c" }, "e"));
            knowledgeBase.AddStatement(new Clause(new string[] { "b", "e" }, "f"));
            knowledgeBase.AddStatement(new Clause(new string[] { "f", "g" }, "h"));
            knowledgeBase.AddStatement(new Clause(new string[] { "p1" }, "d"));
            knowledgeBase.AddStatement(new Clause(new string[] { "p1", "p3" }, "c"));
            knowledgeBase.AddStatement(new Clause(null, "a"));
            knowledgeBase.AddStatement(new Clause(null, "b"));
            knowledgeBase.AddStatement(new Clause(null, "p2"));

            query = "d";

            return true;
        }

        static int Main(string[] args)
        {
            if (args.Count() != 2)
            {
                Console.WriteLine("ERROR: incorrect arguments");
                return 1;
            }

            string method = args[0];
            string filename = args[1];

            KnowledgeBase knowledgeBase = new KnowledgeBase();
            string query = "";

            if (!ParseFile(filename, ref knowledgeBase, ref query))
            {
                Console.WriteLine("ERROR: incorrect file format");
                return 3;
            }

            Result result;
            switch (method)
            {
                case "TT":
                    result = TruthTableChecking.TT(knowledgeBase, query);
                    break;
                case "FC":
                    result = ForwardChaining.FC(knowledgeBase, query);
                    break;
                case "BC":
                    result = BackwardChaining.BC(knowledgeBase, query);
                    break;
                default:
                    Console.WriteLine("ERROR: invalid method provided");
                    return 2;
            }

            if (result.Success)
            {
                string second;
                if (result.Symbols is null)
                    second = result.Count.ToString();
                else
                    second = string.Join(", ", result.Symbols);
                Console.WriteLine("YES: {0}", second);
            }
            else
            {
                Console.WriteLine("NO");
            }
#if DEBUG
            Console.ReadLine(); //stops the console from closing
#endif
            return 0;
        }
    }
}
