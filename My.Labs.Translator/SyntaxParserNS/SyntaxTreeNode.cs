using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.SyntaxParserNS
{
    public class SyntaxTreeNode
    {

        public ComplexToken ComplexToken { get; set; }
        public SyntaxTreeNode Parent { get; private set; }
        public List<SyntaxTreeNode> Children { get; private set; }        

        public int RuleIndex { get; private set; }        

        public SyntaxTreeNode(ComplexToken token)
        {
            this.ComplexToken = token;            
            Children = new List<SyntaxTreeNode>();
        }

        public void SetRuleIndex(int indeX)
        {
            this.RuleIndex = indeX;
        }

        internal void SetChildren(List<ComplexToken> list)
        {            
            foreach (var t in list)
                Children.Add(new SyntaxTreeNode(t) { Parent = this });
        }

        public void AddChildren(List<SyntaxTreeNode> list)
        {
            foreach (var t in list)
            {
                t.Parent = this;
                Children.Add(t);
            }
        }

        public bool HasChildren()
        {
            return Children.Count != 0;
        }

        public override string ToString()
        {
            //return string.Format("Rule {0} : {1}", RuleIndex, Children.Count);
            return string.Format("{0}", ComplexToken);
        }

        internal void ReplaceChild(SyntaxTreeNode what, SyntaxTreeNode with)
        {
            this.Children[Children.IndexOf(what)] = with;
            with.Parent = this;
        }        
    }
}
