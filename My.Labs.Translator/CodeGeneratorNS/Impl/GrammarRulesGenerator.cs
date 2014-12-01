using My.Labs.Translator.CodeGeneratorNS;
using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.CodeGeneratorNS
{
    public class GrammarRulesGenerator : ACodeGenerator
    {

        private List<ComplexToken> tokens;
        private ComplexToken mainToken;

        public List<Rule> Rules { get; private set; }

        public GrammarRulesGenerator()
        {
            tokens = new List<ComplexToken>();
        }

        protected override void OnInit()
        {
            Rules = new List<Rule>();   
        }

        protected override void OnRule(int index, int iterator, SyntaxTreeNode currentNode)
        {
            int k1 = iterator + 1;
            int k2 = iterator + 2;
            switch (index)
            {
                case (4):
                case (8):
                case (12):
                    return; // empty
                case (1):
                case (3):
                case (11):
                    {
                        RunRule(k1);
                        break;
                    }
                case (7):
                    {
                        Rule rule = new Rule(mainToken, tokens.ToArray());
                        tokens.Clear();
                        Rules.Add(rule);
                        RunRule(k1);
                        break;
                    }
                case (2):
                case (6):
                case (10):
                    {
                        RunRule(k2);
                        RunRule(k1);
                        break;
                    }
                case (5):
                    {
                        RunRule(k2);
                        RunRule(k1);
                        Rule rule = new Rule(mainToken, tokens.ToArray());
                        tokens.Clear();
                        Rules.Add(rule);
                        break;
                    }
                case (9):
                    {
                        mainToken = currentNode.Children.First().ComplexToken;
                        break;
                    }
                case (13):
                case (14):
                    {
                        var token = currentNode.Children.First().ComplexToken;
                        var lexem = token.Lexem;
                        bool inQuotes = lexem.Length > 1
                                        && lexem.First() == lexem.Last()
                                        && (lexem.First().Equals('\'') || lexem.First().Equals('\"'));
                        if(inQuotes)
                            token.Lexem = lexem.Substring(1, lexem.Length - 2);
                        tokens.Add(token);
                        break;
                    }
                default: throw new Exception("No rule with such index : " + index);
            }
        }

        protected override void OnExit()
        {
            //throw new NotImplementedException();
        }

        public override string GetPlainResult()
        {
            return this.ToString();
        }
    }
}
