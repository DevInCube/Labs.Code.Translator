using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My.Labs.Translator.LexerNS
{
    public interface ILexerResult
    {

        List<ComplexToken> Lexems { get; }
        Dictionary<int, ComplexToken> Tokens { get; }
        
        void AddToken(ComplexToken token);

        ComplexToken GetToken(string lexem);

        ComplexToken AddToken(string lexem, string token, int line, int pos);

        void AddLexem(ComplexToken token);        
    }
}
