using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Parser
{
    public interface IParser
    {
        ASTNode Parse(IEnumerable<char> lines);
    }
}
