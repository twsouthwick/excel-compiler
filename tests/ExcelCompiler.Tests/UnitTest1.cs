using Xunit;

using static Syntax.Formula;
using static Syntax.Expression;

namespace ExcelCompiler.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Function()
        {
            var expected = NewExpression(NewFloat(1.2));
            var result = ParseUtils.Parse("=1.2");

            Assert.Equal(expected, result);
        }
    }
}
