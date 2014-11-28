using My.Labs.Translator.LexerNS;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.GrammarNS
{
    public class Grammar
    {

        public string Plain { get; set; }
        public List<Rule> Rules { get; private set; }
        public List<ComplexToken> Terminals { get; private set; }
        public List<ComplexToken> NonTerminals { get; private set; }
        public Rule[,] LLTable { get; private set; }

        private Dictionary<ComplexToken, List<ComplexToken>> Fi = new Dictionary<ComplexToken, List<ComplexToken>>();
        private Dictionary<ComplexToken, List<ComplexToken>> Fo = new Dictionary<ComplexToken, List<ComplexToken>>();

        public SymbolAttributeTable AttributeTable { get; private set; }
        public List<ComplexToken> Keywords { get; private set; }
        public List<ComplexToken> Delimiters { get; private set; }

        private Grammar()
        {
            AttributeTable = new SymbolAttributeTable();
            Keywords = new List<ComplexToken>();
            Delimiters = new List<ComplexToken>();
            Rules = new List<Rule>();
            Terminals = new List<ComplexToken>();
            NonTerminals = new List<ComplexToken>();
        }

        public bool IsTerminal(ComplexToken token)
        {            
            foreach (var rule in Rules)
                if (rule.MainToken.Equals(token))
                    return false;
            return true;
        }

        private void ExtractSpecialTokens()
        {
            foreach (var r in Rules)            
                foreach (var t in r.Tokens)
                    if (t.Token.Equals("terminal"))
                    {
                        //@todo this is only for simgle delimiters
                        if (char.IsLetterOrDigit(t.Lexem[0]))
                        {
                            foreach (var ch in t.Lexem)
                                AttributeTable.AllowLetters(ch);
                            if (r.Tokens.Count != 1)
                            {
                                t.Token = "keyword";
                                this.Keywords.Add(t);
                            }
                        }
                        else
                        {
                            foreach (var ch in t.Lexem)
                                AttributeTable.AllowDelimiters(ch);
                            t.Token = "delimiter";
                            this.Delimiters.Add(t);
                        }
                    }
        }

        void RegisterTokens()
        {
            foreach (var rule in this.Rules)
            {
                this.RegisterToken(rule.MainToken);
                foreach (var token in rule.Tokens)
                    this.RegisterToken(token);
            }
        }

        void RegisterToken(ComplexToken token)
        {
            if (IsTerminal(token))
            {
                if (!Terminals.Contains(token))
                    Terminals.Add(token);
            }
            else
            {
                if (!NonTerminals.Contains(token))
                    NonTerminals.Add(token);
            }
        }        

        public override string ToString()
        {
            var sb = new StringBuilder();
            int i = 1;
            foreach (var r in Rules)
                sb.Append((i++) + ". " + r + "\r\n");
            return sb.ToString();
        }

        public List<ComplexToken> FIRST(ComplexToken A)
        {
            if (Fi.ContainsKey(A))
                return Fi[A];
            else
            {               
                var firstSet = new List<ComplexToken>();
                if (IsTerminal(A))
                    firstSet.Add(A);
                else
                {
                    foreach (Rule rule in Rules)
                    {
                        if (rule.MainToken.Equals(A))
                        {
                            var first = rule.Tokens.First();
                            if (IsTerminal(first))
                                firstSet.Add(first);
                            else                                
                                firstSet.AddRange(FIRST(first));
                        }
                    }
                }
                Fi.Add(A, firstSet);
                return firstSet;
            }
        }

        public List<ComplexToken> FOLLOW(ComplexToken X)
        {
            if (Fo.ContainsKey(X))
                return Fo[X];
            throw new Exception("no FOLLOW for " + X);
        }

        void BuildFOLLOW()
        {
            Fo.Clear();
            foreach (var X in this.NonTerminals)
            {
                // #1
                var followSet = new List<ComplexToken>();
                Fo.Add(X, followSet);
                // #2
                if (X.Equals(this.NonTerminals.First()))
                    followSet.Add(LL1Analyzer.EOFToken);    
                // #3
                foreach (Rule rule in Rules)
                {
                    var A = rule.MainToken;
                    var B = X;
                    if (rule.Tokens.Count > 1
                        && rule.Tokens[rule.Tokens.Count - 2].Equals(B))
                    {
                        var v = rule.Tokens.Last();
                        var FIRST_v = FIRST(v);
                        foreach (var token in FIRST_v)
                            if (!token.Equals(ComplexToken.Empty))
                                followSet.Add(token);
                    }                    
                }
                // #4
                foreach (Rule rule in Rules)
                {
                    var A = rule.MainToken;
                    var B = X;
                    var v = rule.Tokens.Last();
                    bool BisLast = v.Equals(B);
                    bool BisPreLast = rule.Tokens.Count > 2 && rule.Tokens[rule.Tokens.Count-2].Equals(B);                    
                    if(BisLast 
                        || (BisPreLast && !IsTerminal(v)&& FOLLOW(v).Contains(ComplexToken.Empty)))
                    {
                        if(!A.Equals(B))
                            followSet.AddRange(FOLLOW(A));
                    }
                }                
            }
        }

        void BuildTable()
        {
            Grammar g = this;
            var nonTermNum = g.NonTerminals.Count;
            var termNum = g.Terminals.Count + 1;
            var table = new Rule[nonTermNum, termNum];
            for (int i = 0; i < nonTermNum; i++)
            {
                var A = g.NonTerminals[i];
                for (int j = 0; j < termNum; j++)
                {
                    ComplexToken a = null;
                    if (j == termNum - 1)
                        a = LL1Analyzer.EOFToken;
                    else 
                        a = g.Terminals[j];
                    foreach (var rule in g.Rules)
                    {
                        if (rule.MainToken.Equals(A))
                        {
                            var w = rule.Tokens.First();
                            var FIRST_w = g.FIRST(w);
                            var FIRST_A = g.FIRST(A);
                            var FOLLOW_A = g.FOLLOW(A);
                            /*bool followOk = false;
                            foreach (var N in rule.Tokens)
                                if (!N.IsTerminal
                                    && FOLLOW(N).Contains(a))
                                {
                                    followOk = true;
                                    break;
                                }*/
                            if ((FIRST_A.Contains(a) && w == a)
                                || FIRST_w.Contains(a)
                                //|| followOk
                                /*|| FIRST_w.Contains(a)*/
                                /*|| (FIRST_w.Contains(Token.Empty) && FOLLOW_A.Contains(a))*/)
                            {
                                if (table[i, j] != null) 
                                    throw new Exception("Grammar is not LL(1)");
                                table[i, j] = rule;
                            }
                        }
                    }
                }
            }
            LLTable = table;
        }

        public static Grammar LexerGrammar { get; private set; }
        public static Grammar SyntaxGrammar { get; private set; }

        static Grammar()
        {
            Grammar g = new Grammar();
            g.AttributeTable.AllowAllHighLetters().AllowAllLowLetters().AllowAllNumbers().AllowDefaultWhiteSpaces();
            g.AttributeTable.AllowDelimiters('|',':',';','"','\'',')','(','=',',');
            g.AttributeTable.AllowLetters('-', '_');
            var rules = new List<Rule>()
            {
                Rule.Create("grammar", "syntax"), //1. {[1]}
                Rule.Create("syntax", "rule", "rules-more"), //2. {[2][1]}
                Rule.Create("rules-more" ,"syntax"), //3. {[1]}
                Rule.Create("rules-more" ,""), //4. return;
                Rule.Create("rule", "rule-name", ":", "alternation-expression",";"), //5. CreateRule();
                Rule.Create("alternation-expression", "expression", "expression-more" ), //6. {[2][1]}
                Rule.Create("expression-more", "|", "alternation-expression" ), //7. {[1]}
                Rule.Create("expression-more", "" ), //8. return;
                Rule.Create("rule-name", "non-terminal"), //9. MainToken;
                Rule.Create("expression", "token", "tokens-more"), //10. {[2][1]}
                Rule.Create("tokens-more", "expression"), //11. {[1]}
                Rule.Create("tokens-more", ""), //12. return;
                Rule.Create("token","terminal"), //13. Terminal
                Rule.Create("token","non-terminal") //14. Terminal
            };            
            g.Rules = rules;
            g.RegisterTokens();
            SyntaxGrammar = g;

            g = new Grammar();
            g.AttributeTable.AllowAllHighLetters().AllowAllLowLetters().AllowAllNumbers().AllowDefaultWhiteSpaces();
            g.AttributeTable.AllowDelimiters('|', ':', ';', '"', '\'', ')', '(', '=', ',','\\');
            g.AttributeTable.AllowLetters('-', '_');
            rules = new List<Rule>()
            {
                Rule.Create("lex","types"),
                Rule.Create("types","type", "types"),
                Rule.Create("types",""),
                Rule.Create("type","type-name",":","elements",";"),
                Rule.Create("type-name","non-terminal"),
                Rule.Create("elements","element", "elements-more"),
                Rule.Create("elements-more",",", "elements"),
                Rule.Create("elements-more",""),
                Rule.Create("element","terminal"),
                Rule.Create("element","non-terminal")
            };
            g.Rules = rules;
            g.RegisterTokens();
            LexerGrammar = g;
        }

        internal static Grammar Create(SyntacticGrammar gram)
        {
            Grammar g = new Grammar();
            g.Rules = gram.Rules;
            g.AttributeTable = gram.Table;
            g.ExtractSpecialTokens();
            g.RegisterTokens();
            //@todo
            return g;
        }

        public static Grammar Create(
            string LexGrammarStr,
            string SyntacticGrammarStr)
        {
            var lexGrammar = LexGrammar.Parse(LexGrammarStr);
            var syntacticGrammar = SyntacticGrammar.Parse(SyntacticGrammarStr, Grammar.SyntaxGrammar, lexGrammar);
            return Grammar.Create(syntacticGrammar);
        }
    }
}
