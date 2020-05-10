using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Program
    {
        static void ParseFile(string filename, ref KnowledgeBase knowledgeBase, ref Query query)
        {

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
            Query query = new Query();

            ParseFile(filename, ref knowledgeBase, ref query);

            bool result;
            switch (method)
            {
                case "TT":
                    result = TruthTableChecking.Equals(knowledgeBase, query);
                    break;
                case "FC":
                    result = TruthTableChecking.Equals(knowledgeBase, query);
                    break;
                case "BC":
                    result = TruthTableChecking.Equals(knowledgeBase, query);
                    break;
                default:
                    Console.WriteLine("ERROR: invalid method provided");
                    return 2;
            }

            if (result)
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }
#if DEBUG
            Console.ReadLine();
#endif
            return 0;
        }
    }
}
