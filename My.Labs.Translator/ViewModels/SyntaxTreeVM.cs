using My.Labs.Translator.Commands;
using My.Labs.Translator.SyntaxParserNS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace My.Labs.Translator.ViewModels
{
    public class SyntaxTreeVM : ObservableObject
    {

        public ObservableCollection<TreeItemVM> TreeItems { get; private set; }

        public ICommand ExpandAll { get; set; }

        public SyntaxTreeVM()
        {
            TreeItems = new ObservableCollection<TreeItemVM>();
            ExpandAll = new SimpleCommand(ExpandAllAction);

        }

        public void Init(SyntaxTreeNode root)
        {
            this.TreeItems.Clear();
            TreeItemVM vm = new TreeItemVM(root);
            this.TreeItems.Add(vm);
        }

        void ExpandAllAction()
        {
            foreach (var node in TreeItems)
            {
                node.ExpandAll();
            }
        }
    }
}
