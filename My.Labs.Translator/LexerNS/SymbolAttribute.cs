using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public enum SymbolAttribute
    {
        Forbidden,
        Whitespace,
        Delimiter,
        Digit,
        Letter
    }

    public static class ASCIISymbolAttributes
    {

        public static SymbolAttribute[] Table { get; private set; }

        static ASCIISymbolAttributes()
        {
            uint[] WHITESPACE_CHAR_CODES = new uint[]{
                    32, // whitespace
                    13, // \n
                    10, // \r
                    9,  // \v
                    11, // \t
                    12, // new page
                };           
            var attributesTable = new SymbolAttribute[256];
            foreach (var ws_index in WHITESPACE_CHAR_CODES)
                attributesTable[ws_index] = SymbolAttribute.Whitespace;
            for (var dt_index = 48; dt_index <= 57; dt_index++)
                attributesTable[dt_index] = SymbolAttribute.Digit;
            for (var lt_index = 65; lt_index <= 90; lt_index++)
                attributesTable[lt_index] = SymbolAttribute.Letter;
            /*for (var lt_index = 97; lt_index <= 122; lt_index++)
                attributesTable[lt_index] = SymbolAttribute.Letter;   */         
            Table = attributesTable;
        }
    }
}
