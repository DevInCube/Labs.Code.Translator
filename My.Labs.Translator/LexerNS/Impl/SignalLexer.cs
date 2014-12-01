using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.LexerNS
{
    public class SignalLexer : ALexer
    {

        private Grammar grammar;

        public SignalLexer(Grammar grammar)
        {
            this.grammar = grammar;            
        }

        protected override ILexerResult CreateResult()
        {
            var result = new SignalLexerResult();          
            return result;
        }

        protected override void OnParse(ILexerResult ires)
        {
            SignalLexerResult result = (SignalLexerResult)ires;
            Symbol sym = Gets();
            do
            {
                switch (sym.Attribute)
                {
                    case (SymbolAttribute.Forbidden):
                        throw new CodeError(CodeErrorType.Lexical, string.Format("Forbidden symbol '{0}'",
                            sym.Value), sym.CodeLine, sym.CodePosition);                       
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
                            int index;
                            var value = Pop();
                            bool hasConstant = result.HasConstant(value, out index);
                            var constantToken = new ComplexToken(value, "unsigned-integer", sym.CodeLine, sym.CodePosition - value.Length);
                            result.AddToken(constantToken);
                            if (!hasConstant)
                                result.AddLexem(constantToken);                            
                            break;
                        }
                    case (SymbolAttribute.Letter):
                        {
                            do
                            {
                                Push(sym);
                                sym = Gets();
                            } while (!(sym.IsEOF()) && (sym.Attribute == SymbolAttribute.Digit ||
                                    sym.Attribute == SymbolAttribute.Letter));
                            int index;
                            string value = Pop();
                            bool hasKeyword = result.HasKeyword(value, out index);
                            ComplexToken token;
                            if (hasKeyword)
                            {                                
                                token = new ComplexToken(value, "keyword", sym.CodeLine, sym.CodePosition - value.Length);
                                token.Key = index;
                            }
                            else
                            {
                                bool hasIdentifier = result.GetIdentifier(value, out index);                                
                                token = new ComplexToken(value, "identifier", sym.CodeLine, sym.CodePosition - value.Length);
                                if (!hasIdentifier)
                                    result.AddToken(token);
                            }
                            result.AddLexem(token);                            
                            break;
                        }
                    case (SymbolAttribute.Delimiter):
                        {
                            if (sym.Value == '(')
                            {
                                var nextSym = Gets();
                                if (nextSym.Value == '*')
                                {
                                    Symbol commentOpenSymbol = nextSym;
                                    sym = Gets();
                                    do
                                    {
                                        if (sym.IsEOF())
                                        {
                                            var msg = string.Format("Comment not closed");
                                            throw new CodeError(CodeErrorType.Lexical, msg, commentOpenSymbol.CodeLine, commentOpenSymbol.CodePosition);
                                        }
                                        if (sym.Value == '*')
                                        {
                                            nextSym = Gets();
                                            if (nextSym.Value == ')')
                                                break;
                                            else
                                                sym = nextSym;
                                        }
                                        else
                                            sym = Gets();
                                    } while (true);
                                    sym = Gets();
                                    break;
                                }
                                else
                                {
                                    var delToken = result.GetDelimiter(sym.Value);
                                    if (delToken == null)
                                    {
                                        var msg = string.Format("Delimiter not recognized '{0}'", sym.Value);
                                        throw new CodeError(CodeErrorType.Lexical, msg, sym.CodeLine, sym.CodePosition);
                                    }                                        
                                    delToken.CodeLine = sym.CodeLine;
                                    delToken.CodePosition = sym.CodePosition;
                                    result.AddLexem(delToken);
                                    sym = nextSym;
                                    break;
                                }
                            }
                            var delToken2 = result.GetDelimiter(sym.Value);
                            if (delToken2 == null)
                            {
                                var msg = string.Format("Delimiter not recognized '{0}'", sym.Value);
                                throw new CodeError(CodeErrorType.Lexical, msg, sym.CodeLine, sym.CodePosition);
                            }                                   
                            delToken2.CodeLine = sym.CodeLine;
                            delToken2.CodePosition = sym.CodePosition;
                            result.AddLexem(delToken2);
                            sym = Gets();
                            break;
                        }
                }
            } while (!sym.IsEOF());
        }       
    }
    
}
