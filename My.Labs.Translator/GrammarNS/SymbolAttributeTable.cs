using My.Labs.Translator.LexerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.GrammarNS
{
    public class SymbolAttributeTable
    {

        public SymbolAttribute[] Table { get; private set; }

        public SymbolAttributeTable()
        {
            Table = new SymbolAttribute[256];
        }

        public SymbolAttributeTable AllowDefaultWhiteSpaces()
        {
             uint[] WHITESPACE_CHAR_CODES = new uint[]{
                    32, // whitespace
                    13, // \n
                    10, // \r
                    9,  // \v
                    11, // \t
                    12, // new page
                };      
            foreach (var ws in WHITESPACE_CHAR_CODES)
                Table[ws] = SymbolAttribute.Whitespace;
            return this;
        }

        public SymbolAttributeTable AllowDelimiters(params char[] dels)
        {
            foreach (var del in dels)
                Table[(int)del] = SymbolAttribute.Delimiter;
            return this;
        }

        public SymbolAttributeTable AllowAllNumbers()
        {
            for (var dt_index = 48; dt_index <= 57; dt_index++)
                Table[dt_index] = SymbolAttribute.Digit;
            return this;
        }

        public SymbolAttributeTable AllowAllHighLetters()
        {
            for (var lt_index = 65; lt_index <= 90; lt_index++)
                Table[lt_index] = SymbolAttribute.Letter;
            return this;
        }

        public SymbolAttributeTable AllowAllLowLetters()
        {
            for (var lt_index = 97; lt_index <= 122; lt_index++)
                Table[lt_index] = SymbolAttribute.Letter;
            return this;
        }


        internal void AllowDigits(char ch)
        {
            Table[(int)ch] = SymbolAttribute.Digit;
        }

        internal SymbolAttributeTable AllowLetters(params char[] chs)
        {
            foreach (var ch in chs)
                Table[(int)ch] = SymbolAttribute.Letter;
            return this;
        }
    }
}
