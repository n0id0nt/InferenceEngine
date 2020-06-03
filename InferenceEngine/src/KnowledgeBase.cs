using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class KnowledgeBase
    {
        public List<Clause> Sentences { get; private set; }
        public List<string> Symbols { get; private set; }

        public bool IsHornClause { get
            {
                return Sentences.All(s => s.IsHornClause == true);
            } }

        public KnowledgeBase()
        {
            Sentences = new List<Clause>();
            Symbols = new List<string>();
        }

        /// <summary>
        /// create a knowledge base with a single query
        /// </summary>
        /// <param name="query"></param>
        public KnowledgeBase(string query) : this()
        {
            AddStatement(new Clause(new List<string> { query }, new List<string>()));
        }

        public void AddStatement(Clause clause)
        {
            Sentences.Add(clause);
            foreach (string l in clause.Symbols)
                if (!Symbols.Contains(l) && l != "")
                    Symbols.Add(l);
        }

        public Queue<string> InitiallyTrue()
        {
            Queue<string> i = new Queue<string>();
            foreach (Clause l in Sentences)
                if (l.Premise.Count == 0)
                    i.Enqueue(l.Conclusion);

            return i;
        }

        public List<Clause> InPremise(string value)
        {
            List<Clause> l = new List<Clause>();
            foreach (Clause c in Sentences)
                if (c.Premise.Contains(value))
                    l.Add(c);
            return l;
        }

        public List<Clause> InConclusion(string value)
        {
            List<Clause> l = new List<Clause>();
            foreach (Clause c in Sentences)
                if (c.Conclusion.Equals(value))
                    l.Add(c);
            return l;
        }
    }
}
