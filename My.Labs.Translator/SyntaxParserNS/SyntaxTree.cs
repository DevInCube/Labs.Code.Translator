using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.SyntaxParserNS
{
    public class SyntaxTreeRule
    {
        public int Index { get; set; }
        public SyntaxTreeNode Node { get; set; }

        public SyntaxTreeRule(SyntaxTreeNode node)
        {
            this.Index = node.RuleIndex;
            this.Node = node;
        }

        public SyntaxTreeRule(int reff)
        {
            this.Index = reff;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }

    public class SyntaxTree
    {

        private Grammar g;
        private List<SyntaxTreeRule> RAS;

        public List<SyntaxTreeRule> BuildLinearTreeForm(SyntaxTreeNode root, Grammar g)
        {
            this.g = g;
            this.RAS = new List<SyntaxTreeRule>();
            var axiom = root;
            Process(root);
            CollectLeftChain(axiom);
            var index = GetValuePointer(axiom);
            var str = new SyntaxTreeRule(index);
            RAS.Add(str);
            return RAS;
        }       
        void Process(SyntaxTreeNode node)
        {
            if (node.HasChildren())
            {
                foreach (var child in node.Children)
                {
                    Process(child);
                    if (IsRightNonTerminal(child))
                    {
                        CollectLeftChain(child);              
                    }                    
                }                
            }
        }

        void CollectLeftChain(SyntaxTreeNode node)
        {
            var str = new SyntaxTreeRule(node);
            RAS.Add(str);
            for (var i = node.Children.Count - 1; i >= 0; i--)
            {
                var child = node.Children[i];
                if (IsRightNonTerminal(child))
                {
                    int refIndex = GetValuePointer(child);
                    str = new SyntaxTreeRule(refIndex);
                    RAS.Add(str);
                }
            } 
            var leftNode = GetLeftNonTerm(node);
            if (leftNode != null)
                CollectLeftChain(leftNode);
        }

        int GetValuePointer(SyntaxTreeNode node)
        {
            for (int index = 0; index < RAS.Count; index++)
            {
                if (RAS[index].Node == node)
                    return -(index);
            }
            throw new Exception("NO pointer in RAS array");
        }

        SyntaxTreeNode GetLeftNonTerm(SyntaxTreeNode node)
        {
            foreach (var ch in node.Children)
                if (IsLeftNonTerminal(ch))
                    return ch;
            return null;
        }

        bool IsLeftNonTerminal(SyntaxTreeNode node)
        {
            if(!IsNonTerminal(node)) return false;
            if (IsAxiom(node)) return false;//?
            foreach (var child in node.Parent.Children)
            {
                if (IsNonTerminal(child))
                    return node.Equals(child);
            }
            return false;
        }

        bool IsRightNonTerminal(SyntaxTreeNode node)
        {
            if(!IsNonTerminal(node)) return false;
            return !IsLeftNonTerminal(node);
        }

        bool IsAxiom(SyntaxTreeNode node)
        {
            return node.Parent == null;
        }

        bool IsNonTerminal(SyntaxTreeNode node)
        {
            bool isNonTerminal = node.HasChildren();
            return isNonTerminal;
        }
    }
}
