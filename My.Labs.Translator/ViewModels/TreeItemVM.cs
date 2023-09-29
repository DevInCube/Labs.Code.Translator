using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.ViewModels
{
    public class TreeItemVM : ObservableObject
    {

        private bool _IsExpanded;
        private SyntaxParserNS.SyntaxTreeNode node;

        public List<TreeItemVM> Children { get; private set; }    

        public string Lexem { get { return node.ComplexToken.Lexem; } }
        public string Token { get { return node.ComplexToken.Token; } }
        public string Key { get { return (node.ComplexToken.Key != 0) ? node.ComplexToken.Key.ToString() : ""; } }
        public string Rule { get { return (node.RuleIndex != 0) ? node.RuleIndex.ToString() : ""; } }

        public bool IsExpanded { 
            get { return _IsExpanded; }
            set { _IsExpanded = value; OnPropertyChanged(); }
        }

        public TreeItemVM(SyntaxParserNS.SyntaxTreeNode node)
        {
            this.node = node;
            this.Children = new List<TreeItemVM>();
            foreach (var ch in node.Children)
            {
                this.Children.Add(new TreeItemVM(ch));
            }
        }

        internal void ExpandAll()
        {
            this.IsExpanded = true;
            foreach (var ch in Children)
                ch.ExpandAll();
        }
    }
}
