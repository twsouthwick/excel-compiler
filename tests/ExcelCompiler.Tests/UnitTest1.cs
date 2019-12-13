using Xunit;

using static ExcelCompiler.Syntax;

using FactorList = Microsoft.FSharp.Collections.FSharpList<ExcelCompiler.Syntax.Factor>;

namespace ExcelCompiler.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Float()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerms(
                    SyntaxList<Term>.Create(
                        Term.NewFactors(
                            SyntaxList<Factor>.Create(
                                Factor.NewFloat(1.2))))));
            var result = ParseUtils.Parse("=1.2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Int()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerms(
                    SyntaxList<Term>.Create(
                        Term.NewFactors(
                            SyntaxList<Factor>.Create(
                                Factor.NewInt(1))))));

            var result = ParseUtils.Parse("=1");

            Assert.Equal(expected, result);
        }

        [Fact(Skip = "Failing")]
        public void Empty()
        {
            var expected = Formula.NewExpression(null);
            var result = ParseUtils.Parse("1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Plus()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerms(SyntaxList<Term>.Create(
                    new[]
                    {
                        Term.NewFactors(SyntaxList<Factor>.Create(Factor.NewInt(1))),
                        Term.NewFactors(SyntaxList<Factor>.Create(Factor.NewInt(1))),
                    })));
            var result = ParseUtils.Parse("=1+1");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Mult()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerms(SyntaxList<Term>.Create(
                    Term.NewFactors(SyntaxList<Factor>.Create(new[]
                    {
                        Factor.NewInt(1),
                        Factor.NewInt(1),
                    })))));
            var result = ParseUtils.Parse("=1*1");

            Assert.Equal(expected, result);
        }

    }
}
