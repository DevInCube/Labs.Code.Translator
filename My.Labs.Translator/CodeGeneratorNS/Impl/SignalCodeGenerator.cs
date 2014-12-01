using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.Models;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.CodeGeneratorNS
{
    public class SignalCodeGenerator : ACodeGenerator
    {

        private string attribute;
        private double constant = double.NaN;
        private string identifier;
        public StringBuilder result;
        private List<string> Identifiers;

        public SignalCodeGenerator()
        {
            result = new StringBuilder();
            Identifiers = new List<string>();
        }

        protected override void OnInit()
        {
            result.Clear();
            Identifiers.Clear();
        }

        protected override void OnRule(int index, int iterator, SyntaxTreeNode currNode)
        {
            int k1 = iterator + 1, k2 = iterator + 2, k3 = iterator + 3, k4 = iterator + 4;
            //throw new NotImplementedException();
            switch (index)
            {
                case (4):
                case (6):
                case (8):
                case (14):
                case (16):
                    return;
                case (1):
                case (3):
                case (5):
                case (12):
                case (13):
                    RunRule(k1);
                    break;
                case (7):               
                case (15):                
                    RunRule(k2);
                    RunRule(k1);
                    break;
                case (9):
                    {
                        RunRule(k2);
                        RunRule(k1);
                        string datatype = null;
                        switch (attribute)
                        {
                            case ("INTEGER"): datatype = "dd"; break;
                            case ("FLOAT"): datatype = "dq"; break;
                        }
                        result.Append(string.Format("\r\n\t{0} {1} ?", identifier, datatype));
                        result.Append(string.Format("\r\n\tPOP {0}", identifier));
                        break;
                    }
                case (17):
                    {
                        RunRule(k2);
                        RunRule(k1);
                        result.Append(string.Format("\r\n\t{0} dw {1}", identifier, constant));
                        break;
                    }
                case (2):
                    RunRule(k4);
                    result.Append(string.Format("{0}:", identifier));
                    RunRule(k3);
                    RunRule(k2);                    
                    RunRule(k1);
                    result.Append(string.Format("\r\nret"));
                    break;
                case(10):
                    attribute = "INTEGER";
                    break;
                case(11):
                    attribute = "FLOAT";
                    break;
                case (18):
                    constant = double.Parse(currNode.Children.First().ComplexToken.Lexem);
                    break;
                case (19):
                    constant = - double.Parse(currNode.Children.Last().ComplexToken.Lexem);
                    break;
                case (20):
                case (21):
                case (22):
                    {
                        ComplexToken token = currNode.Children.First().ComplexToken;
                        identifier = token.Lexem;
                        if (!Identifiers.Contains(identifier))
                            Identifiers.Add(identifier);
                        else
                        {
                            var msg = string.Format("Identifier '{0}' was already initiliazed", identifier);
                            throw new CodeError(CodeErrorType.Semantic, msg, token.CodeLine, token.CodePosition);
                        }                                
                        break;
                    }
                default: throw new Exception("Generate rule " + index);
            }
        }

        protected override void OnExit()
        {
            //throw new NotImplementedException();
        }

        public override string GetPlainResult()
        {
            return result.ToString();
        }
    }


}
