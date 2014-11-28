using My.Labs.Translator.CodeGeneratorNS.Impl;
using My.Labs.Translator.LexerNS;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.GrammarNS
{
    public class LexGrammar
    {

        public SymbolAttributeTable Table { get; set; }

        public static LexGrammar Parse(string rules)
        {
            LexGrammar lex = new LexGrammar();
            var lexGram = Grammar.LexerGrammar;
            var lexer = new TerminalLexer();
            var lexRes = lexer.Parse(rules, lexGram);
            var synRes = (new LL1Analyzer()).Analyze(lexRes.Lexems, lexGram);
            var linearTree = (new SyntaxTree()).BuildLinearTreeForm(synRes.SyntaxTree, lexGram);
            var codeGen = new MetaLexerGenerator();
            codeGen.Generate(linearTree);
            lex.Table = codeGen.Table;
            return lex;
        }
    }
}
