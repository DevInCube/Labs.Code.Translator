using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.LexerNS;
using My.Labs.Translator.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace My.Labs.Translator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Main(object sender, StartupEventArgs e)
        {
            var app = new MainWindow();
            app.Show();
            return;
            /*          
            var grammarStr = Resource.Grammar;
            var grammar = Grammar.Parse(grammarStr);           
            //MessageBox.Show(grammar.ToString());
            StringBuilder sb2 = new StringBuilder();
            foreach (var nonTerm in grammar.NonTerminals)
            {
                sb2.Append("\r\n FOLLOW " + nonTerm.ToString() + " : ");
                foreach (var tok in grammar.FOLLOW(nonTerm))
                    sb2.Append(tok).Append(",");
            }

            //MessageBox.Show(sb2.ToString());
            var lexer = new Lexer();
            var delimitersList = new char[] {
                '(', ')', ':', ';', '=', '-', '*'
            };
            var attributesTable = new SymbolAttribute[256];
            foreach (var ws_index in Lexer.WHITESPACE_CHAR_CODES)
                attributesTable[ws_index] = SymbolAttribute.Whitespace;
            for (var dt_index = 48; dt_index <= 57; dt_index++)
                attributesTable[dt_index] = SymbolAttribute.Digit;
            for( var lt_index = 65; lt_index <= 90; lt_index ++ )
                attributesTable[lt_index] = SymbolAttribute.Letter;
            for (var lt_index = 97; lt_index <= 122; lt_index++)
                attributesTable[lt_index] = SymbolAttribute.Letter;
            foreach(var del in delimitersList)
                    attributesTable[del] = SymbolAttribute.Delimiter;
            var keywordsList = new string[] {
                "PROCEDURE",
                "CONST",
                "BEGIN",
                "END",
                "INTEGER",
                "FLOAT"
            };
            lexer.InitAttributes(attributesTable);
            lexer.InitCharDelimiters(delimitersList);
            lexer.InitKeywords(keywordsList);
            var program = Resource.TestProgram1;

            var appWindow = new MainWindow();
            var vm = appWindow.DataContext as AppVM;
            vm.Input = program;
            vm.Parse = new SimpleCommand(() =>
            {
                lexer.Parse(vm.Input);
                var sb = new StringBuilder();
                foreach (var lex in lexer.GetLexems())
                    sb.Append(lex.ToString() + ", ");
                sb.Append("\r\n");
                sb.Append("\r\n KEYWORDS: \r\n");
                sb.Append(lexer.GetKeywordsTable());
                sb.Append("\r\n IDENTIFIERS: \r\n");
                sb.Append(lexer.GetIdentifiersTable());
                sb.Append("\r\n CONSTANTS: \r\n");
                sb.Append(lexer.GetConstantsTable());
                sb.Append("\r\n DELIMITERS: \r\n");
                sb.Append(lexer.GetDelimitersTable());
                vm.Output = sb.ToString();
            });
            appWindow.Show();
             */
        }
    }
}
