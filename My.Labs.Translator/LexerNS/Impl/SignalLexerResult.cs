using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class SignalLexerResult : ILexerResult
    {

        const int DELIMITER_START = 0;
        const int MULTICHARDELIMITER_START = 256;
        const int KEYWORD_START = 301;
        const int CONSTANT_START = 501;
        const int IDENTIFIER_START = 1001;

        public List<ComplexToken> Lexems { get; private set; }
        public Dictionary<int, ComplexToken> Tokens { get; private set; }

        public SignalLexerResult()
        {
            Tokens = new Dictionary<int, ComplexToken>();
            Lexems = new List<ComplexToken>();
        }

        public void AddToken(ComplexToken t)
        {
            switch (t.Token)
            {
                case ("unsigned-integer"):
                    {
                        for (int i = CONSTANT_START; i < IDENTIFIER_START; i++)
                            if (!Tokens.ContainsKey(i))
                            {
                                Tokens.Add(i, t);
                                t.Key = i;
                                break;
                            }
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
            /*
            if (t.Token == TokenType.NonTerminal)
                throw new Exception("Adding NonTerminal token to Lexer Result");
            else
            {
                if (t.Key != 0)
                {
                    if (Tokens.ContainsKey(t.Key)) throw new Exception("Such tokn key already exists");
                    Tokens[t.Key] = t;
                    return;
                }
                switch (t.Token)
                {
                    case (TokenType.Delimiter):
                        {
                            for (int i = (int)TokenType.Delimiter; i < (int)TokenType.MultiCharDelimiter; i++)
                                if (!Tokens.ContainsKey(i))
                                {
                                    Tokens.Add(i, t);
                                    t.Key = i;
                                    break;
                                }
                            break;
                        }
                    case (TokenType.Keyword):
                        {
                            for (int i = (int)TokenType.Keyword; i < (int)TokenType.Constant; i++)
                                if (!Tokens.ContainsKey(i))
                                {
                                    Tokens.Add(i, t);
                                    t.Key = i;
                                    break;
                                }
                            break;
                        }
                    case (TokenType.Constant):
                        {
                            for (int i = (int)TokenType.Constant; i < (int)TokenType.Identifier; i++)
                                if (!Tokens.ContainsKey(i))
                                {
                                    Tokens.Add(i, t);
                                    t.Key = i;
                                    break;
                                }
                            break;
                        }
                    case (TokenType.Identifier):
                        {
                            for (int i = (int)TokenType.Identifier; true; i++)
                                if (!Tokens.ContainsKey(i))
                                {
                                    Tokens.Add(i, t);
                                    t.Key = i;
                                    break;
                                }
                            break;
                        }
                }
            }*/
        }

        public ComplexToken GetDelimiter(char delimiter)
        {
            for (int i = DELIMITER_START; i < MULTICHARDELIMITER_START; i++)
                if (Tokens.ContainsKey(i) && Tokens[i].Lexem.Equals(delimiter.ToString()))
                    return Tokens[i];
            return null;
        }

        public ComplexToken GetKeyword(string keyword)
        {
            for (int i = KEYWORD_START; i < CONSTANT_START; i++)
                if (Tokens.ContainsKey(i) && Tokens[i].Lexem.Equals(keyword))
                    return Tokens[i];
            return null;
        }

        public void AddLexem(ComplexToken token)
        {
            this.Lexems.Add(token);
        }

        internal IEnumerable<ComplexToken> GetKeywords()
        {
            List<ComplexToken> tokens = new List<ComplexToken>();
            for (int i = KEYWORD_START; i < CONSTANT_START; i++)
                if (Tokens.ContainsKey(i))
                    tokens.Add(Tokens[i]);
                else
                    return tokens;
            return tokens;
        }

        internal IEnumerable<ComplexToken> GetDelimiters()
        {
            List<ComplexToken> tokens = new List<ComplexToken>();
            for (int i = DELIMITER_START; i < MULTICHARDELIMITER_START; i++)
                if (Tokens.ContainsKey(i))
                    tokens.Add(Tokens[i]);
                else
                    return tokens;
            return tokens;
        }

        internal IEnumerable<ComplexToken> GetConstants()
        {
            List<ComplexToken> tokens = new List<ComplexToken>();
            for (int i = CONSTANT_START; i < IDENTIFIER_START; i++)
                if (Tokens.ContainsKey(i))
                    tokens.Add(Tokens[i]);
                else
                    return tokens;
            return tokens;
        }

        internal IEnumerable<ComplexToken> GetIdentifiers()
        {
            List<ComplexToken> tokens = new List<ComplexToken>();
            for (int i = IDENTIFIER_START; true; i++)
                if (Tokens.ContainsKey(i))
                    tokens.Add(Tokens[i]);
                else
                    return tokens;
            return tokens;
        }

        public bool HasConstant(string p, out int index)
        {
            for (index = CONSTANT_START; index < IDENTIFIER_START; index++)
                if (Tokens.ContainsKey(index))
                {
                    if (Tokens[index].Lexem.Equals(p))
                        return true;
                }
                else break;                    
            return false;
        }

        public bool HasKeyword(string p, out int index)
        {            
            for (index = KEYWORD_START; index < CONSTANT_START; index++)
                if (Tokens.ContainsKey(index))
                {
                    if (Tokens[index].Lexem.Equals(p))
                        return true;
                }
                else break;
            return false;
        }

        internal bool GetIdentifier(string p, out int index)
        {         
            for (index = IDENTIFIER_START; true; index++)
                if (Tokens.ContainsKey(index))
                {
                    if (Tokens[index].Lexem.Equals(p))
                        return true;
                }
                else break;
            return false;
        }

        public ComplexToken AddToken(string lexem, string token, int line, int pos)
        {
            ComplexToken t = new ComplexToken(lexem, token, line, pos);
            //@todo
            throw new NotImplementedException();
            return t;
        }

        public ComplexToken GetToken(string p)
        {
            foreach (var val in Tokens.Values)
            {
                if (val.Lexem.Equals(p))
                    return val;
            }
            return null;
        }

    }
}
