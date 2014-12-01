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

        public SyntacticGrammar SyntacticGrammar { get; private set; }

        public List<Rule> Rules { get; private set; }

        private Dictionary<ComplexToken, List<ComplexToken>> Fi = new Dictionary<ComplexToken, List<ComplexToken>>();        

        public SymbolAttributeTable AttributeTable { get; private set; }
        public List<ComplexToken> Keywords { get; private set; }
        public List<ComplexToken> Delimiters { get; private set; }

        private Grammar()
        {
            AttributeTable = new SymbolAttributeTable();
            Keywords = new List<ComplexToken>();
            Delimiters = new List<ComplexToken>();
            Rules = new List<Rule>();
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
                        var lexem = t.Lexem;
                        if (lexem.Length == 0) continue;
                        if (char.IsLetterOrDigit(lexem[0]))
                        {
                            t.Token = "keyword";                            
                            this.Keywords.Add(t);
                        }
                        else
                        {
                            t.Token = "delimiter";
                            this.Delimiters.Add(t);
                        }
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

        public static Grammar LexerGrammar { get; private set; }
        public static Grammar SyntaxGrammar { get; private set; }

        static Grammar()
        {
            string T_DELIMITER = "delimiter";
            string T_TERMINAL = "terminal";
            string T_NONTERMINAL = "non-terminal";

            var NON_TERMINAL = new ComplexToken("non-terminal", T_TERMINAL);
            var TERMINAL = new ComplexToken("terminal", T_TERMINAL);

            var DOTS = new ComplexToken(":", T_DELIMITER);
            var DOTCOMMA = new ComplexToken(";", T_DELIMITER);
            var COMMA = new ComplexToken(",", T_DELIMITER);
            var OR = new ComplexToken("|", T_DELIMITER);
            var EMPTY = ComplexToken.Empty;

            Grammar g = new Grammar();
            g.AttributeTable.AllowAllHighLetters().AllowAllLowLetters().AllowAllNumbers().AllowDefaultWhiteSpaces();
            g.AttributeTable.AllowDelimiters('|',':',';','"','\'',')','(','=',',');
            g.AttributeTable.AllowLetters('-', '_');

            var GRAMMAR = new ComplexToken("grammar", T_NONTERMINAL);
            var SYNTAX = new ComplexToken("syntax", T_NONTERMINAL);
            var RULE = new ComplexToken("rule", T_NONTERMINAL);
            var RULES_MORE = new ComplexToken("rules-more", T_NONTERMINAL);
            var RULE_NAME = new ComplexToken("rule-name", T_NONTERMINAL);
            var ALTERNATION_EXPRESSION = new ComplexToken("alternation-expression", T_NONTERMINAL);
            var EXPRESSION = new ComplexToken("expression", T_NONTERMINAL);
            var EXPRESSION_MORE = new ComplexToken("expression-more", T_NONTERMINAL);
            var TOKEN = new ComplexToken("token", T_NONTERMINAL);
            var TOKENS_MORE = new ComplexToken("tokens-more", T_NONTERMINAL);

            var rules = new List<Rule>()
            {
                Rule.Create(GRAMMAR, SYNTAX), //1. {[1]}
                Rule.Create(SYNTAX, RULE, RULES_MORE), //2. {[2][1]}
                Rule.Create(RULES_MORE, SYNTAX), //3. {[1]}
                Rule.Create(RULES_MORE, EMPTY), //4. return;
                Rule.Create(RULE, RULE_NAME, DOTS, ALTERNATION_EXPRESSION, DOTCOMMA), //5. CreateRule();
                Rule.Create(ALTERNATION_EXPRESSION, EXPRESSION, EXPRESSION_MORE), //6. {[2][1]}
                Rule.Create(EXPRESSION_MORE, OR, ALTERNATION_EXPRESSION), //7. {[1]}
                Rule.Create(EXPRESSION_MORE, EMPTY ), //8. return;
                Rule.Create(RULE_NAME,NON_TERMINAL), //9. MainToken;
                Rule.Create(EXPRESSION, TOKEN, TOKENS_MORE), //10. {[2][1]}
                Rule.Create(TOKENS_MORE, EXPRESSION), //11. {[1]}
                Rule.Create(TOKENS_MORE, EMPTY), //12. return;
                Rule.Create(TOKEN, TERMINAL), //13. Terminal
                Rule.Create(TOKEN, NON_TERMINAL) //14. Terminal
            };            
            g.Rules = rules;
            SyntaxGrammar = g;

            g = new Grammar();
            g.AttributeTable.AllowAllHighLetters().AllowAllLowLetters().AllowAllNumbers().AllowDefaultWhiteSpaces();
            g.AttributeTable.AllowDelimiters('|', ':', ';', '"', '\'', ')', '(', '=', ',','\\');
            g.AttributeTable.AllowLetters('-', '_');            
            
            var LEX = new ComplexToken("lex", T_NONTERMINAL);
            var TYPES = new ComplexToken("types", T_NONTERMINAL);
            var TYPE = new ComplexToken("type", T_NONTERMINAL);
            var TYPE_NAME = new ComplexToken("type-name", T_NONTERMINAL);
            var ELEMENTS = new ComplexToken("elements", T_NONTERMINAL);
            var ELEMENTS_MORE = new ComplexToken("elements-more", T_NONTERMINAL);
            var ELEMENT = new ComplexToken("element", T_NONTERMINAL); 
           
            rules = new List<Rule>()
            {
                Rule.Create(LEX, TYPES),
                Rule.Create(TYPES, TYPE, TYPES),
                Rule.Create(TYPES, EMPTY),
                Rule.Create(TYPE, TYPE_NAME, DOTS, ELEMENTS, DOTCOMMA),
                Rule.Create(TYPE_NAME, NON_TERMINAL),
                Rule.Create(ELEMENTS, ELEMENT, ELEMENTS_MORE),
                Rule.Create(ELEMENTS_MORE, COMMA, ELEMENTS),                
                Rule.Create(ELEMENTS_MORE, EMPTY),                         
                Rule.Create(ELEMENT, TERMINAL),                      
                Rule.Create(ELEMENT, NON_TERMINAL)
            };
            g.Rules = rules;
            LexerGrammar = g;
        }

        internal static Grammar Create(SyntacticGrammar gram)
        {
            Grammar g = new Grammar();
            g.Rules = gram.Rules;
            g.AttributeTable = gram.Table;
            g.ExtractSpecialTokens();
            g.SyntacticGrammar = gram;            
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
