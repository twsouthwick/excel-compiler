using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using Xunit;

using static ExcelCompiler.Syntax;

namespace ExcelCompiler.Tests
{
    public class EvaluatorTests
    {
        [Fact]
        public void EmptyLiteral()
        {
            var doc = new TestCompiledDocument();
            var evaluator = new SyntaxEvaluator(doc, Substitute.For<IFunctionProvider>());

            Assert.Equal(0, evaluator.GetCell<int>(new Cell("A1")));
        }

        [Fact]
        public void SingleLiteral()
        {
            var doc = new TestCompiledDocument
            {
                { new Cell("A1"), Statement.NewLiteral(Literal.NewNumber(Number.NewInt(2))) }
            };

            var evaluator = new SyntaxEvaluator(doc, Substitute.For<IFunctionProvider>());

            Assert.Equal(2, evaluator.GetCell<int>(new Cell("A1")));
        }

        [Fact]
        public void SingleBinaryAddition()
        {
            var doc = new TestCompiledDocument
            {
                { new Cell("A1"), Statement.NewFormula(
                    Expression.NewBinaryExpression(
                        Expression.NewLiteralExpression(
                            Literal.NewNumber(Number.NewInt(2))),
                        Operation.Add,
                        Expression.NewLiteralExpression(
                            Literal.NewNumber(Number.NewInt(3)))))}
            };

            var evaluator = new SyntaxEvaluator(doc, Substitute.For<IFunctionProvider>());

            Assert.Equal(5, evaluator.GetCell<int>(new Cell("A1")));
        }

        private class TestCompiledDocument : ICompiledDocument, IEnumerable<(Cell, Syntax.Statement)>
        {
            private readonly Dictionary<Cell, Syntax.Statement> _lookup = new Dictionary<Cell, Statement>();

            public void Add(Cell reference, Syntax.Statement statement)
            {
                _lookup.Add(reference, statement);
            }

            public Syntax.Statement GetCell(Cell cell)
            {
                if (_lookup.TryGetValue(cell, out var result))
                {
                    return result;
                }

                return Syntax.Statement.Nothing;
            }

            IEnumerator<(Cell, Syntax.Statement)> IEnumerable<(Cell, Syntax.Statement)>.GetEnumerator()
            {
                foreach (var item in _lookup)
                {
                    yield return (item.Key, item.Value);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => _lookup.GetEnumerator();
        }
    }
}
