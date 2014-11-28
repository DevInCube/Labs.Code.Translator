using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class TerminalLexer : ALexer
    {

        public TerminalLexer()
        {
            //
        }

        protected override void OnParse(ILexerResult result)
        {
            Symbol sym = this.Gets();
            do
            {
                switch (sym.Attribute)
                {
                    case (SymbolAttribute.Forbidden):
                        throw new LexerException(string.Format("Forbidden symbol '{0}' at line {1} position {2}",
                            sym.Value, sym.CodeLine, sym.CodePosition));
                    case (SymbolAttribute.Whitespace):
                        {
                            do
                            {
                                sym = Gets();
                            } while (!(sym.IsEOF()) && sym.Attribute == SymbolAttribute.Whitespace);
                            break;
                        }
                    case (SymbolAttribute.Digit):
                        {
                            do
                            {
                                Push(sym);
                                sym = Gets();
                            } while (!(sym.IsEOF()) && sym.Attribute == SymbolAttribute.Digit);
                            var lexem = Pop();
                            var token = result.GetToken(lexem);
                            if (token == null)
                                token = result.AddToken(lexem, "terminal", sym.CodeLine, sym.CodePosition - lexem.Length);
                            result.AddLexem(token);       
                            break;
                        }
                    case (SymbolAttribute.Letter):{
                        do
                        {
                            Push(sym);
                            sym = Gets();
                        } while (!(sym.IsEOF()) && (sym.Attribute == SymbolAttribute.Digit ||
                                    sym.Attribute == SymbolAttribute.Letter));
                        var lexem = Pop();
                        var token = result.GetToken(lexem);
                        if (token == null)
                            token = result.AddToken(lexem, "non-terminal", sym.CodeLine, sym.CodePosition - lexem.Length);
                        result.AddLexem(token);                        
                        break;
                    }
                    case (SymbolAttribute.Delimiter):
                        {
                            if (sym.Value.Equals('"')
                                || sym.Value.Equals('\''))
                            {
                                Symbol startSym = sym;
                                sym = ScanString(sym);
                                string value = Pop();
                                var token = result.GetToken(value);
                                if (token == null)
                                    token = result.AddToken(value, "terminal", startSym.CodeLine, startSym.CodePosition);
                                result.AddLexem(token);
                                break;
                            }
                            else
                            {
                                Symbol startSym = sym;
                                sym = ScanDelimiter(sym);
                                string value = Pop();
                                var token = result.GetToken(value);
                                if (token == null)
                                    token = result.AddToken(value, "delimiter", startSym.CodeLine, startSym.CodePosition);
                                result.AddLexem(token);
                                break;
                            }                            
                        }                    
                }                
            } while (!sym.IsEOF());
        }

        private Symbol ScanDelimiter(Symbol startSym)
        {
            Push(startSym);
            Symbol sym = Gets();
            return sym;
        }

        private Symbol ScanString(Symbol startSym)
        {
            Push(startSym);
            Symbol sym = Gets();
            bool finish = false;
            while (!finish && !sym.IsEOF() && !(sym.Attribute == SymbolAttribute.Forbidden)) 
            {
                Push(sym);
                if (sym.Value.Equals(startSym.Value))                   
                    finish = true;                                    
                //@todo escape in strings
                sym = Gets();
            } 
            return sym;
        }

        protected override ILexerResult CreateResult()
        {
            return new TerminalLexerResult();
        }
    }
}
