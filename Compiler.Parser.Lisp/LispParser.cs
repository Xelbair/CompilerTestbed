using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Parser.Lisp
{
    public class LispParser : IParser
    {
        //Todo: make it a stream input
        public ASTNode Parse(IEnumerable<char> code)
        {
            ASTNode currentNode = null;
            int parenthesisCounter = 0;
            bool quoteClosed = true;
            //TODO: replace with buffer
            List<char> currentToken = new List<char>();

            foreach (var c in code)
            {
                if (!quoteClosed)
                {
                    if (c == '"') quoteClosed = !quoteClosed;
                    else currentToken.Add(c);
                    continue;
                }

                switch (c)
                {
                    case '(':
                        currentNode = currentNode?.BuildChildOperand() ?? new ASTNode();
                        parenthesisCounter++;
                        break;
                    case '"':
                        quoteClosed = !quoteClosed;
                        currentToken.Add(c);
                        break;
                    case ')':
                        parenthesisCounter--;
                        if (currentToken.Any())
                        {
                            HandleToken(currentNode, currentToken);
                            currentToken.Clear();
                        }

                        currentNode.NodeType = (IsConstantValue(currentNode.Value), currentNode.Operands.Any()) switch
                        {
                            (_, true) => NodeType.Expression,
                            (false, false) => NodeType.Variable,
                            (true, false) => NodeType.ConstantValue,
                        };

                        currentNode = currentNode.Parent ?? currentNode;
                        break;
                    case ' ':
                    case '\n':
                        if (currentToken.Any())
                        {
                            HandleToken(currentNode, currentToken);
                            currentToken.Clear();
                        }
                        break;
                    default:
                        currentToken.Add(c);
                        break;
                }
            }

            if (parenthesisCounter != 0)
            {
                throw new Exception("Unclosed Parenthesis!");
            }

            if (!quoteClosed)
            {
                throw new Exception("Unclosed Quotation mark!");
            }

            return currentNode.GetTreeRoot();
        }

        private static bool IsConstantValue(string value)
        {
            return value == "True" || value == "False" || double.TryParse(value, out _) || (value.StartsWith('"') && value.EndsWith('"'));
        }

        private static void HandleToken(ASTNode currentNode, IEnumerable<char> currentToken)
        {
            var value = new string(currentToken.ToArray());
            if (string.IsNullOrEmpty(currentNode.Value))
            {
                currentNode.Value = value;
            }
            else
            {
                var operand = currentNode.BuildChildOperand();
                operand.Value = value;
                operand.NodeType = IsConstantValue(value) ? NodeType.ConstantValue : NodeType.Variable;
            }
        }
    }

}
