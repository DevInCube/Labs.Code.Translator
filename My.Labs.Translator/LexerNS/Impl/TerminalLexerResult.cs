using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class TerminalLexerResult : ILexerResult
    {

        private Dictionary<string, ComplexToken> tokens;
        private List<ComplexToken> lexems;

        public List<ComplexToken> Lexems
        {
            get { return lexems; }
        }

        public Dictionary<int, ComplexToken> Tokens
        {
            get { throw new NotImplementedException(); }
        }

        public TerminalLexerResult()
        {
            tokens = new Dictionary<string, ComplexToken>();
            lexems = new List<ComplexToken>();
        }

        public void AddToken(GrammarNS.ComplexToken token)
        {
            tokens.Add(token.Lexem, token);
        }

        public GrammarNS.ComplexToken GetToken(string lexem)
        {
            if (tokens.ContainsKey(lexem))
                return tokens[lexem];
            return null;
        }

        public GrammarNS.ComplexToken AddToken(string lexem, string token, int line, int pos)
        {
            ComplexToken t = new ComplexToken(lexem, token, line, pos);
            this.AddToken(t);
            return t;
        }

        public void AddLexem(GrammarNS.ComplexToken token)
        {
            lexems.Add(token);
        }

    }
}
