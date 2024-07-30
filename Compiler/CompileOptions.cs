using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    internal class CompileOptions
    {
        public bool Verbose { get; set; } = false;

        public string InputFile { get; set; } = string.Empty;

        public string OutputFile { get; set; } = string.Empty;
    }
}
