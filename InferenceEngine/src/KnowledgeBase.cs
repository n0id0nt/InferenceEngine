using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class KnowledgeBase
    {
        public List<Sentence> Sentences { get; private set; }
        public List<string> Symbols { get; private set; }

        public bool IsHornClause { get
            {
                return Sentences.All(s => s.IsHornClause == true);
            } }

        public KnowledgeBase()
        {
            Sentences = new List<Sentence>();
            Symbols = new List<string>();
        }

        public void AddStatement(Sentence sentence)
        {
            Sentences.Add(sentence);
            foreach (string l in sentence.Symbols)
                if (!Symbols.Contains(l) && l != "")
                    Symbols.Add(l);
        }

        public Queue<string> InitiallyTrue()
        {
            Queue<string> i = new Queue<string>();
            foreach (Sentence l in Sentences)
                if (l.Premise.Count == 0)
                    i.Enqueue(l.Conclusion);

            return i;
        }

        public List<Sentence> InPremise(string value)
        {
            List<Sentence> l = new List<Sentence>();
            foreach (Sentence c in Sentences)
                if (c.Premise.Contains(value))
                    l.Add(c);
            return l;
        }

        public List<Sentence> InConclusion(string value)
        {
            List<Sentence> l = new List<Sentence>();
            foreach (Sentence c in Sentences)
                if (c.Conclusion.Equals(value))
                    l.Add(c);
            return l;
        }
    }
}
