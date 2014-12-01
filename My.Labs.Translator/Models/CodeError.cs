using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Labs.Translator.Models
{
    public class CodeError : Exception
    {
        public CodeErrorType Type { get; set; }
        public string Description { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public CodeError(CodeErrorType type, string desc, int line, int col)
            : base(desc)
        {
            this.Type = type;
            this.Description = desc;
            this.Line = line;
            this.Column = col;
        }
    }

    public enum CodeErrorType
    {
        Lexical,
        Syntax,
        Semantic
    }
}
