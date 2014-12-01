using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.GrammarNS
{
    public class Rule
    {

        public ComplexToken MainToken { get; private set; }
        public List<ComplexToken> Tokens { get; private set; }

        private Rule()
        {
            Tokens = new List<ComplexToken>();
        }

        public Rule(ComplexToken mt, params ComplexToken[] tokens) : this()
        {
            MainToken = mt;            
            foreach (var t in tokens)
                Tokens.Add(t);
        }

        public static Rule Create(string left, params string[] rights)
        {
            Rule r = new Rule();
            r.MainToken = new ComplexToken(left, ""); //@todo
            foreach(var right in rights)
                r.Tokens.Add(new ComplexToken(right, "")); //@todo
            return r;
        }

        public static Rule Create(ComplexToken mt, params ComplexToken[] tokens)
        {
            Rule r = new Rule(mt, tokens);           
            return r;
        }

        public List<Rule> Split()
        {
            var children = new List<Rule>();
            var rule = new Rule(this.MainToken);
            foreach(var token in this.Tokens)
                if (token.Equals(ComplexToken.Alternation))
                {
                    children.Add(rule);
                    rule = new Rule(this.MainToken);
                }
                else
                {
                    rule.Tokens.Add(token);
                }
            children.Add(rule);
            return children;
        }

        public static Rule Parse(string str)
        {
            string[] els = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ComplexToken main = null;
            List<ComplexToken> tokens = new List<ComplexToken>();
            foreach (var el in els)
            {
                if (el.Trim().Equals(ComplexToken.Definition.Lexem)) continue;
                if (main == null)
                    main = new ComplexToken(el, ""); //@todo
                else
                    tokens.Add(new ComplexToken(el, "")); //@todo
            }
            return new Rule(main, tokens.ToArray());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var t in Tokens)
                sb.Append(" " + t);
            return MainToken + " " + ComplexToken.Definition.Lexem + sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == (obj)) return true;
            Rule other = obj as Rule;
            if (other.MainToken != this.MainToken 
                || other.Tokens.Count!=other.Tokens.Count) 
                return false;
            for (int i = 0; i < this.Tokens.Count; i++)
                if (!this.Tokens[i].Equals(other.Tokens[i]))
                    return false;
            return true;
        }
    }
}
