using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.CodeGeneratorNS
{
    public abstract class ACodeGenerator
    {

        private List<SyntaxTreeRule> RAS;

        public ACodeGenerator()
        {
            //
        }

        public void Generate(List<SyntaxTreeRule> RAS)
        {
            OnInit();
            this.RAS = RAS;
            int LRAS = RAS.Count - 1;
            RunRule(LRAS);
            OnExit();
        }

        protected abstract void OnInit();
        protected abstract void OnRule(int index, int iterator, SyntaxTreeNode currNode);
        protected abstract void OnExit();
        public abstract string GetPlainResult();

        public void RunRule(int iterator)
        {
            int k = iterator;
            if (RAS[iterator].Index <= 0)
                k = -RAS[iterator].Index;
            int ruleIndex = RAS[k].Index;
            var CurrentNode = RAS[k].Node;
            OnRule(ruleIndex, k, CurrentNode);            
        }        
    }
}
