using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Parser
{
    public enum NodeType
    {
        Unknown,
        Expression,
        Variable,
        ConstantValue,
    }

    public class ASTNode
    {
        private string _value;
        private List<ASTNode> _operands = new List<ASTNode>();
        public NodeType NodeType { get; set; }
        public string Value { get => _value; set => _value = value; }

        public ASTNode Parent { get; set; }
        public int OperandCounter => _operands.Count;

        public List<ASTNode> Operands => _operands;

        public void AddOperand(ASTNode operand)
        {
            _operands.Add(operand);
        }

        public ASTNode BuildChildOperand()
        {
            var newOperand = new ASTNode()
            {
                Parent = this,
            };
            _operands.Add(newOperand);
            return newOperand;
        }

        public ASTNode GetTreeRoot()
        {
            return Parent is null ? this : Parent.GetTreeRoot();
        }
    }
}
