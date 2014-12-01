using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.Models;
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

        public SyntaxResult Analyze(List<ComplexToken> tokens, Grammar g)
        {
            this.g = g;
            i = 0;
            List<ComplexToken> elements = new List<ComplexToken>(tokens);
            elements.Add(EOFToken);
            stack.Clear();
            stack.Push(EOFToken);
            var axiom = g.Rules.First().MainToken;
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
                    if (el.Lexem.Equals(top.Lexem) 
                        && el.Token.Equals(top.Token))
                    {
                        MoveNextElement();
                        return;
                    }
                    else if (top.Lexem.Equals(el.Token))
                    {                                                              
                        root.Parent.ReplaceChild(root, new SyntaxTreeNode(el));
                        MoveNextElement();
                        return;
                    }
                    else
                    {
                        var msg = string.Format("Token {0} expected ", top.ToString());
                        throw new CodeError(CodeErrorType.Syntax, msg, el.CodeLine, el.CodePosition);                      
                    }
                }
                else
                {
                    foreach (var rule in g.Rules)
                    {
                        var firstLeft = rule.Tokens.First();
                        bool firstContains = TokenFIRSTContains(firstLeft, el); 
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
                            break;                                
                        }
                    var firstStr = "";
                    foreach (var token in g.FIRST(top))
                        firstStr += token + ", ";
                    var msg2 = string.Format("Expected one of [{0}]", firstStr);
                    throw new CodeError(CodeErrorType.Syntax, msg2, el.CodeLine, el.CodePosition);                         
                }
            }
        }

        void MoveNextElement()
        {
            stack.Pop();
            i++;
        }

        bool TokenFIRSTContains(ComplexToken firstLeft, ComplexToken el)
        {        
            var FIRST = g.FIRST(firstLeft);
            foreach (var token in FIRST)
            {
                if ((token.Lexem.Equals(el.Lexem) && token.Token.Equals(el.Token))
                    || token.Lexem.Equals(el.Token))
                    return true;
            }
            return false;
        }
    }
    // @todo issue when non-terminal is lexem 'terminal'    
}
