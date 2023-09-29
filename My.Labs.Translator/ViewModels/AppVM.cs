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
        private string _input, _output;        
        private ILexerResult lexRes;
        private SyntaxResult synRes;
        private LangTranslator _SelectedGrammar;

        public bool HasErrors { get { return Errors.Count > 0; } }
        public ObservableCollection<CodeError> Errors { get; private set; }
        public bool HasLexerResult { get { return lexRes != null; } }
        public ILexerResult LexerResult
        {
            get { return lexRes; }
            set
            {
                lexRes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLexerResult));

            }
        }
        public bool HasSyntaxResult { get { return synRes != null; } }
        public SyntaxResult SyntaxResult
        {
            get { return synRes; }
            set
            {
                synRes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSyntaxResult));

            }
        }

        public ObservableCollection<LangTranslator> Grammars { get; private set; }
        public LangTranslator SelectedGrammar
        {
            get { return _SelectedGrammar; }
            set { _SelectedGrammar = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProcessType> ProcessTypes { get; private set; }
        public ProcessType SelectedProcessType
        {
            get { return _SelectedProcessType; }
            set { _SelectedProcessType = value; OnPropertyChanged(); }
        }

        public ICommand EditGrammar { get; private set; }
        public ICommand CreateGrammar { get; private set; }
        public ICommand RemoveGrammar { get; private set; }
        public ICommand Process { get; private set; }
        public ICommand ShowLexerResult { get; private set; }
        public ICommand ShowSyntaxTree { get; private set; }        
       
        public string Input
        {
            get { return _input; }
            set { _input = value; OnPropertyChanged(); }
        }

        public bool HasOutput { get { return _output != null; } }

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOutput));
            }
        }

        public AppVM()
        {
            Errors = new ObservableCollection<CodeError>();

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
                new SignalCodeGenerator()
            );
            Grammars.Add(signalG);

            if (this.Grammars.Count > 0)
                this.SelectedGrammar = Grammars.First();

            Input = Resource.TestProgram1;                       

            EditGrammar = new SimpleCommand(EditGrammarAction);
            CreateGrammar = new SimpleCommand(CreateGrammarAction);
            RemoveGrammar = new SimpleCommand(RemoveGrammarAction);

            Process = new SimpleCommand(ProcessAction);
            ShowLexerResult = new SimpleCommand(ShowLexerResultAction);
            ShowSyntaxTree = new SimpleCommand(ShowSyntaxTreeAction);

            this.PropertyChanged += AppVM_PropertyChanged;
        }

        void AppVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Input)))
            {
                ProcessAction();
            }
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
            Errors.Clear();
            OnPropertyChanged(nameof(HasErrors));
            LexerResult = null;
            SyntaxResult = null;
            Output = null;
            try
            {
                var translator = this.SelectedGrammar;
                LexerResult = translator.RunLexer(this.Input);
                if (SelectedProcessType == ProcessType.Syntactic
                    || SelectedProcessType == ProcessType.Full)
                {
                    SyntaxResult = translator.RunSyntactic(lexRes);                    
                    if (SelectedProcessType == ProcessType.Full)
                    {
                        var generator = translator.RunGenerator(synRes);
                        Output = translator.Generator.GetPlainResult();
                    }
                }
            }
            catch (CodeError ex)
            {               
                Errors.Add(ex);
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        void ShowLexerResultAction()
        {
            var win = new LexerResultWindow();
            var vm = win.DataContext as LexerResultVM;
            vm.Init(lexRes);
            win.Show();
        }

        void ShowSyntaxTreeAction()
        {
            var win = new SyntaxTreeWindow();
            var vm = win.DataContext as SyntaxTreeVM;
            vm.Init(synRes.SyntaxTree);
            win.Show();
        }                  
    }

    public enum ProcessType
    {
        Full,
        Lexical,
        Syntactic
    }
}
