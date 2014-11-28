using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.LexerNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.CodeGeneratorNS.Impl
{
    public class MetaLexerGenerator : ACodeGenerator
    {

        private string AttrType;        

        public string Result { get; private set; }
        public SymbolAttributeTable Table { get; private set; }

        protected override void OnInit()
        {
            Result = "";
            Table = new SymbolAttributeTable();
            AttrType = null;
        }

        protected override void OnRule(int index, int iterator, SyntaxParserNS.SyntaxTreeNode currNode)
        {
            int k = iterator;
            int k1 = k + 1;
            int k2 = k + 2;
            switch (index)
            {
                case (3):
                case (8):
                    return;
                case (1):
                case (7):
                    {
                        RunRule(k1);
                        break;
                    }
                case (2):
                case (4):
                case (6):
                    {
                        RunRule(k2);
                        RunRule(k1);
                        break;
                    }
                case (5):
                    {
                        AttrType = currNode.Children.First().ComplexToken.Lexem;
                        break;
                    }
                case (9):
                case (10):
                    {
                        if (AttrType == null)
                            throw new Exception("ArrType not specified yet");                        
                        SymbolAttribute attrType = SymbolAttribute.Forbidden;                       
                        switch (AttrType)
                        {
                            case ("whitespace"):
                            case ("ws"):
                                {
                                    attrType = SymbolAttribute.Whitespace;
                                    break;
                                }
                            case ("digit"):
                            case ("dg"):
                                {
                                    attrType = SymbolAttribute.Digit;

                                    break;
                                }
                            case ("letter"):
                            case ("lt"):
                                {
                                    attrType = SymbolAttribute.Letter;

                                    break;
                                }
                            case ("delimiter"):
                            case ("dt"):
                                {
                                    attrType = SymbolAttribute.Delimiter;
                                    break;
                                }
                            default: throw new Exception("Unspecified type: "+AttrType);
                        }
                        var ct = currNode.Children.First().ComplexToken;
                        if (ct.Token.Equals("non-terminal"))
                        {
                            switch (ct.Lexem)
                            {
                                case ("DG"):
                                    {
                                        for (var dt_index = 48; dt_index <= 57; dt_index++)
                                            Table.Table[dt_index] = attrType;
                                        break;
                                    }
                                case ("LC"): //lowercase
                                    {
                                        for (var lt_index = 97; lt_index <= 122; lt_index++)
                                            Table.Table[lt_index] = attrType;
                                        break;
                                    }
                                case ("UC"): //uppercase
                                    {
                                        for (var lt_index = 65; lt_index <= 90; lt_index++)
                                            Table.Table[lt_index] = attrType;
                                        break;
                                    }
                            }
                            break;
                        }
                        var lexem = ct.Lexem;
                        uint digit;
                        bool isDigit = uint.TryParse(lexem, out digit);
                        if (isDigit)
                        {
                            Table.Table[digit] = attrType;
                        }
                        else
                        {
                            foreach (var ch in lexem)
                                Table.Table[(uint)ch] = attrType;
                        }
                        break;
                    }
                default: throw new Exception("unknown rule");                
            }
        }

        protected override void OnExit()
        {
            //throw new NotImplementedException();
        }

        public override string GetPlainResult()
        {
            return Result;
        }
    }
}
