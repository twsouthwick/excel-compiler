using Xunit;

using static ExcelCompiler.Syntax;

namespace ExcelCompiler.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Float()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerm(
                    Term.NewFactor(Factor.NewFloat(1.2))));
            var result = ParseUtils.Parse("=1.2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Int()
        {
            var expected = Formula.NewExpression(
                Expression.NewTerm(
                    Term.NewFactor(Factor.NewInt(1))));
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
                Expression.NewTerms(
                    Term.NewFactor(Factor.NewInt(1)),
                    Op.Add,
                    Term.NewFactor(Factor.NewInt(1))));
            var result = ParseUtils.Parse("=1+1");

            Assert.Equal(expected, result);
        }
    }
}
