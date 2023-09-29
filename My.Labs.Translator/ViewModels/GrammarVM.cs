using My.Labs.Translator.Commands;
using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.Models;
using My.Labs.Translator.Properties;
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
    public class GrammarVM : ObservableObject
    {

        private string _PlainGrammar, _LexGrammar, _SyntacticGrammar, _Error;


        public string LexGrammar
        {
            get { return _LexGrammar; }
            set { _LexGrammar = value; OnPropertyChanged(); }
        }
        public string SyntacticGrammar
        {
            get { return _SyntacticGrammar; }
            set { _SyntacticGrammar = value; OnPropertyChanged(); }
        }
        public string PlainGrammar
        {
            get { return _PlainGrammar; }
            set { _PlainGrammar = value; OnPropertyChanged(); }
        }
        public string Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged(); }
        }

        public Grammar Grammar { get; private set; }
        public ObservableCollection<Rule> Rules { get; private set; }
        public ObservableCollection<string> FirstSet { get; private set; }
        public ObservableCollection<string> FollowSet { get; private set; }

        public ObservableCollection<SyntaxTreeNode> TreeItems { get; private set; }

        public ICommand Generate { get; set; }
        public ICommand Test { get; set; }
        public ICommand ShowTable { get; set; }
        public ICommand CloseOK { get; set; }

        public GrammarVM()
        {
            CloseOK = new RelayCommand((obj) => {
                Window window = obj as Window;
                window.DialogResult = true;
                window.Close();
            });
            /*
            TreeItems = new ObservableCollection<SyntaxTreeNode>();
            Rules = new ObservableCollection<Rule>();
            FirstSet = new ObservableCollection<string>();
            FollowSet = new ObservableCollection<string>();
            Test = new SimpleCommand(() => {
                var analyzer = new LL1Analyzer();
                string[] strs = "PROCEDURE A ; BEGIN END ;".Split(' ');
                var tokens = new List<ComplexToken>();
                //foreach (var str in strs)
                    //tokens.Add(new ComplexToken(str, TokenType.NonTerminal, 0, 0));
                TreeItems.Add(analyzer.Analyze(tokens, Grammar));
                var dialog = new SyntaxTreeWindow();
                dialog.DataContext = this;
                dialog.ShowDialog();
            });
            ShowTable = new SimpleCommand(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" .\t");
                for (var i = 0; i < Grammar.Terminals.Count; i++)
                    sb.Append(Grammar.Terminals[i] + ".\t");
                for (var i = 0; i < Grammar.NonTerminals.Count; i++)
                {
                    sb.Append("\r\n" + Grammar.NonTerminals[i] + ".\t");
                    for (var j = 0; j < Grammar.Terminals.Count; j++)
                    {
                        var rule = Grammar.LLTable[i, j];
                        if (rule != null)
                            sb.Append(Grammar.Rules.IndexOf(rule) + ".\t");
                        else
                            sb.Append(" .\t");
                    }
                }
                MessageBox.Show(sb.ToString());
            });
            Generate = new SimpleCommand(() => {
                Rules.Clear();
                FirstSet.Clear();
                FollowSet.Clear();
                var plainGrammar = this.PlainGrammar;
                try
                {
                    this.Grammar = Grammar.Parse(plainGrammar);
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    return;
                }
                foreach (var rule in Grammar.Rules)
                    Rules.Add(rule);
                foreach (var nonTerm in Grammar.NonTerminals)
                {
                    var str = string.Format("{0} : ", nonTerm.ToString());
                    foreach (var tok in Grammar.FIRST(nonTerm))
                        str += tok + ", ";
                    FirstSet.Add(str);
                    str = string.Format("{0} : ", nonTerm.ToString());                   
                }                
            });
            PlainGrammar = Resource.TestGrammar1;
            */
        }

        public void Init(LangTranslator translator)
        {
            //@todo
            Grammar grammar = translator.Syntax;
            LexGrammar = grammar.SyntacticGrammar.LexGrammar.Plain;
            SyntacticGrammar = grammar.SyntacticGrammar.Plain;
        }
    }
}
