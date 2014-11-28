using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class Symbol
    {

        public char Value { get; private set; }
        public SymbolAttribute Attribute { get; private set; }
        public int CodeLine { get; private set; }
        public int CodePosition { get; private set; }

        public Symbol(char val, SymbolAttribute attr, int line, int pos)
        {
            this.Value = val;
            this.Attribute = attr;
            this.CodeLine = line;
            this.CodePosition = pos;
        }

        public bool IsEOF()
        {
            return this.Value == (char)0;
        }

        public static Symbol CreateEOF(int line, int pos)
        {
            return new Symbol((char)0, SymbolAttribute.Forbidden, line, pos);
        }
    }
}
