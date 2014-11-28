using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My.Labs.Translator.LexerNS
{
    public interface ILexer
    {
        
        SignalLexerResult Parse(string programText);
    }
}
