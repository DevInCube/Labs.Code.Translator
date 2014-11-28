using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class LexerException : Exception
    {

        public LexerException(string msg)
            : base(msg)
        {
            //
        }
    }
}
