using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

            Assert.Equal(0, evaluator.GetCell<int>("A1"));
        }

        [Fact]
        public void SingleLiteral()
        {
            var doc = new TestCompiledDocument
            {
                { "A1", Statement.NewLiteral(Literal.NewNumber(Number.NewInt(2))) }
            };

            var evaluator = new SyntaxEvaluator(doc, Substitute.For<IFunctionProvider>());

            Assert.Equal(2, evaluator.GetCell<int>("A1"));
        }

        [Fact]
        public void SingleBinaryAddition()
        {
            var doc = new TestCompiledDocument
            {
                { "A1", Statement.NewFormula(
                    Expression.NewBinaryExpression(
                        Expression.NewLiteralExpression(
                            Literal.NewNumber(Number.NewInt(2))),
                        Operation.Add,
                        Expression.NewLiteralExpression(
                            Literal.NewNumber(Number.NewInt(3)))))}
            };

            var evaluator = new SyntaxEvaluator(doc, Substitute.For<IFunctionProvider>());

            Assert.Equal(5, evaluator.GetCell<int>("A1"));
        }

        private class TestCompiledDocument : ICompiledDocument, IEnumerable<(CellReference, Syntax.Statement)>
        {
            private readonly Dictionary<CellReference, Syntax.Statement> _lookup = new Dictionary<CellReference, Statement>();

            public void Add(CellReference reference, Syntax.Statement statement)
            {
                _lookup.Add(reference, statement);
            }

            public Syntax.Statement GetCell(CellReference cell)
            {
                if (_lookup.TryGetValue(cell, out var result))
                {
                    return result;
                }

                return Syntax.Statement.Nothing;
            }

            IEnumerator<(CellReference, Syntax.Statement)> IEnumerable<(CellReference, Syntax.Statement)>.GetEnumerator()
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
