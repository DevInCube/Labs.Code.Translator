using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.GrammarNS
{
    public class ComplexToken
    {

        public static readonly ComplexToken Empty = new ComplexToken("", "empty");
        public static readonly ComplexToken Alternation = new ComplexToken("|", "delimiter");
        public static readonly ComplexToken Definition = new ComplexToken("::=", "delimiter");

        public int Key { get; set; }
        public string Lexem { get; set; }
        public string Token { get; set; }
        public int CodeLine { get; set; }
        public int CodePosition { get; set; }
     

        public ComplexToken(
            string val, 
            string tt, 
            int line = 0, 
            int pos = 0)
        {
            this.Lexem = val.Trim();
            this.Token = tt;
            this.CodeLine = line;
            this.CodePosition = pos;
        }
     
        public override string ToString()
        {            
            return string.Format("\"{0}\" ({1})", Lexem, Token);
        }
        
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is ComplexToken)) return false;
            ComplexToken other = obj as ComplexToken;           
            return this.Lexem.Equals(other.Lexem);
        }        

        public override int GetHashCode()
        {
            return this.Lexem.GetHashCode();
        }

        public static ComplexToken CreateNonTerminal(string val)
        {
            return new ComplexToken(val, ""); //@todo
        }
    }
}
