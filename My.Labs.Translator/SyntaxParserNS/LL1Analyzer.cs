using My.Labs.Translator.GrammarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.SyntaxParserNS
{
    public class LL1Analyzer
    {

        private int i;
        private Grammar g;
        private Stack<ComplexToken> stack = new Stack<ComplexToken>();

        public static readonly ComplexToken EOFToken = new ComplexToken("$", "stack-bottom");

        public LL1Analyzer()
        {
            //
        }

        public SyntaxResult Analyze(List<ComplexToken> elements, Grammar g)
        {
            this.g = g;
            i = 0;
            elements.Add(EOFToken);
            stack.Clear();
            stack.Push(EOFToken);
            var axiom = g.NonTerminals.First();
            stack.Push(axiom);
            SyntaxTreeNode root = new SyntaxTreeNode(axiom);            
            Analyze(root, elements);
            var synRes = new SyntaxResult();
            synRes.SyntaxTree = root;
            return synRes;
        }

        private void Analyze(SyntaxTreeNode root, List<ComplexToken> elements)
        {
            while (true)
            {
                var el = elements[i];
                var top = stack.Peek();
                if (g.IsTerminal(top))
                {                    
                    if (el.Lexem.Equals(top.Lexem))
                    {
                        stack.Pop();
                        i++;
                        return;
                    }
                    else if (top.Lexem.Equals(el.Token))
                    {                                                              
                        root.Parent.ReplaceChild(root, new SyntaxTreeNode(el));
                        stack.Pop();
                        i++;
                        return;
                    }
                    else
                    {
                        throw new Exception(string.Format("Token {0} expected at line {1} position {2}",
                            top.ToString(), el.CodeLine, el.CodePosition));
                    }
                }
                else
                {
                    foreach (var rule in g.Rules)
                    {
                        bool firstContains = false;
                        var firstLeft = rule.Tokens.First();
                        var FIRST = g.FIRST(firstLeft);
                        foreach (var token in FIRST)
                        {
                            if (token.Lexem.Equals(el.Lexem) || token.Lexem.Equals(el.Token))
                            {
                                firstContains = true;
                                break;
                            }
                        }                        
                        if (rule.MainToken.Equals(top) && firstContains)
                        {
                            stack.Pop();
                            for (int k = rule.Tokens.Count - 1; k >= 0; k--)
                            {
                                var token = rule.Tokens[k];
                                stack.Push(token);
                            }
                            var ruleIndex = g.Rules.IndexOf(rule) + 1;
                            root.SetRuleIndex(ruleIndex);
                            root.SetChildren(rule.Tokens);
                            for (int j = 0; j < root.Children.Count; j++)
                            {
                                var node = root.Children[j];
                                Analyze(node, elements);
                            }                          
                            return;
                        }
                    }
                    foreach(var token in g.FIRST(top))
                        if (token.Lexem.Equals(ComplexToken.Empty.Lexem))
                        {                            
                            foreach (var rule in g.Rules)
                                if (rule.MainToken.Equals(top) 
                                    && rule.Tokens.First().Equals(ComplexToken.Empty))
                                {
                                     var ruleIndex = g.Rules.IndexOf(rule) + 1;
                                     root.SetRuleIndex(ruleIndex);
                                     root.AddChildren(new List<SyntaxTreeNode>() { new SyntaxTreeNode(ComplexToken.Empty) });
                                    stack.Pop();
                                    return;
                                }
                            throw new Exception("Ned to build a chain to empty rule");
                        }
                    var firstStr = "";
                    foreach (var token in g.FIRST(top))
                        firstStr += token + ", ";
                    throw new Exception(string.Format("Expected one of [{0}] at line {1} position {2}",
                        firstStr, el.CodeLine, el.CodePosition));
                }
            }
        }
    }
    // @todo issue when non-terminal is lexem 'terminal'
    /*
    public class LL1Analyzer
    {

        private int i;
        private Grammar g;
        private Stack<ComplexToken> stack = new Stack<ComplexToken>();

        public static readonly ComplexToken EOFToken = new ComplexToken("$", "stack-bottom");

        public LL1Analyzer(Grammar grammar)
        {
            this.g = grammar;
        }

        public SyntaxTreeNode Analyze(List<ComplexToken> elements)
        {
            i = 0;
            elements.Add(EOFToken);
            stack.Clear();
            stack.Push(EOFToken);
            var axiom = g.NonTerminals.First();
            stack.Push(axiom);
            SyntaxTreeNode root = new SyntaxTreeNode(axiom);
            Analyze(root, elements);
            return root;
        }

        private void Analyze(SyntaxTreeNode root, List<ComplexToken> elements)
        {
            while (true)
            {
                var el = elements[i];
                var top = stack.Peek();
                if (top.IsTerminal)
                {
                    if (el.Equals(top))
                    {
                        stack.Pop();
                        i++;
                        return;
                    }
                    else
                    {
                        throw new Exception(string.Format("Token {0} expected at line {1} position {2}",
                            top.ToString(), el.CodeLine, el.CodePosition));
                    }
                }
                else
                {
                    var A = top;
                    foreach (var rule in g.Rules)
                    {                        
                        bool firstContains = g.FIRST(rule.Tokens.First()).Contains(el);                        
                        if (rule.MainToken.Equals(A) && firstContains)
                        {
                            stack.Pop();
                            for (int k = rule.Tokens.Count - 1; k >= 0; k--)
                            {
                                var token = rule.Tokens[k];
                                stack.Push(token);
                            }
                            root.SetChildren(rule.Tokens);
                            foreach (var node in root.Children)
                                Analyze(node, elements);
                            return;
                        }
                    }
                    if (g.FIRST(A).Contains(ComplexToken.Empty))
                    {
                        stack.Pop();
                        return;
                    }                          
                    var firstStr = "";
                    foreach (var token in g.FIRST(A))
                        firstStr += token + ", ";
                    throw new Exception(string.Format("Expected one of [{0}] at line {1} position {2}", 
                        firstStr, el.CodeLine, el.CodePosition));
                }
            }
        }      
    }*/
}
