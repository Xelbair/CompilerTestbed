namespace Compiler.Parser.Lisp.Tests
{
    public class OperandTests
    {
        [Fact]
        public void ConstantValue()
        {
            const string input = "(1)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.ConstantValue, output.NodeType);
            Assert.Equal("1", output.Value);
        }

        [Fact]
        public void StandaloneVariable()
        {
            const string input = "(a)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.Variable, output.NodeType);
            Assert.Equal("a", output.Value);
        }

        [Fact]
        public void BinaryAddition()
        {
            const string input = "(+ 1 2)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.Expression, output.NodeType);
            Assert.Equal("+", output.Value);

            Assert.Equal(NodeType.ConstantValue, output.Operands[0].NodeType);
            Assert.Equal(NodeType.ConstantValue, output.Operands[1].NodeType);


            Assert.Equal("1", output.Operands[0].Value);
            Assert.Equal("2", output.Operands[1].Value);
        }

        [Fact]
        public void UnaryNegation()
        {
            const string input = "(- 1)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.Expression, output.NodeType);
            Assert.Equal("-", output.Value);
            Assert.Equal(NodeType.ConstantValue, output.Operands[0].NodeType);
            Assert.Equal("1", output.Operands[0].Value);
        }

        [Fact]
        public void Branch()
        {
            const string input = "(if True 2 3)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.Expression, output.NodeType);
            Assert.Equal("if", output.Value);
            Assert.Equal("True", output.Operands[0].Value);
            Assert.Equal("2", output.Operands[1].Value);
            Assert.Equal("3", output.Operands[2].Value);
        }

        [Fact]
        public void NestedExpression()
        {
            const string input = "(if (= a b) 2 3)";
            IParser parser = new LispParser();

            var output = parser.Parse(input.ToArray());
            Assert.NotNull(output);
            Assert.Equal(NodeType.Expression, output.NodeType);
            Assert.Equal("if", output.Value);
            Assert.Equal("2", output.Operands[1].Value);
            Assert.Equal("3", output.Operands[2].Value);

            var nested = output.Operands[0];
            Assert.Equal(NodeType.Expression, nested.NodeType);
            Assert.Equal("=", nested.Value);

            Assert.Equal("a", nested.Operands[0].Value);
            Assert.Equal("b", nested.Operands[1].Value); 
            Assert.Equal(NodeType.Variable, nested.Operands[0].NodeType);
            Assert.Equal(NodeType.Variable, nested.Operands[1].NodeType);

        }
    }
}