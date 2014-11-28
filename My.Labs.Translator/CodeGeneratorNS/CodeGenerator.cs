using My.Labs.Translator.GrammarNS;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.CodeGeneratorNS
{
    public class CodeGenerator : ACodeGenerator
    {

        public CodeGenerator()
        {
            //
        }

        public GenerationResult Generate(SyntaxResult synRes)
        {
            throw new NotImplementedException();
            /*
            var root = synRes.SyntaxTree;
            List<int> linearTreeFormList = (new SyntaxTree()).BuildLinearTreeForm(root);
            GenerationResult result = new GenerationResult();
            result.Program = "TestOK!";
            return result;*/
        }

        void SPR(int u, List<int> RAS)
        {            
            int k = u;
            if (RAS[u] < 0) k = -RAS[u];
            int i = RAS[k];
            //@todo GetRule Number i and apply semantic transform
        }        

        static void TestLinearTreeFormBuid()
        {
            var root = new_TestNode(0,
                    new_TestNode(1,
                        new_TestNode(8),
                        new_TestNode(2,
                            new_TestNode(3, new_TestNode(6, new_TestNode(9))),
                            new_TestNode(4),
                            new_TestNode(6, new_TestNode(10))
                        )
                    )
                );
            //@todo @test  Result = [4, 6, 10, 2, -2, -1, 3, 6, 9, 1, -4, 8, -10]; <- (Count = 13)
        }

        static SyntaxTreeNode new_TestNode(int ruleIndex, params SyntaxTreeNode[] childs)
        {
            var test = new SyntaxTreeNode(new ComplexToken("", ""));
            test.SetRuleIndex(ruleIndex);
            test.AddChildren(childs.ToList());
            return test;
        }

        protected override void OnInit()
        {
            //throw new NotImplementedException();
        }

        protected override void OnRule(int index, int iterator, SyntaxTreeNode currNode)
        {
            //throw new NotImplementedException();
        }

        protected override void OnExit()
        {
            //throw new NotImplementedException();
        }

        public override string GetPlainResult()
        {
            return this.ToString();
        }
    }
}
