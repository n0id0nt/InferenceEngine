using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class KnowledgeBase
    {
        public List<Senctence> Sentences { get; private set; }
        public List<string> Symbols { get; private set; }

        public bool IsHornSenctence { get
            {
                return Sentences.All(s => s.IsHornSenctence == true);
            } }

        public KnowledgeBase()
        {
            Sentences = new List<Senctence>();
            Symbols = new List<string>();
        }

        /// <summary>
        /// create a knowledge base with a single query
        /// </summary>
        /// <param name="query"></param>
        public KnowledgeBase(string query) : this()
        {
            AddStatement(new Senctence(new List<string> { query }, new List<string>()));
        }

        public void AddStatement(Senctence Senctence)
        {
            Sentences.Add(Senctence);
            foreach (string l in Senctence.Symbols)
                if (!Symbols.Contains(l) && l != "")
                    Symbols.Add(l);
        }

        public Queue<string> InitiallyTrue()
        {
            Queue<string> i = new Queue<string>();
            foreach (Senctence l in Sentences)
                if (l.Premise.Count == 0)
                    i.Enqueue(l.Conclusion);

            return i;
        }

        public List<Senctence> InPremise(string value)
        {
            List<Senctence> l = new List<Senctence>();
            foreach (Senctence c in Sentences)
                if (c.Premise.Contains(value))
                    l.Add(c);
            return l;
        }

        public List<Senctence> InConclusion(string value)
        {
            List<Senctence> l = new List<Senctence>();
            foreach (Senctence c in Sentences)
                if (c.Conclusion.Equals(value))
                    l.Add(c);
            return l;
        }
    }
}
