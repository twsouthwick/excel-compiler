using Xunit;
using Xunit.Abstractions;

using static ExcelCompiler.Syntax;

namespace ExcelCompiler.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Float()
        {
            var expected = Formula.NewExpression(
                Expression.NewLiteralExpression(
                    Literal.NewFloat(1.2)));

            var result = Parse("=1.2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Int()
        {
            var expected = Formula.NewExpression(
                Expression.NewLiteralExpression(
                    Literal.NewInt(1)));
            var result = Parse("=1");

            Assert.Equal(expected, result);
        }

        [Fact(Skip = "Failing")]
        public void Empty()
        {
            var expected = Formula.NewExpression(null);
            var result = Parse("1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Plus()
        {
            var expected = Formula.NewExpression(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Add,
                    Expression.NewLiteralExpression(Literal.NewInt(2))));

            var result = Parse("=1+2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Minus()
        {
            var expected = Formula.NewExpression(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Subtract,
                    Expression.NewLiteralExpression(Literal.NewInt(2))));

            var result = Parse("=1-2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Mult()
        {
            var expected = Formula.NewExpression(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Multiply,
                    Expression.NewLiteralExpression(Literal.NewInt(2))));
            var result = Parse("=1*2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MultNegative()
        {
            var expected = Formula.NewExpression(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(-1)),
                    Operation.Multiply,
                    Expression.NewLiteralExpression(Literal.NewInt(-2))));
            var result = Parse("=-1*-2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormulaNoArg()
        {
            var expected = Formula.NewExpression(
                Expression.NewFunctionExpression("F", SyntaxList<Expression>.Empty));
            var result = Parse("=F()");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormula1Arg()
        {
            var expected = Formula.NewExpression(
                Expression.NewFunctionExpression(
                    "F", SyntaxList<Expression>.NewSingle(
                        CreateExpression(1))));
            var result = Parse("=F(1)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormula2Args()
        {
            var expected = Formula.NewExpression(
                Expression.NewFunctionExpression(
                    "F", SyntaxList<Expression>.NewList(new[]
                    {
                        CreateExpression(1),
                        CreateExpression(2),
                    })));
            var result = Parse("=F(1, 2)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NestedFunctions()
        {
            Parse("=1 + F2()");
        }

        [Fact]
        public void NestedParenthesis()
        {
            Parse("=1 + (2+3)");
        }

        [Fact]
        public void MinusNestedParenthesis()
        {
            Parse("=1 - (2+3)");
        }

        private Formula Parse(string input)
        {
            foreach (var token in ParseUtils.Tokenize(input))
            {
                _output.WriteLine(token);
            }

            var result = ParseUtils.Parse(input);

            _output.WriteLine(ParseUtils.Flatten(result));

            return result;
        }

        private Expression CreateExpression(int v)
            => Expression.NewLiteralExpression(Literal.NewInt(v));
    }
}
