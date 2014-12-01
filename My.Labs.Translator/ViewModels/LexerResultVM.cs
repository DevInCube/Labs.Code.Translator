using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.LexerNS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.ViewModels
{
    public class LexerResultVM : ObservableObject
    {

        public ObservableCollection<ComplexToken> Lexems { get; private set; }
        public Dictionary<string, List<ComplexToken>> Tables { get; private set; }

        public LexerResultVM()
        {
            Lexems = new ObservableCollection<ComplexToken>();
            Tables = new Dictionary<string, List<ComplexToken>>();
        }

        public void Init(ILexerResult result)
        {
            this.Lexems.Clear();
            foreach (var lex in result.Lexems)
                this.Lexems.Add(lex);
            foreach (var tkey in result.Tokens.Keys)
            {
                var token = result.Tokens[tkey];
                var tokenType = token.Token;
                if (!Tables.ContainsKey(tokenType))                                   
                    Tables.Add(tokenType, new List<ComplexToken>());
                Tables[tokenType].Add(token);
            }
        }
    }
}
