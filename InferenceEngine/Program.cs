using System;
using System.IO;
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
            // indicate if the file is complete
            bool tell = false;
            bool ask = false;

            StreamReader reader = new StreamReader(filename);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                // read the knowledgeBase
                if (line.Trim().Equals("TELL"))
                {
                    tell = true;
                    if ((line = reader.ReadLine().Replace(" ", "")) != null)
                    {
                        string[] sentences = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string sentence in sentences)
                        {
                            List<string> symbols = new List<string> {sentence};
                            List<string> logic = new List<string>();




                            foreach (string connector in LogicalConnectives.Symbols)
                            {
                                for (int i = 0; i < symbols.Count; i++)
                                {
                                    if (symbols[i].Contains(connector))
                                    {
                                        logic.Insert(i, connector);
                                        int index = symbols[i].IndexOf(connector);

                                        symbols.Insert(i + 1, symbols[i].Substring(index + connector.Length));
                                        symbols[i] = symbols[i].Substring(0, index);
                                    }
                                }
                            }

                            knowledgeBase.AddStatement(new Clause(symbols, logic));
                        }
                    }
                    else
                        return false;
                }

                // read the query
                if (line.Trim().Equals("ASK"))
                {
                    ask = true;
                    if ((line = reader.ReadLine()) != null)
                        query = line.Trim();
                    else
                        return false;
                }
            }

            reader.Close();

            return ask && tell;
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
