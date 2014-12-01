using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public abstract class ALexer 
    {

        private Grammar grammar;
        private int pos, lineCounter, posCounter;
        private string program;
        private StringBuilder buffer;     

        public ALexer()
        {
            this.buffer = new StringBuilder();               
        }

        ILexerResult InitParse(string program, Grammar grammar)
        {
            this.grammar = grammar;
            this.program = program;
            this.pos = 0;
            this.lineCounter = 1;
            this.posCounter = 1;
            ILexerResult result = CreateResult();  
            foreach (var t in grammar.Keywords)
                result.AddToken(t);
            foreach (var t in grammar.Delimiters)
                result.AddToken(t);
            return result;
        }

        protected abstract ILexerResult CreateResult();
        protected abstract void OnParse(ILexerResult result);

        public ILexerResult Parse(string programText, Grammar grammar)
        {               
            if (string.IsNullOrEmpty(programText))
                throw new CodeError(CodeErrorType.Lexical, "Program is empty", 0, 0); 
            if (grammar == null) 
                throw new Exception("Grammar not specified");
            ILexerResult result = InitParse(programText, grammar);            
            OnParse(result);
            return result;            
        }

        protected void Push(Symbol sym)
        {
            buffer.Append(sym.Value);
        }

        protected string Peek()
        {
            return buffer.ToString();
        }

        protected string Pop()
        {
            var buf = buffer.ToString();
            buffer.Clear();
            return buf;
        }

        protected Symbol Gets()
        {
            if (pos == program.Length)
                return Symbol.CreateEOF(lineCounter, posCounter);            
            var value = program[pos++];
            var attr = Attr(value);
            var sym = new Symbol(value, attr, lineCounter, posCounter);            
            posCounter++;
            if (value.Equals('\n'))
            {
                lineCounter++;
                posCounter = 1;
            }
            return sym;
        }

        private SymbolAttribute Attr(char ch)
        {
            if ((int)ch > grammar.AttributeTable.Table.Length)
                return SymbolAttribute.Forbidden;
            return grammar.AttributeTable.Table[(int)ch];
        }  
    }
}
