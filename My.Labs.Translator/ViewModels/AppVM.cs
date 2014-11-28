using My.Labs.Translator.CodeGeneratorNS;
using My.Labs.Translator.CodeGeneratorNS.Impl;
using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.LexerNS;
using My.Labs.Translator.Models;
using My.Labs.Translator.SyntaxParserNS;
using My.Labs.Translator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace My.Labs.Translator.ViewModels
{
    public class AppVM : ObservableObject
    {

        private ProcessType _SelectedProcessType;
        private string _input, _output, _LexerGrammar, _SyntaxGrammar, _GenCode;        
        private ILexerResult lexRes;
        private SyntaxResult synRes;
        private LangTranslator _SelectedGrammar;

        public ObservableCollection<ComplexToken> Keywords { get; private set; }
        public ObservableCollection<ComplexToken> Delimiters { get; private set; }
        public ObservableCollection<ComplexToken> Constants { get; private set; }
        public ObservableCollection<ComplexToken> Identifiers { get; private set; }
        public ObservableCollection<ComplexToken> Lexems { get; private set; }


        public ObservableCollection<LangTranslator> Grammars { get; private set; }
        public LangTranslator SelectedGrammar
        {
            get { return _SelectedGrammar; }
            set { _SelectedGrammar = value; OnPropertyChanged("SelectedGrammar"); }
        }

        public ObservableCollection<ProcessType> ProcessTypes { get; private set; }
        public ProcessType SelectedProcessType
        {
            get { return _SelectedProcessType; }
            set { _SelectedProcessType = value; OnPropertyChanged("SelectedProcessType"); }
        }

        public ICommand EditGrammar { get; private set; }
        public ICommand CreateGrammar { get; private set; }
        public ICommand RemoveGrammar { get; private set; }
        public ICommand Process { get; private set; }
        public ICommand ShowSyntaxTree { get; private set; }        

        public string LexerGrammar
        {
            get { return _LexerGrammar; }
            set { _LexerGrammar = value; OnPropertyChanged("LexerGrammar"); }
        }
        public string SyntaxGrammar
        {
            get { return _SyntaxGrammar; }
            set { _SyntaxGrammar = value; OnPropertyChanged("SyntaxGrammar"); }
        }
        public string GenCode
        {
            get { return _GenCode; }
            set { _GenCode = value; OnPropertyChanged("GenCode"); }
        }
        public string Input
        {
            get { return _input; }
            set { _input = value; OnPropertyChanged("Input"); }
        }
        public string Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged("Output"); }
        }

        public AppVM()
        {

            ProcessTypes = new ObservableCollection<ProcessType>();
            ProcessTypes.Add(ProcessType.Full);
            ProcessTypes.Add(ProcessType.Lexical);
            ProcessTypes.Add(ProcessType.Syntactic);
            SelectedProcessType = ProcessTypes.First();

            Grammars = new ObservableCollection<LangTranslator>();

            LangTranslator signalG = new LangTranslator("SIGNAL");
            Grammar syntax = Grammar.Create(Resource.Lab_LexerGrammar, Resource.Lab_SyntacticGrammar);
            signalG.Init(
                syntax,
                new SignalLexer(syntax),
                new CodeGenerator()
            );
            Grammars.Add(signalG);

            if (this.Grammars.Count > 0)
                this.SelectedGrammar = Grammars.First();

            LexerGrammar = Resource.Grammar;
            SyntaxGrammar = Resource.Grammar2;
            Input = Resource.TestProgram1;            

            Lexems = new ObservableCollection<ComplexToken>();
            Keywords = new ObservableCollection<ComplexToken>();
            Delimiters = new ObservableCollection<ComplexToken>();
            Constants = new ObservableCollection<ComplexToken>();
            Identifiers = new ObservableCollection<ComplexToken>();

            EditGrammar = new SimpleCommand(EditGrammarAction);
            CreateGrammar = new SimpleCommand(CreateGrammarAction);
            RemoveGrammar = new SimpleCommand(RemoveGrammarAction);

            Process = new SimpleCommand(ProcessAction);
            ShowSyntaxTree = new SimpleCommand(ShowSyntaxTreeAction);
        }

        void EditGrammarAction()
        {
            var grWindow = new GrammarWindow();
            var vm = grWindow.DataContext as GrammarVM;
            vm.Init(this.SelectedGrammar);
            var result = grWindow.ShowDialog();
            if (result == true)
            {
                //@todo
            }
        }

        void CreateGrammarAction()
        {
            var grWindow = new GrammarWindow();
            var vm = grWindow.DataContext as GrammarVM;
            var newLangGrammar = new LangTranslator("<NEW>");
            vm.Init(newLangGrammar);
            var result = grWindow.ShowDialog();
            if (result == true)
            {
                this.Grammars.Add(newLangGrammar);
                SelectedGrammar = newLangGrammar;
            }
        }

        void RemoveGrammarAction()
        {
            this.Grammars.Remove(this.SelectedGrammar);
        }

        void ProcessAction()
        {
            var translator = this.SelectedGrammar;
            lexRes = translator.RunLexer(this.Input);            
            if (SelectedProcessType == ProcessType.Syntactic
                || SelectedProcessType == ProcessType.Full)
            {
                synRes = translator.RunSyntactic(lexRes);             
                if (SelectedProcessType == ProcessType.Full)
                {
                    var generator = translator.RunGenerator(synRes);
                    this.Output = translator.Generator.GetPlainResult();
                }
            }            
        }

        void ShowSyntaxTreeAction()
        {
            var win = new SyntaxTreeWindow();
            var vm = win.DataContext as SyntaxTreeVM;
            vm.Init(synRes.SyntaxTree);
            win.Show();
        }

        /*
        void LexerParseAction()
        {
            var attributesTable = ASCIISymbolAttributes.Table;
            var delimitersList = new char[] {
                    '(', ')', ':', ';', '=', '-'
                };
            foreach (var del in delimitersList)
                attributesTable[del] = SymbolAttribute.Delimiter;
            var keywordsList = new string[] {
                    "PROCEDURE",
                    "CONST",
                    "BEGIN",
                    "END",
                    "INTEGER",
                    "FLOAT"
                };            
            //@todo grammar
            var grammar = Grammar.Parse(LexerGrammar, null, 0);            
            //@todo

            var lexer = new Lexer(grammar);
            try
            {
                lexRes = lexer.Parse(this.Input);
                if (lexRes == null)
                    throw new Exception("Lexer failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name.ToString());
                return;
            }
            this.Lexems.Clear();
            foreach (var item in lexRes.Lexems)
                this.Lexems.Add(item);
            this.Keywords.Clear();
            foreach (var item in lexRes.GetKeywords())
                this.Keywords.Add(item);
            this.Delimiters.Clear();
            foreach (var item in lexRes.GetDelimiters())
                this.Delimiters.Add(item);
            this.Constants.Clear();
            foreach (var item in lexRes.GetConstants())
                this.Constants.Add(item);
            this.Identifiers.Clear();
            foreach (var item in lexRes.GetIdentifiers())
                this.Identifiers.Add(item);
        }      
        */
        void GenerateCodeAction()
        {
            var codeGen = new CodeGeneratorNS.CodeGenerator();
            var genResult = codeGen.Generate(synRes);
            this.GenCode = genResult.Program;
            var window = new CodeGeneratorWindow();
            window.DataContext = this;
            window.Show();
        }

        void PrepareGrammarAction()
        {
            /*
            Grammar metaGrammar = Grammar.SyntaxGrammar;
            var lexer = new BNFLexer();
            var lexRes = lexer.Parse(Resource.MetaLexGrammar, metaGrammar);
            var synRes = (new LL1Analyzer()).Analyze(lexRes.Lexems, metaGrammar);
            var linearTree = (new SyntaxTree()).BuildLinearTreeForm(synRes, metaGrammar);
            var gramGen = new GrammarRulesGenerator();
            gramGen.Generate(linearTree);
            List<Rule> rules = gramGen.Rules;
            var sb = new StringBuilder();
            foreach (var rule in rules)
                sb.Append(rule.ToString() + "\r\n");
            MessageBox.Show(sb.ToString());*/
            var lexGram = Grammar.LexerGrammar;
            var lexer = new TerminalLexer();
            var lexRes = lexer.Parse(Resource.Lab_LexerGrammar, lexGram);
            var synRes = (new LL1Analyzer()).Analyze(lexRes.Lexems, lexGram);
            var linearTree = (new SyntaxTree()).BuildLinearTreeForm(synRes.SyntaxTree, lexGram);
            var codeGen = new MetaLexerGenerator();
            codeGen.Generate(linearTree);
            MessageBox.Show(codeGen.Result);
            return;
            /*
            Grammar grammar = Grammar.BNFGrammar;
            //MessageBox.Show(grammar.ToString());
            var lexer = new BNFLexer();
            var lexRes = lexer.Parse(Resource.TestGrammar2, grammar);
            var syntaxParser = new LL1Analyzer();
            var synRes = syntaxParser.Analyze(lexRes.Lexems, grammar);
            ShowSyntaxTree(synRes);
            var RAS = (new SyntaxTree()).BuildLinearTreeForm(synRes, grammar);
            
            var codeGen = new GrammarRulesGenerator();
            codeGen.Generate(RAS);
            var sb = new StringBuilder();
            foreach (var rule in codeGen.Rules)
                sb.Append(rule.ToString()+"\r\n");
            MessageBox.Show(sb.ToString());
            */
            /*
           
            /*
            var metaSemGramText = Resource.MetaSemanticGrammar;           
            Grammar metaSemanticGrammar = Grammar.Parse(metaSemGramText, tokenCheckers);
            string metaSemTestString = @"{[2] [1]@Test_Buf}";
            var lexer = new Lexer(metaSemanticGrammar);
            var lRes = lexer.Parse(metaSemTestString);
            var syntaxParser = new LL1Analyzer(metaSemanticGrammar);
            var sRes = syntaxParser.Analyze(lRes.Lexems);*/
            return; //@test
            // var dialog = new GrammarWindow();
            // var result = dialog.ShowDialog();
        }       
    }

    public enum ProcessType
    {
        Full,
        Lexical,
        Syntactic
    }
}
