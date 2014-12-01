using My.Labs.Translator.CodeGeneratorNS;
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
    public class SyntacticGrammar
    {

        public string Plain { get; private set; }
        public LexGrammar LexGrammar { get; private set; }
        public SymbolAttributeTable Table { get; private set; }
        public List<Rule> Rules { get; private set; }

        public static SyntacticGrammar Parse(string syntacticRules, Grammar grammar, LexGrammar lexGram)
        {
            SyntacticGrammar synGram = new SyntacticGrammar();
            var lexer = new TerminalLexer();
            var lexRes = lexer.Parse(syntacticRules, grammar);
            var synRes = (new LL1Analyzer()).Analyze(lexRes.Lexems, grammar);
            var linearTree = (new SyntaxTree()).BuildLinearTreeForm(synRes.SyntaxTree, grammar);
            var codeGen = new GrammarRulesGenerator();
            codeGen.Generate(linearTree);
            synGram.Plain = syntacticRules;
            synGram.LexGrammar = lexGram;
            synGram.Rules = codeGen.Rules;            
            synGram.Table = lexGram.Table;
            return synGram;
        }
        


    }
}
