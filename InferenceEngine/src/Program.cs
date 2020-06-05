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
        private static int replacementVarIndex = 1;

        private static bool ParseFile(string filename, ref KnowledgeBase knowledgeBase, ref string query)
        {
            // indicate if the file is complete
            bool tell = false;
            bool ask = false;
            StreamReader reader = new StreamReader(filename);

            string line;
            string ReadState = "";
            while ((line = reader.ReadLine()) != null)
            {
                // read the knowledgeBase
                if (line.Trim().Equals("TELL"))
                    ReadState = "TELL";

                else if (line.Trim().Equals("ASK"))
                {
                    // can only be one ask statement
                    if (!ask)
                        ReadState = "ASK";
                    else // there is already a query error in the file 
                    {
                        ask = false;
                        break;
                    }
                }

                else if (ReadState.Equals("TELL"))
                {
                    tell = true;

                    // remove spaces form the line
                    line = line.Replace(" ", "");

                    // create an array of sentances
                    string[] sentences = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    // add each sentance to the knowledge base
                    foreach (string sentence in sentences)
                    {
                        CreateSentence(ref knowledgeBase, sentence);
                    }
                }

                else if (ReadState.Equals("ASK"))
                {
                    if (line != "")
                    {
                        if (!ask)
                        {
                            query = line.Trim();
                            ask = true;
                        }
                        else // there is already a query error in the file 
                        {
                            ask = false;
                            break;
                        }
                    }
                }
            }
                        

            reader.Close();

            // ensure that query is in the knowledge base and there is only one ask statment
            return ask && tell;
        }

        private static void CreateSentence(ref KnowledgeBase knowledgeBase, string sentence)
        {
            // handle parentheses
            if (sentence.Contains('(') || sentence.Contains(')'))
            {
                Stack<int> parenthesesIndex = new Stack<int>();

                for (int i = 0; i < sentence.Length; i++)
                {
                    if (sentence[i].Equals('(')) 
                        parenthesesIndex.Push(i);
                    else if (sentence[i].Equals(')'))
                    {
                        if (parenthesesIndex.Count == 0)
                        {
                            throw new Exception("Parentheses in file do not match");
                        }

                        // copy the inner Sentence in the sentace
                        string inbedSentence = sentence.Substring(parenthesesIndex.Peek() + 1, i-parenthesesIndex.Peek()-1);

                        // create a unique variable to replace the sentance with
                        string replacementVar = string.Format("*{0}", replacementVarIndex++);

                        // replace the inner Sentence with a variable
                        sentence = sentence.Replace(string.Format("({0})", inbedSentence), replacementVar);

                        // set the index back to the where the previous parentheses was
                        i = parenthesesIndex.Peek();

                        // add a new sentance to the knoledge base where the variable is equivilent to the replacement variable
                        CreateSentence(ref knowledgeBase, string.Format("{0}<=>{1}", inbedSentence, replacementVar));

                        parenthesesIndex.Pop();
                    }
                }

                if (parenthesesIndex.Count != 0)
                {
                    throw new Exception("Parentheses in file do not match");
                }
            }

            List<string> symbols = new List<string> { sentence };
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

            for (int i = 0; i < symbols.Count; i++)
            {
                if (symbols[i] == "" && logic[i] != "~")
                {
                    throw new Exception("logical connectives missused");
                }
            }

            knowledgeBase.AddStatement(new Sentence(symbols, logic));
        }

        static int Main(string[] args)
        {
            if (args.Count() != 2)
            {
                Console.WriteLine("ERROR: incorrect arguments");
#if DEBUG
                Console.ReadLine(); //stops the console from closing
#endif
                return 1;
            }

            string method = args[0];
            string filename = args[1];

            KnowledgeBase knowledgeBase = new KnowledgeBase();
            string query = "";

            try
            {
                if (!ParseFile(filename, ref knowledgeBase, ref query))
                {
                    Console.WriteLine("ERROR: incorrect file format");
#if DEBUG
                    Console.ReadLine(); //stops the console from closing
#endif
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
#if DEBUG
                Console.ReadLine(); //stops the console from closing
#endif
                return 3;
            }

            Result result;
            try
            {
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
#if DEBUG
                        Console.ReadLine(); //stops the console from closing
#endif
                        return 4;
                }
            }
            catch(System.Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
#if DEBUG
                Console.ReadLine(); //stops the console from closing
#endif
                return 5;
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
