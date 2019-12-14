using Microsoft.FSharp.Core;
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
            var expected = Statement.NewFormula(
                Expression.NewLiteralExpression(
                    Literal.NewFloat(1.2)));

            var result = Parse("=1.2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Int()
        {
            var expected = Statement.NewFormula(
                Expression.NewLiteralExpression(
                    Literal.NewInt(1)));
            var result = Parse("=1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void LiteralStatement()
        {
            var expected = Statement.NewLiteral(Literal.NewInt(1));
            var result = Parse("1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TextStatement()
        {
            var expected = Statement.NewText("hello");
            var result = Parse("hello");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Plus()
        {
            var expected = Statement.NewFormula(
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
            var expected = Statement.NewFormula(
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
            var expected = Statement.NewFormula(
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
            var expected = Statement.NewFormula(
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
            var expected = Statement.NewFormula(
                Expression.NewFunctionExpression("F1", SyntaxList<Expression>.Empty));
            var result = Parse("=F1()");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormulaMixedNameNoArg()
        {
            var expected = Statement.NewFormula(
                Expression.NewFunctionExpression("F1F1F1F1F1", SyntaxList<Expression>.Empty));
            var result = Parse("=F1F1F1F1F1()");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormula1Arg()
        {
            var expected = Statement.NewFormula(
                Expression.NewFunctionExpression(
                    "F", SyntaxList<Expression>.NewSingle(
                        CreateExpression(1))));
            var result = Parse("=F(1)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleFormula2Args()
        {
            var expected = Statement.NewFormula(
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
            var expected = Statement.NewFormula(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Add,
                    Expression.NewFunctionExpression("F2", SyntaxList<Expression>.Empty)));

            var result = Parse("=1 + F2()");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NestedParenthesis()
        {
            var expected = Statement.NewFormula(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Add,
                    Expression.NewBinaryExpression(
                        Expression.NewLiteralExpression(Literal.NewInt(2)),
                        Operation.Add,
                        Expression.NewLiteralExpression(Literal.NewInt(3)))));

            var result = Parse("=1 + (2+3)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MinusNestedParenthesis()
        {
            var expected = Statement.NewFormula(
                Expression.NewBinaryExpression(
                    Expression.NewLiteralExpression(Literal.NewInt(1)),
                    Operation.Subtract,
                    Expression.NewBinaryExpression(
                        Expression.NewLiteralExpression(Literal.NewInt(2)),
                        Operation.Add,
                        Expression.NewLiteralExpression(Literal.NewInt(3)))));

            var result = Parse("=1 - (2+3)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleCellReference()
        {
            var expected = Statement.NewFormula(
                Expression.NewCellReferenceExpression(
                    CellReference.NewRelative("A", 1)));
            var result = Parse("=A1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CellReferenceInArgument()
        {
            var expected = Statement.NewFormula(
                Expression.NewBinaryExpression(
                    Expression.NewCellReferenceExpression(
                        CellReference.NewRelative("A", 1)),
                    Operation.Multiply,
                    Expression.NewFunctionExpression("A2",
                        SyntaxList<Expression>.NewSingle(
                            Expression.NewCellReferenceExpression(
                                CellReference.NewRelative("A", 3))))));
            var result = Parse("=A1*A2(A3)");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AbsoluteColumnCellReference()
        {
            var expected = Statement.NewFormula(
                Expression.NewCellReferenceExpression(
                    CellReference.NewAbsoluteColumn("A", 1)));
            var result = Parse("=$A1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AbsoluteRowCellReference()
        {
            var expected = Statement.NewFormula(
                Expression.NewCellReferenceExpression(
                    CellReference.NewAbsoluteRow("A", 1)));
            var result = Parse("=A$1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AbsoluteCellReference()
        {
            var expected = Statement.NewFormula(
                Expression.NewCellReferenceExpression(
                    CellReference.NewAbsolute("A", 1)));
            var result = Parse("=$A$1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void RelativeCellReference()
        {
            var expected = Statement.NewFormula(
                Expression.NewCellReferenceExpression(
                    CellReference.NewRelative("A", 1)));
            var result = Parse("=A1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CellReferenceInArgumentWithAnchor()
        {
            var expected = Statement.NewFormula(
                Expression.NewBinaryExpression(
                    Expression.NewCellReferenceExpression(
                        CellReference.NewRelative("A", 1)),
                    Operation.Multiply,
                    Expression.NewFunctionExpression("A2",
                        SyntaxList<Expression>.NewSingle(
                            Expression.NewCellReferenceExpression(
                                CellReference.NewRelative("A", 3))))));
            var result = Parse("=A1*A2(A3)");

            Assert.Equal(expected, result);
        }


        private Statement Parse(string input)
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
