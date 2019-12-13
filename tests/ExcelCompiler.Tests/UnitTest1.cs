using Xunit;

using static ExcelCompiler.Syntax.Formula;
using static ExcelCompiler.Syntax.Expression;

namespace ExcelCompiler.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Float()
        {
            var expected = NewExpression(NewFloat(1.2));
            var result = ParseUtils.Parse("=1.2");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Int()
        {
            var expected = NewExpression(NewInt(1));
            var result = ParseUtils.Parse("=1");

            Assert.Equal(expected, result);
        }

        [Fact(Skip = "Failing")]
        public void Empty()
        {
            var expected = NewExpression(null);
            var result = ParseUtils.Parse("1");

            Assert.Equal(expected, result);
        }
    }
}
