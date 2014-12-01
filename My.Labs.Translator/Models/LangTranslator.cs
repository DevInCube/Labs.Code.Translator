using My.Labs.Translator.CodeGeneratorNS;
using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.LexerNS;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.Models
{
    public class LangTranslator
    {

        public string Name { get; set; }

        public Grammar Syntax { get; private set; }

        public ALexer Lexer { get; private set; }
        public LL1Analyzer SyntaxAnalyzer { get; private set; }
        public ACodeGenerator Generator { get; private set; }

        public LangTranslator(string name)
        {
            this.Name = name;
            SyntaxAnalyzer = new LL1Analyzer();
        }

        public void Init(            
            Grammar SyntaxGrammar,
            ALexer Lexer,
            ACodeGenerator Generator)
        {            
            this.Lexer = Lexer;
            this.Generator = Generator;
            this.Syntax = SyntaxGrammar;
        }

        public ILexerResult RunLexer(string input)
        {
            return Lexer.Parse(input, Syntax);
        }

        public SyntaxResult RunSyntactic(ILexerResult lexRes)
        {
            return SyntaxAnalyzer.Analyze(lexRes.Lexems, Syntax);
        }

        public ACodeGenerator RunGenerator(SyntaxResult synRes)
        {
            List<SyntaxTreeRule> linearTreeForm = (new SyntaxTree()).BuildLinearTreeForm(synRes.SyntaxTree, Syntax);
            Generator.Generate(linearTreeForm);
            return Generator;
        }
    }
}
